using System;

namespace sw.infrastructure.Domain
{
    public interface IVersionedEntity
    {
        long Revision { get; set; }
    }
}