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

        public static SModel Cube
        {
            get
            {
                return cube;
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
            cube = SModel.FromBits(SEdit.Properties.Resources.ss_cube);
            sphere = SModel.FromBits(SEdit.Properties.Resources.ss_sphere);
        }
    }
}
