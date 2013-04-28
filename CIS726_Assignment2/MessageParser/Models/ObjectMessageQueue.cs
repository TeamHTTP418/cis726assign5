using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Runtime.Serialization;
using System.Web;


//Add References: System.Messaging and System.Runtime.Serialization to the Project References
//Add Datatag: [Serializable] or [DataContract] to IModel

namespace MessageParser.Models
{
    public class ObjectMessageQueue
    {
        //Static Locations
        public const string DB_REQUEST = @".\Private$\requests";
        public const string AUTH_REQUEST = @".\Private$\auth_requests";
        //Dynamic Locations
        public const string DB_RESPONSE = @".\Private$\responses";
        public const string AUTH_RESPONSE = @".\Private$\auth_responses";
        //This Machine IP
        public const string THIS_IP = ".";

        //Logic Layers send to DB/AUTH Request which are hard coded locations
        //DB/AUTH responds to IP given by the label
        //Labels are in the format GUID|IP|DeserializeTargetType
        public String RequestGuid;
        public String RequestIP;

        public static void InitializeQueue(string QueuePath)
        {
            if (!MessageQueue.Exists(QueuePath))
            {
                
                MessageQueue.Create(QueuePath);
                MessageQueue Queue = new MessageQueue(QueuePath);
                Queue.Label = QueuePath.Split('\\').LastOrDefault() ?? "Queue";
            }
        }

        //sendObject: Used by Logic Layers to pass messages to DB/AUTH
        public String sendObject(object obj, string DestinationQueue)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            // Get object type
            Type objType = obj.GetType();

            // Open existing queue
            using (MessageQueue queue = new MessageQueue(DestinationQueue))
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    // Serialize the object as XML into the stream
                    DataContractSerializer serializer = new DataContractSerializer(objType);
                    serializer.WriteObject(stream, obj);

                    // Rewind the stream
                    stream.Position = 0;

                    // Create and send new message
                    using (Message message = new Message())
                    {
                        message.BodyStream = stream;
                        Guid request = Guid.NewGuid();

                        // Pass object type as message label
                        message.Label = request.ToString() + "|" + THIS_IP + "|" + objType.FullName;
                        queue.Send(message);
                        return request.ToString();
                    }
                }
            }
        }

        //receiveObject, used by DB/AUTH layer to receive messages from logic layers
        public object receiveObject(string InputQueue)
        {

            // Open existing queue
            using (MessageQueue queue = new MessageQueue(InputQueue))
            {
                // Wait 10 seconds for a message,
                // after that MessageQueueException will be thrown
                using (Message message = queue.Receive())
                {
                    string[] splitLabel = message.Label.Split('|');
                    RequestGuid = splitLabel[0];
                    RequestIP = splitLabel[1];
                    // Gets object type from the message label
                    Type objType = Type.GetType(splitLabel[2], true, true);

                    // Derializes object from the stream
                    DataContractSerializer serializer = new DataContractSerializer(objType);
                    return serializer.ReadObject(message.BodyStream);
                }
            }

        }

        //sendResponse, used by DB/AUTH to respond to logic layers
        public void sendResponse(object obj, string DestinationQueue)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            if (RequestGuid == null)
            {
                throw new NullReferenceException("No Message To Use For Response");
            }

            // Get object type
            Type objType = obj.GetType();

            // Open existing queue
            string destination = DestinationQueue;
            if (RequestIP != ".")
            {
                destination = "DIRECT=TCP:" + RequestIP + DestinationQueue.Substring(1);
            }
            using (MessageQueue queue = new MessageQueue(destination))
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    // Serialize the object as XML into the stream
                    DataContractSerializer serializer = new DataContractSerializer(objType);
                    serializer.WriteObject(stream, obj);

                    // Rewind the stream
                    stream.Position = 0;
                    Message response = new Message();
                    //Add serialized object to message
                    response.BodyStream = stream;

                    // Pass object type as message label
                    response.Label = RequestGuid + "|" + THIS_IP + "|" + objType.FullName;
                    RequestGuid = null;
                    RequestIP = null;
                    queue.Send(response);
                }

            }

        }

        //receiveByID, used by Logic Layers to get responses from DB/AUTH
        public object receiveByID(string MessageID, string InputQueue)
        {
            // Open existing queue
            using (MessageQueue queue = new MessageQueue(InputQueue))
            {
                //Peek to find message with the MessageID in the label
                while (true)
                {
                    Message[] peekedmessage = null;
                    try
                    {
                        peekedmessage = queue.GetAllMessages();
                        foreach (Message m in peekedmessage)
                        {
                            if (m.Label.StartsWith(MessageID))
                            {
                                using (Message message = queue.ReceiveById(m.Id))
                                {
                                    RequestGuid = MessageID;
                                    // Gets object type from the message label
                                    Type objType = Type.GetType(message.Label.Split('|')[2], true, true);

                                    // Derializes object from the stream
                                    DataContractSerializer serializer = new DataContractSerializer(objType);
                                    return serializer.ReadObject(message.BodyStream);
                                }
                            }
                        }
                    }
                    catch (MessageQueueException /*mqe*/)
                    {

                    }
                    
                    System.Threading.Thread.Sleep(10);
                }
            }
        }


    }
}