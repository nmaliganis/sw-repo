using sw.admin.common.dtos.V1.ResourceParameters.Companies;
using sw.admin.common.dtos.V1.Vms.Companies;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.admin.common.dtos.V1.Cqrs.Companies
{
    // Queries
    public record GetCompanyByIdQuery(long Id) : IRequest<BusinessResult<CompanyUiModel>>;

    public class GetCompaniesQuery : GetCompaniesResourceParameters, IRequest<BusinessResult<PagedList<CompanyUiModel>>>
    {
        public GetCompaniesQuery(GetCompaniesResourceParameters parameters) : base()
        {
            Filter = parameters.Filter;
            SearchQuery = parameters.SearchQuery;
            Fields = parameters.Fields;
            OrderBy = parameters.OrderBy;
            SortDirection = parameters.SortDirection;
            PageSize = parameters.PageSize;
            PageIndex = parameters.PageIndex;
        }
    }

    // Commands
    public record CreateCompanyCommand(long CreatedById, string Name, string CodeErp, string Description)
        : IRequest<BusinessResult<CompanyCreationUiModel>>;

    public record UpdateCompanyCommand(long ModifiedById, long Id, string Name, string CodeErp, string Description)
        : IRequest<BusinessResult<CompanyModificationUiModel>>;

    public record DeleteSoftCompanyCommand(long Id, long DeletedBy, string DeletedReason)
        : IRequest<BusinessResult<CompanyDeletionUiModel>>;

    public record DeleteHardCompanyCommand(long Id)
        : IRequest<BusinessResult<CompanyDeletionUiModel>>;
}
