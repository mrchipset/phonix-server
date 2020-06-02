using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace phonix_api.Utility
{
    public static class Memory
    {
        public static MemoryStream ObjectToStream(ref object o)
        {
            MemoryStream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, o);
            return stream;
        }

        public static object StreamToObject(ref MemoryStream stream)
        {
            IFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(stream);
        }
    }
}
