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
using UnityEngine;

namespace BIMservercenter.Toolkit.Public.Model
{
    public partial class BSGltfExtras
    {
        // ---------------------------------------------------------------------------
        // Properties
        // ---------------------------------------------------------------------------
        /// <summary>
        /// Call this property to get the GameObject associated to Extras.
        /// </summary>
        public GameObject GameObject { get { return gameObject; } set { gameObject = value; } }

        /// <summary>
        /// Call this property to get the ID associated to Extras.
        /// </summary>
        public int ID { get { return id; } set { id = value; } }

        /// <summary>
        /// Call this property to get the GUID associated to Extras.
        /// </summary>
        public string GUID { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Call this property to get the Name associated to Extras.
        /// </summary>
        public string Name { get { return name; } set { name = value; } }

        /// <summary>
        /// Call this property to get the TextLines associated to Extras.
        /// </summary>
        public List<TextLine> TextLines { get { return textLines; } set { textLines = value; } }

        /// <summary>
        /// Call this property to get the KeyValues associated to Extras.
        /// </summary>
        public List<KeyValue> KeyValues { get { return keyValues; } set { keyValues = value; } }

        // ---------------------------------------------------------------------------
        // Descriptions
        // ---------------------------------------------------------------------------
        /// <summary>
        /// Call this method to get the glTF description of extras.
        /// </summary>
        public string GetGltfExtrasDescription()
        { return PGetGltfExtrasDescription(); }
    }
}