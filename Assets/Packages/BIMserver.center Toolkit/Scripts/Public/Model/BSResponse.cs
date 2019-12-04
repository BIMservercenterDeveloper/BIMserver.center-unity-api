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

using System.Collections.Generic;

namespace BIMservercenter.Toolkit.Public.Model
{
    /// <summary>
    /// Enum to identify the type of error when BIMserver.center responses with an error.
    /// </summary>
    public enum BSResponseTypeError
    {
        Text,
        InvalidSession,
        ServerConnection,
        Unknown
    }

    /// <summary>
    /// Class to manage simple responses from BIMserver.center.
    /// </summary>
    public partial class BSResponse
    {
        /// <summary>
        /// Call this property to know if the response is successful.
        /// </summary>
        public bool IsSucced;

        /// <summary>
        /// If the response is not successful, this property stores the error message.
        /// </summary>
        public string ErrorMessage;

        /// <summary>
        /// If the response is not successful, this property stores the type of error.
        /// </summary>
        public BSResponseTypeError ErrorType;
    }

    /// <summary>
    /// Class to manage responses with list elements from BIMserver.center.
    /// </summary>
    public partial class BSResponseList<T> : BSResponse
    {
        /// <summary>
        /// Call this property to get the list of elements response if the response is succed.
        /// </summary>
        public List<T> ListElements;
    }
}
