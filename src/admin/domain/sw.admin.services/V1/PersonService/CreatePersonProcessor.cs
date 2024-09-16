using sw.admin.common.dtos.V1.Cqrs.Persons;
using sw.admin.common.dtos.V1.Vms.Persons;
using sw.admin.contracts.ContractRepositories;
using sw.admin.contracts.V1.PersonProcessors;
using sw.admin.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using System.Threading.Tasks;

namespace sw.admin.services.V1.PersonService
{
    public class CreatePersonProcessor : ICreatePersonProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IPersonRepository _personRepository;
        private readonly IAutoMapper _autoMapper;

        public CreatePersonProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IPersonRepository personRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _personRepository = personRepository;
        }

        public async Task<BusinessResult<PersonCreationUiModel>> CreatePersonAsync(CreatePersonCommand createCommand)
        {
            var bc = new BusinessResult<PersonCreationUiModel>(new PersonCreationUiModel());

            if (createCommand is null)
            {
                bc.Model = null;
                bc.AddBrokenRule(new BusinessError(null));
                return await Task.FromResult(bc);
            }

            var nameExists = _personRepository.FindPersonByEmail(createCommand.Parameters.Email);
            if (nameExists != null)
            {
                bc.Model = null;
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(createCommand.Parameters.Email), "ERROR_EMAIL_ALREADY_EXISTS"));
                return await Task.FromResult(bc);

            }

            var person =_autoMapper.Map<Person>(createCommand);
            person.Created(createCommand.CreatedById);

            Persist(person);

            var response = _autoMapper.Map<PersonCreationUiModel>(person);
            response.Message = "Person created successfully.";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        private void Persist(Person person)
        {
            _personRepository.Add(person);
            _uOf.Commit();
        }
    }
}
