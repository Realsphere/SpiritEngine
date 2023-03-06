#region Using Statements
using Realsphere.Spirit.BulletPhysics;
using Realsphere.Spirit.DeveloperConsole;
using Realsphere.Spirit.Input;
using Realsphere.Spirit.Internal;
using Realsphere.Spirit.Mathematics;
using Realsphere.Spirit.Physics;
using Realsphere.Spirit.Rendering;
using Realsphere.Spirit.RenderingCommon;
using Realsphere.Spirit.SceneManagement;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Text;
using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Buffer = SharpDX.Direct3D11.Buffer;
using Cursor = System.Windows.Forms.Cursor;
using DepthStencilState = SharpDX.Direct3D11.DepthStencilState;
using InputElement = SharpDX.Direct3D11.InputElement;
using Size = System.Drawing.Size;
#endregion

namespace Realsphere.Spirit
{
    internal class SpiritD3DApp : D3DApplicationDesktop
    {
        #region Variables
        internal VertexShader vertexShader;

        PixelShader pixelShader;

        internal BoundingSphere cameraBoundingSphere = new BoundingSphere();

        PixelShader depthPixelShader;

        PixelShader lambertShader;

        PixelShader blinnPhongShader;

        internal Matrix projectionMatrix;

        internal PixelShader phongShader;

        InputLayout vertexLayout;

        DepthStencilState depthStencilState;

        internal Buffer perObjectBuffer;

        Buffer perFrameBuffer;

        internal Buffer perMaterialBuffer;

        internal float rotationX = 0f;
        internal float rotationY = 0f;
        internal float dist = 1;
        internal FpsCounter fps;
        internal Matrix viewMatrix = Matrix.LookAtRH(new Vector3(0, 5f, 0), new Vector3(15f, 5f, 15f), Vector3.UnitY);
        internal Vector3 cameraTarget = Vector3.Zero;

        bool fscr;

        Size? toSetSize;

        internal static List<SMouse.SMouseButton> MouseButtons = new();

        internal static IntPtr wndHicon;
        #endregion

        internal List<Keys> keysDown = new();

        internal bool pauseRendering;

        internal SpiritD3DApp(bool fullscreen, string title, int w, int h, Bitmap companyico, Bitmap gameico)
        {
            ThreadPool.SetMaxThreads(Environment.ProcessorCount * 250, Environment.ProcessorCount * 250);
            AudioMaster.init();
            AudioMaster.setListenerData();
            StandarizedShapes.init();
            var wnd = new SpiritWnd();

            if (gameico != null)
            {
                // Get HIcon from gameico
                wndHicon = gameico.GetHicon();

                // Create Icon for Window
                Icon ico = ToDispose(Icon.FromHandle(wndHicon));
                wnd.Icon = ico;
            }

            wnd.BackColor = System.Drawing.Color.Black;
            wnd.BackgroundImage = Properties.Resources.Spirit_Logo;
            wnd.BackgroundImageLayout = ImageLayout.Center;
            wnd.Text = title;
            wnd.FormBorderStyle = FormBorderStyle.Sizable;
            _window = wnd;
            Window.SizeChanged += Window_SizeChanged;
            this.Window.Text = title;
            fscr = fullscreen;
            toSetSize = new Size(w, h);
            Logger.Log("Creating DirectX Device and Context", LogLevel.Information);
            DeviceManager.Initialize();

            _window.GotFocus += (s, e) =>
            {
                if (Game.CameraLookActive) Cursor.Hide();
                if (fscr) SetFullScreen(new ModeDescription(toSetSize.Value.Width, toSetSize.Value.Height, new Rational(120, 1), Format.R8G8B8A8_UNorm));
            };
            _window.MouseDown += (o, e) =>
            {
                if (DevConsole.show) return;
                switch (e.Button)
                {
                    case System.Windows.Forms.MouseButtons.Left:
                        MouseButtons.Add(SMouse.SMouseButton.Left);
                        break;
                    case System.Windows.Forms.MouseButtons.Right:
                        MouseButtons.Add(SMouse.SMouseButton.Right);
                        break;
                    case System.Windows.Forms.MouseButtons.Middle:
                        MouseButtons.Add(SMouse.SMouseButton.Middle);
                        break;
                    case System.Windows.Forms.MouseButtons.XButton1:
                        MouseButtons.Add(SMouse.SMouseButton.X1);
                        break;
                    case System.Windows.Forms.MouseButtons.XButton2:
                        MouseButtons.Add(SMouse.SMouseButton.X2);
                        break;
                }
            };
            _window.MouseUp += (o, e) =>
            {
                if (DevConsole.show) return;
                switch (e.Button)
                {
                    case System.Windows.Forms.MouseButtons.Left:
                        MouseButtons.Remove(SMouse.SMouseButton.Left);
                        break;
                    case System.Windows.Forms.MouseButtons.Right:
                        MouseButtons.Remove(SMouse.SMouseButton.Right);
                        break;
                    case System.Windows.Forms.MouseButtons.Middle:
                        MouseButtons.Remove(SMouse.SMouseButton.Middle);
                        break;
                    case System.Windows.Forms.MouseButtons.XButton1:
                        MouseButtons.Remove(SMouse.SMouseButton.X1);
                        break;
                    case System.Windows.Forms.MouseButtons.XButton2:
                        MouseButtons.Remove(SMouse.SMouseButton.X2);
                        break;
                }
            };
            _window.LostFocus += (s, e) =>
            {
                Cursor.Show();
            };
            _window.FormClosed += (s, e) =>
            {
                Game.ExitGame();
            };
            DateTime dt = DateTime.Now;
            _window.KeyDown += (o, e) =>
            {
                if (!keysDown.Contains(e.KeyCode)) keysDown.Add(e.KeyCode);
                if (e.KeyCode != Keys.Space) return;
                if (!Game.Player.Grounded) return;
                if (dt.Subtract(DateTime.Now).TotalSeconds > -1) return;

                var jumpForce = new System.Numerics.Vector3(Game.Player.rigidBody.LinearVelocity.X, Game.Player.JumpVelocity, Game.Player.rigidBody.LinearVelocity.Z);
                Game.Player.rigidBody.LinearVelocity = (jumpForce * 5f);
                dt = DateTime.Now;
            };
            _window.KeyUp += (o, e) =>
            {
                keysDown.Remove(e.KeyCode);
            };

            if (toSetSize != null) Window.ClientSize = toSetSize.Value;
            Window.WindowState = FormWindowState.Minimized;
            Window.WindowState = FormWindowState.Normal;
        }

