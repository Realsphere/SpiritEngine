using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realsphere.Spirit.Mathematics
{
    public class SVector2
    {
        public float X;
        public float Y;

        public static implicit operator SVector2(Point a)
        {
            return new SVector2(a.X, a.Y);
        }

        public static implicit operator SVector2(SharpDX.Vector2 a)
        {
            return new SVector2(a.X, a.Y);
        }

        public static implicit operator System.Numerics.Vector2(SVector2 a)
        {
            return new(a.X, a.Y);
        }

        internal SharpDX.Vector2 sharpDXVector
        {
            get
            {
                return new(X, Y);
            }
        }

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}";
        }

        public SVector2()
        {
            X = 0;
            Y = 0;
        }

        public SVector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        #region Operators
        public static SVector2 operator +(SVector2 a, SVector2 b)
        {
            return new SVector2(a.X + b.X, a.Y + b.Y);
        }
        public static SVector2 operator -(SVector2 a, SVector2 b)
        {
            return new SVector2(a.X - b.X, a.Y - b.Y);
        }
        public static SVector2 operator /(SVector2 a, SVector2 b)
        {
            return new SVector2(a.X / b.X, a.Y / b.Y);
        }
        public static SVector2 operator *(SVector2 a, SVector2 b)
        {
            return new SVector2(a.X * b.X, a.Y * b.Y);
        }
        public static SVector2 operator *(SVector2 a, float b)
        {
            return new SVector2(a.X * b, a.Y * b);
        }
        public static SVector2 operator /(SVector2 a, float b)
        {
            return new SVector2(a.X / b, a.Y / b);
        }
        #endregion
    }
}
