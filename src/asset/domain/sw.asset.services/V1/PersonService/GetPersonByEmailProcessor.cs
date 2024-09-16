using System;
using sw.asset.common.dtos.Cqrs.Persons;
using sw.asset.common.dtos.Vms.Persons;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.PersonProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.TypeMappings;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using sw.asset.model.Persons;
using Serilog;

namespace sw.asset.services.V1.PersonService;

public class GetPersonByEmailProcessor :
  IGetPersonByEmailProcessor,
  IRequestHandler<GetPersonByEmailQuery, BusinessResult<PersonUiModel>>
{
    private readonly IAutoMapper _autoMapper;
    private readonly IPersonRepository _personRepository;
    public GetPersonByEmailProcessor(IPersonRepository personRepository, IAutoMapper autoMapper)
    {
        this._personRepository = personRepository;
        this._autoMapper = autoMapper;
    }

    public async Task<BusinessResult<PersonUiModel>> GetPersonByEmailAsync(string email)
    {
        var bc = new BusinessResult<PersonUiModel>(new PersonUiModel());

        try
        {
            Person person = _personRepository.FindOneByEmail(email);
            if (person.IsNull())
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(email), "Person Email does not exist"));
                return bc;
            }

            var response = _autoMapper.Map<PersonUiModel>(person);
            response.Message = $"Person Email: {response.Id} fetched successfully";
            bc.Model = response;
        }
        catch (Exception e)
        {
            string errorMessage = "ERROR_INVALID_PERSON_BY_EMAIL_MODEL";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
                $"Get Person ByEmail: {email}" +
                $"Error Message:{errorMessage}" +
                "--GetPersonByEmailAsync--  @NotComplete@ [GetPersonByEmailProcessor]. " +
                $"Broken rules: {e.Message}");
        }

        return await Task.FromResult(bc);
    }

    public async Task<BusinessResult<PersonUiModel>> Handle(GetPersonByEmailQuery qry, CancellationToken cancellationToken)
    {
        return await this.GetPersonByEmailAsync(qry.Email);
    }
}