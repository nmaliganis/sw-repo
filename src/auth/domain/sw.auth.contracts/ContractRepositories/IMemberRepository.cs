using System.Collections.Generic;
using sw.auth.model.Members;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;

namespace sw.auth.contracts.ContractRepositories
{
    public interface IMemberRepository : IRepository<Member, long>
    {
        QueryResult<Member> FindAllActiveMembersPagedOf(int? pageNum, int? pageSize);
        int FindCountAllActiveMembers();
        Member FindMemberByName(string lastname, string firstname);

        Member FindMemberByEmail(string email);
        IList<Member> FindMembersByEmailOrLogin(string email, string login);
    }
}