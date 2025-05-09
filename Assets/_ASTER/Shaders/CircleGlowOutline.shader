Shader "Custom/CircleGlowOutline"
{
    Properties
    {
        _OutlineColor   ("Outline Color",   Color)        = (1,0,0,1)
        _OutlineWidth   ("Outline Width",   Range(0.005,0.2)) = 0.02
        _Falloff        ("Outline Falloff", Range(0,0.1))    = 0.005
        _GlowWidth      ("Glow Width",      Range(0.01,0.5)) = 0.1
        _GlowIntensity  ("Glow Intensity",  Range(0,5))      = 1.5
        _Radius        ("Circle Radius", Range(0.0,2.0)) = 0.8
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f     { float2 uv : TEXCOORD0; float4 pos : SV_POSITION; };

            fixed4 _OutlineColor;
            float  _OutlineWidth, _Falloff, _GlowWidth, _GlowIntensity, _Radius;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                float shrink = 1.0 - (_GlowWidth + _OutlineWidth + _Falloff);
                o.uv = (v.uv * 2 - 1) * shrink * _Radius;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float dist = length(i.uv);
                float edgeD = abs(dist - 1.0);
                float outlineA = 1.0 - smoothstep(_OutlineWidth - _Falloff, _OutlineWidth + _Falloff, edgeD);
                float glowA = (1.0 - smoothstep(_OutlineWidth, _OutlineWidth + _GlowWidth, edgeD)) * _GlowIntensity;
                float alpha = max(outlineA, glowA);
                return fixed4(_OutlineColor.rgb, alpha);
            }
            ENDCG
        }
    }
}