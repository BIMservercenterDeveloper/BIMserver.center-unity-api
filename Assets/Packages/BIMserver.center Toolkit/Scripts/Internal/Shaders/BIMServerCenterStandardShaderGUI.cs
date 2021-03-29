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

using Object = UnityEngine.Object;
using UnityEngine.Rendering;
using UnityEditor;
using UnityEngine;
using System.IO;
using System;

namespace BIMservercenter.Toolkit.Internal.Shaders
{
#if UNITY_EDITOR
    public class BIMServerCenterStandardShaderGUI : BIMServerCenterShaderGUI
    {
        protected enum AlbedoAlphaMode
        {
            Transparency,
            Metallic,
            Smoothness
        }

        protected static class Styles
        {
            public static string primaryMapsTitle = "Main Maps";
            public static string renderingOptionsTitle = "Rendering Options";
            public static string advancedOptionsTitle = "Advanced Options";
            public static string fluentOptionsTitle = "Fluent Options";
            public static string instancedColorName = "_InstancedColor";
            public static string instancedColorFeatureName = "_INSTANCED_COLOR";
            public static string stencilComparisonName = "_StencilComparison";
            public static string stencilOperationName = "_StencilOperation";
            public static string disableAlbedoMapName = "_DISABLE_ALBEDO_MAP";
            public static string albedoMapAlphaMetallicName = "_METALLIC_TEXTURE_ALBEDO_CHANNEL_A";
            public static string albedoMapAlphaSmoothnessName = "_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A";
            public static string propertiesComponentHelp = "Use the {0} component(s) to control {1} properties.";
            public static readonly string[] albedoAlphaModeNames = Enum.GetNames(typeof(AlbedoAlphaMode));
            public static GUIContent instancedColor = new GUIContent("Instanced Color", "Enable a Unique Color Per Instance");
            public static GUIContent albedo = new GUIContent("Albedo", "Albedo (RGB) and Transparency (Alpha)");
            public static GUIContent albedoAssignedAtRuntime = new GUIContent("Assigned at Runtime", "As an optimization albedo operations are disabled when no albedo texture is specified. If a albedo texture will be specified at runtime enable this option.");
            public static GUIContent alphaCutoff = new GUIContent("Alpha Cutoff", "Threshold for Alpha Cutoff");
            public static GUIContent metallic = new GUIContent("Metallic", "Metallic Value");
            public static GUIContent smoothness = new GUIContent("Smoothness", "Smoothness Value");
            public static GUIContent enableNormalMap = new GUIContent("Normal Map", "Enable Normal Map");
            public static GUIContent normalMap = new GUIContent("Normal Map");
            public static GUIContent normalMapScale = new GUIContent("Scale", "Scales the Normal Map Normal");
            public static GUIContent enableEmission = new GUIContent("Emission", "Enable Emission");
            public static GUIContent emissiveColor = new GUIContent("Color");
            public static GUIContent blendedClippingWidth = new GUIContent("Blended Clipping Width", "The Width of the Clipping Primitive Clip Fade Region on Non-Cutout Materials");
            public static GUIContent clippingBorder = new GUIContent("Clipping Border", "Enable a Border Along the Clipping Primitive's Edge");
            public static GUIContent clippingBorderWidth = new GUIContent("Width", "Width of the Clipping Border");
            public static GUIContent clippingBorderColor = new GUIContent("Color", "Interpolated Color of the Clipping Border");
            public static GUIContent hoverLight = new GUIContent("Hover Light", "Enable utilization of Hover Light(s)");
            public static GUIContent enableHoverColorOverride = new GUIContent("Override Color", "Override Global Hover Light Color for this Material");
            public static GUIContent hoverColorOverride = new GUIContent("Color", "Override Hover Light Color");
            public static GUIContent proximityLight = new GUIContent("Proximity Light", "Enable utilization of Proximity Light(s)");
            public static GUIContent enableProximityLightColorOverride = new GUIContent("Override Color", "Override Global Proximity Light Color for this Material");
            public static GUIContent proximityLightCenterColorOverride = new GUIContent("Center Color", "The Override Color of the ProximityLight Gradient at the Center (RGB) and (A) is Gradient Extent");
            public static GUIContent proximityLightMiddleColorOverride = new GUIContent("Middle Color", "The Override Color of the ProximityLight Gradient at the Middle (RGB) and (A) is Gradient Extent");
            public static GUIContent proximityLightOuterColorOverride = new GUIContent("Outer Color", "The Override Color of the ProximityLight Gradient at the Outer Edge (RGB) and (A) is Gradient Extent");
            public static GUIContent proximityLightSubtractive = new GUIContent("Subtractive", "Proximity Lights Remove Light from a Surface, Used to Mimic a Shadow");
            public static GUIContent proximityLightTwoSided = new GUIContent("Two Sided", "Proximity Lights Apply to Both Sides of a Surface");
            public static GUIContent fluentLightIntensity = new GUIContent("Light Intensity", "Intensity Scaler for All Hover and Proximity Lights");
            public static GUIContent edgeSmoothingValue = new GUIContent("Edge Smoothing Value", "Smooths Edges When Round Corners and Transparency Is Enabled");
            public static GUIContent stencil = new GUIContent("Enable Stencil Testing", "Enabled Stencil Testing Operations");
            public static GUIContent stencilReference = new GUIContent("Stencil Reference", "Value to Compared Against (if Comparison is Anything but Always) and/or the Value to be Written to the Buffer (if Either Pass, Fail or ZFail is Set to Replace)");
            public static GUIContent stencilComparison = new GUIContent("Stencil Comparison", "Function to Compare the Reference Value to");
            public static GUIContent stencilOperation = new GUIContent("Stencil Operation", "What to do When the Stencil Test Passes");
            public static GUIContent ignoreZScale = new GUIContent("Ignore Z Scale", "For Features That Use Object Scale (Round Corners, Border Light, etc.), Ignore the Z Scale of the Object");
        }

