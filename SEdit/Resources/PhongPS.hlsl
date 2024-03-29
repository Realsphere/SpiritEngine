Texture2D Texture0 : register(t0);
SamplerState Sampler : register(s0);

#include "Common.hlsl"

float4 PSMain(PixelShaderInput pixel) : SV_Target
{
    float3 normal = normalize(pixel.WorldNormal);
    float3 toEye = normalize(CameraPosition - pixel.WorldPosition);
    float3 toLight = normalize(-Light.Direction);

    float4 sample = (float4)1.0f;
    if (HasTexture)
        sample = Texture0.Sample(Sampler, pixel.TextureUV);

    float3 ambient = MaterialAmbient.rgb;
    float3 emissive = MaterialEmissive.rgb;
    float3 diffuse = Lambert(pixel.Diffuse, normal, toLight);
    float3 specular = SpecularPhong(normal, toLight, toEye);

    float3 color = (saturate(ambient+diffuse) * sample.rgb + specular) * Light.Color.rgb + emissive;
    float alpha = pixel.Diffuse.a * sample.a;

    return float4(color, alpha);

}