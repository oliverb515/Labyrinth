float alpha;

float4 FlashEffect(float2 texCoord: TEXCOORD0) : COLOR
{

	float4 color = float4(alpha, alpha, alpha, alpha);
	return color;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 FlashEffect();
    }
}
