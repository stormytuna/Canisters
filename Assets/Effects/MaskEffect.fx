sampler uImage0 : register(s0);
sampler uImage1 : register(s1)
{
    AddressU = Wrap;
    AddressV = Wrap;
    Filter = Point;
};

float2 position;
float2 screenResolution;

float4 Mask(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    if (any(color)) {
        float2 parallaxCoords = (coords + (position * 0.1)) * screenResolution;
        parallaxCoords = (parallaxCoords - 1) * 40 + 1;
        return tex2D(uImage1, parallaxCoords);
    }

    return color;
}

technique Technique1
{
    pass MaskPass
    {
        PixelShader = compile ps_3_0 Mask();
    }
};