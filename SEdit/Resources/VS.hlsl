#include "Common.hlsl"

PixelShaderInput VSMain(VertexShaderInput vertex)
{
    PixelShaderInput result = (PixelShaderInput)0;

    result.Position = mul(vertex.Position, WorldViewProjection);
    result.Diffuse = vertex.Color * MaterialDiffuse;
    result.TextureUV = mul(float4(vertex.TextureUV.x, vertex.TextureUV.y, 0, 1), (float4x2)UVTransform).xy;

    result.WorldNormal = mul(vertex.Normal, (float3x3)WorldInverseTranspose);
    
    result.WorldPosition = mul(vertex.Position, World).xyz;
    
    return result;
}