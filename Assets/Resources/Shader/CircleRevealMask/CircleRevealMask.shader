Shader "MyShader/CircleRevealMask"
{
	Properties
	{
		
	}
	SubShader
	{
		Tags 
		{
			"Queue" = "Overlay"
			"RenderType" = "Transparent"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask RGBA

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
			};

			float2 _Center;
			float _Radius;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{	
				float2 currentScreenPos = i.vertex.xy;
				float distance = length(currentScreenPos - _Center);
				float alpha = 255;
				if (distance < _Radius)
				{
					alpha = alpha - (_Radius - distance);
				}
				alpha = alpha/255;
				if (alpha < 0) alpha = 0;
				fixed4 col = fixed4(0, 0, 0, alpha);
				return col;
			}
			ENDCG
		}
	}
}
