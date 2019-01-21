using System.IO;
using System.Security.Cryptography;

namespace SocketHelper
{
    public class DESHelper
    {
        public static byte[] Encrypt(byte[] data, byte[] key, byte[] vector)
        {
            var provider = DES.Create();
            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream,
                provider.CreateEncryptor(key, vector),
                CryptoStreamMode.Write);
            
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();

            var result = memoryStream.ToArray();

            cryptoStream.Close();
            memoryStream.Close();

            return result;
        }

        public static byte[] Decrypt(byte[] data, byte[] key, byte[] vector)
        {
            var memoryStream = new MemoryStream(data);

            var provider = DES.Create();

            var csDecrypt = new CryptoStream(memoryStream,
                provider.CreateDecryptor(key, vector),
                CryptoStreamMode.Read);

            byte[] result = new byte[data.Length];
            csDecrypt.Read(result, 0, result.Length);

            return result;
        }
    }
}
