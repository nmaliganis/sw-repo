using System;

namespace sw.localization.common.Exceptions
{

    public class LocalizationKeyDoesNotExistException : Exception
    {
        public string ClassName { get; } = typeof(LocalizationKeyDoesNotExistException).Name;
        public string Key { get; }
        public string Domain { get; }
        public string Language { get; }

        public LocalizationKeyDoesNotExistException(string domain, string language, string key)
        {
            Key = key;
            Domain = domain;
            Language = language;
        }
        public override string Message => $"Language key: {Key} in domain: {Domain} with language: {Language} does NOT exist!";
    }
}
