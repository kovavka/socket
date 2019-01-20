﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using Domain;
using SocketHelper;

namespace Second
{
    public class Controller
    {
        static int inputPort = 8005;
        static int outputPort = 8006;
        private static SocketManager manager = new SocketManager();
        private RSAParameters privateKey;
        private RSAParameters publicKey;

        private string fileName = "file.txt";


        public void Start()
        {

            //получить запрос на создание ключа
            //создать RSA, отправить ключ
            //получить данные
            //записать


            WaitRequest();
            CreateRsa();

            var dtos = WaitData();
            Console.WriteLine(dtos[0].City);
            SaveData();
        }

        void WaitRequest()
        {
            //manager.Listen(inputPort);
        }

        void CreateRsa()
        {
            var rsaProvider = new RSACryptoServiceProvider();
            privateKey = rsaProvider.ExportParameters(true);
            publicKey = rsaProvider.ExportParameters(false);

            Console.WriteLine("Sending publicKey");
            manager.Send<RSAParameters>(outputPort, publicKey);
        }


        List<EventDto> WaitData()
        {
            Console.WriteLine("Listen encrypted data");
            var encData = manager.Listen(inputPort);
            Console.WriteLine("encrypted data is received");

            var data = Decrypt(encData);

            UnicodeEncoding byteConverter = new UnicodeEncoding();
            Console.WriteLine(byteConverter.GetString(data));

            //Console.WriteLine("data are decrypted");
            //var dtos = FromByteArray<List<EventDto>>(data);
            //Console.WriteLine("dtos is ready to parse");
            //return dtos;
            return new List<EventDto>();
        }

        byte[] Decrypt(byte[] encData)
        {
            byte[] data = RSAHelper.Decrypt(encData, privateKey);
            return data;
        }

        void SaveData()
        {

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
