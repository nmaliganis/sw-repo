using System;

namespace sw.interprocess.api.Helpers.Exceptions
{
  internal class InvalidPackageChecksumException : Exception
  {
    private readonly string _chsmValue;

    public InvalidPackageChecksumException(string chsmValue)
    {
            _chsmValue = chsmValue;
    }

    public override string ToString()
    {
      return BuildMessage();
    }

    public override string Message => BuildMessage();

    private string BuildMessage()
    {
      return $"Invalid Package Crc culc: {_chsmValue} ";
    }
  }
}