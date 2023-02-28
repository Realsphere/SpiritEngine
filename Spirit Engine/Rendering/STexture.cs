using Realsphere.Spirit.RenderingCommon;
using SharpDX.Direct3D11;
using System.IO;

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
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp,
                AddressW = TextureAddressMode.Clamp,
                Filter = Filter.MinMagMipLinear,
                ComparisonFunction = Comparison.Never,
                MaximumAnisotropy = 16,
                MipLodBias = 0.0f,
                MinimumLod = 0,
                MaximumLod = 16,
            });

            return st;
        }
        public static STexture Load(byte[] data)
        {
            STexture st = new STexture();

            MemoryStream str = new(data);
            st.texture = ShaderResourceView.FromStream(Game.deviceManager.Direct3DDevice, str, data.Length);
            str.Dispose();
            st.sampler = new SamplerState(Game.deviceManager.Direct3DDevice, new SamplerStateDescription()
            {
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                AddressW = TextureAddressMode.Wrap,
                Filter = Filter.MinMagMipLinear
            });

            return st;
        }

        /// <summary>
        /// !!!WARNING!!! Do not use this constructor, only use STexture.Load, else rendering any object that is using this Texture in its Material, will throw an exception!
        /// </summary>
        public STexture() { }

        internal void apply(RendererBase renderer)
        {
            renderer.samplerState = sampler;
            renderer.texture = texture;
        }
    }
}
