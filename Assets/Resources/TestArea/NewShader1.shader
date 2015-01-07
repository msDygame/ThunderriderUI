Shader "Custom/FadeOut" {
	Properties  
    {  
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}  
        _Color ("Tint", Color) = (1,1,1,1)  
        _FadeEnd ("Fade End", Float) = 0.8  
        _FadeOrigin ("Fade Origin", Float) = 0.0  
        _FadeStart ("Fade Start", Float) = 0.3  
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0  
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
        Fog { Mode Off }  
        Blend SrcAlpha OneMinusSrcAlpha  
  
        Pass  
        {  
        CGPROGRAM  
// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members screenUV)  
#pragma exclude_renderers d3d11 xbox360  
            #pragma vertex vert  
            #pragma fragment frag  
            #pragma multi_compile DUMMY PIXELSNAP_ON  
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
                half2 texcoord  : TEXCOORD0;  
                float2 screenUV;  
            };  
              
            fixed4 _Color;  
            float _FadeStart;  
            float _FadeEnd;  
            float _FadeOrigin;  
              
            v2f vert(appdata_t IN)  
            {  
                v2f OUT;  
                OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);  
                OUT.texcoord = IN.texcoord;  
                OUT.color = IN.color * _Color;  
                OUT.screenUV = OUT.vertex.xy / OUT.vertex.w;  
                #ifdef PIXELSNAP_ON  
                OUT.vertex = UnityPixelSnap (OUT.vertex);  
                #endif  
  
                return OUT;  
            }  
  
            sampler2D _MainTex;  
  
            fixed4 frag(v2f IN) : COLOR  
            {  
                float a;  
                float4 output_color;  
                float r = _FadeEnd - _FadeStart;  
                a = clamp((abs(IN.screenUV.y - _FadeOrigin) - _FadeStart), 0, r) / r;  
                output_color = (tex2D(_MainTex, IN.texcoord) * IN.color);  
                output_color.a = output_color.a - a;  
                return output_color;  
            }  
        ENDCG  
        }  
    }  
}  