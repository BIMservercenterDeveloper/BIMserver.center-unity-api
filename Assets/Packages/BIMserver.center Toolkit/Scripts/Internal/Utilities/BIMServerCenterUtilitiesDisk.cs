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
using System.IO;

namespace BIMservercenter.Toolkit.Internal.Utilities
{
    internal static class BIMServerCenterUtilitiesDisk
    {
        // ---------------------------------------------------------------------------
        // Path Files
        // ---------------------------------------------------------------------------

        private static string JsonFilePath(string directoryPath, string fileName)
        {
            return Path.Combine(directoryPath, fileName);
        }

        // ---------------------------------------------------------------------------
        // Directories
        // ---------------------------------------------------------------------------

        private static void CreateDirectoryIfNoExists(string directoryPath)
        {
            if (Directory.Exists(directoryPath) == false)
                Directory.CreateDirectory(directoryPath);
        }

        // ---------------------------------------------------------------------------
        // Files
        // ---------------------------------------------------------------------------

        private static bool RemoveFileIfExists(string filePath)
        {
            if (File.Exists(filePath) == true)
            {
                File.Delete(filePath);
                return true;
            }

            return false;
        }

        // ---------------------------------------------------------------------------
        // Json Files
        // ---------------------------------------------------------------------------

        public static bool RemoveJsonFile(string directoryPath, string fileName)
        {
            string jsonFilePath;

            jsonFilePath = JsonFilePath(directoryPath, fileName);
            return RemoveFileIfExists(jsonFilePath);
        }

        // ---------------------------------------------------------------------------

        public static void CreateJsonFile<T>(string directoryPath, string fileName, T serializableObject)
        {
            string jsonFilePath, jsonFileData;

            CreateDirectoryIfNoExists(directoryPath);

            jsonFilePath = JsonFilePath(directoryPath, fileName);
            jsonFileData = JsonUtility.ToJson(serializableObject, true);

            File.WriteAllText(jsonFilePath, jsonFileData);
        }

        // ---------------------------------------------------------------------------

        public static T ReadFromJsonFile<T>(string directoryPath, string fileName)
        {
            string jsonFilePath;

            jsonFilePath = JsonFilePath(directoryPath, fileName);

            if (File.Exists(jsonFilePath) == true)
            {
                string jsonFileData;

                jsonFileData = File.ReadAllText(jsonFilePath);
                return JsonUtility.FromJson<T>(jsonFileData);
            }

            return default(T);
        }
    }
}

  