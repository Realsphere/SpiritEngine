using Realsphere.Spirit.BulletPhysics;
using Realsphere.Spirit.Internal;
using Realsphere.Spirit.Physics;
using Realsphere.Spirit.SceneManagement;
using SharpDX;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Realsphere.Spirit.Mathematics
{
    public class STransform
    {
        SVector3 pos = new();
        SQuaternion rot = new();
        SVector3 scle = new();
        public SVector3 Position
        {
            get
            {
                return pos;
            }
            set
            {
                pos = value;
            }
        }
        public SQuaternion Rotation
        {
            get
            {
                return rot;
            }
            set
            {
                rot = value;
            }
        }
        public SVector3 Scale
        {
            get
            {
                return scle;
            }
            set
            {
                scle = value;
            }
        }

        internal GameObject on;

        internal Matrix World
        {
            get
            {
                return Matrix.Translation(Position.sharpDXVector) * Matrix.Scaling(Scale.sharpDXVector) * Matrix.RotationX(Rotation.Y) * Matrix.RotationY(Rotation.X) * Matrix.RotationZ(Rotation.Z);
            }
        }

        public STransform(GameObject o)
        {
            on = o;
            Position = new SVector3();
            Scale = new SVector3(1f, 1f, 1f);
            Rotation = new SQuaternion();
        }
    }
}