        protected override void Dispose(bool disposeManagedResources)
        {
            Logger.Log("Disposing...", LogLevel.Information);
            if (disposeManagedResources)
            {
                if (SwapChain != null && !SwapChain.IsDisposed)
                {
                    SwapChain.IsFullScreen = false;
                }
            }
            DeviceManager.Dispose();
        }
        static bool swapChainPrintLog = false;
        static SwapChain1 res;
        protected override SwapChain1 CreateSwapChain(Factory2 factory1, SharpDX.Direct3D11.Device device, SwapChainDescription1 desc1)
        {
            if (!swapChainPrintLog)
            {
                swapChainPrintLog = true;
            }

            SwapChainFullScreenDescription scd = new SwapChainFullScreenDescription()
            {
                RefreshRate = new Rational(120, 1),
                Scaling = DisplayModeScaling.Stretched,
                Windowed = true
            };

            using (var dxgiDevice2 = device.QueryInterface<Device2>())
            using (var dxgiAdapter = dxgiDevice2.Adapter)
            using (var dxgiFactory2 = dxgiAdapter.GetParent<Factory2>())
            {
                Window.TopMost = false;
                if (fscr) Window.FormBorderStyle = FormBorderStyle.None;
                if (toSetSize.HasValue)
                {
                    desc1.Width = toSetSize.Value.Width;
                    desc1.Height = toSetSize.Value.Height;
                }
                else
                {
                    desc1.Width = Window.ClientSize.Width;
                    desc1.Height = Window.ClientSize.Height;
                }
                if (res == null || res.NativePointer == IntPtr.Zero || !res.IsFullScreen)
                {
                    res = new SwapChain1(dxgiAdapter.GetParent<Factory2>(), device, Window.Handle, ref desc1, scd);
                }
                return res;
            }
        }

        internal Matrix viewProjection;

