using Realsphere.Spirit.Exceptions;
using Realsphere.Spirit.RenderingCommon;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realsphere.Spirit.Rendering
{
    public enum SIEFormat
    {
        Unknown,
        R32G32B32A32_Typeless,
        R32G32B32A32_Float,
        R32G32B32A32_UInt,
        R32G32B32A32_SInt,
        R32G32B32_Typeless,
        R32G32B32_Float,
        R32G32B32_UInt,
        R32G32B32_SInt,
        R16G16B16A16_Typeless,
        R16G16B16A16_Float,
        R16G16B16A16_UNorm,
        R16G16B16A16_UInt,
        R16G16B16A16_SNorm,
        R16G16B16A16_SInt,
        R32G32_Typeless,
        R32G32_Float,
        R32G32_UInt,
        R32G32_SInt,
        R32G8X24_Typeless,
        D32_Float_S8X24_UInt,
        
        Float_X8X24_Typeless,
        X32_Typeless_G8X24_UInt,
        R10G10B10A2_Typeless,
        R10G10B10A2_UNorm,
        R10G10B10A2_UInt,
        R11G11B10_Float,
        R8G8B8A8_Typeless,
        R8G8B8A8_UNorm,
        R8G8B8A8_UNorm_SRgb,
        R8G8B8A8_UInt,
        R8G8B8A8_SNorm,
        R8G8B8A8_SInt,
        R16G16_Typeless,
        R16G16_Float,
        R16G16_UNorm,
        R16G16_UInt,
        R16G16_SNorm,
        R16G16_SInt,
        R32_Typeless,
        D32_Float,
        R32_Float,
        R32_UInt,
        R32_SInt,
        R24G8_Typeless,
        D24_UNorm_S8_UInt,
        R24_UNorm_X8_Typeless,
        X24_Typeless_G8_UInt,
        R8G8_Typeless,
        R8G8_UNorm,
        R8G8_UInt,
        R8G8_SNorm,
        R8G8_SInt,
        R16_Typeless,
        R16_Float,
        D16_UNorm,
        R16_UNorm,
        R16_UInt,
        R16_SNorm,
        R16_SInt,
        R8_Typeless,
        R8_UNorm,
        R8_UInt,
        R8_SNorm,
        R8_SInt,
        A8_UNorm,
        R1_UNorm,
        R9G9B9E5_Sharedexp,
        R8G8_B8G8_UNorm,
        G8R8_G8B8_UNorm,
        BC1_Typeless,
        BC1_UNorm,
        BC1_UNorm_SRgb,
        BC2_Typeless,
        BC2_UNorm,
        BC2_UNorm_SRgb,
        BC3_Typeless,
        BC3_UNorm,
        BC3_UNorm_SRgb,
        BC4_Typeless,
        BC4_UNorm,
        BC4_SNorm,
        BC5_Typeless,
        BC5_UNorm,
        BC5_SNorm,
        B5G6R5_UNorm,
        B5G5R5A1_UNorm,
        B8G8R8A8_UNorm,
        B8G8R8X8_UNorm,
        R10G10B10_Xr_Bias_A2_UNorm,
        B8G8R8A8_Typeless,
        B8G8R8A8_UNorm_SRgb,
        B8G8R8X8_Typeless,
        B8G8R8X8_UNorm_SRgb,
        BC6H_Typeless,
        BC6H_Uf16,
        BC6H_Sf16,
        BC7_Typeless,
        BC7_UNorm,
        BC7_UNorm_SRgb,
        AYUV,
        Y410,
        Y416,
        NV12,
        P010,
        P016,
        Opaque420,
        YUY2,
        Y210,
        Y216,
        NV11,
        AI44,
        IA44,
        P8,
        A8P8,
        B4G4R4A4_UNorm
    }

    
    public struct SInputElement
    {
        public string Name;
        public int Index;
        public SIEFormat Format;
        public int Offset;
        public int Slot;

        public SInputElement(string name, int index, SIEFormat format, int offset, int slot)
        {
            Slot = slot;
            Name = name; 
            Index = index; 
            Format = format; 
            Offset = offset;
        }
    }

    
    public class SShader : SDisposable
    {
        public static SShader SimpleDiffuse
        {
            get
            {
                return new()
                {
                    vs = Game.app.simpleDiffuseVS,
                    layout = Game.app.vertexLayout,
                    ps = Game.app.simpleDiffusePS
                };
            }
        }

        public static SShader DefaultShader
        {
            get
            {
                return new()
                {
                    vs = Game.app.vertexShader,
                    layout = Game.app.vertexLayout,
                    ps = Game.app.phongShader
                };
            }
        }

        internal VertexShader vs;
        internal PixelShader ps;
        internal InputLayout layout;

        /// <summary>
        /// Compiles a DirectX HLSL Shader.
        /// </summary>
        public static SShader Compile(string shaderCode, string vertexShaderFunc, string vertexShaderProfile, string pixelShaderFunc, string pixelShaderProfile, SInputElement[] inputElements)
        {
            SShader ss = new();
            var device = Game.deviceManager.Direct3DDevice;
            using (var vertexShaderBytecode = HLSLCompiler.CompileFromCode(shaderCode, vertexShaderFunc, vertexShaderProfile))
            {
                List<InputElement> inputElements1 = new List<InputElement>();

                inputElements1.AddRange(new[]
                {
                    new InputElement("SV_Position", 0, Format.R32G32B32_Float, 0, 0),
                    new InputElement("NORMAL", 0, Format.R32G32B32_Float, 12, 0),
                    new InputElement("COLOR", 0, Format.R8G8B8A8_UNorm, 24, 0),
                    new InputElement("TEXCOORD", 0, Format.R32G32_Float, 28, 0),
                });

                foreach (var ie in inputElements)
                    inputElements1.Add(new InputElement(ie.Name, ie.Index, (Format)ie.Format, ie.Offset, ie.Slot));

                ss.vs = new VertexShader(device, vertexShaderBytecode);
                // Layout from VertexShader input signature
                ss.layout = new InputLayout(device, vertexShaderBytecode.GetPart(ShaderBytecodePart.InputSignatureBlob), inputElements1.ToArray());
            }

            using (var bytecode = HLSLCompiler.CompileFromCode(shaderCode, pixelShaderFunc, pixelShaderProfile))
                ss.ps = new PixelShader(device, bytecode);

            return ss;
        }

        /// <summary>
        /// Sets the Shader as the active Shader for rendering.
        /// </summary>
        public void Use()
        {
            var context = Game.deviceManager.Direct3DContext;
            context.InputAssembler.InputLayout = layout;
            context.VertexShader.Set(vs);
            context.PixelShader.Set(ps);
        }

        /// <summary>
        /// Sets the default Shader as the active Shader for rendering.
        /// </summary>
        public void Unuse()
        {
            var context = Game.deviceManager.Direct3DContext;
            context.InputAssembler.InputLayout = Game.app.vertexLayout;
            context.VertexShader.Set(Game.app.vertexShader);
            context.PixelShader.Set(Game.app.phongShader);
        }

        public override void SDispose()
        {
            Unuse();
            layout.Dispose();
            vs.Dispose();
            ps.Dispose();
        }
    }
}
