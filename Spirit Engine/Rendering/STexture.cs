using Realsphere.Spirit.RenderingCommon;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct2D1;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.WIC;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using Realsphere.Spirit.SceneManagement;
using Realsphere.Spirit.Internal;
using System.Text;
using System.Threading.Tasks;

namespace Realsphere.Spirit.Rendering
{
    public class STexture
    {
        internal ShaderResourceView texture;
        internal SharpDX.Direct2D1.Bitmap d2dtext;
        internal SamplerState sampler;

        public static STexture Load(string fileName)
        {
            STexture st = new STexture();

            st.texture = ShaderResourceView.FromFile(Game.deviceManager.Direct3DDevice, fileName);
            st.sampler = new SamplerState(Game.deviceManager.Direct3DDevice, new SamplerStateDescription()
            {
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                AddressW = TextureAddressMode.Wrap,
                Filter = SharpDX.Direct3D11.Filter.MinMagMipLinear
            });

            return st;
        }

        /// <summary>
        /// !!!WARNING!!! Do not use this constructor, only use STexture.Load, else rendering any object that is using this Texture in its Material, will throw an exception!
        /// </summary>
        public STexture() { }

        internal void apply(MeshRenderer renderer)
        {
            renderer.samplerState = sampler;
            renderer.texture = texture;
        }
    }
}
