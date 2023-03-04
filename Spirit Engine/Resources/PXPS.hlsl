Texture2D Texture0 : register(t0);
SamplerState Sampler : register(s0);

#include "Common.hlsl"

float4 PSMain(PixelShaderInput pixel) : SV_Target
{
    if (HasTexture)
        return Texture0.Sample(Sampler, pixel.TextureUV);

    return pixel.Diffuse;
}