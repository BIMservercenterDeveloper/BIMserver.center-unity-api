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

namespace BIMservercenter.Toolkit.Public.Utilities
{
    /// <summary>
    /// This exception is thrown when there is a problem parsing a glTF file.
    /// </summary>
    public class BSExceptionFailedToReadGLTF : Exception
    {
        private const string errMsg = "The file cannot be parsed";

        public BSExceptionFailedToReadGLTF() : base(errMsg) { }
    }

    /// <summary>
    /// This exception is thrown when a connection can't be established to the server.
    /// </summary>
    public class BSExceptionServerDownloadError : Exception
    {
        private const string errMsg = "Could not download file (Server error)";

        public BSExceptionServerDownloadError() : base(errMsg) { }
    }

    /// <summary>
    /// This exception is thrown when a connection can't be established locally.
    /// </summary>
    public class BSExceptionNetworkDownloadError : Exception
    {
        private const string errMsg = "Could not download file (Network error)";

        public BSExceptionNetworkDownloadError() : base(errMsg) { }
    }

    /// <summary>
    /// This exception is thrown when a task is cancelled.
    /// </summary>
    public class BSExceptionCancellationRequested : Exception
    {
        private const string errMsg = "The operation was canceled";

        public BSExceptionCancellationRequested() : base(errMsg) { }
    }
}
