using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realsphere.Spirit.BulletPhysics
{
    internal static class DXConversionHelper
    {
        internal static System.Numerics.Matrix4x4 DXToSN(SharpDX.Matrix martx)
        {
            return new(
                martx.M11,
                martx.M12,
                martx.M13,
                martx.M14,
                martx.M21,
                martx.M22,
                martx.M23,
                martx.M24,
                martx.M31,
                martx.M32,
                martx.M33,
                martx.M34,
                martx.M41,
                martx.M42,
                martx.M43,
                martx.M44);
        }
        internal static SharpDX.Matrix SNToDX(System.Numerics.Matrix4x4 martx)
        {
            return new(
                martx.M11,
                martx.M12,
                martx.M13,
                martx.M14,
                martx.M21,
                martx.M22,
                martx.M23,
                martx.M24,
                martx.M31,
                martx.M32,
                martx.M33,
                martx.M34,
                martx.M41,
                martx.M42,
                martx.M43,
                martx.M44);
        }
    }
}
