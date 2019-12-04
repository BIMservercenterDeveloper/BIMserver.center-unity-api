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

using BIMservercenter.Toolkit.Internal.API.Request;
using BIMservercenter.Toolkit.Internal.API.Response;
using BIMservercenter.Toolkit.Internal.Utilities;
using BIMservercenter.Toolkit.Public.Model;

namespace BIMservercenter.Toolkit.Internal.API.Session
{
    public class BSSession
    {
        private const string UserClassFile = "00.binon";

        public bool userLogged { get { return bimServerId != null; } }
        public string versionAPI;
        public string bimServerId;
        public string sessionId;
        public string userName;
        public string urlImage;

        // ---------------------------------------------------------------------------
        // Initializers
        // ---------------------------------------------------------------------------

        public BSSession()
        {
            this.versionAPI = "1.0.0";
            this.bimServerId = null;
            this.sessionId = null;
            this.userName = null;
            this.urlImage = null;
        }

        // ---------------------------------------------------------------------------

        public void ResetSession()
        {
            this.versionAPI = "1.0.0";
            this.bimServerId = null;
            this.sessionId = null;
            this.userName = null;
            this.urlImage = null;
        }

        // ---------------------------------------------------------------------------
        // Disk
        // ---------------------------------------------------------------------------

        public void SaveOnDisk(string directoryPath)
        {
            BIMServerCenterUtilitiesDisk.CreateJsonFile(directoryPath, UserClassFile, this);
        }

        // ---------------------------------------------------------------------------

        public bool LoadFromDisk(string directoryPath)
        {
            BSSession session;

            session = BIMServerCenterUtilitiesDisk.ReadFromJsonFile<BSSession>(directoryPath, UserClassFile);

            if (session != null)
            {
                if (this.versionAPI.Equals(session.versionAPI) == true)
                {
                    this.bimServerId = session.bimServerId;
                    this.sessionId = session.sessionId;
                    this.userName = session.userName;
                    this.urlImage = session.urlImage;

                    return true;
                }
                else
                {
                    BIMServerCenterUtilitiesDisk.RemoveJsonFile(directoryPath, UserClassFile);
                    return false;
                }
            }

            return false;
        }

        // ---------------------------------------------------------------------------

        public bool RemoveFromDisk(string directoryPath)
        {
            return BIMServerCenterUtilitiesDisk.RemoveJsonFile(directoryPath, UserClassFile);
        }

        // ---------------------------------------------------------------------------
        // Authentication
        // ---------------------------------------------------------------------------

        public void Login(BSLanguage bSLanguage, string email, string password, string guidDeveloper, string guidApplication, BSSessionDelegate sessionDelegate)
        {
            BSIResponse response;
            string command;
            BSSessionCommand sessionCommand;

            response = new BSLoginResponse();
            command = BSLoginRequest.LoginRequest(bSLanguage, versionAPI, email, password, guidDeveloper, guidApplication);

            sessionCommand = new BSSessionCommand(command, response, this, sessionDelegate);
            sessionCommand.Execute();
        }

        // ---------------------------------------------------------------------------

        public void Logout(BSLanguage bSLanguage, BSSessionDelegate sessionDelegate)
        {
            BSIResponse response;
            string command;
            BSSessionCommand sessionCommand;

            response = new BSLogoutResponse();
            command = BSLogoutRequest.LogoutRequest(bSLanguage, sessionId);

            sessionCommand = new BSSessionCommand(command, response, this, sessionDelegate);
            sessionCommand.Execute();
        }

        // ---------------------------------------------------------------------------
        // Projects
        // ---------------------------------------------------------------------------

        public void ProjectList(BSLanguage bSLanguage, BSSessionDelegate sessionDelegate)
        {
            BSIResponse response;
            string command;
            BSSessionCommand sessionCommand;

            response = new BSProjectListResponse();
            command = BSProjectListRequest.ProjectListRequest(bSLanguage, sessionId);

            sessionCommand = new BSSessionCommand(command, response, this, sessionDelegate);
            sessionCommand.Execute();
        }

        // ---------------------------------------------------------------------------

        public void ProjectWith3DList(BSLanguage bSLanguage, BSSessionDelegate sessionDelegate)
        {
            BSIResponse response;
            string command;
            BSSessionCommand sessionCommand;

            response = new BSProjectWith3DListResponse();
            command = BSProjectWith3DListRequest.ProjectWith3DListRequest(bSLanguage, sessionId);

            sessionCommand = new BSSessionCommand(command, response, this, sessionDelegate);
            sessionCommand.Execute();
        }

        // ---------------------------------------------------------------------------
        // Documents
        // ---------------------------------------------------------------------------

        public void DocumentList(BSLanguage bSLanguage, string projectBIMServerId, BSSessionDelegate sessionDelegate)
        {
            BSIResponse response;
            string command;
            BSSessionCommand sessionCommand;

            response = new BSDocumentListResponse();
            command = BSDocumentListRequest.DocumentListRequest(bSLanguage, sessionId, projectBIMServerId);

            sessionCommand = new BSSessionCommand(command, response, this, sessionDelegate);
            sessionCommand.Execute();
        }
    }
}
