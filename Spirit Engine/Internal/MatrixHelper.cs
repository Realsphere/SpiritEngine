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
            Matrix4x4 matrix = Matrix4x4.Identity;
            matrix *= Matrix4x4.CreateTranslation(-center); // Translate by negative of center
            matrix *= Matrix4x4.CreateTranslation(translation); // Apply translation
            matrix *= Matrix4x4.CreateFromQuaternion(rotation); // Apply rotation
            matrix *= Matrix4x4.CreateScale(scale); // Apply scale
            matrix *= Matrix4x4.CreateTranslation(center); // Translate back by center
            return matrix;
        }
    }
}
