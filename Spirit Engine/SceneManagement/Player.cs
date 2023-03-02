using BulletSharp;
using Realsphere.Spirit.BulletPhysics;
using Realsphere.Spirit.Internal;
using Realsphere.Spirit.Mathematics;
using SharpDX;
using System;
using System.Numerics;
using System.Threading;
using System.Windows.Documents;
using static Realsphere.Spirit.Game;

namespace Realsphere.Spirit.SceneManagement
{
    public class Player
    {
        SVector3 playerpos = new SVector3();
        public SVector3 PlayerPosition
        {
            get
            {
                return _ghostObject == null ? playerpos : _ghostObject.WorldTransform.Translation;
            }
            set
            {
                playerpos = value;
                if(_ghostObject != null)
                {
                    var org = new System.Numerics.Vector3(value.X, value.Y, value.Z);
                    _character.Warp(ref org);
                }
                AudioMaster.setListenerData();
                app.cameraBoundingSphere = new BoundingSphere(new(Game.Player.PlayerPosition.X, Game.Player.PlayerPosition.Y + Game.Player.PlayerHeight, Game.Player.PlayerPosition.Z), Game.Player.CameraFar);
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
        internal PairCachingGhostObject PairCachingGhostObject;
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
                app.cameraBoundingSphere = new BoundingSphere(new(Game.Player.PlayerPosition.X, Game.Player.PlayerPosition.Y + Game.Player.PlayerHeight, Game.Player.PlayerPosition.Z), Game.Player.CameraFar);
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

        internal PairCachingGhostObject _ghostObject;
        internal KinematicCharacterController _character;

        public Player()
        {
        }

        /// <summary>
        /// Will give Physics to the player, if you want a floating camera don't call this.
        /// </summary>
        public void EnablePhysics()
        {
            throw new NotImplementedException("Sorry, this is currently not available!");
            var _capsuleShape = new CapsuleShape(PlayerWeight, PlayerHeight);
            _ghostObject = new PairCachingGhostObject()
            {
                CollisionShape = _capsuleShape,
                CollisionFlags = CollisionFlags.CharacterObject,
                WorldTransform = Matrix4x4.CreateTranslation(playerpos)
            };
            PhysicsEngine.world.AddCollisionObject(_ghostObject, CollisionFilterGroups.CharacterFilter, CollisionFilterGroups.AllFilter);

            var up = System.Numerics.Vector3.UnitY;
            _character = new KinematicCharacterController(_ghostObject, _capsuleShape, 20f, ref up);
            PhysicsEngine.world.AddAction(_character);

            _ghostObject.WorldTransform = Matrix4x4.CreateTranslation(playerpos);
        }

        /// <summary>
        /// Will remove Physics from the player.
        /// </summary>
        public void DisablePhysics()
        {
            throw new NotImplementedException("Sorry, this is currently not available!");
        }
    }
}
