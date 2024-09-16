using System;

namespace sw.localization.common.Exceptions
{

    public class LocalizationDomainDoesNotExistException : Exception
    {
        public string ClassName { get; } = typeof(LocalizationDomainDoesNotExistException).Name;
        public string Name { get; }

        public LocalizationDomainDoesNotExistException(string name) => Name = name;
        public override string Message => $"Language domain with name: {Name}  doesn't exist!";
    }
}
