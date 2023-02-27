using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace SEdit
{
    public static class BinarySerialization
    {
        /// <summary>
        /// Writes the given object instance to a binary file.
        /// <para>Object type (and all child types) must be decorated with the [Serializable] attribute.</para>
        /// <para>To prevent a variable from being serialized, decorate it with the [NonSerialized] attribute; cannot be applied to properties.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the XML file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the XML file.</param>
        /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
        public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        /// <summary>
        /// Reads an object instance from a binary file.
        /// </summary>
        /// <typeparam name="T">The type of object to read from the XML.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the binary file.</returns>
        public static T ReadFromBinaryFile<T>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }
    }

    public class ByteStream : IDisposable
    {
        MemoryStream ms;

        private const int StringLength = 1024;

        public ByteStream(byte[] bits)
        {
            ms = new MemoryStream(bits);
        }

        public ByteStream(MemoryStream ms)
        {
            this.ms = ms;
        }

        public ByteStream()
        {
            ms = new MemoryStream();
        }

        public void WriteInt(int value)
        {
            ms.Write(BitConverter.GetBytes(value));
        }
        public void WriteFloat(float value)
        {
            ms.Write(BitConverter.GetBytes(value));
        }
        public void WriteShort(short value)
        {
            ms.Write(BitConverter.GetBytes(value));
        }
        public void WriteString(string str)
        {
            string value = str;

            while(value.Length < StringLength)
            {
                value += '\0';
            }

            for (int i = 0; i < StringLength; i++)
                WriteInt((byte)value[i] + i);
        }

        public int ReadInt()
        {
            int size = sizeof(int);
            byte[] buffer = new byte[size];
            ms.Read(buffer, 0, size);
            return BitConverter.ToInt32(buffer, 0);
        }
        public float ReadFloat()
        {
            int size = sizeof(float);
            byte[] buffer = new byte[size];
            ms.Read(buffer, 0, size);
            return BitConverter.ToSingle(buffer, 0);
        }
        public short ReadShort()
        {
            int size = sizeof(short);
            byte[] buffer = new byte[size];
            ms.Read(buffer, 0, size);
            return BitConverter.ToInt16(buffer, 0);
        }
        public string ReadString()
        {
            string val = "";

            List<int> data = new();

            for (int i = 0; i < StringLength; i++)
                data.Add(ReadInt() - i);

            foreach (int i in data)
            {
                val += (char)i;
            }

            return val;
        }

        public void Save(string path)
        {
            File.WriteAllBytes(path, ms.ToArray());
        }

        public void Dispose()
        {
            ms.Close();
            ms.Dispose();
        }
    }
}
