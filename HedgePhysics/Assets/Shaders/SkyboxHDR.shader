// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Volund/Skybox Following Sun" {
Properties {
	_Tex ("Cubemap", Cube) = "white" {}
	_Exposure ("Exposure", Range(0,4)) = 1.0

	_Rotation ("Rotation", Range(0,6.28)) = 0

	_TintDay ("Sky Tint Day", Color) = (1,1,1,1)
	_TintNight ("Sky Tint Night", Color)  = (.5,.156,0,1)
	_SkyExponent ("Sky Gradient", Float) = 2.0
}

SubShader {
	Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
	Cull Off ZWrite Off Fog { Mode Off }

	Pass {
		
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma only_renderers d3d11 d3d9 opengl glcore

		#include "UnityCG.cginc"
		#include "Lighting.cginc"

		samplerCUBE _Tex;
		half4 _Tex_HDR;
		half _Exposure;
		float _Rotation;
		half4 _TintDay;
		half4 _TintNight;
		half _SkyExponent;
		
		struct appdata_t {
			float4 vertex : POSITION;
			float3 texcoord : TEXCOORD0;
		};

		struct v2f {
			float4 vertex : SV_POSITION;
			float3 texcoord : TEXCOORD0;
			half3 color : COLOR0;
		};

		v2f vert (appdata_t v)
		{
			v2f o;
			float s, c;

			half3 sunDir = normalize(_WorldSpaceLightPos0.xyz);

			float rotation = _Rotation;
			rotation += atan2 (sunDir.x, -sunDir.z);

			sincos(rotation, s, c);
			float2x2 m = float2x2(c, -s, s, c);
			o.vertex = float4(mul(m, v.vertex.xz), v.vertex.yw).xzyw;
			o.vertex = UnityObjectToClipPos(o.vertex);
			o.texcoord = v.texcoord;
			o.color = lerp (_TintDay.rgb, _TintNight.rgb, saturate(pow(1-sunDir.y, _SkyExponent)));
			if (length (_LightColor0.rgb) <= 1e-3) // HACK: detecting if we render into defualt-reflecion probe here
				o.color = _TintDay.rgb;
			return o;
		}

		fixed4 frag (v2f i) : SV_Target
		{
			fixed4 tex = texCUBE (_Tex, i.texcoord);
			tex.rgb = DecodeHDR (tex, _Tex_HDR);
			tex.rgb *= i.color * _Exposure;
			tex.a = 1;
			return tex;
		}
		ENDCG 
	}
} 	


Fallback Off

}
