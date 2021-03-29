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
using System.Threading.Tasks;
using System.Threading;

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
        /// <param name="generateCollider">Input boolean to generate colliders during construction.</param>
        /// <param name="hide">Input boolean the gLTF object when is loaded.</param>     
        /// <exception cref="BSExceptionFailedToReadGLTF">Thrown when a GLTF file can't be parsed</exception>
        public async Task<BSGltf> LoadGltfAsync(string documentPath, bool generateColliders, bool hide)
        { return await PLoadGltfAsync(documentPath, generateColliders, hide, null, null); }

        /// <summary>
        /// Call this method to load the associated document gLTF files to the openned unity scene.
        /// </summary>
        /// <param name="documentPath">Input documentPath directory where is the document files.</param>
        /// <param name="generateCollider">Input boolean to generate colliders during construction.</param>
        /// <param name="hide">Input boolean the gLTF object when is loaded.</param>     
        /// <param name="funcProgressPercUpdate">Callback to notify the project loading progress.</param>
        /// <exception cref="BSExceptionFailedToReadGLTF">Thrown when a GLTF file can't be parsed</exception>
        public async Task<BSGltf> LoadGltfAsync(string documentPath, bool generateColliders, bool hide, FuncProgressPercUpdate funcProgressPercUpdate)
        { return await PLoadGltfAsync(documentPath, generateColliders, hide, funcProgressPercUpdate, null); }

        /// <summary>
        /// Call this method to load the associated document gLTF files to the openned unity scene.
        /// </summary>
        /// <param name="documentPath">Input documentPath directory where is the document files.</param>
        /// <param name="generateCollider">Input boolean to generate colliders during construction.</param>
        /// <param name="hide">Input boolean the gLTF object when is loaded.</param>     
        /// <param name="funcProgressPercUpdate">Callback to notify the project loading progress.</param>
        /// <param name="cancellationTokenSource">Signals to a CancellationToken that it should be canceled.</param>
        /// <exception cref="BSExceptionFailedToReadGLTF">Thrown when a GLTF file can't be parsed</exception>
        /// <exception cref="BSExceptionCancellationRequested">Thrown when cancellation is requested</exception>
        public async Task<BSGltf> LoadGltfAsync(string documentPath, bool generateColliders, bool hide, FuncProgressPercUpdate funcProgressPercUpdate, CancellationTokenSource cancellationTokenSource)
        { return await PLoadGltfAsync(documentPath, generateColliders, hide, funcProgressPercUpdate, cancellationTokenSource); }

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
        /// <exception cref="BSExceptionServerDownloadError">Thrown when a connection can't be established to the server</exception>
        /// <exception cref="BSExceptionNetworkDownloadError">Thrown when a local connection can't be established</exception>
        public async Task SaveOnDiskAsync(string documentPath)
        { await PSaveOnDiskAsync(documentPath, null, null); }

        /// <summary>
        /// Call this method to save the associated document on disk.
        /// </summary>
        /// <param name="documentPath">Input documentPath directory where is the document files.</param>
        /// <param name="funcProgressPercUpdate">Callback to notify the project loading progress.</param>
        /// <exception cref="BSExceptionServerDownloadError">Thrown when a connection can't be established to the server</exception>
        /// <exception cref="BSExceptionNetworkDownloadError">Thrown when a local connection can't be established</exception>
        public async Task SaveOnDiskAsync(string documentPath, FuncProgressPercUpdate funcProgressPercUpdate)
        { await PSaveOnDiskAsync(documentPath, funcProgressPercUpdate, null); }

        /// <summary>
        /// Call this method to save the associated document on disk.
        /// </summary>
        /// <param name="documentPath">Input documentPath directory where is the document files.</param>
        /// <param name="funcProgressPercUpdate">Callback to notify the project loading progress.</param>
        /// <param name="cancellationTokenSource">Signals to a CancellationToken that it should be canceled.</param>
        /// <exception cref="BSExceptionServerDownloadError">Thrown when a connection can't be established to the server</exception>
        /// <exception cref="BSExceptionNetworkDownloadError">Thrown when a local connection can't be established</exception>
        /// <exception cref="BSExceptionCancellationRequested">Thrown when cancellation is requested</exception>
        public async Task SaveOnDiskAsync(string documentPath, FuncProgressPercUpdate funcProgressPercUpdate, CancellationTokenSource cancellationTokenSource)
        { await PSaveOnDiskAsync(documentPath, funcProgressPercUpdate, cancellationTokenSource); }
    }
}
