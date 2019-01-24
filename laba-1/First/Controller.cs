using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using Domain;
using Infrastructure;
using SocketHelper;
using EventDto = Domain.EventDto;

namespace First
{
    public class Controller
    {
        static int inputPort = 8006;
        static int outputPort = 8005;
        private SocketManager socketManager = new SocketManager();
        private MessageQueueManager queueManager = new MessageQueueManager();
        private byte[] key;
        private byte[] vector;

        public void Start()
        {
            var des = DES.Create();
            key = des.Key;
            vector = des.IV;

            var publicKey = GetPublicKey();
            var dtos = GetDtos();
            
            var enc = Encrypt(dtos);
            var encryptedKey = RSAHelper.Encrypt(key, publicKey);
            var data = new EncryptData()
            {
                Data = enc,
                Key = encryptedKey,
                Vector = vector
            };

            Send(data);
            
            Console.ReadLine();
        }

        List<EventDto> GetDtos()
        {
            var dtos = DataBaseHelper.GetDtos().ToList();

            Console.WriteLine("dtos are received from db");

            return dtos;
        }

        byte[] Encrypt(List<EventDto> dtos)
        {
            var bytes = ObjectToByteArray(dtos);
            var enc = DESHelper.Encrypt(bytes, key, vector);
            Console.WriteLine("dtos are encrypted");

            return enc;
        }

        RSAParameters GetPublicKey()
        {
            Console.WriteLine("Get publicKey");
            var value = queueManager.Receive<RSAParameters>("publicKey");
            //var value = socketManager.Listen<RSAParameters>(inputPort);
            Console.WriteLine("PublicKey is received");

            return value;
        }
        
        void Send(EncryptData data)
        {
            Console.WriteLine("Sending encrypted data");
            queueManager.Send(data, "EncryptData");
            //socketManager.Send(outputPort, data);
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
    }
}
