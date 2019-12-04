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
using UnityEngine;

namespace BIMservercenter.Toolkit.Internal.Shaders
{
#if UNITY_EDITOR
    public static class BIMServerCenterShaderGUIUtilities
    {
        public static class Styles
        {
            public static readonly GUIContent DepthWriteWarning = new GUIContent("<color=yellow>Warning:</color> Depth buffer sharing is enabled for this project, but this material does not write depth. Enabling depth will improve reprojection, but may cause rendering artifacts in translucent materials.");
            public static readonly GUIContent DepthWriteFixNowButton = new GUIContent("Fix Now", "Enables Depth Write For This Material");
        }

        public static bool DisplayDepthWriteWarning(MaterialEditor materialEditor, string dialogTitle = "Depth Write", string dialogMessage = "Change this material to write to the depth buffer?")
        {
            bool dialogConfirmed = false;

            if (BIMServerCenterOptimizeUtils.IsDepthBufferSharingEnabled())
            {
                var defaultValue = EditorStyles.helpBox.richText;
                EditorStyles.helpBox.richText = true;

                if (materialEditor.HelpBoxWithButton(Styles.DepthWriteWarning, Styles.DepthWriteFixNowButton))
                {
                    if (EditorUtility.DisplayDialog(dialogTitle, dialogMessage, "Yes", "No"))
                    {
                        dialogConfirmed = true;
                    }
                }

                EditorStyles.helpBox.richText = defaultValue;
            }

            return dialogConfirmed;
        }
    }
#endif
}
