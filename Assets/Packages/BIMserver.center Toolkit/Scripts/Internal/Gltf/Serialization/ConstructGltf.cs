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

using BIMservercenter.Toolkit.Internal.Gltf.AsyncAwaitUtil;
using BIMservercenter.Toolkit.Internal.Gltf.Schema;
using BIMservercenter.Toolkit.Public.Utilities;
using BIMservercenter.Toolkit.Public.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Rendering;
using System.Collections;
using System.Threading;
using UnityEngine;
using System.IO;
using System;

namespace BIMservercenter.Toolkit.Internal.Gltf.Serialization
{
    public static class ConstructGltf
    {
        private static readonly WaitForUpdate Update = new WaitForUpdate();
        private static readonly WaitForBackgroundThread BackgroundThread = new WaitForBackgroundThread();
        private static readonly int SrcBlendId = Shader.PropertyToID("_SrcBlend");
        private static readonly int DstBlendId = Shader.PropertyToID("_DstBlend");
        private static readonly int ZWriteId = Shader.PropertyToID("_ZWrite");
        private static readonly int ModeId = Shader.PropertyToID("_Mode");
        private static readonly int EmissionMapId = Shader.PropertyToID("_EmissionMap");
        private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");
        private static readonly int MetallicGlossMapId = Shader.PropertyToID("_MetallicGlossMap");
        private static readonly int GlossinessId = Shader.PropertyToID("_Glossiness");
        private static readonly int MetallicId = Shader.PropertyToID("_Metallic");
        private static readonly int BumpMapId = Shader.PropertyToID("_BumpMap");
        private static readonly int EmissiveColorId = Shader.PropertyToID("_EmissiveColor");
        private static readonly int ChannelMapId = Shader.PropertyToID("_ChannelMap");
        private static readonly int SmoothnessId = Shader.PropertyToID("_Smoothness");
        private static readonly int NormalMapId = Shader.PropertyToID("_NormalMap");
        private static readonly int NormalMapScaleId = Shader.PropertyToID("_NormalMapScale");
        private static readonly int CullModeId = Shader.PropertyToID("_CullMode");
        private static readonly int OffsetFactorId = Shader.PropertyToID("_ZOffsetFactor");
        private static readonly int OffsetUnitsId = Shader.PropertyToID("_ZOffsetUnits");

        /// <summary>
        /// Constructs the glTF Object.
        /// </summary>
        /// <param name="gltfObject"></param>
        /// <returns>The new <see href="https://docs.unity3d.com/ScriptReference/GameObject.html">GameObject</see> of the final constructed <see cref="Schema.GltfScene"/></returns>
        /// <exception cref="BSExceptionCancellationRequested">Thrown when cancellation is requested</exception>    
        public static async void Construct(this GltfObject gltfObject, bool generateColliders, CancellationToken cancellationToken, FuncProgressPercUpdate funcProgressPercUpdate)
        {
            await gltfObject.ConstructAsync(generateColliders, cancellationToken, funcProgressPercUpdate);
        }

        /// <summary>
        /// Constructs the glTF Object.
        /// </summary>
        /// <param name="gltfObject"></param>
        /// <returns>The new <see href="https://docs.unity3d.com/ScriptReference/GameObject.html">GameObject</see> of the final constructed <see cref="Schema.GltfScene"/></returns>
        /// <exception cref="BSExceptionCancellationRequested">Thrown when cancellation is requested</exception>   
        public static async Task<GameObject> ConstructAsync(this GltfObject gltfObject, bool generateColliders, CancellationToken cancellationToken, FuncProgressPercUpdate funcProgressPercUpdate)
        {
            GameObject rootObject;

            if (gltfObject.UseBackgroundThread) await Update;

            if (gltfObject.Name != null && gltfObject.Name != string.Empty)
                rootObject = new GameObject(gltfObject.Name);
            else
                rootObject = new GameObject($"glTF Scene");

            rootObject.transform.position = Vector3.up * 2000f;

            if (gltfObject.UseBackgroundThread) await BackgroundThread;

            for (int i = 0; i < gltfObject.bufferViews?.Length; i++)
            {
                if (cancellationToken.IsCancellationRequested == true)
                    break;

                await Task.Run(() => gltfObject.ConstructBufferView(gltfObject.bufferViews[i]), cancellationToken);
            }

            for (int i = 0; i < gltfObject.textures?.Length; i++)
            {
                if (cancellationToken.IsCancellationRequested == true)
                    break;

                await gltfObject.ConstructTextureAsync(gltfObject.textures[i], cancellationToken);
            }

            for (int i = 0; i < gltfObject.scenes?.Length; i++)
            {
                if (cancellationToken.IsCancellationRequested == true)
                    break;

                gltfObject.ConstructSceneAccessorData(gltfObject.scenes[i]);
            }

            for (int i = 0; i < gltfObject.materials?.Length; i++)
            {
                if (cancellationToken.IsCancellationRequested == true)
                    break;

                await gltfObject.ConstructMaterialAsync(gltfObject.materials[i], i);
            }

            if (gltfObject.UseBackgroundThread) await Update;

            for (int i = 0; i < gltfObject.animations?.Length; i++)
            {
                if (cancellationToken.IsCancellationRequested == true)
                    break;

                gltfObject.ConstructAnimationAsync(gltfObject.animations[i], i);
            }

            if (gltfObject.scenes == null)
            {
                Debug.LogError($"No scenes found for {gltfObject.Name}");
            }

            for (int i = 0; i < gltfObject.scenes?.Length; i++)
            {
                if (cancellationToken.IsCancellationRequested == true)
                    break;

                await gltfObject.ConstructSceneAsync(gltfObject.scenes[i], rootObject, generateColliders, cancellationToken, funcProgressPercUpdate);
            }

            if (cancellationToken.IsCancellationRequested == true)
            {
                GameObject.Destroy(rootObject);
                throw new BSExceptionCancellationRequested();
            }

            rootObject.SetActive(false);
            rootObject.transform.position = Vector3.zero;
            return gltfObject.GameObjectReference = rootObject;
        }

        // ---------------------------------------------------------------------------

        private static void ConstructBufferView(this GltfObject gltfObject, GltfBufferView bufferView)
        {
            bufferView.Buffer = gltfObject.buffers[bufferView.buffer];

            if (bufferView.Buffer.BufferData == null &&
                !string.IsNullOrEmpty(gltfObject.Uri) &&
                !string.IsNullOrEmpty(bufferView.Buffer.uri))
            {

                if (!Path.HasExtension(bufferView.Buffer.uri))
                {
                    string base64Data = bufferView.Buffer.uri.Substring(bufferView.Buffer.uri.IndexOf(',') + 1);
                    bufferView.Buffer.BufferData = Convert.FromBase64String(base64Data);
                }
                else
                {
                    var parentDirectory = Directory.GetParent(gltfObject.Uri).FullName;
                    bufferView.Buffer.BufferData = File.ReadAllBytes($"{parentDirectory}\\{bufferView.Buffer.uri}");
                }
            }
        }

        // ---------------------------------------------------------------------------

