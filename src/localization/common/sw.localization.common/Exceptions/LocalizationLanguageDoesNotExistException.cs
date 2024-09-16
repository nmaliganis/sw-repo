using System;

namespace sw.localization.common.Exceptions
{

    public class LocalizationLanguageDoesNotExistException : Exception
    {
        public string ClassName { get; } = typeof(LocalizationLanguageDoesNotExistException).Name;
        public string Name { get; }

        public LocalizationLanguageDoesNotExistException(string name) => Name = name;
        public override string Message => $"Language domain with name: {Name}  doesn't exist!";
    }
}
