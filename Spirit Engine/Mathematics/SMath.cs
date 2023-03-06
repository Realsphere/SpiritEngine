using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realsphere.Spirit.Mathematics
{
    public static class SMath
    {
        public static float Normalize(float value)
        {
            float magnitude = (float)Math.Sqrt(value * value);
            if (magnitude > 0)
                return value / magnitude;
            else
                return 0f;
        }

        public static int Min(int value, int minimum)
        {
            return value < minimum ? value : minimum;
        }

        public static int Max(int value, int maximum)
        {
            return value > maximum ? value : maximum;
        }
    }
}
