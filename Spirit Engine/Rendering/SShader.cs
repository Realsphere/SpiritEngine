using Realsphere.Spirit.Exceptions;
using Realsphere.Spirit.RenderingCommon;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realsphere.Spirit.Rendering
{
    public enum SInputElementFormat
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
        R32_Float_X8X24_Typeless,
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

    /// <summary>
    /// Shader Input Element
    /// </summary>
    public struct SInputElement
    {
        internal InputElement value;

        public SInputElement(string name, int index, SInputElementFormat format, int offset, int slot)
        {
            value = new InputElement(name, index, (SharpDX.DXGI.Format)format, offset, slot);
        }
    }

    /// <summary>
    /// One of the only ways to directly interact with the rendering pipeline, by using shaders.
    /// </summary>
    public class SShader : IDisposable
    {
        internal VertexShader vs;
        internal PixelShader pixel;
        internal InputLayout vl;
        internal List<SInputElement> ie;

        public void Dispose()
        {
            vs.Dispose();
            pixel.Dispose();
        }

        /// <summary>
        /// Sets the Pixel and Vertex shader of the rendering pipeline to the Shader.
        /// </summary>
        public void UseForRendering()
        {
            Game.deviceManager.Direct3DContext.PixelShader.Set(pixel);
            Game.deviceManager.Direct3DContext.VertexShader.Set(vs);
        }


        /// <summary>
        /// Sets the Pixel and Vertex shader of the rendering pipeline to the default Shaders.
        /// </summary>
        public void StopUsingForRendering()
        {
            Game.deviceManager.Direct3DContext.PixelShader.Set(Game.app.phongShader);
            Game.deviceManager.Direct3DContext.VertexShader.Set(Game.app.vertexShader);
        }

        /// <summary>
        /// Check for the Documentation at http://realsphere.org/spirit/docs/shaders.html#using_hlsl_to_create_a_custom_shader
        /// </summary>
        public SShader(string shaderCode, string vertexShaderMainName, string pixelShaderMainName, List<SInputElement> vertexShaderInputElements, string vertexShaderMainProfile = "vs_5_0", string pixelShaderMainProfile = "ps_5_0")
        {
            if (Game.app == null) throw new NotSupportedException("It is not supported loading shaders, without having the game started.\nPlease start the Game first!");

            string hlslCode = shaderCode.Replace("%spiritHLSLcommons", @"cbuffer PerObject : register(b0)
{
    float4x4 WorldViewProjection;

    float4x4 World;

    float4x4 WorldInverseTranspose;
};

struct DirectionalLight
{
    float4 Color;
    float3 Direction;
};

cbuffer PerFrame: register (b1)
{
    DirectionalLight Light;
    float3 CameraPosition;
};

cbuffer PerMaterial : register (b2)
{
    float4 MaterialAmbient;
    float4 MaterialDiffuse;
    float4 MaterialSpecular;
    float MaterialSpecularPower;
    bool HasTexture;
    float4 MaterialEmissive;
    float4x4 UVTransform;
};

struct VertexShaderInput
{
    float4 Position : SV_Position;
    float3 Normal : NORMAL;
    float4 Color : COLOR0;
    float2 TextureUV: TEXCOORD0;
};

struct PixelShaderInput
{
    float4 Position : SV_Position;
    float4 Diffuse : COLOR;
    float2 TextureUV: TEXCOORD0;

    float3 WorldNormal : NORMAL;
    float3 WorldPosition : WORLDPOS;
};

float3 Lambert(float4 pixelDiffuse, float3 normal, float3 toLight)
{
    float3 diffuseAmount = saturate(dot(normal, toLight));
    return pixelDiffuse.rgb * diffuseAmount;
}

float3 SpecularPhong(float3 normal, float3 toLight, float3 toEye)
{
    float3 reflection = reflect(-toLight, normal);

    float specularAmount = pow(saturate(dot(reflection, toEye)), max(MaterialSpecularPower, 0.00001f));
    return MaterialSpecular.rgb * specularAmount;
}

float3 SpecularBlinnPhong(float3 normal, float3 toLight, float3 toEye)
{
    float3 halfway = normalize(toLight + toEye);

    float specularAmount = pow(saturate(dot(normal, halfway)), max(MaterialSpecularPower, 0.00001f));
    return MaterialSpecular.rgb * specularAmount;
}");
            using (var vertexShaderBytecode = HLSLCompiler.CompileFromCode(hlslCode, vertexShaderMainName, vertexShaderMainProfile))
            {
                if (vertexShaderBytecode == null || vertexShaderBytecode.GetPart(ShaderBytecodePart.InputSignatureBlob) == null) throw new SpiritDXShaderException("The vertex shader is corrupted.\nPlease read the documentation at: http://realsphere.org/spirit/docs/shaders.html!");
                vs = new VertexShader(Game.deviceManager.Direct3DDevice, vertexShaderBytecode);
                ie = vertexShaderInputElements;
                InputElement[] sie = new InputElement[ie.Count];
                int i = 0;
                vertexShaderInputElements.ForEach(iev =>
                {
                    sie[i] = iev.value;
                    i++;
                });
                var inputSignatureBlob = vertexShaderBytecode.GetPart(ShaderBytecodePart.InputSignatureBlob);
                vl = new InputLayout(Game.deviceManager.Direct3DDevice,
                    inputSignatureBlob,
                    sie);
            }
            using (var bytecode = HLSLCompiler.CompileFromCode(hlslCode, pixelShaderMainName, pixelShaderMainProfile))
                pixel = new PixelShader(Game.deviceManager.Direct3DDevice, bytecode);
        }
    }
}
