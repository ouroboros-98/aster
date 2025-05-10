Shader "Custom/WobbleShader"
{
    Properties
    {
        _Speed ("Speed", Float) = 1.0
        _Intensity ("Intensity", Float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            float _Speed;
            float _Intensity;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                float wobble = sin(_Time.y * _Speed + v.vertex.x + v.vertex.z) * _Intensity;
                v.vertex.y += wobble;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                return float4(1,1,1,1);
            }
            ENDCG
        }
    }
}