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
    /// A view into a buffer generally representing a subset of the buffer.
    /// https://github.com/KhronosGroup/glTF/blob/master/specification/2.0/schema/bufferView.schema.json
    /// </summary>
    [Serializable]
    public class GltfBufferView : GltfChildOfRootProperty
    {
        /// <summary>
        /// The index of the buffer.
        /// </summary>
        public int buffer = -1;

        /// <summary>
        /// The offset into the buffer in bytes.
        /// <minimum>0</minimum>
        /// </summary>
        public int byteOffset = 0;

        /// <summary>
        /// The length of the bufferView in bytes.
        /// <minimum>0</minimum>
        /// </summary>
        public int byteLength = 0;

        /// <summary>
        /// The stride, in bytes, between vertex attributes or other interleavable data.
        /// When this is zero, data is tightly packed.
        /// <minimum>0</minimum>
        /// <maximum>255</maximum>
        /// </summary>
        public int byteStride = 0;

        /// <summary>
        /// The target that the WebGL buffer should be bound to.
        /// All valid values correspond to WebGL enums.
        /// When this is not provided, the bufferView contains animation or skin data.
        /// </summary>
        public GltfBufferViewTarget target = GltfBufferViewTarget.None;

        /// <summary>
        /// https://github.com/KhronosGroup/glTF/blob/master/specification/2.0/schema/buffer.schema.json
        /// </summary>
        public GltfBuffer Buffer { get; internal set; }
    }
}