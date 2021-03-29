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

using UnityEditor;

namespace BIMservercenter.Toolkit.Internal.Shaders
{
#if UNITY_EDITOR
    public static class BIMServerCenterOptimizeUtils
    {
        public static bool IsDepthBufferSharingEnabled()
        {
            if (IsBuildTargetOpenVR())
            {
#pragma warning disable 0618
                if (PlayerSettings.VROculus.sharedDepthBuffer)
#pragma warning restore 0618
                {
                    return true;
                }
            }
            else if (IsBuildTargetUWP())
            {
#if UNITY_2019_1_OR_NEWER
#pragma warning disable 0618
                if (PlayerSettings.VRWindowsMixedReality.depthBufferSharingEnabled)
#pragma warning restore 0618
                {
                    return true;
                }
#else
                var playerSettings = GetSettingsObject("PlayerSettings");
                var property = playerSettings?.FindProperty("vrSettings.hololens.depthBufferSharingEnabled");
                if (property != null && property.boolValue)
                {
                    return true;
                }
#endif
            }

            return false;
        }

        public static bool IsBuildTargetOpenVR()
        {
            return EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows ||
                EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64;
        }

        public static bool IsBuildTargetUWP()
        {
            return EditorUserBuildSettings.activeBuildTarget == BuildTarget.WSAPlayer;
        }
    }
#endif
}