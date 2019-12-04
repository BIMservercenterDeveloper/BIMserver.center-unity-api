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

namespace BIMservercenter.Toolkit.Internal.Editor
{
    [CustomEditor(typeof(BIMServerCenter))]
    internal class BIMServerCenterEditorGUI : UnityEditor.Editor
    {
        private Texture2D bimServerCenterLogo;
        private SerializedProperty developerGUID;
        private SerializedProperty applicationGUID;

        private void OnEnable()
        {
            string pathBIMServerCenterLogo;

            pathBIMServerCenterLogo = "Assets/Packages/BIMserver.center Toolkit/Textures/BIMServerCenterToolkitLogo.png";

            bimServerCenterLogo = (Texture2D)AssetDatabase.LoadAssetAtPath(pathBIMServerCenterLogo, typeof(Texture2D));
            developerGUID = serializedObject.FindProperty("developerGUID");
            applicationGUID = serializedObject.FindProperty("applicationGUID");
        }

        // ---------------------------------------------------------------------------

        public override void OnInspectorGUI()
        {
            BIMServerCenter bimServerCenter;

            bimServerCenter = (BIMServerCenter)target;

            if (bimServerCenter.InstanceIsActive == false)
            {
                EditorGUILayout.HelpBox("This instance of the toolkit is inactive. There can only be one active instance loaded at any time.", MessageType.Warning);
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Select Active Instance"))
                    Selection.activeGameObject = BIMServerCenter.InstanceGameObject;
                else if (GUILayout.Button("Make this the Active Instance"))
                    BIMServerCenter.SetActiveInstance(bimServerCenter);

                EditorGUILayout.EndHorizontal();

                return;
            }

            serializedObject.Update();

            GUILayout.Space(25f);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(bimServerCenterLogo, GUILayout.MaxHeight(80f));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(5f);

            GUILayout.BeginVertical();
            EditorGUILayout.PropertyField(developerGUID);
            EditorGUILayout.PropertyField(applicationGUID);
            GUILayout.Space(5f);
            GUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}