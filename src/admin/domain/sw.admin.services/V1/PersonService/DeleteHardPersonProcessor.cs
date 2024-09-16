using sw.admin.common.dtos.V1.Cqrs.Persons;
using sw.admin.common.dtos.V1.Vms.Persons;
using sw.admin.contracts.ContractRepositories;
using sw.admin.contracts.V1.PersonProcessors;
using sw.admin.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using System.Threading.Tasks;

namespace sw.admin.services.V1.PersonService
{
    public class DeleteHardPersonProcessor : IDeleteHardPersonProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IPersonRepository _personRepository;

        public DeleteHardPersonProcessor(IUnitOfWork uOf, IPersonRepository personRepository)
        {
            _uOf = uOf;
            _personRepository = personRepository;
        }

        public async Task<BusinessResult<PersonDeletionUiModel>> DeleteHardPersonAsync(DeleteHardPersonCommand deleteCommand)
        {
            var bc = new BusinessResult<PersonDeletionUiModel>(new PersonDeletionUiModel());

            if (deleteCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var person = _personRepository.FindBy(deleteCommand.Id);
            if (person is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "Person Id does not exist"));
                return bc;
            }

            Persist(person);

            bc.Model.Id = deleteCommand.Id;
            bc.Model.Successful = true;
            bc.Model.Hard = true;
            bc.Model.Message = $"Person with id: {deleteCommand.Id} has been hard deleted successfully.";

            return await Task.FromResult(bc);
        }

        private void Persist(Person person)
        {
            _personRepository.Remove(person);
            _uOf.Commit();
        }
    }
}
