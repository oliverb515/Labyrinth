uniform extern texture ScreenTexture;

int num_lights;
float2 lightPos;
float intensity;

sampler ScreenS = sampler_state
{
    Texture = <ScreenTexture>;
};


float distanceSq(float2 a, float2 b) {
	float y=(720.0f/1280.0f);
	return ((a[0] - b[0])*(a[0] - b[0]) + y*y*(a[1] - b[1])*(a[1] - b[1]));
};

float4 myShader(float2 texCoord: TEXCOORD0) : COLOR
{
	
	float distance = distanceSq(texCoord, lightPos);

	if (distance < 0) {
		distance = -distance;
	}
	if (distance > 1.0f) distance = 1.0f;
	distance *= intensity;
	float4 color = float4(0, 0, 0, distance);
	return color;
}

float4 menuShader(float2 texCoord: TEXCOORD0) : COLOR
{
	
	float distance = distanceSq(texCoord, lightPos);

	if (distance > 1.0f) distance = 1.0f;
	distance *= intensity;
	float4 color = float4(0, 0, 0, distance);
	return color;
}

technique Technique1
{
    pass Pass0
    {
        PixelShader = compile ps_2_0 myShader();
    }
	pass Pass1
	{
		PixelShader = compile ps_2_0 myShader();
	}
}