using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace Realsphere.Spirit.RenderingCommon
{
    internal class DeviceManager: SharpDX.Component
    {
        // Direct3D Objects
        protected SharpDX.Direct3D11.Device1 d3dDevice;
        protected SharpDX.Direct3D11.DeviceContext1 d3dContext;
        protected float dpi;

        // Declare Direct2D Objects
        protected SharpDX.Direct2D1.Factory1 d2dFactory;
        protected SharpDX.Direct2D1.Device d2dDevice;
        protected SharpDX.Direct2D1.DeviceContext d2dContext;

        // Declare DirectWrite & Windows Imaging Component Objects
        protected SharpDX.DirectWrite.Factory dwriteFactory;
        protected SharpDX.WIC.ImagingFactory2 wicFactory;

        /// <summary>
        /// The list of feature levels to accept
        /// </summary>
        internal FeatureLevel[] Direct3DFeatureLevels = new FeatureLevel[] {
            //FeatureLevel.Level_11_1, 
            //FeatureLevel.Level_11_0,
            //FeatureLevel.Level_10_1,
            //FeatureLevel.Level_10_0,
            FeatureLevel.Level_9_1,
            FeatureLevel.Level_9_2,
            FeatureLevel.Level_9_3,
        };

        /// <summary>
        /// Gets the Direct3D11 device.
        /// </summary>
        internal SharpDX.Direct3D11.Device1 Direct3DDevice { get { return d3dDevice; } }

        /// <summary>
        /// Gets the Direct3D11 immediate context.
        /// </summary>
        internal SharpDX.Direct3D11.DeviceContext1 Direct3DContext { get { return d3dContext; } }

        /// <summary>
        /// Gets the Direct2D factory.
        /// </summary>
        internal SharpDX.Direct2D1.Factory1 Direct2DFactory { get { return d2dFactory; } }

        /// <summary>
        /// Gets the Direct2D device.
        /// </summary>
        internal SharpDX.Direct2D1.Device Direct2DDevice { get { return d2dDevice; } }

        /// <summary>
        /// Gets the Direct2D context.
        /// </summary>
        internal SharpDX.Direct2D1.DeviceContext Direct2DContext { get { return d2dContext; } }

        /// <summary>
        /// Gets the DirectWrite factory.
        /// </summary>
        internal SharpDX.DirectWrite.Factory DirectWriteFactory { get { return dwriteFactory; } }

        /// <summary>
        /// Gets the WIC factory.
        /// </summary>
        internal SharpDX.WIC.ImagingFactory2 WICFactory { get { return wicFactory; } }

        /// <summary>
        /// Gets or sets the DPI.
        /// </summary>
        /// <remarks>
        /// This method will fire the event <see cref="OnDpiChanged"/>
        /// if the dpi is modified.
        /// </remarks>
        internal virtual float Dpi
        {
            get { return dpi; }
            set
            {
                if (dpi != value)
                {
                    dpi = value;
                    d2dContext.DotsPerInch = new SharpDX.Size2F(dpi, dpi);

                    if (OnDpiChanged != null)
                        OnDpiChanged(this);
                }
            }
        }

        /// <summary>
        /// This event is fired when the DeviceManager is initialized by the <see cref="Initialize"/> method.
        /// </summary>
        internal event Action<DeviceManager> OnInitialize;
        
        /// <summary>
        /// This event is fired when the <see cref="Dpi"/> is called,
        /// </summary>
        internal event Action<DeviceManager> OnDpiChanged;

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        /// <param name="window">Window to receive the rendering</param>
        internal virtual void Initialize(float dpi = 96.0f)
        {
            CreateInstances();

            if (OnInitialize != null)
                OnInitialize(this);

            Dpi = dpi;
        }

        /// <summary>
        /// Creates device manager objects
        /// </summary>
        /// <remarks>
        /// This method is called at the initialization of this instance.
        /// </remarks>
        protected virtual void CreateInstances()
        {
            // Dispose previous references and set to null
            RemoveAndDispose(ref d3dDevice);
            RemoveAndDispose(ref d3dContext);
            RemoveAndDispose(ref d2dDevice);
            RemoveAndDispose(ref d2dContext);
            RemoveAndDispose(ref d2dFactory);
            RemoveAndDispose(ref dwriteFactory);
            RemoveAndDispose(ref wicFactory);

            #region Create Direct3D 11.1 device and retrieve device context

            // Bgra performs better especially with Direct2D software
            // render targets
            var creationFlags = DeviceCreationFlags.BgraSupport;

            // Retrieve the Direct3D 11.1 device and device context
            using (var device = new Device(DriverType.Hardware, creationFlags, Direct3DFeatureLevels))
            {
                d3dDevice = ToDispose(device.QueryInterface<Device1>());
            }

            // Get Direct3D 11.1 context
            d3dContext = ToDispose(d3dDevice.ImmediateContext.QueryInterface<DeviceContext1>());
            #endregion

            #region Create Direct2D device and context

            var debugLevel = SharpDX.Direct2D1.DebugLevel.None;

            // Allocate new references
            d2dFactory = ToDispose(new SharpDX.Direct2D1.Factory1(SharpDX.Direct2D1.FactoryType.SingleThreaded, debugLevel));
            dwriteFactory = ToDispose(new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Shared));
            wicFactory = ToDispose(new SharpDX.WIC.ImagingFactory2());

            // Create Direct2D device
            using (var dxgiDevice = d3dDevice.QueryInterface<SharpDX.DXGI.Device>())
            {
                d2dDevice = ToDispose(new SharpDX.Direct2D1.Device(d2dFactory, dxgiDevice));
            }

            // Create Direct2D context
            d2dContext = ToDispose(new SharpDX.Direct2D1.DeviceContext(d2dDevice, SharpDX.Direct2D1.DeviceContextOptions.None));
            #endregion
            Game.RGUI = new RGUI.RGUI(d2dDevice);
        }
    }
}
