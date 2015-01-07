Shader "Custom/ColorMask" {
	Properties {
	}
	SubShader {
	Pass{
		ColorMask A
	}
	} 
	FallBack "Diffuse"
}
