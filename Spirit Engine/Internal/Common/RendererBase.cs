using Realsphere.Spirit.Rendering;
using Realsphere.Spirit.SceneManagement;
using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realsphere.Spirit.RenderingCommon
{
    internal abstract class RendererBase: Component
    {
        internal DeviceManager DeviceManager { get; set; }
        internal D3DApplicationBase Target { get; set; }
        internal SMaterial mat;
        internal virtual bool Show { get; set; }
        internal Matrix World;
        internal GameObject objectOn;
        internal ShaderResourceView texture;
        internal SamplerState samplerState;

        // Allow the context used for rendering to be specified
        SharpDX.Direct3D11.DeviceContext _renderContext = null;
        internal SharpDX.Direct3D11.DeviceContext RenderContext
        {
            get { return _renderContext ?? this.DeviceManager.Direct3DContext; }
            set { _renderContext = value; }
        }

        internal RendererBase()
        {
            Game.ToDispose.Add(this);
            World = Matrix.Identity;
            Show = true;
        }

        /// <summary>
        /// Initialize with the provided deviceManager
        /// </summary>
        /// <param name="deviceManager"></param>
        internal virtual void Initialize(D3DApplicationBase app)
        {
            // If there is a previous device manager, remove event handler
            if (this.DeviceManager != null)
                this.DeviceManager.OnInitialize -= DeviceManager_OnInitialize;
            
            this.DeviceManager = app.DeviceManager;
            // Handle OnInitialize event so that device dependent
            // resources can be reinitialized.
            this.DeviceManager.OnInitialize += DeviceManager_OnInitialize;

            // If there is a previous target, remove event handler
            if (this.Target != null)
                this.Target.OnSizeChanged -= Target_OnSizeChanged;
            
            this.Target = app;
            // Handle OnSizeChanged event so that size dependent
            // resources can be reinitialized.
            this.Target.OnSizeChanged += Target_OnSizeChanged;

            // If the device is already initialized, then create
            // any device resources immediately.
            if (this.DeviceManager.Direct3DDevice != null)
            {
                CreateDeviceDependentResources();
            }
        }

        void DeviceManager_OnInitialize(DeviceManager deviceManager)
        {
            CreateDeviceDependentResources();
        }

        void Target_OnSizeChanged(D3DApplicationBase target)
        {
            CreateSizeDependentResources();
        }

        /// <summary>
        /// Create any resources that depend on the device or device context
        /// </summary>
        protected virtual void CreateDeviceDependentResources()
        {
        }

        /// <summary>
        /// Create any resources that depend upon the size of the render target
        /// </summary>
        protected virtual void CreateSizeDependentResources()
        {
        }

        /// <summary>
        /// Render a frame
        /// </summary>
        internal void Render()
        {
            if (Show)
                DoRender();
        }

        /// <summary>
        /// Each descendant of RendererBase performs a frame
        /// render within the implementation of DoRender
        /// </summary>
        protected abstract void DoRender();

        internal void Render(SharpDX.Direct3D11.DeviceContext context)
        {
            if (Show)
                DoRender(context);
        }

        protected virtual void DoRender(SharpDX.Direct3D11.DeviceContext context)
        {

        }
    }
}
