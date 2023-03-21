using Realsphere.Spirit.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Realsphere.Spirit.Input
{
    public static class SKeyboard
    {
        public enum SKey
        {
            A = Key.A,
            B = Key.B,
            C = Key.C,
            D = Key.D,
            E = Key.E,
            F = Key.F,
            G = Key.G,
            H = Key.H,
            I = Key.I,
            J = Key.J,
            K = Key.K,
            L = Key.L,
            M = Key.M,
            N = Key.N,
            O = Key.O,
            P = Key.P,
            Q = Key.Q,
            R = Key.R,
            S = Key.S,
            T = Key.T,
            U = Key.U,
            V = Key.V,
            W = Key.W,
            X = Key.X,
            Y = Key.Y,
            Z = Key.Z,
            Enter = Key.Enter,
            LShift = Key.LeftShift,
            RShift = Key.RightShift,
            LControl = Key.LeftCtrl,
            RControl = Key.RightCtrl,
            Oem1 = Key.Oem1, 
            Oem2 = Key.Oem2, 
            Oem3 = Key.Oem3, 
            Oem4 = Key.Oem4,
            Oem5 = Key.Oem5, 
            Oem6 = Key.Oem6, 
            Oem7 = Key.Oem7, 
            Oem8 = Key.Oem8,
            Num1 = Key.NumPad1,
            Num2 = Key.NumPad2,
            Num3 = Key.NumPad3,
            Num4 = Key.NumPad4,
            Num5 = Key.NumPad5,
            Num6 = Key.NumPad6,
            Num7 = Key.NumPad7,
            Num8 = Key.NumPad8,
            Num9 = Key.NumPad9,
            Num0 = Key.NumPad0,
            Backspace = Key.Back,
            Escape = Key.Escape,
            F1 = Key.F1,
            F2 = Key.F2,
            F3 = Key.F3,
            F4 = Key.F4,
            F5 = Key.F5,
            F6 = Key.F6,
            F7 = Key.F7,
            F8 = Key.F8,
            F9 = Key.F9,
            F10 = Key.F10,
            F11 = Key.F11,
            F12 = Key.F12,
            Space = Key.Space,
        }

        [STAThread]
        public static bool IsKeyDown(SKey key)
        {
            bool result = false;
            bool finsihed = false;

            Thread t = new Thread(() =>
            {
                result = Keyboard.IsKeyDown((Key)key);
                finsihed = true;
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            while(!finsihed) { }

            return result;
        }
    }

    public static class SMouse
    {
        public enum SMouseButton
        {
            Left,
            Right,
            Middle,
            X1,
            X2
        }

        public static bool IsMouseButtonDown(SMouseButton btn)
        {
            return SpiritD3DApp.MouseButtons.Contains(btn);
        }

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref System.Drawing.Point lpPoint);

        public static SVector2 ScreenLocation
        {
            get
            {
                var p = new System.Drawing.Point();
                if(!GetCursorPos(ref p)) return new SVector2(0, 0);
                return p;
            }
        }

        public static SVector2 Location
        {
            get
            {
                var p = new System.Drawing.Point();
                if (!GetCursorPos(ref p)) return new SVector2(0, 0);
                return Game.app.Window.PointToClient(p);
            }
        }
    }
}
