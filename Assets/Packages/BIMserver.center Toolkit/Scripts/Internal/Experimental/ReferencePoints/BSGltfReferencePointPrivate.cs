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
using UnityEngine;
using System;

namespace BIMservercenter.Toolkit.Experimental.Schema
{
    [Serializable]
    public class BSGltfReferencePointCoordsJSON
    {
        public string guid;
        public string description;
        public double x, y, z;
        public string img;

        public BSGltfReferencePointCoordsJSON(
                        string guid,
                        string description,
                        double x, double y, double z,
                        string imagen)
        {
            this.guid = guid;
            this.description = description;
            this.x = x;
            this.y = y;
            this.z = z;
            this.img = imagen;
        }
    }

    [Serializable]
    public class BSGltfReferencePointJSON
    {
        public string architectural_model_glTF_file;
        public string markers_gltf_file;
        public BSGltfReferencePointCoordsJSON[] markers;

        public BSGltfReferencePointJSON(
                        string architectural_model_glTF_file,
                        string markers_gltf_file,
                        BSGltfReferencePointCoordsJSON[] markers)
        {
            this.architectural_model_glTF_file = architectural_model_glTF_file;
            this.markers_gltf_file = markers_gltf_file;
            this.markers = markers;
        }
    }

    public class BSGltfReferencePointCoords
    {
        public string GUID;
        public string description;
        public Vector3 position;
        public string imageRef_Opc;

        public BSGltfReferencePointCoords(
                        string GUID,
                        string description,
                        Vector3 position,
                        string imageRef_Opc)
        {
            this.GUID = GUID;
            this.description = description;
            this.position = position;
            this.imageRef_Opc = imageRef_Opc;
        }

        // ---------------------------------------------------------------------------

        public BSGltfReferencePointCoords(BSGltfReferencePointCoordsJSON bSGltfReferencePointCoordsJSON)
        {
            this.GUID = bSGltfReferencePointCoordsJSON.guid;
            this.description = bSGltfReferencePointCoordsJSON.description;
            this.position = new Vector3(Convert.ToSingle(bSGltfReferencePointCoordsJSON.x), Convert.ToSingle(bSGltfReferencePointCoordsJSON.y), Convert.ToSingle(bSGltfReferencePointCoordsJSON.z));
            this.imageRef_Opc = bSGltfReferencePointCoordsJSON.img;
        }
    }
    
    public partial class BSGltfReferencePoints
    {
        private string architectural_model_glTF_file;
        private string markers_gltf_file;
        private List<BSGltfReferencePointCoords> markers;

        public BSGltfReferencePoints(
                        string architectural_model_glTF_file,
                        string markers_gltf_file,
                        List<BSGltfReferencePointCoords> markers)
        {
            this.architectural_model_glTF_file = architectural_model_glTF_file;
            this.markers_gltf_file = markers_gltf_file;
            this.markers = markers;
        }

        // ---------------------------------------------------------------------------

        public BSGltfReferencePoints(string JSONstring)
        {
            BSGltfReferencePointJSON bSGltfReferencePointJSON;

            bSGltfReferencePointJSON = JsonUtility.FromJson<BSGltfReferencePointJSON>(JSONstring);

            this.architectural_model_glTF_file = bSGltfReferencePointJSON.architectural_model_glTF_file;
            this.markers_gltf_file = bSGltfReferencePointJSON.markers_gltf_file;
            this.markers = new List<BSGltfReferencePointCoords>();

            for(int i = 0; i < bSGltfReferencePointJSON.markers.Length; i++)
            {
                BSGltfReferencePointCoords bSGltfReferencePointCoords;

                bSGltfReferencePointCoords = new BSGltfReferencePointCoords(bSGltfReferencePointJSON.markers[i]);

                BIMServerCenterAssertion.AssertNotNull(bSGltfReferencePointCoords);

                this.markers.Add(bSGltfReferencePointCoords);
            }
        }
    }
}
