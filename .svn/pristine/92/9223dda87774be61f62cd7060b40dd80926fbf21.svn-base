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

using System.Windows.Forms;

namespace MessageParser
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Application.StartupPath);

            CourseDBContext context = new CourseDBContext();
            
            ObjectMessageQueue.InitializeQueue(ObjectMessageQueue.DB_REQUEST);
            ObjectMessageQueue queue = new ObjectMessageQueue();

            while (true)
            {
                try
                {
                    Object obj = queue.receiveObject(ObjectMessageQueue.DB_REQUEST);
                    GenericRequest gen_req = obj as GenericRequest;
                    
                    if (gen_req == null)
                    {
                        Console.WriteLine("Error processing request, it is not a request object!");
                        queue.sendObject(null, ObjectMessageQueue.DB_RESPONSE);
                    }
                    else
                    {
                        Console.WriteLine("Fetching data...");
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
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    continue;
                }
            }
        }
    }
}