        protected MaterialProperty instancedColor;
        protected MaterialProperty albedoMap;
        protected MaterialProperty albedoColor;
        protected MaterialProperty albedoAlphaMode;
        protected MaterialProperty albedoAssignedAtRuntime;
        protected MaterialProperty alphaCutoff;
        protected MaterialProperty enableNormalMap;
        protected MaterialProperty normalMap;
        protected MaterialProperty normalMapScale;
        protected MaterialProperty enableEmission;
        protected MaterialProperty emissiveColor;
        protected MaterialProperty metallic;
        protected MaterialProperty smoothness;
        protected MaterialProperty blendedClippingWidth;
        protected MaterialProperty clippingBorder;
        protected MaterialProperty clippingBorderWidth;
        protected MaterialProperty clippingBorderColor;
        protected MaterialProperty hoverLight;
        protected MaterialProperty enableHoverColorOverride;
        protected MaterialProperty hoverColorOverride;
        protected MaterialProperty proximityLight;
        protected MaterialProperty enableProximityLightColorOverride;
        protected MaterialProperty proximityLightCenterColorOverride;
        protected MaterialProperty proximityLightMiddleColorOverride;
        protected MaterialProperty proximityLightOuterColorOverride;
        protected MaterialProperty proximityLightSubtractive;
        protected MaterialProperty proximityLightTwoSided;
        protected MaterialProperty fluentLightIntensity;
        protected MaterialProperty edgeSmoothingValue;
        protected MaterialProperty stencil;
        protected MaterialProperty stencilReference;
        protected MaterialProperty stencilComparison;
        protected MaterialProperty stencilOperation;
        protected MaterialProperty ignoreZScale;

