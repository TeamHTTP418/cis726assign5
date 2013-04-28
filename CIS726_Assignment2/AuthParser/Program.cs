﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;
using MessageParser.Models;
using MessageParser;
using System.Data.SqlClient;
using AuthParser.Models;
using System.Windows.Forms;

using System.Windows.Forms;
using System.Collections.Concurrent;
using System.Threading;

namespace AuthParser
{
    class Program
    {
        static private ConcurrentQueue<AccountDBContext> contextQueue;
        static private Object context_lock;
        static private int context_count;
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Application.StartupPath);

            AccountDBContext context = new AccountDBContext();

            ObjectMessageQueue queue = new ObjectMessageQueue();
            ObjectMessageQueue.InitializeQueue(ObjectMessageQueue.AUTH_REQUEST);

            contextQueue = new ConcurrentQueue<AccountDBContext>();
            context_lock = new object();
            context_count = 0;
            ThreadPool.SetMaxThreads(25, 0);
            ThreadPool.SetMinThreads(5, 0);

            while (true)
            {
                try
                {
                    Object obj = queue.receiveObject(ObjectMessageQueue.AUTH_REQUEST);
                    GenericRequest gen_req = obj as GenericRequest;
                    gen_req.requester_guid = queue.RequestGuid;
                    gen_req.requester_ip = queue.RequestIP;
                    ThreadPool.QueueUserWorkItem(DoMessage, obj);
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
                queue.sendObject(null, ObjectMessageQueue.AUTH_RESPONSE);
            }
            else
            {
                Console.WriteLine("Fetching data...");
                AccountDBContext context = null;
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
                        context = new AccountDBContext();
                    }
                    else //Else, wait for a context to be available
                    {
                        while (contextQueue.TryDequeue(out context) == false)
                        {
                            Thread.Sleep(5);
                        }
                    }
                }
                AuthProcessor parser = new AuthProcessor(context, gen_req);
                try
                {
                    Object result = typeof(AuthProcessor).GetMethod(gen_req.Method.ToString()).Invoke(parser, null);
                    Console.WriteLine("It worked!");
                    queue.sendResponse(result, ObjectMessageQueue.AUTH_RESPONSE);
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
