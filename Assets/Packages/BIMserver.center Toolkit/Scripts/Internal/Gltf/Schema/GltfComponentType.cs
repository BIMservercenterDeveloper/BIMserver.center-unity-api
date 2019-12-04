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
    /// <summary>
    /// https://github.com/KhronosGroup/glTF/blob/master/specification/2.0/schema/accessor.schema.json:componentType <para/>
    /// The datatype of components in the attribute.  All valid values correspond to WebGL enums.
    /// The corresponding typed arrays are 'Int8Array', 'Uint8Array', 'Int16Array', 'Uint16Array', 'Uint32Array', and 'Float32Array', respectively.
    /// 5125 (UNSIGNED_INT) is only allowed when the accessor contains indices, i.e., the accessor is only referenced by 'primitive.indices'.
    /// </summary>
    public enum GltfComponentType
    {
        Byte = 5120,
        UnsignedByte = 5121,
        Short = 5122,
        UnsignedShort = 5123,
        UnsignedInt = 5125,
        Float = 5126
    }
}