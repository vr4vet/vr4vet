Shader "Custom/RoundedRectShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", COLOR) = (.54, .95, .99, 0.5)
		[ShowAsVector2] _Size("_Size", VECTOR) = (256,256,0,0)
	}
		SubShader
		{
			// No culling or depth
			Tags { "RenderType" = "Opaque" "Queue" = "Transparent" }
			Cull Off ZWrite Off ZTest LEqual
			Blend SrcAlpha OneMinusSrcAlpha


			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				uniform float4 _Color;
				uniform float4 _Size;

				bool ignore(float2 coords, float2 dimensions, float radius)
				{
					float2 circle_center = float2(radius, radius);

					if (length(coords - circle_center) > radius
						&& coords.x < circle_center.x && coords.y < circle_center.y) return true; //first circle

					circle_center.x += dimensions.x - 2 * radius;

					if (length(coords - circle_center) > radius
						&& coords.x > circle_center.x && coords.y < circle_center.y) return true; //second circle

					circle_center.y += dimensions.y - 2 * radius;

					if (length(coords - circle_center) > radius
						&& coords.x > circle_center.x && coords.y > circle_center.y) return true; //third circle

					circle_center.x -= dimensions.x - 2 * radius;

					if (length(coords - circle_center) > radius
						&& coords.x < circle_center.x && coords.y > circle_center.y) return true; //fourth circle

					return false;

				}

				sampler2D _MainTex;
				float4 _MainTex_TexelSize;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					UNITY_TRANSFER_FOG(o, o.vertex);
					return o;
				}

				fixed4 frag(v2f input) : SV_Target
				{
					float2 coord = input.uv * _Size.xy;
					if (ignore(coord, _Size.xy, 1))
						discard;

					float4 col = _Color;
					col.a = _Color.w;
					return col;
				}
				ENDCG
			}
		}
}