        protected override void CreateDeviceDependentResources(DeviceManager deviceManager)
        {
            base.CreateDeviceDependentResources(deviceManager);

            RemoveAndDispose(ref vertexShader);

            RemoveAndDispose(ref pixelShader);
            RemoveAndDispose(ref depthPixelShader);
            RemoveAndDispose(ref lambertShader);
            RemoveAndDispose(ref blinnPhongShader);
            RemoveAndDispose(ref phongShader);

            RemoveAndDispose(ref vertexLayout);
            RemoveAndDispose(ref perObjectBuffer);
            RemoveAndDispose(ref perFrameBuffer);
            RemoveAndDispose(ref perMaterialBuffer);

            RemoveAndDispose(ref depthStencilState);

            var device = deviceManager.Direct3DDevice;
            var context = deviceManager.Direct3DContext;

            // Compile and create the vertex shader and input layout
            using (var vertexShaderBytecode = HLSLCompiler.CompileFromCode(Encoding.UTF8.GetString(Properties.Resources.SDVS), "VSMain", "vs_5_0"))
            {
                vertexShader = ToDispose(new VertexShader(device, vertexShaderBytecode));
                // Layout from VertexShader input signature
                vertexLayout = ToDispose(new InputLayout(device,
                    vertexShaderBytecode.GetPart(ShaderBytecodePart.InputSignatureBlob),
                new[]
                {
                    new InputElement("SV_Position", 0, Format.R32G32B32_Float, 0, 0),
                    new InputElement("NORMAL", 0, Format.R32G32B32_Float, 12, 0),
                    new InputElement("COLOR", 0, Format.R8G8B8A8_UNorm, 24, 0),
                    new InputElement("TEXCOORD", 0, Format.R32G32_Float, 28, 0),
                }));
            }

            using (var bytecode = HLSLCompiler.CompileFromCode(Encoding.UTF8.GetString(Properties.Resources.SDPX), "PSMain", "ps_5_0"))
                phongShader = ToDispose(new PixelShader(device, bytecode));

            perObjectBuffer = ToDispose(new SharpDX.Direct3D11.Buffer(device, Utilities.SizeOf<ConstantBuffers.PerObject>(), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0));

            perFrameBuffer = ToDispose(new Buffer(device, Utilities.SizeOf<ConstantBuffers.PerFrame>(), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0));

            perMaterialBuffer = ToDispose(new Buffer(device, Utilities.SizeOf<ConstantBuffers.PerMaterial>(), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0));

            depthStencilState = ToDispose(new DepthStencilState(device,
                new DepthStencilStateDescription()
                {
                    IsDepthEnabled = true,
                    DepthComparison = Comparison.Less,
                    DepthWriteMask = SharpDX.Direct3D11.DepthWriteMask.All,
                    IsStencilEnabled = false,
                    StencilReadMask = 0xff,
                    StencilWriteMask = 0xff,
                    FrontFace = new DepthStencilOperationDescription()
                    {
                        Comparison = Comparison.Always,
                        PassOperation = StencilOperation.Keep,
                        FailOperation = StencilOperation.Keep,
                        DepthFailOperation = StencilOperation.Increment
                    },
                    BackFace = new DepthStencilOperationDescription()
                    {
                        Comparison = Comparison.Always,
                        PassOperation = StencilOperation.Keep,
                        FailOperation = StencilOperation.Keep,
                        DepthFailOperation = StencilOperation.Decrement
                    },
                }));

            context.InputAssembler.InputLayout = vertexLayout;

            context.VertexShader.SetConstantBuffer(0, perObjectBuffer);
            context.VertexShader.SetConstantBuffer(1, perFrameBuffer);
            context.VertexShader.SetConstantBuffer(2, perMaterialBuffer);

            context.VertexShader.Set(vertexShader);

            context.PixelShader.SetConstantBuffer(1, perFrameBuffer);
            context.PixelShader.SetConstantBuffer(2, perMaterialBuffer);

            context.PixelShader.Set(phongShader);

            context.OutputMerger.DepthStencilState = depthStencilState;

            context.Rasterizer.State = ToDispose(new RasterizerState(device, new RasterizerStateDescription()
            {
                FillMode = FillMode.Solid,
                CullMode = CullMode.Front,
                IsFrontCounterClockwise = false,
            }));

            BlendStateDescription blendStateDesc = new BlendStateDescription()
            {
                AlphaToCoverageEnable = false,
                IndependentBlendEnable = false,
            };

            for (int i = 0; i < blendStateDesc.RenderTarget.Length; i++)
            {
                blendStateDesc.RenderTarget[i] = new RenderTargetBlendDescription()
                {
                    IsBlendEnabled = true,
                    BlendOperation = BlendOperation.Add,
                    SourceBlend = BlendOption.SourceAlpha,
                    DestinationBlend = BlendOption.InverseSourceAlpha,
                    AlphaBlendOperation = BlendOperation.Add,
                    SourceAlphaBlend = BlendOption.One,
                    DestinationAlphaBlend = BlendOption.Zero,
                    RenderTargetWriteMask = ColorWriteMaskFlags.All
                };
            }

            // Create a blend state object from the description
            BlendState blendState = new BlendState(device, blendStateDesc);

            // Set the blend state on the context
            context.OutputMerger.SetBlendState(blendState);
        }

