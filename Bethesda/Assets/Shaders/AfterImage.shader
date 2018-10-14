Shader "Custom/AfterImage"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_TintColor ("Tint Color", Color) = (1,1,1,1)
		_TintFactor ("Tint Factor", Float) = 0
		_Opacity ("Opacity", Float) = 1
	}
	SubShader
	{
		Tags {"Queue"="Geometry" "IgnoreProjector"="True" "RenderType"="Transparent" }
	
		LOD 100

		/*Pass
		{
			ZWrite On
			ColorMask 0

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : COLOR
			{
				return 0;
			}

			ENDCG
		}*/

		Pass
		{
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;

			};

			sampler2D _MainTex;
			fixed4 _MainTex_ST;
			fixed4 _Color;
			fixed4 _TintColor;
			half _TintFactor;
			fixed _Opacity;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = lerp(tex2D(_MainTex, i.uv) * _Color, _TintColor, _TintFactor);
				col.a = _Opacity;
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
