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

namespace BIMservercenter.Toolkit.Internal.Gltf.Schema
{
    [System.Serializable]
    public class GltfNodeExtras
    {
        public int bsvdescidx = -1;
    }

    [System.Serializable]
    public class Textline
    {
        public string text;
    }

    [System.Serializable]
    public class Keyvalue
    {
        public string name;
        public string value;
    }

    [System.Serializable]
    public class GroupTitle
    {
        public string text;
    }

    [System.Serializable]
    public class Bsvgenericattr
    {
        public enum CodingKeys { textline, keyvalue, grouptitle }

        public CodingKeys type;
        public Textline textline;
        public Keyvalue keyvalue;
        public GroupTitle grouptitle;
    }

    [System.Serializable]
    public class Bsvbimdescription
    {
        public string bsvguid;
        public string bsvname;
        public Bsvgenericattr[] bsvgenericattrs;
    }

    [System.Serializable]
    public class GltfExtras : GltfChildOfRootProperty
    {
        public Bsvbimdescription[] bsvbimdescriptions;
    }
}