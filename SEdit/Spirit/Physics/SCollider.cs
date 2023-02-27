using BulletSharp.SoftBody;
using Realsphere.Spirit.Internal;
using Realsphere.Spirit.Mathematics;
using Realsphere.Spirit.SceneManagement;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realsphere.Spirit.Physics
{
    public class SBoxCollider : SCollider
    {
        public SBoxCollider()
        {
            Position = new SVector3();
            Rotation = new SQuaternion();
            Scale = new SVector3(1f, 1f, 1f);
            type = SColliderType.Box;
        }
        public SBoxCollider(SVector3 pos)
        {
            Position = pos;
            Rotation = new SQuaternion();
            Scale = new SVector3(1f, 1f, 1f);
            type = SColliderType.Box;
        }

        public SBoxCollider(SVector3 pos, SVector3 scale)
        {
            Position = pos;
            Rotation = new SQuaternion();
            Scale = scale;
            type = SColliderType.Box;
        }

        public SBoxCollider(SVector3 pos, SQuaternion rot)
        {
            Position = pos;
            Rotation = rot;
            Scale = new SVector3(1f, 1f, 1f);
            type = SColliderType.Box;
        }

        public SBoxCollider(SVector3 pos, SVector3 scale, SQuaternion rot)
        {
            Position = pos;
            Rotation = rot;
            Scale = scale;
            type = SColliderType.Box;
        }
    }

    internal enum SColliderType
    {
        Box,
        Sphere,
        Convex,
        Capsule
    }

    public class SCapsuleCollider : SCollider
    {
        public float Radius { get; }
        public float Height { get; }

        public SCapsuleCollider(SVector3 pos, float radius, float height)
        {
            Position = pos;
            Radius = radius;
            Height = height;
            type = SColliderType.Capsule;
        }

        public SCapsuleCollider(SVector3 pos, SQuaternion rot, float radius, float height)
        {
            Position = pos;
            Rotation = rot;
            Radius = radius;
            Height = height;
            type = SColliderType.Capsule;
        }
    }

    public class SSphereCollider : SCollider
    {
        public float Radius { get; }

        public SSphereCollider(SVector3 pos, float radius)
        {
            Position = pos;
            Radius = radius;
            type = SColliderType.Sphere;
        }

        public SSphereCollider() : this(new(), 1f) { }
    }

    public class SConvexCollider : SCollider
    {
        public SVector3[] Vertices { get; }

        internal float[] vertices;

        public SConvexCollider(SVector3[] verts, SVector3 pos)
            : this(verts, pos, new())
        {
        }

        public SConvexCollider(SVector3[] verts, SVector3 pos, SQuaternion rot)
        {
            Vertices = verts;

            Position = pos;
            Rotation = rot;

            float[] f = new float[verts.Length * 3];
            int i = 0;
            foreach (SVector3 vert in verts)
            {
                f[i] = vert.X;
                f[i + 1] = vert.Y;
                f[i + 2] = vert.Z;
                i += 3;
            }
            vertices = f;

            type = SColliderType.Convex;
        }
    }

    public abstract class SCollider
    {
        SVector3 pos;
        SQuaternion rot;
        SVector3 scale;
        internal SColliderType type;
        public SVector3 Position
        {
            get => pos;
            set
            {
                pos = value;
                if (on != null) if (PhysicsEngine.running) PhysicsEngine.updateObject(on);
            }
        }
        public SQuaternion Rotation
        {
            get => rot;
            set
            {
                rot = value;
                if (on != null) if (PhysicsEngine.running) PhysicsEngine.updateObject(on);
            }
        }
        public SVector3 Scale
        {
            get => scale;
            set
            {
                scale = value;
                if (on != null) if (PhysicsEngine.running) PhysicsEngine.updateObject(on);
            }
        }
        internal GameObject on;

        public SCollider()
        {
            pos = new SVector3();
            rot = new SQuaternion();
            scale = new SVector3(1f, 1f, 1f);
        }
        public SCollider(SVector3 pos)
        {
            this.pos = pos;
            rot = new SQuaternion();
            scale = new SVector3(1f, 1f, 1f);
        }

        public SCollider(SVector3 pos, SVector3 scale)
        {
            this.pos = pos;
            rot = new SQuaternion();
            this.scale = scale;
        }

        public SCollider(SVector3 pos, SQuaternion rot)
        {
            this.pos = pos;
            this.rot = rot;
            scale = new SVector3(1f, 1f, 1f);
        }

        public SCollider(SVector3 pos, SVector3 scale, SQuaternion rot)
        {
            this.pos = pos;
            this.rot = rot;
            this.scale = scale;
        }
    }
}
