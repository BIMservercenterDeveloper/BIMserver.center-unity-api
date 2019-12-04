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

using BIMservercenter.Toolkit.Internal.API.Response;
using BIMservercenter.Toolkit.Internal.API.Response.Schema;
using BIMservercenter.Toolkit.Internal.API.Utilities;
using System;
using System.Net.Http;
using System.Text;
using System.Xml;

namespace BIMservercenter.Toolkit.Internal.API.Session
{
    public class BSSessionCommand
    {        
        private static string URL_API = "https://api.bimserver.center/api.asp";

        private string command;
        private BSIResponse response;
        private BSSession session;
        private BSSessionDelegate sessionDelegate;

        public BSSessionCommand(string command, BSIResponse response, BSSession session, BSSessionDelegate sessionDelegate)
        {
            this.command = command;
            this.response = response;
            this.session = session;
            this.sessionDelegate = sessionDelegate;
        }

        // ---------------------------------------------------------------------------
        // Execute
        // ---------------------------------------------------------------------------

        private void ExecuteRequest(object extras)
        {
            HttpClient client;
            HttpContent content;

            client = new HttpClient();
            content = new StringContent(this.command, Encoding.UTF8, "text/xml");

            try
            {
                HttpResponseMessage result;

                result = client.PostAsync(URL_API, content).Result;

                if (result.IsSuccessStatusCode == true)
                {
                    string response;
                    XmlDocument xmlDocument;
                    BSBaseSchema baseSchema;

                    response = result.Content.ReadAsStringAsync().Result;
                    xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(response);

                    baseSchema = BSXmlParser<BSBaseSchema>.Parse(xmlDocument.DocumentElement.InnerXml);

                    if (baseSchema.status.ToLower() == "ok")
                    {
                        this.response.OnSucced(response, this.session, this.sessionDelegate, extras);
                    }
                    else if (baseSchema.status.ToLower() == "error")
                    {
                        BSErrorSchema errorSchema;
                        
                        errorSchema = BSXmlParser<BSErrorSchema>.Parse(xmlDocument.DocumentElement.InnerXml);

                        {
                            int errorCode;

                            errorCode = int.Parse(errorSchema.error_code);

                            switch (errorCode)
                            {
                                case 106: this.session.ResetSession(); break;
                            }

                            this.response.OnError(errorCode, errorSchema.error, this.sessionDelegate);
                        }
                    }
                    else
                    {
                        this.response.OnError(0, "Bad Formatting", this.sessionDelegate);
                    }
                }
                else
                {
                    this.response.OnError(500, result.ReasonPhrase, this.sessionDelegate);
                }
            }
            catch (Exception e)
            {
                this.response.OnError(500, e.Message, this.sessionDelegate);
            }
        }

        // ---------------------------------------------------------------------------

        public void Execute(object extras = null)
        {
            this.ExecuteRequest(extras);
        }
    }
}
