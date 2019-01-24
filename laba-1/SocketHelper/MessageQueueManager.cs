using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Messaging;
using System.Threading;

namespace SocketHelper
{
    public class MessageQueueManager
    {
        private static string path = @".\private$\MyNewPrivateQueue";
        public void Send<T>(T obj, string label)
        {
            var data = ObjectToByteArray(obj);
            Send(data, label);
        }

        public void Send(byte[] data, string label)
        {
            if (!MessageQueue.Exists(path))
            {
                MessageQueue.Create(path);
            }

            using (Message msg = new Message())
            {
                msg.Body = data;
                msg.Recoverable = true;
                msg.Label = label;
                msg.Formatter = new BinaryMessageFormatter();
                string queuePath = @".\private$\MyNewPrivateQueue";
                using (MessageQueue messageQueue = new MessageQueue(queuePath))
                {
                    messageQueue.Formatter = new BinaryMessageFormatter();
                    messageQueue.Send(msg);
                }
            }
        }

        public T Receive<T>(string label)
        {
            var data = Receive(label);
            return FromByteArray<T>(data);
        }

        public byte[] Receive(string label)
        {
            using (var messageQueue = new MessageQueue(@".\private$\MyNewPrivateQueue"))
                try
                {
                    messageQueue.Formatter = new BinaryMessageFormatter();
                    messageQueue.MessageReadPropertyFilter.SetAll();

                    var messages = new List<Message>();
                    byte[] data = null;
                    while (!messages.Any())
                    {
                        List<Message> allMessages = messageQueue.GetAllMessages().ToList();
                        messages = allMessages.Where(x => x.Label == label).ToList();
                        
                        foreach (Message message in messages)
                        {
                            data = (byte[])message.Body;
                            messageQueue.ReceiveById(message.Id);
                        }

                        if (!messages.Any())
                            Thread.Sleep(1000);
                    }

                    return data;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                    throw;
                }
        }

        byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        T FromByteArray<T>(byte[] data)
        {
            if (data == null)
                return default(T);
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(data))
            {
                object obj = bf.Deserialize(ms);
                return (T)obj;
            }
        }
    }
}