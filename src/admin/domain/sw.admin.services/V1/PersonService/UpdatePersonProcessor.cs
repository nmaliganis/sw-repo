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
    public class UpdatePersonProcessor : IUpdatePersonProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IPersonRepository _personRepository;
        private readonly IAutoMapper _autoMapper;
        public UpdatePersonProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
            IPersonRepository personRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _personRepository = personRepository;
        }

        public async Task<BusinessResult<PersonModificationUiModel>> UpdatePersonAsync(UpdatePersonCommand updateCommand)
        {
            var bc = new BusinessResult<PersonModificationUiModel>(new PersonModificationUiModel());

            if (updateCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var person = _personRepository.FindBy(updateCommand.Id);
            if (person is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "Person Id does not exist"));
                return bc;
            }

            var modifiedPerson = _autoMapper.Map<Person>(updateCommand);
            person.Modified(updateCommand.ModifiedById, modifiedPerson);

            Persist(person, updateCommand.Id);

            var response = _autoMapper.Map<PersonModificationUiModel>(person);
            response.Message = $"Person id: {response.Id} updated successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        private void Persist(Person person, long id)
        {
            _personRepository.Save(person, id);
            _uOf.Commit();
        }
    }
}
