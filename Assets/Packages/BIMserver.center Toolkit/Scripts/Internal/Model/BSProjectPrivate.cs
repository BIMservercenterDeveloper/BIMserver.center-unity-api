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
using BIMservercenter.Toolkit.Public.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using System.IO;
using System;

namespace BIMservercenter.Toolkit.Public.Model
{
    [System.Serializable]
    public partial class BSProject
    {
        private const string ProjectClassFile = "00.binon";
        private const string ProjectImageFile = "01.binong";
        private const string ProjectImageSmallFile = "01.binong";
        private const string ProjectImageLargeFile = "02.binong";
        private const string ProjectImageLandscapeFile = "03.binong";

        public string bimServerId;
        public string nameProject;
        public string dateLastChange;
        public string imgUrl;
        public string imgUrlSmall;
        public string imgUrlLarge;
        public string imgUrlLandscape;
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

        private string ProjectImageSmallPath(string projectPath)
        {
            return Path.Combine(projectPath, ProjectImageSmallFile);
        }

        // ---------------------------------------------------------------------------

        private string ProjectImageLargePath(string projectPath)
        {
            return Path.Combine(projectPath, ProjectImageLargeFile);
        }

        // ---------------------------------------------------------------------------

        private string ProjectImageLandscapePath(string projectPath)
        {
            return Path.Combine(projectPath, ProjectImageLandscapeFile);
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

        private static async Task<Texture2D> PLoadTextureImageCommonAsync(
                        string imgUrl,
                        string projectImagePath,
                        Texture2D defaultTexture = null)
        {
            if (File.Exists(projectImagePath) == true)
                return BIMServerCenterUtilities.LoadTexture(projectImagePath);

            return await BIMServerCenterUtilities.DownloadTextureAsync(imgUrl, defaultTexture);
        }

        // ---------------------------------------------------------------------------

        private async Task<Texture2D> PLoadTextureImageAsync(string rootPath = null, Texture2D defaultTexture = null)
        {
            if (rootPath != null)
            {
                string projectPath, projectImagePath;

                projectPath = ProjectPath(rootPath);
                projectImagePath = ProjectImagePath(projectPath);
                return await PLoadTextureImageCommonAsync(imgUrl, projectImagePath, defaultTexture);
            }

            return await BIMServerCenterUtilities.DownloadTextureAsync(imgUrl, defaultTexture);
        }

        // ---------------------------------------------------------------------------

        private async Task<Texture2D> PLoadTextureImageSmallAsync(string rootPath = null, Texture2D defaultTexture = null)
        {
            if (rootPath != null)
            {
                string projectPath, projectImagePath;

                projectPath = ProjectPath(rootPath);
                projectImagePath = ProjectImageSmallPath(projectPath);
                return await PLoadTextureImageCommonAsync(imgUrlSmall, projectImagePath, defaultTexture);
            }

            return await BIMServerCenterUtilities.DownloadTextureAsync(imgUrlSmall, defaultTexture);
        }

        // ---------------------------------------------------------------------------

        private async Task<Texture2D> PLoadTextureImageLargeAsync(string rootPath = null, Texture2D defaultTexture = null)
        {
            if (rootPath != null)
            {
                string projectPath, projectImagePath;

                projectPath = ProjectPath(rootPath);
                projectImagePath = ProjectImageLargePath(projectPath);
                return await PLoadTextureImageCommonAsync(imgUrlLarge, projectImagePath, defaultTexture);
            }

            return await BIMServerCenterUtilities.DownloadTextureAsync(imgUrlLarge, defaultTexture);
        }

        // ---------------------------------------------------------------------------

        private async Task<Texture2D> PLoadTextureImageLandscapeAsync(string rootPath = null, Texture2D defaultTexture = null)
        {
            if (rootPath != null)
            {
                string projectPath, projectImagePath;

                projectPath = ProjectPath(rootPath);
                projectImagePath = ProjectImageLandscapePath(projectPath);
                return await PLoadTextureImageCommonAsync(imgUrlLandscape, projectImagePath, defaultTexture);
            }

            return await BIMServerCenterUtilities.DownloadTextureAsync(imgUrlLandscape, defaultTexture);
        }

        // ---------------------------------------------------------------------------
        // Gltf
        // ---------------------------------------------------------------------------

        private async Task<List<BSGltf>> PLoadGltfsAsync(
                        string rootPath,
                        bool generateColliders,
                        bool hide,
                        FuncProgressPercIndexesUpdate funcProgressPercIndexesUpdate,
                        CancellationTokenSource cancellationTokenSource)
        {
            List<BSGltf> gltfList;

            gltfList = new List<BSGltf>();

            {
                int numDocuments;
                string projectPath;

                numDocuments = documents.Count;
                projectPath = ProjectPath(rootPath);

                funcProgressPercIndexesUpdate?.Invoke(0, numDocuments, 0f);

                for (int i = 0; i < numDocuments; i++)
                {
                    BSDocument bSDocument;
                    List<BSGltf> documentGltfObjectList;
                    FuncProgressPercIndexesUpdate funcProgressPercIndexesUpdateLocal;

                    bSDocument = documents[i];

                    funcProgressPercIndexesUpdateLocal = (actualIndex, total, percentage) =>
                    {
                        funcProgressPercIndexesUpdate?.Invoke(i, numDocuments, ((i + percentage) / numDocuments));
                    };

                    documentGltfObjectList = await bSDocument.LoadGltfsAsync(projectPath, generateColliders, hide, funcProgressPercIndexesUpdateLocal, cancellationTokenSource);
                    gltfList.AddRange(documentGltfObjectList);
                }

                funcProgressPercIndexesUpdate?.Invoke(numDocuments, numDocuments, 1f);
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

        private async Task PSaveOnDiskAsync(
                        string rootPath,
                        FuncProgressPercIndexesUpdate funcProgressPercIndexesUpdate,
                        CancellationTokenSource cancellationTokenSource)
        {
            string projectPath;
            string projectImagePath;
            string projectImageSmallPath;
            string projectImageLargePath;
            string projectImageLandscapePath;

            projectPath = ProjectPath(rootPath);
            projectImagePath = ProjectImagePath(projectPath);
            projectImageSmallPath = ProjectImageSmallPath(projectPath);
            projectImageLargePath = ProjectImageLargePath(projectPath);
            projectImageLandscapePath = ProjectImageLandscapePath(projectPath);

            if (Directory.Exists(rootPath) == false)
                Directory.CreateDirectory(rootPath);

            if (Directory.Exists(projectPath) == false)
                Directory.CreateDirectory(projectPath);

            if (HasDocuments() == true)
            {
                int numDocuments;

                numDocuments = documents.Count;

                funcProgressPercIndexesUpdate?.Invoke(0, numDocuments, 0f);

                for (int i = 0; i < numDocuments; i++)
                {
                    BSDocument bSDocument;
                    FuncProgressPercIndexesUpdate funcProgressPercIndexesUpdateLocal;

                    bSDocument = documents[i];

                    funcProgressPercIndexesUpdateLocal = (actualIndex, total, percentage) =>
                    {
                        funcProgressPercIndexesUpdate?.Invoke(i, numDocuments, ((i + percentage) / numDocuments));
                    };

                    await bSDocument.SaveOnDiskAsync(projectPath, funcProgressPercIndexesUpdateLocal, cancellationTokenSource);
                }

                funcProgressPercIndexesUpdate?.Invoke(numDocuments, numDocuments, 1f);
            }

            {
                Texture2D projectTexture;
                byte[] projectTextureData;

                if (string.IsNullOrEmpty(imgUrl) == false)
                {
                    projectTexture = await PLoadTextureImageAsync();
                    projectTextureData = projectTexture.EncodeToJPG();
                    await BIMServerCenterUtilities.WriteDataAsync(projectTextureData, projectImagePath, false);
                }

                if (string.IsNullOrEmpty(imgUrlSmall) == false)
                {
                    projectTexture = await PLoadTextureImageSmallAsync();
                    projectTextureData = projectTexture.EncodeToJPG();
                    await BIMServerCenterUtilities.WriteDataAsync(projectTextureData, projectImageSmallPath, false);
                }

                if (string.IsNullOrEmpty(imgUrlLarge) == false)
                {
                    projectTexture = await PLoadTextureImageLargeAsync();
                    projectTextureData = projectTexture.EncodeToJPG();
                    await BIMServerCenterUtilities.WriteDataAsync(projectTextureData, projectImageLargePath, false);
                }

                if (string.IsNullOrEmpty(imgUrlLandscape) == false)
                {
                    projectTexture = await PLoadTextureImageLandscapeAsync();
                    projectTextureData = projectTexture.EncodeToJPG();
                    await BIMServerCenterUtilities.WriteDataAsync(projectTextureData, projectImageLandscapePath, false);
                }
            }
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
                imgUrlSmall = bSProject.imgUrlSmall;
                imgUrlLarge = bSProject.imgUrlLarge;
                imgUrlLandscape = bSProject.imgUrlLandscape;
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
