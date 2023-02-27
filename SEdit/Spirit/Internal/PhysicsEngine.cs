using BulletSharp;
using Realsphere.Spirit.BulletPhysics;
using Realsphere.Spirit.Physics;
using Realsphere.Spirit.SceneManagement;
using SharpDX;
using System.Diagnostics;

namespace Realsphere.Spirit.Internal
{
    internal class SMotionState : MotionState
    {
        internal GameObject go;
        internal SMotionState(GameObject go)
        {
            this.go = go;
        }

        Matrix MSWorldTransform
        {
            get => go.WorldTransform;
            set
            {
                go.Transform.Position = value.TranslationVector;
                go.Transform.Scale = value.ScaleVector;
                go.WorldTransform = value;
            }
        }

        public override void GetWorldTransform(out System.Numerics.Matrix4x4 worldTrans)
        {
            worldTrans = new System.Numerics.Matrix4x4(
                MSWorldTransform.M11,
                MSWorldTransform.M12,
                MSWorldTransform.M13,
                MSWorldTransform.M14,
                MSWorldTransform.M21,
                MSWorldTransform.M22,
                MSWorldTransform.M23,
                MSWorldTransform.M24,
                MSWorldTransform.M31,
                MSWorldTransform.M32,
                MSWorldTransform.M33,
                MSWorldTransform.M34,
                MSWorldTransform.M41,
                MSWorldTransform.M42,
                MSWorldTransform.M43,
                MSWorldTransform.M43);
        }

        public override void SetWorldTransform(ref System.Numerics.Matrix4x4 worldTrans)
        {
            MSWorldTransform = DXConversionHelper.SNToDX(worldTrans);
        }
    }

    internal class SFrozenMotionState : MotionState
    {
        internal GameObject go;
        internal SFrozenMotionState(GameObject go)
        {
            this.go = go;
        }

        Matrix MSWorldTransform
        {
            get => go.WorldTransform;
            set
            {
                go.Transform.Position = value.TranslationVector;
                go.Transform.Scale = value.ScaleVector;
                go.WorldTransform = value;
            }
        }

        public override void GetWorldTransform(out System.Numerics.Matrix4x4 worldTrans)
        {
            worldTrans = new System.Numerics.Matrix4x4(
                MSWorldTransform.M11,
                MSWorldTransform.M12,
                MSWorldTransform.M13,
                MSWorldTransform.M14,
                MSWorldTransform.M21,
                MSWorldTransform.M22,
                MSWorldTransform.M23,
                MSWorldTransform.M24,
                MSWorldTransform.M31,
                MSWorldTransform.M32,
                MSWorldTransform.M33,
                MSWorldTransform.M34,
                MSWorldTransform.M41,
                MSWorldTransform.M42,
                MSWorldTransform.M43,
                MSWorldTransform.M43);
        }

        public override void SetWorldTransform(ref System.Numerics.Matrix4x4 worldTrans)
        {
            MSWorldTransform = DXConversionHelper.SNToDX(worldTrans);
        }
    }

    internal static class PhysicsEngine
    {
        internal static DynamicsWorld world;
        static CollisionConfiguration conf = new DefaultCollisionConfiguration();
        static ConstraintSolver solver = new SequentialImpulseConstraintSolver();
        static Dispatcher dispatcher = new CollisionDispatcher(conf);
        static BroadphaseInterface broadphase = new DbvtBroadphase();
        static Stopwatch simTime = new Stopwatch();
        static float time, timeStep;
        internal static bool pause;

        internal static DynamicsWorld DW
        {
            get
            {
                return world;
            }
        }

        internal static bool running
        {
            get
            {
                return world != null;
            }
        }

        internal static void init()
        {
            simTime.Start();
            time = 0f;
            timeStep = 0f;
            world = new DiscreteDynamicsWorld(dispatcher, broadphase, solver, conf);
            world.Gravity = new System.Numerics.Vector3(0f, -9.81f, 0f);
        }

        internal static System.Numerics.Vector3 raycast(System.Numerics.Vector3 pos, System.Numerics.Vector3 dir, float distance)
        {
            System.Numerics.Vector3 to = pos + dir * distance;

            ClosestRayResultCallback closestResults = new(ref pos, ref to);

            world.RayTest(pos, to, closestResults);

            if (closestResults.HasHit)
                return closestResults.HitNormalWorld;
            else
                return new();
        }

