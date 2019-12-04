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

namespace BIMservercenter.Toolkit.Internal.API.Response.Schema
{
    public class BSProjectItem
    {
        public BSProjectItem()
        {
            id = null;
            nombre = null;
            creado_por = null;
            city = null;
            country = null;
            language = null;
            num_filess = null;
            num_files_ifc = null;
            num_members_team = null;
            id_owner = null;
            name_owner = null;
            img_owner = null;
            type_project = null;
            description_project = null;
            rol = null;
            size_project = null;
            create_date = null;
            fecha_ultimo_cambio = null;
            id_curso = null;
            privacity_project = null;
            collaboration_request_project = null;
            course_name = null;
            img = null;
            type_project_edit = null;
        }

        public string id { get; set; }
        public string nombre { get; set; }
        public string creado_por { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string language { get; set; }
        public string num_filess { get; set; }
        public string num_files_ifc { get; set; }
        public string num_members_team { get; set; }
        public string id_owner { get; set; }
        public string name_owner { get; set; }
        public string img_owner { get; set; }
        public string type_project { get; set; }
        public string description_project { get; set; }
        public string rol { get; set; }
        public string size_project { get; set; }
        public string create_date { get; set; }
        public string fecha_ultimo_cambio { get; set; }
        public string id_curso { get; set; }
        public string privacity_project { get; set; }
        public string collaboration_request_project { get; set; }
        public string course_name { get; set; }
        public string img { get; set; }
        public List<string> type_project_edit { get; set; }
    }

    public class BSProjectListSchema : BSBaseSchema
    {
        public BSProjectListSchema()
        {
            list = null;
        }

        public List<BSProjectItem> list { get; set; }
    }
}