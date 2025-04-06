namespace LanguageConversionProxy
{
    internal class LanguageRequest
    {
        LanguageEnum _language = LanguageEnum.English;
        LanguageEnum _textLanguage = LanguageEnum.English;
        string _translationText = "";

        public LanguageEnum Language { get => _language; set => _language = value; }
        public string TranslationText { get => _translationText; set => _translationText = value; }
        public LanguageEnum TextLanguage { get => _textLanguage; set => _textLanguage = value; }
    }
}

