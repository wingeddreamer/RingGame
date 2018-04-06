Shader "Custom/ClockShader" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Specular ("Specular (RGBA)", 2D) = "black" {}
		_SpecularVal ("SpecularV",Range(0,3))=1.0
		_SmoothnessVal("SmoothnessV",Range(0,1)) = 1.0
		_Normal ("Normal (RGB)" , 2D) = "black" {}
		_Emission("Emission (RGB)" , 2D) = "black" {}
		_EmissionPenalty("Emission Penalty (RGB)" , 2D) = "black" {}
		_EmissionVal("EmissionV",Range(0,3)) = 1.0
		_TimeLeft("TimeLeft(0-1)",Range(0,1)) = 1.0
		_Penalty("PenaltyPos(0-1)",Range(0,1)) = 1.0

	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf StandardSpecular fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _Specular;
		sampler2D _Normal;
		sampler2D _Emission;
		sampler2D _EmissionPenalty;
		float _SpecularVal;
		float _SmoothnessVal;
		float _EmissionVal;
		float _TimeLeft;
		float _Penalty;

		struct Input {
			float2 uv_MainTex;
		};


		void surf (Input IN, inout SurfaceOutputStandardSpecular o) {
			fixed2 newUV = fixed2(IN.uv_MainTex.x*4, IN.uv_MainTex.y);
			o.Albedo = tex2D(_MainTex, newUV);
			o.Specular = tex2D(_Specular, newUV)*_SpecularVal;
			o.Smoothness = tex2D(_Specular, newUV).a*_SmoothnessVal;
			o.Normal = UnpackNormal(tex2D(_Normal, newUV));
			if((1-IN.uv_MainTex.x)<_TimeLeft)
				o.Emission = tex2D(_Emission, newUV) * 2.5;
			else if ((1 - IN.uv_MainTex.x)>_TimeLeft && (1 - IN.uv_MainTex.x)<_Penalty)
				o.Emission = tex2D(_EmissionPenalty, newUV);
			else
				o.Emission = tex2D(_Emission, newUV)*_EmissionVal;

		}
		ENDCG
	}
	FallBack "Diffuse"
}
