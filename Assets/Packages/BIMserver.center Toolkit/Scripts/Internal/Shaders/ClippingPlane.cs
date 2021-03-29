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

using UnityEngine;

namespace BIMservercenter.Toolkit.Internal.Shaders
{
    [ExecuteInEditMode]
    public class ClippingPlane : ClippingPrimitive
    {
        private int clipPlaneID;

        protected override string Keyword
        {
            get { return "_CLIPPING_PLANE"; }
        }

        protected override string ClippingSideProperty
        {
            get { return "_ClipPlaneSide"; }
        }

        private void OnDrawGizmosSelected()
        {
            if (enabled)
            {
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawWireCube(Vector3.zero, new Vector3(1.0f, 0.0f, 1.0f));
                Gizmos.DrawLine(Vector3.zero, Vector3.up * -0.5f);
            }
        }

        protected override void Initialize()
        {
            base.Initialize();

            clipPlaneID = Shader.PropertyToID("_ClipPlane");
        }

        protected override void UpdateShaderProperties(MaterialPropertyBlock materialPropertyBlock)
        {
            Vector3 up = transform.up;
            Vector4 plane = new Vector4(up.x, up.y, up.z, Vector3.Dot(up, transform.position));

            materialPropertyBlock.SetVector(clipPlaneID, plane);
        }
    }
}