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
using System;

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
        /// <param name="defaultTexture">Input defaultTexture to return if the image cannot been downloaded.</param>
        [Obsolete]
        public async Task<Texture2D> LoadTextureImageAsync(Texture2D defaultTexture)
        { return await PLoadTextureImageAsync(null, defaultTexture); }

        /// <summary>
        /// Call this method to load the project image into a Texture2D object.
        /// </summary>
        /// <param name="rootPath">Input rootPath directory to find the project image file downloaded.</param>
        [Obsolete]
        public async Task<Texture2D> LoadTextureImageAsync(string rootPath)
        { return await PLoadTextureImageAsync(rootPath, null); }

        /// <summary>
        /// Call this method to load the project image into a Texture2D object.
        /// </summary>
        /// <param name="rootPath">Input rootPath directory to find the project image file downloaded.</param>
        /// <param name="defaultTexture">Input defaultTexture to return if the image cannot been downloaded.</param>
        [Obsolete]
        public async Task<Texture2D> LoadTextureImageAsync(string rootPath, Texture2D defaultTexture)
        { return await PLoadTextureImageAsync(rootPath, defaultTexture); }

        /// <summary>
        /// Call this method to load the small project image into a Texture2D object.
        /// </summary>
        /// <param name="rootPath">Input rootPath directory to find the project image file downloaded.</param>
        public async Task<Texture2D> LoadTextureImageSmallAsync(string rootPath)
        { return await PLoadTextureImageSmallAsync(rootPath, null); }

        /// <summary>
        /// Call this method to load the small project image into a Texture2D object.
        /// </summary>
        /// <param name="defaultTexture">Input defaultTexture to return if the image cannot been downloaded.</param>
        public async Task<Texture2D> LoadTextureImageSmallAsync(Texture2D defaultTexture)
        { return await PLoadTextureImageSmallAsync(null, defaultTexture); }

        /// <summary>
        /// Call this method to load the small project image into a Texture2D object.
        /// </summary>
        /// <param name="rootPath">Input rootPath directory to find the project image file downloaded.</param>
        /// <param name="defaultTexture">Input defaultTexture to return if the image cannot been downloaded.</param>
        public async Task<Texture2D> LoadTextureImageSmallAsync(string rootPath, Texture2D defaultTexture)
        { return await PLoadTextureImageSmallAsync(rootPath, defaultTexture); }

        /// <summary>
        /// Call this method to load the large project image into a Texture2D object.
        /// </summary>
        /// <param name="rootPath">Input rootPath directory to find the project image file downloaded.</param>
        public async Task<Texture2D> LoadTextureImageLargeAsync(string rootPath)
        { return await PLoadTextureImageLargeAsync(rootPath, null); }

        /// <summary>
        /// Call this method to load the large project image into a Texture2D object.
        /// </summary>
        /// <param name="defaultTexture">Input defaultTexture to return if the image cannot been downloaded.</param>
        public async Task<Texture2D> LoadTextureImageLargeAsync(Texture2D defaultTexture)
        { return await PLoadTextureImageLargeAsync(null, defaultTexture); }

        /// <summary>
        /// Call this method to load the large project image into a Texture2D object.
        /// </summary>
        /// <param name="rootPath">Input rootPath directory to find the project image file downloaded.</param>
        /// <param name="defaultTexture">Input defaultTexture to return if the image cannot been downloaded.</param>
        public async Task<Texture2D> LoadTextureImageLargeAsync(string rootPath, Texture2D defaultTexture)
        { return await PLoadTextureImageLargeAsync(rootPath, defaultTexture); }

        /// <summary>
        /// Call this method to load landscape the project image into a Texture2D object.
        /// </summary>
        /// <param name="rootPath">Input rootPath directory to find the project image file downloaded.</param>
        public async Task<Texture2D> LoadTextureImageLanscapeAsync(string rootPath)
        { return await PLoadTextureImageLandscapeAsync(rootPath, null); }

        /// <summary>
        /// Call this method to load landscape the project image into a Texture2D object.
        /// </summary>
        /// <param name="defaultTexture">Input defaultTexture to return if the image cannot been downloaded.</param>
        public async Task<Texture2D> LoadTextureImageLanscapeAsync(Texture2D defaultTexture)
        { return await PLoadTextureImageLandscapeAsync(null, defaultTexture); }

        /// <summary>
        /// Call this method to load landscape the project image into a Texture2D object.
        /// </summary>
        /// <param name="rootPath">Input rootPath directory to find the project image file downloaded.</param>
        /// <param name="defaultTexture">Input defaultTexture to return if the image cannot been downloaded.</param>
        public async Task<Texture2D> LoadTextureImageLanscapeAsync(string rootPath, Texture2D defaultTexture)
        { return await PLoadTextureImageLandscapeAsync(rootPath, defaultTexture); }

        // ---------------------------------------------------------------------------
        // Gltf
        // ---------------------------------------------------------------------------
        /// <summary>
        /// Call this method to load the project gLTF files to the openned unity scene.
        /// </summary>
        /// <param name="rootPath">Input rootPath directory to save the project files downloaded.</param>
        /// <param name="generateCollider">Input boolean to generate colliders during construction.</param>
        /// <param name="hide">Input boolean to show or hide the glTFGameObject after load it.</param>
        /// <exception cref="BSExceptionFailedToReadGLTF">Thrown when a GLTF file can't be parsed</exception>
        public async Task<List<BSGltf>> LoadGltfsAsync(string rootPath, bool generateColliders, bool hide)
        { return await PLoadGltfsAsync(rootPath, generateColliders, hide, null, null); }

        /// <summary>
        /// Call this method to load the project gLTF files to the openned unity scene.
        /// </summary>
        /// <param name="rootPath">Input rootPath directory to save the project files downloaded.</param>
        /// <param name="generateCollider">Input boolean to generate colliders during construction.</param>
        /// <param name="hide">Input boolean to show or hide the glTFGameObject after load it.</param>
        /// <param name="funcProgressPercIndexesUpdate">Callback to notify the project loading progress.</param>
        /// <exception cref="BSExceptionFailedToReadGLTF">Thrown when a GLTF file can't be parsed</exception>
        public async Task<List<BSGltf>> LoadGltfsAsync(string rootPath, bool generateColliders, bool hide, FuncProgressPercIndexesUpdate funcProgressPercIndexesUpdate)
        { return await PLoadGltfsAsync(rootPath, generateColliders, hide, funcProgressPercIndexesUpdate, null); }

        /// <summary>
        /// Call this method to load the project gLTF files to the openned unity scene.
        /// </summary>
        /// <param name="rootPath">Input rootPath directory to save the project files downloaded.</param>
        /// <param name="generateCollider">Input boolean to generate colliders during construction.</param>
        /// <param name="hide">Input boolean to show or hide the glTFGameObject after load it.</param>
        /// <param name="funcProgressPercIndexesUpdate">Callback to notify the project loading progress.</param>
        /// <param name="cancellationTokenSource">Signals to a CancellationToken that it should be canceled.</param>
        /// <exception cref="BSExceptionFailedToReadGLTF">Thrown when a GLTF file can't be parsed</exception>
        /// <exception cref="BSExceptionCancellationRequested">Thrown when cancellation is requested</exception>
        public async Task<List<BSGltf>> LoadGltfsAsync(string rootPath, bool generateColliders, bool hide, FuncProgressPercIndexesUpdate funcProgressPercIndexesUpdate, CancellationTokenSource cancellationTokenSource)
        { return await PLoadGltfsAsync(rootPath, generateColliders, hide, funcProgressPercIndexesUpdate, cancellationTokenSource); }

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
        /// <exception cref="BSExceptionServerDownloadError">Thrown when a connection can't be established to the server</exception>
        /// <exception cref="BSExceptionNetworkDownloadError">Thrown when a local connection can't be established</exception>
        public async Task SaveOnDiskAsync(string rootPath)
        { await PSaveOnDiskAsync(rootPath, null, null); }

        /// <summary>
        /// Call this method to save the project on disk.
        /// </summary>
        /// <param name="rootPath">Input rootPath directory to save the project.</param>
        /// <param name="funcProgressPercIndexesUpdate">Callback to notify the project loading progress.</param>
        /// <exception cref="BSExceptionServerDownloadError">Thrown when a connection can't be established to the server</exception>
        /// <exception cref="BSExceptionNetworkDownloadError">Thrown when a local connection can't be established</exception>
        public async Task SaveOnDiskAsync(string rootPath, FuncProgressPercIndexesUpdate funcProgressPercIndexesUpdate)
        { await PSaveOnDiskAsync(rootPath, funcProgressPercIndexesUpdate, null); }

        /// <summary>
        /// Call this method to save the project on disk.
        /// </summary>
        /// <param name="rootPath">Input rootPath directory to save the project.</param>
        /// <param name="funcProgressPercIndexesUpdate">Callback to notify the project loading progress.</param>
        /// <param name="cancellationTokenSource">Signals to a CancellationToken that it should be canceled.</param>
        /// <exception cref="BSExceptionServerDownloadError">Thrown when a connection can't be established to the server</exception>
        /// <exception cref="BSExceptionNetworkDownloadError">Thrown when a local connection can't be established</exception>
        /// <exception cref="BSExceptionCancellationRequested">Thrown when cancellation is requested</exception>
        public async Task SaveOnDiskAsync(string rootPath, FuncProgressPercIndexesUpdate funcProgressPercIndexesUpdate, CancellationTokenSource cancellationTokenSource)
        { await PSaveOnDiskAsync(rootPath, funcProgressPercIndexesUpdate, cancellationTokenSource); }

        /// <summary>
        /// Call this method to remove the project from disk.
        /// </summary>
        /// <param name="rootPath">Input rootPath directory where is the project.</param>
        [System.Obsolete]
        public bool RemoveFromDisk(string rootPath)
        { return PRemoveFromDisk(rootPath); }

        /// <summary>
        /// Call this method to load the project from disk.
        /// </summary>
        /// <param name="projectPath">Input projectPath directory where is the project.</param>
        [System.Obsolete]
        public bool LoadFromDisk(string projectPath)
        { return PLoadFromDisk(projectPath); }

        /// <summary>
        /// Call this method to move the project from originPath to destinationPath disk.
        /// </summary>
        /// <param name="srcRootPath">Input srcRootPath directory where is the project.</param>
        /// <param name="destRootPath">Input destRootPath directory where is the new path project.</param>
        [System.Obsolete]
        public bool MoveAtDisk(string srcRootPath, string destRootPath)
        { return PMoveAtDisk(srcRootPath, destRootPath); }
    }
}
