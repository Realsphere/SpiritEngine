using BulletSharp;
using Realsphere.Spirit.BulletPhysics;
using Realsphere.Spirit.SceneManagement;
using SharpDX;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Vector3 = System.Numerics.Vector3;
using DXVector3 = SharpDX.Vector3;

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
            get => go.PWorldTransform;
            set
            {
                value.Decompose(out DXVector3 scale, out SharpDX.Quaternion rot, out DXVector3 trans);
                go.Transform.Position = trans;
                go.Transform.Scale = scale;
                go.Transform.Rotation = new(rot.X, rot.Y, rot.Z, rot.W);
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
        internal static float time, timeStep;
        internal static bool pause, stepping;
    
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
    
        internal static GameObject raycast(System.Numerics.Vector3 pos, System.Numerics.Vector3 dir, float distance)
        {
            System.Numerics.Vector3 to = pos + dir * distance;
    
            ClosestRayResultCallback closestResults = new(ref pos, ref to);
    
            world.RayTest(pos, to, closestResults);
    
            if (closestResults.HasHit)
                if (Game.ActiveScene.GameObjects.Where(x => x.rig.Native == closestResults.CollisionObject.Native).Any())
                    return Game.ActiveScene.GameObjects.Where(x => x.rig.Native == closestResults.CollisionObject.Native).First();
            return null;
        }
    
        internal static void step()
        {
            if (pause) return;
            if (Game.ActiveScene == null) return;
            if (Game.ActiveScene.GameObjects == null) return;
            if (Game.ActiveScene.GameObjects.Count == 0) return;
            if ((float)simTime.Elapsed.TotalSeconds < time)
            {
                time = 0;
                timeStep = 0;
            }
            timeStep = (float)simTime.Elapsed.TotalSeconds - time;
            time = (float)simTime.Elapsed.TotalSeconds;
            stepping = true;
            if (!pause) world.StepSimulation(timeStep, Game.ActiveScene.SubStepCount);
            stepping = false;
        }
    
        internal static CollisionShape GetObjShape(GameObject go)
        {
            List<Vector3> verts = new();
    
            foreach (var renderer in go.renderers)
                foreach (var buf in renderer.Mesh.VertexBuffers)
                    verts.AddRange(buf.Select(x => new Vector3(x.Position.X, x.Position.Y, x.Position.Z)));
    
            for (int i = 0; i < verts.Count; i++)
            {
                if (i == 0) continue;
    
                verts[i] *= (Vector3)go.Transform.Scale;
            }
    
            var shape = new ConvexHullShape(verts);
            shape.Margin = 0f;
            return shape;
        }
    
        internal static void addToScene(GameObject go)
        {
            if (world == null) return;
            pause = true;
            while(stepping) { }
            CollisionShape shape = GetObjShape(go);
    
            foreach(var renderer in go.renderers) renderer.World = Matrix.Identity;
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
    
            go.rig = body;
            world.AddRigidBody(body);
            body.Gravity = new System.Numerics.Vector3(0f, go.HasGravity ? -9.81f : 0f, 0f);
            pause = false;
        }
    
        internal static void setScene(Scene scene)
        {
            while (stepping) { }
            pause = true;
            foreach (GameObject go in scene.GameObjects)
            {
                addToScene(go);
            }
            pause = false;
        }
    
        internal static void drop()
        {
            while (stepping) { }
            world.Dispose();
            conf.Dispose();
            broadphase.Dispose();
            solver.Dispose();
            dispatcher.Dispose();
            simTime.Stop();
        }
    }
}