using System;

namespace Domain
{
    [Serializable]
    public class EncryptData
    {
        public byte[] Data { get; set; }
        public byte[] Key { get; set; }
        public byte[] Vector { get; set; }
    }
}
