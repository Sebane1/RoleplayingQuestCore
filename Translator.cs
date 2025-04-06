using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LanguageConversionProxy;

namespace RoleplayingQuestCore
{
    public class Translator
    {
        static string[] _languageStrings = new string[] { "English", "Français", "Deutsch", "日本語", "中国人", "Кореиатәи" };

        public static string[] LanguageStrings { get => _languageStrings; set => _languageStrings = value; }

        public static async Task<string> LocalizeText(string translationText, LanguageEnum userLanguage, LanguageEnum textLanguage)
        {
            if (userLanguage != textLanguage)
            {
                LanguageRequest proxiedVoiceRequest = new LanguageRequest()
                {
                    Language = userLanguage,
                    TextLanguage = textLanguage,
                    TranslationText = translationText
                };
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://ai.hubujubu.com:5681");
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.Timeout = new TimeSpan(0, 6, 0);
                    var post = await httpClient.PostAsync(httpClient.BaseAddress, new StringContent(JsonConvert.SerializeObject(proxiedVoiceRequest)));
                    if (post.StatusCode == HttpStatusCode.OK)
                    {
                        var result = await post.Content.ReadAsStringAsync();
                        return result;
                    }
                }
            }
            return translationText;
        }
    }
}
