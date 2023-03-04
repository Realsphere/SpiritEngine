#include "Common.hlsl"

float4 PSMain(PixelShaderInput pixel) : SV_Target
{
    float4 output = float4(pixel.Position.z, 0, 0, 1);
    return output;
}