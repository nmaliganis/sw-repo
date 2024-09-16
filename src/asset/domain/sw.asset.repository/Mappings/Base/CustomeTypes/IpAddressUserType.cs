using System;
using System.Data;
using System.Data.Common;
using System.Net;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using Npgsql;

namespace sw.asset.repository.Mappings.Base.CustomeTypes;

[Serializable]
public class IpAddressUserType : IUserType
{
    public new bool Equals(object x, object y)
    {
        if (x == null && y == null)
            return true;

        if (x == null || y == null)
            return false;

        return x.Equals(y); 
    }

    public int GetHashCode(object x)
    {
        if (x == null)
            return 0;

        return x.GetHashCode();
    }

    public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
    {
        if (names.Length == 0)
            throw new InvalidOperationException("Expected at least 1 column");

        if (rs.IsDBNull(rs.GetOrdinal(names[0])))
            return null;

        object value = rs[names[0]]; 

        return value;
    }

    public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
    {
        NpgsqlParameter parameter = (NpgsqlParameter) cmd.Parameters[index];
        parameter.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Inet;

        if (value == null)
        {
            parameter.Value = DBNull.Value;
        }
        else
        { 
            parameter.Value = value;
        }
    }

    public object DeepCopy(object value)
    {
        if (value == null)
            return null;

        IPAddress copy = IPAddress.Parse(value.ToString());
        return copy;
    }

    public object Replace(object original, object target, object owner)
    {
        return original;
    }

    public object Assemble(object cached, object owner)
    {
        if (cached == null)
            return null;

        if (IPAddress.TryParse((string)cached, out var address))
        {
            return address;
        }
        return null;
    }

    public object Disassemble(object value)
    {
        if (value == null)
            return null;

        return value.ToString();

    }

    public SqlType[] SqlTypes => new SqlType[] { new NpgsqlSqlType(DbType.String, NpgsqlTypes.NpgsqlDbType.Inet),  };

    public Type ReturnedType => typeof(IPAddress);

    public bool IsMutable => false;
}