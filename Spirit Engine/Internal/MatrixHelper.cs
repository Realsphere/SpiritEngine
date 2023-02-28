using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Realsphere.Spirit.Internal
{
    internal class MatrixHelper
    {
        internal static Matrix4x4 TRS(Vector3 translation, Quaternion rotation, Vector3 scale, Vector3 center)
        {
            return Matrix4x4.CreateTranslation(translation) * Matrix4x4.CreateRotationX(rotation.X) * Matrix4x4.CreateRotationY(rotation.Y) * Matrix4x4.CreateRotationZ(rotation.Z) * Matrix4x4.CreateScale(scale);
        }
    }
}
