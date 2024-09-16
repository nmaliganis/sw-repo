using sw.infrastructure.Domain;

namespace sw.asset.repository.Mappings.Devices.Simcards;

public class SimcardBusinessRules
{
    public static BusinessRule Iccid => new BusinessRule("SIM", "Sim Iccid must not be null or empty!");
    public static BusinessRule Imsi => new BusinessRule("SIM", "Sim Imsi must not be null or empty!");
    public static BusinessRule Number => new BusinessRule("SIM", "Sim Number must not be null or empty!");
}