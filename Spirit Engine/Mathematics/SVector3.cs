using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realsphere.Spirit.Mathematics
{
    public class SVector3
    {
        public float X;
        public float Y;
        public float Z;

        public SVector3 Normalized
        {
            get
            {
                return new(SMath.Normalize(X), SMath.Normalize(Y), SMath.Normalize(Z));
            }
        }

        public void Normalize()
        {
            X = SMath.Normalize(X);
            Y = SMath.Normalize(Y);
            Z = SMath.Normalize(Z);
        }

        public SVector3 Cross(SVector3 vec)
        {
            return new(
                Y * vec.Z - Z * vec.Y,
                Z * vec.X - X * vec.Z,
                X * vec.Y - Y * vec.X);
        }

        public static SVector3 Cross(SVector3 vec1, SVector3 vec2)
        {
            return new(
                vec1.Y * vec2.Z - vec1.Z * vec2.Y,
                vec1.Z * vec2.X - vec1.X * vec2.Z,
                vec1.X * vec2.Y - vec1.Y * vec2.X);
        }

        public float Dot(SVector3 other)
        {
            return X * other.X + Y * other.Y + Z * other.Z;
        }
        public static float Dot(SVector3 a, SVector3 other)
        {
            return a.X * other.X + a.Y * other.Y + a.Z * other.Z;
        }

        public SVector3 Cross(float x, float y)
        {
            return new(
                Y * 0f - Z * y,
                Z * x - X * 0f,
                X * y - Y * x);
        }

        public static implicit operator SVector3(SharpDX.Vector3 a)
        {
            return new SVector3(a.X, a.Y, a.Z);
        }

        public static implicit operator SVector3(System.Numerics.Vector3 a)
        {
            return new SVector3(a.X, a.Y, a.Z);
        }

        public static implicit operator System.Numerics.Vector3(SVector3 a)
        {
            return new(a.X, a.Y, a.Z);
        }

        internal SharpDX.Vector3 sharpDXVector
        {
            get
            {
                return new(X, Y, Z);
            }
        }

        public float DistanceTo(SVector3 point)
        {
            float num = X - point.X;
            float num2 = Y - point.Y;
            float num3 = Z - point.Z;
            return (float)Math.Sqrt(num * num + num2 * num2 + num3 * num3);
        }

        public static float Distance(SVector3 a, SVector3 b) => a.DistanceTo(b);

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}, Z: {Z}";
        }

        internal float LengthSquared()
        {
            return ((System.Numerics.Vector3)this).LengthSquared();
        }

        public SVector3()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        public SVector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        #region Operators
        public static SVector3 operator +(SVector3 a, SVector3 b)
        {
            return new SVector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }
        public static SVector3 operator -(SVector3 a, SVector3 b)
        {
            return new SVector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }
        public static SVector3 operator /(SVector3 a, SVector3 b)
        {
            return new SVector3(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
        }
        public static SVector3 operator *(SVector3 a, SVector3 b)
        {
            return new SVector3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        }
        public static SVector3 operator *(SVector3 a, float b)
        {
            return new SVector3(a.X * b, a.Y * b, a.Z * b);
        }
        public static SVector3 operator *(float b, SVector3 a)
        {
            return new SVector3(a.X * b, a.Y * b, a.Z * b);
        }
        public static SVector3 operator /(SVector3 a, float b)
        {
            return new SVector3(a.X / b, a.Y / b, a.Z / b);
        }
        public static SVector3 operator +(SVector3 a, float b)
        {
            return new SVector3(a.X + b, a.Y + b, a.Z + b);
        }
        public static SVector3 operator -(SVector3 a, float b)
        {
            return new SVector3(a.X - b, a.Y - b, a.Z - b);
        }
        #endregion
    }
}
