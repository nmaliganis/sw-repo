using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.auth.common.dtos.Cqrs.Members
{
    // Queries
    public record SearchMemberByEmailOrLoginQuery(string Email, string Login) 
        : IRequest<BusinessResult<bool>>;
    
}//Namespace : sw.common.dtos.Cqrs.Members
