using Realsphere.Spirit.Mathematics;
using Realsphere.Spirit.SceneManagement;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realsphere.Spirit.Physics
{
    /// <summary>
    /// A Physics Trigger.
    /// </summary>
    public class Trigger
    {
        public SVector3 Position = new();
        public SVector3 Scale = new();
        public SQuaternion Rotation = new();

        public Trigger(SVector3 position, SVector3 scale, SQuaternion rot)
        {
            this.Position = position;
            this.Scale = scale;
            this.Rotation = rot;
        }

        public Trigger()
            : this(new(), new(), new()) { }

        Vector3[] TransformCoordinates(Vector3[] pts, Matrix m)
        {
            List<Vector3> result = new List<Vector3>();

            foreach (Vector3 vec in pts)
                result.Add(Vector3.TransformCoordinate(vec, m));

            return result.ToArray();
        }

        bool AreMatricesInsideEachOther(Matrix matrix1, Matrix matrix2)
        {
            // Calculate the bounding boxes for each matrix
            var bb1 = CalculateBoundingBox(matrix1);
            var bb2 = CalculateBoundingBox(matrix2);

            // Convert the bounding boxes to world space
            bb1 = BoundingBox.FromPoints(TransformCoordinates(bb1.GetCorners(), matrix1));
            bb2 = BoundingBox.FromPoints(TransformCoordinates(bb2.GetCorners(), matrix2));

            // Check if bb1 is fully inside bb2 or bb2 is fully inside bb1
            return bb1.Contains(bb2) == ContainmentType.Contains || bb2.Contains(bb1) == ContainmentType.Contains;
        }

        BoundingBox CalculateBoundingBox(Matrix matrix)
        {
            var corners = new[]
            {
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f),
                new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, 0.5f, 0.5f),
                new Vector3(0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, 0.5f),
                new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, 0.5f),
            };

            Vector3.TransformCoordinate(corners, ref matrix, corners);

            return BoundingBox.FromPoints(corners);
        }

        public bool IsInside(GameObject go)
        {
            return AreMatricesInsideEachOther(go.WorldTransform, Matrix.Translation(Position.sharpDXVector) *
                Matrix.Scaling(Scale.sharpDXVector) * Matrix.RotationX(Rotation.X) * Matrix.RotationY(Rotation.Y) * Matrix.RotationZ(Rotation.Z));
        }
    }
}
