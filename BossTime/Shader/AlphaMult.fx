sampler2D AlphaTexture : register(s0);
sampler2D DiffuseTexture : register(s1);

float4 main(float2 texCoord : TEXCOORD0) : COLOR
{
	float4 color = tex2D(DiffuseTexture, texCoord);
	color.w *= tex2D(AlphaTexture, texCoord).w;
	return color;
}