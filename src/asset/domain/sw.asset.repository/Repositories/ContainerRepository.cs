using sw.asset.common.infrastructure.Exceptions.Assets.Containers;
using sw.asset.contracts.ContractRepositories;
using sw.asset.model.Assets.Categories;
using sw.asset.model.Assets.Containers;
using sw.asset.model.Companies;
using sw.asset.model.Companies.Zones;
using sw.asset.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Exceptions.Repositories.NHibernate;
using sw.infrastructure.Extensions;
using sw.infrastructure.Paging;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Spatial.Criterion;
using NHibernate.SqlCommand;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Spatial.Criterion.Lambda;
using sw.asset.model.Assets.Containers.Projections;
using sw.asset.model.Assets.Containers.Types;

namespace sw.asset.repository.Repositories;

public class ContainerRepository : RepositoryBase<Container, long>, IContainerRepository
{
    private readonly WKTReader _wkt;

    public ContainerRepository(ISession session)
      : base(session)
    {
        _wkt = new WKTReader();
    }

    public QueryResult<Container> FindAllActivePagedOf(int? pageNum, int? pageSize)
    {
        var queryStr =
            $@"select c.Id, 
                    c.Name,
                    c.LastServicedDate,
                    c.MandatoryPickupDate,
                    NHSP.AsText(c.GeoPoint),
                    c.ContainerStatus,
                    c.ContainerCondition,
                    c.Capacity,
                    c.Level,
                    c.CodeErp,
                    c.Material,
                    c.TimeToFull,
                    c.Image,
                    c.Description,
                    c.WasteType,
                    c.Company,
                    c.AssetCategory,
                    c.ModifiedDate,
                    c.PrevLevel
                    from Container as c 
                    where c.GeoPoint is not null and c.Active = :Active"
          ;
        try
        {
            var containersRepo = Session
              .CreateQuery(queryStr)
              .SetParameter("Active", true)
              .List();

            List<Container> containers = new List<Container>();

            containers.AddRange(from object[] point in containersRepo
                                select new Container()
                                {
                                    Id = (long)point[0],
                                    Name = (string)point[1],
                                    LastServicedDate = (DateTime)point[2],
                                    MandatoryPickupDate = (DateTime)point[3],
                                    GeoPoint = (Point)_wkt.Read((string)point[4]),
                                    ContainerStatus = (ContainerStatus)point[5],
                                    ContainerCondition = (ContainerCondition)point[6],
                                    Capacity = (ContainerCapacity)point[7],
                                    Level = (int)point[8],
                                    CodeErp = (string)point[9],
                                    Material = (MaterialType)point[10],
                                    TimeToFull = (double)point[11],
                                    Image = (string)point[12],
                                    Description = (string)point[13],
                                    WasteType = (ContainerType)point[14],
                                    Company = (Company)point[15],
                                    AssetCategory = (AssetCategory)point[16],
                                    ModifiedDate = (DateTime)point[17],
                                    PrevLevel = (int)point[18]
                                });

            if (pageNum == -1 & pageSize == -1)
            {
                return new QueryResult<Container>(containers.AsQueryable());
            }

            return new QueryResult<Container>(containers
                  .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                  .Take((int)pageSize).AsQueryable(),
                containers.Count,
                (int)pageSize)
              ;
        }
        catch (Exception e)
        {
            throw new FindAllContainersException(e.Message);
        }
    }

