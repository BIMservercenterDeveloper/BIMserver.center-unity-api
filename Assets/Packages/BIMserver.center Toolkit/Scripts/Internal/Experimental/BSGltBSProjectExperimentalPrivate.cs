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

using BIMservercenter.Toolkit.Experimental.Schema;

namespace BIMservercenter.Toolkit.Public.Model.Experimental
{
    public static partial class BSProjectExperimental
    {
        private static BSGltfReferencePoints glTFReferencePoints
        {
            get
            {
                string jsonString;
                BSGltfReferencePoints bSGltfReferencePoints;

                jsonString = "{\"architectural_model_glTF_file\": \"Modelo arquitect�nico.gltf\",\"markers_gltf_file\": \"Mobiliario.gltf\",\"markers\": [{\"guid\": \"AR Automatic GUID-Mobiliario\",\"descripcion\": \"AR Automatic GUID-Mobiliario\",\"x\": -7.9,\"y\": 0,\"z\": -1.2,\"img\": \"ref Imagen\"},{\"guid\": \"AR Automatic GUID-MobiliarioOffice\",\"descripcion\": \"AR Automatic GUID-MobiliarioOffice\",\"x\": 8.3,\"y\": 3.1,\"z\": 8.1,\"img\": \"ref Imagen\"}]}";

                bSGltfReferencePoints = new BSGltfReferencePoints(jsonString);

                return bSGltfReferencePoints;
            }
        }
    }
}
