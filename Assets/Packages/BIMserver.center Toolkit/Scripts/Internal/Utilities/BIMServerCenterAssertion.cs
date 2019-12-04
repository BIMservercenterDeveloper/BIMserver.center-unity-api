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

namespace BIMservercenter.Toolkit.Internal.Utilities
{
    public class BIMServerCenterAssertion
    {
        private static void ThrowException(string message, bool stopEditor)
        {
#if UNITY_EDITOR
            if (stopEditor == true)
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            throw new System.Exception(message);
        }

        public static void AssertNull(object objectToAssert, bool stopEditor = true)
        { if (objectToAssert != null) ThrowException("Object should be null", stopEditor); }

        public static void AssertNotNull(object objectToAssert, bool stopEditor = true)
        { if (objectToAssert == null) ThrowException("Object shouldn't be null", stopEditor); }

        public static void AssertEquals<T>(T objectToAssert, T value, bool stopEditor = true)
        { if (objectToAssert.Equals(value) == false) ThrowException("Object should be equal to value", stopEditor); }

        public static void AssertNotEquals<T>(T objectToAssert, T value, bool stopEditor = true)
        { if (objectToAssert.Equals(value) == true) ThrowException("Object shouldn't be equal to value", stopEditor); }

        public static void AssertSmaller(int objectToAssert, int value, bool stopEditor = true)
        { if (objectToAssert > value) ThrowException("Object should be smaller than the value", stopEditor); }

        public static void AssertGreater(int objectToAssert, int value, bool stopEditor = true)
        { if (objectToAssert < value) ThrowException("Object should be greater than the value", stopEditor); }
    }
}