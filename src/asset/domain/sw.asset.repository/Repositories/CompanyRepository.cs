using sw.asset.contracts.ContractRepositories;
using sw.asset.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Paging;
using System.Linq;
using sw.asset.model.Companies;
using NHibernate;
using sw.asset.model.Devices;
using NHibernate.Criterion;

namespace sw.asset.repository.Repositories;

public class CompanyRepository : RepositoryBase<Company, long>, ICompanyRepository
{
    public CompanyRepository(ISession session)
        : base(session)
    {
    }

    public QueryResult<Company> FindAllActivePagedOf(int? pageNum, int? pageSize)
    {
        var query = Session.QueryOver<Company>();

        if (pageNum == -1 & pageSize == -1)
        {
            return new QueryResult<Company>(query?
              .Where(com => com.Active == true)
              .List().AsQueryable());
        }

        return new QueryResult<Company>(query
              .Where(com => com.Active == true)
              .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
              .Take((int)pageSize).List().AsQueryable(),
            query.ToRowCountQuery().RowCount(),
            (int)pageSize)
          ;
    }

    public Company FindActiveById(long id)
    {
        return (Company)
            Session.CreateCriteria(typeof(Company))
                .Add(Restrictions.Eq("Id", id))
                .Add(Restrictions.Eq("Active", true))
                .UniqueResult()
            ;
    }

    public Company FindActiveByName(string name)
    {
        return (Company)
            Session.CreateCriteria(typeof(Company))
               .Add(Restrictions.Eq("Name", name))
               .Add(Restrictions.Eq("Active", true))
               .UniqueResult()
               ;
    }

}// Class: CompanyRepository