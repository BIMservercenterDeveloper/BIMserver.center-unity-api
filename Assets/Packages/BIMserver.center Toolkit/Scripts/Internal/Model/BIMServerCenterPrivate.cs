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

using BIMservercenter.Toolkit.Internal.API.Model;
using BIMservercenter.Toolkit.Internal.API.Session;
using BIMservercenter.Toolkit.Internal.Gltf.AsyncAwaitUtil;
using BIMservercenter.Toolkit.Internal.Utilities;
using BIMservercenter.Toolkit.Public.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BIMservercenter.Toolkit
{
    [DisallowMultipleComponent]
    public partial class BIMServerCenter : MonoBehaviour
    {
        public bool InstanceIsActive { get; private set; }
        public static GameObject InstanceGameObject { get { Integrity(); return pInstance.gameObject; } }

        private static BIMServerCenter pInstance;
        private static BIMServerCenter Instance { get { Integrity(); return pInstance; } }

        private BSSession bsSession;
        private BSSessionDelegate bsDelegate;
        private BIMServerCenterError bimServerCenterError = null;

        private static void Integrity()
        {
            BIMServerCenterAssertion.AssertNotNull(pInstance);

            BIMServerCenterAssertion.AssertNotNull(pInstance.developerGUID);
            BIMServerCenterAssertion.AssertNotEquals(pInstance.developerGUID, string.Empty);
            Debug.Assert(pInstance.developerGUID != string.Empty, "BIMServerCenter.developerGUID must be different from empty.");

            BIMServerCenterAssertion.AssertNotNull(pInstance.applicationGUID);
            BIMServerCenterAssertion.AssertNotEquals(pInstance.applicationGUID, string.Empty);
            Debug.Assert(pInstance.applicationGUID != string.Empty, "BIMServerCenter.applicationGUID must be different from empty.");
        }

        // ---------------------------------------------------------------------------
        // Initialization
        // ---------------------------------------------------------------------------

        private void InitializeInstance()
        {
            if (pInstance != null && pInstance != this)
                SetActiveInstance(this, false);
            else
                SetActiveInstance(this, true);
        }

        // ---------------------------------------------------------------------------

        private void Awake()
        {
            if (InstanceIsActive == false)
                InitializeInstance();

            bsSession = new BSSession();
            bsDelegate = new BSSessionDelegate();
            bsDelegate.funcFinishRequestWithError = (errorCode, errorMessage) =>
            {
                bimServerCenterError = new BIMServerCenterError(errorCode, errorMessage);
            };
        }

        // ---------------------------------------------------------------------------

        private void OnValidate()
        {
            if (Application.isPlaying == false)
                InitializeInstance();
        }

        // ---------------------------------------------------------------------------
        // Instance Activation
        // ---------------------------------------------------------------------------

        private static void SetActiveInstance(BIMServerCenter instance, bool isActiveInstance)
        {
            if (instance == null)
                return;

            if (isActiveInstance == true)
            {
                pInstance = instance;
                instance.gameObject.name = "BIMserver.center";
            }
            else
            {
                instance.gameObject.name = "BIMserver.center (Inactive)";
            }

            instance.InstanceIsActive = isActiveInstance;
        }

        // ---------------------------------------------------------------------------

        public static void SetActiveInstance(BIMServerCenter instance)
        {
            SetActiveInstance(pInstance, false);
            SetActiveInstance(instance, true);
        }

        // ---------------------------------------------------------------------------
        // Server Functions
        // ---------------------------------------------------------------------------

        private async Task CheckForErrorsAsync(BSResponse bSResponse)
        {
            await new WaitForUpdate();

            if (bimServerCenterError != null)
            {
                BSResponseTypeError errorType;

                switch (bimServerCenterError.errorCode)
                {
                    case 0: errorType = BSResponseTypeError.Text; break;
                    case 106: errorType = BSResponseTypeError.InvalidSession; break;
                    case 500: errorType = BSResponseTypeError.ServerConnection; break;
                    default: errorType = BSResponseTypeError.Unknown; break;
                }

                bSResponse.IsSucced = false;
                bSResponse.ErrorMessage = bimServerCenterError.errorMessage;
                bSResponse.ErrorType = errorType;

                bimServerCenterError = null;
            }
            else
            {
                bSResponse.IsSucced = true;
                bSResponse.ErrorMessage = null;
                bSResponse.ErrorType = BSResponseTypeError.Unknown;
            }

            await new WaitForBackgroundThread();
        }

        // ---------------------------------------------------------------------------

        private static async Task<BSResponse> PLoginAsync(BSLanguage bSLanguage, string email, string password)
        {
            BSResponse bSResponse;

            bSResponse = new BSResponse();
            bSResponse.IsSucced = false;

            Instance.bsDelegate.funcFinishLoginRequest = () =>
            {
                bSResponse.IsSucced = true;
            };

            await Task.Run(() => Instance.bsSession.Login(bSLanguage, email, password, Instance.developerGUID, Instance.applicationGUID, Instance.bsDelegate));
            await Instance.CheckForErrorsAsync(bSResponse);

            return bSResponse;
        }

        // ---------------------------------------------------------------------------

        private static async Task<BSResponse> PLogoutAsync(BSLanguage bSLanguage)
        {
            BSResponse bSResponse;

            bSResponse = new BSResponse();
            bSResponse.IsSucced = false;

            Instance.bsDelegate.funcFinishLogoutRequest = () =>
            {
                bSResponse.IsSucced = true;
            };

            await Task.Run(() => Instance.bsSession.Logout(bSLanguage, Instance.bsDelegate));
            await Instance.CheckForErrorsAsync(bSResponse);

            return bSResponse;
        }

        // ---------------------------------------------------------------------------

        private static async Task<BSResponseList<BSMProject>> PGetProjectListAsync(BSLanguage bSLanguage)
        {
            BSResponseList<BSMProject> bSResponseList;

            bSResponseList = new BSResponseList<BSMProject>();
            bSResponseList.IsSucced = false;
            bSResponseList.ListElements = null;

            Instance.bsDelegate.funcFinishProjectListRequest = (projectList) =>
            {
                bSResponseList.IsSucced = true;
                bSResponseList.ListElements = projectList;
            };

            await Task.Run(() => Instance.bsSession.ProjectList(bSLanguage, Instance.bsDelegate));
            await Instance.CheckForErrorsAsync(bSResponseList);

            return bSResponseList;
        }

        // ---------------------------------------------------------------------------

        private static async Task<BSResponseList<BSMProject>> PGetProjectEducationalListAsync(BSLanguage bSLanguage)
        {
            BSResponseList<BSMProject> bSResponseList;

            bSResponseList = new BSResponseList<BSMProject>();
            bSResponseList.IsSucced = false;
            bSResponseList.ListElements = null;

            Instance.bsDelegate.funcFinishProjectEducationalListRequest = (projectEducationalList) =>
            {
                bSResponseList.IsSucced = true;
                bSResponseList.ListElements = projectEducationalList;
            };

            await Task.Run(() => Instance.bsSession.ProjectEducationalList(bSLanguage, Instance.bsDelegate));
            await Instance.CheckForErrorsAsync(bSResponseList);

            return bSResponseList;
        }

        // ---------------------------------------------------------------------------

        private static async Task<BSResponseList<BSMProject>> PGetProjectWith3DListAsync(BSLanguage bSLanguage)
        {
            BSResponseList<BSMProject> bSResponseList;

            bSResponseList = new BSResponseList<BSMProject>();
            bSResponseList.IsSucced = false;
            bSResponseList.ListElements = null;

            Instance.bsDelegate.funcFinishProjectWith3DListRequest = (projectWith3DList) =>
            {
                bSResponseList.IsSucced = true;
                bSResponseList.ListElements = projectWith3DList;
            };

            await Task.Run(() => Instance.bsSession.ProjectWith3DList(bSLanguage, Instance.bsDelegate));
            await Instance.CheckForErrorsAsync(bSResponseList);

            return bSResponseList;
        }

        // ---------------------------------------------------------------------------

        private static async Task<BSResponseList<BSMProject>> PGetProjectEducationalWith3DListAsync(BSLanguage bSLanguage)
        {
            BSResponseList<BSMProject> bSResponseList;

            bSResponseList = new BSResponseList<BSMProject>();
            bSResponseList.IsSucced = false;
            bSResponseList.ListElements = null;

            Instance.bsDelegate.funcFinishProjectEducationalWith3DListRequest = (projectEducationalWith3DList) =>
            {
                bSResponseList.IsSucced = true;
                bSResponseList.ListElements = projectEducationalWith3DList;
            };

            await Task.Run(() => Instance.bsSession.ProjectEducationalWith3DList(bSLanguage, Instance.bsDelegate));
            await Instance.CheckForErrorsAsync(bSResponseList);

            return bSResponseList;
        }

        // ---------------------------------------------------------------------------

        private static async Task<BSResponseList<BSMDocument>> PGetDocumentListAsync(BSLanguage bSLanguage, string projectBIMServerId)
        {
            BSResponseList<BSMDocument> bSResponseList;

            bSResponseList = new BSResponseList<BSMDocument>();
            bSResponseList.IsSucced = false;
            bSResponseList.ListElements = null;

            Instance.bsDelegate.funcFinishDocumentListRequest = (documentList) =>
            {
                bSResponseList.IsSucced = true;
                bSResponseList.ListElements = documentList;
            };

            await Task.Run(() => Instance.bsSession.DocumentList(bSLanguage, projectBIMServerId, Instance.bsDelegate));
            await Instance.CheckForErrorsAsync(bSResponseList);

            return bSResponseList;
        }

        // ---------------------------------------------------------------------------
        // BIMserver.center
        // ---------------------------------------------------------------------------

        private static void PAppendBSAssociatedDocumentListAsync(BSMDocument bSModelDocument, BSDocument bSDocument)
        {
            List<BSMAssociatedDocument> associatedDocumentList;

            bSDocument.associatedDocuments = new List<BSAssociatedDocument>();
            associatedDocumentList = bSModelDocument.associatedDocumentsVisiblesList;

            for (int i = 0; i < associatedDocumentList.Count; i++)
            {
                BSAssociatedDocument bSAssociatedDocument;
                BSMAssociatedDocument bSModelAssociatedDocument;

                bSAssociatedDocument = new BSAssociatedDocument();
                bSModelAssociatedDocument = associatedDocumentList[i];

                bSAssociatedDocument.nameFile = bSModelAssociatedDocument.name;
                bSAssociatedDocument.urlDownload = bSModelAssociatedDocument.url;
                bSAssociatedDocument.dateLastChange = bSModelAssociatedDocument.dateTimedate;

                bSDocument.associatedDocuments.Add(bSAssociatedDocument);
            }
        }

        // ---------------------------------------------------------------------------

        private static async Task<BSResponse> PAppendBSDocumentListAsync(BSLanguage bSLanguage, BSProject bSProject)
        {
            BSResponse bSResponse;
            BSResponseList<BSMDocument> bSResponseList;

            bSResponse = new BSResponse();
            bSResponseList = await PGetDocumentListAsync(bSLanguage, bSProject.bimServerId);

            if (bSResponseList.IsSucced == true)
            {
                List<BSMDocument> documentList;

                bSProject.documents = new List<BSDocument>();
                documentList = bSResponseList.ListElements;

                for (int i = 0; i < documentList.Count; i++)
                {
                    BSDocument bSDocument;
                    BSMDocument bSModelDocument;

                    bSDocument = new BSDocument();
                    bSModelDocument = documentList[i];

                    bSDocument.bimServerId = bSModelDocument.bimserverId;
                    bSDocument.imgUrl = bSModelDocument.img;
                    bSDocument.nameDocument = bSModelDocument.name;
                    bSDocument.dateLastChange = bSModelDocument.dateTimedate;
                    bSDocument.urlDownloadDocument = bSModelDocument.url;

                    PAppendBSAssociatedDocumentListAsync(bSModelDocument, bSDocument);
                    bSProject.documents.Add(bSDocument);
                }

                bSResponse.IsSucced = true;
                bSResponse.ErrorType = BSResponseTypeError.Unknown;
                bSResponse.ErrorMessage = null;
            }
            else
            {
                bSResponse.IsSucced = false;
                bSResponse.ErrorType = bSResponseList.ErrorType;
                bSResponse.ErrorMessage = bSResponseList.ErrorMessage;
            }

            return bSResponse;
        }

        // ---------------------------------------------------------------------------

        private static async Task<BSResponseList<BSProject>> PGetBSProjectListAsync(BSLanguage bSLanguage)
        {
            BSResponseList<BSProject> bSResponseProjectList;
            BSResponseList<BSMProject> bSResponseModelProjectList;

            bSResponseProjectList = new BSResponseList<BSProject>();
            bSResponseModelProjectList = await PGetProjectListAsync(bSLanguage);

            if (bSResponseModelProjectList.IsSucced == true)
            {
                List<BSProject> bSProjectList;
                List<BSMProject> bSModelProjectModelList;

                bSProjectList = new List<BSProject>();
                bSModelProjectModelList = bSResponseModelProjectList.ListElements;

                for (int i = 0; i < bSModelProjectModelList.Count; i++)
                {
                    BSProject bSProject;
                    BSMProject bSModelProject;

                    bSProject = new BSProject();
                    bSModelProject = bSModelProjectModelList[i];

                    bSProject.bimServerId = bSModelProject.bimServerId;
                    bSProject.nameProject = bSModelProject.name;
                    bSProject.dateLastChange = bSModelProject.dateLastChange;
                    bSProject.imgUrl = bSModelProject.img;
                    bSProject.imgUrlSmall = bSModelProject.imgSmall;
                    bSProject.imgUrlLarge = bSModelProject.imgLarge;
                    bSProject.imgUrlLandscape = bSModelProject.imgLandscape;

                    bSProjectList.Add(bSProject);
                }

                bSResponseProjectList.IsSucced = true;
                bSResponseProjectList.ErrorMessage = null;
                bSResponseProjectList.ErrorType = BSResponseTypeError.Unknown;
                bSResponseProjectList.ListElements = bSProjectList;
            }
            else
            {
                bSResponseProjectList.IsSucced = false;
                bSResponseProjectList.ErrorMessage = bSResponseModelProjectList.ErrorMessage;
                bSResponseProjectList.ErrorType = bSResponseModelProjectList.ErrorType;
                bSResponseProjectList.ListElements = null;
            }

            return bSResponseProjectList;
        }

        // ---------------------------------------------------------------------------

        private static async Task<BSResponseList<BSProject>> PGetBSProjectEducationalListAsync(BSLanguage bSLanguage)
        {
            BSResponseList<BSProject> bSResponseProjectEducationalList;
            BSResponseList<BSMProject> bSResponseModelProjectEducationalList;

            bSResponseProjectEducationalList = new BSResponseList<BSProject>();
            bSResponseModelProjectEducationalList = await PGetProjectEducationalListAsync(bSLanguage);

            if (bSResponseModelProjectEducationalList.IsSucced == true)
            {
                List<BSProject> bSProjectEducationalList;
                List<BSMProject> bSModelProjectEducationalModelList;

                bSProjectEducationalList = new List<BSProject>();
                bSModelProjectEducationalModelList = bSResponseModelProjectEducationalList.ListElements;

                for (int i = 0; i < bSModelProjectEducationalModelList.Count; i++)
                {
                    BSProject bSProject;
                    BSMProject bSModelProject;

                    bSProject = new BSProject();
                    bSModelProject = bSModelProjectEducationalModelList[i];

                    bSProject.bimServerId = bSModelProject.bimServerId;
                    bSProject.nameProject = bSModelProject.name;
                    bSProject.dateLastChange = bSModelProject.dateLastChange;
                    bSProject.imgUrl = bSModelProject.img;
                    bSProject.imgUrlSmall = bSModelProject.imgSmall;
                    bSProject.imgUrlLarge = bSModelProject.imgLarge;
                    bSProject.imgUrlLandscape = bSModelProject.imgLandscape;

                    bSProjectEducationalList.Add(bSProject);
                }

                bSResponseProjectEducationalList.IsSucced = true;
                bSResponseProjectEducationalList.ErrorMessage = null;
                bSResponseProjectEducationalList.ErrorType = BSResponseTypeError.Unknown;
                bSResponseProjectEducationalList.ListElements = bSProjectEducationalList;
            }
            else
            {
                bSResponseProjectEducationalList.IsSucced = false;
                bSResponseProjectEducationalList.ErrorMessage = bSResponseModelProjectEducationalList.ErrorMessage;
                bSResponseProjectEducationalList.ErrorType = bSResponseModelProjectEducationalList.ErrorType;
                bSResponseProjectEducationalList.ListElements = null;
            }

            return bSResponseProjectEducationalList;
        }

        // ---------------------------------------------------------------------------

        private static async Task<BSResponseList<BSProject>> PGetBSProjectWith3DListAsync(BSLanguage bSLanguage)
        {
            BSResponseList<BSProject> bSResponseProjectWith3DList;
            BSResponseList<BSMProject> bSResponseModelProjectWith3DList;

            bSResponseProjectWith3DList = new BSResponseList<BSProject>();
            bSResponseModelProjectWith3DList = await PGetProjectWith3DListAsync(bSLanguage);

            if (bSResponseModelProjectWith3DList.IsSucced == true)
            {
                List<BSProject> bSProjectWith3DList;
                List<BSMProject> bSModelProjectWith3DModelList;

                bSProjectWith3DList = new List<BSProject>();
                bSModelProjectWith3DModelList = bSResponseModelProjectWith3DList.ListElements;

                for (int i = 0; i < bSModelProjectWith3DModelList.Count; i++)
                {
                    BSProject bSProject;
                    BSMProject bSModelProject;

                    bSProject = new BSProject();
                    bSModelProject = bSModelProjectWith3DModelList[i];

                    bSProject.bimServerId = bSModelProject.bimServerId;
                    bSProject.nameProject = bSModelProject.name;
                    bSProject.dateLastChange = bSModelProject.dateLastChange;
                    bSProject.imgUrl = bSModelProject.img;
                    bSProject.imgUrlSmall = bSModelProject.imgSmall;
                    bSProject.imgUrlLarge = bSModelProject.imgLarge;
                    bSProject.imgUrlLandscape = bSModelProject.imgLandscape;

                    bSProjectWith3DList.Add(bSProject);
                }

                bSResponseProjectWith3DList.IsSucced = true;
                bSResponseProjectWith3DList.ErrorMessage = null;
                bSResponseProjectWith3DList.ErrorType = BSResponseTypeError.Unknown;
                bSResponseProjectWith3DList.ListElements = bSProjectWith3DList;
            }
            else
            {
                bSResponseProjectWith3DList.IsSucced = false;
                bSResponseProjectWith3DList.ErrorMessage = bSResponseModelProjectWith3DList.ErrorMessage;
                bSResponseProjectWith3DList.ErrorType = bSResponseModelProjectWith3DList.ErrorType;
                bSResponseProjectWith3DList.ListElements = null;
            }

            return bSResponseProjectWith3DList;
        }

        // ---------------------------------------------------------------------------

        private static async Task<BSResponseList<BSProject>> PGetBSProjectEducationalWith3DListAsync(BSLanguage bSLanguage)
        {
            BSResponseList<BSProject> bSResponseProjectEducationalWith3DList;
            BSResponseList<BSMProject> bSResponseModelProjectEducationalWith3DList;

            bSResponseProjectEducationalWith3DList = new BSResponseList<BSProject>();
            bSResponseModelProjectEducationalWith3DList = await PGetProjectEducationalWith3DListAsync(bSLanguage);

            if (bSResponseModelProjectEducationalWith3DList.IsSucced == true)
            {
                List<BSProject> bSProjectEducationalWith3DList;
                List<BSMProject> bSModelProjectEducationalWith3DModelList;

                bSProjectEducationalWith3DList = new List<BSProject>();
                bSModelProjectEducationalWith3DModelList = bSResponseModelProjectEducationalWith3DList.ListElements;

                for (int i = 0; i < bSModelProjectEducationalWith3DModelList.Count; i++)
                {
                    BSProject bSProject;
                    BSMProject bSModelProject;

                    bSProject = new BSProject();
                    bSModelProject = bSModelProjectEducationalWith3DModelList[i];

                    bSProject.bimServerId = bSModelProject.bimServerId;
                    bSProject.nameProject = bSModelProject.name;
                    bSProject.dateLastChange = bSModelProject.dateLastChange;
                    bSProject.imgUrl = bSModelProject.img;
                    bSProject.imgUrlSmall = bSModelProject.imgSmall;
                    bSProject.imgUrlLarge = bSModelProject.imgLarge;
                    bSProject.imgUrlLandscape = bSModelProject.imgLandscape;

                    bSProjectEducationalWith3DList.Add(bSProject);
                }

                bSResponseProjectEducationalWith3DList.IsSucced = true;
                bSResponseProjectEducationalWith3DList.ErrorMessage = null;
                bSResponseProjectEducationalWith3DList.ErrorType = BSResponseTypeError.Unknown;
                bSResponseProjectEducationalWith3DList.ListElements = bSProjectEducationalWith3DList;
            }
            else
            {
                bSResponseProjectEducationalWith3DList.IsSucced = false;
                bSResponseProjectEducationalWith3DList.ErrorMessage = bSResponseModelProjectEducationalWith3DList.ErrorMessage;
                bSResponseProjectEducationalWith3DList.ErrorType = bSResponseModelProjectEducationalWith3DList.ErrorType;
                bSResponseProjectEducationalWith3DList.ListElements = null;
            }

            return bSResponseProjectEducationalWith3DList;
        }
    }
}