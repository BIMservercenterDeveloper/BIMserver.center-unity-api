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
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace BIMservercenter.Toolkit.Public.Model
{
    public partial class BSProject
    {
        // ---------------------------------------------------------------------------
        // Equals
        // ---------------------------------------------------------------------------
        /// <summary>
        /// Call this method to know if two bsProjects are equals.
        /// </summary>
        /// <param name="bSProject">Input bSProject to compare.</param>
        public bool EqualsBIMServerId(BSProject bSProject)
        { return PEqualsBIMServerId(bSProject); }

        // ---------------------------------------------------------------------------
        // Image
        // ---------------------------------------------------------------------------
        /// <summary>
        /// Call this method to load the project image into a Texture2D object.
        /// </summary>
        /// <param name="rootPath">Input rootPath directory to find the project image file downloaded.</param>
        /// <param name="defaultTexture">Input defaultTexture to return if the image cannot been downloaded.</param>
        public async Task<Texture2D> LoadTextureImageAsync(string rootPath = null, Texture2D defaultTexture = null)
        { return await PLoadTextureImageAsync(rootPath, defaultTexture); }

        // ---------------------------------------------------------------------------
        // Gltf
        // ---------------------------------------------------------------------------
        /// <summary>
        /// Call this method to load the project gLTF files to the openned unity scene.
        /// </summary>
        /// <param name="rootPath">Input rootPath directory to save the project files downloaded.</param>
        /// <param name="hide">Input hide the gLTF object when is loaded.</param>
        /// <param name="progress">Input pogress unityaction to notify the project loading progress.</param>
        public async Task<List<BSGltf>> LoadGltfsAsync(string rootPath, bool hide, UnityAction<int, int, float> progress = null)
        { return await PLoadGltfsAsync(rootPath, hide, progress); }

        // ---------------------------------------------------------------------------
        // Disk
        // ---------------------------------------------------------------------------
        /// <summary>
        /// Call this method to check the existence of the project on disk.
        /// </summary>
        /// <param name=“rootPath”>Input rootPath directory to check the project files.</param>
        public bool ExistOnDisk(string rootPath)
        { return PExistOnDisk(rootPath); }

        /// <summary>
        /// Call this method to save the project on disk.
        /// </summary>
        /// <param name="rootPath">Input rootPath directory to save the project.</param>
        /// <param name="progress">Input pogress unityaction to notify the project saving progress.</param>
        public async Task<bool> SaveOnDiskAsync(string rootPath, UnityAction<int, int, float> progress = null)
        { return await PSaveOnDiskAsync(rootPath, progress); }

        /// <summary>
        /// Call this method to remove the project from disk.
        /// </summary>
        /// <param name="rootPath">Input rootPath directory where is the project.</param>
        public bool RemoveFromDisk(string rootPath)
        { return PRemoveFromDisk(rootPath); }

        /// <summary>
        /// Call this method to load the project from disk.
        /// </summary>
        /// <param name="projectPath">Input projectPath directory where is the project.</param>
        public bool LoadFromDisk(string projectPath)
        { return PLoadFromDisk(projectPath); }

        /// <summary>
        /// Call this method to move the project from originPath to destinationPath disk.
        /// </summary>
        /// <param name="srcRootPath">Input srcRootPath directory where is the project.</param>
        /// <param name="destRootPath">Input destRootPath directory where is the new path project.</param>
        public bool MoveAtDisk(string srcRootPath, string destRootPath)
        { return PMoveAtDisk(srcRootPath, destRootPath); }
    }
}
