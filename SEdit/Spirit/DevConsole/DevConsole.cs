using Realsphere.Spirit.Input;
using Realsphere.Spirit.Mathematics;
using Realsphere.Spirit.Rendering;
using Realsphere.Spirit.SceneManagement;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace Realsphere.Spirit.DeveloperConsole
{
    internal class DevConsoleRHook : SRenderHook
    {
        public override void OnRender(IntPtr d2dDevice, IntPtr d2dContext, IntPtr d3dDevice, IntPtr d3dContext, Scene scene, SVector3 playerPosition, SVector2 mousePosition)
        {
            if (!DevConsole.show) return;
            Device dev = new(d2dDevice);
            DeviceContext ctx = new(d2dContext);
            ctx.BeginDraw();

            var sceneColorBrush = new SolidColorBrush(Game.app.DeviceManager.Direct2DContext, Color.White);
            var textFormat = new TextFormat(Game.app.DeviceManager.DirectWriteFactory, "Arial", 12f) { TextAlignment = TextAlignment.Leading, ParagraphAlignment = ParagraphAlignment.Center };

            ctx.Transform = Matrix.Identity;
            ctx.DrawText("Text is: " + DevConsole.CommandText, textFormat, new RectangleF(50f, 50f, 200f, 200f), sceneColorBrush);

            textFormat.Dispose();
            sceneColorBrush.Dispose();

            ctx.EndDraw();
        }
    }

    public class DevCommand
    {
        /// <summary>
        /// Name of the Command
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Types of the Arguments this command takes
        /// </summary>
        public List<Type> ArgumentTypes { get; }
        /// <summary>
        /// On Execute Event handler
        /// </summary>
        public EventHandler<object[]> OnExecute;
    }

    public static class DevConsole
    {
        internal static DevConsoleRHook rHook;
        internal static List<DevCommand> commands= new List<DevCommand>();
        internal static bool show;
        public static string CommandText = "";
        internal static bool shift;

        internal static void keydown(object sender, int key)
        {
            if (!show) return;
            if(key == (int)Keys.Space)
            {
                CommandText += " ";
                return;
            }
            if (key == (int)Keys.Back && CommandText.Length > 0)
            {
                CommandText = CommandText.Substring(0, CommandText.Length - 1);
                return;
            }
            char chr = SKeyTranslator.KeyToChar(key);
            CommandText += (shift ? char.ToUpper(chr) : chr);
        }

        public static void Enable()
        {
            rHook = new DevConsoleRHook();
            SRenderHookRegister.RegisterRenderHook(rHook);
            Game.KeyDown += keydown;
        }

        public static void Disable()
        {
            SRenderHookRegister.UnregisterRenderHook(rHook);
            Game.KeyDown -= keydown;
        }

        public static void ToggleActive() => show = !show;
        public static void RegisterCommand(DevCommand cmd) => commands.Add(cmd);
        public static void UnregisterCommand(DevCommand cmd) => commands.Remove(cmd);
        public static bool IsCommandRegistered(DevCommand cmd) => commands.Contains(cmd);
    }
}
