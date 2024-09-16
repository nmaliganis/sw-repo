using System;
using System.Collections.Generic;
using sw.asset.model.Devices.Simcards;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;

namespace sw.asset.contracts.ContractRepositories;

public interface ISimcardRepository : IRepository<Simcard, long>
{
  Simcard FindOneByIccid(string iccid);
  Simcard FindOneByImsi(string imsi);
  Simcard FindOneByIccidAndImsi(string iccid, string imsi);
  Simcard FindOneByIccidOrImsiOrNumber(string iccid, string imsi, string number);
  Simcard FindOneByNumber(string number);
  QueryResult<Simcard> FindAllSimcardsPagedOf(int? pageNum, int? pageSize);
  QueryResult<Simcard> FindAllActivePagedOf(int? pageNum, int? pageSize);
  IList<Simcard> FindAllActiveSimcards();
  Simcard FindOneActiveById(long id);
}