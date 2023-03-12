Texture2D ReflectionTexture : register(t0);
SamplerState ReflectionSampler : register(s0);

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

float3 Reflect(float3 i, float3 n) {
    return i - 2 * dot(i, n) * n;
}

float4 PSMain(PixelShaderInput pixel) : SV_Target
{
    // Sample the texture
    float4 texColor;
    // Calculate diffuse light
    float3 toLight = normalize(-Light.Direction);
    float3 diffuse = Lambert(MaterialDiffuse, pixel.WorldNormal, toLight) * Light.Color.rgb;

    // Calculate specular light
    float3 toEye = normalize(CameraPosition - pixel.WorldPosition);
    float3 specular = SpecularBlinnPhong(pixel.WorldNormal, toLight, toEye) * Light.Color.rgb;

    // Calculate reflection
    float3 reflected = Reflect(-toEye, pixel.WorldNormal);
    float3 reflectionColor = ReflectionTexture.Sample(ReflectionSampler, reflected.xy).rgb;

    // Calculate final color
    float4 result = (float4(MaterialAmbient.rgb, 1.0f) + float4(diffuse, 1.0f)) + float4(specular, 1.0f);
    result.rgb += reflectionColor * MaterialSpecularPower;

    // Add emissive color
    result.rgb += MaterialEmissive.rgb;

    return result;
}