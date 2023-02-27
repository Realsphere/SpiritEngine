using SharpDX;
using System;
using System.Collections.Generic;
using Realsphere.Spirit.Mathematics;
using Realsphere.Spirit.Modelling;
using Realsphere.Spirit.Physics;
using Realsphere.Spirit.Rendering;
using Realsphere.Spirit.Internal;
using BulletSharp;

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
        internal RigidBody rig;
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

        public SVector3 Force
        {
            set
            {
                if (rig != null)
                {
                    rig.LinearVelocity = value;
                }
            }
            get => rig != null ? rig.LinearVelocity : new Vector3();
        }
        public float Weight
        {
            get => weight;
            set
            {
                weight = value;
                if (rig != null) rig.SetMassProps(weight, rig.CollisionShape.CalculateLocalInertia(weight));
            }
        }
        internal Matrix PWorldTransform
        {
            get
            {
                WorldTransform.Decompose(out _, out Quaternion rot, out _);
                return Matrix.Translation(WorldTransform.TranslationVector) * Matrix.Scaling(1f) * Matrix.RotationQuaternion(rot);
            }
        }
        public float Friction
        {
            get => frict;
            set
            {
                frict = value;
            }
        }
        public float Restitution
        {
            get => restit;
            set
            {
                restit = value;
            }
        }

        public void InitPhysics()
        {
            PhysicsEngine.addToScene(this);
        }

        public void ApplyForce(SVector3 force)
        {
            rig.ApplyCentralForce(force);
        }

        public void ApplyTorque(SVector3 torque)
        {
            rig.ApplyTorque(torque);
        }

        public void ApplyImpulse(SVector3 force)
        {
            rig.ApplyCentralImpulse(force);
        }

        public void ApplyTorqueImpulse(SVector3 torque)
        {
            rig.ApplyTorqueImpulse(torque);
        }

        public IntPtr BulletPhysicsRigidBodyNative
        {
            get
            {
                return rig.Native;
            }
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
            rig.CollisionShape.Dispose();
            rig.ForceActivationState(ActivationState.DisableSimulation);
            rig.Dispose();
            rig = null;
            Material = null;
            renderer.Dispose();
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