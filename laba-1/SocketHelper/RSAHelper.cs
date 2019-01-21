using System.Security.Cryptography;

namespace SocketHelper
{
    public class RSAHelper
    {
        public static byte[] Encrypt(byte[] data, RSAParameters publicKey)
        {
            var provider = new RSACryptoServiceProvider();

            provider.ImportParameters(publicKey);
            return provider.Encrypt(data, false);
        }

        public static byte[] Decrypt(byte[] data, RSAParameters privateKey)
        {
            var provider = new RSACryptoServiceProvider();

            provider.ImportParameters(privateKey);
            return provider.Decrypt(data, false);
        }
    }
}
