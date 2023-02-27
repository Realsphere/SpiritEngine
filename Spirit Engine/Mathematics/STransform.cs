using Realsphere.Spirit.SceneManagement;
using SharpDX;

namespace Realsphere.Spirit.Mathematics
{
    public class STransform
    {
        SVector3 pos;
        SQuaternion rot;
        SVector3 scle;
        public SVector3 Position
        {
            get
            {
                return pos;
            }
            set
            {
                pos = value;
                if (on != null) on.WorldTransform = Matrix.Translation(value.sharpDXVector) * Matrix.RotationX(rot.X) * Matrix.RotationY(rot.Y) * Matrix.RotationZ(rot.Z) * Matrix.Scaling(scle.X, scle.Y, scle.Z);
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
                if (on != null) on.WorldTransform = Matrix.Translation(pos.sharpDXVector) * Matrix.RotationX(value.X) * Matrix.RotationY(value.Y) * Matrix.RotationZ(value.Z) * Matrix.Scaling(scle.X, scle.Y, scle.Z);
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
                if (on != null) on.WorldTransform = Matrix.Translation(pos.sharpDXVector) * Matrix.RotationX(rot.X) * Matrix.RotationY(rot.Y) * Matrix.RotationZ(rot.Z) * Matrix.Scaling(scle.X, scle.Y, scle.Z);
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

        public STransform()
        {
            Position = new SVector3();
            Scale = new SVector3(1f, 1f, 1f);
            Rotation = new SQuaternion();
        }
    }
}
