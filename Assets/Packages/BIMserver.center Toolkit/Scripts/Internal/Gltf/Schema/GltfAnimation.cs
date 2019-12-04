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

using System;
using UnityEngine;

namespace BIMservercenter.Toolkit.Internal.Gltf.Schema
{
    /// <summary>
    /// A keyframe animation.
    /// https://github.com/KhronosGroup/glTF/blob/master/specification/2.0/schema/animation.schema.json
    /// </summary>
    [Serializable]
    public class GltfAnimation : GltfChildOfRootProperty
    {
        /// <summary>
        /// An array of channels, each of which targets an animation's sampler at a
        /// node's property. Different channels of the same animation can't have equal
        /// targets.
        /// </summary>
        public GltfAnimationChannel[] channels;

        /// <summary>
        /// An array of samplers that combines input and output accessors with an
        /// interpolation algorithm to define a keyframe graph (but not its target).
        /// </summary>
        public GltfAnimationSampler[] samplers;

        /// <summary>
        /// Unity Animation wrapper for the GltfAnimation
        /// </summary>
        public AnimationClip animation { get; internal set; }
    }
}