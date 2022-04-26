Shader "UnityCommunity/Sprites/SpriteDropShadow"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		_ShadowColor ("Shadow", Color) = (0,0,0,1)
		_ShadowOffset ("ShadowOffset", Vector) = (0,-0.1,0,0)
		_Blur ("Blur", Float) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;
			fixed4 _ShadowColor;
			float4 _ShadowOffset;
			float2 _MainTex_TexelSize;
			float _Blur;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex+_ShadowOffset);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color *_ShadowColor;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);
				color.rgb = _ShadowColor.rgb;

				#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;
				#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
				c.rgb *= c.a;

				return c;
			}
		ENDCG
		}
		
		GrabPass{
			"_ShadowTex"
		}

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;
			fixed4 _ShadowColor;
			float4 _ShadowOffset;
			float2 _MainTex_TexelSize;
			float _Blur;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex+_ShadowOffset);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color *_ShadowColor;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			sampler2D _ShadowTex;
			float _AlphaSplitEnabled;

			float gaussian(int x, int y)
			{
				float sigmaSqu = 5 * 5;
				const float pi = 3.141592653589793;
				return (1 / sqrt(2 * pi * sigmaSqu)) * exp(-((x * x) + (y * y)) / (2 * sigmaSqu));
			}

			fixed4 blur(sampler2D tex, float2 uv) 
			{
				const int boxSize = 5;
				const int iter = int((boxSize - 1.0f)/2.0f);
				fixed4 color = 0;
				color.a = 0;
				float normalizer = 1.0f/float(boxSize*boxSize);

				float kernelSum = 0.0;
				for (int i = -iter; i < -iter + boxSize; i++) {
					for (int j = -iter; j < -iter + boxSize; j++) {
						float gauss = gaussian(i, j);
						kernelSum += gauss;

						fixed2 offset = fixed2(i, j);
						color.a += gauss * tex2D(_ShadowTex, uv + offset);
					}
				}
				color.a /= kernelSum;
				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				// fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
				fixed4 c = blur(_MainTex, IN.texcoord);
				// c.rgb *= c.a;
				// c.a = tex2D(_MainTex, IN.texcoord).a;

				return c;
			}
		ENDCG
		}

		// draw real sprite
		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

				#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;
				#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
}