        internal static void step()
        {
            if (pause) return;
            if ((float)simTime.Elapsed.TotalSeconds < time)
            {
                time = 0;
                timeStep = 0;
            }
            timeStep = (float)simTime.Elapsed.TotalSeconds - time;
            time = (float)simTime.Elapsed.TotalSeconds;
            world.StepSimulation(timeStep);
        }

        internal static CollisionShape GetObjShape(GameObject go)
        {
            CompoundShape cs = new();

            foreach (SCollider collider in go.colliders)
            {
                switch (collider.type)
                {
                    case SColliderType.Box:
                        Matrix boxMatrix =
                            Matrix.Translation(collider.Position.sharpDXVector) *
                            Matrix.RotationX(collider.Rotation.X) *
                            Matrix.RotationY(collider.Rotation.Y) *
                            Matrix.RotationZ(collider.Rotation.Z) *
                            Matrix.Scaling(1f);

                        cs.AddChildShape(DXConversionHelper.DXToSN(boxMatrix), new BoxShape(collider.Scale));
                        break;
                    case SColliderType.Sphere:
                        Matrix sphereMatrix =
                            Matrix.Translation(collider.Position.sharpDXVector) * Matrix.Scaling(1f);

                        cs.AddChildShape(DXConversionHelper.DXToSN(sphereMatrix), new SphereShape(((SSphereCollider)collider).Radius));
                        break;
                    case SColliderType.Capsule:
                        Matrix capsuleMatrix =
                            Matrix.Translation(collider.Position.sharpDXVector) * Matrix.Scaling(1f) *
                            Matrix.RotationX(collider.Rotation.X) *
                            Matrix.RotationY(collider.Rotation.Y) *
                            Matrix.RotationZ(collider.Rotation.Z);

                        cs.AddChildShape(DXConversionHelper.DXToSN(capsuleMatrix), new CapsuleShape(((SCapsuleCollider)collider).Radius, ((SCapsuleCollider)collider).Height));
                        break;
                    case SColliderType.Convex:
                        Matrix convexMatrix =
                            Matrix.Translation(collider.Position.sharpDXVector) * Matrix.Scaling(1f) *
                            Matrix.RotationX(collider.Rotation.X) *
                            Matrix.RotationY(collider.Rotation.Y) *
                            Matrix.RotationZ(collider.Rotation.Z);

                        cs.AddChildShape(DXConversionHelper.DXToSN(convexMatrix), new ConvexHullShape(((SConvexCollider)collider).vertices));
                        break;
                    default:
                        throw new System.TypeLoadException("Unknown type of collider on " + go.Name + "!");
                }
            }

            return cs;
        }

        internal static void updateObject(GameObject go)
        {
            if (running && go.body != null)
            {
                go.body.Gravity = new System.Numerics.Vector3(0f, go.HasGravity ? -9.81f : 0f, 0f);
                go.body.ApplyGravity();
                go.body.CollisionShape = GetObjShape(go);
            }
        }

        internal static void addToScene(GameObject go)
        {
            if (world == null) return;
            CollisionShape shape = GetObjShape(go);

            go.renderer.World = Matrix.Identity;
            RigidBody body = new RigidBody(
                                    new RigidBodyConstructionInfo(
                                        go.Weight,
                                        new SMotionState(go),
                                        shape, shape.CalculateLocalInertia(go.Weight)));

            if (body.IsStaticObject)
            {
                body.Restitution = go.Restitution;
                body.Friction = go.Friction;
            }

            go.body = body;
            world.AddRigidBody(body);
            body.Gravity = new System.Numerics.Vector3(0f, go.HasGravity ? -9.81f : 0f, 0f);
        }

        internal static void setScene(Scene scene)
        {
            pause = true;
            world.Broadphase.Dispose();
            world.Broadphase = new DbvtBroadphase();
            world.CollisionObjectArray.Clear();

            foreach (GameObject go in scene.GameObjects)
            {
                addToScene(go);
                updateObject(go);
            }
            pause = false;
        }

        internal static void drop()
        {
            world.Dispose();
            conf.Dispose();
            broadphase.Dispose();
            solver.Dispose();
            dispatcher.Dispose();
            simTime.Stop();
        }
    }
}
