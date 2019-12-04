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

using BIMservercenter.Toolkit.Public.Model;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BIMservercenter.Toolkit.Example
{
    public class BIMServerCenterConnection : MonoBehaviour
    {
        /*
     * 1. Enter your "Developer GUID" and "Application GUID" data, which allow you to identify yourself
     *      as a developer on the platform. This data can be found in the profile of the application created in
     *      BIMserver.center Developers. The data must be entered in the gameObject "BIMserver.center".
     *      
     * 2. Enter the "UserEmail" and "UserPassword" data of a user registered on the platform to open a session with him
     *      in order to access user information stored on the platform. The data must be entered in the 
     *      gameObject "BIMserver.center Connection".
    */

        public string UserEmail;
        public string UserPassword;

        // ---------------------------------------------------------------------------

        private async void Start()
        {
            BSLanguage responseLanguage;
            BSResponse responseLogin;

            Debug.Assert(UserEmail != string.Empty, "BIMServerCenterConnection.UserEmail must be different from empty.");
            Debug.Assert(UserPassword != string.Empty, "BIMServerCenterConnection.UserPassword must be different from empty.");

            //The language in which the platform must respond in case an error message appears.
            responseLanguage = BSLanguage.English;

            //The platform is requested to login with the user data provided by the developer.
            responseLogin = await BIMServerCenter.BSLoginAsync(UserEmail, UserPassword, responseLanguage);

            //It is verified if the login on the platform has been satisfactory.
            if (responseLogin.IsSucced == true)
            {
                BSResponseList<BSProject> responseGetBSProjectWith3DList;
                BSResponse responseLogout;

                //The platform is requested to obtain the list of projects with 3D information that belongs to the user.
                responseGetBSProjectWith3DList = await BIMServerCenter.GetBSProjectWith3DListAsync(responseLanguage);

                //It is verified if the list of projects with 3D information obtained from the platform has been satisfactory.
                if (responseGetBSProjectWith3DList.IsSucced == true)
                {
                    List<BSProject> bSProjectWith3DList;

                    bSProjectWith3DList = responseGetBSProjectWith3DList.ListElements;

                    if (bSProjectWith3DList.Count > 0)
                    {
                        BSProject bSProjectWith3D;
                        BSResponse responseAppendBSDocumentList;

                        bSProjectWith3D = bSProjectWith3DList[0];

                        //The platform is requested to obtain the list of documents and associates of the project that belongs to the user.
                        responseAppendBSDocumentList = await BIMServerCenter.AppendBSDocumentListAsync(bSProjectWith3D, responseLanguage);

                        //It is verified if the list of documents and associates of the project obtained from the platform has been satisfactory.
                        if (responseAppendBSDocumentList.IsSucced == true)
                        {
                            string temporalFolder;

                            temporalFolder = Path.Combine(Application.temporaryCachePath, "BIMserver.center Toolkit");

                            //The project files belonging to the user are downloaded to the temporary directory.
                            if (await bSProjectWith3D.SaveOnDiskAsync(temporalFolder) == true)
                            {
                                List<BSGltf> glTFs;

                                //The project glTF files belonging to the user are loaded from the temporary directory.
                                glTFs = await bSProjectWith3D.LoadGltfsAsync(temporalFolder, false);
                            }
                        }
                    }
                }

                //The platform is requested to logout.
                responseLogout = await BIMServerCenter.BSLogoutAsync(responseLanguage);
            }
        }
    }
}
