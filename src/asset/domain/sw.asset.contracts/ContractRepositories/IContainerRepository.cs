using sw.asset.model.Assets.Containers;
using sw.asset.model.Assets.Containers.Projections;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;

namespace sw.asset.contracts.ContractRepositories;

public interface IContainerRepository : IRepository<Container, long>
{
	QueryResult<Container> FindAllActivePagedOf(int? pageNum, int? pageSize);
	QueryResult<Container> FindAllActiveByZoneIdPagedOf(long zoneId, int? pageNum, int? pageSize);
	Task<List<Container>> FindAllActiveByZones(List<long> zones);


    List<ContainerProjection> SearchWithCriteria(List<long> zones, string criteria);
    List<Container> SearchBetweenLevel(List<long> zones, int start, int end);


    Task<List<Container>> SearchNativeWithCriteria(List<long> zones, string criteria);
    Task<List<Container>> SearchNativeBetweenLevel(List<long> zones, int start, int end);


    Container FindActiveById(long id);
	Task UpdateContainerWithNewPositionById(long containerId, double lat, double lon);
	int FindCountTotal();
	int FindCountInZonesTotal(List<long> zones);
	int FindCountPerContainerType(int type);
	int FindCountPerContainerTypeInZones(int type, List<long> zones);
	Container FindOneByNameAndCompanyId(string name, long companyId);
	Container FindOneByName(string name);
	Task<long> FindOneByDeviceImei(string deviceImei);

}// Class: IContainerRepository