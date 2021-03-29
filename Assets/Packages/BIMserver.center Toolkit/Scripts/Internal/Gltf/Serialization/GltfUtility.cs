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

using BIMservercenter.Toolkit.Internal.Gltf.AsyncAwaitUtil;
using BIMservercenter.Toolkit.Internal.Gltf.Schema;
using BIMservercenter.Toolkit.Public.Utilities;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using UnityEngine;
using System.IO;
using System;

namespace BIMservercenter.Toolkit.Internal.Gltf.Serialization
{
    public static class GltfUtility
    {
        private const uint GltfMagicNumber = 0x46546C67;

        private static readonly WaitForUpdate Update = new WaitForUpdate();
        private static readonly WaitForBackgroundThread BackgroundThread = new WaitForBackgroundThread();
        private static readonly string DefaultObjectName = "GLTF Object";

        /// <summary>
        /// Imports a glTF object from the provided uri.
        /// </summary>
        /// <param name="uri">the path to the file to load</param>
        /// <returns>New <see cref="Schema.GltfObject"/> imported from uri.</returns>
        /// <remarks>
        /// Must be called from the main thread.
        /// If the <see href="https://docs.unity3d.com/ScriptReference/Application-isPlaying.html">Application.isPlaying</see> is false, then this method will run synchronously.
        /// </remarks>
        /// <exception cref="BSExceptionCancellationRequested">Thrown when cancellation had requested</exception>
        /// <exception cref="BSExceptionFailedToReadGLTF">Thrown when fail to read GLTF</exception>
        public static async Task<GltfObject> ImportGltfObjectFromPathAsync(string uri, bool generateColliders, CancellationToken cancellationToken, FuncProgressPercUpdate funcProgressPercUpdate)
        {
            GltfObject gltfObject;
            bool useBackgroundThread = Application.isPlaying;

            if (string.IsNullOrWhiteSpace(uri) || uri.EndsWith(".gltf") == false)
                throw new BSExceptionFailedToReadGLTF();

            if (useBackgroundThread) { await BackgroundThread; }

            {
                string gltfJson;

                using (FileStream stream = new FileStream(uri, FileMode.Open))
                using (StreamReader streamReader = new StreamReader(stream))
                using (StringWriter writter = new StringWriter())
                {
                    while (streamReader.EndOfStream == false)
                    {
                        writter.WriteLine(streamReader.ReadLine());
                    }

                    gltfJson = writter.ToString();
                }

                gltfObject = await GetGltfObjectFromJson(gltfJson, cancellationToken);

                if (gltfObject.asset.version.Contains("2.0") == false)
                    throw new BSExceptionFailedToReadGLTF();
            }

            gltfObject.Uri = uri;

            {
                int nameStart = uri.Replace("\\", "/").LastIndexOf("/", StringComparison.Ordinal) + 1;
                int nameLength = uri.Length - nameStart;

                gltfObject.Name = Path.GetFileNameWithoutExtension(uri.Substring(nameStart, nameLength));
            }

            gltfObject.UseBackgroundThread = useBackgroundThread;
            await gltfObject.ConstructAsync(generateColliders, cancellationToken, funcProgressPercUpdate);

            if (useBackgroundThread) { await Update; }

            return gltfObject;
        }

