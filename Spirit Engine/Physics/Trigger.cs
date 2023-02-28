using BulletSharp;
using Realsphere.Spirit.BulletPhysics;
using Realsphere.Spirit.Internal;
using Realsphere.Spirit.Mathematics;
using Realsphere.Spirit.Rendering;
using Realsphere.Spirit.SceneManagement;
using SharpDX;
using SharpDX.Direct2D1.Effects;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System.Collections.Generic;
using System.Linq;

namespace Realsphere.Spirit.Physics
{
    /// <summary>
    /// A Physics Trigger.
    /// </summary>
    public class Trigger
    {
        SVector3 pos = new();
        SVector3 scl = new();
        SQuaternion rot = new();
        internal GameObject go;

        public SVector3 Position
        {
            get => pos;
            set
            {
                pos = value;
                setRendererWorld();
            }
        }
        public SVector3 Scale
        {
            get => scl;
            set
            {
                scl = value;
                setRendererWorld();
            }
        }
        public SQuaternion Rotation
        {
            get => rot;
            set
            {
                rot = value;
                setRendererWorld();
            }
        }

        void setRendererWorld()
        {
            go.Transform.Position = pos;
            go.Transform.Scale = scl;
            go.Transform.Rotation = rot;
        }

        public Trigger(SVector3 position, SVector3 scale, SQuaternion rot)
        {
            go = GameObject.CreateUsingMesh(StandarizedShapes.Cube, "Mesh");
            go.Material = SMaterial.Create(STexture.Load("coke.png"));

            this.Rotation = rot;
            this.Position = position;
            this.Scale = scale;
        }

        public Trigger()
            : this(new(), new(), new()) { }

        public bool IsInside(GameObject go)
        {
            return doCheck(go, go.Transform.Position.sharpDXVector, go.Transform.Scale.sharpDXVector,
                new Quaternion(go.Transform.Rotation.X, go.Transform.Rotation.Y, go.Transform.Rotation.Z, go.Transform.Rotation.W), pos.sharpDXVector, scl.sharpDXVector, new Quaternion(rot.X, rot.Y, rot.Z, rot.W));
        }

        bool doCheck(GameObject go, Vector3 posA, Vector3 scaleA, Quaternion rotA, Vector3 posB, Vector3 scaleB, Quaternion rotB)
        {
            var cornersA = new Vector3[]
            {
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f),
                new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, 0.5f, 0.5f),
                new Vector3(0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, 0.5f),
                new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, 0.5f)
            };

            List<Vector3> verts = new();

            foreach (var buf in go.renderer.Mesh.VertexBuffers)
                verts.AddRange(buf.Select(x => new Vector3(x.Position.X, x.Position.Y, x.Position.Z)));

            for (int i = 0; i < verts.Count; i++)
            {
                if (i == 0) continue;

                verts[i] *= go.Transform.Scale.sharpDXVector;
            }

            var cornersB = verts.ToArray();


            // Calculate the world matrix for each object
            Matrix worldA = Matrix.Scaling(scaleA) * Matrix.RotationQuaternion(rotA) * Matrix.Translation(posA);
            Matrix worldB = Matrix.Scaling(scaleB) * Matrix.RotationQuaternion(rotB) * Matrix.Translation(posB);

            for (int i = 0; i < cornersA.Length; i++)
            {
                var tr = Vector3.Transform(cornersA[i], Matrix.RotationQuaternion(rotA));
                cornersA[i] = new(tr.X, tr.Y, tr.Z);
            }
            for (int i = 0; i < cornersB.Length; i++)
            {
                var tr = Vector3.Transform(cornersB[i], Matrix.RotationQuaternion(rotA));
                cornersB[i] = new(tr.X, tr.Y, tr.Z);
            }

            // Transform the AABB of each
            BoundingBox boxA = BoundingBox.FromPoints(cornersA);
            BoundingBox boxB = BoundingBox.FromPoints(cornersB);

            boxA.Minimum = new(SharpDX.Vector3.Transform(boxA.Minimum, worldA).X, SharpDX.Vector3.Transform(boxA.Minimum, worldA).Y, SharpDX.Vector3.Transform(boxA.Minimum, worldA).Z);
            boxA.Maximum = new(SharpDX.Vector3.Transform(boxA.Maximum, worldA).X, SharpDX.Vector3.Transform(boxA.Maximum, worldA).Y, SharpDX.Vector3.Transform(boxA.Maximum, worldA).Z);
            boxB.Minimum = new(SharpDX.Vector3.Transform(boxB.Minimum, worldB).X, SharpDX.Vector3.Transform(boxB.Minimum, worldB).Y, SharpDX.Vector3.Transform(boxB.Minimum, worldB).Z);
            boxB.Maximum = new(SharpDX.Vector3.Transform(boxB.Maximum, worldB).X, SharpDX.Vector3.Transform(boxB.Maximum, worldB).Y, SharpDX.Vector3.Transform(boxB.Maximum, worldB).Z);

            return boxA.Intersects(boxB);
        }

        internal void render(DeviceContext context)
        {
            Game.app.RenderObject(go, go.WorldTransform, context, Game.app.viewProjection);
        }
    }
}
