using System.Collections.Generic;

namespace RealEstateCRM.Core.DTOs
{
    /// <summary>
    /// Generic response wrapper DTO returning paginated items along with metadata like total count and current page.
    /// </summary>
    /// <typeparam name="T">The type of the returned DTO items.</typeparam>
    public class PagedResultDto<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public IReadOnlyList<T> Items { get; set; } = new List<T>();

        public PagedResultDto(int pageIndex, int pageSize, int totalCount, IReadOnlyList<T> items)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            Items = items;
        }
    }
}