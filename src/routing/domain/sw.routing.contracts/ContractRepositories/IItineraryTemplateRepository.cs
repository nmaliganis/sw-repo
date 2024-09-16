using sw.routing.model.Itineraries;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;
using System.Threading.Tasks;
using sw.routing.model.ItineraryTemplates;

namespace sw.routing.contracts.ContractRepositories;

public interface IItineraryTemplateRepository : IRepository<ItineraryTemplate, long>
{
    QueryResult<ItineraryTemplate> FindAllActiveItineraryTemplatesByPagedOf(int? pageNum, int? pageSize);
    QueryResult<ItineraryTemplate> FindAllActiveItineraryTemplatesPagedOf(int? pageNum, int? pageSize);
    int FindCountAllActiveItineraries();

    Task CreateItineraryTemplate(ItineraryTemplate itineraryTemplateToBeCreated);

    ItineraryTemplate FindItineraryTemplateByName(string name);
}