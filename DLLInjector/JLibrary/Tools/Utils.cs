namespace _i
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization.Formatters.Binary;

    public static class Utils
    {
        public static T DeepClone<T>(T obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0L;
                return (T) formatter.Deserialize(stream);
            }
        }

        public static uint SizeOf(this Type t)
        {
            return (uint) Marshal.SizeOf(t);
        }

        public static string WriteTempData(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            string path = null;
            try
            {
                path = System.IO.Path.GetTempFileName();
            }
            catch (IOException)
            {
                path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), System.IO.Path.GetRandomFileName());
            }
            try
            {
                File.WriteAllBytes(path, data);
            }
            catch
            {
                path = null;
            }
            return path;
        }
    }
}

