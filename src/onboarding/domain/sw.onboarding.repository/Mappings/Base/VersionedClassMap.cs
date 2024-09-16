using sw.infrastructure.Domain;
using FluentNHibernate.Mapping;

namespace sw.auth.repository.Mappings.Base
{
    public abstract class VersionedClassMap<T> : ClassMap<T> where T : IVersionedEntity
    {
        protected VersionedClassMap()
        {
            Version(x => x.Revision)
                .Column("Revision")
                .CustomSqlType("integer")
                .Generated.Always()
                .UnsavedValue("null");
        }
    }
}