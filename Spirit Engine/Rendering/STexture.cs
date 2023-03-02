using Realsphere.Spirit.RenderingCommon;
using SharpDX.Direct2D1;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.WIC;
using System.IO;
using SharpDX.DXGI;
using SharpDX.IO;
using System;

namespace Realsphere.Spirit.Rendering
{
    public class STexture : SDisposable
    {
        internal ShaderResourceView texture;
        internal SharpDX.Direct2D1.Bitmap d2dtext;
        internal SamplerState sampler;

        public static STexture FromBase64(string b64)
            => Load(Convert.FromBase64String(b64));

        public static STexture Load(string fileName)
        {
            STexture st = new STexture();

            st.texture = ShaderResourceView.FromFile(Game.deviceManager.Direct3DDevice, fileName);
            ImagingFactory imagingFactory = new ImagingFactory();
            NativeFileStream fileStream = new NativeFileStream(fileName,
                NativeFileMode.Open, NativeFileAccess.Read);

            BitmapDecoder bitmapDecoder = new BitmapDecoder(imagingFactory, fileStream, DecodeOptions.CacheOnDemand);
            BitmapFrameDecode frame = bitmapDecoder.GetFrame(0);

            FormatConverter converter = new FormatConverter(imagingFactory);
            converter.Initialize(frame, SharpDX.WIC.PixelFormat.Format32bppPRGBA);

            st.d2dtext = SharpDX.Direct2D1.Bitmap1.FromWicBitmap(Game.deviceManager.Direct2DContext, converter);
            imagingFactory.Dispose();
            fileStream.Dispose();
            bitmapDecoder.Dispose();
            frame.Dispose();
            converter.Dispose();

            st.sampler = new SamplerState(Game.deviceManager.Direct3DDevice, new SamplerStateDescription()
            {
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp,
                AddressW = TextureAddressMode.Clamp,
                Filter = SharpDX.Direct3D11.Filter.MinMagMipLinear,
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
                Filter = SharpDX.Direct3D11.Filter.MinMagMipLinear
            });

            ImagingFactory imagingFactory = new ImagingFactory();
            MemoryStream stream = new(data);

            BitmapDecoder bitmapDecoder = new BitmapDecoder(imagingFactory, stream, DecodeOptions.CacheOnDemand);
            BitmapFrameDecode frame = bitmapDecoder.GetFrame(0);

            FormatConverter converter = new FormatConverter(imagingFactory);
            converter.Initialize(frame, SharpDX.WIC.PixelFormat.Format32bppPRGBA);

            st.d2dtext = SharpDX.Direct2D1.Bitmap1.FromWicBitmap(Game.deviceManager.Direct2DContext, converter);
            imagingFactory.Dispose();
            stream.Dispose();
            bitmapDecoder.Dispose();
            frame.Dispose();
            converter.Dispose();

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
        public override void SDispose()
        {
            d2dtext.Dispose();
            texture.Dispose();
            sampler.Dispose();
        }
    }
}
