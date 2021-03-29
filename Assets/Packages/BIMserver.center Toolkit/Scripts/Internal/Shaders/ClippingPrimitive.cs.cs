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

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BIMservercenter.Toolkit.Internal.Shaders
{
    [ExecuteAlways]
    public abstract class ClippingPrimitive : MonoBehaviour
    {
        [Tooltip("The renderer(s) that should be affected by the primitive.")]
        [SerializeField]
        protected List<Renderer> renderers = new List<Renderer>();

        public enum Side
        {
            Inside = 1,
            Outside = 1
        }

        [Tooltip("Which side of the primitive to clip pixels against.")]
        [SerializeField]
        protected Side clippingSide = Side.Inside;

        public Side ClippingSide
        {
            get { return clippingSide; }
            set { clippingSide = value; }
        }


        [SerializeField]
        [Tooltip("Toggles whether the primitive will use the Camera OnPreRender event")]
        private bool useOnPreRender;

        public bool UseOnPreRender
        {
            get { return useOnPreRender; }
            set
            {
                if (cameraMethods == null)
                {
                    cameraMethods = CameraCache.Main.gameObject.GetComponent<CameraEventRouter>();
                }

                if (value)
                {
                    cameraMethods.OnCameraPreRender += OnCameraPreRender;
                }
                else
                {
                    cameraMethods.OnCameraPreRender -= OnCameraPreRender;
                }

                useOnPreRender = value;
            }
        }


        protected abstract string Keyword { get; }
        protected abstract string ClippingSideProperty { get; }

        protected MaterialPropertyBlock materialPropertyBlock;
        protected Dictionary<Material, bool> modifiedMaterials = new Dictionary<Material, bool>();
        protected List<Material> allocatedMaterials = new List<Material>();

        private int clippingSideID;

        [SerializeField]
        [HideInInspector]
        private CameraEventRouter cameraMethods;

        public void AddRenderer(Renderer _renderer)
        {
            if (_renderer != null && !renderers.Contains(_renderer))
            {
                renderers.Add(_renderer);

                Material[] materials = GetMaterials(_renderer, false);

                foreach (var material in materials)
                {
                    if (material != null)
                    {
                        ToggleClippingFeature(material, true);
                    }
                }
            }
        }

        public void RemoveRenderer(Renderer _renderer)
        {
            if (renderers.Contains(_renderer))
            {
                Material[] materials = GetMaterials(_renderer, false);

                foreach (var material in materials)
                {
                    if (material != null)
                    {
                        ToggleClippingFeature(material, false);
                    }
                }

                renderers.Remove(_renderer);
            }
        }

        protected void OnValidate()
        {
            ToggleClippingFeature(true);
            RestoreUnassignedMaterials();
        }

        protected void OnEnable()
        {
            Initialize();
            UpdateRenderers();
            ToggleClippingFeature(true);

            if (useOnPreRender)
            {
                cameraMethods = CameraCache.Main.gameObject.GetComponent<CameraEventRouter>();
                cameraMethods.OnCameraPreRender += OnCameraPreRender;
            }
        }

        protected void OnDisable()
        {
            UpdateRenderers();
            ToggleClippingFeature(false);

            if (cameraMethods != null)
            {
                cameraMethods.OnCameraPreRender -= OnCameraPreRender;
            }
        }

        protected void Start()
        {
            if (useOnPreRender)
            {
                cameraMethods = CameraCache.Main.gameObject.GetComponent<CameraEventRouter>();
                cameraMethods.OnCameraPreRender += OnCameraPreRender;
            }
        }


#if UNITY_EDITOR
        protected void Update()
        {
            if (Application.isPlaying)
            {
                return;
            }

            Initialize();
            UpdateRenderers();
        }

#endif

        protected void LateUpdate()
        {
            if (!useOnPreRender)
            {
                UpdateRenderers();
            }
        }

        protected void OnCameraPreRender(CameraEventRouter router)
        {
            UpdateRenderers();
        }

        protected void OnDestroy()
        {
            if (renderers == null)
            {
                return;
            }

            for (int i = 0; i < renderers.Count; ++i)
            {
                Material[] materials = GetMaterials(renderers[i]);

                foreach (var material in materials)
                {
                    if (material != null)
                    {
                        bool clippingPlaneOn;

                        if (modifiedMaterials.TryGetValue(material, out clippingPlaneOn))
                        {
                            ToggleClippingFeature(material, clippingPlaneOn);
                            modifiedMaterials.Remove(material);
                        }
                    }
                }
            }

            RestoreUnassignedMaterials();

            for (int i = 0; i < allocatedMaterials.Count; ++i)
            {
                Destroy(allocatedMaterials[i]);
            }
        }

        protected virtual void Initialize()
        {
            materialPropertyBlock = new MaterialPropertyBlock();
            clippingSideID = Shader.PropertyToID(ClippingSideProperty);
        }

        protected virtual void UpdateRenderers()
        {
            if (renderers == null)
            {
                return;
            }

            for (int i = 0; i < renderers.Count; ++i)
            {
                Renderer _renderer = renderers[i];

                if (_renderer == null)
                {
                    continue;
                }

                _renderer.GetPropertyBlock(materialPropertyBlock);
                materialPropertyBlock.SetFloat(clippingSideID, (float)clippingSide);
                UpdateShaderProperties(materialPropertyBlock);
                _renderer.SetPropertyBlock(materialPropertyBlock);
            }
        }

        protected abstract void UpdateShaderProperties(MaterialPropertyBlock materialPropertyBlock);

        protected void ToggleClippingFeature(bool keywordOn)
        {
            if (renderers == null)
            {
                return;
            }

            for (int i = 0; i < renderers.Count; ++i)
            {
                Material[] materials = GetMaterials(renderers[i]);

                foreach (var material in materials)
                {
                    if (material != null)
                    {
                        if (!modifiedMaterials.ContainsKey(material))
                        {
                            modifiedMaterials[material] = material.IsKeywordEnabled(Keyword);
                        }

                        ToggleClippingFeature(material, keywordOn);
                    }
                }
            }
        }

        protected void ToggleClippingFeature(Material material, bool keywordOn)
        {
            if (keywordOn)
            {
                material.EnableKeyword(Keyword);
            }
            else
            {
                material.DisableKeyword(Keyword);
            }
        }

        protected Material[] GetMaterials(Renderer _renderer, bool trackAllocations = true)
        {
            if (_renderer == null)
            {
                return null;
            }

            if (Application.isEditor && !Application.isPlaying)
            {
                return _renderer.sharedMaterials;
            }
            else
            {
                Material[] materials = _renderer.materials;

                foreach (var material in materials)
                {
                    if (trackAllocations && !allocatedMaterials.Contains(material))
                    {
                        allocatedMaterials.Add(material);
                    }
                }

                return materials;
            }
        }

        protected void RestoreUnassignedMaterials()
        {
            List<Material> toRemove = new List<Material>();

            foreach (var modifiedMaterial in modifiedMaterials)
            {
                if (modifiedMaterial.Key == null)
                {
                    toRemove.Add(modifiedMaterial.Key);
                }
                else if (renderers.Find(x => (GetMaterials(x).Contains(modifiedMaterial.Key))) == null)
                {
                    ToggleClippingFeature(modifiedMaterial.Key, modifiedMaterial.Value);
                    toRemove.Add(modifiedMaterial.Key);
                }
            }

            foreach (var material in toRemove)
            {
                modifiedMaterials.Remove(material);
            }
        }
    }
}