        protected override void FindProperties(MaterialProperty[] props)
        {
            base.FindProperties(props);

            instancedColor = FindProperty(Styles.instancedColorName, props);
            albedoMap = FindProperty("_MainTex", props);
            albedoColor = FindProperty("_Color", props);
            albedoAlphaMode = FindProperty("_AlbedoAlphaMode", props);
            albedoAssignedAtRuntime = FindProperty("_AlbedoAssignedAtRuntime", props);
            alphaCutoff = FindProperty("_Cutoff", props);
            metallic = FindProperty("_Metallic", props);
            smoothness = FindProperty("_Smoothness", props);
            enableNormalMap = FindProperty("_EnableNormalMap", props);
            normalMap = FindProperty("_NormalMap", props);
            normalMapScale = FindProperty("_NormalMapScale", props);
            enableEmission = FindProperty("_EnableEmission", props);
            emissiveColor = FindProperty("_EmissiveColor", props);
            blendedClippingWidth = FindProperty("_BlendedClippingWidth", props);
            clippingBorder = FindProperty("_ClippingBorder", props);
            clippingBorderWidth = FindProperty("_ClippingBorderWidth", props);
            clippingBorderColor = FindProperty("_ClippingBorderColor", props);
            hoverLight = FindProperty("_HoverLight", props);
            enableHoverColorOverride = FindProperty("_EnableHoverColorOverride", props);
            hoverColorOverride = FindProperty("_HoverColorOverride", props);
            proximityLight = FindProperty("_ProximityLight", props);
            enableProximityLightColorOverride = FindProperty("_EnableProximityLightColorOverride", props);
            proximityLightCenterColorOverride = FindProperty("_ProximityLightCenterColorOverride", props);
            proximityLightMiddleColorOverride = FindProperty("_ProximityLightMiddleColorOverride", props);
            proximityLightOuterColorOverride = FindProperty("_ProximityLightOuterColorOverride", props);
            proximityLightSubtractive = FindProperty("_ProximityLightSubtractive", props);
            proximityLightTwoSided = FindProperty("_ProximityLightTwoSided", props);
            fluentLightIntensity = FindProperty("_FluentLightIntensity", props);
            edgeSmoothingValue = FindProperty("_EdgeSmoothingValue", props);
            stencil = FindProperty("_Stencil", props);
            stencilReference = FindProperty("_StencilReference", props);
            stencilComparison = FindProperty(Styles.stencilComparisonName, props);
            stencilOperation = FindProperty(Styles.stencilOperationName, props);
            ignoreZScale = FindProperty("_IgnoreZScale", props);
        }

        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
        {
            Material material = (Material)materialEditor.target;

            base.OnGUI(materialEditor, props);

            MainMapOptions(materialEditor, material);
            RenderingOptions(materialEditor, material);
            FluentOptions(materialEditor, material);
            AdvancedOptions(materialEditor, material);
        }

        public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
        {
            // Cache old shader properties with potentially different names than the new shader.
            float? smoothness = GetFloatProperty(material, "_Glossiness");
            float? diffuse = GetFloatProperty(material, "_UseDiffuse");
            float? normalMap = null;
            Texture normalMapTexture = material.GetTexture("_BumpMap");
            float? normalMapScale = GetFloatProperty(material, "_BumpScale");
            float? emission = null;
            Color? emissionColor = GetColorProperty(material, "_EmissionColor");
            Vector4? textureScaleOffset = null;
            float? cullMode = GetFloatProperty(material, "_Cull");

            if (oldShader)
            {
                if (oldShader.name.Contains("Standard"))
                {
                    normalMap = material.IsKeywordEnabled("_NORMALMAP") ? 1.0f : 0.0f;
                    emission = material.IsKeywordEnabled("_EMISSION") ? 1.0f : 0.0f;
                }
                else if (oldShader.name.Contains("Fast Configurable"))
                {
                    normalMap = material.IsKeywordEnabled("_USEBUMPMAP_ON") ? 1.0f : 0.0f;
                    emission = GetFloatProperty(material, "_UseEmissionColor");
                    textureScaleOffset = GetVectorProperty(material, "_TextureScaleOffset");
                }
            }

            base.AssignNewShaderToMaterial(material, oldShader, newShader);

            // Apply old shader properties to the new shader.
            SetShaderFeatureActive(material, null, "_Smoothness", smoothness);
            SetShaderFeatureActive(material, "_DIRECTIONAL_LIGHT", "_DirectionalLight", diffuse);
            SetShaderFeatureActive(material, "_NORMAL_MAP", "_EnableNormalMap", normalMap);

            if (normalMapTexture)
            {
                material.SetTexture("_NormalMap", normalMapTexture);
            }

            SetShaderFeatureActive(material, null, "_NormalMapScale", normalMapScale);
            SetShaderFeatureActive(material, "_EMISSION", "_EnableEmission", emission);
            SetColorProperty(material, "_EmissiveColor", emissionColor);
            SetVectorProperty(material, "_MainTex_ST", textureScaleOffset);
            SetShaderFeatureActive(material, null, "_CullMode", cullMode);

            // Setup the rendering mode based on the old shader.
            if (oldShader == null || !oldShader.name.Contains(LegacyShadersPath))
            {
                SetupMaterialWithRenderingMode(material, (RenderingMode)material.GetFloat(BaseStyles.renderingModeName), CustomRenderingMode.Opaque, -1);
            }
            else
            {
                RenderingMode mode = RenderingMode.Opaque;

                if (oldShader.name.Contains(TransparentCutoutShadersPath))
                {
                    mode = RenderingMode.Cutout;
                }
                else if (oldShader.name.Contains(TransparentShadersPath))
                {
                    mode = RenderingMode.Fade;
                }

                material.SetFloat(BaseStyles.renderingModeName, (float)mode);

                MaterialChanged(material);
            }
        }