        protected override void CreateSizeDependentResources(D3DApplicationBase app)
        {
            base.CreateSizeDependentResources(app);
        }

        // basically dispose, but better
        public void Drop()
        {
            Dispose();
            fps.Drop();
            Window.Close();
            Window.Dispose();
            {
                DeviceManager.Direct2DDevice.Dispose();
                DeviceManager.Direct2DContext.Dispose();
                DeviceManager.Direct3DDevice.Dispose();
                DeviceManager.Direct3DContext.Dispose();
                DeviceManager.DirectWriteFactory.Dispose();
                DeviceManager.WICFactory.Dispose();
                DeviceManager.Dispose();
            }
        }

        internal bool canCameraSeeObject(GameObject go)
        {
            return go.BoundingBox.dx.Intersects(cameraBoundingSphere);
        }

        internal override void Run()
        {
            fps = new FpsCounter(this);

            var worldMatrix = Matrix.Identity;

            projectionMatrix = Matrix.PerspectiveFovRH((float)Math.PI / 3f, Width / (float)Height, Game.Player.CameraNear, Game.Player.CameraFar);

            Window.Resize += (s, e) =>
            {
                projectionMatrix = Matrix.PerspectiveFovRH((float)Math.PI / 3f, Width / (float)Height, Game.Player.CameraNear, Game.Player.CameraFar);
            };
            float cos(float a) => MathF.Cos(a);
            float sin(float a) => MathF.Sin(a);

            Game.WindowHWND = Window.Handle;

            Logger.Log("Initialising FPS Camera...", LogLevel.Information);
            Window.MouseMove += (s, e) =>
            {
                if (!Game.CameraLookActive)
                {
                    Cursor.Show();
                    return;
                }
                if (!Game.app.Window.Focused) return;

                Cursor.Hide();

                var yRotate = (Game.app.Window.ClientSize.Width / 2) - e.X;
                var xRotate = (Game.app.Window.ClientSize.Height / 2) - e.Y;
                Game.app.rotationX += xRotate * Game.Settings.Sensitivity;
                Game.app.rotationY -= yRotate * Game.Settings.Sensitivity;

                Game.app.rotationX = Math.Clamp(Game.app.rotationX, -1.57f, 1.57f);

                float h = cos(Game.app.rotationX) * Game.app.dist;
                Game.app.cameraTarget = new Vector3(cos(Game.app.rotationY) * h, sin(Game.app.rotationX) * Game.app.dist, sin(Game.app.rotationY) * h);
                Game.app.viewMatrix = Matrix.LookAtRH(new(Game.Player.PlayerPosition.X, Game.Player.PlayerPosition.Y + Game.Player.PlayerHeight, Game.Player.PlayerPosition.Z), new Vector3(Game.Player.PlayerPosition.X, Game.Player.PlayerPosition.Y + Game.Player.PlayerHeight, Game.Player.PlayerPosition.Z) + Game.app.cameraTarget, Vector3.UnitY);

                Cursor.Position = Game.app.Window.PointToScreen(new System.Drawing.Point((Game.app.Window.ClientSize.Width / 2), (Game.app.Window.ClientSize.Height / 2)));
            };

            #region Rotation and window event handlers

            var rotation = new Vector3(0.0f, 0.0f, 0.0f);

            #endregion

            var clock = new Stopwatch();
            clock.Start();

            Game.deviceManager = DeviceManager;
            Logger.Log("Initialising Physics Engine...", LogLevel.Information);
            PhysicsEngine.init();
            #region Render loop
            Logger.Log("Initialisation finished, starting Render Loop!", LogLevel.Information);
            RenderLoop.Run(Window, () =>
            {
                if (pauseRendering) return;

                if (!Game.IsRunning)
                    Game.IsRunning = true;

                var context = DeviceManager.Direct3DContext;

                if (Game.ActiveScene == null)
                {
                    context.ClearDepthStencilView(DepthStencilView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1.0f, 0);
                    context.ClearRenderTargetView(RenderTargetView, Color4.Black);
                    return;
                }

                context.ClearDepthStencilView(DepthStencilView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1.0f, 0);
                context.ClearRenderTargetView(RenderTargetView, Game.ActiveScene.SkyBoxColor.sharpdxcolor);

                viewProjection = Matrix.Multiply(viewMatrix, projectionMatrix);

                var camPosition = Matrix.Transpose(Matrix.Invert(viewMatrix)).Column4;

                var time = clock.ElapsedMilliseconds / 1000.0f;
                var worldRotation = Matrix.RotationAxis(Vector3.UnitY, time);

                var perFrame = new ConstantBuffers.PerFrame();
                perFrame.Light.Color = Game.ActiveScene.Light.LightColor.sharpdxcolor;
                var lightDir = Vector3.Transform(Game.ActiveScene.Light.LightDirection.sharpDXVector, worldMatrix);
                perFrame.Light.Direction = new Vector3(lightDir.X, lightDir.Y, lightDir.Z);
                perFrame.CameraPosition = Game.Player.PlayerPosition.sharpDXVector;
                context.UpdateSubresource(ref perFrame, perFrameBuffer);
                try
                {
                    foreach (GameObject go in Game.ActiveScene.GameObjects.Where(x => x.renderer != null && canCameraSeeObject(x)))
                    {
                        go.renderer.PerMaterialBuffer = perMaterialBuffer;
                        RenderObject(go, go.WorldTransform, context, viewProjection);
                    }
                }
                catch (InvalidOperationException) { }

                if (Game.ShowTriggers)
                {
                    //Display Triggers
                    foreach (Trigger trigger in Game.ActiveScene.Triggers)
                    {
                        trigger.go.renderer.PerMaterialBuffer = perMaterialBuffer;
                        trigger.render(context);
                    }
                }

                if (Game.ShowFPS) fps.Render();

                if (!SRenderHookRegister.dontCall)
                {
                    foreach (var renderHook in SRenderHookRegister.Hooks)
                    {
                        renderHook.OnRender(DeviceManager.Direct2DDevice.NativePointer,
                            DeviceManager.Direct2DContext.NativePointer,
                            DeviceManager.Direct3DDevice.NativePointer,
                            DeviceManager.Direct3DContext.NativePointer,
                            Game.ActiveScene,
                            Game.Player.PlayerPosition,
                            SMouse.Location);
                    }
                }

                Game.RGUI.Render(DeviceManager.Direct2DContext);

                if (Game.ShowFPS)
                {
                    fps.show = true;
                    fps.Render();
                }

                Present();

                if (!Game.hasRenderedFirstFrame) Game.hasRenderedFirstFrame = true;
            });
            #endregion
            fps.Drop();
        }

