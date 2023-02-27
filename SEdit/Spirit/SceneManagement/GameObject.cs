using Realsphere.Spirit.RenderingCommon;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Interop;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using BulletSharp;
using Newtonsoft.Json.Linq;
using System.Threading;
using Realsphere.Spirit.Mathematics;
using Realsphere.Spirit.Modelling;
using Realsphere.Spirit.Physics;
using Realsphere.Spirit.Rendering;
using Realsphere.Spirit.Internal;

namespace Realsphere.Spirit.SceneManagement
{
    public class GameObject : IDisposable
    {
        public string Name { get; private set; }
        SMaterial mat;
        public SMaterial Material
        {
            set
            {
                mat = value;
                if (sm != null && sm.ssuvs != null && sm.ssuvs.Length > 0)
                {
                    mat.UVs = new SVector2[sm.ssuvs.Length];
                    for (int i = 0; i < sm.ssuvs.Length; i++)
                    {
                        mat.UVs[i] = sm.ssuvs[i];
                    }
                }
            }
            get
            {
                return mat;
            }
        }
        internal MeshRenderer renderer;
        internal RigidBody body;
        public SModel Model
        {
            get
            {
                return sm;
            }
            set
            {
                sm = value;
            }
        }
        public STransform Transform { get; }
        internal Matrix WorldTransform;
        float weight = 0f;
        float frict = 0.4f;
        float restit = 1f;
        bool hgravity = true;
        internal bool setgrav;
        public bool HasGravity
        {
            get => hgravity;
            set
            {
                hgravity = value;
                setgrav = value;
                if (PhysicsEngine.running) PhysicsEngine.updateObject(this);
            }
        }
        internal SModel sm;
        internal List<SCollider> colliders = new List<SCollider>();
        public void AddCollider(SCollider collider)
        {
            collider.on = this;
            colliders.Add(collider);
            if (PhysicsEngine.running) PhysicsEngine.updateObject(this);
        }
        public void RemoveCollider(SCollider collider)
        {
            colliders.Remove(collider);
            if (PhysicsEngine.running) PhysicsEngine.updateObject(this);
        }

        public SCollider[] GetAllColliders()
        {
            return colliders.ToArray();
        }

        public SVector3 Force
        {
            set
            {
                if (body != null)
                {
                    body.LinearVelocity = value;
                }
            }
            get => body != null ? body.LinearVelocity : new Vector3();
        }
        public float Weight
        {
            get => weight;
            set
            {
                weight = value;
                if (body != null) body.SetMassProps(weight, body.CollisionShape.CalculateLocalInertia(weight));
            }
        }
        public float Friction
        {
            get => frict;
            set
            {
                frict = value;
                if (body != null) body.Friction = frict;
            }
        }
        public float Restitution
        {
            get => restit;
            set
            {
                restit = value;
                if (body != null) body.Restitution = restit;
            }
        }

        public void InitPhysics()
        {
            PhysicsEngine.addToScene(this);
        }

        public static GameObject CreateUsingMesh(SModel model, string name)
        {
            GameObject go = new GameObject(name);
            go.Transform.on = go;
            var mesh = new MeshRenderer(model.mesh);
            mesh.Initialize(Game.app);
            mesh.World = Matrix.Identity;
            go.renderer = mesh;
            go.renderer.objectOn = go;
            go.sm = model;
            return go;
        }

        public void Dispose()
        {
            PhysicsEngine.DW.RemoveRigidBody(body);
            body.MotionState.Dispose();
            body.CollisionShape.Dispose();
            body.Dispose();
            body = null;
            Material = null;
            renderer.Dispose();
            for (int i = 0; i < colliders.Count; i++)
                colliders.RemoveAt(i);
        }

        public GameObject(string name)
        {
            Name = name;
            Transform = new STransform();
            Transform.on = this;
            Material = new SMaterial()
            {
                SpecularPower = 20f,
                Specular = SColor.fromsharpdx(Color4.White),
                Emissive = new SColor(0f, 0f, 0f, 0f),
                Diffuse = SColor.fromsharpdx(Color.White),
                Ambient = SColor.fromsharpdx(new Color4(0.2f))
            };
        }
    }
}
