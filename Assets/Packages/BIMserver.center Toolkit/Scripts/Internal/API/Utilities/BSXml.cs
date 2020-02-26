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
using System.Xml;

namespace BIMservercenter.Toolkit.Internal.API.Utilities
{
    public static class BSXml
    {
        public static XmlNode CreateNodeElement(XmlDocument document, string name)
        {
            return document.CreateNode(XmlNodeType.Element, name, null);
        }

        public static void CreateElement(XmlDocument document, XmlNode xmlParentNode, string name, string value)
        {
            XmlElement element;

            element = document.CreateElement(name);
            element.InnerText = value;

            xmlParentNode.AppendChild(element);
        }

        public static XmlNode SearchNodeByName(string name, XmlNodeList xmlNodeList)
        {
            int numElements;

            numElements = xmlNodeList.Count;

            for (int i = 0; i < numElements; i++)
            {
                if (xmlNodeList[i].Name == name)
                    return xmlNodeList[i];
            }

            return null;
        }

        public static List<XmlNode> SearchNodesByName(string name, XmlNodeList xmlNodeList)
        {
            List<XmlNode> xmlNodes;
            int numElements;

            xmlNodes = new List<XmlNode>();
            numElements = xmlNodeList.Count;

            for (int i = 0; i < numElements; i++)
            {
                if (xmlNodeList[i].Name == name)
                    xmlNodes.Add(xmlNodeList[i]);
            }

            return xmlNodes;
        }
    }
}
