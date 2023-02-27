using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realsphere.Spirit.Modelling;

namespace Realsphere.Spirit.Rendering
{
    public static class StandarizedShapes
    {
        static SModel cube;
        static SModel sphere;
        static SModel empty;

        public static SModel Cube
        {
            get
            {
                return cube;
            }
        }
        public static SModel Empty
        {
            get
            {
                return empty;
            }
        }
        public static SModel Sphere
        {
            get
            {
                return sphere;
            }
        }
        internal static void init()
        {
            cube = SModel.FromBits(Properties.Resources.ss_cube);
            sphere = SModel.FromBits(Properties.Resources.ss_sphere);
            empty = new()
            {
            };
        }
    }
}
