using sw.routing.model.Itineraries;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;
using System.Threading.Tasks;
using sw.routing.model.Vehicles;

namespace sw.routing.contracts.ContractRepositories;

public interface IVehicleRepository : IRepository<Vehicle, long>
{
    QueryResult<Vehicle> FindAllActiveVehiclesPagedOf(int? pageNum, int? pageSize);
    int FindCountAllActiveVehicles();

    Vehicle FindVehicleByNumPlate(string numPlate);
}