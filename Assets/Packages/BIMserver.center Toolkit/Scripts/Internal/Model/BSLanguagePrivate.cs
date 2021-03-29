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

namespace BIMservercenter.Toolkit.Public.Model
{
    public static class BSLanguageMethods
    {
        public static string GetStringValue(this BSLanguage bSLanguage)
        {
            return $"{(int)bSLanguage}";
        }

        public static string Get2LetterISOCode(this UnityEngine.SystemLanguage systemLanguage)
        {
            string isoCode;

            switch (systemLanguage)
            {
                case UnityEngine.SystemLanguage.Afrikaans: isoCode = "AF"; break;
                case UnityEngine.SystemLanguage.Arabic: isoCode = "AR"; break;
                case UnityEngine.SystemLanguage.Basque: isoCode = "EU"; break;
                case UnityEngine.SystemLanguage.Belarusian: isoCode = "BY"; break;
                case UnityEngine.SystemLanguage.Bulgarian: isoCode = "BG"; break;
                case UnityEngine.SystemLanguage.Catalan: isoCode = "CA"; break;
                case UnityEngine.SystemLanguage.Chinese: isoCode = "ZH"; break;
                case UnityEngine.SystemLanguage.Czech: isoCode = "CS"; break;
                case UnityEngine.SystemLanguage.Danish: isoCode = "DA"; break;
                case UnityEngine.SystemLanguage.Dutch: isoCode = "NL"; break;
                case UnityEngine.SystemLanguage.English: isoCode = "EN"; break;
                case UnityEngine.SystemLanguage.Estonian: isoCode = "ET"; break;
                case UnityEngine.SystemLanguage.Faroese: isoCode = "FO"; break;
                case UnityEngine.SystemLanguage.Finnish: isoCode = "FI"; break;
                case UnityEngine.SystemLanguage.French: isoCode = "FR"; break;
                case UnityEngine.SystemLanguage.German: isoCode = "DE"; break;
                case UnityEngine.SystemLanguage.Greek: isoCode = "EL"; break;
                case UnityEngine.SystemLanguage.Hebrew: isoCode = "IW"; break;
                case UnityEngine.SystemLanguage.Hungarian: isoCode = "HU"; break;
                case UnityEngine.SystemLanguage.Icelandic: isoCode = "IS"; break;
                case UnityEngine.SystemLanguage.Indonesian: isoCode = "IN"; break;
                case UnityEngine.SystemLanguage.Italian: isoCode = "IT"; break;
                case UnityEngine.SystemLanguage.Japanese: isoCode = "JA"; break;
                case UnityEngine.SystemLanguage.Korean: isoCode = "KO"; break;
                case UnityEngine.SystemLanguage.Latvian: isoCode = "LV"; break;
                case UnityEngine.SystemLanguage.Lithuanian: isoCode = "LT"; break;
                case UnityEngine.SystemLanguage.Norwegian: isoCode = "NO"; break;
                case UnityEngine.SystemLanguage.Polish: isoCode = "PL"; break;
                case UnityEngine.SystemLanguage.Portuguese: isoCode = "PT"; break;
                case UnityEngine.SystemLanguage.Romanian: isoCode = "RO"; break;
                case UnityEngine.SystemLanguage.Russian: isoCode = "RU"; break;
                case UnityEngine.SystemLanguage.SerboCroatian: isoCode = "SH"; break;
                case UnityEngine.SystemLanguage.Slovak: isoCode = "SK"; break;
                case UnityEngine.SystemLanguage.Slovenian: isoCode = "SL"; break;
                case UnityEngine.SystemLanguage.Spanish: isoCode = "ES"; break;
                case UnityEngine.SystemLanguage.Swedish: isoCode = "SV"; break;
                case UnityEngine.SystemLanguage.Thai: isoCode = "TH"; break;
                case UnityEngine.SystemLanguage.Turkish: isoCode = "TR"; break;
                case UnityEngine.SystemLanguage.Ukrainian: isoCode = "UK"; break;
                case UnityEngine.SystemLanguage.Vietnamese: isoCode = "VI"; break;
                case UnityEngine.SystemLanguage.ChineseSimplified: isoCode = "ZH"; break;
                case UnityEngine.SystemLanguage.ChineseTraditional: isoCode = "ZH"; break;
                default: isoCode = "EN"; break;
            }

            return isoCode;
        }
    }
}
