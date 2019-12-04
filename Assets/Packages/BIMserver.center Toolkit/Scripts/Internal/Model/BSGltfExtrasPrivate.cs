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
using UnityEngine;

namespace BIMservercenter.Toolkit.Public.Model
{
    // ---------------------------------------------------------------------------
    // TextLine
    // ---------------------------------------------------------------------------

    [System.Serializable]
    public class TextLine
    {
        public string GroupText;
        public string Text;

        public TextLine(string groupText, string text)
        {
            GroupText = groupText;
            Text = text;
        }
    }

    // ---------------------------------------------------------------------------
    // KeyValue
    // ---------------------------------------------------------------------------

    [System.Serializable]
    public class KeyValue
    {
        public string Key;
        public string Value;

        public KeyValue(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }

    // ---------------------------------------------------------------------------
    // Extras
    // ---------------------------------------------------------------------------

    public partial class BSGltfExtras
    {
        private GameObject gameObject;

        private int id;
        private string guid;
        private string name;
        private List<TextLine> textLines = new List<TextLine>();
        private List<KeyValue> keyValues = new List<KeyValue>();

        // ---------------------------------------------------------------------------
        // Descriptions
        // ---------------------------------------------------------------------------

        private string PGetGltfExtrasDescription()
        {
            string description;

            description = $"<b>{name}</b>\n";

            foreach (TextLine textLine in textLines)
            {
                if (string.IsNullOrEmpty(textLine.GroupText) == false)
                {
                    string formattedText;

                    formattedText = $"<u>{textLine.GroupText}</u>";
                    description = $"{description}{formattedText}\n";
                }
                else if (string.IsNullOrEmpty(textLine.Text) == false)
                {
                    string formattedText;

                    formattedText = $"-<indent=1em>{textLine.Text}</indent>";
                    description = $"{description}{formattedText}\n";
                }
            }

            foreach (KeyValue keyValue in keyValues)
            {
                string formattedText;

                formattedText = $"-<indent=1em>{keyValue.Key}: {keyValue.Value}</indent>";
                description = $"{description}{formattedText}\n";
            }

            return description;
        }
    }
}