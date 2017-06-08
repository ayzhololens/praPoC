Shader "Unlit/Texture Colored" {
    Properties {
        _ColorAdd ("Add Color", Color) = (0, 0, 0, 0)      
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }

    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100
        
        Pass {
            Lighting Off
            
            SetTexture [_MainTex] { 
                // Sets our color as the 'constant' variable
                constantColor [_ColorAdd]
                
                // Multiplies color (in constant) with texture
                combine constant + texture
            } 
        }
    }
}
