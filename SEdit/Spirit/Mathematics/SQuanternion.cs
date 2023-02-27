using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realsphere.Spirit.Mathematics
{
    public class SQuaternion
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public static SQuaternion operator +(SQuaternion a, SQuaternion b)
        {
            // Max: 1048576
            // Min: -1048576
            SQuaternion res = new SQuaternion()
            {
                X = a.X + b.X,
                Y = a.Y + b.Y,
                Z = a.Z + b.Z,
                W = a.W + b.W
            };

            if (res.X >= 1048576) res.X = 0;
            if (res.X <= -1048576) res.X = 0;
            if (res.Y >= 1048576) res.Y = 0;
            if (res.Y <= -1048576) res.Y = 0;
            if (res.Z >= 1048576) res.Z = 0;
            if (res.Z <= -1048576) res.Z = 0;
            if (res.W >= 1048576) res.W = 0;
            if (res.W <= -1048576) res.W = 0;

            return res;
        }

        public static SQuaternion operator -(SQuaternion a, SQuaternion b)
        {
            // Max: 1048576
            // Min: -1048576
            SQuaternion res = new SQuaternion()
            {
                X = a.X - b.X,
                Y = a.Y - b.Y,
                Z = a.Z - b.Z,
                W = a.W - b.W
            };

            if (res.X >= 1048576) res.X = 0;
            if (res.X <= -1048576) res.X = 0;
            if (res.Y >= 1048576) res.Y = 0;
            if (res.Y <= -1048576) res.Y = 0;
            if (res.Z >= 1048576) res.Z = 0;
            if (res.Z <= -1048576) res.Z = 0;
            if (res.W >= 1048576) res.W = 0;
            if (res.W <= -1048576) res.W = 0;

            return res;
        }

        public static SQuaternion operator /(SQuaternion a, SQuaternion b)
        {
            // Max: 1048576
            // Min: -1048576
            SQuaternion res = new SQuaternion()
            {
                X = a.X / b.X,
                Y = a.Y / b.Y,
                Z = a.Z / b.Z,
                W = a.W / b.W
            };

            if (res.X >= 1048576) res.X = 0;
            if (res.X <= -1048576) res.X = 0;
            if (res.Y >= 1048576) res.Y = 0;
            if (res.Y <= -1048576) res.Y = 0;
            if (res.Z >= 1048576) res.Z = 0;
            if (res.Z <= -1048576) res.Z = 0;
            if (res.W >= 1048576) res.W = 0;
            if (res.W <= -1048576) res.W = 0;

            return res;
        }

        public static SQuaternion operator *(SQuaternion a, SQuaternion b)
        {
            // Max: 1048576
            // Min: -1048576
            SQuaternion res = new SQuaternion()
            {
                X = a.X * b.X,
                Y = a.Y * b.Y,
                Z = a.Z * b.Z,
                W = a.W * b.W
            };

            if (res.X >= 1048576) res.X = 0;
            if (res.X <= -1048576) res.X = 0;
            if (res.Y >= 1048576) res.Y = 0;
            if (res.Y <= -1048576) res.Y = 0;
            if (res.Z >= 1048576) res.Z = 0;
            if (res.Z <= -1048576) res.Z = 0;
            if (res.W >= 1048576) res.W = 0;
            if (res.W <= -1048576) res.W = 0;

            return res;
        }

        public SQuaternion()
        {
            X = 0;
            Y = 0;
            Z = 0;
            W = 0;
        }

        public SQuaternion(SVector3 vec)
        {
            X = vec.X;
            Y = vec.Y;
            Z = vec.Z;
            W = 0f;
        }

        public SQuaternion(SVector3 vec, float w)
        {
            X = vec.X;
            Y = vec.Y;
            Z = vec.Z;
            W = w;
        }

        public SQuaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
    }
}
