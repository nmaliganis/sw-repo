using sw.asset.model.Assets;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sw.asset.model.Assets.Vehicles;

namespace sw.asset.repository.Mappings.Assets
{
    public class VehicleMap : SubclassMap<Vehicle>
    {
        public VehicleMap()
        {
            DiscriminatorValue("vehicle");
            this.Table("`Vehicle`");
            Abstract();
            KeyColumn("id");

            this.Map(x => x.NumPlate)
                .Column("numplate")
                .CustomType("String")
                .Access.Property()
                .Generated.Never()
                .Unique()
                .CustomSqlType("varchar(16)")
                .Not.Nullable()
                .Length(16)
                ;

            this.Map(x => x.Brand)
                .Column("brand")
                .CustomType("String")
                .Access.Property()
                .Generated.Never()
                .CustomSqlType("varchar(32)")
                .Not.Nullable()
                .Length(32)
                ;

            this.Map(x => x.RegisteredDate)
                .Column("registereddate")
                .Access.Property()
                .Generated.Never()
                .Not.Nullable()
                ;

            Map(x => x.Type)
                .Column("type")
                .CustomType<VehicleType>()
                .Access.Property()
                .Generated.Never()
                .Default(@"1")
                .CustomSqlType("integer")
                .Not.Nullable()
                ;

            Map(x => x.Status)
                .Column("status")
                .CustomType<VehicleStatus>()
                .Access.Property()
                .Generated.Never()
                .Default(@"1")
                .CustomSqlType("integer")
                .Not.Nullable()
                ;

            Map(x => x.Gas)
                .Column("Gas")
                .CustomType<GasType>()
                .Access.Property()
                .Generated.Never()
                .Default(@"1")
                .CustomSqlType("integer")
                .Not.Nullable()
                ;

            this.Map(x => x.Height)
                .Column("height")
                .CustomType("double")
                .Access.Property()
                .Generated.Never()
                .Nullable()
                ;

            this.Map(x => x.Width)
                .Column("width")
                .CustomType("double")
                .Access.Property()
                .Generated.Never()
                .Nullable()
                ;

            this.Map(x => x.Axels)
                .Column("axels")
                .CustomType("double")
                .Access.Property()
                .Generated.Never()
                .Nullable()
                ;

            this.Map(x => x.MinTurnRadius)
                .Column("min_turn_radius")
                .CustomType("double")
                .Access.Property()
                .Generated.Never()
                .Nullable()
                ;

            this.Map(x => x.Length)
                .Column("length")
                .CustomType("double")
                .Access.Property()
                .Generated.Never()
                .Nullable()
                ;
        }
    } // Class: VehicleMap
}