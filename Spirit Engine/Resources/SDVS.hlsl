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
    bool HasTexture;
    float4 MaterialEmissive;
    float2 UVTransform;
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
}

PixelShaderInput VSMain(VertexShaderInput vertex)
{
    PixelShaderInput result = (PixelShaderInput)0;

    result.Position = mul(vertex.Position, WorldViewProjection);
    result.Diffuse = vertex.Color * MaterialDiffuse;
    result.TextureUV = vertex.TextureUV;

    result.WorldNormal = mul(vertex.Normal, (float3x3)WorldInverseTranspose);
    
    result.WorldPosition = mul(vertex.Position, World).xyz;
    
    return result;
}