    public QueryResult<Container> FindAllActiveByZoneIdPagedOf(long zoneId, int? pageNum, int? pageSize)
    {
        var queryStr =
            $@"select c.Id, 
                    c.Name,
                    c.LastServicedDate,
                    c.MandatoryPickupDate,
                    NHSP.AsText(c.GeoPoint),
                    c.ContainerStatus,
                    c.ContainerCondition,
                    c.Capacity,
                    c.Level,
                    c.CodeErp,
                    c.Material,
                    c.TimeToFull,
                    c.Image,
                    c.Description,
                    c.WasteType,
                    c.Company,
                    c.AssetCategory,
                    c.ModifiedDate,
                    c.PrevLevel,
                    z.Id,
                    z.Name
                    from Container as c 
                    inner join c.Zone as z
                    where c.GeoPoint is not null 
                    and c.Active = :Active 
                    and z.Id = :ZoneId 
                    and z.Active = :Active"
          ;
        try
        {
            var containersRepo = Session
              .CreateQuery(queryStr)
              .SetParameter("ZoneId", zoneId)
              .SetParameter("Active", true)
              .List();

            List<Container> containers = new List<Container>();

            containers.AddRange(from object[] objects in containersRepo
                                select new Container()
                                {
                                    Id = (long)objects[0],
                                    Name = (string)objects[1],
                                    LastServicedDate = (DateTime)objects[2],
                                    MandatoryPickupDate = (DateTime)objects[3],
                                    GeoPoint = (Point)_wkt.Read((string)objects[4]),
                                    ContainerStatus = (ContainerStatus)objects[5],
                                    ContainerCondition = (ContainerCondition)objects[6],
                                    Capacity = (ContainerCapacity)objects[7],
                                    Level = (int)objects[8],
                                    CodeErp = (string)objects[9],
                                    Material = (MaterialType)objects[10],
                                    TimeToFull = (double)objects[11],
                                    Image = (string)objects[12],
                                    Description = (string)objects[13],
                                    WasteType = (ContainerType)objects[14],
                                    Company = (Company)objects[15],
                                    AssetCategory = (AssetCategory)objects[16],
                                    ModifiedDate = (DateTime)objects[17],
                                    PrevLevel = (int)objects[18],
                                    Zone = new Zone()
                                    {
                                        Id = (long)objects[19],
                                        Name = (string)objects[20],
                                    }
                                });

            if (pageNum == -1 & pageSize == -1)
            {
                return new QueryResult<Container>(containers.AsQueryable());
            }

            return new QueryResult<Container>(containers
                  .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                  .Take((int)pageSize).AsQueryable(),
                containers.Count,
                (int)pageSize)
              ;
        }
        catch (Exception e)
        {
            throw new FindAllContainersException(e.Message);
        }
    }

