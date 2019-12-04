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
    /// The name of the node's TRS property to modify, or the weights of the Morph Target it istantiates.
    /// For the translation property, the values that are provided by the sampler are the translation along the x, y, and z axes.
    /// For the rotation property, the values are a quaternion in the order (x, y, z, w), where w is the scalar.
    /// For the scale property, the values are the scaling factors along the x, y, and z axes.
    /// https://github.com/KhronosGroup/glTF/blob/master/specification/2.0/schema/animation.channel.target.schema.json
    /// </summary>
    public enum GltfAnimationChannelPath
    {
        translation,
        rotation,
        scale,
        weights
    }
}