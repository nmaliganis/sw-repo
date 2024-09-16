using sw.routing.model.Itineraries;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;
using System.Threading.Tasks;
using sw.routing.model.Drivers;

namespace sw.routing.contracts.ContractRepositories;

public interface IDriverRepository : IRepository<Driver, long>
{
    QueryResult<Driver> FindAllActiveDriversPagedOf(int? pageNum, int? pageSize);
    int FindCountAllActiveDrivers();

    Driver FindDriverByEmail(string email);
}