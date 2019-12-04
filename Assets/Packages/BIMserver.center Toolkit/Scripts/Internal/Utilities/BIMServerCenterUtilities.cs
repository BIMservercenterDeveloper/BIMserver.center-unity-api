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
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace BIMservercenter.Toolkit.Internal.Utilities
{
    internal static class BIMServerCenterUtilities
    {
        // ---------------------------------------------------------------------------
        // URI
        // ---------------------------------------------------------------------------

        public static string ReplaceHTTPSUrlToHTTP(string url)
        {
            if (url.Length > 5 && url.Substring(0, 5).Equals("https"))
                return url.Replace("https", "http");

            return url;
        }

        // ---------------------------------------------------------------------------
        // Texture
        // ---------------------------------------------------------------------------

        public static Texture2D LoadTexture(string pathTexture)
        {
            Texture2D texture;

            using (BinaryReader binaryReader = new BinaryReader(File.Open(pathTexture, FileMode.Open)))
            {
                texture = new Texture2D(1, 1);
                byte[] data = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
                texture.LoadImage(data);
            }

            return texture;
        }

        // ---------------------------------------------------------------------------

        public static async Task<bool> DownloadAsync(string url, string path)
        {
            UnityWebRequest unityWebRequest;

            unityWebRequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();

            await unityWebRequest.SendWebRequest();
            if (unityWebRequest.isHttpError == false && unityWebRequest.isNetworkError == false)
            {
                await BIMServerCenterUtilities.WriteDataAsync(unityWebRequest.downloadHandler.data, path);
                return true;
            }

            return false;
        }

        // ---------------------------------------------------------------------------

        public static async Task<Texture2D> DownloadTextureAsync(string urlTexture, Texture2D defaultTexture = null)
        {
            string urlTextureHTTP;
            UnityWebRequest unityWebRequest;
            DownloadHandlerTexture downloadHandlerTexture;

            urlTextureHTTP = ReplaceHTTPSUrlToHTTP(urlTexture);
            unityWebRequest = new UnityWebRequest(urlTextureHTTP, UnityWebRequest.kHttpVerbGET);
            downloadHandlerTexture = new DownloadHandlerTexture();
            unityWebRequest.downloadHandler = downloadHandlerTexture;

            await unityWebRequest.SendWebRequest();
            if (unityWebRequest.isHttpError == false && unityWebRequest.isNetworkError == false && downloadHandlerTexture.isDone == true)
                return downloadHandlerTexture.texture;

            if (defaultTexture != null)
                return defaultTexture;
            else
                return Texture2D.blackTexture;
        }

        // ---------------------------------------------------------------------------
        // Write Data Disk
        // ---------------------------------------------------------------------------

        public static void RemoveCompleteDirectoryFromDiskIfExist(string path)
        {
            if (Directory.Exists(path))
            {
                string[] directories, files;

                directories = Directory.GetDirectories(path);
                files = Directory.GetFiles(path);

                for (int i = 0; i < directories.Length; i++)
                {
                    RemoveCompleteDirectoryFromDiskIfExist(directories[i]);
                }

                for (int i = 0; i < files.Length; i++)
                {
                    File.Delete(files[i]);
                }

                Directory.Delete(path);
            }
        }

        // ---------------------------------------------------------------------------

        private static async Task<MemoryStream> DecompressDataAsync(byte[] data)
        {
            MemoryStream decompressedData;
            decompressedData = new MemoryStream();

            using (MemoryStream compressedData = new MemoryStream(data))
            {
                compressedData.Seek(2, SeekOrigin.Begin);

                using (DeflateStream zOut = new DeflateStream(compressedData, CompressionMode.Decompress))
                {
                    await zOut.CopyToAsync(decompressedData);
                }
            }

            return decompressedData;
        }

        // ---------------------------------------------------------------------------

        public static async Task WriteDataAsync(byte[] data, string path, bool isCompressed = true)
        {
            MemoryStream decompressedData;

            if (isCompressed == true)
                decompressedData = await DecompressDataAsync(data);
            else
                decompressedData = new MemoryStream(data);

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                decompressedData.Seek(0, SeekOrigin.Begin);
                await decompressedData.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }
        }

        // ---------------------------------------------------------------------------
        // Json
        // ---------------------------------------------------------------------------

        public static void SaveJsonOnDisk<T>(string path, T serializableObject)
        {
            string json;

            json = JsonUtility.ToJson(serializableObject, true);

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.Write(json);
                    writer.Close();
                    writer.Dispose();
                }
                fileStream.Close();
                fileStream.Dispose();
            }
        }

        // ---------------------------------------------------------------------------

        public static bool LoadJsonFromDisk<T>(string path, ref T serializableObject)
        {
            if (File.Exists(path) == true)
            {
                string json;

                using (FileStream fileStream = new FileStream(path, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        json = reader.ReadToEnd();
                        reader.Close();
                        reader.Dispose();
                    }
                    fileStream.Close();
                    fileStream.Dispose();
                }

                serializableObject = JsonUtility.FromJson<T>(json);

                return true;
            }

            return false;
        }
    }
}