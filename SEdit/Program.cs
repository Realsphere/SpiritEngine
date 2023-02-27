using Realsphere.Spirit;
using Realsphere.Spirit.Input;
using Realsphere.Spirit.Mathematics;
using Realsphere.Spirit.Rendering;
using Realsphere.Spirit.SceneManagement;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System;
using System.Runtime.InteropServices;
using SharpDX.Direct2D1;
using SharpDX;
using SharpDX.DirectWrite;
using System.Windows.Forms;
using System.IO;
using Cursor = System.Windows.Forms.Cursor;
using System.Collections.Generic;
using Realsphere.Spirit.DeveloperConsole;

namespace SEdit
{
    public static class Data
    {
        public static Scene Scene;
    }

    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            ApplicationConfiguration.Initialize();
            string logo;
            using (MemoryStream m = new MemoryStream())
            {
                Properties.Resources.Spirit_Logo.Save(m, Properties.Resources.Spirit_Logo.RawFormat);
                byte[] imageBytes = m.ToArray();
                logo = Convert.ToBase64String(imageBytes);
            }

            Game.StartGame(new SpiritStartInfo()
            {
                GameName = "SEdit",
                FullScreen = false,
                VSync = true,
                Company = "Realsphere Entertainment",
                GameIcon = logo,
                Resolution = SpiritGameResolution.HD,
                ShowFPS = true,
            });
            DevConsole.Enable();
            Game.CameraLookActive = false;
            Data.Scene = new();
            Game.ActiveScene = Data.Scene;
            Data.Scene.SkyBoxColor = new SColor(120f, 120f, 120f, 255f);

            Game.MouseRightDown += (o, e) =>
            {
                Game.CameraLookActive = true;
            };
            Game.MouseRightUp += (o, e) =>
            {
                Game.CameraLookActive = false;
            };
            Game.MouseRightDown += (o, e) =>
            {
                Game.CameraLookActive = true;
            };
            Game.MouseRightUp += (o, e) =>
            {
                Game.CameraLookActive = false;
            };

            Game.ExecuteInGameThread(() =>
            {
                EditorWindow f = new();
                Game.app.Window.Dock = DockStyle.Fill;
                f.MDIPanel.Controls.Add(Game.app.Window);
                Game.app.Window.Click += (o, e) =>
                {
                    Game.app.Window.Select();
                    Game.app.Window.Focus();
                };
                f.Icon = Properties.Resources.Spirit_Logo1;
                f.Text = "SEdit";
                f.Show();
            });
        }
    }
}