    public async Task<List<Container>> FindAllActiveByZones(List<long> zones)
    {
        using ITransaction transaction = Session?.BeginTransaction();
        try
        {
            if (Session == null)
                throw new NHibernateSessionTransactionNotExistException("Session == null");
            if (!Session.GetCurrentTransaction()!.IsActive)
                throw new NHibernateSessionTransactionNotActiveException("!Session.GetCurrentTransaction()!.IsActive");

            StringBuilder zonesStr = new StringBuilder();

            int lastZoneIndex = zones.Count;

            foreach (long zone in zones)
            {
                zonesStr.Append(zone);
                if (zone != zones[lastZoneIndex - 1])
                    zonesStr.Append(", ");
            }

            string selectQuery = "SELECT " +
                                 $"Distinct (C.id), " +
                                 $"A.name, " +
                                 $"C.lastserviceddate, " +
                                 $"C.mandatorypickupdate, " +
                                 $"ST_X(C.point), ST_Y(C.point), " +
                                 $"C.status," +
                                 $" C.binstatus," +
                                 $" C.capacity, " +
                                 $"c.level, " +
                                 $"A.codeerp, " +
                                 $"C.material, " +
                                 $"C.timetofull, " +
                                 $"A.image, " +
                                 $"A.description, " +
                                 $"C.wastetype, " +
                                 $"A.\"companyId\", " +
                                 $"A.assetcategory_id, " +
                                 $"A.modified_date, " +
                                 $"C.prevlevel, " +
                                 $"C.\"zoneId\" " +
                                 $"FROM \"Container\" AS C " +
                                 $"INNER JOIN \"Asset\" AS A on A.id = C.id " +
                                 $"INNER JOIN \"Zones\" AS Z on Z.id = ANY('{{{zonesStr.ToString()}}}') " +
                                 $"WHERE " +
                                 $"C.point IS NOT NULL " +
                                 $"AND C.active = 'true' " +
                                 $"AND A.active = 'true'" +
                                 $";";
            ;

            var query = Session.CreateSQLQuery(selectQuery);

            List<Container> containers = new List<Container>();
            try
            {
                IList<object[]> results = query.List<object[]>();
                transaction?.CommitAsync();

                containers.AddRange(from object[] objects in results
                                    select new Container()
                                    {
                                        Id = Convert.ToInt64(objects[0]),
                                        Name = (string)objects[1],
                                        LastServicedDate = (DateTime)objects[2],
                                        MandatoryPickupDate = (DateTime)objects[3],
                                        GeoPoint = new Point((double)(objects[4] == null ? 23.80591666666667 : objects[4]),
                                            (double)(objects[5] == null ? 38.01784500000001 : objects[5])),
                                        ContainerStatus = (ContainerStatus)objects[6],
                                        ContainerCondition = (ContainerCondition)objects[7],
                                        Capacity = (ContainerCapacity)objects[8],
                                        Level = (int)objects[9],
                                        CodeErp = (string)objects[10],
                                        Material = (MaterialType)objects[11],
                                        TimeToFull = (double)objects[12],
                                        Image = (string)objects[13],
                                        Description = (string)objects[14],
                                        WasteType = (ContainerType)objects[15],
                                        Company = new Company()
                                        {
                                            Id = Convert.ToInt64(objects[16])
                                        },
                                        AssetCategory = new AssetCategory()
                                        {
                                            Id = Convert.ToInt64(objects[17])
                                        },
                                        ModifiedDate = (DateTime)objects[18],
                                        PrevLevel = (int)objects[19],
                                        Zone = new Zone()
                                        {
                                            Id = Convert.ToInt64(objects[20]),
                                        }
                                    });

                await Session.FlushAsync();
                return containers.ToList();
            }
            catch (Exception e)
            {
                transaction?.RollbackAsync();
                throw new NHibernateSessionTransactionFailedException(e.Message);
            }
        }
        catch (Exception ex)
        {
            transaction?.RollbackAsync();
            throw new NHibernateSessionTransactionFailedException(ex.Message);
        }
    }

    public List<ContainerProjection> SearchWithCriteria(List<long> zones, string criteria)
    {
        var subCriteria = DetachedCriteria.For<Zone>();
        subCriteria.Add(Expression.Eq("Active", true));
        subCriteria.Add(Expression.In("Id", zones));
        subCriteria.SetProjection(Projections.Id());


        var criteriaBuilder = Session.CreateCriteria(typeof(Container))
            .SetProjection(Projections.ProjectionList()
                .Add(SpatialProjections.Collect("GeoPoint"))
            );

        var conjunction = Restrictions.Conjunction();
        var disjunction = Restrictions.Disjunction();

        if (!String.IsNullOrEmpty(criteria))
        {
            disjunction.Add(Restrictions.InsensitiveLike("Name", criteria, MatchMode.Anywhere));
            disjunction.Add(Restrictions.InsensitiveLike("Description", criteria, MatchMode.Anywhere));

            disjunction.Add(Restrictions.IsNotNull("GeoPoint"));
            disjunction.Add(SpatialRestrictions.IsValid("GeoPoint"));
        }

        conjunction.Add(Expression.Eq("Active", true));

        conjunction.Add(Subqueries.PropertyIn("Zone", subCriteria));

        criteriaBuilder.Add(disjunction);
        criteriaBuilder.Add(conjunction);
        criteriaBuilder.SetCacheable(true);
        criteriaBuilder.SetCacheMode(CacheMode.Normal);
        return criteriaBuilder.List<ContainerProjection>().ToList();
    }