        protected override void MaterialChanged(Material material)
        {
            SetupMaterialWithAlbedo(material, albedoMap, albedoAlphaMode, albedoAssignedAtRuntime);

            base.MaterialChanged(material);
        }

        protected void MainMapOptions(MaterialEditor materialEditor, Material material)
        {
            GUILayout.Label(Styles.primaryMapsTitle, EditorStyles.boldLabel);

            materialEditor.TexturePropertySingleLine(Styles.albedo, albedoMap, albedoColor);

            if (albedoMap.textureValue == null)
            {
                materialEditor.ShaderProperty(albedoAssignedAtRuntime, Styles.albedoAssignedAtRuntime, 2);
            }

            EditorGUI.indentLevel += 2;

            materialEditor.ShaderProperty(albedoAlphaMode, albedoAlphaMode.displayName);

            if ((RenderingMode)renderingMode.floatValue == RenderingMode.Cutout ||
                (RenderingMode)renderingMode.floatValue == RenderingMode.Custom)
            {
                materialEditor.ShaderProperty(alphaCutoff, Styles.alphaCutoff.text);
            }

            if ((AlbedoAlphaMode)albedoAlphaMode.floatValue != AlbedoAlphaMode.Metallic)
            {
                materialEditor.ShaderProperty(metallic, Styles.metallic);
            }

            if ((AlbedoAlphaMode)albedoAlphaMode.floatValue != AlbedoAlphaMode.Smoothness)
            {
                materialEditor.ShaderProperty(smoothness, Styles.smoothness);
            }

            SetupMaterialWithAlbedo(material, albedoMap, albedoAlphaMode, albedoAssignedAtRuntime);

            EditorGUI.indentLevel -= 2;

            materialEditor.ShaderProperty(enableNormalMap, Styles.enableNormalMap);

            if (PropertyEnabled(enableNormalMap))
            {
                EditorGUI.indentLevel += 2;
                materialEditor.TexturePropertySingleLine(Styles.normalMap, normalMap, normalMap.textureValue != null ? normalMapScale : null);
                EditorGUI.indentLevel -= 2;
            }

            materialEditor.ShaderProperty(enableEmission, Styles.enableEmission);

            if (PropertyEnabled(enableEmission))
            {
                materialEditor.ShaderProperty(emissiveColor, Styles.emissiveColor, 2);
            }

            EditorGUILayout.Space();
            materialEditor.TextureScaleOffsetProperty(albedoMap);
        }

