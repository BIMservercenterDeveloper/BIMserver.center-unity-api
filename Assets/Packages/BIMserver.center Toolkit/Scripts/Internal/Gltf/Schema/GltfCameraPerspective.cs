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

namespace BIMservercenter.Toolkit.Internal.Gltf.Schema
{
    /// <summary>
    /// A perspective camera containing properties to create a perspective projection
    /// matrix.
    /// https://github.com/KhronosGroup/glTF/blob/master/specification/2.0/schema/camera.perspective.schema.json
    /// </summary>
    [Serializable]
    public class GltfCameraPerspective : GltfProperty
    {
        /// <summary>
        /// The floating-point aspect ratio of the field of view.
        /// When this is undefined, the aspect ratio of the canvas is used.
        /// <minimum>0.0</minimum>
        /// </summary>
        public double aspectRatio;

        /// <summary>
        /// The floating-point vertical field of view in radians.
        /// <minimum>0.0</minimum>
        /// </summary>
        public double yFov;

        /// <summary>
        /// The floating-point distance to the far clipping plane. When defined,
        /// `zfar` must be greater than `znear`.
        /// If `zfar` is undefined, runtime must use infinite projection matrix.
        /// <minimum>0.0</minimum>
        /// </summary>
        public double zFar;

        /// <summary>
        /// The floating-point distance to the near clipping plane.
        /// <minimum>0.0</minimum>
        /// </summary>
        public double zNear;
    }
}