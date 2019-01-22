
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ITour.Utilities
{
    public class SortedPaginatedList<T> : List<T>
    {
        public string SortBy { get; private set; }
        public int TotalCount { get; private set; }
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }

        public SortedPaginatedList(List<T> items, string sortBy, int count, int pageIndex, int pageSize)
        {
            SortBy = sortBy;
            TotalCount = count;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;        

        public static async Task<SortedPaginatedList<T>> CreateAsync(IQueryable<T> source, string sortBy, int pageIndex, int pageSize)
        {
            int count = await source.CountAsync();
            if (!string.IsNullOrEmpty(sortBy))
                source = source.OrderBy(sortBy);

            List<T> items = await source                
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new SortedPaginatedList<T>(items, sortBy, count, pageIndex, pageSize);
        }
    }
}