        /// <summary>
        /// Gets a glTF object from the provided json string.
        /// </summary>
        /// <param name="jsonString">String defining a glTF Object.</param>
        /// <returns><see cref="Schema.GltfObject"/></returns>
        /// <remarks>Returned <see cref="Schema.GltfObject"/> still needs to be initialized using <see cref="ConstructGltf.ConstructAsync"/>.</remarks>
        /// <exception cref="BSExceptionFailedToReadGLTF">Thrown when a GLTF file can't be parsed</exception>
        /// <exception cref="BSExceptionCancellationRequested">Thrown when cancellation is requested</exception>
        public static async Task<GltfObject> GetGltfObjectFromJson(string jsonString, CancellationToken cancellationToken)
        {
            GltfObject gltfObject = null; ;

            if (cancellationToken.IsCancellationRequested == true)
                throw new BSExceptionCancellationRequested();

            try
            {
                await Task.Run(() => { gltfObject = JsonUtility.FromJson<GltfObject>(jsonString); }, cancellationToken);
            }
            catch
            {
                throw new BSExceptionFailedToReadGLTF();
            }

            List<string> meshPrimitiveAttributes = null;

            if (cancellationToken.IsCancellationRequested == true)
                throw new BSExceptionCancellationRequested();

            await Task.Run(() => { meshPrimitiveAttributes = GetGltfMeshPrimitiveAttributes(jsonString); }, cancellationToken);

            var meshPrimitiveTargets = GetGltfMeshPrimitiveTargets(jsonString);
            int numPrimitives = 0;

            for (var i = 0; i < gltfObject.meshes?.Length; i++)
                numPrimitives += gltfObject.meshes[i]?.primitives?.Length ?? 0;

            int primitiveIndex = 0;

            for (int i = 0; i < gltfObject.meshes?.Length; i++)
            {
                for (int j = 0; j < gltfObject.meshes[i].primitives.Length; j++)
                {
                    var content = meshPrimitiveAttributes[primitiveIndex];
                    content = content.Substring(content.IndexOf('{'));

                    gltfObject.meshes[i].primitives[j].Attributes = JsonUtility.FromJson<GltfMeshPrimitiveAttributes>(content);
                    primitiveIndex++;
                }
            }

            primitiveIndex = 0;

            if (meshPrimitiveTargets.Count != 0)
            {
                for (int i = 0; i < gltfObject.meshes?.Length; i++)
                {
                    for (int j = 0; j < gltfObject.meshes[i].primitives.Length; j++)
                    {
                        var content = meshPrimitiveTargets[primitiveIndex] + ']' + '}';
                        content = "{\"Items\":" + content.Substring(content.IndexOf('['));

                        gltfObject.meshes[i].primitives[j].Targets = JsonHelper.FromJson<GltfMeshPrimitiveAttributes>(content);
                        primitiveIndex++;
                    }
                }
            }

            return gltfObject;
        }

        /// <summary>
        /// Gets a glTF object from the provided byte array
        /// </summary>
        /// <param name="glbData">Raw glb byte data.</param>
        /// <returns><see cref="Schema.GltfObject"/></returns>
        /// <remarks>Returned <see cref="Schema.GltfObject"/> still needs to be initialized using <see cref="ConstructGltf.ConstructAsync"/>.</remarks>
        public static async Task<GltfObject> GetGltfObjectFromGlb(byte[] glbData, CancellationToken cancellationToken)
        {
            const int stride = sizeof(uint);

            var magicNumber = BitConverter.ToUInt32(glbData, 0);
            var version = BitConverter.ToUInt32(glbData, stride);
            var length = BitConverter.ToUInt32(glbData, stride * 2);

            if (magicNumber != GltfMagicNumber)
            {
                Debug.LogError("File is not a glb object!");
                return null;
            }

            if (version != 2)
            {
                Debug.LogError("Glb file version mismatch! Glb must use version 2");
                return null;
            }

            if (length != glbData.Length)
            {
                Debug.LogError("Glb file size does not match the glb header defined size");
                return null;
            }

            var chunk0Length = (int)BitConverter.ToUInt32(glbData, stride * 3);
            var chunk0Type = BitConverter.ToUInt32(glbData, stride * 4);

            if (chunk0Type != (ulong)GltfChunkType.Json)
            {
                Debug.LogError("Expected chunk 0 to be Json data!");
                return null;
            }

            var jsonChunk = Encoding.ASCII.GetString(glbData, stride * 5, chunk0Length);
            var gltfObject = await GetGltfObjectFromJson(jsonChunk, cancellationToken);
            var chunk1Length = (int)BitConverter.ToUInt32(glbData, stride * 5 + chunk0Length);
            var chunk1Type = BitConverter.ToUInt32(glbData, stride * 6 + chunk0Length);

            if (chunk1Type != (ulong)GltfChunkType.BIN)
            {
                Debug.LogError("Expected chunk 1 to be BIN data!");
                return null;
            }

            Debug.Assert(gltfObject.buffers[0].byteLength == chunk1Length, "chunk 1 & buffer 0 length mismatch");

            gltfObject.buffers[0].BufferData = new byte[chunk1Length];
            Array.Copy(glbData, stride * 7 + chunk0Length, gltfObject.buffers[0].BufferData, 0, chunk1Length);

            return gltfObject;
        }

        /// <summary>
        /// Get a single Json Object using the handle provided.
        /// </summary>
        /// <param name="jsonString">The json string to search.</param>
        /// <param name="handle">The handle to look for.</param>
        /// <returns>A snippet of the json string that defines the object.</returns>
        private static string GetJsonObject(string jsonString, string handle)
        {
            var regex = new Regex($"\"{handle}\"\\s*:\\s*\\{{");
            var match = regex.Match(jsonString);
            return match.Success ? GetJsonObject(jsonString, match.Index + match.Length) : null;
        }

        private static List<string> GetGltfMeshPrimitiveAttributes(string jsonString)
        {
            var regex = new Regex("(?<Attributes>\"attributes\"[^}]+})");
            return GetGltfMeshPrimitiveAttributes(jsonString, regex);
        }

