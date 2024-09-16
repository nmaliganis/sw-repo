using sw.auth.common.dtos.ResourceParameters.Companies;
using sw.auth.common.dtos.Vms.Companies;
using sw.common.dtos.Vms.Companies;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.auth.common.dtos.Cqrs.Companies {
    // Queries
    public record GetCompanyByIdQuery(long Id) : IRequest<BusinessResult<CompanyUiModel>>;

    public class GetCompaniesQuery : GetCompaniesResourceParameters, IRequest<BusinessResult<PagedList<CompanyUiModel>>> {
        public GetCompaniesQuery(GetCompaniesResourceParameters parameters) : base() {
            this.Filter = parameters.Filter;
            this.SearchQuery = parameters.SearchQuery;
            this.Fields = parameters.Fields;
            this.OrderBy = parameters.OrderBy;
            this.SortDirection = parameters.SortDirection;
            this.PageSize = parameters.PageSize;
            this.PageIndex = parameters.PageIndex;
        }
    }


    public class GetCompaniesByUserQuery : GetCompaniesResourceParameters, IRequest<BusinessResult<PagedList<CompanyUiModel>>> {
        public GetCompaniesByUserQuery(long userId, GetCompaniesResourceParameters parameters) : base() {
            this.Filter = parameters.Filter;
            this.SearchQuery = parameters.SearchQuery;
            this.Fields = parameters.Fields;
            this.OrderBy = parameters.OrderBy;
            this.SortDirection = parameters.SortDirection;
            this.PageSize = parameters.PageSize;
            this.PageIndex = parameters.PageIndex;
        }
    }

    // Commands
    public record CreateCompanyCommand(long CreatedById, string Name, string CodeErp, string Description)
        : IRequest<BusinessResult<CompanyUiModel>>;

    public record UpdateCompanyCommand(long ModifiedById, long Id, string Name, string CodeErp, string Description)
        : IRequest<BusinessResult<CompanyModificationUiModel>>;

    public record DeleteSoftCompanyCommand(long CompanyIdToBeDeleted, long DeletedBy, string DeletedReason)
        : IRequest<BusinessResult<CompanyDeletionUiModel>>;

    public record DeleteHardCompanyCommand(long CompanyIdToBeDeleted)
        : IRequest<BusinessResult<CompanyDeletionUiModel>>;
}
