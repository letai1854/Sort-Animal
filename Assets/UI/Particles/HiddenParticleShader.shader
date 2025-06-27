Shader "UI/Particles/Hidden" // Quan trọng: Tên này phải khớp với Shader.Find() trong script
{
    Properties
    {
        // Không cần properties vì nó chỉ ẩn ParticleSystemRenderer
    }
    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque" } // Hoặc "Transparent" nếu bạn muốn nó ở queue sau
        Cull Off Lighting Off ZWrite Off Fog { Mode Off }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            v2f vert ()
            {
                v2f o;
                o.vertex = fixed4(0, 0, 0, 0); // Vẽ ra một điểm vô hình
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                discard; // Không vẽ pixel nào
                return fixed4(0, 0, 0, 0);
            }
            ENDCG
        }
    }
}