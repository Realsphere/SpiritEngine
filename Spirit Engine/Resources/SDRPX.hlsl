Texture2D reflectionTexture : register(t0);
SamplerState Sampler : register(s0);

cbuffer PerObject : register(b0)
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
    float4 MaterialEmissive;
};

struct VertexShaderInput
{
    float4 Position : SV_Position;
    float3 Normal : NORMAL;
};

struct PixelShaderInput
{
    float4 Position : SV_Position;
    float3 WorldNormal : NORMAL;
    float3 WorldPosition : WORLDPOS;
};

float3 Reflect(float3 i, float3 n) {
    return i - 2 * dot(i, n) * n;
}

float3 ReconstructWorldPosition(float2 screenPosition, float depth)
{
    float4 clipSpacePosition = float4(screenPosition, depth, 1.0f);
    float4 worldPosition = mul(clipSpacePosition, ViewProjectionInverse);
    return worldPosition.xyz / worldPosition.w;
}

float3 ReconstructWorldNormal(float2 screenPosition)
{
    float3 normal = reflectionTexture.Sample(Sampler, screenPosition).xyz * 2 - 1;
    return normalize(mul(normal, (float3x3)WorldInverseTranspose));
}

float4 PSMain(PixelShaderInput pixel) : SV_Target
{
    float2 screenPosition = pixel.Position.xy / pixel.Position.w;
    float depth = pixel.Position.z / pixel.Position.w;

    float3 worldNormal = ReconstructWorldNormal(screenPosition);
    float3 worldPosition = ReconstructWorldPosition(screenPosition, depth);

    float3 reflectedPosition = Reflect(CameraPosition - worldPosition, worldNormal);
    float2 reflectedScreenPosition = (mul(float4(reflectedPosition, 1), WorldViewProjection)).xy / (mul(float4(reflectedPosition, 1), WorldViewProjection)).w;

    float4 reflectedColor = reflectionTexture.Sample(Sampler, reflectedScreenPosition);
    return reflectedColor;
}