    public List<Container> SearchBetweenLevel(List<long> zones, int start, int end)
    {
        var subCriteria = DetachedCriteria.For<Zone>();
        subCriteria.Add(Expression.Eq("Active", true));
        subCriteria.Add(Expression.In("Id", zones));
        subCriteria.SetProjection(Projections.Id());

        var criteriaBuilder = Session.CreateCriteria(typeof(Container))
            .SetProjection(SpatialProjections.Collect("GeoPoint"));

        var conjunction = Restrictions.Conjunction();

        conjunction.Add(Expression.Eq("Active", true));

        conjunction.Add(
            Expression.Conjunction()
                .Add(Restrictions.Ge("Level", start))
                .Add(Restrictions.Lt("Level", end))
        );

        conjunction.Add(Subqueries.PropertyIn("Zone", subCriteria));

        criteriaBuilder.Add(conjunction);
        criteriaBuilder.AddOrder(Order.Asc("Name"));
        criteriaBuilder.SetCacheable(true);
        criteriaBuilder.SetCacheMode(CacheMode.Normal);

        criteriaBuilder.List<object>().ToList();

        return criteriaBuilder.List<Container>().ToList();
    }

    public async Task<List<Container>> SearchNativeWithCriteria(List<long> zones, string criteria)
    {
        using ITransaction transaction = Session?.BeginTransaction();
        try
        {
            if (Session == null)
                throw new NHibernateSessionTransactionNotExistException("Session == null");
            if (!Session.GetCurrentTransaction()!.IsActive)
                throw new NHibernateSessionTransactionNotActiveException("!Session.GetCurrentTransaction()!.IsActive");

            StringBuilder zonesStr = new StringBuilder();

            int lastZoneIndex = zones.Count;

            foreach (long zone in zones)
            {
                zonesStr.Append(zone);
                if (zone != zones[lastZoneIndex - 1])
                    zonesStr.Append(", ");
            }

            string selectQuery = "SELECT " +
                                 $"Distinct (C.id), " +
                                 $"A.name, " +
                                 $"C.lastserviceddate, " +
                                 $"C.mandatorypickupdate, " +
                                 $"ST_X(C.point), ST_Y(C.point), " +
                                 $"C.status," +
                                 $" C.binstatus," +
                                 $" C.capacity, " +
                                 $"c.level, " +
                                 $"A.codeerp, " +
                                 $"C.material, " +
                                 $"C.timetofull, " +
                                 $"A.image, " +
                                 $"A.description, " +
                                 $"C.wastetype, " +
                                 $"A.\"companyId\", " +
                                 $"A.assetcategory_id, " +
                                 $"A.modified_date, " +
                                 $"C.prevlevel, " +
                                 $"Z.id, " +
                                 $"Z.name " +
                                 $"FROM \"Container\" AS C " +
                                 $"INNER JOIN \"Asset\" AS A on A.id = C.id " +
                                 $"INNER JOIN \"Zones\" AS Z on Z.id = ANY('{{{zonesStr.ToString()}}}') " +
                                 $"WHERE " +
                                 $"C.point IS NOT NULL " +
                                 $"AND C.active = 'true' " +
                                 $"AND A.active = 'true' " +
                                 $"AND (A.name LIKE '%{criteria}%' OR A.description LIKE '%{criteria}%')" +
                                 $";";
            ;

            var query = Session.CreateSQLQuery(selectQuery);

            List<Container> containers = new List<Container>();
            try
            {
                IList<object[]> results = query.List<object[]>();
                transaction?.CommitAsync();

                containers.AddRange(from object[] objects in results
                                    select new Container()
                                    {
                                        Id = Convert.ToInt64(objects[0]),
                                        Name = (string)objects[1],
                                        LastServicedDate = (DateTime)objects[2],
                                        MandatoryPickupDate = (DateTime)objects[3],
                                        GeoPoint = new Point((double)(objects[4] == null ? 23.80591666666667 : objects[4]),
                                            (double)(objects[5] == null ? 38.01784500000001 : objects[5])),
                                        ContainerStatus = (ContainerStatus)objects[6],
                                        ContainerCondition = (ContainerCondition)objects[7],
                                        Capacity = (ContainerCapacity)objects[8],
                                        Level = (int)objects[9],
                                        CodeErp = (string)objects[10],
                                        Material = (MaterialType)objects[11],
                                        TimeToFull = (double)objects[12],
                                        Image = (string)objects[13],
                                        Description = (string)objects[14],
                                        WasteType = (ContainerType)objects[15],
                                        Company = new Company()
                                        {
                                            Id = Convert.ToInt64(objects[16])
                                        },
                                        AssetCategory = new AssetCategory()
                                        {
                                            Id = Convert.ToInt64(objects[17])
                                        },
                                        ModifiedDate = (DateTime)objects[18],
                                        PrevLevel = (int)objects[19]
                                    });

                await Session.FlushAsync();
                return containers.ToList();
            }
            catch (Exception e)
            {
                transaction?.RollbackAsync();
                throw new NHibernateSessionTransactionFailedException(e.Message);
            }
        }
        catch (Exception ex)
        {
            transaction?.RollbackAsync();
            throw new NHibernateSessionTransactionFailedException(ex.Message);
        }
    }

