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
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Application.StartupPath);

            CourseDBContext context = new CourseDBContext();
            
            ObjectMessageQueue.InitializeQueue(ObjectMessageQueue.DB_REQUEST);
            ObjectMessageQueue queue = new ObjectMessageQueue();
            contextQueue = new ConcurrentQueue<CourseDBContext>();
            for (int i = 0; i < 25; i++)
                contextQueue.Enqueue(new CourseDBContext());
            ThreadPool.SetMaxThreads(25, 0);
            ThreadPool.SetMinThreads(5, 0);
            while (true)
            {
                try
                {
                    Object obj = queue.receiveObject(ObjectMessageQueue.DB_REQUEST);
                    GenericRequest gen_req = obj as GenericRequest;
                    gen_req.requester_guid = queue.RequestGuid;
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
            if (gen_req == null)
            {
                Console.WriteLine("Error processing request, it is not a request object!");
                queue.sendObject(null, ObjectMessageQueue.DB_RESPONSE);
            }
            else
            {
                Console.WriteLine("Fetching data...");
                CourseDBContext context = null;
                while (contextQueue.TryDequeue(out context) == false)
                {
                    Thread.Sleep(5);
                }
                MessageProcessor parser = new MessageProcessor(context, gen_req);
                try
                {
                    Object result = typeof(MessageProcessor).GetMethod(gen_req.Method.ToString()).Invoke(parser, null);
                    Console.WriteLine("It worked!");
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
