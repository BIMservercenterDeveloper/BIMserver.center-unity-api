﻿/*
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
using System;

namespace BIMservercenter.Toolkit.Internal.Shaders
{
#if UNITY_EDITOR
    public abstract class BIMServerCenterShaderGUI : ShaderGUI
    {
        protected enum RenderingMode
        {
            Opaque = 0,
            Cutout = 1,
            Fade = 2,
            Transparent = 3,
            Additive = 4,
            Custom = 5
        }

        protected enum CustomRenderingMode
        {
            Opaque = 0,
            Cutout = 1,
            Fade = 2
        }

        protected enum DepthWrite
        {
            Off = 0,
            On = 1
        }

        protected static class BaseStyles
        {
            public static string renderingOptionsTitle = "Rendering Options";
            public static string advancedOptionsTitle = "Advanced Options";
            public static string renderTypeName = "RenderType";
            public static string renderingModeName = "_Mode";
            public static string customRenderingModeName = "_CustomMode";
            public static string sourceBlendName = "_SrcBlend";
            public static string destinationBlendName = "_DstBlend";
            public static string blendOperationName = "_BlendOp";
            public static string depthTestName = "_ZTest";
            public static string depthWriteName = "_ZWrite";
            public static string depthOffsetFactorName = "_ZOffsetFactor";
            public static string depthOffsetUnitsName = "_ZOffsetUnits";
            public static string colorWriteMaskName = "_ColorWriteMask";

            public static string cullModeName = "_CullMode";
            public static string renderQueueOverrideName = "_RenderQueueOverride";

            public static string alphaTestOnName = "_ALPHATEST_ON";
            public static string alphaBlendOnName = "_ALPHABLEND_ON";

            public static readonly string[] renderingModeNames = Enum.GetNames(typeof(RenderingMode));
            public static readonly string[] customRenderingModeNames = Enum.GetNames(typeof(CustomRenderingMode));
            public static readonly string[] depthWriteNames = Enum.GetNames(typeof(DepthWrite));
            public static GUIContent sourceBlend = new GUIContent("Source Blend", "Blend Mode of Newly Calculated Color");
            public static GUIContent destinationBlend = new GUIContent("Destination Blend", "Blend Mode of Existing Color");
            public static GUIContent blendOperation = new GUIContent("Blend Operation", "Operation for Blending New Color With Existing Color");
            public static GUIContent depthTest = new GUIContent("Depth Test", "How Should Depth Testing Be Performed.");
            public static GUIContent depthWrite = new GUIContent("Depth Write", "Controls Whether Pixels From This Material Are Written to the Depth Buffer");
            public static GUIContent depthOffsetFactor = new GUIContent("Depth Offset Factor", "Scales the Maximum Z Slope, with Respect to X or Y of the Polygon");
            public static GUIContent depthOffsetUnits = new GUIContent("Depth Offset Units", "Scales the Minimum Resolvable Depth Buffer Value");
            public static GUIContent colorWriteMask = new GUIContent("Color Write Mask", "Color Channel Writing Mask");
            public static GUIContent cullMode = new GUIContent("Cull Mode", "Triangle Culling Mode");
            public static GUIContent renderQueueOverride = new GUIContent("Render Queue Override", "Manually Override the Render Queue");
        }

        protected bool initialized;

        protected MaterialProperty renderingMode;
        protected MaterialProperty customRenderingMode;
        protected MaterialProperty sourceBlend;
        protected MaterialProperty destinationBlend;
        protected MaterialProperty blendOperation;
        protected MaterialProperty depthTest;
        protected MaterialProperty depthWrite;
        protected MaterialProperty depthOffsetFactor;
        protected MaterialProperty depthOffsetUnits;
        protected MaterialProperty colorWriteMask;
        protected MaterialProperty cullMode;
        protected MaterialProperty renderQueueOverride;

        protected const string LegacyShadersPath = "Legacy Shaders/";
        protected const string TransparentShadersPath = "/Transparent/";
        protected const string TransparentCutoutShadersPath = "/Transparent/Cutout/";

        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
        {
            Material material = (Material)materialEditor.target;

            FindProperties(props);
            Initialize(material);

            RenderingModeOptions(materialEditor);
        }

        protected virtual void FindProperties(MaterialProperty[] props)
        {
            renderingMode = FindProperty(BaseStyles.renderingModeName, props);
            customRenderingMode = FindProperty(BaseStyles.customRenderingModeName, props);
            sourceBlend = FindProperty(BaseStyles.sourceBlendName, props);
            destinationBlend = FindProperty(BaseStyles.destinationBlendName, props);
            blendOperation = FindProperty(BaseStyles.blendOperationName, props);
            depthTest = FindProperty(BaseStyles.depthTestName, props);
            depthWrite = FindProperty(BaseStyles.depthWriteName, props);
            depthOffsetFactor = FindProperty(BaseStyles.depthOffsetFactorName, props);
            depthOffsetUnits = FindProperty(BaseStyles.depthOffsetUnitsName, props);
            colorWriteMask = FindProperty(BaseStyles.colorWriteMaskName, props);

            cullMode = FindProperty(BaseStyles.cullModeName, props);
            renderQueueOverride = FindProperty(BaseStyles.renderQueueOverrideName, props);
        }

        protected void Initialize(Material material)
        {
            if (!initialized)
            {
                MaterialChanged(material);
                initialized = true;
            }
        }

        protected virtual void MaterialChanged(Material material)
        {
            SetupMaterialWithRenderingMode(material,
                (RenderingMode)renderingMode.floatValue,
                (CustomRenderingMode)customRenderingMode.floatValue,
                (int)renderQueueOverride.floatValue);
        }

        protected void RenderingModeOptions(MaterialEditor materialEditor)
        {
            EditorGUI.BeginChangeCheck();

            EditorGUI.showMixedValue = renderingMode.hasMixedValue;
            RenderingMode mode = (RenderingMode)renderingMode.floatValue;
            EditorGUI.BeginChangeCheck();
            mode = (RenderingMode)EditorGUILayout.Popup(renderingMode.displayName, (int)mode, BaseStyles.renderingModeNames);

            if (EditorGUI.EndChangeCheck())
            {
                materialEditor.RegisterPropertyChangeUndo(renderingMode.displayName);
                renderingMode.floatValue = (float)mode;

                Object[] targets = renderingMode.targets;

                foreach (Object target in targets)
                {
                    MaterialChanged((Material)target);
                }
            }

            EditorGUI.showMixedValue = false;

            if ((RenderingMode)renderingMode.floatValue == RenderingMode.Custom)
            {
                EditorGUI.indentLevel += 2;
                customRenderingMode.floatValue = EditorGUILayout.Popup(customRenderingMode.displayName, (int)customRenderingMode.floatValue, BaseStyles.customRenderingModeNames);
                materialEditor.ShaderProperty(sourceBlend, BaseStyles.sourceBlend);
                materialEditor.ShaderProperty(destinationBlend, BaseStyles.destinationBlend);
                materialEditor.ShaderProperty(blendOperation, BaseStyles.blendOperation);
                materialEditor.ShaderProperty(depthTest, BaseStyles.depthTest);
                depthWrite.floatValue = EditorGUILayout.Popup(depthWrite.displayName, (int)depthWrite.floatValue, BaseStyles.depthWriteNames);
                materialEditor.ShaderProperty(depthOffsetFactor, BaseStyles.depthOffsetFactor);
                materialEditor.ShaderProperty(depthOffsetUnits, BaseStyles.depthOffsetUnits);
                materialEditor.ShaderProperty(colorWriteMask, BaseStyles.colorWriteMask);
                EditorGUI.indentLevel -= 2;
            }

            if (!PropertyEnabled(depthWrite))
            {
                if (BIMServerCenterShaderGUIUtilities.DisplayDepthWriteWarning(materialEditor))
                {
                    renderingMode.floatValue = (float)RenderingMode.Custom;
                    depthWrite.floatValue = (float)DepthWrite.On;
                }
            }

            materialEditor.ShaderProperty(cullMode, BaseStyles.cullMode);
        }

        protected static void SetupMaterialWithRenderingMode(Material material, RenderingMode mode, CustomRenderingMode customMode, int renderQueueOverride)
        {
            if (mode != RenderingMode.Custom)
            {
                material.SetInt(BaseStyles.blendOperationName, (int)BlendOp.Add);
                material.SetInt(BaseStyles.depthTestName, (int)CompareFunction.LessEqual);
                material.SetFloat(BaseStyles.depthOffsetFactorName, 0.0f);
                material.SetFloat(BaseStyles.depthOffsetUnitsName, 0.0f);
                material.SetInt(BaseStyles.colorWriteMaskName, (int)ColorWriteMask.All);
            }

            switch (mode)
            {
                case RenderingMode.Opaque:
                    {
                        material.SetOverrideTag(BaseStyles.renderTypeName, BaseStyles.renderingModeNames[(int)RenderingMode.Opaque]);
                        material.SetInt(BaseStyles.customRenderingModeName, (int)CustomRenderingMode.Opaque);
                        material.SetInt(BaseStyles.sourceBlendName, (int)BlendMode.One);
                        material.SetInt(BaseStyles.destinationBlendName, (int)BlendMode.Zero);
                        material.SetInt(BaseStyles.depthWriteName, (int)DepthWrite.On);
                        material.DisableKeyword(BaseStyles.alphaTestOnName);
                        material.DisableKeyword(BaseStyles.alphaBlendOnName);
                        material.renderQueue = (renderQueueOverride >= 0) ? renderQueueOverride : (int)RenderQueue.Geometry;
                    }
                    break;

                case RenderingMode.Cutout:
                    {
                        material.SetOverrideTag(BaseStyles.renderTypeName, BaseStyles.renderingModeNames[(int)RenderingMode.Cutout]);
                        material.SetInt(BaseStyles.customRenderingModeName, (int)CustomRenderingMode.Cutout);
                        material.SetInt(BaseStyles.sourceBlendName, (int)BlendMode.One);
                        material.SetInt(BaseStyles.destinationBlendName, (int)BlendMode.Zero);
                        material.SetInt(BaseStyles.depthWriteName, (int)DepthWrite.On);
                        material.EnableKeyword(BaseStyles.alphaTestOnName);
                        material.DisableKeyword(BaseStyles.alphaBlendOnName);
                        material.renderQueue = (renderQueueOverride >= 0) ? renderQueueOverride : (int)RenderQueue.AlphaTest;
                    }
                    break;

                case RenderingMode.Fade:
                    {
                        material.SetOverrideTag(BaseStyles.renderTypeName, BaseStyles.renderingModeNames[(int)RenderingMode.Fade]);
                        material.SetInt(BaseStyles.customRenderingModeName, (int)CustomRenderingMode.Fade);
                        material.SetInt(BaseStyles.sourceBlendName, (int)BlendMode.SrcAlpha);
                        material.SetInt(BaseStyles.destinationBlendName, (int)BlendMode.OneMinusSrcAlpha);
                        material.SetInt(BaseStyles.depthWriteName, (int)DepthWrite.Off);
                        material.DisableKeyword(BaseStyles.alphaTestOnName);
                        material.EnableKeyword(BaseStyles.alphaBlendOnName);
                        material.renderQueue = (renderQueueOverride >= 0) ? renderQueueOverride : (int)RenderQueue.Transparent;
                    }
                    break;

                case RenderingMode.Transparent:
                    {
                        material.SetOverrideTag(BaseStyles.renderTypeName, BaseStyles.renderingModeNames[(int)RenderingMode.Fade]);
                        material.SetInt(BaseStyles.customRenderingModeName, (int)CustomRenderingMode.Fade);
                        material.SetInt(BaseStyles.sourceBlendName, (int)BlendMode.One);
                        material.SetInt(BaseStyles.destinationBlendName, (int)BlendMode.OneMinusSrcAlpha);
                        material.SetInt(BaseStyles.depthWriteName, (int)DepthWrite.Off);
                        material.DisableKeyword(BaseStyles.alphaTestOnName);
                        material.EnableKeyword(BaseStyles.alphaBlendOnName);
                        material.renderQueue = (renderQueueOverride >= 0) ? renderQueueOverride : (int)RenderQueue.Transparent;
                    }
                    break;

                case RenderingMode.Additive:
                    {
                        material.SetOverrideTag(BaseStyles.renderTypeName, BaseStyles.renderingModeNames[(int)RenderingMode.Fade]);
                        material.SetInt(BaseStyles.customRenderingModeName, (int)CustomRenderingMode.Fade);
                        material.SetInt(BaseStyles.sourceBlendName, (int)BlendMode.One);
                        material.SetInt(BaseStyles.destinationBlendName, (int)BlendMode.One);
                        material.SetInt(BaseStyles.depthWriteName, (int)DepthWrite.Off);
                        material.DisableKeyword(BaseStyles.alphaTestOnName);
                        material.EnableKeyword(BaseStyles.alphaBlendOnName);
                        material.renderQueue = (renderQueueOverride >= 0) ? renderQueueOverride : (int)RenderQueue.Transparent;
                    }
                    break;

                case RenderingMode.Custom:
                    {
                        material.SetOverrideTag(BaseStyles.renderTypeName, BaseStyles.customRenderingModeNames[(int)customMode]);
                        
                        switch (customMode)
                        {
                            case CustomRenderingMode.Opaque:
                                {
                                    material.DisableKeyword(BaseStyles.alphaTestOnName);
                                    material.DisableKeyword(BaseStyles.alphaBlendOnName);
                                }
                                break;

                            case CustomRenderingMode.Cutout:
                                {
                                    material.EnableKeyword(BaseStyles.alphaTestOnName);
                                    material.DisableKeyword(BaseStyles.alphaBlendOnName);
                                }
                                break;

                            case CustomRenderingMode.Fade:
                                {
                                    material.DisableKeyword(BaseStyles.alphaTestOnName);
                                    material.EnableKeyword(BaseStyles.alphaBlendOnName);
                                }
                                break;
                        }

                        material.renderQueue = (renderQueueOverride >= 0) ? renderQueueOverride : material.renderQueue;
                    }
                    break;
            }
        }

        protected static bool PropertyEnabled(MaterialProperty property)
        {
            return !property.floatValue.Equals(0.0f);
        }

        protected static float? GetFloatProperty(Material material, string propertyName)
        {
            if (material.HasProperty(propertyName))
            {
                return material.GetFloat(propertyName);
            }

            return null;
        }

        protected static Vector4? GetVectorProperty(Material material, string propertyName)
        {
            if (material.HasProperty(propertyName))
            {
                return material.GetVector(propertyName);
            }

            return null;
        }

        protected static Color? GetColorProperty(Material material, string propertyName)
        {
            if (material.HasProperty(propertyName))
            {
                return material.GetColor(propertyName);
            }

            return null;
        }

        protected static void SetShaderFeatureActive(Material material, string keywordName, string propertyName, float? propertyValue)
        {
            if (propertyValue.HasValue)
            {
                if (keywordName != null)
                {
                    if (!propertyValue.Value.Equals(0.0f))
                    {
                        material.EnableKeyword(keywordName);
                    }
                    else
                    {
                        material.DisableKeyword(keywordName);
                    }
                }

                material.SetFloat(propertyName, propertyValue.Value);
            }
        }

        protected static void SetVectorProperty(Material material, string propertyName, Vector4? propertyValue)
        {
            if (propertyValue.HasValue)
            {
                material.SetVector(propertyName, propertyValue.Value);
            }
        }

        protected static void SetColorProperty(Material material, string propertyName, Color? propertyValue)
        {
            if (propertyValue.HasValue)
            {
                material.SetColor(propertyName, propertyValue.Value);
            }
        }
    }
#endif
}
