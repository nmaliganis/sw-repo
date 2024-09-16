using sw.localization.common.dtos.Cqrs.LocalizationValues;
using sw.localization.common.dtos.Vms.LocalizationValues;
using OneOf;
using System;
using System.Threading.Tasks;

namespace sw.localization.contracts.V1.LocalizationValueProcessors
{
    public interface IUpdateLocalizationValueProcessor
    {
        Task<OneOf<LocalizationValueModificationUiModel, Exception>> UpdateLocalizationValueAsync(UpdateLocalizationValueCommand updateCommand);
    }
}