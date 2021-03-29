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

using BIMservercenter.Toolkit.Internal.Gltf.Serialization;
using BIMservercenter.Toolkit.Internal.Gltf.Schema;
using BIMservercenter.Toolkit.Internal.Utilities;
using BIMservercenter.Toolkit.Public.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using System.IO;

namespace BIMservercenter.Toolkit.Public.Model
{
    public partial class BSGltf
    {
        private GltfObject glTF;
        private GameObject glTFGameObject { get { return glTF?.GameObjectReference; } }
        private List<Collider> glTFColliders { get { return glTF?.RegisteredColliders; } }
        private List<Renderer> glTFRenderers { get { return glTF?.RegisteredRenderers; } }
        private List<Animation> glTFAnimations { get { return glTF?.RegisteredAnimations; } }
        private List<BSGltfExtras> glTFExtras { get { return glTF?.RegisteredExtras; } }

        // ---------------------------------------------------------------------------
        // Disk
        // ---------------------------------------------------------------------------

        private async Task PLoadGltfFromDiskAsync(
                        string glTFPath,
                        bool generateColliders,
                        bool hide,
                        FuncProgressPercUpdate funcProgressPercUpdate,
                        CancellationTokenSource cancellationTokenSource)
        {
            CancellationTokenSource currentTokenSource;

            currentTokenSource = cancellationTokenSource ?? new CancellationTokenSource();

            BIMServerCenterAssertion.AssertEquals(File.Exists(glTFPath), true);
            this.glTF = await GltfUtility.ImportGltfObjectFromPathAsync(glTFPath, generateColliders, currentTokenSource.Token, funcProgressPercUpdate);

            if (this.glTF != null && this.glTF.GameObjectReference != null)
                this.glTF.GameObjectReference.SetActive(!hide);

            if (cancellationTokenSource == null)
                currentTokenSource.Dispose();
        }
    }
}
