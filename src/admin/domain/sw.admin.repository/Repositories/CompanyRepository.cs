using sw.admin.contracts.ContractRepositories;
using sw.admin.model;
using sw.admin.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Paging;
using System.Linq;
using sw.admin.repository.DbContexts;

namespace sw.admin.repository.Repositories
{
    public class CompanyRepository : RepositoryBase<Company, long>, ICompanyRepository
    {
        public CompanyRepository(swDbContext context)
            : base(context)
        {
        }

        public QueryResult<Company> FindAllActivePagedOf(int? pageNum, int? pageSize)
        {
            var qry = from com in Context.Companies
                      where com.Active
                      select com;

            if (pageNum == -1 & pageSize == -1)
            {
                return new QueryResult<Company>(qry.AsQueryable());
            }

            var result = qry.Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                            .Take((int)pageSize)
                            .AsQueryable();

            return new QueryResult<Company>(result, qry.Count(), (int)pageSize);
        }

        public Company FindActiveBy(long id)
        {
            var qry = from com in Context.Companies
                      where com.Active
                         && com.Id == id
                      select com;

            return qry.FirstOrDefault();
        }

        public Company FindCompanyByName(string name)
        {
            var qry = from com in Context.Companies
                      where com.Active
                         && com.Name == name
                      select com;

            return qry.FirstOrDefault();
        }
    }// Class: CompanyRepository
}// Namespace: sw.admin.repository.Repositories
