using sw.asset.model.Devices;
using sw.asset.model.Devices.Simcards;
using FluentNHibernate.Mapping;

namespace sw.asset.repository.Mappings.Devices;

public class SimcardMap : ClassMap<Simcard>
{
  public SimcardMap()
  {
    Table(@"`SimCard`");
    Id(x => x.Id)
      .Column("id")
      .CustomType("long")
      .Access.Property()
      .Not.Nullable()
      .GeneratedBy
      .Identity()
      ;

    Map(x => x.Iccid)
      .Column("iccid")
      .Access.Property()
      .Generated.Never()
      .Not.Nullable()
      ;

    Map(x => x.Imsi)
      .Column("imsi")
      .Access.Property()
      .Generated.Never()
      .Not.Nullable()
      ;

    Map(x => x.CountryIso)
      .Column("country_iso")
      .Access.Property()
      .Generated.Never()
      .Not.Nullable()
      ;

    Map(x => x.Number)
      .Column("number")
      .Access.Property()
      .Generated.Never()
      .Not.Nullable()
      .Unique()
      ;

    Map(x => x.PurchaseDate)
      .Column("purchase_date")
      .Access.Property()
      .Generated.Never()
      .Not.Nullable()
      ;

    Map(x => x.CardType)
      .Column("simcardtype")
      .CustomType<SimCardType>()
      .Access.Property()
      .Generated.Never()
      .Default(@"1")
      .Not.Nullable()
      ;

    Map(x => x.NetworkType)
      .Column("simnetworktype")
      .CustomType<SimNetworkType>()
      .Access.Property()
      .Generated.Never()
      .Default(@"1")
      .Not.Nullable()
      ;

    Map(x => x.IsEnabled)
      .Column("enable")
      .CustomType("Boolean")
      .Access.Property()
      .Generated.Never()
      .Default("true")
      .CustomSqlType("boolean")
      .Not.Nullable()
      ;

    this.Map(x => x.CreatedBy)
      .Column("created_by")
      .Access.Property()
      .Generated.Never()
      .Nullable()
      ;

    this.Map(x => x.ModifiedBy)
      .Column("modified_by")
      .Access.Property()
      .Generated.Never()
      .Nullable()
      ;

    this.Map(x => x.DeletedBy)
      .Column("deleted_by")
      .Access.Property()
      .Generated.Never()
      .Nullable()
      ;

    this.Map(x => x.CreatedDate)
      .Column("created_date")
      .Access.Property()
      .Generated.Never()
      .Not.Nullable()
      ;

    this.Map(x => x.ModifiedDate)
      .Column("modified_date")
      .Access.Property()
      .Generated.Never()
      .Not.Nullable()
      ;

    this.Map(x => x.DeletedDate)
      .Column("deleted_date")
      .Access.Property()
      .Generated.Never()
      .Not.Nullable()
      ;

    this.Map(x => x.DeletedReason)
      .Column("deleted_reason")
      .Access.Property()
      .Generated.Never()
      .Nullable()
      ;
    this.Map(x => x.Active)
      .Column("active")
      .CustomType("Boolean")
      .Access.Property()
      .Generated.Never()
      .Default("true")
      .CustomSqlType("boolean")
      .Not.Nullable()
      ;

        //HasOne
        References(x => x.Device)
      .Class<Device>()
      .Access.Property()
      .Cascade.SaveUpdate()
      .LazyLoad()
      .Columns("`deviceId`")
      ;
  }
}//Class : SimcardMap