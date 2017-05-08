Shader "Legacy Shaders/Transparent/Cutout/DiffuseParti" {
	Properties{
		_TintColor("Tint Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
	}

Category{
			Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" }
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask RGB
			Cull Off

		SubShader {
			LOD 200

		CGPROGRAM
		#pragma surface surf Lambert alphatest:_Cutoff

		sampler2D _MainTex;
		fixed4 _TintColor;

		struct Input {
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _TintColor;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
		}
}
		
Fallback "Legacy Shaders/Transparent/Cutout/VertexLit"
}