        private static async Task ConstructTextureAsync(this GltfObject gltfObject, GltfTexture gltfTexture, CancellationToken cancellationToken)
        {
            if (gltfTexture.source >= 0)
            {
                GltfImage gltfImage = gltfObject.images[gltfTexture.source];

                byte[] imageData = null;
                Texture2D texture = null;

                if (!string.IsNullOrEmpty(gltfObject.Uri) && !string.IsNullOrEmpty(gltfImage.uri))
                {
                    var parentDirectory = Directory.GetParent(gltfObject.Uri).FullName;
                    var path = $"{parentDirectory}\\{gltfImage.uri}";

                    if (texture == null && !Path.HasExtension(gltfImage.uri))
                    {
                        string base64Data = gltfImage.uri.Substring(gltfImage.uri.IndexOf(',') + 1);
                        byte[] imgData = Convert.FromBase64String(base64Data);

                        if (gltfObject.UseBackgroundThread) await Update;
                        texture = new Texture2D(2, 2);
                        texture.LoadImage(imgData);
                        if (gltfObject.UseBackgroundThread) await BackgroundThread;
                    }

                    if (texture == null)
                    {
#if WINDOWS_UWP
                        if (gltfObject.UseBackgroundThread)
                        {
                            try
                            {
                                var storageFile = await Windows.Storage.StorageFile.GetFileFromPathAsync(path);

                                if (storageFile != null)
                                {

                                    var buffer = await Windows.Storage.FileIO.ReadBufferAsync(storageFile);

                                    using (Windows.Storage.Streams.DataReader dataReader = Windows.Storage.Streams.DataReader.FromBuffer(buffer))
                                    {
                                        imageData = new byte[buffer.Length];
                                        dataReader.ReadBytes(imageData);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.LogError(e.Message);
                            }
                        }
                        else
                        {
                            imageData = UnityEngine.Windows.File.ReadAllBytes(path);
                        }
#else
                        using (FileStream stream = File.Open(path, FileMode.Open))
                        {
                            imageData = new byte[stream.Length];

                            if (gltfObject.UseBackgroundThread)
                            {
                                await stream.ReadAsync(imageData, 0, (int)stream.Length, cancellationToken);
                            }
                            else
                            {
                                stream.Read(imageData, 0, (int)stream.Length);
                            }
                        }
#endif
                    }
                }
                else
                {
                    var imageBufferView = gltfObject.bufferViews[gltfImage.bufferView];
                    imageData = new byte[imageBufferView.byteLength];
                    Array.Copy(imageBufferView.Buffer.BufferData, imageBufferView.byteOffset, imageData, 0, imageData.Length);
                }

                if (texture == null)
                {
                    if (gltfObject.UseBackgroundThread) await Update;
                    // TODO Load texture async
                    texture = new Texture2D(2, 2);
                    gltfImage.Texture = texture;
                    gltfImage.Texture.LoadImage(imageData);
                }
                else
                {
                    gltfImage.Texture = texture;
                }

                gltfTexture.Texture = texture;

                if (gltfObject.UseBackgroundThread) await BackgroundThread;
            }
        }

        // ---------------------------------------------------------------------------

        private static void ConstructAnimationAsync(this GltfObject gltfObject, GltfAnimation gltfAnimation, int animationId)
        {
            gltfAnimation.animation = CreateAnimationClip(gltfObject, gltfAnimation, animationId);
        }

        // ---------------------------------------------------------------------------

        private static float GetCurveKeyframeLeftLinearSlope(Keyframe[] curveKeys, int keyframeIndex)
        {
            if (keyframeIndex <= 0 || keyframeIndex >= curveKeys.Length)
            {
                return 0;
            }

            var valueDelta = curveKeys[keyframeIndex].value - curveKeys[keyframeIndex - 1].value;
            var timeDelta = curveKeys[keyframeIndex].time - curveKeys[keyframeIndex - 1].time;

            Debug.Assert(timeDelta > 0, "Unity does not allow you to put two keyframes in with the same time, so this should never occur.");

            return valueDelta / timeDelta;
        }

        // ---------------------------------------------------------------------------

        private static void SetTangentMode(Keyframe[] curveKeys, int keyframeIndex, string interpolation)
        {
            var key = curveKeys[keyframeIndex];

            switch (interpolation)
            {
                case "CATMULLROMSPLINE":
                    key.inTangent = 0;
                    key.outTangent = 0;
                    break;
                case "LINEAR":
                    key.inTangent = GetCurveKeyframeLeftLinearSlope(curveKeys, keyframeIndex);
                    key.outTangent = GetCurveKeyframeLeftLinearSlope(curveKeys, keyframeIndex + 1);
                    break;
                case "STEP":
                    key.inTangent = float.PositiveInfinity;
                    key.outTangent = float.PositiveInfinity;
                    break;
                default:
                    throw new NotImplementedException();
            }

            curveKeys[keyframeIndex] = key;
        }

        // ---------------------------------------------------------------------------

        static void SetTranslationCurve(AnimationClip Clip, Vector3[] pos, float[] keyframeInput, string mode, string relativePath)
        {
            Keyframe[] keyframeArrayX, keyframeArrayY, keyframeArrayZ;

            keyframeArrayX = new Keyframe[keyframeInput.Length];
            keyframeArrayY = new Keyframe[keyframeInput.Length];
            keyframeArrayZ = new Keyframe[keyframeInput.Length];

            for (int k = 0; k < keyframeInput.Length; k++)
            {
                if (mode == "CUBICSPLINE")
                {
                    keyframeArrayX[k] = new Keyframe(keyframeInput[k], pos[k * 3 + 1].x, pos[k * 3].x, pos[k * 3 + 2].x);
                    keyframeArrayY[k] = new Keyframe(keyframeInput[k], pos[k * 3 + 1].y, pos[k * 3].y, pos[k * 3 + 2].y);
                    keyframeArrayZ[k] = new Keyframe(keyframeInput[k], pos[k * 3 + 1].z, pos[k * 3].z, pos[k * 3 + 2].z);
                }
                else
                {
                    keyframeArrayX[k] = new Keyframe(keyframeInput[k], pos[k].x);
                    keyframeArrayY[k] = new Keyframe(keyframeInput[k], pos[k].y);
                    keyframeArrayZ[k] = new Keyframe(keyframeInput[k], -pos[k].z);
                }
            }

            if (mode != "CUBICSPLINE")
            {
                for (int k = 0; k < keyframeInput.Length; k++)
                {
                    SetTangentMode(keyframeArrayX, k, mode);
                    SetTangentMode(keyframeArrayY, k, mode);
                    SetTangentMode(keyframeArrayZ, k, mode);
                }
            }

            AnimationCurve animationCurveX, animationCurveY, animationCurveZ;

            animationCurveX = new AnimationCurve(keyframeArrayX);
            animationCurveY = new AnimationCurve(keyframeArrayY);
            animationCurveZ = new AnimationCurve(keyframeArrayZ);

            Clip.SetCurve(relativePath, typeof(Transform), "localPosition.x", animationCurveX);
            Clip.SetCurve(relativePath, typeof(Transform), "localPosition.y", animationCurveY);
            Clip.SetCurve(relativePath, typeof(Transform), "localPosition.z", animationCurveZ);
        }

        // ---------------------------------------------------------------------------

        static void SetRotationCurve(AnimationClip Clip, Vector4[] rot, float[] keyframeInput, string mode, string relativePath)
        {
            Keyframe[] keyframeArrayX, keyframeArrayY, keyframeArrayZ, keyframeArrayW;

            keyframeArrayX = new Keyframe[keyframeInput.Length];
            keyframeArrayY = new Keyframe[keyframeInput.Length];
            keyframeArrayZ = new Keyframe[keyframeInput.Length];
            keyframeArrayW = new Keyframe[keyframeInput.Length];

            for (int k = 0; k < keyframeInput.Length; k++)
            {
                if (mode == "CUBICSPLINE")
                {
                    keyframeArrayX[k] = (new Keyframe(keyframeInput[k], rot[k * 3 + 1].x, rot[k * 3].x, rot[k * 3 + 2].x));
                    keyframeArrayY[k] = (new Keyframe(keyframeInput[k], rot[k * 3 + 1].y, rot[k * 3].y, rot[k * 3 + 2].y));
                    keyframeArrayZ[k] = (new Keyframe(keyframeInput[k], rot[k * 3 + 1].z, rot[k * 3].z, rot[k * 3 + 2].z));
                    keyframeArrayW[k] = (new Keyframe(keyframeInput[k], rot[k * 3 + 1].w, rot[k * 3].w, rot[k * 3 + 2].w));
                }
                else
                {
                    keyframeArrayX[k] = new Keyframe(keyframeInput[k], rot[k].x);
                    keyframeArrayY[k] = new Keyframe(keyframeInput[k], rot[k].y);
                    keyframeArrayZ[k] = new Keyframe(keyframeInput[k], -rot[k].z);
                    keyframeArrayW[k] = new Keyframe(keyframeInput[k], -rot[k].w);
                }
            }

            if (mode != "CUBICSPLINE")
            {
                for (int k = 0; k < keyframeInput.Length; k++)
                {

                    SetTangentMode(keyframeArrayX, k, mode);
                    SetTangentMode(keyframeArrayY, k, mode);
                    SetTangentMode(keyframeArrayZ, k, mode);
                    SetTangentMode(keyframeArrayW, k, mode);
                }
            }

            AnimationCurve animationCurveX, animationCurveY, animationCurveZ, animationCurveW;

            animationCurveX = new AnimationCurve(keyframeArrayX);
            animationCurveY = new AnimationCurve(keyframeArrayY);
            animationCurveZ = new AnimationCurve(keyframeArrayZ);
            animationCurveW = new AnimationCurve(keyframeArrayW);

            Clip.SetCurve(relativePath, typeof(Transform), "localRotation.x", animationCurveX);
            Clip.SetCurve(relativePath, typeof(Transform), "localRotation.y", animationCurveY);
            Clip.SetCurve(relativePath, typeof(Transform), "localRotation.z", animationCurveZ);
            Clip.SetCurve(relativePath, typeof(Transform), "localRotation.w", animationCurveW);
        }

        // ---------------------------------------------------------------------------

        static void SetScaleCurve(AnimationClip Clip, Vector3[] scale, float[] keyframeInput, string mode, string relativePath)
        {
            Keyframe[] keyframeArrayX, keyframeArrayXY, keyframeArrayXZ;

            keyframeArrayX = new Keyframe[keyframeInput.Length];
            keyframeArrayXY = new Keyframe[keyframeInput.Length];
            keyframeArrayXZ = new Keyframe[keyframeInput.Length];

            for (int k = 0; k < keyframeInput.Length; k++)
            {
                keyframeArrayX[k] = new Keyframe(keyframeInput[k], scale[k].x);
                keyframeArrayXY[k] = new Keyframe(keyframeInput[k], scale[k].y);
                keyframeArrayXZ[k] = new Keyframe(keyframeInput[k], scale[k].z);
            }

            if (mode != "CUBICSPLINE")
            {
                for (int k = 0; k < keyframeInput.Length; k++)
                {

                    SetTangentMode(keyframeArrayX, k, mode);
                    SetTangentMode(keyframeArrayXY, k, mode);
                    SetTangentMode(keyframeArrayXZ, k, mode);
                }
            }

            AnimationCurve animationCurveX, animationCurveY, animationCurveZ;

            animationCurveX = new AnimationCurve(keyframeArrayX);
            animationCurveY = new AnimationCurve(keyframeArrayXY);
            animationCurveZ = new AnimationCurve(keyframeArrayXZ);

            Clip.SetCurve(relativePath, typeof(Transform), "localScale.x", animationCurveX);
            Clip.SetCurve(relativePath, typeof(Transform), "localScale.y", animationCurveY);
            Clip.SetCurve(relativePath, typeof(Transform), "localScale.z", animationCurveZ);
        }

        // ---------------------------------------------------------------------------

        static void SetWeightsCurve(AnimationClip Clip, float[] weights, float[] keyframeInput, string mode, string relativePath)
        {
            int curveNum;

            curveNum = weights.Length / keyframeInput.Length;

            for (int i = 0; i < curveNum; i++)
            {
                Keyframe[] keyframeArray;

                keyframeArray = new Keyframe[keyframeInput.Length];

                for (int k = 0; k < keyframeInput.Length; k++)
                {
                    int weightIndex;

                    weightIndex = curveNum == 1 ? k : k * 2 + i;

                    keyframeArray[k] = new Keyframe(keyframeInput[k], weights[weightIndex] * 100f);
                }

                if (mode != "CUBICSPLINE")
                {
                    for (int k = 0; k < keyframeInput.Length; k++)
                    {
                        SetTangentMode(keyframeArray, k, mode);
                    }
                }

                AnimationCurve animationCurve;

                animationCurve = new AnimationCurve(keyframeArray);

                Clip.SetCurve(relativePath, typeof(SkinnedMeshRenderer), "blendShape." + "BlendShape" + i, animationCurve);
            }
        }

        // ---------------------------------------------------------------------------

        private static void FlipXZ(Vector3[] positions)
        {
            for (int j = 0; j < positions.Length; j++)
            {
                positions[j].x *= -1f;
                positions[j].z *= -1f;
            }
        }

        private static AnimationClip CreateAnimationClip(GltfObject gltfObject, GltfAnimation gltfAnimation, int animationId)
        {
            AnimationClip Clip = new AnimationClip();
            Clip.wrapMode = WrapMode.Loop;
            Clip.legacy = true;
            Clip.name = $"{gltfObject.Name} Animation";

            for (int i = 0; i < gltfObject.nodes.Length; i++)
            {
                gltfObject.nodes[i].name = string.IsNullOrEmpty(gltfObject.nodes[i].name) ? $"glTF Node {i}" : gltfObject.nodes[i].name;
            }

            for (int i = 0; i < gltfAnimation.channels.Length; i++)
            {
                GltfAnimationChannel channel = gltfAnimation.channels[i];
                if (gltfAnimation.samplers.Length <= channel.sampler)
                {
                    Debug.LogWarning("Animation channel points to sampler at index " + channel.sampler + " which doesn't exist. Skipping animation clip.");
                    continue;
                }

                GltfAnimationSampler sampler = gltfAnimation.samplers[channel.sampler];

                GltfAccessor inputAccessor = null;
                inputAccessor = gltfObject.accessors[sampler.input];
                inputAccessor.BufferView = gltfObject.bufferViews[inputAccessor.bufferView];
                inputAccessor.BufferView.Buffer = gltfObject.buffers[inputAccessor.BufferView.buffer];

                GltfAccessor outputAccessor = null;
                outputAccessor = gltfObject.accessors[sampler.output];
                outputAccessor.BufferView = gltfObject.bufferViews[outputAccessor.bufferView];
                outputAccessor.BufferView.Buffer = gltfObject.buffers[outputAccessor.BufferView.buffer];

                string relativePath = "";

                float[] keyframeInput = inputAccessor.GetFloatArray(false);
                switch (channel.target.path)
                {
                    case "translation":
                        Vector3[] pos = outputAccessor.GetVector3Array(false);
                        FlipXZ(pos);
                        SetTranslationCurve(Clip, pos, keyframeInput, sampler.interpolation, relativePath);
                        break;
                    case "rotation":
                        Vector4[] rot = outputAccessor.GetVector4Array(false);
                        SetRotationCurve(Clip, rot, keyframeInput, sampler.interpolation, relativePath);
                        break;
                    case "scale":
                        Vector3[] scale = outputAccessor.GetVector3Array(false);
                        SetScaleCurve(Clip, scale, keyframeInput, sampler.interpolation, relativePath);
                        break;
                    case "weights":
                        float[] weights = outputAccessor.GetFloatArray(false);
                        SetWeightsCurve(Clip, weights, keyframeInput, sampler.interpolation, relativePath);
                        break;
                }
            }

            return Clip;
        }

        // ---------------------------------------------------------------------------

        private static async Task ConstructMaterialAsync(this GltfObject gltfObject, GltfMaterial gltfMaterial, int materialId)
        {
            if (gltfObject.UseBackgroundThread) await Update;

            Material material = await CreateBIMServerCenterShaderMaterial(gltfObject, gltfMaterial, materialId);
            if (material == null)
            {
                Debug.LogWarning("The BIMserver.center Toolkit/Standard Shader was not found. Falling back to Standard Shader");
                material = await CreateStandardShaderMaterial(gltfObject, gltfMaterial, materialId);
            }

            if (material == null)
            {
                Debug.LogWarning("The Standard Shader was not found. Failed to create material for glTF object");
            }
            else
            {
                gltfMaterial.Material = material;
            }

            if (gltfObject.UseBackgroundThread) await BackgroundThread;
        }

        // ---------------------------------------------------------------------------

        private static async Task<Material> CreateBIMServerCenterShaderMaterial(GltfObject gltfObject, GltfMaterial gltfMaterial, int materialId)
        {
            var shader = Shader.Find("BIMserver.center Toolkit/Standard");

            if (shader == null) { return null; }

            var material = new Material(shader)
            {
                name = string.IsNullOrEmpty(gltfMaterial.name) ? $"glTF Material {materialId}" : gltfMaterial.name
            };

            if (gltfMaterial.pbrMetallicRoughness.baseColorTexture != null && gltfMaterial.pbrMetallicRoughness.baseColorTexture.index >= 0)
            {
                material.mainTexture = gltfObject.images[gltfMaterial.pbrMetallicRoughness.baseColorTexture.index].Texture;
            }
            else
            {
                material.mainTexture = null;
            }

            material.color = gltfMaterial.pbrMetallicRoughness.baseColorFactor.GetColorValue();

            if (gltfMaterial.alphaMode == "MASK")
            {
                material.SetInt(SrcBlendId, (int)BlendMode.One);
                material.SetInt(DstBlendId, (int)BlendMode.Zero);
                material.SetInt(ZWriteId, 1);
                material.SetInt(ModeId, 3);
                material.SetOverrideTag("RenderType", "Cutout");
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 2450;
            }
            else if (gltfMaterial.alphaMode == "BLEND")
            {
                material.SetInt(SrcBlendId, (int)BlendMode.SrcAlpha);
                material.SetInt(DstBlendId, (int)BlendMode.OneMinusSrcAlpha);
                material.SetInt(ZWriteId, 0);
                material.SetInt(ModeId, 2);
                material.SetOverrideTag("RenderType", "Fade");
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
            }

            if (gltfMaterial.emissiveTexture.index >= 0 && material.HasProperty("_EmissionMap"))
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor(EmissiveColorId, gltfMaterial.emissiveFactor.GetColorValue());
            }

            if (gltfMaterial.pbrMetallicRoughness.metallicRoughnessTexture != null && gltfMaterial.pbrMetallicRoughness.metallicRoughnessTexture.index >= 0)
            {
                var texture = gltfObject.images[gltfMaterial.pbrMetallicRoughness.metallicRoughnessTexture.index].Texture;

                Texture2D occlusionTexture = null;
                if (gltfMaterial.occlusionTexture.index >= 0)
                {
                    occlusionTexture = gltfObject.images[gltfMaterial.occlusionTexture.index].Texture;
                }

                if (texture.isReadable)
                {
                    var pixels = texture.GetPixels();
                    Color[] occlusionPixels = null;
                    if (occlusionTexture != null &&
                        occlusionTexture.isReadable)
                    {
                        occlusionPixels = occlusionTexture.GetPixels();
                    }

                    if (gltfObject.UseBackgroundThread) await BackgroundThread;

                    var pixelCache = new Color[pixels.Length];

                    for (int c = 0; c < pixels.Length; c++)
                    {
                        pixelCache[c].r = pixels[c].b; // Metallic value, glTF metallic value
                        pixelCache[c].g = occlusionPixels?[c].r ?? 1.0f; // Occlusion value, glTF occlusion value if available
                        pixelCache[c].b = 0f; // Emission value
                        pixelCache[c].a = (1.0f - pixels[c].g); // Smoothness value, invert of glTF roughness value
                    }

                    if (gltfObject.UseBackgroundThread) await Update;
                    texture.SetPixels(pixelCache);
                    texture.Apply();

                    material.SetTexture(ChannelMapId, texture);
                    material.EnableKeyword("_CHANNEL_MAP");
                }
                else
                {
                    material.DisableKeyword("_CHANNEL_MAP");
                }

                material.SetFloat(SmoothnessId, Mathf.Abs((float)gltfMaterial.pbrMetallicRoughness.roughnessFactor - 1f));
                material.SetFloat(MetallicId, (float)gltfMaterial.pbrMetallicRoughness.metallicFactor);
            }


            if (gltfMaterial.normalTexture.index >= 0)
            {
                material.SetTexture(NormalMapId, gltfObject.images[gltfMaterial.normalTexture.index].Texture);
                material.SetFloat(NormalMapScaleId, (float)gltfMaterial.normalTexture.scale);
                material.EnableKeyword("_NORMAL_MAP");
            }

            if (gltfMaterial.doubleSided)
            {
                material.SetFloat(CullModeId, (float)UnityEngine.Rendering.CullMode.Off);
            }

            if (gltfMaterial.extras != null && gltfMaterial.extras.bsPolygonOffset != null)
            {
                float offsetFactor, offsetUnits;

                offsetFactor = gltfMaterial.extras.bsPolygonOffset.factor;
                offsetUnits = gltfMaterial.extras.bsPolygonOffset.units;

                material.SetFloat(OffsetFactorId, offsetFactor);
                material.SetFloat(OffsetUnitsId, offsetUnits);
            }

            material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            return material;
        }

        // ---------------------------------------------------------------------------

        private static async Task<Material> CreateStandardShaderMaterial(GltfObject gltfObject, GltfMaterial gltfMaterial, int materialId)
        {
            var shader = Shader.Find("Standard");

            if (shader == null) { return null; }

            var material = new Material(shader)
            {
                name = string.IsNullOrEmpty(gltfMaterial.name) ? $"glTF Material {materialId}" : gltfMaterial.name
            };

            if (gltfMaterial.pbrMetallicRoughness.baseColorTexture != null && gltfMaterial.pbrMetallicRoughness.baseColorTexture.index >= 0)
            {
                material.mainTexture = gltfObject.images[gltfMaterial.pbrMetallicRoughness.baseColorTexture.index].Texture;
            }
            else
            {
                material.mainTexture = null;
            }

            material.color = gltfMaterial.pbrMetallicRoughness.baseColorFactor.GetColorValue();

            if (gltfMaterial.alphaMode == "MASK")
            {
                material.SetInt(SrcBlendId, (int)BlendMode.One);
                material.SetInt(DstBlendId, (int)BlendMode.Zero);
                material.SetInt(ZWriteId, 1);
                material.SetInt(ModeId, 3);
                material.SetOverrideTag("RenderType", "Cutout");
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 2450;
            }
            else if (gltfMaterial.alphaMode == "BLEND")
            {
                material.SetInt(SrcBlendId, (int)BlendMode.One);
                material.SetInt(DstBlendId, (int)BlendMode.OneMinusSrcAlpha);
                material.SetInt(ZWriteId, 0);
                material.SetInt(ModeId, 3);
                material.SetOverrideTag("RenderType", "Transparency");
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
            }

            if (gltfMaterial.emissiveTexture.index >= 0)
            {
                material.EnableKeyword("_EmissionMap");
                material.EnableKeyword("_EMISSION");
                material.SetTexture(EmissionMapId, gltfObject.images[gltfMaterial.emissiveTexture.index].Texture);
                material.SetColor(EmissionColorId, gltfMaterial.emissiveFactor.GetColorValue());
            }

            if (gltfMaterial.pbrMetallicRoughness.metallicRoughnessTexture != null && gltfMaterial.pbrMetallicRoughness.metallicRoughnessTexture.index >= 0)
            {
                var texture = gltfObject.images[gltfMaterial.pbrMetallicRoughness.metallicRoughnessTexture.index].Texture;

                if (texture.isReadable)
                {
                    var pixels = texture.GetPixels();
                    if (gltfObject.UseBackgroundThread) await BackgroundThread;

                    var pixelCache = new Color[pixels.Length];

                    for (int c = 0; c < pixels.Length; c++)
                    {
                        // Unity only looks for metal in R channel, and smoothness in A.
                        pixelCache[c].r = pixels[c].g;
                        pixelCache[c].g = 0f;
                        pixelCache[c].b = 0f;
                        pixelCache[c].a = pixels[c].b;
                    }

                    if (gltfObject.UseBackgroundThread) await Update;
                    texture.SetPixels(pixelCache);
                    texture.Apply();

                    material.SetTexture(MetallicGlossMapId, texture);
                }

                material.SetFloat(GlossinessId, Mathf.Abs((float)gltfMaterial.pbrMetallicRoughness.roughnessFactor - 1f));
                material.SetFloat(MetallicId, (float)gltfMaterial.pbrMetallicRoughness.metallicFactor);
                material.EnableKeyword("_MetallicGlossMap");
                material.EnableKeyword("_METALLICGLOSSMAP");
            }

            if (gltfMaterial.normalTexture.index >= 0)
            {
                material.SetTexture(BumpMapId, gltfObject.images[gltfMaterial.normalTexture.index].Texture);
                material.EnableKeyword("_BumpMap");
            }

            material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            return material;
        }

        // ---------------------------------------------------------------------------

        private static void ConstructSceneAccessorData(this GltfObject gltfObject, GltfScene gltfScene)
        {
            for (int i = 0; i < gltfScene.nodes.Length; i++)
            {
                // Note: glTF objects are currently imported with their original scale from the glTF scene, which may apply an unexpected transform
                // to the root node. If this behavior needs to be changed, functionality should be added below to ConstructNodeAsync
                ConstructNodeAccessorData(gltfObject, gltfObject.nodes[gltfScene.nodes[i]], gltfScene.nodes[i]);
            }
        }

        // ---------------------------------------------------------------------------

        private static async Task ConstructSceneAsync(
                        this GltfObject gltfObject,
                        GltfScene gltfScene,
                        GameObject root,
                        bool generateColliders,
                        CancellationToken cancellationToken,
                        FuncProgressPercUpdate funcProgressPercUpdate)
        {
            await ConstructSceneRoutine(gltfObject, gltfScene, root, generateColliders, cancellationToken, funcProgressPercUpdate);
        }

        // ---------------------------------------------------------------------------

        private static IEnumerator CycleThroughActionArray(
                        List<Action> createActions,
                        int actionsPerFrame,
                        CancellationToken cancellationToken,
                        FuncProgressIndexesUpdate funcProgressIndexesUpdate)
        {
            for (int i = 0; i < createActions.Count; i = i + actionsPerFrame)
            {
                int max;

                if (cancellationToken.IsCancellationRequested)
                    break;

                max = Mathf.Clamp(i + actionsPerFrame, 0, createActions.Count);

                for (int j = i; j < max; j++)
                {
                    createActions[j].Invoke();
                }

                funcProgressIndexesUpdate?.Invoke(max - i, max);

                yield return 0;
            }
        }

        // ---------------------------------------------------------------------------

        private static async Task ConstrucSceneElements(
                        List<Action> createGameobjectActions,
                        List<Action> createMeshActions,
                        List<Action> createAnimationActions,
                        List<Action> createExtraActions,
                        CancellationToken cancellationToken,
                        FuncProgressPercUpdate funcProgressPercUpdate)
        {
            int numSteps;
            int currentSteps;
            int actionsPerFrame;
            FuncProgressIndexesUpdate funcProgressIndexesUpdate;

            numSteps = createGameobjectActions.Count + createMeshActions.Count + createAnimationActions.Count + createExtraActions.Count;
            currentSteps = 0;

            funcProgressIndexesUpdate = (actualIndex, total) =>
            {
                currentSteps += actualIndex;
                funcProgressPercUpdate?.Invoke((float)currentSteps / (float)numSteps);
            };

            funcProgressPercUpdate?.Invoke(0f);

            actionsPerFrame = 40;
            await CycleThroughActionArray(createGameobjectActions, actionsPerFrame, cancellationToken, funcProgressIndexesUpdate);

            actionsPerFrame = 20;
            await CycleThroughActionArray(createMeshActions, actionsPerFrame, cancellationToken, funcProgressIndexesUpdate);

            actionsPerFrame = 60;
            await CycleThroughActionArray(createAnimationActions, actionsPerFrame, cancellationToken, funcProgressIndexesUpdate);

            actionsPerFrame = 40;
            await CycleThroughActionArray(createExtraActions, actionsPerFrame, cancellationToken, funcProgressIndexesUpdate);

            funcProgressPercUpdate?.Invoke(1f);
        }

        // ---------------------------------------------------------------------------

        private static async Task ConstructSceneRoutine(
                        GltfObject gltfObject,
                        GltfScene gltfScene,
                        GameObject root,
                        bool generateColliders,
                        CancellationToken cancellationToken,
                        FuncProgressPercUpdate funcProgressPercUpdate)
        {
            List<Action> createGameobjectActions, createMeshActions, createAnimationActions, createExtraActions;

            createGameobjectActions = new List<Action>();
            createMeshActions = new List<Action>();
            createAnimationActions = new List<Action>();
            createExtraActions = new List<Action>();

            GltfNode obj = new GltfNode();
            obj.GameObjectReference = root;

            for (int i = 0; i < gltfScene.nodes.Length; i++)
            {
                ConstructNode(gltfObject, gltfObject.nodes[gltfScene.nodes[i]], gltfScene.nodes[i], obj, generateColliders, createGameobjectActions, createMeshActions, createAnimationActions, createExtraActions);
            }

            await ConstrucSceneElements(
                        createGameobjectActions,
                        createMeshActions,
                        createAnimationActions,
                        createExtraActions,
                        cancellationToken,
                        funcProgressPercUpdate);
        }

        // ---------------------------------------------------------------------------

        private static void ConstructNodeAccessorData(GltfObject gltfObject, GltfNode node, int nodeId)
        {
            if (node.mesh >= 0)
            {
                ConstructMeshAccessorData(gltfObject, node.mesh);
            }

            if (node.children != null)
            {
                for (int i = 0; i < node.children.Length; i++)
                {
                    ConstructNodeAccessorData(gltfObject, gltfObject.nodes[node.children[i]], node.children[i]);
                }
            }
        }

        // ---------------------------------------------------------------------------

        private static void ConstructExtra(GltfObject gltfObject, GltfNode node)
        {
            Bsvbimdescription[] bimServerCenterDescriptions;

            bimServerCenterDescriptions = gltfObject.extras.bsvbimdescriptions;

            if (bimServerCenterDescriptions != null && bimServerCenterDescriptions.Length > 0)
            {
                BSGltfExtras glTFExtras;

                glTFExtras = new BSGltfExtras();
                glTFExtras.ID = node.extras.bsvdescidx;
                glTFExtras.Name = bimServerCenterDescriptions[glTFExtras.ID].bsvname;
                glTFExtras.GUID = bimServerCenterDescriptions[glTFExtras.ID].bsvguid;

                if (bimServerCenterDescriptions[glTFExtras.ID].bsvgenericattrs != null)
                {
                    int numElements;

                    numElements = bimServerCenterDescriptions[glTFExtras.ID].bsvgenericattrs.Length;

                    for (int i = 0; i < numElements; i++)
                    {
                        Bsvgenericattr attribute;

                        attribute = bimServerCenterDescriptions[glTFExtras.ID].bsvgenericattrs[i];

                        switch (attribute.type)
                        {
                            case Bsvgenericattr.CodingKeys.grouptitle:
                                {
                                    string groupText;
                                    TextLine textLine;

                                    groupText = attribute.grouptitle.text;
                                    textLine = new TextLine(groupText, null);

                                    glTFExtras.TextLines.Add(textLine);
                                    break;
                                }

                            case Bsvgenericattr.CodingKeys.textline:
                                {
                                    string text;
                                    TextLine textLine;

                                    text = attribute.textline.text;
                                    textLine = new TextLine(null, text);

                                    glTFExtras.TextLines.Add(textLine);
                                    break;
                                }

                            case Bsvgenericattr.CodingKeys.keyvalue:
                                {
                                    string key, value;
                                    KeyValue keyValue;

                                    key = attribute.keyvalue.name;
                                    value = attribute.keyvalue.value;
                                    keyValue = new KeyValue(key, value);

                                    glTFExtras.KeyValues.Add(keyValue);
                                    break;
                                }
                        }
                    }
                }

                glTFExtras.GameObject = node.GameObjectReference;
                gltfObject.RegisteredExtras.Add(glTFExtras);
            }
        }

        // ---------------------------------------------------------------------------

        private static void ConstructAnimation(GltfObject gltfObject, GltfNode node, int animationIndex)
        {
            Animation animationComponent;

            animationComponent = node.GameObjectReference.GetComponent<Animation>();
            if (animationComponent == null)
                animationComponent = node.GameObjectReference.AddComponent<Animation>();

            animationComponent.playAutomatically = false;
            animationComponent.AddClip(gltfObject.animations[animationIndex].animation, gltfObject.animations[animationIndex].animation.name);
            animationComponent.clip = gltfObject.animations[animationIndex].animation;

            gltfObject.RegisteredAnimations.Add(animationComponent);
        }

        // ---------------------------------------------------------------------------

        private static void ConstructGameObject(GltfNode node, int nodeId, Transform parent)
        {
            var nodeName = string.IsNullOrEmpty(node.name) ? $"glTF Node {nodeId}" : node.name;
            var nodeGameObject = node.GameObjectReference = new GameObject(nodeName);
            nodeGameObject.transform.SetParent(parent, false);

            node.Matrix = node.GetTrsProperties(out Vector3 position, out Quaternion rotation, out Vector3 scale);

            if (node.Matrix == Matrix4x4.identity)
            {
                if (node.translation != null)
                {
                    position = node.translation.GetVector3Value();
                }

                if (node.rotation != null)
                {
                    rotation = node.rotation.GetQuaternionValue();
                }

                if (node.scale != null)
                {
                    scale = node.scale.GetVector3Value(false);
                }
            }

            nodeGameObject.transform.localPosition = position;
            nodeGameObject.transform.localRotation = rotation;
            nodeGameObject.transform.localScale = scale;
        }

        // ---------------------------------------------------------------------------

        private static void ConstructNode(GltfObject gltfObject, GltfNode node, int nodeId, GltfNode parentNode, bool generateColliders,
                                          List<Action> constructGameobject,
                                          List<Action> constructMesh,
                                          List<Action> constructAnimation,
                                          List<Action> costructExtras)
        {
            constructGameobject.Add(() => ConstructGameObject(node, nodeId, parentNode.GameObjectReference.transform));

            if (node.mesh >= 0)
            {
                constructMesh.Add(async () => await ConstructMesh(gltfObject, node.GameObjectReference, node.mesh, generateColliders));

                for (int i = 0; i < gltfObject.animations?.Length; i++)
                {
                    int animationIndex = i;
                    foreach (var item in gltfObject.animations[animationIndex].channels)
                    {
                        if (item.target.node == nodeId)
                        {
                            constructAnimation.Add(() => ConstructAnimation(gltfObject, node, animationIndex));
                        }
                    }
                }
            }

            if (node.extras.bsvdescidx > -1)
                costructExtras.Add(() => ConstructExtra(gltfObject, node));

            if (node.children != null)
            {
                for (int i = 0; i < node.children.Length; i++)
                {
                    ConstructNode(gltfObject, gltfObject.nodes[node.children[i]], node.children[i], node, generateColliders, constructGameobject, constructMesh, constructAnimation, costructExtras);
                }
            }
        }

        // ---------------------------------------------------------------------------

        private static List<Mesh> GetSameMaterialMeshes(List<Mesh> meshPrimitives, GltfObject gltfObject, GltfMesh gltfMesh, Material material)
        {
            List<Mesh> returnList = new List<Mesh>();
            for (int j = 0; j < meshPrimitives.Count; j++)
            {
                var meshMaterial = gltfObject.materials[gltfMesh.primitives[j].material].Material;
                if (meshMaterial.Equals(material))
                {
                    returnList.Add(meshPrimitives[j]);
                }
            }

            return returnList;
        }

        // ---------------------------------------------------------------------------

        private static Mesh CombineMeshList(List<Mesh> toMerge, Transform parent, bool mergeSubMeshes, Material material = null, List<Material> materialList = null)
        {
            List<Mesh> meshesTopologyTriangles;
            List<Mesh> meshesTopologyNotTriangles;

            meshesTopologyTriangles = new List<Mesh>();
            meshesTopologyNotTriangles = new List<Mesh>();

            for (int i = 0; i < toMerge.Count; i++)
            {
                Mesh mesh;
                MeshTopology meshTopology;

                mesh = toMerge[i];
                meshTopology = mesh.GetTopology(0);

                switch (meshTopology)
                {
                    case MeshTopology.Quads:
                    case MeshTopology.Lines:
                    case MeshTopology.Points:
                    case MeshTopology.LineStrip:
                        {
                            GameObject primitive;

                            primitive = new GameObject($"Primitive {i}");
                            primitive.transform.SetParent(parent, false);

                            primitive.AddComponent<MeshFilter>().sharedMesh = mesh;
                            primitive.AddComponent<MeshRenderer>().material = material ?? materialList[i];

                            meshesTopologyNotTriangles.Add(mesh);
                            break;
                        }

                    case MeshTopology.Triangles:
                        meshesTopologyTriangles.Add(mesh);
                        break;
                }
            }

            if (meshesTopologyTriangles.Count > 0)
            {
                Mesh combinedMesh;
                CombineInstance[] combinedInstances;

                combinedMesh = new Mesh();
                combinedInstances = new CombineInstance[meshesTopologyTriangles.Count];

                for (int i = 0; i < meshesTopologyTriangles.Count; i++)
                {
                    combinedInstances[i].mesh = meshesTopologyTriangles[i];
                    combinedInstances[i].transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
                }

                combinedMesh.CombineMeshes(combinedInstances, mergeSubMeshes);

                return combinedMesh;
            }

            return null;
        }

        // ---------------------------------------------------------------------------

        static void SetBlendShapeWeights(SkinnedMeshRenderer smr, GltfMesh mesh)
        {
            if (mesh.weights == null || mesh.weights.Length == 0)
            {
                return;
            }
            int blendshapeCount = smr.sharedMesh.blendShapeCount;
            if (blendshapeCount != mesh.weights.Length)
            {
                Debug.LogWarning("GLTFMesh weights count does not match Unity mesh blendshape count! Using minimum between GLTFMesh weights: " + mesh.weights.Length + ", Unity blendshapes: " + blendshapeCount);
                blendshapeCount = Mathf.Min(blendshapeCount, mesh.weights.Length);
            }
            for (int i = 0; i < blendshapeCount; ++i)
            {
                // GLTF weights are [0, 1] range but Unity weights are [0, 100] range
                smr.SetBlendShapeWeight(i, (float)(mesh.weights[i] * 100));
            }
        }

        // ---------------------------------------------------------------------------

        private static void ConstructMeshAccessorData(GltfObject gltfObject, int meshId)
        {
            GltfMesh gltfMesh;
            int numElements;

            gltfMesh = gltfObject.meshes[meshId];
            numElements = gltfMesh.primitives.Length;

            for (int i = 0; i < numElements; i++)
            {
                GltfMeshPrimitive gltfMeshPrimitive;

                gltfMeshPrimitive = gltfMesh.primitives[i];

                ConstructMeshPrimitiveAccessorData(gltfObject, gltfMeshPrimitive);
            }
        }

#if UNITY_2018
#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará｡ de forma sincrónica
#endif

        private static async Task ConstructMesh(GltfObject gltfObject, GameObject parent, int meshId, bool generateColliders)
        {
            GltfMesh gltfMesh;
            int numElements;
            List<Mesh> meshPrimitiveList;
            List<Material> materialList;
            bool isSkinnedMesh;

            gltfMesh = gltfObject.meshes[meshId];
            numElements = gltfMesh.primitives.Length;
            meshPrimitiveList = new List<Mesh>();
            materialList = new List<Material>();
            isSkinnedMesh = false;

            for (int i = 0; i < numElements; i++)
            {
                GltfMeshPrimitive gltfMeshPrimitive;
                Material meshPrimitiveMaterial;

                gltfMeshPrimitive = gltfMesh.primitives[i];
                meshPrimitiveMaterial = gltfObject.materials[gltfMeshPrimitive.material].Material;

                if (materialList.Contains(meshPrimitiveMaterial) == false)
                    materialList.Add(meshPrimitiveMaterial);

                if (gltfMeshPrimitive.Targets != null)
                    isSkinnedMesh = true;

                meshPrimitiveList.Add(ConstructMeshPrimitiveAsync(gltfObject, gltfMeshPrimitive));
            }

            List<Mesh> meshesSameMaterialCombined;

            numElements = materialList.Count;
            meshesSameMaterialCombined = new List<Mesh>();

            for (int i = 0; i < numElements; i++)
            {
                Material material;
                List<Mesh> meshesWithSameMaterial;
                Mesh meshesCombinedWithSameMaterial;

                material = materialList[i];
                meshesWithSameMaterial = GetSameMaterialMeshes(meshPrimitiveList, gltfObject, gltfMesh, material);

                if (meshesWithSameMaterial.Count > 1)
                    meshesCombinedWithSameMaterial = CombineMeshList(meshesWithSameMaterial, parent.transform, true, material);
                else if (meshesWithSameMaterial.Count > 0)
                    meshesCombinedWithSameMaterial = meshesWithSameMaterial[0];
                else
                    meshesCombinedWithSameMaterial = null;

                if (meshesCombinedWithSameMaterial != null)
                    meshesSameMaterialCombined.Add(meshesCombinedWithSameMaterial);
            }

            {
                Mesh combinedMesh;

                if (meshesSameMaterialCombined.Count > 1)
                    combinedMesh = CombineMeshList(meshesSameMaterialCombined, parent.transform, false, null, materialList);
                else if (meshesSameMaterialCombined.Count > 0)
                    combinedMesh = meshesSameMaterialCombined[0];
                else
                    combinedMesh = null;

                if (combinedMesh != null)
                {
                    gltfMesh.Mesh = combinedMesh;
                    gltfMesh.Mesh.name = gltfMesh.name;

                    if (isSkinnedMesh == true)
                    {
                        SkinnedMeshRenderer skinnedMeshRenderer;

                        skinnedMeshRenderer = parent.AddComponent<SkinnedMeshRenderer>();
                        skinnedMeshRenderer.materials = materialList.ToArray();
                        skinnedMeshRenderer.quality = SkinQuality.Auto;
                        skinnedMeshRenderer.sharedMesh = gltfMesh.Mesh;

                        SetBlendShapeWeights(skinnedMeshRenderer, gltfMesh);

                        gltfObject.RegisteredRenderers.Add(skinnedMeshRenderer);
                    }
                    else
                    {
                        MeshRenderer meshRenderer;

                        meshRenderer = parent.AddComponent<MeshRenderer>();
                        meshRenderer.materials = materialList.ToArray();

                        gltfObject.RegisteredRenderers.Add(meshRenderer);
                    }

                    MeshFilter meshFilter;
                    meshFilter = parent.AddComponent<MeshFilter>();
                    meshFilter.sharedMesh = gltfMesh.Mesh;

                    if (generateColliders == true)
                    {
                        Collider objectCollider;

                        if (gltfMesh.Mesh.GetTopology(0) == MeshTopology.Triangles)
                        {
                            int meshID;

                            meshID = gltfMesh.Mesh.GetInstanceID();

#if UNITY_2019_3_OR_NEWER
                            await new WaitForBackgroundThread();

                            Physics.BakeMesh(meshID, false);

                            await new WaitForUpdate();
#endif

                            objectCollider = parent.AddComponent<MeshCollider>();
                        }
                        else
                        {
                            objectCollider = parent.AddComponent<BoxCollider>();
                        }

                        gltfObject.RegisteredColliders.Add(objectCollider);
                    }
                }
            }
        }

#if UNITY_2018
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará｡ de forma sincrónica
#endif

        public static int[] StripToIndex(int[] mainIndex, bool flip)
        {
            if (mainIndex.Length < 3) return new int[0];

            int total = (mainIndex.Length - 2) * 3;
            int[] index = new int[total];

            int count = 0;

            for (int i = 2; i < mainIndex.Length; i++)
            {
                if (flip)
                {
                    index[count] = mainIndex[i]; count++;
                    index[count] = mainIndex[i - 1]; count++;
                    index[count] = mainIndex[i - 2]; count++;
                }
                else
                {
                    index[count] = mainIndex[i - 2]; count++;
                    index[count] = mainIndex[i - 1]; count++;
                    index[count] = mainIndex[i]; count++;
                }

                flip = !flip;
            }
            return index;
        }

        // ---------------------------------------------------------------------------

        private static void ConstructMeshPrimitiveAccessorData(GltfObject gltfObject, GltfMeshPrimitive meshPrimitive)
        {
            GltfAccessor positionAccessor = null;
            GltfAccessor normalsAccessor = null;
            GltfAccessor textCoord0Accessor = null;
            GltfAccessor textCoord1Accessor = null;
            GltfAccessor textCoord2Accessor = null;
            GltfAccessor textCoord3Accessor = null;
            GltfAccessor colorAccessor = null;
            GltfAccessor indicesAccessor = null;
            GltfAccessor tangentAccessor = null;
            GltfAccessor weight0Accessor = null;
            GltfAccessor joint0Accessor = null;
            int vertexCount = 0;

            GltfAccessor[] targetPositionAccessor = null;
            GltfAccessor[] targetNormalsAccessor = null;
            GltfAccessor[] targetTangentAccessor = null;

            if (meshPrimitive.Attributes.POSITION >= 0)
            {
                positionAccessor = gltfObject.accessors[meshPrimitive.Attributes.POSITION];
                positionAccessor.BufferView = gltfObject.bufferViews[positionAccessor.bufferView];
                positionAccessor.BufferView.Buffer = gltfObject.buffers[positionAccessor.BufferView.buffer];
                vertexCount = positionAccessor.count;
            }

            if (meshPrimitive.Attributes.NORMAL >= 0)
            {
                normalsAccessor = gltfObject.accessors[meshPrimitive.Attributes.NORMAL];
                normalsAccessor.BufferView = gltfObject.bufferViews[normalsAccessor.bufferView];
                normalsAccessor.BufferView.Buffer = gltfObject.buffers[normalsAccessor.BufferView.buffer];
            }

            if (meshPrimitive.Attributes.TEXCOORD_0 >= 0)
            {
                textCoord0Accessor = gltfObject.accessors[meshPrimitive.Attributes.TEXCOORD_0];
                textCoord0Accessor.BufferView = gltfObject.bufferViews[textCoord0Accessor.bufferView];
                textCoord0Accessor.BufferView.Buffer = gltfObject.buffers[textCoord0Accessor.BufferView.buffer];
            }

            if (meshPrimitive.Attributes.TEXCOORD_1 >= 0)
            {
                textCoord1Accessor = gltfObject.accessors[meshPrimitive.Attributes.TEXCOORD_1];
                textCoord1Accessor.BufferView = gltfObject.bufferViews[textCoord1Accessor.bufferView];
                textCoord1Accessor.BufferView.Buffer = gltfObject.buffers[textCoord1Accessor.BufferView.buffer];
            }

            if (meshPrimitive.Attributes.TEXCOORD_2 >= 0)
            {
                textCoord2Accessor = gltfObject.accessors[meshPrimitive.Attributes.TEXCOORD_2];
                textCoord2Accessor.BufferView = gltfObject.bufferViews[textCoord2Accessor.bufferView];
                textCoord2Accessor.BufferView.Buffer = gltfObject.buffers[textCoord2Accessor.BufferView.buffer];
            }

            if (meshPrimitive.Attributes.TEXCOORD_3 >= 0)
            {
                textCoord3Accessor = gltfObject.accessors[meshPrimitive.Attributes.TEXCOORD_3];
                textCoord3Accessor.BufferView = gltfObject.bufferViews[textCoord3Accessor.bufferView];
                textCoord3Accessor.BufferView.Buffer = gltfObject.buffers[textCoord3Accessor.BufferView.buffer];
            }

            if (meshPrimitive.Attributes.COLOR_0 >= 0)
            {
                colorAccessor = gltfObject.accessors[meshPrimitive.Attributes.COLOR_0];
                colorAccessor.BufferView = gltfObject.bufferViews[colorAccessor.bufferView];
                colorAccessor.BufferView.Buffer = gltfObject.buffers[colorAccessor.BufferView.buffer];
            }

            if (meshPrimitive.indices >= 0)
            {
                indicesAccessor = gltfObject.accessors[meshPrimitive.indices];
                indicesAccessor.BufferView = gltfObject.bufferViews[indicesAccessor.bufferView];
                indicesAccessor.BufferView.Buffer = gltfObject.buffers[indicesAccessor.BufferView.buffer];
            }

            if (meshPrimitive.Attributes.TANGENT >= 0)
            {
                tangentAccessor = gltfObject.accessors[meshPrimitive.Attributes.TANGENT];
                tangentAccessor.BufferView = gltfObject.bufferViews[tangentAccessor.bufferView];
                tangentAccessor.BufferView.Buffer = gltfObject.buffers[tangentAccessor.BufferView.buffer];
            }

            if (meshPrimitive.Attributes.WEIGHTS_0 >= 0)
            {
                weight0Accessor = gltfObject.accessors[meshPrimitive.Attributes.WEIGHTS_0];
                weight0Accessor.BufferView = gltfObject.bufferViews[weight0Accessor.bufferView];
                weight0Accessor.BufferView.Buffer = gltfObject.buffers[weight0Accessor.BufferView.buffer];
            }

            if (meshPrimitive.Attributes.JOINTS_0 >= 0)
            {
                joint0Accessor = gltfObject.accessors[meshPrimitive.Attributes.JOINTS_0];
                joint0Accessor.BufferView = gltfObject.bufferViews[joint0Accessor.bufferView];
                joint0Accessor.BufferView.Buffer = gltfObject.buffers[joint0Accessor.BufferView.buffer];
            }

            if (meshPrimitive.Targets != null)
            {
                int targetLength = meshPrimitive.Targets.Length;

                targetPositionAccessor = new GltfAccessor[targetLength];
                targetNormalsAccessor = new GltfAccessor[targetLength];
                targetTangentAccessor = new GltfAccessor[targetLength];

                for (int i = 0; i < targetLength; i++)
                {
                    if (meshPrimitive.Targets[i].POSITION >= 0)
                    {
                        targetPositionAccessor[i] = gltfObject.accessors[meshPrimitive.Targets[i].POSITION];
                        targetPositionAccessor[i].BufferView = gltfObject.bufferViews[targetPositionAccessor[i].bufferView];
                        targetPositionAccessor[i].BufferView.Buffer = gltfObject.buffers[targetPositionAccessor[i].BufferView.buffer];
                    }

                    if (meshPrimitive.Targets[i].NORMAL >= 0)
                    {
                        targetNormalsAccessor[i] = gltfObject.accessors[meshPrimitive.Targets[i].NORMAL];
                        targetNormalsAccessor[i].BufferView = gltfObject.bufferViews[targetNormalsAccessor[i].bufferView];
                        targetNormalsAccessor[i].BufferView.Buffer = gltfObject.buffers[targetNormalsAccessor[i].BufferView.buffer];
                    }

                    if (meshPrimitive.Targets[i].TANGENT >= 0)
                    {
                        targetTangentAccessor[i] = gltfObject.accessors[meshPrimitive.Targets[i].TANGENT];
                        targetTangentAccessor[i].BufferView = gltfObject.bufferViews[targetTangentAccessor[i].bufferView];
                        targetTangentAccessor[i].BufferView.Buffer = gltfObject.buffers[targetTangentAccessor[i].BufferView.buffer];
                    }
                }
            }

            GltfAccessorData gltfAccessorData = new GltfAccessorData();

            gltfAccessorData.vertexCount = vertexCount;

            if (positionAccessor != null)
            {
                gltfAccessorData.vertices = positionAccessor.GetVector3Array();
            }

            if (normalsAccessor != null)
            {
                gltfAccessorData.normals = normalsAccessor.GetVector3Array();
            }

            if (textCoord0Accessor != null)
            {
                gltfAccessorData.uvCoords = textCoord0Accessor.GetVector2Array();
                if (gltfAccessorData.uvCoords.Length != gltfAccessorData.vertices.Length)
                    gltfAccessorData.uvCoords = UvCalculator.CalculateUVs(gltfAccessorData.vertices, 1f);
            }

            if (textCoord1Accessor != null)
            {
                gltfAccessorData.uv2Coords = textCoord1Accessor.GetVector2Array();
            }

            if (textCoord2Accessor != null)
            {
                gltfAccessorData.uv3Coords = textCoord2Accessor.GetVector2Array();
            }

            if (textCoord3Accessor != null)
            {
                gltfAccessorData.uv4Coords = textCoord3Accessor.GetVector2Array();
            }

            if (colorAccessor != null)
            {
                gltfAccessorData.colors = colorAccessor.GetColorArray();
            }

            if (indicesAccessor != null)
            {
                switch (meshPrimitive.mode)
                {
                    case GltfDrawMode.Points:
                    case GltfDrawMode.Lines:
                    case GltfDrawMode.LineLoop:
                    case GltfDrawMode.LineStrip:
                        gltfAccessorData.indices = indicesAccessor.GetIntArray(false);
                        break;

                    case GltfDrawMode.Triangles:
                        gltfAccessorData.indices = indicesAccessor.GetIntArray(true);
                        break;

                    case GltfDrawMode.TriangleStrip:
                        int[] intArray = indicesAccessor.GetIntArray(false);
                        gltfAccessorData.indices = StripToIndex(intArray, true);
                        break;
                }
            }

            if (tangentAccessor != null)
            {
                gltfAccessorData.tangents = tangentAccessor.GetVector4Array();
            }

            if (weight0Accessor != null && joint0Accessor != null)
            {
                gltfAccessorData.boneWeights = CreateBoneWeightArray(joint0Accessor.GetVector4Array(false),
                                                         weight0Accessor.GetVector4Array(false),
                                                         vertexCount);
            }

            if (meshPrimitive.Targets != null)
            {
                gltfAccessorData.targetPosition = new List<Vector3[]>();
                gltfAccessorData.targetNormals = new List<Vector3[]>();
                gltfAccessorData.targetTangent = new List<Vector3[]>();

                Vector3[] zeroes = new Vector3[vertexCount];

                for (int i = 0; i < meshPrimitive.Targets.Length; i++)
                {
                    gltfAccessorData.targetPosition.Add(meshPrimitive.Targets[i].POSITION >= 0 ? targetPositionAccessor[i].GetVector3Array() : zeroes);
                    gltfAccessorData.targetNormals.Add(meshPrimitive.Targets[i].NORMAL >= 0 ? targetNormalsAccessor[i].GetVector3Array() : zeroes);
                    gltfAccessorData.targetTangent.Add(meshPrimitive.Targets[i].TANGENT >= 0 ? targetTangentAccessor[i].GetVector3Array() : zeroes);
                }
            }

            meshPrimitive.AccessorData = gltfAccessorData;
        }

        // ---------------------------------------------------------------------------

        private static Mesh ConstructMeshPrimitiveAsync(GltfObject gltfObject, GltfMeshPrimitive meshPrimitive)
        {
            var mesh = new Mesh
            {
                indexFormat = meshPrimitive.AccessorData.vertexCount > UInt16.MaxValue ? IndexFormat.UInt32 : IndexFormat.UInt16,
            };

            if (meshPrimitive.AccessorData.vertices != null)
            {
                mesh.vertices = meshPrimitive.AccessorData.vertices;
            }

            if (meshPrimitive.AccessorData.normals != null)
            {
                mesh.normals = meshPrimitive.AccessorData.normals;
            }

            if (meshPrimitive.AccessorData.uvCoords != null)
            {
                mesh.uv = meshPrimitive.AccessorData.uvCoords;
            }

            if (meshPrimitive.AccessorData.uv2Coords != null)
            {
                mesh.uv2 = meshPrimitive.AccessorData.uv2Coords;
            }

            if (meshPrimitive.AccessorData.uv3Coords != null)
            {
                mesh.uv3 = meshPrimitive.AccessorData.uv3Coords;
            }

            if (meshPrimitive.AccessorData.uv4Coords != null)
            {
                mesh.uv4 = meshPrimitive.AccessorData.uv4Coords;
            }

            if (meshPrimitive.AccessorData.colors != null)
            {
                mesh.colors = meshPrimitive.AccessorData.colors;
            }

            if (meshPrimitive.AccessorData.indices != null)
            {
                switch (meshPrimitive.mode)
                {
                    case GltfDrawMode.Points:
                        mesh.SetIndices(meshPrimitive.AccessorData.indices, MeshTopology.Points, 0);
                        break;
                    case GltfDrawMode.Lines:
                    case GltfDrawMode.LineLoop:
                        mesh.SetIndices(meshPrimitive.AccessorData.indices, MeshTopology.Lines, 0);
                        break;
                    case GltfDrawMode.LineStrip:
                        mesh.SetIndices(meshPrimitive.AccessorData.indices, MeshTopology.LineStrip, 0);
                        break;
                    case GltfDrawMode.Triangles:
                    case GltfDrawMode.TriangleFan:
                    case GltfDrawMode.TriangleStrip:
                        mesh.SetIndices(meshPrimitive.AccessorData.indices, MeshTopology.Triangles, 0);
                        break;
                }
            }

            if (meshPrimitive.AccessorData.tangents != null)
            {
                mesh.tangents = meshPrimitive.AccessorData.tangents;
            }

            if (meshPrimitive.AccessorData.boneWeights != null)
            {
                mesh.boneWeights = meshPrimitive.AccessorData.boneWeights;
            }

            if (meshPrimitive.Targets != null)
            {
                Vector3[] zeroes = new Vector3[mesh.vertexCount];

                for (int i = 0; i < meshPrimitive.Targets.Length; i++)
                {
                    mesh.AddBlendShapeFrame("BlendShape" + i, 0, zeroes, zeroes, zeroes);
                    mesh.AddBlendShapeFrame("BlendShape" + i, 100,
                                                                   meshPrimitive.AccessorData.targetPosition[i],
                                                                   meshPrimitive.AccessorData.targetNormals[i],
                                                                   meshPrimitive.AccessorData.targetTangent[i]);
                }
            }
            mesh.RecalculateBounds();
            meshPrimitive.SubMesh = mesh;
            return mesh;
        }

        // ---------------------------------------------------------------------------

        private static BoneWeight[] CreateBoneWeightArray(Vector4[] joints, Vector4[] weights, int vertexCount)
        {
            NormalizeBoneWeightArray(weights);

            var boneWeights = new BoneWeight[vertexCount];

            for (int i = 0; i < vertexCount; i++)
            {
                boneWeights[i].boneIndex0 = (int)joints[i].x;
                boneWeights[i].boneIndex1 = (int)joints[i].y;
                boneWeights[i].boneIndex2 = (int)joints[i].z;
                boneWeights[i].boneIndex3 = (int)joints[i].w;

                boneWeights[i].weight0 = weights[i].x;
                boneWeights[i].weight1 = weights[i].y;
                boneWeights[i].weight2 = weights[i].z;
                boneWeights[i].weight3 = weights[i].w;
            }

            return boneWeights;
        }

        // ---------------------------------------------------------------------------

        private static void NormalizeBoneWeightArray(Vector4[] weights)
        {
            for (int i = 0; i < weights.Length; i++)
            {
                var weightSum = weights[i].x + weights[i].y + weights[i].z + weights[i].w;

                if (!Mathf.Approximately(weightSum, 0))
                {
                    weights[i] /= weightSum;
                }
            }
        }
    }
}
