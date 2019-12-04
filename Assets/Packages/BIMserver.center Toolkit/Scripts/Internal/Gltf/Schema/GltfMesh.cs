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

using System;
using UnityEngine;

namespace BIMservercenter.Toolkit.Internal.Gltf.Schema
{
    /// <summary>
    /// A set of primitives to be rendered. A node can contain one or more meshes.
    /// A node's transform places the mesh in the scene.
    /// https://github.com/KhronosGroup/glTF/blob/master/specification/2.0/schema/mesh.schema.json
    /// </summary>
    [Serializable]
    public class GltfMesh : GltfChildOfRootProperty
    {
        /// <summary>
        /// An array of primitives, each defining geometry to be rendered with
        /// a material.
        /// <minItems>1</minItems>
        /// </summary>
        public GltfMeshPrimitive[] primitives;

        /// <summary>
        /// Array of weights to be applied to the Morph Targets.
        /// <minItems>0</minItems>
        /// </summary>
        public double[] weights;

        /// <summary>
        /// Unity Mesh wrapper for the GltfMesh
        /// </summary>
        public Mesh Mesh { get; internal set; }
    }
}