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

using BIMservercenter.Toolkit.Internal.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace BIMservercenter.Toolkit.Public.Model
{
    [System.Serializable]
    public partial class BSProject
    {
        private const string ProjectClassFile = "00.binon";
        private const string ProjectImageFile = "01.binong";

        public string bimServerId;
        public string nameProject;
        public string dateLastChange;
        public string imgUrl;
        public List<BSDocument> documents;

        // ---------------------------------------------------------------------------
        // Paths
        // ---------------------------------------------------------------------------

        private string ProjectPath(string rootPath)
        {
            return Path.Combine(rootPath, bimServerId);
        }

        // ---------------------------------------------------------------------------

        private string ProjectImagePath(string projectPath)
        {
            return Path.Combine(projectPath, ProjectImageFile);
        }

        // ---------------------------------------------------------------------------

        private string ProjectJsonPath(string projectPath)
        {
            return Path.Combine(projectPath, ProjectClassFile);
        }

        // ---------------------------------------------------------------------------
        // Equals
        // ---------------------------------------------------------------------------

        private bool PEqualsBIMServerId(BSProject bSProject)
        {
            return bimServerId.Equals(bSProject.bimServerId);
        }

        // ---------------------------------------------------------------------------
        // Image
        // ---------------------------------------------------------------------------

        private async Task<Texture2D> PLoadTextureImageAsync(string rootPath = null, Texture2D defaultTexture = null)
        {
            if (rootPath != null)
            {
                string projectPath, projectImagePath;

                projectPath = ProjectPath(rootPath);
                projectImagePath = ProjectImagePath(projectPath);

                if (File.Exists(projectImagePath) == true)
                    return BIMServerCenterUtilities.LoadTexture(projectImagePath);
            }

            return await BIMServerCenterUtilities.DownloadTextureAsync(imgUrl, defaultTexture);
        }

        // ---------------------------------------------------------------------------
        // Gltf
        // ---------------------------------------------------------------------------

        private async Task<List<BSGltf>> PLoadGltfsAsync(string rootPath, bool hide, UnityAction<int, int, float> progress = null)
        {
            List<BSGltf> gltfList;

            gltfList = new List<BSGltf>();

            {
                string projectPath;

                projectPath = ProjectPath(rootPath);

                for (int i = 0; i < documents.Count; i++)
                {
                    BSDocument bSDocument;
                    List<BSGltf> documentGltfObjectList;

                    bSDocument = documents[i];

                    documentGltfObjectList = await bSDocument.LoadGltfsAsync(projectPath, hide);
                    progress?.Invoke(i, documents.Count, (i / (float)documents.Count));

                    gltfList.AddRange(documentGltfObjectList);
                }
            }

            return gltfList;
        }

        // ---------------------------------------------------------------------------
        // Disk
        // ---------------------------------------------------------------------------

        private bool HasDocuments()
        {
            return documents != null && documents.Count > 0;
        }

        // ---------------------------------------------------------------------------

        private bool PExistOnDisk(string rootPath)
        {
            string projectPath;

            projectPath = ProjectPath(rootPath);

            if (Directory.Exists(projectPath) == true)
            {
                if (documents != null)
                {
                    for (int i = 0; i < documents.Count; i++)
                    {
                        if (documents[i].ExistOnDisk(projectPath) == false)
                            return false;
                    }
                }

                return true;
            }

            return false;
        }

        // ---------------------------------------------------------------------------

        private async Task<bool> PSaveOnDiskAsync(string rootPath, UnityAction<int, int, float> progress = null)
        {
            string projectPath;
            string projectJsonPath;
            string projectImagePath;

            projectPath = ProjectPath(rootPath);
            projectJsonPath = ProjectJsonPath(projectPath);
            projectImagePath = ProjectImagePath(projectPath);

            if (Directory.Exists(rootPath) == false)
                Directory.CreateDirectory(rootPath);

            if (Directory.Exists(projectPath) == false)
                Directory.CreateDirectory(projectPath);

            if (HasDocuments() == true)
            {
                for (int i = 0; i < documents.Count; i++)
                {
                    BSDocument bSDocument;

                    bSDocument = documents[i];

                    if (await bSDocument.SaveOnDiskAsync(projectPath) == false)
                        return false;

                    progress?.Invoke(i, documents.Count, (i / (float)documents.Count));
                }
            }

            BIMServerCenterUtilities.SaveJsonOnDisk(projectJsonPath, this);

            {
                Texture2D projectTexture;
                byte[] projectTextureData;

                projectTexture = await PLoadTextureImageAsync();
                projectTextureData = projectTexture.EncodeToJPG();
                await BIMServerCenterUtilities.WriteDataAsync(projectTextureData, projectImagePath, false);
            }

            return true;
        }

        // ---------------------------------------------------------------------------

        private bool PRemoveFromDisk(string rootPath)
        {
            string projectPath;

            projectPath = ProjectPath(rootPath);

            if (Directory.Exists(projectPath) == false)
                return false;

            BIMServerCenterUtilities.RemoveCompleteDirectoryFromDiskIfExist(projectPath);
            return true;
        }

        // ---------------------------------------------------------------------------

        private bool PLoadFromDisk(string projectPath)
        {
            BSProject bSProject;
            string projectJsonPath;

            bSProject = null;
            projectJsonPath = ProjectJsonPath(projectPath);

            if (Directory.Exists(projectPath) == false)
                return false;

            if (BIMServerCenterUtilities.LoadJsonFromDisk(projectJsonPath, ref bSProject) == true)
            {
                bimServerId = bSProject.bimServerId;
                nameProject = bSProject.nameProject;
                dateLastChange = bSProject.dateLastChange;
                imgUrl = bSProject.imgUrl;
                documents = bSProject.documents;

                return true;
            }

            return false;
        }

        // ---------------------------------------------------------------------------

        private bool PMoveAtDisk(string srcRootPath, string destRootPath)
        {
            string srcProjectPath, destProjectPath;

            srcProjectPath = ProjectPath(srcRootPath);
            destProjectPath = ProjectPath(destRootPath);

            if (Directory.Exists(destProjectPath) == false)
                Directory.CreateDirectory(destRootPath);

            if (Directory.Exists(srcProjectPath) == true)
            {
                Directory.Move(srcProjectPath, destProjectPath);
                return true;
            }

            return false;
        }
    }
}
