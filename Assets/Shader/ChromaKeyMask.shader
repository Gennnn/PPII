Shader "UI/ChromaKeyMask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ChromaColor ("Chroma Color", Color) = (0,0,0,1)
        _Threshold ("Threshold", Float) = 0.01
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _ChromaColor;
            float _Threshold;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float diff = distance(col.rgb, _ChromaColor.rgb);

                if (diff < _Threshold)
                    discard;

                return col;
            }
            ENDCG
        }
    }
}
