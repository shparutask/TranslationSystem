using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace TranslationLib
{
    class YandexTranslator
    {
        public string Translate(string s, string lang)
        {
            if (s.Length > 0)
            {
                WebRequest request = WebRequest.Create("https://translate.yandex.net/api/v1.5/tr.json/translate?"
                    + "key=trnsl.1.1.20190317T141236Z.1cd24321f942fe70.b9b1bce3582eee6c5dab521d27c73f9e9781dff8"
                    + "&text=" + s
                    + "&lang=" + lang);

                WebResponse response = request.GetResponse();

                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    string line;

                    if ((line = stream.ReadLine()) != null)
                    {
                        YaTranslation translation = JsonConvert.DeserializeObject<YaTranslation>(line);

                        s = "";

                        foreach (string str in translation.text)
                        {
                            s += str;
                        }
                    }
                }

                return s;
            }
            else
                return "";
        }
    }

    class YaTranslation
    {
        public string code { get; set; }
        public string lang { get; set; }
        public string[] text { get; set; }
    }
}
