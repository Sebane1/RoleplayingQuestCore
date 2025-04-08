using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LanguageConversionProxy;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace RoleplayingQuestCore
{
    public static class Translator
    {
        static ConcurrentDictionary<int, ConcurrentDictionary<string, string>> _dictionary = new ConcurrentDictionary<int, ConcurrentDictionary<string, string>>();
        static ConcurrentDictionary<int, ConcurrentDictionary<string, bool>> _alreadyProcessing = new ConcurrentDictionary<int, ConcurrentDictionary<string, bool>>();
        static string[] _languageStrings = new string[] { "English", "Français", "Deutsch", "日本語", "中国人", "한국인", "Svenska" };
        static LanguageEnum _uiLanguage = LanguageEnum.English;
        private static string _cacheLocation = string.Empty;
        public static string[] LanguageStrings { get => _languageStrings; set => _languageStrings = value; }
        public static LanguageEnum UiLanguage { get => _uiLanguage; set => _uiLanguage = value; }
        public static string CacheLocation { get => _cacheLocation; set => _cacheLocation = value; }
        static Stopwatch _cooldown = new Stopwatch();
        public static event EventHandler<string> OnError;
        public static event EventHandler<string> OnTranslationEvent;
        public static void LoadCache(string cacheLocation)
        {
            _cacheLocation = cacheLocation;
            if (File.Exists(_cacheLocation))
            {
                _dictionary = JsonConvert.DeserializeObject<ConcurrentDictionary<int, ConcurrentDictionary<string, string>>>(File.ReadAllText(_cacheLocation));
            }
        }
        public static async Task<string> LocalizeText(string translationValue, LanguageEnum userLanguage, LanguageEnum textLanguage)
        {
            if (userLanguage != textLanguage && !string.IsNullOrEmpty(translationValue))
            {
                int languageId = (int)_uiLanguage;

                string cachedTranslation = GetCachedTranslation(translationValue, languageId);
                if (!string.IsNullOrWhiteSpace(cachedTranslation))
                {
                    return cachedTranslation;
                }
                else
                {
                    if (!_alreadyProcessing.ContainsKey(languageId))
                    {
                        _alreadyProcessing[languageId] = new ConcurrentDictionary<string, bool>();
                    }
                    if (!_alreadyProcessing[languageId].ContainsKey(translationValue))
                    {
                        _alreadyProcessing[languageId][translationValue] = true;
                        LanguageRequest languageRequest = new LanguageRequest()
                        {
                            Language = userLanguage,
                            TextLanguage = textLanguage,
                            TranslationText = translationValue
                        };
                        using (HttpClient httpClient = new HttpClient())
                        {
                            httpClient.BaseAddress = new Uri("http://ai.hubujubu.com:5681");
                            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            httpClient.Timeout = new TimeSpan(0, 0, 200);
                            var post = await httpClient.PostAsync(httpClient.BaseAddress, new StringContent(JsonConvert.SerializeObject(languageRequest)));
                            if (post.StatusCode == HttpStatusCode.OK)
                            {
                                var value = await post.Content.ReadAsStringAsync();
                                if (!string.IsNullOrWhiteSpace(value))
                                {
                                    _dictionary[languageId][translationValue] = value;
                                    OnTranslationEvent.Invoke(null, translationValue + " -> " + value);
                                    try
                                    {
                                        await File.WriteAllTextAsync(_cacheLocation, JsonConvert.SerializeObject(_dictionary, Formatting.Indented));
                                    }
                                    catch (Exception ex)
                                    {
                                        OnError?.Invoke(null, ex.Message + "\r\n" + ex.StackTrace);
                                    }
                                    return value.CleanPunctuation();
                                }
                                else
                                {
                                    _alreadyProcessing[languageId].TryRemove(translationValue, out var poop);
                                }
                            }
                            else
                            {
                                _alreadyProcessing[languageId].TryRemove(translationValue, out var poop);
                            }
                        }
                    }
                }
            }
            return translationValue;
        }

        private static string CleanPunctuation(this string value)
        {
            return value.Replace(" !", "!").Replace(" ?", "?").Replace(" .", ".").Replace(" :", ":");
        }

        private static string GetCachedTranslation(string translationValue, int languageId)
        {
            if (!_dictionary.ContainsKey(languageId))
            {
                _dictionary[languageId] = new ConcurrentDictionary<string, string>();
            }
            if (_dictionary[languageId].ContainsKey(translationValue))
            {
                var value = _dictionary[languageId][translationValue];
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value.CleanPunctuation();
                }
                else
                {
                    _dictionary[languageId].TryRemove(translationValue, out value);
                }
            }
            return null;
        }

        public static string LocalizeUI(string translationText)
        {
            return LocalizeUI(translationText, LanguageEnum.English);
        }
        public static string LocalizeUI(string translationText, LanguageEnum textLanguage)
        {
            var userLanguage = _uiLanguage;
            try
            {
                if (userLanguage != textLanguage)
                {
                    if (!string.IsNullOrEmpty(translationText))
                    {
                        string[] uiText = translationText.Split("##");
                        string translationValue = uiText.Length > 1 ? uiText[0] : translationText;
                        int languageId = (int)_uiLanguage;
                        if (!string.IsNullOrEmpty(translationValue))
                        {
                            string cachedTranslation = GetCachedTranslation(translationValue, languageId);
                            if (cachedTranslation != null)
                            {
                                return cachedTranslation + (uiText.Length > 1 ? "##" + uiText[1] : "");
                            }
                            else
                            {
                                Task.Run(async () =>
                                {
                                    try
                                    {
                                        var value = await LocalizeText(translationValue, userLanguage, textLanguage);
                                    }
                                    catch (Exception ex)
                                    {
                                        OnError?.Invoke(null, ex.Message + "\r\n" + ex.StackTrace);
                                    }
                                });
                            }
                        }
                        else
                        {
                            OnError?.Invoke(null, translationText + " is not valid for translation.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(null, ex.StackTrace);
            }
            return translationText;
        }
        public static string[] LocalizeTextArray(string[] strings)
        {
            List<string> result = new List<string>();
            foreach (string s in strings)
            {
                result.Add(LocalizeUI(AddSpacesToSentence(s, true)));
            }
            return result.ToArray();
        }
        static string AddSpacesToSentence(string text, bool preserveAcronyms)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }
    }
}
