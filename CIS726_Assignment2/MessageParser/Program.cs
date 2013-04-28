using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;
using MessageParser.Models;
using System.Data.SqlClient;
using MessageParser.Processor;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Concurrent;

namespace MessageParser
{
    class Program
    {
        static private ConcurrentQueue<CourseDBContext> contextQueue;
        static private Object context_lock;
        static private int context_count;
        static private ConcurrentDictionary<string, object> getAll_cache;
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Application.StartupPath);

            ObjectMessageQueue.InitializeQueue(ObjectMessageQueue.DB_REQUEST);
            ObjectMessageQueue queue = new ObjectMessageQueue();
            contextQueue = new ConcurrentQueue<CourseDBContext>();
            context_lock = new object();
            context_count = 0;
            getAll_cache = new ConcurrentDictionary<string, object>();

            ThreadPool.SetMaxThreads(25, 0);
            ThreadPool.SetMinThreads(5, 0);
            while (true)
            {
                try
                {
                    Object obj = queue.receiveObject(ObjectMessageQueue.DB_REQUEST);
                    GenericRequest gen_req = obj as GenericRequest;
                    gen_req.requester_guid = queue.RequestGuid;
                    gen_req.requester_ip = queue.RequestIP;
                    ThreadPool.QueueUserWorkItem(DoMessage, gen_req);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    continue;
                }
            }
        }

        private static void DoMessage(Object obj)
        {
            GenericRequest gen_req = obj as GenericRequest;
            ObjectMessageQueue queue = new ObjectMessageQueue();
            queue.RequestGuid = gen_req.requester_guid;
            queue.RequestIP = gen_req.requester_ip;
            if (gen_req == null)
            {
                Console.WriteLine("Error processing request, it is not a request object!");
                queue.sendObject(null, ObjectMessageQueue.DB_RESPONSE);
            }
            else
            {
                Console.WriteLine("Fetching data...");
                Object result = null;
                if (gen_req.Method == MethodType.GetAll && getAll_cache.TryGetValue(gen_req.Type.ToString(), out result))
                {
                    Console.WriteLine("It worked! Found Cached Result!");
                    queue.sendResponse(result, ObjectMessageQueue.DB_RESPONSE);
                }
                else
                {
                    if (gen_req.Method == MethodType.Add || gen_req.Method == MethodType.Delete || gen_req.Method == MethodType.Update)
                    {
                        object n = null;
                        getAll_cache.TryRemove(gen_req.Type.ToString(), out n);
                    }
                    CourseDBContext context = null;
                    if (contextQueue.TryDequeue(out context) == false)
                    {
                        //Check if we are under the max count of contexts allowed
                        bool createNew = false;
                        lock (context_lock)
                        {
                            if (context_count < 25)
                            {
                                context_count++;
                                createNew = true;
                            }
                        }
                        //If we are allowed, create a context for ourself and the pool.
                        if (createNew)
                        {
                            context = new CourseDBContext();
                        }
                        else //Else, wait for a context to be available
                        {
                            while (contextQueue.TryDequeue(out context) == false)
                            {
                                Thread.Sleep(5);
                            }
                        }
                    }

                    MessageProcessor parser = new MessageProcessor(context, gen_req);
                    try
                    {
                        //Object result = typeof(MessageProcessor).GetMethod(gen_req.Method.ToString()).Invoke(parser, null);
                        result = typeof(MessageProcessor).GetMethod(gen_req.Method.ToString()).Invoke(parser, null);
                        Console.WriteLine("It worked!");
                        if (gen_req.Method == MethodType.GetAll)
                            getAll_cache.TryAdd(gen_req.Type.ToString(), result);
                        queue.sendResponse(result, ObjectMessageQueue.DB_RESPONSE);
                    }
                    catch (SqlException)
                    {
                        Console.WriteLine("An error occurred.");
                    }
                    contextQueue.Enqueue(context);
                }
            }
        }
    }
}
