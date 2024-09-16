using sw.routing.common.infrastructure.Exceptions.Templates;
using sw.routing.contracts.ContractRepositories;
using sw.routing.model.CustomTypes;
using sw.routing.model.Itineraries;
using sw.routing.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Exceptions.Repositories.NHibernate;
using sw.infrastructure.Paging;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using sw.routing.common.infrastructure.Exceptions.Locations;
using sw.routing.model.ItineraryTemplates;
using sw.routing.model.ItineraryTemplates.LocationPoints;

namespace sw.routing.repository.Repositories;

public class LocationRepository : RepositoryBase<LocationPoint, long>, ILocationRepository
{

    private readonly WKTReader _wkt;
    public LocationRepository(ISession session)
      : base(session)
    {
        _wkt = new WKTReader();
    }

    public QueryResult<LocationPoint> FindAllActiveLocationsPagedOf(int? pageNum, int? pageSize)
    {
        var queryStr =
            $@"select t.Id, 
                    t.Name,
                    NHSP.AsText(t.Location),
                    t.Description
                    from LocationPoint as t 
                    where t.Active = :Active"
          ;
        try
        {
            var LocationsRepo = Session
              .CreateQuery(queryStr)
              .SetParameter("Active", true)
              .List();

            List<LocationPoint> locations = new List<LocationPoint>();

            locations.AddRange(from object[] point in LocationsRepo
                                        select new LocationPoint()
                                        {
                                            Id = (long)point[0],
                                            Name = (string)point[1],
                                            Location = (Point)_wkt.Read((string)point[2]),
                                            Description = (string)point[3],
                                        });

            if (pageNum == -1 & pageSize == -1)
            {
                return new QueryResult<LocationPoint>(locations.AsQueryable());
            }

            return new QueryResult<LocationPoint>(locations
                  .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                  .Take((int)pageSize).AsQueryable(),
                locations.Count,
                (int)pageSize)
              ;
        }
        catch (Exception e)
        {
            throw new FindAllLocationException(e.Message);
        }
    }

    public int FindCountAllActiveLocations()
    {
        var count = Session
            .CreateCriteria<Itinerary>()
            .Add(Expression.Eq("Active", true))
            .SetProjection(
                Projections.Count(Projections.Id())
            )
            .UniqueResult<int>();

        return count;
    }

    public async Task CreateLocation(LocationPoint locationToBeCreated)
    {
        using ITransaction transaction = Session?.BeginTransaction();
        try
        {
            if (Session == null)
                throw new NHibernateSessionTransactionNotExistException("Session == null");
            if (!Session.GetCurrentTransaction()!.IsActive)
                throw new NHibernateSessionTransactionNotActiveException("!Session.GetCurrentTransaction()!.IsActive");

            string sqlInsertion = "INSERT INTO public.\"Locations\" (name, stream, start_from, end_to, description, created_by, modified_by, deleted_by, deleted_reason, deleted_date, created_date, modified_date, active, occurrence, zones, min_fill_level, start_time) "
                                          + "VALUES ("
                                          + $"'{locationToBeCreated.Name}', "
                                          //+ $"'ST_SetSRID(ST_MakePoint({locationToBeCreated.StartFrom.Coordinate.X.ToString(CultureInfo.InvariantCulture)}, {locationToBeCreated.StartFrom.Coordinate.Y.ToString(CultureInfo.InvariantCulture)}), 4326)', "
                                          + ");"
                        ;

            var query = Session.CreateSQLQuery(sqlInsertion);

            var result = await query.ExecuteUpdateAsync();

            await Session.FlushAsync();
            transaction?.CommitAsync();
        }
        catch (Exception ex)
        {
            transaction?.RollbackAsync();
            throw new NHibernateSessionTransactionFailedException(ex.Message);
        }
    }

    public LocationPoint FindOneLocationByName(string name)
    {
        return
            (LocationPoint)
            Session.CreateCriteria(typeof(Itinerary))
                .Add(Expression.Eq("Name", name))
                .Add(Expression.Eq("Active", true))
                .SetCacheable(true)
                .SetCacheMode(CacheMode.Normal)
                .UniqueResult()
            ;
    }
}