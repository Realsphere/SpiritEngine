using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realsphere.Spirit.Rendering
{
    public class SColor
    {
        public float R;
        public float G;
        public float B;
        public float A;
        internal Color sharpdxcolor;

        public static SColor White
        {
            get
            {
                return new SColor(255f, 255f, 255f, 255f);
            }
        }

        internal static SColor fromsharpdx(Color4 c)
        {
            return new SColor(c.Red, c.Green, c.Blue, c.Alpha);
        }

        public static SColor operator +(SColor a, SColor b)
        {
            return new SColor(Math.Clamp(a.R + b.R, 0f, 255f),
                              Math.Clamp(a.G + b.G, 0f, 255f),
                              Math.Clamp(a.B + b.B, 0f, 255f),
                              Math.Clamp(a.A + b.A, 0f, 255f));
        }

        public static SColor operator -(SColor a, SColor b)
        {
            return new SColor(Math.Clamp(a.R - b.R, 0f, 255f),
                              Math.Clamp(a.G - b.G, 0f, 255f),
                              Math.Clamp(a.B - b.B, 0f, 255f),
                              Math.Clamp(a.A - b.A, 0f, 255f));
        }

        public static SColor operator *(SColor a, SColor b)
        {
            return new SColor(Math.Clamp(a.R * b.R, 0f, 255f),
                              Math.Clamp(a.G * b.G, 0f, 255f),
                              Math.Clamp(a.B * b.B, 0f, 255f),
                              Math.Clamp(a.A * b.A, 0f, 255f));
        }

        public override string ToString()
        {
            return $"{R}, {G}, {B}, {A}";
        }

        public static SColor operator /(SColor a, SColor b)
        {
            return new SColor(Math.Clamp(a.R / b.R, 0f, 255f), Math.Clamp(a.G / b.G, 0f, 255f), Math.Clamp(a.B / b.B, 0f, 255f), Math.Clamp(a.A / b.A, 0f, 255f));
        }

        public SColor(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
            sharpdxcolor = new Color();
            sharpdxcolor.R = (byte)R;
            sharpdxcolor.G = (byte)G;
            sharpdxcolor.B = (byte)B;
            sharpdxcolor.A = (byte)A;
        }
    }
}
