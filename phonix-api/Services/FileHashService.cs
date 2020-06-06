using System;
using System.IO;
using System.Security.Cryptography;

using phonix_api.Utility;
namespace phonix_api.Services
{
    public class FileHashService 
    {
        public enum HashType 
        {
            HYPER1,
            HYPER2,
            MD5,
            SHA256,
            SHA1
        }

        public FileHashService(HashType hashType=HashType.MD5)
        {
        }

        public string GetHash(ref object obj)
        {
            MemoryStream s = Utility.Memory.ObjectToStream(ref obj);
            return GetHash(ref s);
        }

        public string GetHash(ref MemoryStream stream)
        {
            HashAlgorithm hashAlgorithm = HashAlgorithm.Create("MD5");
            stream.Seek(0, SeekOrigin.Begin);
            byte[] hashBytes = hashAlgorithm.ComputeHash(stream);
            string[] hex = BitConverter.ToString(hashBytes).Split('-');
            return string.Concat(hex);
        }

    }
}