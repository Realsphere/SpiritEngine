using Realsphere.Spirit.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realsphere.Spirit.Networking
{
    /// <summary>
    /// Spirit Packet class, intenteded for usage with <see cref="Client"/>
    /// </summary>
    public class Packet : IDisposable
    {
        public const int MaxSize = 1024;

        internal MemoryStream ms;

        public Packet() { ms = new(); }
        public Packet(byte[] data) { ms = new(data); }

        public void Write(float value) 
            => ms.Write(BitConverter.GetBytes(value));
        public void Write(int value) 
            => ms.Write(BitConverter.GetBytes(value));
        public void Write(long value) 
            => ms.Write(BitConverter.GetBytes(value));
        public void Write(short value) 
            => ms.Write(BitConverter.GetBytes(value));
        public void Write(uint value) 
            => ms.Write(BitConverter.GetBytes(value));
        public void Write(ulong value) 
            => ms.Write(BitConverter.GetBytes(value));
        public void Write(ushort value) 
            => ms.Write(BitConverter.GetBytes(value));
        public void Write(SVector3 value)
        {
            Write(value.X);
            Write(value.Y);
            Write(value.Z);
        }
        public void Write(SVector2 value)
        {
            Write(value.X);
            Write(value.Y);
        }
        public void Write(string str)
        {
            int stringLength = str.Length;
            Write(stringLength);
            for (int i = 0; i < stringLength; i++)
                ms.WriteByte((byte)str[i]);
        }

        public float ReadFloat()
        {
            byte[] bytes = new byte[sizeof(float)];
            ms.Read(bytes, 0, bytes.Length);
            return BitConverter.ToSingle(bytes, 0);
        }

        public int ReadInt()
        {
            byte[] bytes = new byte[sizeof(int)];
            ms.Read(bytes, 0, bytes.Length);
            return BitConverter.ToInt32(bytes, 0);
        }

        public short ReadShort()
        {
            byte[] bytes = new byte[sizeof(short)];
            ms.Read(bytes, 0, bytes.Length);
            return BitConverter.ToInt16(bytes, 0);
        }

        public long ReadLong()
        {
            byte[] bytes = new byte[sizeof(long)];
            ms.Read(bytes, 0, bytes.Length);
            return BitConverter.ToInt64(bytes, 0);
        }

        public uint ReadUInt()
        {
            byte[] bytes = new byte[sizeof(uint)];
            ms.Read(bytes, 0, bytes.Length);
            return BitConverter.ToUInt32(bytes, 0);
        }

        public ushort ReadUShort()
        {
            byte[] bytes = new byte[sizeof(ushort)];
            ms.Read(bytes, 0, bytes.Length);
            return BitConverter.ToUInt16(bytes, 0);
        }

        public ulong ReadULong()
        {
            byte[] bytes = new byte[sizeof(ulong)];
            ms.Read(bytes, 0, bytes.Length);
            return BitConverter.ToUInt64(bytes, 0);
        }

        public SVector3 ReadVector3()
        {
            float x = ReadFloat();
            float y = ReadFloat();
            float z = ReadFloat();
            return new(x, y, z);
        }

        public SVector2 ReadVector2()
        {
            float x = ReadFloat();
            float y = ReadFloat();
            return new(x, y);
        }

        public string ReadString()
        {
            string result = "";
            int stringLength = ReadInt();
            for (int i = 0; i < stringLength; i++)
            {
                byte[] character = new byte[1];
                ms.Read(character, 0, character.Length);
                result += (char)character[0];
            }
            return result;
        }

        public byte[] GetBytes()
            => ms.ToArray();

        public void Dispose()
        {
            ms.Dispose();
        }
    }
}
