﻿Shader "Custom/FogOfWar" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		FogRadius("FogRadius", Float) = 1.0
		_FogMaxRadius("FogMaxRadius", Float) = 0.5
		Player1("Player1", Vector) = (0,0,0,1)
		Player2("Player2", Vector) = (0,0,0,1)
		Player3("Player3", Vector) = (0,0,0,1)
		Player4("Player4", Vector) = (0,0,0,1)
	}

		SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off

		CGPROGRAM
#pragma surface surf Lambert vertex:vert alpha:blend

		sampler2D _MainTex;
	fixed4 	_Color;
	float 	FogRadius;
	float 	_FogMaxRadius;
	float4 	Player1;
	float4  Player2;
	float4 	Player3;
	float4  Player4;

	struct Input {
		float2 uv_MainTex;
		float2 location;
	};

	float powerForPos(float4 pos, float2 nearVertex);

	void vert(inout appdata_full vertexData, out Input outData) {
		float4 pos = UnityObjectToClipPos(vertexData.vertex);
		float4 posWorld = mul(unity_ObjectToWorld, vertexData.vertex);
		outData.uv_MainTex = vertexData.texcoord;
		outData.location = posWorld.xz;
	}

	void surf(Input IN, inout SurfaceOutput o) {
		fixed4 baseColor = tex2D(_MainTex, IN.uv_MainTex) * _Color;

		float alpha = (1.0 - (baseColor.a + powerForPos(Player1, IN.location) + powerForPos(Player2, IN.location) + powerForPos(Player3, IN.location) + powerForPos(Player4, IN.location)));

		o.Albedo = baseColor.rgb;
		o.Alpha = alpha;
	}

	//return 0 if (pos - nearVertex) > _FogRadius
	float powerForPos(float4 pos, float2 nearVertex) {
		float atten = clamp(FogRadius - length(pos.xz - nearVertex.xy), 0.0, FogRadius);

		return (1.0 / _FogMaxRadius)*atten / FogRadius;
	}

	ENDCG
	}

		Fallback "Transparent/VertexLit"
}