        internal void RenderObject(GameObject obj, Matrix objMatrix, DeviceContext context, Matrix viewProjection)
        {
            var perObject = new ConstantBuffers.PerObject();

            var perMaterial = new ConstantBuffers.PerMaterial();
            perMaterial.Ambient = obj.Material.Ambient.sharpdxcolor;
            perMaterial.Diffuse = obj.Material.Diffuse.sharpdxcolor;
            perMaterial.Emissive = obj.Material.Emissive.sharpdxcolor;
            perMaterial.Specular = obj.Material.Specular.sharpdxcolor;
            perMaterial.SpecularPower = obj.Material.SpecularPower;
            context.UpdateSubresource(ref perMaterial, perMaterialBuffer);

            perObject.World = objMatrix;

            perObject.WorldInverseTranspose = Matrix.Transpose(Matrix.Invert(perObject.World));
            perObject.WorldViewProjection = perObject.World * viewProjection;
            perObject.Transpose();
            obj.renderer.mat = obj.Material;
            if (obj.Material != null && obj.Material.Texture != null)
            {
                perMaterial.Ambient = new Color4(1f);
                perMaterial.Diffuse = new Color4(1f);
                perMaterial.Specular = new Color4(0f);
                obj.Material.Texture.apply(obj.renderer);
            }
            obj.renderer.RenderContext.UpdateSubresource(ref perObject, perObjectBuffer);
            obj.renderer.Render();
        }
    }
}