namespace sw.infrastructure.Paging
{
    public interface IQueryOptions<T> : IExportOptions<T>
    {
        uint? Skip { get; }

        uint? Top { get; }

        bool? Count { get; }


        IQueryOptions<T> NextPage();
        IQueryOptions<T> PreviousPage();
    }
}