using BulletSharp;
using Realsphere.Spirit.BulletPhysics;
using Realsphere.Spirit.SceneManagement;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using Vector3 = System.Numerics.Vector3;
using DXVector3 = SharpDX.Vector3;
using Realsphere.Spirit.RenderingCommon;
using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;
using SharpDX.Direct3D;
using SharpDX.Toolkit.Graphics;
using System.Text.Json;
using System.Printing;
using Realsphere.Spirit.Mathematics;
using System.Windows;

namespace Realsphere.Spirit.Internal
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PositionColored
    {
        public static readonly int Stride = DXVector3.SizeInBytes + sizeof(int);

        public DXVector3 Position;
        public int Color;

        public PositionColored(Vector3 pos, int col)
        {
            Position = new(pos.X, pos.Y, pos.Z);
            Color = col;
        }

        public PositionColored(ref Vector3 pos, int col)
        {
            Position = new(pos.X, pos.Y, pos.Z);
            Color = col;
        }

        public PositionColored(DXVector3 pos, int col)
        {
            Position = new(pos.X, pos.Y, pos.Z);
            Color = col;
        }

        public PositionColored(ref DXVector3 pos, int col)
        {
            Position = new(pos.X, pos.Y, pos.Z);
            Color = col;
        }
    }

    public abstract class BufferedDebugDraw : DebugDraw, IDisposable
    {
        protected List<PositionColored> lines = new List<PositionColored>();

        public override DebugDrawModes DebugMode { get; set; }

        public int ToRgba(ref Vector3 c)
        {
            uint a = 255;
            uint r = (uint)(c.X * 255.0f) & 255;
            uint g = (uint)(c.Y * 255.0f) & 255;
            uint b = (uint)(c.Z * 255.0f) & 255;

            uint value = r;
            value |= g << 8;
            value |= b << 16;
            value |= a << 24;

            return (int)value;
        }

        protected virtual int ColorToInt(ref Vector3 c)
        {
            var ci = ToRgba(ref c);

            return (ci & 0xff0000) >> 16 | (ci & 0xff00) | (ci & 0xff) << 16;
        }

        public override void Draw3DText(ref Vector3 location, string textString)
        {
            throw new NotImplementedException();
        }

        public override void DrawLine(ref Vector3 from, ref Vector3 to, ref Vector3 fromColor, ref Vector3 toColor)
        {
            lines.Add(new PositionColored(ref from, ColorToInt(ref fromColor)));
            lines.Add(new PositionColored(ref to, ColorToInt(ref toColor)));
        }

        public override void DrawLine(ref Vector3 from, ref Vector3 to, ref Vector3 color)
        {
            int intColor = ColorToInt(ref color);
            lines.Add(new PositionColored(ref from, intColor));
            lines.Add(new PositionColored(ref to, intColor));
        }

        public override void DrawBox(ref Vector3 bbMin, ref Vector3 bbMax, ref Vector3 color)
        {
            var p1 = bbMin;
            var p2 = new Vector3(bbMax.X, bbMin.Y, bbMin.Z);
            var p3 = new Vector3(bbMax.X, bbMax.Y, bbMin.Z);
            var p4 = new Vector3(bbMin.X, bbMax.Y, bbMin.Z);
            var p5 = new Vector3(bbMin.X, bbMin.Y, bbMax.Z);
            var p6 = new Vector3(bbMax.X, bbMin.Y, bbMax.Z);
            var p7 = bbMax;
            var p8 = new Vector3(bbMin.X, bbMax.Y, bbMax.Z);

            int intColor = ColorToInt(ref color);
            lines.Add(new PositionColored(ref p1, intColor)); lines.Add(new PositionColored(ref p2, intColor));
            lines.Add(new PositionColored(ref p2, intColor)); lines.Add(new PositionColored(ref p3, intColor));
            lines.Add(new PositionColored(ref p3, intColor)); lines.Add(new PositionColored(ref p4, intColor));
            lines.Add(new PositionColored(ref p4, intColor)); lines.Add(new PositionColored(ref p1, intColor));

            lines.Add(new PositionColored(ref p1, intColor)); lines.Add(new PositionColored(ref p5, intColor));
            lines.Add(new PositionColored(ref p2, intColor)); lines.Add(new PositionColored(ref p6, intColor));
            lines.Add(new PositionColored(ref p3, intColor)); lines.Add(new PositionColored(ref p7, intColor));
            lines.Add(new PositionColored(ref p4, intColor)); lines.Add(new PositionColored(ref p8, intColor));

            lines.Add(new PositionColored(ref p5, intColor)); lines.Add(new PositionColored(ref p6, intColor));
            lines.Add(new PositionColored(ref p6, intColor)); lines.Add(new PositionColored(ref p7, intColor));
            lines.Add(new PositionColored(ref p7, intColor)); lines.Add(new PositionColored(ref p8, intColor));
            lines.Add(new PositionColored(ref p8, intColor)); lines.Add(new PositionColored(ref p5, intColor));
        }

        public override void DrawBox(ref Vector3 bbMin, ref Vector3 bbMax, ref Matrix4x4 trans, ref Vector3 color)
        {
            var p1 = DXVector3.TransformCoordinate(new(bbMin.X, bbMin.Y, bbMin.Z), DXConversionHelper.SNToDX(trans));
            var p2 = DXVector3.TransformCoordinate(new DXVector3(bbMax.X, bbMin.Y, bbMin.Z), DXConversionHelper.SNToDX(trans));
            var p3 = DXVector3.TransformCoordinate(new DXVector3(bbMax.X, bbMax.Y, bbMin.Z), DXConversionHelper.SNToDX(trans));
            var p4 = DXVector3.TransformCoordinate(new DXVector3(bbMin.X, bbMax.Y, bbMin.Z), DXConversionHelper.SNToDX(trans));
            var p5 = DXVector3.TransformCoordinate(new DXVector3(bbMin.X, bbMin.Y, bbMax.Z), DXConversionHelper.SNToDX(trans));
            var p6 = DXVector3.TransformCoordinate(new DXVector3(bbMax.X, bbMin.Y, bbMax.Z), DXConversionHelper.SNToDX(trans));
            var p7 = DXVector3.TransformCoordinate(new DXVector3(bbMax.X, bbMin.Y, bbMax.Z), DXConversionHelper.SNToDX(trans));
            var p8 = DXVector3.TransformCoordinate(new DXVector3(bbMin.X, bbMax.Y, bbMax.Z), DXConversionHelper.SNToDX(trans));

            int intColor = ColorToInt(ref color);
            lines.Add(new PositionColored(ref p1, intColor)); lines.Add(new PositionColored(ref p2, intColor));
            lines.Add(new PositionColored(ref p2, intColor)); lines.Add(new PositionColored(ref p3, intColor));
            lines.Add(new PositionColored(ref p3, intColor)); lines.Add(new PositionColored(ref p4, intColor));
            lines.Add(new PositionColored(ref p4, intColor)); lines.Add(new PositionColored(ref p1, intColor));

            lines.Add(new PositionColored(ref p1, intColor)); lines.Add(new PositionColored(ref p5, intColor));
            lines.Add(new PositionColored(ref p2, intColor)); lines.Add(new PositionColored(ref p6, intColor));
            lines.Add(new PositionColored(ref p3, intColor)); lines.Add(new PositionColored(ref p7, intColor));
            lines.Add(new PositionColored(ref p4, intColor)); lines.Add(new PositionColored(ref p8, intColor));

            lines.Add(new PositionColored(ref p5, intColor)); lines.Add(new PositionColored(ref p6, intColor));
            lines.Add(new PositionColored(ref p6, intColor)); lines.Add(new PositionColored(ref p7, intColor));
            lines.Add(new PositionColored(ref p7, intColor)); lines.Add(new PositionColored(ref p8, intColor));
            lines.Add(new PositionColored(ref p8, intColor)); lines.Add(new PositionColored(ref p5, intColor));
        }

        public override void DrawTriangle(ref Vector3 v0, ref Vector3 v1, ref Vector3 v2, ref Vector3 color, float __unnamed004)
        {
            int intColor = ColorToInt(ref color);
            lines.Add(new PositionColored(ref v0, intColor));
            lines.Add(new PositionColored(ref v1, intColor));
            lines.Add(new PositionColored(ref v2, intColor));
            lines.Add(new PositionColored(ref v0, intColor));
        }

        public override void DrawTransform(ref Matrix4x4 transform, float orthoLen)
        {
            Vector3 start = transform.Translation;

            lines.Add(new PositionColored(ref start, 0xff0000));
            lines.Add(new PositionColored(start + new Vector3(orthoLen, 0, 0), 0xff0000));
            lines.Add(new PositionColored(ref start, 0x00ff00));
            lines.Add(new PositionColored(start + new Vector3(0, orthoLen, 0), 0x00ff00));
            lines.Add(new PositionColored(ref start, 0x0000ff));
            lines.Add(new PositionColored(start + new Vector3(0, 0, orthoLen), 0x0000ff));
        }

        public override void DrawArc(ref Vector3 center, ref Vector3 normal, ref Vector3 axis, float radiusA, float radiusB, float minAngle, float maxAngle, ref Vector3 color, bool drawSect, float stepDegrees)
        {
            Vector3 vx = axis;
            Vector3 vy = Vector3.Cross(normal, axis);
            float step = stepDegrees * ((float)Math.PI / 180.0f);
            int nSteps = (int)((maxAngle - minAngle) / step);
            if (nSteps == 0)
                nSteps = 1;

            Vector3 next = center + radiusA * vx * (float)Math.Cos(minAngle) + radiusB * vy * (float)Math.Sin(minAngle);

            if (drawSect)
                DrawLine(ref center, ref next, ref color);

            int intColor = ColorToInt(ref color);
            PositionColored last = new PositionColored(ref next, intColor);
            for (int i = 1; i <= nSteps; i++)
            {
                lines.Add(last);
                float angle = minAngle + (maxAngle - minAngle) * i / nSteps;
                next = center + radiusA * vx * (float)Math.Cos(angle) + radiusB * vy * (float)Math.Sin(angle);
                last = new PositionColored(ref next, intColor);
                lines.Add(last);
            }

            if (drawSect)
                DrawLine(ref center, ref next, ref color);
        }

        public override void DrawContactPoint(ref Vector3 pointOnB, ref Vector3 normalOnB, float distance, int lifeTime, ref Vector3 color)
        {
            int intColor = ColorToInt(ref color);
            Vector3 to = pointOnB + normalOnB * 1; // distance
            lines.Add(new PositionColored(ref pointOnB, intColor));
            lines.Add(new PositionColored(ref to, intColor));
        }

        public override void ReportErrorWarning(string warningString)
        {
            System.Windows.Forms.MessageBox.Show(warningString);
        }

        ~BufferedDebugDraw()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {

        }
    }

    public class PhysicsDebugDraw : BufferedDebugDraw
    {
        struct line
        {
            public DXVector3 start;
            public DXVector3 end;
            public Color color;
        }

        List<line> lines = new();

        public PhysicsDebugDraw()
        {
        }

        public override void DrawLine(ref Vector3 from, ref Vector3 to, ref Vector3 color)
        {
            lines.Add(new()
            {
                color = new Color(color.X, color.Y, color.Z),
                start = new(from.X, from.Y, from.Z),
                end = new(to.X, to.Y, to.Z)
            });
        }

        public override void DrawLine(ref Vector3 from, ref Vector3 to, ref Vector3 fromColor, ref Vector3 toColor)
        {
            lines.Add(new()
            {
                color = new Color(fromColor.X, fromColor.Y, fromColor.Z),
                start = new(from.X, from.Y, from.Z),
                end = new(to.X, to.Y, to.Z)
            });
        }

        public void DrawDebugWorld(DynamicsWorld world)
        {
            world.DebugDrawWorld();

            if (lines.Count == 0)
                return;

            foreach (line line in lines)
            {
                DrawLine1(Game.deviceManager.Direct3DDevice, line.start, line.end, line.color);
            }
            lines.Clear();
        }

        void DrawLine1(Device device, DXVector3 start, DXVector3 end, Color color)
        {
            var vertices = new[]
            {
                new VertexPositionColor(start, color),
                new VertexPositionColor(end, color)
            };

            // Create a new VertexBuffer containing the line vertices
            var vertexBuffer = Buffer.Create(device,
                BindFlags.VertexBuffer,
                vertices);

            // Draw the line using a basic vertex shader
            var deviceContext = device.ImmediateContext;
            var primTop = deviceContext.InputAssembler.PrimitiveTopology;
            deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.LineList;
            deviceContext.InputAssembler.SetVertexBuffers(0, new SharpDX.Direct3D11.VertexBufferBinding(vertexBuffer, Utilities.SizeOf<VertexPositionColor>(), 0));
            deviceContext.Draw(vertices.Length, 0);
            deviceContext.InputAssembler.PrimitiveTopology = primTop;
        }
    }
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
            if ((float)simTime.Elapsed.TotalSeconds < time)
            {
                time = 0;
                timeStep = 0;
            }
            timeStep = (float)simTime.Elapsed.TotalSeconds - time;
            time = (float)simTime.Elapsed.TotalSeconds;
            stepping = true;
            if (!pause) world.StepSimulation(timeStep, SMath.Min(Environment.ProcessorCount / 2, 1));
            stepping = false;
        }

        internal static CollisionShape GetObjShape(GameObject go)
        {
            List<Vector3> verts = new();

            foreach (var buf in go.renderer.Mesh.VertexBuffers)
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

        internal static void updateObject(GameObject go)
        {
        }

        internal static void addToScene(GameObject go)
        {
            if (world == null) return;
            pause = true;
            while(stepping) { }
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
                updateObject(go);
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