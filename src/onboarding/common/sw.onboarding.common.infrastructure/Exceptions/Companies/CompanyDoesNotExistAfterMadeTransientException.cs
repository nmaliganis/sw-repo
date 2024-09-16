using System;

namespace sw.auth.common.infrastructure.Exceptions.Companies;

public class CompanyDoesExistAfterMadeTransientException : Exception {
  public long CompanyId { get; }

  public CompanyDoesExistAfterMadeTransientException(long companyId) {
    this.CompanyId = companyId;
  }

  public override string Message => $"Company with Id: {this.CompanyId}  doesn't exist After Made Transient!";
}//Class : CompanyDoesExistAfterMadeTransientException