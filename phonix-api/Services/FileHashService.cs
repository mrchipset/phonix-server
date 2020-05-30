using System;

namespace phonix_api.Services
{
    public class FileHashService 
    {
        public enum HashType 
        {
            HYPER1,
            HYPER2,
            MD5,
            SHA
        }

        public FileHashService(HashType hashType)
        {
            throw new NotImplementedException();
        }

        public string GetHash(ref object obj)
        {
            throw new NotImplementedException();
        }

    }
}