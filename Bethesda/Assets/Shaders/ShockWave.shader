Shader "Custom/ShockWave"
{
	Properties
	{
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "Queue"="Transparent+100" }
		LOD 100

		ZWrite Off

		GrabPass
		{
			"_ShockWaveGrabTexture"
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 grabPos : TEXCOORD0;
				float4 localPos : TEXCOORD1;
			};

			sampler2D _ShockWaveGrabTexture;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.grabPos = ComputeGrabScreenPos(o.vertex);
				o.localPos = v.vertex;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float4 uv = i.grabPos;
				uv.y += 0.1f;
				fixed4 screenCol = tex2Dproj(_ShockWaveGrabTexture, uv);
				return screenCol;
			}
			ENDCG
		}
	}
}