        protected void RenderingOptions(MaterialEditor materialEditor, Material material)
        {
            EditorGUILayout.Space();
            GUILayout.Label(Styles.renderingOptionsTitle, EditorStyles.boldLabel);

            materialEditor.ShaderProperty(clippingBorder, Styles.clippingBorder);

            if (PropertyEnabled(clippingBorder))
            {
                materialEditor.ShaderProperty(clippingBorderWidth, Styles.clippingBorderWidth, 2);
                materialEditor.ShaderProperty(clippingBorderColor, Styles.clippingBorderColor, 2);
                GUILayout.Box(string.Format(Styles.propertiesComponentHelp, nameof(ClippingPrimitive), "other clipping"), EditorStyles.helpBox, Array.Empty<GUILayoutOption>());
            }
        }

        protected void FluentOptions(MaterialEditor materialEditor, Material material)
        {
            EditorGUILayout.Space();
            GUILayout.Label(Styles.fluentOptionsTitle, EditorStyles.boldLabel);
            RenderingMode mode = (RenderingMode)renderingMode.floatValue;
            CustomRenderingMode customMode = (CustomRenderingMode)customRenderingMode.floatValue;

            materialEditor.ShaderProperty(hoverLight, Styles.hoverLight);

            if (PropertyEnabled(hoverLight))
            {
                GUILayout.Box(string.Format(Styles.propertiesComponentHelp, nameof(HoverLight), Styles.hoverLight.text), EditorStyles.helpBox, Array.Empty<GUILayoutOption>());

                materialEditor.ShaderProperty(enableHoverColorOverride, Styles.enableHoverColorOverride, 2);

                if (PropertyEnabled(enableHoverColorOverride))
                {
                    materialEditor.ShaderProperty(hoverColorOverride, Styles.hoverColorOverride, 4);
                }
            }

            materialEditor.ShaderProperty(proximityLight, Styles.proximityLight);

            if (PropertyEnabled(proximityLight))
            {
                materialEditor.ShaderProperty(enableProximityLightColorOverride, Styles.enableProximityLightColorOverride, 2);

                if (PropertyEnabled(enableProximityLightColorOverride))
                {
                    materialEditor.ShaderProperty(proximityLightCenterColorOverride, Styles.proximityLightCenterColorOverride, 4);
                    materialEditor.ShaderProperty(proximityLightMiddleColorOverride, Styles.proximityLightMiddleColorOverride, 4);
                    materialEditor.ShaderProperty(proximityLightOuterColorOverride, Styles.proximityLightOuterColorOverride, 4);
                }

                materialEditor.ShaderProperty(proximityLightSubtractive, Styles.proximityLightSubtractive, 2);
                materialEditor.ShaderProperty(proximityLightTwoSided, Styles.proximityLightTwoSided, 2);
                GUILayout.Box(string.Format(Styles.propertiesComponentHelp, nameof(ProximityLight), Styles.proximityLight.text), EditorStyles.helpBox, Array.Empty<GUILayoutOption>());
            }

            if (PropertyEnabled(hoverLight) || PropertyEnabled(proximityLight))
            {
                materialEditor.ShaderProperty(fluentLightIntensity, Styles.fluentLightIntensity);
            }
        }

