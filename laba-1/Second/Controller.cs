using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using Domain;
using Infrastructure;
using SocketHelper;

namespace Second
{
    public class Controller
    {
        static int inputPort = 8005;
        static int outputPort = 8006;
        private SocketManager socketManager = new SocketManager();
        private MessageQueueManager queueManager = new MessageQueueManager();
        private RSAParameters privateKey;
        private RSAParameters publicKey;
        
        public void Start()
        {
            CreateRsa();

            var dtos = WaitData();
            Console.WriteLine(dtos[0].City);
            SaveData(dtos);
        }

        void CreateRsa()
        {
            var rsaProvider = new RSACryptoServiceProvider();
            privateKey = rsaProvider.ExportParameters(true);
            publicKey = rsaProvider.ExportParameters(false);

            Console.WriteLine("Sending publicKey");
            queueManager.Send(publicKey, "publicKey");
            //socketManager.Send<RSAParameters>(outputPort, publicKey);
        }


        List<EventDto> WaitData()
        {
            Console.WriteLine("Listen encrypted data");
            var data = queueManager.Receive<EncryptData>("EncryptData");
            //var data = socketManager.Listen<EncryptData>(inputPort);
            Console.WriteLine("encrypted data is received");

            var encData = data.Data;
            var encKey = data.Key;
            var vector = data.Vector;

            var key = RsaDecrypt(encKey);
            var bytes = DESHelper.Decrypt(encData, key, vector);
            var dtos = FromByteArray<List<EventDto>>(bytes);

            Console.WriteLine("data are decrypted");
            return dtos;
        }

        byte[] RsaDecrypt(byte[] encData)
        {
            var temp = encData.ToList().Take(128);
            encData = temp.ToArray();
            byte[] data = RSAHelper.Decrypt(encData, privateKey);
            return data;
        }

        void SaveData(List<EventDto> dtos)
        {
            new Parser().Start(dtos);
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
