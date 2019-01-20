using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SocketHelper
{
    public class RSAHelper
    {
        static public byte[] Encrypt(byte[] data, RSAParameters publicKey)
        {
            var provider = new RSACryptoServiceProvider();

            provider.ImportParameters(publicKey);
            return provider.Encrypt(data, false);
        }

        static public byte[] Decrypt(byte[] data, RSAParameters privateKey)
        {
            var provider = new RSACryptoServiceProvider();

            provider.ImportParameters(privateKey);
            return provider.Decrypt(data, false);
        }
    }
}
