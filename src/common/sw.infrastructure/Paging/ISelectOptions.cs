namespace sw.infrastructure.Paging
{
    public interface ISelectOptions<T> : IQueryBuilder
    {
        string Select { get; }
    }
}