using System;
using System.IO;

namespace phonix_api.Services
{
    public class FileStorageService
    {
        private const string ROOTDIR = "pictures";
        public FileStorageService(FileHashService fileHashService)
        {
            this.fileHashService = fileHashService;
        }

        public bool Create(ref MemoryStream stream, out string path, out string hashCode)
        {
            hashCode = fileHashService.GetHash(ref stream);
            path = GetPathFromHashCode(hashCode);
            return TryToWriteToDisk(path, ref stream);
        }

        public bool Get(string path, out MemoryStream stream)
        {
            path = ROOTDIR + "/" + path;
            stream = null;
            if (!File.Exists(path))
            {
                return false;
            }
            stream = new MemoryStream();
            var file = new FileStream(path, FileMode.Open);
            file.CopyTo(stream);
            file.Close();
            return true;
        }

        private string GetPathFromHashCode(string hashCode)
        {
            string fileName = hashCode.Substring(2);
            string parentDir = hashCode.Substring(0, 1);
            string childDir = hashCode.Substring(1, 1);
            string[] s = { parentDir, childDir, fileName };
            return string.Join('/', s);
        }

        private bool TryToWriteToDisk(string path, ref MemoryStream stream)
        {
            string actualPath = ROOTDIR + "/" + path;
            string[] dir = actualPath.Split('/');
            string[] dirPath = new string[dir.Length-1];
            for (int i = 0;i < dirPath.Length; ++i)
            {
                dirPath[i] = dir[i];
            }
            Directory.CreateDirectory(string.Join('/', dirPath));
            if (File.Exists(actualPath))
            {
                return false;
            }
            FileStream fileStream =
                new FileStream(actualPath, FileMode.CreateNew);
            stream.Seek(0, SeekOrigin.Begin);
            stream.CopyTo(fileStream);
            fileStream.Close();
            return true;
        }

        private readonly FileHashService fileHashService;
    }
}
