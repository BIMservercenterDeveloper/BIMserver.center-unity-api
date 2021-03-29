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

using UnityEngine;
using System;

namespace BIMservercenter.Toolkit.Internal.Gltf.Schema
{
    /// <summary>
    /// Image data used to create a texture. Image can be referenced by URI or
    /// `bufferView` index. `mimeType` is required in the latter case.
    /// https://github.com/KhronosGroup/glTF/blob/master/specification/2.0/schema/image.schema.json
    /// </summary>
    [Serializable]
    public class GltfImage : GltfChildOfRootProperty
    {
        #region Serialized Fields

        /// <summary>
        /// The uri of the image.  Relative paths are relative to the .gltf file.
        /// Instead of referencing an external file, the uri can also be a data-uri.
        /// The image format must be jpg, png, bmp, or gif.
        /// </summary>
        public string uri;

        /// <summary>
        /// The image's MIME type.
        /// <minLength>1</minLength>
        /// </summary>
        public string mimeType;

        /// <summary>
        /// The index of the bufferView that contains the image.
        /// Use this instead of the image's uri property.
        /// </summary>
        public int bufferView;

        #endregion Serialized Fields

        /// <summary>
        /// Unity Texture2D wrapper for the GltfImage
        /// </summary>
        public Texture2D Texture { get; internal set; }
    }
}