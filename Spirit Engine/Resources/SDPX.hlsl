Texture2D Texture0 : register(t0);
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

    float3 color = float3(0, 0, 0);
    float alpha = 1.0f;

    if (HasTexture)
    {
        color = (saturate(ambient + diffuse) * sample.rgb + specular) * Light.Color.rgb;
        alpha = sample.a;
    }
    else
    {
        color = (saturate(ambient + diffuse) * sample.rgb + specular) * Light.Color.rgb + emissive;
        alpha = pixel.Diffuse.a;
    }

    return float4(color, alpha);
}