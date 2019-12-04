﻿/*
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

using System.Threading.Tasks;

namespace BIMservercenter.Toolkit.Public.Model
{
    public partial class BSAssociatedDocument
    {
        // ---------------------------------------------------------------------------
        // Gltf
        // ---------------------------------------------------------------------------
        /// <summary>
        /// Call this method to load the associated document gLTF files to the openned unity scene.
        /// </summary>
        /// <param name="documentPath">Input documentPath directory where is the document files.</param>
        /// <param name="hide">Input hide the gLTF object when is loaded.</param>        
        public async Task<BSGltf> LoadGltfAsync(string documentPath, bool hide)
        { return await PLoadGltfAsync(documentPath, hide); }

        // ---------------------------------------------------------------------------
        // Disk
        // ---------------------------------------------------------------------------
        /// <summary>
        /// Call this method to check the existence of the associated document on disk.
        /// </summary>
        /// <param name=“documentPath”>Input documentPath directory to check the associated document files.</param>
        public bool ExistOnDisk(string documentPath)
        { return PExistOnDisk(documentPath); }

        /// <summary>
        /// Call this method to save the associated document on disk.
        /// </summary>
        /// <param name="documentPath">Input documentPath directory where is the document files.</param>
        public async Task<bool> SaveOnDiskAsync(string documentPath)
        { return await PSaveOnDiskAsync(documentPath); }
    }
}