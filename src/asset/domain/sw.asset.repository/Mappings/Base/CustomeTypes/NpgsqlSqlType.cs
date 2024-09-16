using System.Data;
using NHibernate.SqlTypes;
using NpgsqlTypes;

namespace sw.asset.repository.Mappings.Base.CustomeTypes;

public class NpgsqlSqlType : SqlType
{
    public NpgsqlDbType NpgDbType { get; }

    public NpgsqlSqlType(DbType dbType, NpgsqlDbType npgDbType)
        : base(dbType)
    {
        NpgDbType = npgDbType;
    }

    public NpgsqlSqlType(DbType dbType, NpgsqlDbType npgDbType, int length)
        : base(dbType, length)
    {
        NpgDbType = npgDbType;
    }

    public NpgsqlSqlType(DbType dbType, NpgsqlDbType npgDbType, byte precision, byte scale)
        : base(dbType, precision, scale)
    {
        NpgDbType = npgDbType;
    }
}