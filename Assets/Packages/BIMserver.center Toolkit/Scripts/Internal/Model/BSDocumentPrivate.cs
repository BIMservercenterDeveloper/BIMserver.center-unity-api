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
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BIMservercenter.Toolkit.Public.Model
{
    [System.Serializable]
    public partial class BSDocument
    {
        public bool IsIFCFile { get { return nameDocument.EndsWith(".ifc"); } }
        public string bimServerId;
        public string imgUrl;
        public string nameDocument;
        public string dateLastChange;
        public string urlDownloadDocument;
        public List<BSAssociatedDocument> associatedDocuments;

        // ---------------------------------------------------------------------------
        // Paths
        // ---------------------------------------------------------------------------

        private string DocumentPath(string projectPath)
        {
            return Path.Combine(projectPath, bimServerId);
        }

        // ---------------------------------------------------------------------------

        private string DocumentFilePath(string projectPath)
        {
            return Path.Combine(projectPath, nameDocument);
        }
        
        // ---------------------------------------------------------------------------
        // Equals
        // ---------------------------------------------------------------------------

        private bool PEqualsBIMServerId(BSDocument bSDocument)
        {
            return bimServerId.Equals(bSDocument.bimServerId);
        }

        // ---------------------------------------------------------------------------
        // Gltf
        // ---------------------------------------------------------------------------

        private async Task<List<BSGltf>> PLoadGltfsAsync(string projectPath, bool hide)
        {
            List<BSGltf> gltfList;

            gltfList = new List<BSGltf>();

            {
                string documentPath;

                documentPath = DocumentPath(projectPath);

                for (int i = 0; i < associatedDocuments.Count; i++)
                {
                    BSAssociatedDocument bSAssociatedDocument;

                    bSAssociatedDocument = associatedDocuments[i];

                    if (bSAssociatedDocument.IsGltfFile == true)
                    {
                        if (bSAssociatedDocument.ExistOnDisk(documentPath) == true)
                        {
                            BSGltf gltf;

                            gltf = await bSAssociatedDocument.LoadGltfAsync(documentPath, hide);
                            gltfList.Add(gltf);
                        }
                    }
                }
            }

            return gltfList;
        }

        // ---------------------------------------------------------------------------
        // Disk
        // ---------------------------------------------------------------------------

        private bool HasAssociatedDocuments()
        {
            return IsIFCFile == true && associatedDocuments != null && associatedDocuments.Count > 0;
        }

        // ---------------------------------------------------------------------------

        private bool PExistOnDisk(string projectPath)
        {
            string documentFilePath;

            documentFilePath = DocumentFilePath(projectPath);

            if (File.Exists(documentFilePath) == false)
                return false;

            if (HasAssociatedDocuments() == true)
            {
                string documentPath;

                documentPath = DocumentPath(projectPath);

                if (Directory.Exists(documentPath) == true)
                {
                    for (int i = 0; i < associatedDocuments.Count; i++)
                    {
                        if (associatedDocuments[i].ExistOnDisk(documentPath) == false)
                            return false;
                    }

                    return true;
                }
            }
            else
            {
                return true;
            }

            return false;
        }

        // ---------------------------------------------------------------------------

        private async Task<bool> PSaveOnDiskAsync(string projectPath)
        {
            string documentPath;
            string documentFilePath;

            documentPath = DocumentPath(projectPath);
            documentFilePath = DocumentFilePath(projectPath);

            if (File.Exists(documentFilePath) == false)
            {
                if (Directory.Exists(projectPath) == false)
                    Directory.CreateDirectory(projectPath);

                if (await BIMServerCenterUtilities.DownloadAsync(urlDownloadDocument, documentFilePath) == false)
                    return false;
            }

            if (HasAssociatedDocuments() == true)
            {
                for (int i = 0; i < associatedDocuments.Count; i++)
                {
                    BSAssociatedDocument bSAssociatedDocument;

                    bSAssociatedDocument = associatedDocuments[i];

                    if (await bSAssociatedDocument.SaveOnDiskAsync(documentPath) == false)
                        return false;
                }
            }

            return true;
        }
    }
}