using System;
using Lean.Localization;

namespace Localization
{
    public class LocalizationInitializer
    {
        public void ApplyLocalization(string language)
        {
            const string Russian = nameof(Russian);
            const string English = nameof(English);

            const string CommandRussianLanguage = "ru";
            const string CommandBelarusLanguage = "be";
            const string CommandKazakhstanLanguage = "kk";
            const string CommandUzbekistanLanguage = "uz";
            const string CommandUkraineLanguage = "uk";

            if (language == null)
                throw new NullReferenceException(nameof(language));

            switch (language)
            {
                case CommandRussianLanguage:
                case CommandBelarusLanguage:
                case CommandKazakhstanLanguage:
                case CommandUzbekistanLanguage:
                case CommandUkraineLanguage:
                    LeanLocalization.SetCurrentLanguageAll(Russian);
                    break;

                default:
                    LeanLocalization.SetCurrentLanguageAll(English);
                    break;
            }

            LeanLocalization.UpdateTranslations();
        }
    }
}