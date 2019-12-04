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
using BIMservercenter.Toolkit.Public.Model;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace BIMservercenter.Toolkit
{
    public partial class BIMServerCenter : MonoBehaviour
    {
        // ---------------------------------------------------------------------------
        // Editor
        // ---------------------------------------------------------------------------
        [Header("Toolkit GUIDs")]
        public string developerGUID;
        public string applicationGUID;

        // ---------------------------------------------------------------------------
        // Properties
        // ---------------------------------------------------------------------------
        /// <summary>
        /// Call this property to know if there is any user logged.
        /// </summary>
        public static bool UserLogged
        { get { return Instance.bsSession.userLogged; } }

        /// <summary>
        /// Call this property to get the user name logged.
        /// </summary>
        public static string UserName
        { get { return Instance.bsSession.userName; } }

        /// <summary>
        /// Call this property to get the user image logged.
        /// </summary>
        public static string UserURLImage
        { get { return Instance.bsSession.urlImage; } }

        // ---------------------------------------------------------------------------
        // Disk
        // ---------------------------------------------------------------------------
        /// <summary>
        /// Call this method to save the user session on disk.
        /// </summary>
        /// <param name="rootPath">Input rootPath directory to save the session.</param>
        public static void SaveSessionOnDisk(string rootPath)
        { Instance.bsSession.SaveOnDisk(rootPath); }

        /// <summary>
        /// Call this method to load the user session from disk.
        /// </summary>
        /// <param name="rootPath">Input rootPath directory where load session.</param>
        public static bool LoadSessionFromDisk(string rootPath)
        { return Instance.bsSession.LoadFromDisk(rootPath); }

        /// <summary>
        /// Call this method to remove the user session from disk.
        /// </summary>
        /// <param name="rootPath">Input rootPath directory where remove session.</param>
        public static bool RemoveSessionFromDisk(string rootPath)
        { return Instance.bsSession.RemoveFromDisk(rootPath); }

        // ---------------------------------------------------------------------------
        // BIMserver.center
        // ---------------------------------------------------------------------------        
        /// <summary>
        /// Call this method to login into your BIMserver.center account.
        /// </summary>
        /// <param name="email">Input email to login in your account.</param>
        /// <param name="password">Input password to login in your account.</param>
        public static async Task<BSResponse> BSLoginAsync(string email, string password, BSLanguage bSLanguage = BSLanguage.English)
        { return await PLoginAsync(bSLanguage, email, password); }

        /// <summary>
        /// Call this method to logout from your BIMserver.center account.
        /// </summary>
        public static async Task<BSResponse> BSLogoutAsync(BSLanguage bSLanguage = BSLanguage.English)
        { return await PLogoutAsync(bSLanguage); }

        /// <summary>
        /// Call this method to get all the projects from your BIMserver.center account.
        /// </summary>
        public static async Task<BSResponseList<BSProject>> GetBSProjectListAsync(BSLanguage bSLanguage = BSLanguage.English)
        { return await PGetBSProjectListAsync(bSLanguage); }

        /// <summary>
        /// Call this method to get all the projects with 3D view from your BIMserver.center account.
        /// </summary>
        public static async Task<BSResponseList<BSProject>> GetBSProjectWith3DListAsync(BSLanguage bSLanguage = BSLanguage.English)
        { return await PGetBSProjectWith3DListAsync(bSLanguage); }

        /// <summary>
        /// Call this method to append all the documents into the project.
        /// </summary>
        /// <param name="bSProject">Input project to append documents.</param>
        public static async Task<BSResponse> AppendBSDocumentListAsync(BSProject bSProject, BSLanguage bSLanguage = BSLanguage.English)
        { return await PAppendBSDocumentListAsync(bSLanguage, bSProject); }
    }
}