    public async Task<List<Container>> SearchNativeBetweenLevel(List<long> zones, int start, int end)
    {
        using ITransaction transaction = Session?.BeginTransaction();
        try
        {
            if (Session == null)
                throw new NHibernateSessionTransactionNotExistException("Session == null");
            if (!Session.GetCurrentTransaction()!.IsActive)
                throw new NHibernateSessionTransactionNotActiveException("!Session.GetCurrentTransaction()!.IsActive");

            StringBuilder zonesStr = new StringBuilder();

            int lastZoneIndex = zones.Count;

            foreach (long zone in zones)
            {
                zonesStr.Append(zone);
                if (zone != zones[lastZoneIndex - 1])
                    zonesStr.Append(", ");
            }

            string selectQuery = "SELECT " +
                                 $"Distinct (C.id), " +
                                 $"A.name, " +
                                 $"C.lastserviceddate, " +
                                 $"C.mandatorypickupdate, " +
                                 $"ST_X(C.point), ST_Y(C.point), " +
                                 $"C.status," +
                                 $" C.binstatus," +
                                 $" C.capacity, " +
                                 $"c.level, " +
                                 $"A.codeerp, " +
                                 $"C.material, " +
                                 $"C.timetofull, " +
                                 $"A.image, " +
                                 $"A.description, " +
                                 $"C.wastetype, " +
                                 $"A.\"companyId\", " +
                                 $"A.assetcategory_id, " +
                                 $"A.modified_date, " +
                                 $"C.prevlevel " +
                                 $"FROM \"Container\" AS C " +
                                 $"INNER JOIN \"Asset\" AS A on A.id = C.id " +
                                 $"INNER JOIN \"Zones\" AS Z on Z.id = ANY('{{{zonesStr}}}') " +
                                 $"WHERE " +
                                 $"C.point IS NOT NULL " +
                                 $"AND C.active = 'true' " +
                                 $"AND A.active = 'true' " +
                                 $"AND(C.level >= '{start}' AND C.level <= '{end}')" +
                                 $";";
            ;

            var query = Session.CreateSQLQuery(selectQuery);

            List<Container> containers = new List<Container>();
            try
            {
                IList<object[]> results = query.List<object[]>();
                transaction?.CommitAsync();

                containers.AddRange(from object[] objects in results
                                    select new Container()
                                    {
                                        Id = Convert.ToInt64(objects[0]),
                                        Name = (string)objects[1],
                                        LastServicedDate = (DateTime)objects[2],
                                        MandatoryPickupDate = (DateTime)objects[3],
                                        GeoPoint = new Point((double)(objects[4] == null ? 23.80591666666667 : objects[4]),
                                            (double)(objects[5] == null ? 38.01784500000001 : objects[5])),
                                        ContainerStatus = (ContainerStatus)objects[6],
                                        ContainerCondition = (ContainerCondition)objects[7],
                                        Capacity = (ContainerCapacity)objects[8],
                                        Level = (int)objects[9],
                                        CodeErp = (string)objects[10],
                                        Material = (MaterialType)objects[11],
                                        TimeToFull = (double)objects[12],
                                        Image = (string)objects[13],
                                        Description = (string)objects[14],
                                        WasteType = (ContainerType)objects[15],
                                        Company = new Company()
                                        {
                                            Id = Convert.ToInt64(objects[16])
                                        },
                                        AssetCategory = new AssetCategory()
                                        {
                                            Id = Convert.ToInt64(objects[17])
                                        },
                                        ModifiedDate = (DateTime)objects[18],
                                        PrevLevel = (int)objects[19]
                                    });

                await Session.FlushAsync();
                return containers.ToList();
            }
            catch (Exception e)
            {
                transaction?.RollbackAsync();
                throw new NHibernateSessionTransactionFailedException(e.Message);
            }
        }
        catch (Exception ex)
        {
            transaction?.RollbackAsync();
            throw new NHibernateSessionTransactionFailedException(ex.Message);
        }
    }

