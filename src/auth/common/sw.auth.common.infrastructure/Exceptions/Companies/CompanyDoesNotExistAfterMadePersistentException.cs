using System;

namespace sw.auth.common.infrastructure.Exceptions.Companies;

public class CompanyDoesNotExistAfterMadePersistentException : Exception
{
  public long CompanyId { get; }
  public string Name { get; private set; }

  public CompanyDoesNotExistAfterMadePersistentException(string name)
  {
    this.Name = name;
  }

  public CompanyDoesNotExistAfterMadePersistentException(long companyId)
  {
    this.CompanyId = companyId;
  }

  public override string Message => $" Company with Name: {Name} Or/And {CompanyId} was not made Persistent!";
}// Class : CompanyDoesNotExistAfterMadePersistentException