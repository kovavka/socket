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
        private byte[] key;
        private byte[] vector;

        public void Start()
        {
            var des = DES.Create();
            key = des.Key;
            vector = des.IV;


            var publicKey = GetPublicKey();
            var dtos = GetDtos();

            var bytes = ObjectToByteArray(dtos);
            var enc = DESHelper.Encrypt(bytes, key, vector);
            var result = DESHelper.Decrypt(enc, key, vector);
            var rrr = FromByteArray<List<EventDto>>(result);


            var data = Encrypt(dtos, publicKey);
            
            Thread.Sleep(2000);
            Send(data);

            Console.ReadLine();

            //getdata

            //шифрование
            //получить ключ
            //зашифровать ключ DES
            //отправить данные
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
            UnicodeEncoding byteConverter = new UnicodeEncoding();


            byte[] data = RSAHelper.Encrypt(byteConverter.GetBytes("237"), publicKey);
            Console.WriteLine(data.Length);
            Console.WriteLine("dtos are encrypted");

            return data;
        }

        RSAParameters GetPublicKey()
        {
            Console.WriteLine("Get publicKey");
            var value = manager.Listen<RSAParameters>(inputPort);
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
