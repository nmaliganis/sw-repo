using sw.asset.repository.Mappings.Devices;
using sw.asset.repository.Mappings.Devices.Simcards;
using sw.infrastructure.Domain;
using System;

namespace sw.asset.model.Devices.Simcards;

public class Simcard : EntityBase<long>, IAggregateRoot
{
    public Simcard()
    {
        this.CardType = SimCardType.SimOnChip;
        this.NetworkType = SimNetworkType.NBIoT;
        this.IsEnabled = true;

        this.PurchaseDate = DateTime.UtcNow.ToUniversalTime();

        this.Active = true;
        this.CreatedDate = DateTime.UtcNow.ToUniversalTime();
        this.ModifiedDate = new DateTime(2000, 1, 1, 20, 0, 0).ToUniversalTime();
        this.ModifiedBy = 0;
        this.DeletedDate = new DateTime(2000, 1, 1, 20, 0, 0).ToUniversalTime();
        this.DeletedBy = 0;
        this.DeletedReason = "No Reason";

        this.Iccid = Guid.NewGuid().ToString();
        this.Imsi = Guid.NewGuid().ToString();
        this.CountryIso = "GRC-300-ISO3166-2:GR";
    }

    public virtual string Iccid { get; set; }
    public virtual string Imsi { get; set; }
    public virtual string CountryIso { get; set; }
    public virtual string Number { get; set; }
    public virtual DateTime PurchaseDate { get; set; }
    public virtual SimCardType CardType { get; set; }
    public virtual SimNetworkType NetworkType { get; set; }
    public virtual bool IsEnabled { get; set; }


    #region Audit --> Attributes
    public virtual bool Active { get; set; }
    public virtual DateTime CreatedDate { get; set; }
    public virtual long CreatedBy { get; set; }
    public virtual DateTime ModifiedDate { get; set; }
    public virtual long ModifiedBy { get; set; }

    public virtual DateTime DeletedDate { get; set; }
    public virtual long DeletedBy { get; set; }
    public virtual string DeletedReason { get; set; }

    #endregion

    public virtual Device Device { get; set; }

    protected override void Validate()
    {
        if (Iccid == string.Empty)
        {
            AddBrokenRule(SimcardBusinessRules.Iccid);
        }
        if (Imsi == string.Empty)
        {
            AddBrokenRule(SimcardBusinessRules.Imsi);
        }
        if (Number == string.Empty)
        {
            AddBrokenRule(SimcardBusinessRules.Number);
        }
    }


    public virtual void InjectWithAudit(long accountIdToCreateThisSimcard)
    {
        this.CreatedBy = accountIdToCreateThisSimcard;
        this.CreatedDate = DateTime.Now;
    }

    public virtual void InjectWithInitialAttributes(string number)
    {
        this.Number = !string.IsNullOrEmpty(number) ? number : string.Empty;
    }

    public virtual void DeleteWithAudit(long deleteCommandDeletedBy, string deleteCommandDeletedReason)
    {
        this.DeletedBy = deleteCommandDeletedBy;
        this.DeletedDate = DateTime.UtcNow;
        this.DeletedReason = deleteCommandDeletedReason;
    }

    public virtual void ModifyWithAudit(long updateCommandModifiedById)
    {
        this.ModifiedBy = updateCommandModifiedById;
        this.ModifiedDate = DateTime.UtcNow;
    }


}// Class : Simcard