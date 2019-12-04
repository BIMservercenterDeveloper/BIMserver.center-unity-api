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
    /// Joints and matrices defining a skin.
    /// https://github.com/KhronosGroup/glTF/blob/master/specification/2.0/schema/skin.schema.json
    /// </summary>
    [Serializable]
    public class GltfSkin : GltfChildOfRootProperty
    {
        /// <summary>
        /// The index of the accessor containing the floating-point 4x4 inverse-bind matrices.
        /// The default is that each matrix is a 4x4 Identity matrix, which implies that inverse-bind
        /// matrices were pre-applied.
        /// </summary>
        public int inverseBindMatrices = -1;

        /// <summary>
        /// The index of the node used as a skeleton root.
        /// When undefined, joints transforms resolve to scene root.
        /// </summary>
        public int skeleton = -1;

        /// <summary>
        /// Indices of skeleton nodes, used as joints in this skin.  The array length must be the
        /// same as the `count` property of the `inverseBindMatrices` accessor (when defined).
        /// </summary>
        public int[] joints;
    }
}