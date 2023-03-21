using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Realsphere.Spirit.Internal
{
    class ShadowRenderer
    {
        internal struct ShadowObject
        {
            internal Vector3 Position;
            internal Quaternion Rotation;
            internal Vector3 Scale;
            internal Vector3[] Vertices;
            internal ushort[] Indices;
        }
    }
}