        protected void AdvancedOptions(MaterialEditor materialEditor, Material material)
        {
            EditorGUILayout.Space();
            GUILayout.Label(Styles.advancedOptionsTitle, EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();

            materialEditor.ShaderProperty(renderQueueOverride, BaseStyles.renderQueueOverride);

            if (EditorGUI.EndChangeCheck())
            {
                MaterialChanged(material);
            }

            // Show the RenderQueueField but do not allow users to directly manipulate it. That is done via the renderQueueOverride.
            GUI.enabled = false;
            materialEditor.RenderQueueField();

            if (!GUI.enabled && !material.enableInstancing)
            {
                material.enableInstancing = true;
            }

            materialEditor.EnableInstancingField();

            if (material.enableInstancing)
            {
                GUI.enabled = true;
                materialEditor.ShaderProperty(instancedColor, Styles.instancedColor, 2);
            }
            else
            {
                // When instancing is disable, disable instanced color.
                SetShaderFeatureActive(material, Styles.instancedColorFeatureName, Styles.instancedColorName, 0.0f);
            }

            materialEditor.ShaderProperty(stencil, Styles.stencil);

            if (PropertyEnabled(stencil))
            {
                materialEditor.ShaderProperty(stencilReference, Styles.stencilReference, 2);
                materialEditor.ShaderProperty(stencilComparison, Styles.stencilComparison, 2);
                materialEditor.ShaderProperty(stencilOperation, Styles.stencilOperation, 2);
            }
            else
            {
                // When stencil is disable, revert to the default stencil operations. Note, when tested on D3D11 hardware the stencil state 
                // is still set even when the CompareFunction.Disabled is selected, but this does not seem to affect performance.
                material.SetInt(Styles.stencilComparisonName, (int)CompareFunction.Disabled);
                material.SetInt(Styles.stencilOperationName, (int)StencilOp.Keep);
            }
        }

        protected static void SetupMaterialWithAlbedo(Material material, MaterialProperty albedoMap, MaterialProperty albedoAlphaMode, MaterialProperty albedoAssignedAtRuntime)
        {
            if (albedoMap.textureValue || PropertyEnabled(albedoAssignedAtRuntime))
            {
                material.DisableKeyword(Styles.disableAlbedoMapName);
            }
            else
            {
                material.EnableKeyword(Styles.disableAlbedoMapName);
            }

            switch ((AlbedoAlphaMode)albedoAlphaMode.floatValue)
            {
                case AlbedoAlphaMode.Transparency:
                    {
                        material.DisableKeyword(Styles.albedoMapAlphaMetallicName);
                        material.DisableKeyword(Styles.albedoMapAlphaSmoothnessName);
                    }
                    break;

                case AlbedoAlphaMode.Metallic:
                    {
                        material.EnableKeyword(Styles.albedoMapAlphaMetallicName);
                        material.DisableKeyword(Styles.albedoMapAlphaSmoothnessName);
                    }
                    break;

                case AlbedoAlphaMode.Smoothness:
                    {
                        material.DisableKeyword(Styles.albedoMapAlphaMetallicName);
                        material.EnableKeyword(Styles.albedoMapAlphaSmoothnessName);
                    }
                    break;
            }
        }

        [MenuItem("BIMserver.center Toolkit/Utilities/Upgrade BIMserver.center Toolkit (Standard) Shader for Lightweight Render Pipeline", false, 11)]
        protected static void UpgradeShaderForLightweightRenderPipeline()
        {
            if (EditorUtility.DisplayDialog("Upgrade BIMserver.center Standard Shader?",
                                            "This will alter the BIMserver.center Standard Shader for use with Unity's Lightweight Render Pipeline. You cannot undo this action.",
                                            "Ok",
                                            "Cancel"))
            {
                string path = AssetDatabase.GetAssetPath(StandardShaderUtility.BIMServerCenterStandardShader);

                if (!string.IsNullOrEmpty(path))
                {
                    try
                    {
                        string upgradedShader = File.ReadAllText(path);
                        upgradedShader = upgradedShader.Replace("Tags{ \"RenderType\" = \"Opaque\" \"LightMode\" = \"ForwardBase\" }",
                                                                "Tags{ \"RenderType\" = \"Opaque\" \"LightMode\" = \"LightweightForward\" }");
                        upgradedShader = upgradedShader.Replace("//#define _LIGHTWEIGHT_RENDER_PIPELINE",
                                                                "#define _LIGHTWEIGHT_RENDER_PIPELINE");
                        File.WriteAllText(path, upgradedShader);
                        AssetDatabase.Refresh();

                        Debug.LogFormat("Upgraded {0} for use with the Lightweight Render Pipeline.", path);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
                else
                {
                    Debug.LogErrorFormat("Failed to get asset path to: {0}", StandardShaderUtility.BIMServerCenterStandardShaderName);
                }
            }
        }

        [MenuItem("BIMserver.center Toolkit/Utilities/Upgrade BIMserver.center Toolkit (Standard) Shader for Lightweight Render Pipeline", true)]
        protected static bool UpgradeShaderForLightweightRenderPipelineValidate()
        {
            return GraphicsSettings.renderPipelineAsset != null;
        }
    }
#endif
}
