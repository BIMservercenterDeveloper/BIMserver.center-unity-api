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
    /// A buffer points to binary geometry, animation, or skins.
    /// https://github.com/KhronosGroup/glTF/blob/master/specification/2.0/schema/buffer.schema.json
    /// </summary>
    [Serializable]
    public class GltfBuffer : GltfChildOfRootProperty
    {
        /// <summary>
        /// The uri of the buffer.
        /// Relative paths are relative to the .gltf file.
        /// Instead of referencing an external file, the uri can also be a data-uri.
        /// </summary>
        public string uri;

        /// <summary>
        /// The length of the buffer in bytes.
        /// <minimum>0</minimum>
        /// </summary>
        public int byteLength = 0;

        public byte[] BufferData { get; internal set; }
    }
}