    public Container FindActiveById(long id)
    {
        var queryStr =
        $@"select c.Id, 
                        c.Name,
                        c.LastServicedDate,
                        c.MandatoryPickupDate,
                        NHSP.AsText(c.GeoPoint),
                        c.ContainerStatus,
                        c.ContainerCondition,
                        c.Capacity,
                        c.Level,
                        c.CodeErp,
                        c.Material,
                        c.TimeToFull,
                        c.Image,
                        c.Description,
                        c.WasteType,
                        c.Company,
                        c.AssetCategory
                        from Container as c 
                        where c.Id = :Id and c.GeoPoint is not null and c.Active = :Active "
      ;

        try
        {
            var containersRepo = Session
              .CreateQuery(queryStr)
              .SetParameter("Active", true)
              .SetParameter("Id", id)
              .List();

            List<Container> containers = new List<Container>();

            containers.AddRange(from object[] point in containersRepo
                                select new Container()
                                {
                                    Id = (long)point[0],
                                    Name = (string)point[1],
                                    LastServicedDate = (DateTime)point[2],
                                    MandatoryPickupDate = (DateTime)point[3],
                                    GeoPoint = (Point)_wkt.Read((string)point[4]),
                                    ContainerStatus = (ContainerStatus)point[5],
                                    ContainerCondition = (ContainerCondition)point[6],
                                    Capacity = (ContainerCapacity)point[7],
                                    Level = (int)point[8],
                                    CodeErp = (string)point[9],
                                    Material = (MaterialType)point[10],
                                    TimeToFull = (double)point[11],
                                    Image = (string)point[12],
                                    Description = (string)point[13],
                                    WasteType = (ContainerType)point[14],
                                    Company = (Company)point[15],
                                    AssetCategory = (AssetCategory)point[16],
                                });


            return containers.FirstOrDefault();
        }
        catch (Exception e)
        {
            throw new ContainerDoesNotExistException(id);
        }
    }

    public async Task UpdateContainerWithNewPositionById(long containerId, double lat, double lon)
    {
        using ITransaction transaction = Session?.BeginTransaction();
        try
        {
            if (!Session.GetCurrentTransaction()!.IsActive)
                return;
            if (Session.IsNull())
                return;

            string queryStr =
                $"UPDATE \"Container\" SET point = " +
                $"ST_SetSRID(ST_MakePoint({lon.ToString(CultureInfo.InvariantCulture)}," +
                $" {lat.ToString(CultureInfo.InvariantCulture)}), 4326) where id = {containerId}";

            var query = Session!.CreateSQLQuery(queryStr);
            var result = await query.ExecuteUpdateAsync();

            transaction?.CommitAsync();
            await Session.FlushAsync();
        }
        catch (Exception ex)
        {
            transaction?.RollbackAsync();
            throw new NHibernateSessionTransactionFailedException(ex.Message);
        }
    }


    public int FindCountTotal()
    {
        var count = Session
            .CreateCriteria<Container>()
            .Add(Expression.IsNotNull("GeoPoint"))
            .Add(Expression.Eq("Active", true))
            .SetProjection(
                Projections.Count(Projections.Id())
            )
            .UniqueResult<int>();

        return count;
    }

