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

namespace BIMservercenter.Toolkit.Public.Model
{
    public partial class BSDocument
    {
        // ---------------------------------------------------------------------------
        // Equals
        // ---------------------------------------------------------------------------
        /// <summary>
        /// Call this method to know if two bSDocuments are equals.
        /// </summary>
        /// <param name="bSDocument">Input bSDocument to compare.</param>
        public bool EqualsBIMServerId(BSDocument bSDocument)
        { return PEqualsBIMServerId(bSDocument); }

        // ---------------------------------------------------------------------------
        // Gltf
        // ---------------------------------------------------------------------------
        /// <summary>
        /// Call this method to load the document gLTF files to the openned unity scene.
        /// </summary>
        /// <param name="projectPath">Input projectPath directory where is the project files.</param>
        /// <param name="generateCollider">Input boolean to generate colliders during construction.</param>
        /// <param name="hide">Input boolean to show or hide the glTFGameObject after load it.</param>
        /// <exception cref="BSExceptionFailedToReadGLTF">Thrown when a GLTF file can't be parsed</exception>
        public async Task<List<BSGltf>> LoadGltfsAsync(string projectPath, bool generateColliders, bool hide)
        { return await PLoadGltfsAsync(projectPath, generateColliders, hide, null, null); }

        /// <summary>
        /// Call this method to load the document gLTF files to the openned unity scene.
        /// </summary>
        /// <param name="projectPath">Input projectPath directory where is the project files.</param>
        /// <param name="generateCollider">Input boolean to generate colliders during construction.</param>
        /// <param name="hide">Input boolean to show or hide the glTFGameObject after load it.</param>
        /// <param name="funcProgressPercIndexesUpdate">Callback to notify the project loading progress.</param>
        /// <exception cref="BSExceptionFailedToReadGLTF">Thrown when a GLTF file can't be parsed</exception>
        public async Task<List<BSGltf>> LoadGltfsAsync(string projectPath, bool generateColliders, bool hide, FuncProgressPercIndexesUpdate funcProgressPercIndexesUpdate)
        { return await PLoadGltfsAsync(projectPath, generateColliders, hide, funcProgressPercIndexesUpdate, null); }

        /// <summary>
        /// Call this method to load the document gLTF files to the openned unity scene.
        /// </summary>
        /// <param name="projectPath">Input projectPath directory where is the project files.</param>
        /// <param name="generateCollider">Input boolean to generate colliders during construction.</param>
        /// <param name="hide">Input boolean to show or hide the glTFGameObject after load it.</param>
        /// <param name="funcProgressPercIndexesUpdate">Callback to notify the project loading progress.</param>
        /// <param name="cancellationTokenSource">Signals to a CancellationToken that it should be canceled.</param>
        /// <exception cref="BSExceptionFailedToReadGLTF">Thrown when a GLTF file can't be parsed</exception>
        /// <exception cref="BSExceptionCancellationRequested">Thrown when cancellation is requested</exception>
        public async Task<List<BSGltf>> LoadGltfsAsync(string projectPath, bool generateColliders, bool hide, FuncProgressPercIndexesUpdate funcProgressPercIndexesUpdate, CancellationTokenSource cancellationTokenSource)
        { return await PLoadGltfsAsync(projectPath, generateColliders, hide, funcProgressPercIndexesUpdate, cancellationTokenSource); }

        // ---------------------------------------------------------------------------
        // Disk
        // ---------------------------------------------------------------------------
        /// <summary>
        /// Call this method to check the existence of the document on disk.
        /// </summary>
        /// <param name=“projectPath”>Input projectPath directory to check the document files.</param>
        public bool ExistOnDisk(string projectPath)
        { return PExistOnDisk(projectPath); }

        /// <summary>
        /// Call this method to save the document on disk.
        /// </summary>
        /// <param name="projectPath">Input projectPath directory where is the project files.</param>
        /// <exception cref="BSExceptionServerDownloadError">Thrown when a connection can't be established to the server</exception>
        /// <exception cref="BSExceptionNetworkDownloadError">Thrown when a local connection can't be established</exception>
        public async Task SaveOnDiskAsync(string projectPath)
        { await PSaveOnDiskAsync(projectPath, null, null); }

        /// <summary>
        /// Call this method to save the document on disk.
        /// </summary>
        /// <param name="projectPath">Input projectPath directory where is the project files.</param>
        /// <param name="funcProgressPercIndexesUpdate">Callback to notify the project loading progress.</param>
        /// <exception cref="BSExceptionServerDownloadError">Thrown when a connection can't be established to the server</exception>
        /// <exception cref="BSExceptionNetworkDownloadError">Thrown when a local connection can't be established</exception>
        public async Task SaveOnDiskAsync(string projectPath, FuncProgressPercIndexesUpdate funcProgressPercIndexesUpdate)
        { await PSaveOnDiskAsync(projectPath, funcProgressPercIndexesUpdate, null); }

        /// <summary>
        /// Call this method to save the document on disk.
        /// </summary>
        /// <param name="projectPath">Input projectPath directory where is the project files.</param>
        /// <param name="funcProgressPercIndexesUpdate">Callback to notify the project loading progress.</param>
        /// <param name="cancellationTokenSource">Signals to a CancellationToken that it should be canceled.</param>
        /// <exception cref="BSExceptionServerDownloadError">Thrown when a connection can't be established to the server</exception>
        /// <exception cref="BSExceptionNetworkDownloadError">Thrown when a local connection can't be established</exception>
        /// <exception cref="BSExceptionCancellationRequested">Thrown when cancellation is requested</exception>
        public async Task SaveOnDiskAsync(string projectPath, FuncProgressPercIndexesUpdate funcProgressPercIndexesUpdate, CancellationTokenSource cancellationTokenSource)
        { await PSaveOnDiskAsync(projectPath, funcProgressPercIndexesUpdate, cancellationTokenSource); }
    }
}
