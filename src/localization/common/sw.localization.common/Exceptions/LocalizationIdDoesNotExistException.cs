using System;

namespace sw.localization.common.Exceptions
{

    public class LocalizationIdDoesNotExistException : Exception
    {
        public string ClassName { get; } = typeof(LocalizationIdDoesNotExistException).Name;
        public long Id { get; }

        public LocalizationIdDoesNotExistException(long id) => Id = id;
        public override string Message => $"Language value id: {Id} doesn't exist!";
    }
}
