using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OfficeCulture.Translate
{
    public class TranslateClient
    {
        private Dictionary<string, string> Languages = new Dictionary<string, string>
        {
            { "af", "Afrikaans"},
            { "ar", "Arabic"},
            { "bn", "Bangla"},
            { "bs", "Bosnian "},
            { "bg", "Bulgarian"},            
            { "ca", "Catalan"},
            { "cs", "Czech"},
            { "cy", "Welsh"},
            { "da", "Danish"},
            { "de", "German"},
            { "el", "Greek"},
            { "et", "Estonian"},
            { "es", "Spanish"},
            { "fj", "Fijian"},
            { "fil", "Filipino"},
            { "fi", "Finnish"},
            { "fr", "French"},
            { "fa", "Persian"},
            { "hr", "Croatian"},
            { "ht", "Haitian Creole"},
            { "he", "Hebrew"},
            { "hi", "Hindi"},
            { "hu", "Hungarian"},
            { "id", "Indonesian"},
            { "is", "Icelandic"},
            { "it", "Italian"},
            { "ja", "Japanese"},
            { "ko", "Korean"},
            { "lv", "Latvian"},
            { "lt", "Lithuanian"},
            { "mg", "Malagasy"},
            { "ms", "Malay"},
            { "mt", "Maltese"},
            { "nb", "Norwegian"},
            { "nl", "Dutch"},
            { "otq", "Queretaro Otomi"},
            { "pl", "Polish"},
            { "pt", "Portuguese"},
            { "ro", "Romanian"},
            { "ru", "Russian"},
            { "sm", "Samoan"},
            { "sw", "Kiswahili"},
            { "sr-Latn", "Serbian"},
            { "sk", "Slovak"},
            { "sl", "Slovenian"},
            { "sv", "Swedish"},
            { "tlh", "Klingon"},
            { "ty", "Tahitian"},
            { "ta", "Tamil"},
            { "te", "Telugu"},
            { "th", "Thai"},
            { "to", "Tongan"},
            { "tr", "Turkish"},
            { "uk", "Ukrainian"},
            { "ur", "Urdu"},
            { "vi", "Vietnamese"},
            { "yua", "Yucatec Maya"},
            { "zh-Hans", "Chinese "},
        };

        public async Task<Dictionary<string, string>> GetTranslation(string message)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "0ce0b0d6cf7b4987bfaffb70de33e62d");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var random = new Random();
            List<string> languageCodes = Enumerable.ToList(Languages.Keys);

            string to = string.Empty;
            List<int> indexes = new List<int>();
            do {

                int index = 0;
                do
                {
                    index = random.Next(Languages.Count);
                } while (indexes.Contains(index));

                var languageCode = languageCodes.ElementAt(index);

                to += $"&to={languageCode}";

            } while (random.Next(0, 3) > 0);

            var translationText = new List<TranslatableText>
            {
                new TranslatableText { Text = message }
            };

            var content = new StringContent(JsonConvert.SerializeObject(translationText), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"https://api.cognitive.microsofttranslator.com/translate?api-version=3.0{to}", content);
            var responseJson = await response.Content.ReadAsStringAsync();
            var translationData = JsonConvert.DeserializeObject<List<RootObject>>(responseJson);

            return translationData.First().translations.ToDictionary(x => Languages[x.to], x => x.text);
        }

        public class TranslatableText
        {
            public string Text { get; set; }
        }

        public class DetectedLanguage
        {
            public string language { get; set; }
            public double score { get; set; }
        }

        public class Translation
        {
            public string text { get; set; }
            public string to { get; set; }
        }

        public class RootObject
        {
            public DetectedLanguage detectedLanguage { get; set; }
            public List<Translation> translations { get; set; }
        }
    }
}
