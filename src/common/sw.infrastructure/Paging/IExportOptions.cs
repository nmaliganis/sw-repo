namespace sw.infrastructure.Paging
{
    public interface IExportOptions<T> : ISelectOptions<T>
    {
        string Filter { get; }
        string Sort { get; }
        IQueryOptions<T> ToQueryOptions();
    }
}