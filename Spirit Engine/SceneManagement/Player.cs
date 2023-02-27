using BulletSharp;
using Realsphere.Spirit.Internal;
using Realsphere.Spirit.Mathematics;
using SharpDX;
using System;
using System.Numerics;
using System.Threading;
using static Realsphere.Spirit.Game;

namespace Realsphere.Spirit.SceneManagement
{
    internal class VariableMotionState : MotionState
    {
        internal Matrix4x4 matrix;

        public override void GetWorldTransform(out Matrix4x4 worldTrans)
        {
            worldTrans = matrix;
        }

        public override void SetWorldTransform(ref Matrix4x4 worldTrans)
        {
            matrix = worldTrans;
        }
    }

    public class Player
    {
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
                AudioMaster.setListenerData();
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
                AudioMaster.setListenerData();
            }
        }
        public SVector3 CameraRotation
        {
            get => new(RotationX, RotationY, 0f);
        }
        public SVector3 CameraRight
        {
            get
            {
                return new SVector3(Game.app.viewProjection.M11, 0f, Game.app.viewProjection.M31);
            }
        }
        public float PlayerHeight = 1f;
        public float PlayerRadius = 0.5f;
        public float PlayerWeight = 2.5f;
        public float Speed = 5f;
        float camf = 100f, camn = 0.001f;
        public float JumpVelocity = 0.5f;
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

        /// <summary>
        /// Will give Physics to the player, if you want a floating camera don't call this.
        /// </summary>
        public void EnablePhysics()
        {
        }

        /// <summary>
        /// Will remove Physics from the player.
        /// </summary>
        public void DisablePhysics()
        {
        }
    }
}
