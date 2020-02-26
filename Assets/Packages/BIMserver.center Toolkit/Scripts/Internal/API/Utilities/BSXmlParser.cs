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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace BIMservercenter.Toolkit.Internal.API.Utilities
{
    public class BSXmlParser<T>
    {
        private static IList ParseList(string data, Type type)
        {
            Type makeGeneric;
            IList list;
            XmlDocument xmlDocument;
            int numItems;
            List<XmlNode> itemList;

            makeGeneric = typeof(List<>).MakeGenericType(type);
            list = (IList)Activator.CreateInstance(makeGeneric);
            xmlDocument = new XmlDocument();

            xmlDocument.LoadXml(data);

            {
                string numItemsString;

                numItemsString = BSXml.SearchNodeByName("count", xmlDocument.DocumentElement.ChildNodes).InnerText;

                numItems = int.Parse(numItemsString);
            }

            itemList = BSXml.SearchNodesByName("item", xmlDocument.DocumentElement.ChildNodes);

            if (itemList.Count != numItems)
                return list;

            if (type == typeof(string))
            {
                for (int i = 0; i < numItems; i++)
                {
                    object result;
                    result = itemList[i].InnerText;
                    list.Add(result);
                }
            }
            else if (type.IsPrimitive == true)
            {
                for (int i = 0; i < numItems; i++)
                {
                    object result;
                    result = Convert.ChangeType(itemList[i].InnerText, type);

                    list.Add(result);
                }
            }
            else
            {
                Type bsXmlSerializationType;
                ConstructorInfo constructorInfo;
                MethodInfo methodInfo;
                object bsXmlSerialization;

                bsXmlSerializationType = typeof(BSXmlParser<>).MakeGenericType(type);
                constructorInfo = bsXmlSerializationType.GetConstructor(Type.EmptyTypes);
                methodInfo = bsXmlSerializationType.GetMethod("Parse");
                bsXmlSerialization = constructorInfo.Invoke(new object[] { });

                for (int i = 0; i < numItems; i++)
                {
                    object result;
                    result = methodInfo.Invoke(bsXmlSerialization, new object[] { itemList[i].OuterXml });

                    list.Add(result);
                }
            }
            return list;
        }

        public static T Parse(string data)
        {
            T instance;
            Type instanceType;
            XmlDocument xmlDocument;
            int numElements;

            instance = Activator.CreateInstance<T>();
            instanceType = instance.GetType();
            xmlDocument = new XmlDocument();

            xmlDocument.LoadXml(data);
            numElements = xmlDocument.DocumentElement.ChildNodes.Count;

            for (int i = 0; i < numElements; i++)
            {
                XmlNode xmlNode;
                PropertyInfo propertyInfo;

                xmlNode = xmlDocument.DocumentElement.ChildNodes[i];
                propertyInfo = instanceType.GetProperty(xmlNode.Name);

                if (propertyInfo == null)
                    continue;

                if (propertyInfo.PropertyType.IsGenericType == false)
                {
                    if (propertyInfo.PropertyType == typeof(string))
                    {
                        propertyInfo.SetValue(instance, xmlNode.InnerText);
                    }
                    else if (propertyInfo.PropertyType.IsPrimitive)
                    {
                        object converted;
                        converted = Convert.ChangeType(xmlNode.InnerText, propertyInfo.PropertyType);

                        propertyInfo.SetValue(instance, converted);
                    }
                    else
                    {
                        Type bsXmlSerializationType;
                        ConstructorInfo constructorInfo;
                        MethodInfo methodInfo;
                        object bsXmlSerialization;
                        object result;

                        bsXmlSerializationType = typeof(BSXmlParser<>).MakeGenericType(propertyInfo.GetType());
                        constructorInfo = bsXmlSerializationType.GetConstructor(Type.EmptyTypes);
                        methodInfo = bsXmlSerializationType.GetMethod("Parse");
                        bsXmlSerialization = constructorInfo.Invoke(new object[] { });

                        result = methodInfo.Invoke(bsXmlSerialization, new object[] { xmlNode.OuterXml });

                        propertyInfo.SetValue(instance, result);
                    }
                }
                else
                {
                    Type propertyType;
                    IList parsedList;

                    propertyType = propertyInfo.PropertyType.GetGenericArguments()[0];
                    parsedList = ParseList(xmlNode.OuterXml, propertyType);

                    propertyInfo.SetValue(instance, parsedList);
                }
            }

            return instance;
        }
    }
}