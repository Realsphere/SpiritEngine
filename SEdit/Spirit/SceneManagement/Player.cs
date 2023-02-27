using BulletSharp;
using Realsphere.Spirit.BulletPhysics;
using Realsphere.Spirit.Internal;
using Realsphere.Spirit.Mathematics;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace Realsphere.Spirit.SceneManagement
{
    internal class PlayerMotionState : MotionState
    {
        public PlayerMotionState() { }

        public override void GetWorldTransform(out System.Numerics.Matrix4x4 worldTrans)
        {
            worldTrans = DXConversionHelper.DXToSN(
                Matrix.Translation(Game.Player.PlayerPosition.sharpDXVector) *
                Matrix.Scaling(1f)
                );
        }

        public override void SetWorldTransform(ref System.Numerics.Matrix4x4 worldTrans)
        {
            Game.Player.PlayerPosition.Y = worldTrans.Translation.Y;
        }
    }

    public class Player
    {
        float cos(float a) => MathF.Cos(a);
        float sin(float a) => MathF.Sin(a);
        SVector3 playerpos = new SVector3();
        public SVector3 PlayerPosition
        {
            get
            {
                return playerpos;
            }
            set
            {
                playerpos = value;
                AudioMaster.setListenerData();
                if (rig != null) rig.WorldTransform = DXConversionHelper.DXToSN(DXConversionHelper.SNToDX(rig.WorldTransform) * Matrix.Translation(PlayerPosition.sharpDXVector));
            }
        }
        public SVector3 CameraForward
        {
            get
            {
                if (Game.app != null) return Game.app.cameraTarget;
                else return new();
            }
        }
        public SVector3 CameraUp
        {
            get
            {
                if (Game.app != null) return Game.app.viewProjection.Up;
                else return new();
            }
        }
        public float RotationX
        {
            get
            {
                if (Game.app != null) return Game.app.rotationY;
                else return new();
            }
            set
            {
                if (Game.app != null) Game.app.rotationY = value;
            }
        }
        public float RotationY
        {
            get
            {
                if (Game.app != null) return Game.app.rotationX;
                else return new();
            }
            set
            {
                if (Game.app != null) Game.app.rotationX = value;
            }
        }
        public SVector3 CameraRight
        {
            get
            {
                if (Game.app != null) return new(Game.app.viewProjection.Right.X, 0f, Game.app.viewProjection.Right.Z);
                else return new();
            }
        }
        public float PlayerHeight = 2f;
        public float PlayerRadius = 1f;
        public float PlayerWeight = 2.5f;
        public float Speed = 1f;
        float camf = 100f, camn = 0.001f;
        public float CameraFar
        {
            get
            {
                return camf;
            }
            set
            {
                camf = value;
                if (Game.app == null) return;
                Game.app.projectionMatrix = Matrix.PerspectiveFovRH((float)Math.PI / 3f, Game.app.Width / (float)Game.app.Height, Game.Player.CameraNear, Game.Player.CameraFar);
            }
        }
        public float CameraNear
        {
            get
            {
                return camn;
            }
            set
            {
                camn = value;
                if (Game.app == null) return;
                Game.app.projectionMatrix = Matrix.PerspectiveFovRH((float)Math.PI / 3f, Game.app.Width / (float)Game.app.Height, Game.Player.CameraNear, Game.Player.CameraFar);
            }
        }

        internal RigidBody rig;

        /// <summary>
        /// Will give Physics to the player, if you want a floating camera dont call this.
        /// </summary>
        public void EnablePhysics()
        {
            PhysicsEngine.pause = true;
            if (PhysicsEngine.world == null) return;
            CollisionShape shape = new CylinderShape(PlayerRadius / 2f, PlayerHeight / 2f, PlayerRadius / 2f);

            rig = new RigidBody(
                                    new RigidBodyConstructionInfo(
                                        PlayerWeight,
                                        new PlayerMotionState(),
                                        shape, shape.CalculateLocalInertia(PlayerWeight)));

            PhysicsEngine.world.AddRigidBody(rig);
            rig.Gravity = new System.Numerics.Vector3(0f, -9.81f, 0f);
            PhysicsEngine.pause = false;
        }

        /// <summary>
        /// Will remove Physics from the player.
        /// </summary>
        public void DisablePhysics()
        {
            if (PhysicsEngine.world == null) return;
            PhysicsEngine.pause = true;
            PhysicsEngine.world.RemoveRigidBody(rig);
            rig.Dispose();
            PhysicsEngine.pause = false;
        }
    }
}
