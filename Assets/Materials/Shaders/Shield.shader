Shader "Unlit/Shield"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR]_FresnelColor ("Fresnel Color", Color) = (1,1,1,1)
        _FresnelPower ("Fresnel Power", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _FresnelColor;
            fixed _FresnelPower;
            
            v2f vert (appdata v)
            {
                v2f o;
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex) ;
                o.normal = UnityObjectToWorldNormal(v.normal);
                // o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.worldPos = 0.5 * pow(1 + v.normal, _FresnelPower);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv + float2(0, _Time.y * 0.5));
                col = lerp(col, _FresnelColor, 1 - i.worldPos);
                return col;
            }
            ENDCG
        }
    }
}
