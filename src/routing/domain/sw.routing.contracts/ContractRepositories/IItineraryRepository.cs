using sw.routing.model.Itineraries;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;

namespace sw.routing.contracts.ContractRepositories;

public interface IItineraryRepository : IRepository<Itinerary, long>
{
    QueryResult<Itinerary> FindAllActiveItinerariesPagedOf(int? pageNum, int? pageSize);
    int FindCountAllActiveItineraries();

    Itinerary FindItineraryByName(string name);
}