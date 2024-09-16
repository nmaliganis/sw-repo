using System;

namespace sw.auth.common.infrastructure.Exceptions.Companies;

public class CompanyDoesNotExistException : Exception {
  public long CompanyId { get; }

  public CompanyDoesNotExistException(long companyId) {
    this.CompanyId = companyId;
  }

  public override string Message => $"Company with Id: {this.CompanyId}  doesn't exists!";
}//Class : CompanyDoesNotExistException