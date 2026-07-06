Shader "Unlit/Ether"
{

	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BgBottomColor ("BgBottomColor", Color) = (0.0,0.0,0.0,1.0)
		_BgTopColor ("BgTopColor", Color) = (0.0,0.0,0.0,1.0)
		_EtherColor1 ("EtherColor1", Color) = (0.0,0.0,0.0,1.0)
		_EtherColor2 ("EtherColor2", Color) = (0.0,0.0,0.0,1.0)
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" }
		LOD 100

		Pass
		{
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.0
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color: COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : POSITION;
				float4 color: COLOR;
			};

			// sampler2D _MainTex;
			// float4 _MainTex_ST;
            float4 _BgBottomColor;
            float4 _BgTopColor;
            float4 _EtherColor1;
            float4 _EtherColor2;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.color = v.color;
				// o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

            float2x2 m(float a)
            {
                float c=cos(a), s=sin(a);
                return float2x2(c,-s,s,c);
            }
            
            float map(float3 p)
            {
                float t = _Time.y*0.5;
                p.xz = mul(p.xz,m(t*0.4));
                p.xy = mul(p.xy,m(t*0.3));
                float3 q = p*2.+t;
                float s = sin(t*0.7);
                return length(p+float3(s,s,s))*log(length(p)+1.) + sin(q.x+sin(q.z+sin(q.y)))*0.5 - 1.;
            }
            
            float4 mainImage( float2 fragCoord)
            {
                float2 p = fragCoord.xy/_ScreenParams.y - float2(.9,.5);
                float3 cl = float3(0.,0.,0.);
                float d = 2.5;
                for(int i=0; i<=5; i++)	{
                    float3 p2 = float3(0,0,5.) + normalize(float3(p.x,p.y, -1.))*d;
                    float rz = map(p2);
                    float f =  clamp((rz - map(p2+.1))*0.5, -.1, 1. );
					// float3 l = float3(0.1,0.3,.4) + float3(5., 2.5, 3.)*f;
                    float3 l = float3(_EtherColor1.r,_EtherColor1.g,_EtherColor1.b) + float3(_EtherColor2.r,_EtherColor2.g,_EtherColor2.b)*f;
                    cl = cl*l + (1.-smoothstep(0., 2.5, rz))*.7*l;
                    d += min(rz, 1.);
                }
                return float4(cl, 1.);
            }

			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				// fixed4 col = tex2D(_MainTex, i.uv);
                float2 uv = i.vertex.xy / _ScreenParams.xy;
                
                // Bottom to TopRight
                float4 bgCol = lerp(uv.x * _BgTopColor, uv.y * _BgBottomColor, uv.y);
                
                // Bottom to Top
				// float4 bgCol = lerp(_BgTopColor, _BgBottomColor, uv.y);
                
                float4 retVal = (bgCol + mainImage(i.vertex)) * float4(1.,1.,1.,i.color.a);
				return retVal;
			}
            ENDCG
		}
	}
}
