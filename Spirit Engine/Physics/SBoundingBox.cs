using Realsphere.Spirit.Mathematics;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realsphere.Spirit.Physics
{
    public struct SBoundingBox
    {
        public SVector3 Min;
        public SVector3 Max;
        internal BoundingBox dx;

        internal static SBoundingBox FDX(BoundingBox d)
        {
            return new SBoundingBox(d.Minimum, d.Maximum);
        }

        public SBoundingBox(SVector3 min, SVector3 max)
        {
            Min = min; Max = max;
            dx = new(min.sharpDXVector, max.sharpDXVector);
        }

        public bool Intersects(SBoundingBox box)
        {
            return dx.Intersects(box.dx);
        }
    }
}
