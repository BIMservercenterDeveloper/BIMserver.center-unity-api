/*
BIMserver.center license
This file is part of BIMserver.center IFC frameworks.
Copyright (c) 2017 BIMserver.center
Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files, to use this software with the
purpose of developing new tools for the BIMserver.center platform or interacting
with it.
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

Shader "BIMserver.center Toolkit/Standard"
{
    Properties
    {
        // Main maps.
        _Color("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _MainTex("Albedo", 2D) = "white" {}
        [Enum(AlbedoAlphaMode)] _AlbedoAlphaMode("Albedo Alpha Mode", Float) = 0 // "Transparency"
        [Toggle] _AlbedoAssignedAtRuntime("Albedo Assigned at Runtime", Float) = 0.0
        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
        _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
        _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
        [Toggle(_NORMAL_MAP)] _EnableNormalMap("Enable Normal Map", Float) = 0.0
        [NoScaleOffset] _NormalMap("Normal Map", 2D) = "bump" {}
        _NormalMapScale("Scale", Float) = 1.0
        [Toggle(_EMISSION)] _EnableEmission("Enable Emission", Float) = 0.0
        [HDR]_EmissiveColor("Emissive Color", Color) = (0.0, 0.0, 0.0, 1.0)

        // Rendering options.
        _BlendedClippingWidth("Blended Clipping With", Range(0.0, 10.0)) = 1.0
        [Toggle(_CLIPPING_BORDER)] _ClippingBorder("Clipping Border", Float) = 0.0
        _ClippingBorderWidth("Clipping Border Width", Range(0.0, 1.0)) = 0.025
        _ClippingBorderColor("Clipping Border Color", Color) = (1.0, 0.2, 0.0, 1.0)

        // Fluent options.
        [Toggle(_HOVER_LIGHT)] _HoverLight("Hover Light", Float) = 1.0
        [Toggle(_HOVER_COLOR_OVERRIDE)] _EnableHoverColorOverride("Hover Color Override", Float) = 0.0
        _HoverColorOverride("Hover Color Override", Color) = (1.0, 1.0, 1.0, 1.0)
        [Toggle(_PROXIMITY_LIGHT)] _ProximityLight("Proximity Light", Float) = 0.0
        [Toggle(_PROXIMITY_LIGHT_COLOR_OVERRIDE)] _EnableProximityLightColorOverride("Proximity Light Color Override", Float) = 0.0
        [HDR]_ProximityLightCenterColorOverride("Proximity Light Center Color Override", Color) = (1.0, 0.0, 0.0, 0.0)
        [HDR]_ProximityLightMiddleColorOverride("Proximity Light Middle Color Override", Color) = (0.0, 1.0, 0.0, 0.5)
        [HDR]_ProximityLightOuterColorOverride("Proximity Light Outer Color Override", Color) = (0.0, 0.0, 1.0, 1.0)
        [Toggle(_PROXIMITY_LIGHT_SUBTRACTIVE)] _ProximityLightSubtractive("Proximity Light Subtractive", Float) = 0.0
        [Toggle(_PROXIMITY_LIGHT_TWO_SIDED)] _ProximityLightTwoSided("Proximity Light Two Sided", Float) = 0.0
        _FluentLightIntensity("Fluent Light Intensity", Range(0.0, 1.0)) = 1.0
        _EdgeSmoothingValue("Edge Smoothing Value", Range(0.0, 0.2)) = 0.002

        // Advanced options.
        [Enum(RenderingMode)] _Mode("Rendering Mode", Float) = 0                                     // "Opaque"
        [Enum(CustomRenderingMode)] _CustomMode("Mode", Float) = 0                                   // "Opaque"
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("Source Blend", Float) = 1                 // "One"
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("Destination Blend", Float) = 0            // "Zero"
        [Enum(UnityEngine.Rendering.BlendOp)] _BlendOp("Blend Operation", Float) = 0                 // "Add"
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("Depth Test", Float) = 4                // "LessEqual"
        [Enum(DepthWrite)] _ZWrite("Depth Write", Float) = 1                                         // "On"
        _ZOffsetFactor("Depth Offset Factor", Float) = 0                                             // "Zero"
        _ZOffsetUnits("Depth Offset Units", Float) = 0                                               // "Zero"
        [Enum(UnityEngine.Rendering.ColorWriteMask)] _ColorWriteMask("Color Write Mask", Float) = 15 // "All"
        [Enum(UnityEngine.Rendering.CullMode)] _CullMode("Cull Mode", Float) = 2                     // "Back"
        _RenderQueueOverride("Render Queue Override", Range(-1.0, 5000)) = -1
        [Toggle(_INSTANCED_COLOR)] _InstancedColor("Instanced Color", Float) = 0.0
        [Toggle(_IGNORE_Z_SCALE)] _IgnoreZScale("Ignore Z Scale", Float) = 0.0
        [Toggle(_STENCIL)] _Stencil("Enable Stencil Testing", Float) = 0.0
        _StencilReference("Stencil Reference", Range(0, 255)) = 0
        [Enum(UnityEngine.Rendering.CompareFunction)]_StencilComparison("Stencil Comparison", Int) = 0
        [Enum(UnityEngine.Rendering.StencilOp)]_StencilOperation("Stencil Operation", Int) = 0
    }

    SubShader
    {
        // Extracts information for lightmapping, GI (emission, albedo, ...)
        // This pass it not used during regular rendering.
        Pass
        {
            Name "Meta"
            Tags { "LightMode" = "Meta" }

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #pragma shader_feature _EMISSION

            #include "UnityCG.cginc"
            #include "UnityMetaPass.cginc"

            // This define will get commented in by the UpgradeShaderForLightweightRenderPipeline method.
            //#define _LIGHTWEIGHT_RENDER_PIPELINE

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _MainTex_ST;

            v2f vert(appdata_full v)
            {
                v2f o;
                o.vertex = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

                return o;
            }

            sampler2D _MainTex;

            fixed4 _Color;
            fixed4 _EmissiveColor;

#if defined(_LIGHTWEIGHT_RENDER_PIPELINE)
            CBUFFER_START(_LightBuffer)
            float4 _MainLightPosition;
            half4 _MainLightColor;
            CBUFFER_END
#else
            fixed4 _LightColor0;
#endif

            half4 frag(v2f i) : SV_Target
            {
                UnityMetaInput output;
                UNITY_INITIALIZE_OUTPUT(UnityMetaInput, output);

                output.Albedo = tex2D(_MainTex, i.uv) * _Color;
#if defined(_EMISSION)
                output.Emission += _EmissiveColor;
#endif
#if defined(_LIGHTWEIGHT_RENDER_PIPELINE)
                output.SpecularColor = _MainLightColor.rgb;
#else
                output.SpecularColor = _LightColor0.rgb;
#endif

                return UnityMetaFragment(output);
            }
            ENDCG
        }

        Pass
        {
            Name "Main"
            Tags{ "RenderType" = "Opaque" "LightMode" = "ForwardBase" }
            LOD 100
            Blend[_SrcBlend][_DstBlend]
            BlendOp[_BlendOp]
            ZTest[_ZTest]
            ZWrite[_ZWrite]
            Cull[_CullMode]
            Offset[_ZOffsetFactor],[_ZOffsetUnits]
            ColorMask[_ColorWriteMask]

            Stencil
            {
                Ref[_StencilReference]
                Comp[_StencilComparison]
                Pass[_StencilOperation]
            }

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_instancing
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ _MULTI_HOVER_LIGHT
            #pragma multi_compile _ _CLIPPING_PLANE
            #pragma multi_compile _ _CLIPPING_SPHERE
            #pragma multi_compile _ _CLIPPING_BOX

            #pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON
            #pragma shader_feature _DISABLE_ALBEDO_MAP
            #pragma shader_feature _ _METALLIC_TEXTURE_ALBEDO_CHANNEL_A _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature _NORMAL_MAP
            #pragma shader_feature _EMISSION
            #pragma shader_feature _CLIPPING_BORDER
            #pragma shader_feature _HOVER_LIGHT
            #pragma shader_feature _HOVER_COLOR_OVERRIDE
            #pragma shader_feature _PROXIMITY_LIGHT
            #pragma shader_feature _PROXIMITY_LIGHT_COLOR_OVERRIDE
            #pragma shader_feature _PROXIMITY_LIGHT_SUBTRACTIVE
            #pragma shader_feature _PROXIMITY_LIGHT_TWO_SIDED
            #pragma shader_feature _INSTANCED_COLOR
            #pragma shader_feature _IGNORE_Z_SCALE

            #define IF(a, b, c) lerp(b, c, step((fixed) (a), 0.0)); 

            #include "UnityCG.cginc"
            #include "UnityStandardConfig.cginc"
            #include "UnityStandardUtils.cginc"

            // This define will get commented in by the UpgradeShaderForLightweightRenderPipeline method.
            //#define _LIGHTWEIGHT_RENDER_PIPELINE

            #define _NORMAL

#if defined(_CLIPPING_PLANE) || defined(_CLIPPING_SPHERE) || defined(_CLIPPING_BOX)
            #define _CLIPPING_PRIMITIVE
#else
            #undef _CLIPPING_PRIMITIVE
#endif

#if defined(_NORMAL) || defined(_CLIPPING_PRIMITIVE) || defined(_HOVER_LIGHT) || defined(_PROXIMITY_LIGHT)
            #define _WORLD_POSITION
#else
            #undef _WORLD_POSITION
#endif

#if defined(_ALPHATEST_ON) || defined(_CLIPPING_PRIMITIVE)
            #define _ALPHA_CLIP
#else
            #undef _ALPHA_CLIP
#endif

#if defined(_ALPHABLEND_ON)
            #define _TRANSPARENT
            #undef _ALPHA_CLIP
#else
            #undef _TRANSPARENT
#endif

            #define _FRESNEL

#if !defined(_DISABLE_ALBEDO_MAP) || defined(_NORMAL_MAP) || defined(_DISTANCE_TO_EDGE)
            #define _UV
#else
            #undef _UV
#endif

            struct appdata_t
            {
                float4 vertex : POSITION;
                // The default UV channel used for texturing.
                float2 uv : TEXCOORD0;
#if defined(LIGHTMAP_ON)
                // Reserved for Unity's light map UVs.
                float2 uv1 : TEXCOORD1;
#endif
                // Used for smooth normal data (or UGUI scaling data).
                float4 uv2 : TEXCOORD2;
                // Used for UGUI scaling data.
                float2 uv3 : TEXCOORD3;
                fixed4 color : COLOR0;
                fixed3 normal : NORMAL;
#if defined(_NORMAL_MAP)
                fixed4 tangent : TANGENT;
#endif
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f 
            {
                float4 position : SV_POSITION;
#if defined(_UV)
                float2 uv : TEXCOORD0;
#endif
#if defined(LIGHTMAP_ON)
                float2 lightMapUV : TEXCOORD1;
#endif

                fixed4 color : COLOR0;

#if defined(_WORLD_POSITION)
                float3 worldPosition : TEXCOORD2;
#endif
#if defined(_SCALE)
                float3 scale : TEXCOORD3;
#endif
#if defined(_NORMAL)
#if defined(_NORMAL_MAP)
                fixed3 tangentX : COLOR3;
                fixed3 tangentY : COLOR4;
                fixed3 tangentZ : COLOR5;
#else
                fixed3 worldNormal : COLOR3;
#endif
#endif
                UNITY_VERTEX_OUTPUT_STEREO
#if defined(_INSTANCED_COLOR)
                UNITY_VERTEX_INPUT_INSTANCE_ID
#endif
            };

#if defined(_INSTANCED_COLOR)
            UNITY_INSTANCING_BUFFER_START(Props)
            UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
            UNITY_INSTANCING_BUFFER_END(Props)
#else
            fixed4 _Color;
#endif
            sampler2D _MainTex;
            fixed4 _MainTex_ST;

#if defined(_ALPHA_CLIP)
            fixed _Cutoff;
#endif

            fixed _Metallic;
            fixed _Smoothness;

#if defined(_NORMAL_MAP)
            sampler2D _NormalMap;
            float _NormalMapScale;
#endif

#if defined(_EMISSION)
            fixed4 _EmissiveColor;
#endif

#if defined(_LIGHTWEIGHT_RENDER_PIPELINE)
            CBUFFER_START(_LightBuffer)
            float4 _MainLightPosition;
            half4 _MainLightColor;
            CBUFFER_END
#else
            fixed4 _LightColor0;
#endif

#if defined(_CLIPPING_PLANE)
            fixed _ClipPlaneSide;
            float4 _ClipPlane;
#endif

#if defined(_CLIPPING_SPHERE)
            fixed _ClipSphereSide;
            float4 _ClipSphere;
#endif

#if defined(_CLIPPING_BOX)
            fixed _ClipBoxSide;
            float4 _ClipBoxSize;
            float4x4 _ClipBoxInverseTransform;
#endif

#if defined(_CLIPPING_PRIMITIVE)
            float _BlendedClippingWidth;
#endif

#if defined(_CLIPPING_BORDER)
            fixed _ClippingBorderWidth;
            fixed3 _ClippingBorderColor;
#endif

#if defined(_HOVER_LIGHT)
#if defined(_MULTI_HOVER_LIGHT)
#define HOVER_LIGHT_COUNT 3
#else
#define HOVER_LIGHT_COUNT 1
#endif
#define HOVER_LIGHT_DATA_SIZE 2
            float4 _HoverLightData[HOVER_LIGHT_COUNT * HOVER_LIGHT_DATA_SIZE];
#if defined(_HOVER_COLOR_OVERRIDE)
            fixed3 _HoverColorOverride;
#endif
#endif

#if defined(_PROXIMITY_LIGHT)
#define PROXIMITY_LIGHT_COUNT 2
#define PROXIMITY_LIGHT_DATA_SIZE 6
            float4 _ProximityLightData[PROXIMITY_LIGHT_COUNT * PROXIMITY_LIGHT_DATA_SIZE];
#if defined(_PROXIMITY_LIGHT_COLOR_OVERRIDE)
            float4 _ProximityLightCenterColorOverride;
            float4 _ProximityLightMiddleColorOverride;
            float4 _ProximityLightOuterColorOverride;
#endif
#endif

#if defined(_HOVER_LIGHT) || defined(_PROXIMITY_LIGHT)
            fixed _FluentLightIntensity;
#endif

            static const fixed _MinMetallicLightContribution = 0.7;
            static const fixed _IblContribution = 0.1;

#if defined(_FRESNEL)
            static const float _FresnelPower = 8.0;
#endif

#if defined(_HOVER_LIGHT)
            inline float HoverLight(float4 hoverLight, float inverseRadius, float3 worldPosition)
            {
                return (1.0 - saturate(length(hoverLight.xyz - worldPosition) * inverseRadius)) * hoverLight.w;
            }
#endif

#if defined(_PROXIMITY_LIGHT)
            inline float ProximityLight(float4 proximityLight, float4 proximityLightParams, float4 proximityLightPulseParams, float3 worldPosition, float3 worldNormal, out fixed colorValue)
            {
                float proximityLightDistance = dot(proximityLight.xyz - worldPosition, worldNormal);
#if defined(_PROXIMITY_LIGHT_TWO_SIDED)
                worldNormal = IF(proximityLightDistance < 0.0, -worldNormal, worldNormal);
                proximityLightDistance = abs(proximityLightDistance);
#endif
                float normalizedProximityLightDistance = saturate(proximityLightDistance * proximityLightParams.y);
                float3 projectedProximityLight = proximityLight.xyz - (worldNormal * abs(proximityLightDistance));
                float projectedProximityLightDistance = length(projectedProximityLight - worldPosition);
                float attenuation = (1.0 - normalizedProximityLightDistance) * proximityLight.w;
                colorValue = saturate(projectedProximityLightDistance * proximityLightParams.z);
                float pulse = step(proximityLightPulseParams.x, projectedProximityLightDistance) * proximityLightPulseParams.y;

                return smoothstep(1.0, 0.0, projectedProximityLightDistance / (proximityLightParams.x * max(pow(normalizedProximityLightDistance, 0.25), proximityLightParams.w))) * pulse * attenuation;
            }

            inline fixed3 MixProximityLightColor(fixed4 centerColor, fixed4 middleColor, fixed4 outerColor, fixed t)
            {
                fixed3 color = lerp(centerColor.rgb, middleColor.rgb, smoothstep(centerColor.a, middleColor.a, t));
                return lerp(color, outerColor, smoothstep(middleColor.a, outerColor.a, t));
            }
#endif

#if defined(_CLIPPING_PLANE)
            inline float PointVsPlane(float3 worldPosition, float4 plane)
            {
                float3 planePosition = plane.xyz * plane.w;
                return dot(worldPosition - planePosition, plane.xyz);
            }
#endif

#if defined(_CLIPPING_SPHERE)
            inline float PointVsSphere(float3 worldPosition, float4 sphere)
            {
                return distance(worldPosition, sphere.xyz) - sphere.w;
            }
#endif

#if defined(_CLIPPING_BOX)
            inline float PointVsBox(float3 worldPosition, float3 boxSize, float4x4 boxInverseTransform)
            {
                float3 distance = abs(mul(boxInverseTransform, float4(worldPosition, 1.0))) - boxSize;
                return length(max(distance, 0.0)) + min(max(distance.x, max(distance.y, distance.z)), 0.0);
            }
#endif

            v2f vert(appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
#if defined(_INSTANCED_COLOR)
                UNITY_TRANSFER_INSTANCE_ID(v, o);
#endif
                float4 vertexPosition = v.vertex;

#if defined(_WORLD_POSITION)
                float3 worldVertexPosition = mul(unity_ObjectToWorld, vertexPosition).xyz;
#endif

#if defined(_SCALE)
                o.scale.x = length(mul(unity_ObjectToWorld, float4(1.0, 0.0, 0.0, 0.0)));
                o.scale.y = length(mul(unity_ObjectToWorld, float4(0.0, 1.0, 0.0, 0.0)));
#if defined(_IGNORE_Z_SCALE)
                o.scale.z = o.scale.x;
#else
                o.scale.z = length(mul(unity_ObjectToWorld, float4(0.0, 0.0, 1.0, 0.0)));
#endif
#endif

                fixed3 localNormal = v.normal;

#if defined(_NORMAL)
                fixed3 worldNormal = UnityObjectToWorldNormal(localNormal);
#endif

                o.position = UnityObjectToClipPos(vertexPosition);

#if defined(_WORLD_POSITION)
                o.worldPosition.xyz = worldVertexPosition;
#endif

#if defined(_UV)
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
#endif

#if defined(LIGHTMAP_ON)
                o.lightMapUV.xy = v.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#endif

                o.color = v.color;

#if defined(_NORMAL)
#if defined(_NORMAL_MAP)
                fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
                fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
                fixed3 worldBitangent = cross(worldNormal, worldTangent) * tangentSign;
                o.tangentX = fixed3(worldTangent.x, worldBitangent.x, worldNormal.x);
                o.tangentY = fixed3(worldTangent.y, worldBitangent.y, worldNormal.y);
                o.tangentZ = fixed3(worldTangent.z, worldBitangent.z, worldNormal.z);
#else
                o.worldNormal = worldNormal;
#endif
#endif

                return o;
            }

            fixed4 frag(v2f i, fixed facing : VFACE) : SV_Target
            {
#if defined(_INSTANCED_COLOR)
                UNITY_SETUP_INSTANCE_ID(i);
#endif

            // Texturing.
#if defined(_DISABLE_ALBEDO_MAP)
                fixed4 albedo = fixed4(1.0, 1.0, 1.0, 1.0);
#else
                fixed4 albedo = tex2D(_MainTex, i.uv);
#endif

#ifdef LIGHTMAP_ON
                albedo.rgb *= DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lightMapUV));
#endif

#if defined(_METALLIC_TEXTURE_ALBEDO_CHANNEL_A)
                _Metallic = albedo.a;
                albedo.a = 1.0;
#elif defined(_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A)
                _Smoothness = albedo.a;
                albedo.a = 1.0;
#endif 

                // Primitive clipping.
#if defined(_CLIPPING_PRIMITIVE)
                float primitiveDistance = 1.0;
#if defined(_CLIPPING_PLANE)
                primitiveDistance = min(primitiveDistance, PointVsPlane(i.worldPosition.xyz, _ClipPlane) * _ClipPlaneSide);
#endif
#if defined(_CLIPPING_SPHERE)
                primitiveDistance = min(primitiveDistance, PointVsSphere(i.worldPosition.xyz, _ClipSphere) * _ClipSphereSide);
#endif
#if defined(_CLIPPING_BOX)
                primitiveDistance = min(primitiveDistance, PointVsBox(i.worldPosition.xyz, _ClipBoxSize.xyz, _ClipBoxInverseTransform) * _ClipBoxSide);
#endif
#if defined(_CLIPPING_BORDER)
                fixed3 primitiveBorderColor = lerp(_ClippingBorderColor, fixed3(0.0, 0.0, 0.0), primitiveDistance / _ClippingBorderWidth);
                albedo.rgb += primitiveBorderColor * IF((primitiveDistance < _ClippingBorderWidth), 1.0, 0.0);
#endif
#endif

#if defined(_DISTANCE_TO_EDGE)
                fixed2 distanceToEdge;
                distanceToEdge.x = abs(i.uv.x - 0.5) * 2.0;
                distanceToEdge.y = abs(i.uv.y - 0.5) * 2.0;
#endif

#if defined(_INSTANCED_COLOR)
                albedo *= UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
#else
                albedo *= _Color;
#endif

                albedo *= i.color;

                // Normal calculation.
#if defined(_NORMAL)
                fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPosition.xyz));

                fixed3 worldNormal;

#if defined(_NORMAL_MAP)
                fixed3 tangentNormal = UnpackScaleNormal(tex2D(_NormalMap, i.uv), _NormalMapScale);
                worldNormal.x = dot(i.tangentX, tangentNormal);
                worldNormal.y = dot(i.tangentY, tangentNormal);
                worldNormal.z = dot(i.tangentZ, tangentNormal);
                worldNormal = normalize(worldNormal) * facing;
#else
                worldNormal = normalize(i.worldNormal) * facing;
#endif
#endif

                fixed pointToLight = 1.0;
                fixed3 fluentLightColor = fixed3(0.0, 0.0, 0.0);

                // Hover light.
#if defined(_HOVER_LIGHT)
                pointToLight = 0.0;

                [unroll]
                for (int hoverLightIndex = 0; hoverLightIndex < HOVER_LIGHT_COUNT; ++hoverLightIndex)
                {
                    int dataIndex = hoverLightIndex * HOVER_LIGHT_DATA_SIZE;
                    fixed hoverValue = HoverLight(_HoverLightData[dataIndex], _HoverLightData[dataIndex + 1].w, i.worldPosition.xyz);
                    pointToLight += hoverValue;
#if !defined(_HOVER_COLOR_OVERRIDE)
                    fluentLightColor += lerp(fixed3(0.0, 0.0, 0.0), _HoverLightData[dataIndex + 1].rgb, hoverValue);
#endif
                }
#if defined(_HOVER_COLOR_OVERRIDE)
                fluentLightColor = _HoverColorOverride.rgb * pointToLight;
#endif
#endif

                // Proximity light.
#if defined(_PROXIMITY_LIGHT)
#if !defined(_HOVER_LIGHT)
                pointToLight = 0.0;
#endif
                [unroll]
                for (int proximityLightIndex = 0; proximityLightIndex < PROXIMITY_LIGHT_COUNT; ++proximityLightIndex)
                {
                    int dataIndex = proximityLightIndex * PROXIMITY_LIGHT_DATA_SIZE;
                    fixed colorValue;
                    fixed proximityValue = ProximityLight(_ProximityLightData[dataIndex], _ProximityLightData[dataIndex + 1], _ProximityLightData[dataIndex + 2], i.worldPosition.xyz, worldNormal, colorValue);
                    pointToLight += proximityValue;
#if defined(_PROXIMITY_LIGHT_COLOR_OVERRIDE)
                    fixed3 proximityColor = MixProximityLightColor(_ProximityLightCenterColorOverride, _ProximityLightMiddleColorOverride, _ProximityLightOuterColorOverride, colorValue);
#else
                    fixed3 proximityColor = MixProximityLightColor(_ProximityLightData[dataIndex + 3], _ProximityLightData[dataIndex + 4], _ProximityLightData[dataIndex + 5], colorValue);
#endif  
#if defined(_PROXIMITY_LIGHT_SUBTRACTIVE)
                    fluentLightColor -= lerp(fixed3(0.0, 0.0, 0.0), proximityColor, proximityValue);
#else
                    fluentLightColor += lerp(fixed3(0.0, 0.0, 0.0), proximityColor, proximityValue);
#endif    
                }
#endif    

#if defined(_ALPHA_CLIP)
#if !defined(_ALPHATEST_ON)
                _Cutoff = 0.5;
#endif
#if defined(_CLIPPING_PRIMITIVE)
                albedo *= (primitiveDistance > 0.0);
#endif
                clip(albedo.a - _Cutoff);
                albedo.a = 1.0;
#endif

                // Blinn phong lighting.
#if defined(_LIGHTWEIGHT_RENDER_PIPELINE)
                float4 directionalLightDirection = _MainLightPosition;
#else
                float4 directionalLightDirection = _WorldSpaceLightPos0;
#endif
                fixed diffuse = max(0.0, dot(worldNormal, directionalLightDirection));

                fixed specular = 0.0;

                fixed3 ibl = unity_IndirectSpecColor.rgb;

                // Fresnel lighting.
#if defined(_FRESNEL)
                fixed fresnel = 1.0 - saturate(abs(dot(worldViewDir, worldNormal)));
                fixed3 fresnelColor = unity_IndirectSpecColor.rgb * (pow(fresnel, _FresnelPower) * max(_Smoothness, 0.5));
#endif
                // Final lighting mix.
                fixed4 output = albedo;
                fixed3 ambient = glstate_lightmodel_ambient + fixed3(0.25, 0.25, 0.25);
                fixed minProperty = min(_Smoothness, _Metallic);
                fixed oneMinusMetallic = (1.0 - _Metallic);
                output.rgb = lerp(output.rgb, ibl, minProperty);
#if defined(_LIGHTWEIGHT_RENDER_PIPELINE)
                fixed3 directionalLightColor = _MainLightColor.rgb;
#else
                fixed3 directionalLightColor = _LightColor0.rgb;
#endif
                output.rgb *= lerp((ambient + directionalLightColor * diffuse + directionalLightColor * specular) * max(oneMinusMetallic, _MinMetallicLightContribution), albedo, minProperty);
                output.rgb += (directionalLightColor * albedo * specular) + (directionalLightColor * specular * _Smoothness);
                output.rgb += ibl * oneMinusMetallic * _IblContribution;

#if defined(_FRESNEL)
                output.rgb += fresnelColor;
#endif

#if defined(_EMISSION)
                output.rgb += _EmissiveColor;
#endif

#if defined(_NEAR_PLANE_FADE)
                output *= i.worldPosition.w;
#endif

                // Hover and proximity lighting should occur after near plane fading.
#if defined(_HOVER_LIGHT) || defined(_PROXIMITY_LIGHT)
                output.rgb += fluentLightColor * _FluentLightIntensity * pointToLight;
#endif

                // Perform non-alpha clipped primitive clipping on the final output.
#if defined(_CLIPPING_PRIMITIVE) && !defined(_ALPHA_CLIP)
                output *= saturate(primitiveDistance * (1.0f / _BlendedClippingWidth));
#endif

                return output;
            }

            ENDCG
        }
    }
    
    Fallback "Standard"
	CustomEditor "BIMservercenter.Toolkit.Internal.Shaders.BIMServerCenterStandardShaderGUI"
}
