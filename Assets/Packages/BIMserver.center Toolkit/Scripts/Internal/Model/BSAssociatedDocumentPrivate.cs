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

using BIMservercenter.Toolkit.Internal.Gltf.AsyncAwaitUtil;
using BIMservercenter.Toolkit.Internal.Utilities;
using BIMservercenter.Toolkit.Public.Utilities;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace BIMservercenter.Toolkit.Public.Model
{
    [System.Serializable]
    public partial class BSAssociatedDocument
    {
        public bool IsGltfFile;
        public string nameFile;
        public string urlDownload;
        public string dateLastChange;

        // ---------------------------------------------------------------------------
        // Paths
        // ---------------------------------------------------------------------------

        private string AssociatedDocumentPath(string documentPath)
        {
            return Path.Combine(documentPath, nameFile);
        }

        // ---------------------------------------------------------------------------
        // Gltf
        // ---------------------------------------------------------------------------

        private async Task<BSGltf> PLoadGltfAsync(
                        string documentPath, 
                        bool generateColliders, 
                        bool hide, 
                        FuncProgressPercUpdate funcProgressPercUpdate, 
                        CancellationTokenSource cancellationTokenSource)
        {
            BSGltf gltf;
            string associatedDocumentPath;

            BIMServerCenterAssertion.AssertEquals(IsGltfFile, true);
            gltf = new BSGltf();
            associatedDocumentPath = AssociatedDocumentPath(documentPath);

            BIMServerCenterAssertion.AssertEquals(File.Exists(associatedDocumentPath), true);
            await gltf.LoadGltfFromDiskAsync(associatedDocumentPath, generateColliders, hide, funcProgressPercUpdate, cancellationTokenSource);

            return gltf;
        }

        // ---------------------------------------------------------------------------
        // Disk
        // ---------------------------------------------------------------------------

        private bool PExistOnDisk(string documentPath)
        {
            string associatedDocumentPath;

            associatedDocumentPath = AssociatedDocumentPath(documentPath);
            return File.Exists(associatedDocumentPath);
        }

        // ---------------------------------------------------------------------------

        private async Task PSaveOnDiskAsync(
                        string documentPath,
                        FuncProgressPercUpdate funcProgressPercUpdate,
                        CancellationTokenSource cancellationTokenSource)
        {
            string associatedDocumentPath;

            if (Directory.Exists(documentPath) == false)
                Directory.CreateDirectory(documentPath);

            associatedDocumentPath = AssociatedDocumentPath(documentPath);

            await BIMServerCenterUtilities.DownloadAsync(urlDownload, associatedDocumentPath, funcProgressPercUpdate, cancellationTokenSource);
        }
    }
}
