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

namespace BIMservercenter.Toolkit.Internal.Gltf.AsyncAwaitUtil.Internal
{
    public class AsyncCoroutineRunner : MonoBehaviour
    {
        static AsyncCoroutineRunner _instance;

        public static AsyncCoroutineRunner Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("AsyncCoroutineRunner")
                        .AddComponent<AsyncCoroutineRunner>();
                }

                return _instance;
            }
        }

        void Awake()
        {
            // Don't show in scene hierarchy
            gameObject.hideFlags = HideFlags.HideAndDontSave;

            DontDestroyOnLoad(gameObject);
        }
    }
}
