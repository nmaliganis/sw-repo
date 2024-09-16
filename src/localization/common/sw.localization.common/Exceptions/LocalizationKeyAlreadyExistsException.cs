using System;

namespace sw.localization.common.Exceptions
{

    public class LocalizationKeyAlreadyExistsException : Exception
    {
        public string ClassName { get; } = typeof(LocalizationKeyAlreadyExistsException).Name;
        public string Key { get; }
        public string Domain { get; }
        public string Language { get; }

        public LocalizationKeyAlreadyExistsException(string domain, string language, string key)
        {
            Key = key;
            Domain = domain;
            Language = language;
        }
        public override string Message => $"Language key: {Key} in domain: {Domain} with language: {Language} already exists!";
    }
}
