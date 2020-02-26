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

using BIMservercenter.Toolkit.Internal.API.Model;
using BIMservercenter.Toolkit.Internal.API.Response.Schema;
using BIMservercenter.Toolkit.Internal.API.Session;
using BIMservercenter.Toolkit.Internal.API.Utilities;
using System.Collections.Generic;
using System.Xml;

namespace BIMservercenter.Toolkit.Internal.API.Response
{
    public class BSProjectEducationalWith3DListResponse : BSBaseResponse
    {
        public override void OnSucced(string response, BSSession session, BSSessionDelegate sessionDelegate, object extras)
        {
            XmlDocument xmlDocument;
            BSProjectEducationalWith3DListSchema schema;

            xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(response);

            schema = BSXmlParser<BSProjectEducationalWith3DListSchema>.Parse(xmlDocument.DocumentElement.InnerXml);

            {
                List<BSMProject> projectList;

                projectList = new List<BSMProject>();

                foreach (var item in schema.list)
                {
                    BSMProject project = new BSMProject();

                    project.bimServerId = item.id;
                    project.city = item.city;
                    project.collaborationRequestType = item.collaboration_request_project;
                    project.countryType = item.country;
                    project.dateCreate = item.create_date;
                    project.dateLastChange = item.fecha_ultimo_cambio;
                    project.description = item.description_project;
                    project.idOwner = item.id_owner;
                    project.img = item.img;
                    project.imgSmall = item.img_small;
                    project.imgLarge = item.img_large;
                    project.imgLandscape = item.img_landscape;
                    project.imgOwnerOpc = item.img_owner;
                    project.languageType = item.language;
                    project.name = item.nombre;
                    project.nameOwner = item.name_owner;
                    project.numFiles = item.num_filess;
                    project.numFilesIFC = item.num_files_ifc;
                    project.numMembersTeam = item.num_members_team;
                    project.privacyType = item.privacity_project;
                    project.rolType = item.rol;
                    project.sizeBytes = item.size_project;
                    project.typesProjectToEdit = item.type_project_edit;
                    project.typeType = item.type_project;

                    projectList.Add(project);
                }

                sessionDelegate.funcFinishProjectEducationalWith3DListRequest(projectList);
            }
        }
    }
}