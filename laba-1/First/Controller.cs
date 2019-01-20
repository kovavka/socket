using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Domain;
using SocketHelper;

namespace First
{
    public class Controller
    {
        static int inputPort = 8006;
        static int outputPort = 8005;

        private SocketManager manager = new SocketManager();

        public void Start()
        {
            var dtos = GetDtos();
            var publicKey = GetPublicKey();

            var data = Encrypt(dtos, publicKey);

            Send(data);

            //getdata

            //шифрование
            //получить ключ
            //зашифровать ключ DES
            //отправить данные
        }

        List<EventDto> GetDtos()
        {
            var dtos = new List<EventDto>()
            {
                new EventDto()
                {
                    City = "Пермь",
                    CityType = "город",
                    Country = "Россия",
                    EventComment = "кауккупуеип",
                    EventInfo = "рикпркил",
                    EventName = "Полет",
                    Execution = new DateTime(2017, 2, 22),
                    House = "37",
                    Region = "Пермский край",
                    Street = "Студенческая"
                }
            };

            Console.WriteLine("dtos are received from db");

            return dtos;
        }

        byte[] Encrypt(List<EventDto> dtos, RSAParameters publicKey)
        {
            byte[] data = RSAHelper.RSAEncrypt(ObjectToByteArray(dtos), publicKey, false);
            Console.WriteLine("dtos are encrypted");

            return data;
        }

        RSAParameters GetPublicKey()
        {
            Console.WriteLine("Get publicKey");
            var value = new RSAParameters();
            Thread thread = new Thread(() =>
            {
                value = manager.Listen<RSAParameters>(inputPort);
            });
            thread.Start();
            Console.WriteLine("Send request for publicKey");
            manager.Send(outputPort, new byte[1]);
            Console.WriteLine("Send request for publicKey");
            thread.Join();

            Console.WriteLine("PublicKey is received");
            return value;
        }
        
        void Send(byte[] data)
        {
            Console.WriteLine("Sending encrypted dtos");
            manager.Send(outputPort, data);
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
