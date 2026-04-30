namespace CondosmartWeb.Models
{
    public interface IPagedListMetadata
    {
        int Page { get; }
        int PageSize { get; }
        int TotalItems { get; }
        int TotalPages { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
    }

    public class PagedListViewModel<T> : IPagedListMetadata
    {
        public IReadOnlyList<T> Items { get; init; } = [];
        public int Page { get; init; }
        public int PageSize { get; init; }
        public int TotalItems { get; init; }
        public int TotalPages { get; init; }
        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;

        public static PagedListViewModel<T> Create(IEnumerable<T> source, int page, int pageSize)
        {
            var safePage = page < 1 ? 1 : page;
            var safePageSize = pageSize <= 0 ? 10 : pageSize;
            var totalItems = source.Count();
            var totalPages = totalItems == 0 ? 1 : (int)Math.Ceiling(totalItems / (double)safePageSize);
            if (safePage > totalPages)
                safePage = totalPages;

            return new PagedListViewModel<T>
            {
                Page = safePage,
                PageSize = safePageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = source.Skip((safePage - 1) * safePageSize).Take(safePageSize).ToList()
            };
        }
    }
}
