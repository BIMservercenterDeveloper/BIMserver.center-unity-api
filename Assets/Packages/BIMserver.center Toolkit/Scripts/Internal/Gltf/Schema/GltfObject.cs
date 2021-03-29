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

using BIMservercenter.Toolkit.Internal.Gltf.Schema.Extensions;
using BIMservercenter.Toolkit.Public.Model;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BIMservercenter.Toolkit.Internal.Gltf.Schema
{
    [Serializable]
    public class GltfObject : GltfProperty
    {
        #region Serialized Fields

        /// <summary>
        /// Names of glTF extensions used somewhere in this asset.
        /// </summary>
        public string[] extensionsUsed;

        /// <summary>
        /// Names of glTF extensions required to properly load this asset.
        /// </summary>
        public string[] extensionsRequired;

        /// <summary>
        /// An array of accessors. An accessor is a typed view into a bufferView.
        /// </summary>
        public GltfAccessor[] accessors;

        /// <summary>
        /// An array of keyframe animations.
        /// </summary>
        public GltfAnimation[] animations;

        /// <summary>
        /// Metadata about the glTF asset.
        /// </summary>
        public GltfAssetInfo asset;

        /// <summary>
        /// An array of buffers. A buffer points to binary geometry, animation, or skins.
        /// </summary>
        public GltfBuffer[] buffers;

        /// <summary>
        /// An array of bufferViews.
        /// A bufferView is a view into a buffer generally representing a subset of the buffer.
        /// </summary>
        public GltfBufferView[] bufferViews;

        /// <summary>
        /// An array of cameras. A camera defines a projection matrix.
        /// </summary>
        public GltfCamera[] cameras;

        /// <summary>
        /// An array of images. An image defines data used to create a texture.
        /// </summary>
        public GltfImage[] images;

        /// <summary>
        /// An array of materials. A material defines the appearance of a primitive.
        /// </summary>
        public GltfMaterial[] materials;

        /// <summary>
        /// An array of meshes. A mesh is a set of primitives to be rendered.
        /// </summary>
        public GltfMesh[] meshes;

        /// <summary>
        /// An array of nodes.
        /// </summary>
        public GltfNode[] nodes;

        /// <summary>
        /// An array of samplers. A sampler contains properties for texture filtering and wrapping modes.
        /// </summary>
        public GltfSampler[] samplers;

        /// <summary>
        /// The index of the default scene.
        /// </summary>
        public int scene;

        /// <summary>
        /// An array of scenes.
        /// </summary>
        public GltfScene[] scenes;

        /// <summary>
        /// An array of skins. A skin is defined by joints and matrices.
        /// </summary>
        public GltfSkin[] skins;

        /// <summary>
        /// An array of textures.
        /// </summary>
        public GltfTexture[] textures;

        /// <summary>
        /// An array the extras.
        /// </summary>
        public GltfExtras extras;

        #endregion Serialized Fields

        /// <summary>
        /// The name of the gltf Object.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// The absolute path to the glTF Object on disk.
        /// </summary>
        public string Uri { get; internal set; }

        /// <summary>
        /// The <see href="https://docs.unity3d.com/ScriptReference/GameObject.html">GameObject</see> reference for the gltf Object.
        /// </summary>
        public GameObject GameObjectReference { get; internal set; }

        /// <summary>
        /// The list of registered glTF extensions found for this object.
        /// </summary>
        public List<GltfExtension> RegisteredExtensions { get; internal set; } = new List<GltfExtension>();

        /// <summary>
        /// Flag for setting object load behavior.
        /// Importers should run on the main thread; all other loading scenarios should likely use the background thread
        /// </summary>
        internal bool UseBackgroundThread { get; set; } = true;

        /// <summary>
        /// The list of registered extras found for this object.
        /// </summary>
        public List<BSGltfExtras> RegisteredExtras { get; internal set; } = new List<BSGltfExtras>();

        /// <summary>
        /// The list of registered animations found for this object.
        /// </summary>
        public List<Animation> RegisteredAnimations { get; internal set; } = new List<Animation>();

        /// <summary>
        /// The list of registered renderers found for this object.
        /// </summary>
        public List<Renderer> RegisteredRenderers { get; internal set; } = new List<Renderer>();

        /// <summary>
        /// The list of registered colliders found for this object.
        /// </summary>
        public List<Collider> RegisteredColliders { get; internal set; } = new List<Collider>();
    }
}
