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

using BIMservercenter.Toolkit.Internal.API.Response.Schema;
using BIMservercenter.Toolkit.Internal.API.Utilities;
using BIMservercenter.Toolkit.Internal.API.Session;
using BIMservercenter.Toolkit.Internal.API.Model;
using System.Collections.Generic;
using System.Xml;

namespace BIMservercenter.Toolkit.Internal.API.Response
{
    public class BSDocumentListResponse : BSBaseResponse
    {
        public override void OnSucced(string response, BSSession session, BSSessionDelegate sessionDelegate, object extras)
        {
            XmlDocument xmlDocument = new XmlDocument();
            BSDocumentListSchema schema;

            xmlDocument.LoadXml(response);

            schema = BSXmlParser<BSDocumentListSchema>.Parse(xmlDocument.DocumentElement.InnerXml);

            {
                List<BSMDocument> documentList;

                documentList = new List<BSMDocument>();

                foreach (var item in schema.list)
                {
                    BSMDocument document;

                    document = new BSMDocument();

                    document.bimserverId = item.id;
                    document.dateTimedate = item.fecha_ultimo_cambio;
                    document.idOwner = item.id_propietario;
                    document.imgOwnerOpc = item.imagen_propietario;
                    document.name = item.nombre;
                    document.nameOwnerOpc = item.nombre_propietario;
                    document.numAssociatedDocumentsAvailable = item.numero_ficheros_asociados;
                    document.sizeBytes = item.tamanyo;
                    document.version = item.version;
                    document.url = item.url_file;
                    document.img = item.img_file;
                    document.isCreatedInitialProgram = item.is_created_initial_program;

                    document.associatedDocumentsVisiblesList = new List<BSMAssociatedDocument>();

                    foreach (var aDocument in item.list)
                    {
                        BSMAssociatedDocument associatedDocument = new BSMAssociatedDocument();

                        associatedDocument.dateTimedate = aDocument.fecha_ultimo_cambio;
                        associatedDocument.name = aDocument.nombre;
                        associatedDocument.url = aDocument.url_file;

                        document.associatedDocumentsVisiblesList.Add(associatedDocument);
                    }

                    documentList.Add(document);
                }

                sessionDelegate.funcFinishDocumentListRequest(documentList);
            }
        }
    }
}