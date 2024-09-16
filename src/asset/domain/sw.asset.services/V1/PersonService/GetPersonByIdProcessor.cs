using sw.asset.common.dtos.Cqrs.Persons;
using sw.asset.common.dtos.Vms.Persons;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.PersonProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.TypeMappings;
using MediatR;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace sw.asset.services.V1.PersonService;

public class GetPersonByIdProcessor :
  IGetPersonByIdProcessor, IRequestHandler<GetPersonByIdQuery, BusinessResult<PersonUiModel>>
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

        var person = this._personRepository.FindOneActiveById(id);
        if (person.IsNull())
        {
            Log.Information(
              $"--Method:GetPersonByIdAsync -- Message:Person_FETCH" +
              $" -- Datetime:{DateTime.Now} -- Just After : _PersonRepository.FindBy");
            bc.AddBrokenRule(BusinessError.CreateInstance(nameof(id), "Person Id does not exist"));
            return bc;
        }

        var response = this._autoMapper.Map<PersonUiModel>(person);
        response.Message = $"Person id: {response.Id} fetched successfully";

        bc.Model = response;

        return await Task.FromResult(bc);
    }
    public async Task<BusinessResult<PersonUiModel>> Handle(GetPersonByIdQuery qry, CancellationToken cancellationToken)
    {
        return await this.GetPersonByIdAsync(qry.Id);
    }
}