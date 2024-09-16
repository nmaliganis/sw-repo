using System.Collections.Generic;
using sw.asset.common.dtos.ResourceParameters.Companies;
using sw.asset.common.dtos.Vms.Companies;
using sw.asset.common.dtos.Vms.Companies.Zones;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.asset.common.dtos.Cqrs.Companies
{
    // Queries
    public record GetCompanyByIdQuery(long Id) : IRequest<BusinessResult<CompanyUiModel>>;
    public class GetZonesByCompanyIdQuery : GetZonesResourceParameters, IRequest<BusinessResult<PagedList<ZoneUiModel>>>
    {
        public long CompanyId { get; set; }

        public GetZonesByCompanyIdQuery(long companyId, GetZonesResourceParameters parameters) : base()
        {
            CompanyId = companyId;

            Filter = parameters.Filter;
            SearchQuery = parameters.SearchQuery;
            Fields = parameters.Fields;
            OrderBy = parameters.OrderBy;
            SortDirection = parameters.SortDirection;
            PageSize = parameters.PageSize;
            PageIndex = parameters.PageIndex;
        }
    }

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

    public record UpdateCompanyWithZoneCommand(long ModifiedById, long CompanyId, List<string> Zones)
        : IRequest<BusinessResult<CompanyModificationUiModel>>;


    public record DeleteSoftCompanyCommand(long Id, long DeletedBy, string DeletedReason)
        : IRequest<BusinessResult<CompanyDeletionUiModel>>;

    public record DeleteHardCompanyCommand(long Id)
        : IRequest<BusinessResult<CompanyDeletionUiModel>>;
}
