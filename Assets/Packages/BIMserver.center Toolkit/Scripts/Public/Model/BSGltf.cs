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

using BIMservercenter.Toolkit.Public.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace BIMservercenter.Toolkit.Public.Model
{
    public partial class BSGltf
    {
        // ---------------------------------------------------------------------------
        // Properties
        // ---------------------------------------------------------------------------
        /// <summary>
        /// Call this property to get the glTF GameObject.
        /// </summary>
        public GameObject GltfGameObject { get { return glTFGameObject; } }

        /// <summary>
        /// Call this property to get the glTF Colliders.
        /// </summary>
        public List<Collider> GltfColliders { get { return glTFColliders; } }

        /// <summary>
        /// Call this property to get the glTF Renderers.
        /// </summary>
        public List<Renderer> GltfRenderers { get { return glTFRenderers; } }

        /// <summary>
        /// Call this property to get the glTF Animations.
        /// </summary>
        public List<Animation> GltfAnimations { get { return glTFAnimations; } }

        /// <summary>
        /// Call this property to get the glTF Extras.
        /// </summary>
        public List<BSGltfExtras> GltfExtras { get { return glTFExtras; } }

        // ---------------------------------------------------------------------------
        // Disk
        // ---------------------------------------------------------------------------
        /// <summary>
        /// Call this method to load a glTF file stored at disk into your actual openned scene.
        /// </summary>
        /// <param name="glTFPath">Input glTFPath where is the glTF file.</param>
        /// <param name="generateCollider">Input boolean to generate colliders during construction.</param>
        /// <param name="hide">Input boolean to show or hide the glTFGameObject after load it.</param>
        /// <exception cref="BSExceptionFailedToReadGLTF">Thrown when a GLTF file can't be parsed</exception>
        public async Task LoadGltfFromDiskAsync(string glTFPath, bool generateCollider, bool hide)
        { await PLoadGltfFromDiskAsync(glTFPath, generateCollider, hide, null, null); }

        /// <summary>
        /// Call this method to load a glTF file stored at disk into your actual openned scene.
        /// </summary>
        /// <param name="glTFPath">Input glTFPath where is the glTF file.</param>
        /// <param name="generateCollider">Input boolean to generate colliders during construction.</param>
        /// <param name="hide">Input boolean to show or hide the glTFGameObject after load it.</param>
        /// <param name="funcProgressPercUpdate">Callback to notify the project loading progress.</param>
        /// <exception cref="BSExceptionFailedToReadGLTF">Thrown when a GLTF file can't be parsed</exception>
        public async Task LoadGltfFromDiskAsync(string glTFPath, bool generateCollider, bool hide, FuncProgressPercUpdate funcProgressPercUpdate)
        { await PLoadGltfFromDiskAsync(glTFPath, generateCollider, hide, funcProgressPercUpdate, null); }

        /// <summary>
        /// Call this method to load a glTF file stored at disk into your actual openned scene.
        /// </summary>
        /// <param name="glTFPath">Input glTFPath where is the glTF file.</param>
        /// <param name="generateCollider">Input boolean to generate colliders during construction.</param>
        /// <param name="hide">Input boolean to show or hide the glTFGameObject after load it.</param>
        /// <param name="funcProgressPercUpdate">Callback to notify the project loading progress.</param>
        /// <param name="cancellationTokenSource">Signals to a CancellationToken that it should be canceled.</param>
        /// <exception cref="BSExceptionFailedToReadGLTF">Thrown when a GLTF file can't be parsed</exception>
        /// <exception cref="BSExceptionCancellationRequested">Thrown when cancellation is requested</exception>
        public async Task LoadGltfFromDiskAsync(string glTFPath, bool generateCollider, bool hide, FuncProgressPercUpdate funcProgressPercUpdate, CancellationTokenSource cancellationTokenSource)
        { await PLoadGltfFromDiskAsync(glTFPath, generateCollider, hide, funcProgressPercUpdate, cancellationTokenSource); }
    }
}
