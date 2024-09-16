using sw.asset.contracts.ContractRepositories;
using sw.asset.model.Persons;
using sw.asset.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using NHibernate;
using NHibernate.Criterion;

namespace sw.asset.repository.Repositories;

public class PersonRepository : RepositoryBase<Person, long>, IPersonRepository
{

    public PersonRepository(ISession session)
      : base(session)
    {
    }

    public QueryResult<Person> FindAllActivePagedOf(int? pageNum, int? pageSize)
    {
        //var query = Session.QueryOver<Person>();

        //if (pageNum == -1 & pageSize == -1)
        //{
        //  return new QueryResult<Person>(query?
        //    .Where(mc => mc.Active == true)
        //    .List().AsQueryable());
        //}

        //return new QueryResult<Person>(query
        //      .JoinAlias(dev => dev.PersonModel, () => PersonModel)
        //      .Where(dev => dev.Active == true)
        //      .And(() => PersonModel.Active == true)
        //      .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
        //      .Take((int)pageSize).List().AsQueryable(),
        //    query.ToRowCountQuery().RowCount(),
        //    (int)pageSize)
        //  ;
        return null;
    }

    public Person FindOneActiveById(long id)
    {
        throw new System.NotImplementedException();
    }

    public int FindCountAllActive()
    {
        int count;

        count = Session
          .CreateCriteria<Person>()
          .Add(Expression.Eq("Active", true))
          .SetProjection(
            Projections.Count(Projections.Id())
          )
          .UniqueResult<int>();

        return count;
    }

    public Person FindOneByEmail(string email)
    {
        return (Person)
          Session.CreateCriteria(typeof(Person))
            .Add(Restrictions.Eq("Email", email))
            .Add(Restrictions.Eq("Active", true))
            .UniqueResult()
          ;
    }

    public Person FindActiveById(long id)
    {
        //Person dev = null;

        //return this.Session.QueryOver<Person>(() => dev)
        //  .JoinAlias(dev => dev.PersonModel, () => PersonModel)
        //  .Where(() => dev.Active == true)
        //  .And(() => PersonModel.Active == true)
        //  .And(() => dev.Id == id)
        //  .Cacheable()
        //  .CacheMode(CacheMode.Normal)
        //  .SetFlushMode(FlushMode.Manual)
        //  .SingleOrDefault();
        return null;
    }
}// Class: PersonRepository