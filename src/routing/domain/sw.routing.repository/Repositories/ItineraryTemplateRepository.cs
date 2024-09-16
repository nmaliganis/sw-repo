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
using sw.routing.model.ItineraryTemplates;

namespace sw.routing.repository.Repositories;

public class ItineraryTemplateRepository : RepositoryBase<ItineraryTemplate, long>, IItineraryTemplateRepository
{

    private readonly WKTReader _wkt;
    public ItineraryTemplateRepository(ISession session)
      : base(session)
    {
        _wkt = new WKTReader();
    }

    public QueryResult<ItineraryTemplate> FindAllActiveItineraryTemplatesByPagedOf(int? pageNum, int? pageSize)
    {
        var queryStr =
            $@"select t.Id, 
                    t.Name,
                    NHSP.AsText(t.StartFrom),
                    NHSP.AsText(t.EndTo),
                    t.Stream,
                    t.Description,
                    t.MinFillLevel,
                    t.StartTime,
                    t.Zones,
                    t.Occurrence
                    from ItineraryTemplate as t 
                    where t.Active = :Active"
          ;
        try
        {
            var itineraryTemplatesRepo = Session
              .CreateQuery(queryStr)
              .SetParameter("Active", true)
              .List();

            List<ItineraryTemplate> itineraryTemplates = new List<ItineraryTemplate>();

            itineraryTemplates.AddRange(from object[] point in itineraryTemplatesRepo
                                        select new ItineraryTemplate()
                                        {
                                            Id = (long)point[0],
                                            Name = (string)point[1],
                                            //StartFrom = (Point)_wkt.Read((string)point[2]),
                                            //EndTo = (Point)_wkt.Read((string)point[3]),
                                            Stream = (StreamType)point[4],
                                            Description = (string)point[5],
                                            MinFillLevel = (double)point[6],
                                            StartTime = (TimeSpan)point[7],
                                            Zones = (ZoneJsonbType)point[8],
                                            Occurrence = (OccurrenceJsonbType)point[9],
                                        });

            if (pageNum == -1 & pageSize == -1)
            {
                return new QueryResult<ItineraryTemplate>(itineraryTemplates.AsQueryable());
            }

            return new QueryResult<ItineraryTemplate>(itineraryTemplates
                  .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                  .Take((int)pageSize).AsQueryable(),
                itineraryTemplates.Count,
                (int)pageSize)
              ;
        }
        catch (Exception e)
        {
            throw new FindAllItineraryTemplateException(e.Message);
        }
    }

    public QueryResult<ItineraryTemplate> FindAllActiveItineraryTemplatesPagedOf(int? pageNum, int? pageSize)
    {
        var query = Session.QueryOver<ItineraryTemplate>();

        if (pageNum == -1 & pageSize == -1)
        {
            return new QueryResult<ItineraryTemplate>(query?
                .Where(r => r.Active == true)
                .List().AsQueryable());
        }

        return new QueryResult<ItineraryTemplate>(query
                    .Where(r => r.Active == true)
                    .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                    .Take((int)pageSize).List().AsQueryable(),
                query.ToRowCountQuery().RowCount(),
                (int)pageSize)
            ;
    }

    public int FindCountAllActiveItineraries()
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

    public async Task CreateItineraryTemplate(ItineraryTemplate itineraryTemplateToBeCreated)
    {
        using ITransaction transaction = Session?.BeginTransaction();
        try
        {
            if (Session == null)
                throw new NHibernateSessionTransactionNotExistException("Session == null");
            if (!Session.GetCurrentTransaction()!.IsActive)
                throw new NHibernateSessionTransactionNotActiveException("!Session.GetCurrentTransaction()!.IsActive");

            string sqlInsertion = "INSERT INTO public.\"ItineraryTemplates\" (name, stream, start_from, end_to, description, created_by, modified_by, deleted_by, deleted_reason, deleted_date, created_date, modified_date, active, occurrence, zones, min_fill_level, start_time) "
                                          + "VALUES ("
                                          + $"'{itineraryTemplateToBeCreated.Name}', "
                                          + $"'{itineraryTemplateToBeCreated.Stream.ToString()}', "
                                          //+ $"'ST_SetSRID(ST_MakePoint({itineraryTemplateToBeCreated.StartFrom.Coordinate.X.ToString(CultureInfo.InvariantCulture)}, {itineraryTemplateToBeCreated.StartFrom.Coordinate.Y.ToString(CultureInfo.InvariantCulture)}), 4326)', "
                                          //+ $"'ST_SetSRID(ST_MakePoint({itineraryTemplateToBeCreated.EndTo.Coordinate.X.ToString(CultureInfo.InvariantCulture)}, {itineraryTemplateToBeCreated.EndTo.Coordinate.Y.ToString(CultureInfo.InvariantCulture)}), 4326)', "
                                          + $"'{itineraryTemplateToBeCreated.Description}', "
                                          + $"'{itineraryTemplateToBeCreated.CreatedBy}', "
                                          + $"'{itineraryTemplateToBeCreated.ModifiedBy}', "
                                          + $"'{itineraryTemplateToBeCreated.DeletedBy}', "
                                          + $"'{itineraryTemplateToBeCreated.DeletedReason}', "
                                          + $"'{itineraryTemplateToBeCreated.DeletedDate}', "
                                          + $"'{itineraryTemplateToBeCreated.CreatedDate}', "
                                          + $"'{itineraryTemplateToBeCreated.ModifiedDate}', "
                                          + $"'{itineraryTemplateToBeCreated.Active}', "
                                          + $"'{itineraryTemplateToBeCreated.Occurrence}', "
                                          + $"'{itineraryTemplateToBeCreated.Zones}', "
                                          + $"'{itineraryTemplateToBeCreated.MinFillLevel}', "
                                          + $"'{itineraryTemplateToBeCreated.StartTime}'"
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

    public ItineraryTemplate FindItineraryTemplateByName(string name)
    {
        return
            (ItineraryTemplate)
            Session.CreateCriteria(typeof(Itinerary))
                .Add(Expression.Eq("Name", name))
                .Add(Expression.Eq("Active", true))
                .SetCacheable(true)
                .SetCacheMode(CacheMode.Normal)
                .UniqueResult()
            ;
    }
}