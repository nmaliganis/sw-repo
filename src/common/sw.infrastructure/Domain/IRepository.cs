namespace sw.infrastructure.Domain
{
    public interface IRepository<T, TId> : IReadOnlyRepository<T, TId>
        where T : IAggregateRoot
    {
        void Save(T entity);
        void Save(T entity, TId id);
        void Add(T entity);
        void Remove(T entity);
    }
}
