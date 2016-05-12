Shader "Custom/Outline_2DSprite" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_OutLineSpreadX("Outline Spread X", Range(0,0.03)) = 0.007
		_OutLineSpreadY("Outline Spread Y", Range(0,0.03)) = 0.007
		_Color("Outline Color", Color) = (1.0,1.0,1.0,1.0)
	}
		SubShader{
		Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
	}

		Pass{

		Cull Off
		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM

#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"


			sampler2D _MainTex;
		float _OutLineSpreadX;
		float _OutLineSpreadY;
		float4 _Color;

		struct fragmentInput {
			float4 pos : SV_POSITION;
			float2 uv : TEXTCOORD0;
		};

	fragmentInput vert(appdata_base v)
	{
		fragmentInput o;

		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord.xy;

		return o;
	}

	fixed4 frag(fragmentInput IN) : SV_Target{
		fixed4 TempColor = tex2D(_MainTex, IN.uv + float2(_OutLineSpreadX,0.0)) + tex2D(_MainTex, IN.uv - float2(_OutLineSpreadX,0.0));
	TempColor = TempColor + tex2D(_MainTex, IN.uv + float2(0.0,_OutLineSpreadY)) + tex2D(_MainTex, IN.uv - float2(0.0,_OutLineSpreadY));
	//if (TempColor.a > 0.1) {
	//	TempColor.a = 1;
	//}
	fixed4 AlphaColor = fixed4(TempColor.a, TempColor.a, TempColor.a, TempColor.a);
	fixed4 mainColor = AlphaColor * _Color.rgba;
	fixed4 addcolor = tex2D(_MainTex, IN.uv);

	if (addcolor.a > 0.95) {
		mainColor = addcolor;
	}

	return fixed4(mainColor.r, mainColor.g, mainColor.b, mainColor.a);
	}


		ENDCG
	}
	}
}