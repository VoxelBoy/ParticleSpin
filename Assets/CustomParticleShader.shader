//Adapted from Particles/Additive shader

Shader "Particles/CustomParticleShader" {
Properties {
    _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
}

SubShader {
        Pass {
        	Tags { "LightMode"="Deferred" "RenderType"="Opaque" }
        	ZWrite On

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #pragma target 3.0
            #pragma exclude_renderers nomrt
            #pragma shader_feature _EMISSION

            #include "UnityCG.cginc"
            #include "UnityGBuffer.cginc"

            fixed4 _TintColor;

            struct appdata_t {
                float4 vertex : POSITION;
                half4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                half4 color : COLOR;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                return o;
            }

            void frag (v2f i,
					    out half4 outGBuffer0 : SV_Target0,
					    out half4 outGBuffer1 : SV_Target1,
					    out half4 outGBuffer2 : SV_Target2,
					    out half4 outEmission : SV_Target3)
            {
	            outGBuffer0 = 1;
		        outGBuffer1 = 1;
		        outGBuffer2 = 0;
		        
		        UnityStandardData data;
			    data.diffuseColor   = 0;
			    data.occlusion      = 1;
			    data.specularColor  = 0;
			    data.smoothness     = 0;
			    data.normalWorld    = 0;

			    UnityStandardDataToGbuffer(data, outGBuffer0, outGBuffer1, outGBuffer2);
		        
                half4 col = 2.0f * i.color * _TintColor;
                outEmission = col;
            }
            ENDCG
        }
    }
}
