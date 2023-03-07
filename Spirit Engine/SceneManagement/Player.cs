using BulletSharp;
using Realsphere.Spirit.Internal;
using Realsphere.Spirit.Mathematics;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.Toolkit.Graphics;
using System;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using static Realsphere.Spirit.Game;

namespace Realsphere.Spirit.SceneManagement
{
    public class Player
    {
        internal RigidBody rigidBody;
        SVector3 playerpos = new SVector3();
        public bool Grounded
        {
            get
            {
                if (rigidBody == null) return false;

                var rbPosition = rigidBody.CenterOfMassPosition;

                var rayStart = rbPosition;

                var rayEnd = rbPosition - new System.Numerics.Vector3(0, PlayerHeight + 0.01f, 0);

                using (ClosestRayResultCallback rayCallback = new ClosestRayResultCallback(ref rayStart, ref rayEnd))
                {
                    PhysicsEngine.world.RayTest(rayStart, rayEnd, rayCallback);

                    return rayCallback.HasHit && rayCallback.CollisionObject != rigidBody;
                }
            }
        }
        public SVector3 PlayerPosition
        {
            get
            {
                return rigidBody == null ? playerpos : rigidBody.WorldTransform.Translation;
            }
            set
            {
                playerpos = value;
                if(rigidBody != null)
                {
                    var org = new System.Numerics.Vector3(value.X, value.Y, value.Z);
                    rigidBody.Translate(org);
                }
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
        public float PlayerWeight = 5f;
        public float Speed = 5f;
        float camf = 100f, camn = 0.001f;
        public float JumpVelocity = 1f;
        public bool AirControl;
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
                app.cameraBoundingSphere = new BoundingSphere(new(Game.Player.PlayerPosition.X, Game.Player.PlayerPosition.Y + Game.Player.PlayerHeight, Game.Player.PlayerPosition.Z), Game.Player.CameraFar);
            }
        }
        public SVector3 PlayerForward
        {
            get
            {
                float h = Game.app.dist;
                return new SVector3((float)Math.Cos(Game.app.rotationY) * h, 0f, (float)Math.Sin(Game.app.rotationY) * h);
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
                app.cameraBoundingSphere = new BoundingSphere(new(Game.Player.PlayerPosition.X, Game.Player.PlayerPosition.Y + Game.Player.PlayerHeight, Game.Player.PlayerPosition.Z), Game.Player.CameraFar);
            }
        }

        /// <summary>
        /// Will give Physics to the player, if you want a floating camera don't call this.
        /// </summary>
        public void EnablePhysics()
        {
            if (PhysicsEngine.world == null) return;
            PhysicsEngine.pause = true;
            CollisionShape shape = new CapsuleShape(PlayerRadius, PlayerHeight);

            RigidBody body = new RigidBody(
                                    new RigidBodyConstructionInfo(
                                        PlayerWeight,
                                        null,
                                        shape, shape.CalculateLocalInertia(PlayerWeight)));

            body.Restitution = 0f;
            body.WorldTransform = Matrix4x4.CreateTranslation(PlayerPosition);
            rigidBody = body;
            PhysicsEngine.world.AddRigidBody(body);
            body.Gravity = new System.Numerics.Vector3(0f, -(9.81f * 2f), 0f);
            PhysicsEngine.pause = false;
        }

        /// <summary>
        /// Will remove Physics from the player.
        /// </summary>
        public void DisablePhysics()
        {
            PhysicsEngine.pause = true;
            PhysicsEngine.world.RemoveRigidBody(rigidBody);
            rigidBody.CollisionShape.Dispose();
            rigidBody.Dispose();
            rigidBody = null;
            PhysicsEngine.pause = false;
        }
    }
}