    public int FindCountInZonesTotal(List<long> zones)
    {
        var subCriteria = DetachedCriteria.For<Zone>();
        subCriteria.Add(Expression.Eq("Active", true));
        subCriteria.Add(Expression.In("Id", zones));
        subCriteria.SetProjection(Projections.Id());

        var count = Session
            .CreateCriteria<Container>()
            .Add(Expression.IsNotNull("GeoPoint"))
            .Add(Subqueries.PropertyIn("Zone", subCriteria))
            .Add(Expression.Eq("Active", true))
            .SetProjection(
                Projections.Count(Projections.Id())
            )
            .UniqueResult<int>();

        return count;
    }

    public int FindCountPerContainerType(int type)
    {
        var count = Session
            .CreateCriteria<Container>()
            .Add(Expression.IsNotNull("GeoPoint"))
            .Add(Expression.Eq("WasteType", (ContainerType)type))
            .Add(Expression.Eq("Active", true))
            .SetProjection(
                Projections.Count(Projections.Id())
            )
            .UniqueResult<int>();

        return count;
    }

    public int FindCountPerContainerTypeInZones(int type, List<long> zones)
    {
        var subCriteria = DetachedCriteria.For<Zone>();
        subCriteria.Add(Expression.Eq("Active", true));
        subCriteria.Add(Expression.In("Id", zones));
        subCriteria.SetProjection(Projections.Id());

        var count = Session
            .CreateCriteria<Container>()
            .Add(Expression.IsNotNull("GeoPoint"))
            .Add(Expression.Eq("WasteType", (ContainerType)type))
            .Add(Subqueries.PropertyIn("Zone", subCriteria))
            .Add(Expression.Eq("Active", true))
            .SetProjection(
                Projections.Count(Projections.Id())
            )
            .UniqueResult<int>();

        return count;
    }

    public Container FindOneByNameAndCompanyId(string name, long companyId)
    {
        return
          (Container)
          Session.CreateCriteria(typeof(Container))
            .CreateAlias("Company", "c", JoinType.InnerJoin)
            .Add(Expression.Eq("c.Id", companyId))
            .Add(Expression.Eq("Name", name))
            .Add(Expression.Eq("Active", true))
            .SetCacheable(true)
            .SetCacheMode(CacheMode.Normal)
            .UniqueResult()
          ;
    }

    public Container FindOneByName(string name)
    {
        return
          (Container)
          Session.CreateCriteria(typeof(Container))
            .Add(Expression.Eq("Name", name))
            .Add(Expression.Eq("Active", true))
            .SetCacheable(true)
            .SetCacheMode(CacheMode.Normal)
            .UniqueResult()
          ;
    }

    public async Task<long> FindOneByDeviceImei(string deviceImei)
    {
        using ITransaction transaction = Session?.BeginTransaction();
        try
        {
            if (!Session.GetCurrentTransaction()!.IsActive)
                throw new NHibernateSessionTransactionFailedException("GetCurrentTransaction Not Active");
            if (Session.IsNull())
                throw new NHibernateSessionTransactionFailedException("Session Is Null");

            string selectQuery =
                $"SELECT Distinct(C2.id) " +
                $"FROM \"Device\" AS D " +
                $"INNER JOIN \"Sensor\" S on D.id = S.\"deviceId\" " +
                $"INNER JOIN \"Asset\" A on A.id = S.\"assetId\" " +
                $"INNER JOIN \"Container\" C2 on A.id = C2.id " +
                $"WHERE D.imei = '{deviceImei}'";

            var query = Session!.CreateSQLQuery(selectQuery);


            try
            {
                long containerId = Convert.ToInt64(query.UniqueResult<int>());
                transaction?.CommitAsync();
                await Session.FlushAsync();
                return containerId;
            }
            catch (Exception e)
            {
                transaction?.RollbackAsync();
                throw new NHibernateSessionTransactionFailedException(e.Message);
            }
        }
        catch (Exception ex)
        {
            transaction?.RollbackAsync();
            throw new NHibernateSessionTransactionFailedException(ex.Message);
        }
    }
}// Class: ContainerRepository