using System.Threading;
using System.Threading.Tasks;
using sw.auth.common.dtos.Cqrs.Members;
using sw.auth.contracts.ContractRepositories;
using sw.auth.contracts.V1.Members;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using MediatR;

namespace sw.auth.services.V1.Members
{
    public class SearchMemberByEmailOrLoginProcessor : ISearchMemberByEmailOrLoginProcessor,
        IRequestHandler<SearchMemberByEmailOrLoginQuery, BusinessResult<bool>>
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IMemberRepository _memberRepository;
        public SearchMemberByEmailOrLoginProcessor(IMemberRepository memberRepository, IAutoMapper autoMapper)
        {
            _memberRepository = memberRepository;
            _autoMapper = autoMapper;
        }

        public async Task<BusinessResult<bool>> SearchIfAnyMemberByEmailOrLoginExistsAsync(string email, string login)
        {
            var bc = new BusinessResult<bool>(false)
            {
                Model = _memberRepository.FindMembersByEmailOrLogin(email, login).Count > 0
            };

            return await Task.FromResult(bc);
        }

        public async Task<BusinessResult<bool>> Handle(SearchMemberByEmailOrLoginQuery qry, CancellationToken cancellationToken)
        {
            return await SearchIfAnyMemberByEmailOrLoginExistsAsync(qry.Email, qry.Login);
        }
    }
}
