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
using System.Collections.Generic;

namespace BIMservercenter.Toolkit.Internal.API.Session
{
    public class BSSessionDelegate
    {
        public delegate void FinishLoginRequest();
        public FinishLoginRequest funcFinishLoginRequest = null;

        public delegate void FinishLogoutRequest();
        public FinishLogoutRequest funcFinishLogoutRequest = null;

        public delegate void FinishRegisterRequest();
        public FinishRegisterRequest funcFinishRegisterRequest = null;

        public delegate void FinishForgotPasswordRequest();
        public FinishForgotPasswordRequest funcFinishForgotPasswordRequest = null;

        public delegate void FinishProjectListRequest(List<BSMProject> projectList);
        public FinishProjectListRequest funcFinishProjectListRequest = null;

        public delegate void FinishProjectEducationalListRequest(List<BSMProject> projectEducationalList);
        public FinishProjectListRequest funcFinishProjectEducationalListRequest = null;

        public delegate void FinishProjectWith3DListRequest(List<BSMProject> projectWith3DList);
        public FinishProjectWith3DListRequest funcFinishProjectWith3DListRequest = null;

        public delegate void FinishProjectEducationalWith3DListRequest(List<BSMProject> projectEducationalWith3DList);
        public FinishProjectWith3DListRequest funcFinishProjectEducationalWith3DListRequest = null;

        public delegate void FinishDocumentListRequest(List<BSMDocument> documentList);
        public FinishDocumentListRequest funcFinishDocumentListRequest = null;

        public delegate void FinishRequestWithError(int code, string message);
        public FinishRequestWithError funcFinishRequestWithError = null;
    }
}
