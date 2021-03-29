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
    public class BSAssociatedDocumentItem
    {
        public BSAssociatedDocumentItem()
        {
            nombre = null;
            fecha_ultimo_cambio = null;
            url_file = null;
        }

        public string nombre { get; set; }
        public string fecha_ultimo_cambio { get; set; }
        public string url_file { get; set; }
    }

    public class BSDocumentItem
    {
        public BSDocumentItem()
        {
            version = null;
            nombre = null;
            id = null;
            id_propietario = null;
            nombre_propietario = null;
            imagen_propietario = null;
            fecha_ultimo_cambio = null;
            tamanyo = null;
            url_file = null;
            img_file = null;
            numero_ficheros_asociados = null;
            is_created_initial_program = null;
            list = null;
        }

        public string version { get; set; }
        public string nombre { get; set; }
        public string id { get; set; }
        public string id_propietario { get; set; }
        public string nombre_propietario { get; set; }
        public string imagen_propietario { get; set; }
        public string fecha_ultimo_cambio { get; set; }
        public string tamanyo { get; set; }
        public string url_file { get; set; }
        public string img_file { get; set; }
        public string numero_ficheros_asociados { get; set; }
        public string is_created_initial_program { get; set; }
        public List<BSAssociatedDocumentItem> list { get; set; }
    }

    public class BSDocumentListSchema : BSBaseSchema
    {
        public BSDocumentListSchema()
        {
            list = null;
        }

        public List<BSDocumentItem> list { get; set; }
    }
}