        private static List<string> GetGltfMeshPrimitiveAttributes(string jsonString, Regex regex)
        {
            var jsonObjects = new List<string>();

            if (!regex.IsMatch(jsonString))
            {
                return jsonObjects;
            }

            MatchCollection matches = regex.Matches(jsonString);

            for (var i = 0; i < matches.Count; i++)
            {
                jsonObjects.Add(matches[i].Groups["Attributes"].Captures[0].Value.Replace("\"attributes\":", string.Empty));
            }

            return jsonObjects;
        }

        private static List<string> GetGltfMeshPrimitiveTargets(string jsonString)
        {
            var regex = new Regex("(?<Targets>\"targets\"[^]]+})");
            return GetGltfMeshPrimitiveTargets(jsonString, regex);
        }

        private static List<string> GetGltfMeshPrimitiveTargets(string jsonString, Regex regex)
        {
            var jsonObjects = new List<string>();

            if (!regex.IsMatch(jsonString))
            {
                return jsonObjects;
            }

            MatchCollection matches = regex.Matches(jsonString);

            for (var i = 0; i < matches.Count; i++)
            {
                jsonObjects.Add(matches[i].Groups["Targets"].Captures[0].Value.Replace("\"targets\":", string.Empty));
            }

            return jsonObjects;
        }

        /// <summary>
        /// Get a collection of glTF Extensions using the handle provided.
        /// </summary>
        /// <param name="jsonString">The json string to search.</param>
        /// <param name="handle">The handle to look for.</param>
        /// <returns>A collection of snippets with the json string that defines the object.</returns>
        private static Dictionary<string, string> GetGltfExtensionObjects(string jsonString, string handle)
        {
            // Assumption: This code assumes that a name is declared before extensions in the glTF schema.
            // This may not work for all exporters. Some exporters may fail to adhere to the standard glTF schema.
            var regex = new Regex($"(\"name\":\\s*\"\\w*\",\\s*\"extensions\":\\s*{{\\s*?)(\"{handle}\"\\s*:\\s*{{)");
            return GetGltfExtensions(jsonString, regex);
        }

        /// <summary>
        /// Get a collection of glTF Extras using the handle provided.
        /// </summary>
        /// <param name="jsonString">The json string to search.</param>
        /// <param name="handle">The handle to look for.</param>
        /// <returns>A collection of snippets with the json string that defines the object.</returns>
        private static Dictionary<string, string> GetGltfExtraObjects(string jsonString, string handle)
        {
            // Assumption: This code assumes that a name is declared before extensions in the glTF schema.
            // This may not work for all exporters. Some exporters may fail to adhere to the standard glTF schema.
            var regex = new Regex($"(\"name\":\\s*\"\\w*\",\\s*\"extras\":\\s*{{\\s*?)(\"{handle}\"\\s*:\\s*{{)");
            return GetGltfExtensions(jsonString, regex);
        }

        private static Dictionary<string, string> GetGltfExtensions(string jsonString, Regex regex)
        {
            var jsonObjects = new Dictionary<string, string>();

            if (!regex.IsMatch(jsonString))
            {
                return jsonObjects;
            }

            var matches = regex.Matches(jsonString);
            var nodeName = string.Empty;

            for (var i = 0; i < matches.Count; i++)
            {
                for (int j = 0; j < matches[i].Groups.Count; j++)
                {
                    for (int k = 0; k < matches[i].Groups[i].Captures.Count; k++)
                    {
                        nodeName = GetGltfNodeName(matches[i].Groups[i].Captures[i].Value);
                    }
                }

                if (!jsonObjects.ContainsKey(nodeName))
                {
                    jsonObjects.Add(nodeName, GetJsonObject(jsonString, matches[i].Index + matches[i].Length));
                }
            }

            return jsonObjects;
        }

        private static string GetJsonObject(string jsonString, int startOfObject)
        {
            int index;
            int bracketCount = 1;

            for (index = startOfObject; bracketCount > 0; index++)
            {
                if (jsonString[index] == '{')
                {
                    bracketCount++;
                }
                else if (jsonString[index] == '}')
                {
                    bracketCount--;
                }
            }

            return $"{{{jsonString.Substring(startOfObject, index - startOfObject)}";
        }

        private static string GetGltfNodeName(string jsonString)
        {
            jsonString = jsonString.Replace("\"name\"", string.Empty);
            jsonString = jsonString.Replace(": \"", string.Empty);
            jsonString = jsonString.Replace(":\"", string.Empty);
            jsonString = jsonString.Substring(0, jsonString.IndexOf("\"", StringComparison.Ordinal));
            return jsonString;
        }
    }
}