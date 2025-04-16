Shader "Outlined/Uniform"
{
	Properties
	{
		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Texture", 2D) = "white" {}
		_OutlineColor ("Outline color", Color) = (0,0,0,1)
		_OutlineWidth ("Outlines width", Range (0.0, 2.0)) = 1.1
        _LineWidth("Line Width", float) = 10.0
        _CellSizeX("Cell Size X", float) = 1.5
        _CellSizeY("Cell Size Y", float) = 1.5
        _MoveSpeed("Move Speed", float) = 0.1
        _XSlant("X Slant", float) = 0.0
        _YSlant("Y Slant", float) = 0.0
        _MaximumLuminance("Max Luminance", float) = 0.25
        _MinimumLuminance("Min Luminance", float) = 0.0 
        _CurvedXOffset("Curved X Offset", int) = 0
        _CurvedYOffset("Curved Y Offset", int) = 0
        _CurveFrequencyX("Curve Frequency X", float) = 5.0
        _CurveFrequencyY("Curve Frequency Y", float) = 5.0
        _CurveMagnitudeX("Curve Magnitude X", float) = 0.01
        _CurveMagnitudeY("Curve Magnitude Y", float) = 0.01
        _PulseFrequency("Pulse Frequency", float) = 0.8
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	struct appdata
	{
		float4 vertex : POSITION;
	};

	struct v2f
	{
		float4 pos : POSITION;
	};

	uniform float _OutlineWidth;
	uniform float4 _OutlineColor;
	uniform sampler2D _MainTex;
	uniform float4 _Color;
    

	ENDCG

	SubShader
	{
        
		Pass //Outline
		{
			Name "Outline"
		    Tags{ "Queue" = "Geometry" "RenderType"="Opaque" "IgnoreProjector" = "True" }
            Cull Front
            ZWrite On
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			v2f vert(appdata v)
			{
				appdata original = v;
				v.vertex.xyz += _OutlineWidth * normalize(v.vertex.xyz);

				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;

			}

			half4 frag(v2f i) : COLOR
			{
				return _OutlineColor;
			}

			ENDCG
		}
        Pass 
        {
            Tags { "Queue"="Geometry" "RenderType"="Opaque" }
            ZWrite On
            ZTest LEqual
            Cull Back
            Blend Off
            Name "Grid"
            CGINCLUDE
            #include "UnityCG.cginc"
            float2 _MainTex_TexelSize;
            sampler2D _CameraDepthTexture;

            

            struct MeshData //per-vertex mesh data
            {
                float4 vertex : POSITION; //vertex position
                float2 uv : TEXCOORD0; //uv coordinates
            };

            struct Interpolators
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD1; // Add screenPos
            };
            float3 Sample(float2 uv) {
                return tex2D(_MainTex, uv).rgb;
            }

            float3 SampleBox(float2 uv, float delta) {
                float4 o = _MainTex_TexelSize.xyxy * float2(-delta, delta).xxyy;
                float3 s = Sample(uv + o.xy) + Sample(uv + o.zy) + Sample(uv + o.xw) + Sample(uv + o.zw);

                return s * 0.25f;
            }
            Interpolators vert (MeshData v)
            {
                Interpolators o;
                float4 clipPos = UnityObjectToClipPos(v.vertex);
                o.vertex = clipPos;
                o.uv = ComputeScreenPos(clipPos).xy / clipPos.w;
                o.screenPos = ComputeScreenPos(clipPos);
                return o;
            }
        ENDCG


        Tags { "Queue"="Geometry+1" "RenderType"="Opaque" }
        ZWrite Off
        ZTest LEqual
        Blend Off
        //Blend One One
        

            //Blend One One
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #define TAU 6.28318530718    

            float _LineWidth;
            int _LineFrequency;
            float _CellSizeX;
            float _CellSizeY;
            float _MoveSpeed;
            float _YSlant;
            float _XSlant;
            float _MaximumLuminance;
            float _MinimumLuminance;
            int _CurvedXOffset;
            int _CurvedYOffset;
            float _CurveFrequencyX;
            float _CurveFrequencyY;
            float _CurveMagnitudeX;
            float _CurveMagnitudeY;
            float _PulseFrequency;
            float3 _GlowColor;
            float _RotationX;
            float _RotationY;
            float _Perspective;
            int _UseDepth;
            float _DepthAggression;
            int _InvertTimeForX;
            int _InvertTimeForY;
            float _GridXScale;
            float _GridYScale;
            float _GridXOffset;
            float _GridYOffset;
            float _GlowIntensity;
            float _GlowFalloff;

            fixed4 frag (Interpolators i) : SV_Target
            {
                float2 uv = i.uv;
                float2 screenUV = i.screenPos.xy / i.screenPos.w;
                float rawDepth = tex2D(_CameraDepthTexture, screenUV).r;
                float sceneDepth = LinearEyeDepth(rawDepth);
                float fragDepth = i.screenPos.z;

                if (fragDepth > sceneDepth + 0.0001)
                    discard;
                uv.x = (uv.x - 0.5) / _GridXScale + 0.5 + _GridXOffset;
                uv.y = (uv.y - 0.5) / _GridYScale + 0.5 + _GridYOffset;
                if (uv.x < 0 || uv.x > 1 || uv.y < 0 || uv.y > 1) {
                    return fixed4(0,0,0,0);
                }

                // Convert degrees to radians
                float radX = radians(_RotationX);
                float radY = radians(_RotationY);

                // Map 2D UVs to 3D space (assuming the grid is on the XY plane at Z = 0)
                float3 pos = float3((uv.x - 0.5) * 2.0, (uv.y - 0.5) * 2.0, 0.0);

                // Rotation around X-axis
                float3x3 rotX = float3x3(
                    1, 0, 0,
                    0, cos(radX), -sin(radX),
                    0, sin(radX), cos(radX)
                );

                // Rotation around Y-axis
                float3x3 rotY = float3x3(
                    cos(radY), 0, sin(radY),
                    0, 1, 0,
                    -sin(radY), 0, cos(radY)
                );

                // Apply rotations
                pos = mul(rotX, pos); // Rotate around X
                pos = mul(rotY, pos); // Rotate around Y

                // Perspective correction (simulates depth)
                float perspective = 1.0 / (1.0 + pos.z * _Perspective);
                float depth = saturate((1.0 - _DepthAggression + (pos.z))/(perspective*(1.0/(1.0-_DepthAggression))));
                if (_UseDepth < 1)
                    depth = 1.0f;
                pos.xy *= perspective; 

                //float depthAlpha = saturate(1.0 - abs(pos.z) * 0.5); // Closer = more opaque, further = transparent

                // Remap back to UV space
                uv.x = (pos.x + 1.0) * 0.5;
                uv.y = (pos.y + 1.0) * 0.5;
                
                

                //uv = mul(rotX, uv); // Rotate around X axis
                //uv = mul(rotY, uv); // Rotate around Y axis

                float xOffset = i.uv.y * _XSlant;
                if (_CurvedXOffset > 0)
                    xOffset = cos(i.uv.y * TAU * _CurveFrequencyX) * _CurveMagnitudeX;

                float yOffset = i.uv.x * _YSlant;
                if (_CurvedYOffset > 0)
                    yOffset = cos(i.uv.x * TAU * _CurveFrequencyY) * _CurveMagnitudeY;

                
                uv.x = _InvertTimeForX == 1 ? fmod((uv.x - (_Time.y * _MoveSpeed) + xOffset), _CellSizeX) : fmod((uv.x + (_Time.y * _MoveSpeed) + xOffset), _CellSizeX);
                uv.y = _InvertTimeForY == 1 ? fmod((uv.y - (_Time.y * _MoveSpeed)  + yOffset), _CellSizeY) : fmod((uv.y + (_Time.y * _MoveSpeed)  + yOffset), _CellSizeY);

                uv.x = smoothstep(_LineWidth, _LineWidth, abs(uv.x));
                uv.y = smoothstep(_LineWidth, _LineWidth, abs(uv.y));

                return ( 1 - uv.x * uv.y) * (cos(_Time.y * _PulseFrequency) * 0.5 + 0.5 + _MinimumLuminance) * float4(_GlowColor, 1.0f) * _MaximumLuminance * depth;


            }
            ENDCG
        }

		
	}
	Fallback "Diffuse"
}