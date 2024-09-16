using sw.asset.model.Devices;
using sw.asset.model.Devices.Simcards;
using sw.asset.repository.Mappings.Base.CustomeTypes;
using FluentNHibernate.Mapping;

namespace sw.asset.repository.Mappings.Devices;

public class DeviceMap : ClassMap<Device>
{
    public DeviceMap()
    {
        this.Id(x => x.Id)
            .Column("id")
            .CustomType("long")
            .Access.Property()
            .Not.Nullable()
            .GeneratedBy
            .Identity()
            ;

        this.Map(x => x.Imei)
            .Column("imei")
            .CustomType("String")
            .Unique()
            .Access.Property()
            .Generated.Never()
            .CustomSqlType("varchar(250)")
            .Not.Nullable()
            .Length(250)
            ;

        this.Map(x => x.SerialNumber)
            .Column("serialnumber")
            .CustomType("String")
            .Unique()
            .Access.Property()
            .Generated.Never()
            .CustomSqlType("varchar(250)")
            .Not.Nullable()
            .Length(250)
            ;

        this.Map(x => x.ActivationCode)
            .Column("activationcode")
            .CustomType("String")
            .Unique()
            .Access.Property()
            .Generated.Never()
            .Not.Nullable()
            ;

        this.Map(x => x.ActivationBy)
            .Column("activationby")
            .CustomType("long")
            .Access.Property()
            .Generated.Never()
            .Not.Nullable()
            ;

        this.Map(x => x.ActivationDate)
            .Column("activationdate")
            .Access.Property()
            .Generated.Never()
            .Not.Nullable()
            ;

        this.Map(x => x.ProvisioningCode)
            .Column("provisioningcode")
            .CustomType("String")
            .Unique()
            .Access.Property()
            .Generated.Never()
            .Not.Nullable()
            ;

        this.Map(x => x.ProvisioningBy)
            .Column("provisioningby")
            .CustomType("long")
            .Access.Property()
            .Generated.Never()
            .Not.Nullable()
            ;

        this.Map(x => x.ProvisioningDate)
            .Column("provisioningdate")
            .Access.Property()
            .Generated.Never()
            .Not.Nullable()
            ;

        this.Map(x => x.ResetCode)
            .Column("resetcode")
            .CustomType("String")
            .Unique()
            .Access.Property()
            .Generated.Never()
            .Not.Nullable()
            ;

        this.Map(x => x.ResetBy)
            .Column("resetby")
            .CustomType("long")
            .Access.Property()
            .Generated.Never()
            .Not.Nullable()
            ;

        this.Map(x => x.ResetDate)
            .Column("resetdate")
            .Access.Property()
            .Generated.Never()
            .Not.Nullable()
            ;


        this.Map(x => x.Activated)
            .Column("activated")
            .CustomType("Boolean")
            .Access.Property()
            .Generated.Never()
            .Default("false")
            .CustomSqlType("boolean")
            .Not.Nullable()
            ;

        this.Map(x => x.Enabled)
            .Column("enabled")
            .CustomType("Boolean")
            .Access.Property()
            .Generated.Never()
            .Default("false")
            .CustomSqlType("boolean")
            .Not.Nullable()
            ;

        Map(x => x.IpAddress)
            .Column("ipAddress")
            .CustomSqlType("inet")
            .CustomType<IpAddressUserType>()
            .Unique()
            ;

        this.Map(x => x.LastRecordedDate)
            .Column("lastrecordeddate")
            .Access.Property()
            .Generated.Never()
            .Not.Nullable()
            ;

        this.Map(x => x.LastReceivedDate)
            .Column("lastreceiveddate")
            .Access.Property()
            .Generated.Never()
            .Not.Nullable()
            ;

        this.Map(x => x.Notes)
            .Column("notes")
            .Access.Property()
            .Generated.Never()
            .Nullable()
            ;

        this.Map(x => x.CodeErp)
            .Column("`codeErp`")
            .Access.Property()
            .Generated.Never()
            .Nullable()
            ;

        this.Map(x => x.CreatedBy)
            .Column("created_by")
            .CustomType("long")
            .Access.Property()
            .Generated.Never()
            .Not.Nullable()
            ;

        this.Map(x => x.ModifiedBy)
            .Column("modified_by")
            .CustomType("long")
            .Access.Property()
            .Generated.Never()
            .Not.Nullable()
            ;

        this.Map(x => x.DeletedBy)
            .Column("deleted_by")
            .CustomType("long")
            .Access.Property()
            .Generated.Never()
            .Not.Nullable()
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

        this.References(x => x.DeviceModel)
            .Class<DeviceModel>()
            .Access.Property()
            .Cascade.SaveUpdate()
            .LazyLoad()
            .Columns("`deviceModelId`")
            ;

        this.HasOne(x => x.Simcard)
          .Class<Simcard>()
          .Access.Property()
          .Cascade.SaveUpdate()
          .LazyLoad()
          .PropertyRef(p => p.Device)
          ;

        this.HasMany(x => x.Sensors)
            .Access.Property()
            .AsSet()
            .Cascade.SaveUpdate()
            .LazyLoad()
            .Inverse()
            .Generic()
            .KeyColumns.Add("`deviceId`", mapping =>
                mapping.Name("`deviceId`")
                    .Not.Nullable())
            ;
    }
}// Class: DeviceMap