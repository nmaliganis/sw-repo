﻿using sw.landmark.common.dtos.V1.Cqrs.GeocoderProfiles;
using sw.landmark.common.dtos.V1.Vms.GeocoderProfiles;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using System.Threading.Tasks;

namespace sw.landmark.contracts.V1.GeocoderProfileProcessors
{
    public interface IGetGeocoderProfilesProcessor
    {
        Task<BusinessResult<PagedList<GeocoderProfileUiModel>>> GetGeocoderProfilesAsync(GetGeocoderProfilesQuery qry);
    }
}
