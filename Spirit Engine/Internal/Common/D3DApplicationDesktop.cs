using SharpDX.DXGI;
using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realsphere.Spirit.RenderingCommon
{
    /// <summary>
    /// Implements support for swap chain creation from a System.Windows.Form
    /// </summary>
    internal abstract class D3DApplicationDesktop: D3DApplicationBase
    {
        internal System.Windows.Forms.Form _window;

        internal System.Windows.Forms.Form Window { get { return _window; } }

        internal void Window_SizeChanged(object sender, EventArgs e)
        {
            SizeChanged();
        }

        internal override SharpDX.Rectangle CurrentBounds
        {
            get
            {
                return new SharpDX.Rectangle(_window.ClientRectangle.X, _window.ClientRectangle.Y, _window.ClientRectangle.Width, _window.ClientRectangle.Height);
            }
        }

        protected virtual SharpDX.DXGI.SwapChainFullScreenDescription CreateFullScreenDescription(bool windowed)
        {
            return new SharpDX.DXGI.SwapChainFullScreenDescription()
            {
                RefreshRate = new SharpDX.DXGI.Rational(120, 1),
                Scaling = SharpDX.DXGI.DisplayModeScaling.Stretched,
                Windowed = windowed
            };
        }

        protected override SharpDX.DXGI.SwapChain1 CreateSwapChain(SharpDX.DXGI.Factory2 factory, SharpDX.Direct3D11.Device device, SharpDX.DXGI.SwapChainDescription1 desc)
        {
            // Creates the swap chain for the Window's Hwnd
            return new SwapChain1(factory, device, Window.Handle, ref desc, CreateFullScreenDescription(false), null);
        }

        //internal bool TrySetDisplayMode(int width, int height, bool fullScreen)
        //{
        //    // Fail attempts when the DisplayModeList has not be initialized
        //    if (DisplayModeList == null)
        //        return false;

        //    // Try to find the first mode that matches the provided dimensions
        //    ModeDescription firstMatch = (from mode in DisplayModeList
        //                                   where mode.Width == width && mode.Height == height
        //                                   select mode).FirstOrDefault();

        //    // If the width > 0 then a matching mode was found
        //    if (firstMatch.Width > 0)
        //    {
        //        if (fullScreen)
        //            SetFullScreen(firstMatch);
        //        else
        //            SetWindowed(firstMatch);
        //        return true;
        //    }

        //    return false;
        //}

        internal void SetWindowed(ModeDescription mode)
        {
            Window.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            Window.ClientSize = new System.Drawing.Size(mode.Width, mode.Height);
            SwapChain.IsFullScreen = false;
            SwapChain.ResizeTarget(ref mode);
        }

        internal void SetFullScreen(ModeDescription mode)
        {
            Window.SizeChanged -= Window_SizeChanged;
            Window.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Window.ClientSize = new System.Drawing.Size(mode.Width, mode.Height);
            Window.SizeChanged += Window_SizeChanged;
          
            SizeChanged();

            SwapChain.SetFullscreenState(true, null);
            //SwapChain.IsFullScreen = true;
            SwapChain.ResizeTarget(ref mode);
        }
    }
}
