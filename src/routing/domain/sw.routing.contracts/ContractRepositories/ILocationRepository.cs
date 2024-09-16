using sw.routing.model.Itineraries;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;
using System.Threading.Tasks;
using sw.routing.model.ItineraryTemplates.LocationPoints;

namespace sw.routing.contracts.ContractRepositories;

public interface ILocationRepository : IRepository<LocationPoint, long>
{
    QueryResult<LocationPoint> FindAllActiveLocationsPagedOf(int? pageNum, int? pageSize);
    int FindCountAllActiveLocations();

    Task CreateLocation(LocationPoint locationToBeCreated);

    LocationPoint FindOneLocationByName(string name);
}