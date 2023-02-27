using Realsphere.Spirit.DeveloperConsole;
using Realsphere.Spirit.Internal;
using Realsphere.Spirit.Mathematics;
using Realsphere.Spirit.RenderingCommon;
using Realsphere.Spirit.RGUI;
using Realsphere.Spirit.SceneManagement;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using Cursor = System.Windows.Forms.Cursor;

namespace Realsphere.Spirit
{
    public struct SpiritGameResolution
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public static SpiritGameResolution HD
        {
            get
            {
                return new SpiritGameResolution()
                {
                    Width = 1920,
                    Height = 1080
                };
            }
        }
        public static SpiritGameResolution I4K
        {
            get
            {
                return new SpiritGameResolution()
                {
                    Width = 3840,
                    Height = 2160
                };
            }
        }
        public static SpiritGameResolution I480P
        {
            get
            {
                return new SpiritGameResolution()
                {
                    Width = 640,
                    Height = 480
                };
            }
        }
        public static bool operator !=(SpiritGameResolution a, SpiritGameResolution b)
        {
            return !(a == b);
        }
        public static bool operator ==(SpiritGameResolution a, SpiritGameResolution b)
        {
            return (
                (a.Width == b.Width) && (a.Height == b.Height));
        }
    }
    public struct SpiritStartInfo
    {
        /// <summary>
        /// Enables the FPS counter in the upper left corner, because you cant have a game engine without a FPS counter in the upper left corner
        /// </summary>
        public bool ShowFPS { get; set; }

        /// <summary>
        /// Should the game be full Screen?
        /// </summary>
        public bool FullScreen { get; set; }

        /// <summary>
        /// Game Resolution
        /// </summary>
        public SpiritGameResolution Resolution { get; set; }

        /// <summary>
        /// Game name
        /// </summary>
        public string GameName { get; set; }

        /// <summary>
        /// Should the renderer VSync?
        /// </summary>
        public bool VSync { get; set; }

        /// <summary>
        /// Company name
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// The Game logo in Base64 encoding
        /// </summary>
        public string GameIcon { get; set; }

        /// <summary>
        /// Your Company logo in Base64 encoding
        /// </summary>
        public string CompanyLogo { get; set; }
    }
    public static class Game
    {
        internal static SpiritD3DApp app = null;

        /// <summary>
        /// The Player
        /// </summary>
        public static Player Player { get; private set; }

        /// <summary>
        /// The current Scene
        /// </summary>
        public static Scene ActiveScene
        {
            get
            {
                return ascene;
            }
            set
            {
                ascene = value;
                PhysicsEngine.setScene(value);
            }
        }
        static Scene ascene;

        public static IntPtr WindowHWND { get; internal set; }

        public static class Settings
        {
            public static float Sensitivity { get; set; } = 0.0025f;
        }

        public static EventHandler<SVector2> MouseLeftDown;
        public static EventHandler<SVector2> MouseRightDown;
        public static EventHandler<SVector2> MouseMiddleDown;
        public static EventHandler<SVector2> MouseRightUp;
        public static EventHandler<SVector2> MouseLeftUp;
        public static EventHandler<SVector2> MouseMiddleUp;
        public static EventHandler<int> KeyDown;
        public static EventHandler<int> KeyUp;
        public static EventHandler OnExit;

        public static float SimulationDistance { get; set; } = 100f;

        static Bitmap Base64StringToBitmap(this string
                                           base64String)
        {
            if (string.IsNullOrWhiteSpace(base64String)) return new Bitmap(1, 1);

            Bitmap bmpReturn = null;


            byte[] byteBuffer = Convert.FromBase64String(base64String);
            MemoryStream memoryStream = new MemoryStream(byteBuffer);


            memoryStream.Position = 0;


            bmpReturn = (Bitmap)Bitmap.FromStream(memoryStream);


            memoryStream.Close();
            memoryStream = null;
            byteBuffer = null;


            return bmpReturn;
        }

        static Thread appThread;
        static Thread t;

        internal static bool splashScreenFinished;
        internal static string gameName;
        internal static string logPath;
        internal static SpiritStartInfo info;

        public static bool ShowFPS { get; set; }

        internal static bool hasRenderedFirstFrame;

        /// <summary>
        /// Checks if the Game is running
        /// </summary>
        public static bool IsRunning
        {
            get;
            internal set;
        }

        static string zeroifyNum(int num)
        {
            if (num < 10)
                return "0" + num;
            else
                return "" + num;
        }

        internal static DeviceManager deviceManager;
        internal static bool PhysicsDebug = false;

        public static IntPtr Direct2DDevice
        {
            get
            {
                if (deviceManager.Direct2DDevice == null) return IntPtr.Zero;
                else return deviceManager.Direct2DDevice.NativePointer;
            }
        }
        public static IntPtr Direct2DContext
        {
            get
            {
                if (deviceManager.Direct3DContext == null) return IntPtr.Zero;
                else return deviceManager.Direct2DContext.NativePointer;
            }
        }
        public static IntPtr Direct3DDevice
        {
            get
            {
                if (deviceManager.Direct3DDevice == null) return IntPtr.Zero;
                else return deviceManager.Direct3DDevice.NativePointer;
            }
        }
        public static IntPtr Direct3DContext
        {
            get
            {
                if (deviceManager.Direct3DContext == null) return IntPtr.Zero;
                else return deviceManager.Direct3DContext.NativePointer;
            }
        }

        static bool cla = true;

        /// <summary>
        /// If true: Mouse is locked, camera can be moved, click events will be dispatched.
        /// If false: Mouse is free, camera cant be moved, click events will be dispatched.
        /// </summary>
        public static bool CameraLookActive
        {
            get { return cla; }
            set
            {
                cla = value;
                if (value)
                    Cursor.Hide();
                else
                    Cursor.Show();
                if (app != null && app.Window != null)
                {
                    Cursor.Hide();
                    if (!Game.CameraLookActive)
                        Cursor.Show();
                }
            }
        }

        public static void SetFullScreen(bool val)
        {
            if (info.FullScreen == val) return;
            info.FullScreen = val;

            if (val)
            {
                app.SetFullScreen(new(
                    info.Resolution.Width
                    , info.Resolution.Height
                    , new(120, 1)
                    , SharpDX.DXGI.Format.R8G8B8A8_UNorm));
            }
            else
            {
                app.SetWindowed(new(
                    info.Resolution.Width
                    , info.Resolution.Height
                    , new(120, 1)
                    , SharpDX.DXGI.Format.R8G8B8A8_UNorm));
            }
        }

        public static RGUI.RGUI RGUI
        {
            get;
            internal set;
        }
        public static void SetResolution(SpiritGameResolution res)
        {
            app.Window.ClientSize = new(res.Width, res.Height);
        }

        /// <summary>
        /// Opens a Window and starts rendering using Direct3D11.
        /// </summary>
        [STAThread]
        public static void StartGame(SpiritStartInfo ssi)
        {
            ShowFPS = ssi.ShowFPS;
            gameName = ssi.GameName;
            DateTime dt = DateTime.Now;
            logPath = Path.GetDirectoryName(Application.ExecutablePath) + $"\\logs\\{zeroifyNum(dt.Day)}-{zeroifyNum(dt.Month)}-{zeroifyNum(dt.Year)} {zeroifyNum(dt.Hour)}.{zeroifyNum(dt.Minute)}.{zeroifyNum(dt.Second)}.log";
            if (!Directory.Exists("logs")) Directory.CreateDirectory("logs");
            Logger.init(logPath);
            Logger.PrintSysInfo();
            Logger.Log("Writing Native Libraries", LogLevel.Information);
            try { File.WriteAllBytes("d3dcompiler_46.dll", Properties.Resources.d3dcompiler_46); } catch (IOException) { }
            try { File.WriteAllBytes("d3dcsx_46.dll", Properties.Resources.d3dcsx_46); } catch (IOException) { }
            try { File.WriteAllBytes("libbulletc.dll", Properties.Resources.libbulletc); } catch (IOException) { }
            try { File.WriteAllBytes("soft_oal.dll", Properties.Resources.soft_oal); } catch (IOException) { }
            try { File.WriteAllBytes("libsndfile-1.dll", Properties.Resources.libsndfile_1); } catch (IOException) { }
            try { File.WriteAllBytes("ogg.dll", Properties.Resources.ogg); } catch (IOException) { }
            try { File.WriteAllBytes("vorbis.dll", Properties.Resources.vorbis); } catch (IOException) { }
            try { File.WriteAllBytes("vorbisenc.dll", Properties.Resources.vorbisenc); } catch (IOException) { }
            try { File.WriteAllBytes("flac.dll", Properties.Resources.flac); } catch (IOException) { }
            Logger.Log("Finished!", LogLevel.Information);
            try
            {
                Logger.Log("Initializing", LogLevel.Information);
                Player = new Player();
                t = new Thread(InputThread);
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
                var t1 = new Thread(() =>
                {
                    while (app == null) { }
                    while (app.Window == null) { }
                    while (!app.Window.IsDisposed)
                        while (PhysicsEngine.running)
                            if (!PhysicsEngine.pause) { }
                });
                t1.SetApartmentState(ApartmentState.STA);
                t1.Start();
                Logger.Log("DirectX Initializing", LogLevel.Information);
                appThread = new Thread(() =>
                {
                    //try
                    //{
                    app = new SpiritD3DApp(ssi.FullScreen, ssi.GameName, ssi.Resolution.Width, ssi.Resolution.Height, ssi.CompanyLogo.Base64StringToBitmap(), ssi.GameIcon.Base64StringToBitmap());
                    app.Window.FormClosed += Window_FormClosed;
                    app.VSync = ssi.VSync;
                    app.Initialize();
                    app.Run();
                    //}
                    //catch(Exception ex)
                    //{
                    //    Throw(ex);
                    //}
                });
                appThread.SetApartmentState(ApartmentState.STA);
                appThread.Start();
                Logger.Log("Finished!", LogLevel.Information);
                while (!IsRunning) { }
                app.Window.MouseDown += (o, e) =>
                {
                    if (!DevConsole.show) if (e.Button == MouseButtons.Left && MouseLeftDown != null) MouseLeftDown.Invoke(null, e.Location);
                    if (!DevConsole.show) if (e.Button == MouseButtons.Middle && MouseMiddleDown != null) MouseMiddleDown.Invoke(null, e.Location);
                    if (!DevConsole.show) if (e.Button == MouseButtons.Right && MouseRightDown != null) MouseRightDown.Invoke(null, e.Location);
                };
                app.Window.MouseUp += (o, e) =>
                {
                    if (!DevConsole.show) if (e.Button == MouseButtons.Left && MouseLeftUp != null) MouseLeftUp.Invoke(null, e.Location);
                    if (!DevConsole.show) if (e.Button == MouseButtons.Middle && MouseMiddleUp != null) MouseMiddleUp.Invoke(null, e.Location);
                    if (!DevConsole.show) if (e.Button == MouseButtons.Right && MouseRightUp != null) MouseRightUp.Invoke(null, e.Location);
                };
            }
            catch (Exception ex)
            {
                Throw(ex);
            }
        }

        /// <summary>
        /// Executes a Action in the Game's thread.
        /// </summary>
        public static void ExecuteInGameThread(Action a)
        {
            if (app != null && app.Window != null)
                app.Window.Invoke(a);
        }

        private static void Window_FormClosed(object sender, FormClosedEventArgs e)
        {
            Logger.Close();
        }

        /// <summary>
        /// Logs the Exception, Shows crash message and Quits.
        /// </summary>
        public static void Throw(Exception ex)
        {
            Logger.Log(ex.Message, LogLevel.Error);
            Logger.Close();
            MessageBox.Show("An fatal error occurred in " + gameName + "!\n\nHResult: " + ex.HResult + "\n\n" + ex.Message + (ex.StackTrace != null ? "\n\nStack Trace:" + ex.StackTrace.Split(" in")[0] + ": " + ex.StackTrace.Split(" in")[1].Split("\\").Last() : "") + "\nHelp Link (if available): " + ex.HelpLink + "\n(this does not replace Realsphere support, you should still contact us if this error keeps recurring!)\nThe log has been written to: " + logPath + "\nIf you contact support, we might ask for this file.", gameName + " (Spirit Crash Handler)", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Environment.Exit(ex.HResult);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        extern static bool DestroyIcon(IntPtr handle);

        /// <summary>
        /// Stops the Game.
        /// </summary>
        public static void ExitGame()
        {
            void dispose(IDisposable dis)
            {
                if (dis != null) dis.Dispose();
            }

            void drop(SDroppable sdr)
            {
                if (sdr != null) sdr.Drop();
            }

            if (app != null)
            {
                if (app.DeviceManager != null)
                {
                    try
                    {
                        try
                        {
                            if (app != null && app.DeviceManager != null && app.DeviceManager.Direct2DDevice != null) app.DeviceManager.Direct2DDevice.ClearResources(0);
                        }
                        catch (NullReferenceException) { }
                        if (app.DeviceManager.Direct2DContext != null) dispose(app.DeviceManager.Direct2DContext);
                        if (app.DeviceManager.Direct2DDevice != null) dispose(app.DeviceManager.Direct2DDevice);
                        if (app.DeviceManager.Direct3DContext != null) dispose(app.DeviceManager.Direct3DContext);
                        if (app.DeviceManager.Direct3DContext != null) dispose(app.DeviceManager.Direct3DDevice);
                        if (app.DeviceManager.DirectWriteFactory != null) dispose(app.DeviceManager.DirectWriteFactory);
                        if (app.DeviceManager.WICFactory != null) dispose(app.DeviceManager.WICFactory);
                        if (app.DeviceManager != null) dispose(app.DeviceManager);
                    }
                    catch (Exception) { }
                }
                try
                {
                    if (app._backBuffer != null) dispose(app._backBuffer);
                    if (app._depthBuffer != null) dispose(app._depthBuffer);
                    if (app._bitmapTarget != null) dispose(app._bitmapTarget);
                    if (app._depthStencilView != null) dispose(app._depthStencilView);
                    if (app._renderTargetView != null) dispose(app._renderTargetView);
                    if (app._window != null) dispose(app._window);
                    if (app.BitmapTarget2D != null) dispose(app.BitmapTarget2D);
                    if (app.fps != null) drop(app.fps);
                    if (app != null) dispose(app);
                }
                catch (Exception) { }
            }
            Logger.Close();

            // Destroy known Memory Leaks
            {
                // wndHicon in SpiritD3DApp
                DestroyIcon(SpiritD3DApp.wndHicon);
                Marshal.FreeHGlobal(SpiritD3DApp.wndHicon);
            }

            if (OnExit != null) OnExit.Invoke(null, new());
            Environment.Exit(0);
        }

        static Action<object> UpdateSoundEnginePlayerParams = (object e) =>
        {
            AudioMaster.setListenerData();
        };

        [STAThread]
        static void InputThread()
        {
            while (app == null) { }
            while (app.Window == null) { }
            while (!app.Window.IsDisposed)
            {
                if (DevConsole.show)
                {
                    Cursor.Show();
                    continue;
                }
                if (!CameraLookActive) Cursor.Show();
                float speed = Game.Player.Speed / 50000f;
                if (Keyboard.IsKeyDown(Key.A))
                {
                    Game.Player.PlayerPosition -= Player.CameraRight * speed;
                }
                if (Keyboard.IsKeyDown(Key.D))
                {
                    Game.Player.PlayerPosition += Player.CameraRight * speed;
                }
                if (Keyboard.IsKeyDown(Key.W))
                {
                    Game.Player.PlayerPosition += app.cameraTarget * speed;
                }
                if (Keyboard.IsKeyDown(Key.S))
                {
                    Game.Player.PlayerPosition -= app.cameraTarget * speed;
                }
                ThreadPool.QueueUserWorkItem(UpdateSoundEnginePlayerParams, null, false);
            }
        }
    }
}
