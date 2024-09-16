using sw.admin.common.dtos.V1.Vms.Persons;
using sw.admin.contracts.ContractRepositories;
using sw.admin.contracts.V1.PersonProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using System.Threading.Tasks;

namespace sw.admin.services.V1.PersonService
{
    public class GetPersonByIdProcessor : IGetPersonByIdProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IPersonRepository _personRepository;

        public GetPersonByIdProcessor(IPersonRepository personRepository, IAutoMapper autoMapper)
        {
            _personRepository = personRepository;
            _autoMapper = autoMapper;
        }

        public async Task<BusinessResult<PersonUiModel>> GetPersonByIdAsync(long id)
        {
            var bc = new BusinessResult<PersonUiModel>(new PersonUiModel());

            var person = _personRepository.FindActiveBy(id);
            if (person is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(id), "Person Id does not exist"));
                return bc;
            }

            var response = _autoMapper.Map<PersonUiModel>(person);
            response.Message = $"Person id: {response.Id} fetched successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }
    }
}
