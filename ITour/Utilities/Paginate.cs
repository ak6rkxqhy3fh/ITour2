using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITour.Utilities
{
    public class Paginate<T>
    {
        public int Count { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        public IQueryable<T> Process(IQueryable<T> entityIQ, int? pageSize = 20)
        {
            pageSize = pageSize ?? 20;
            PageSize = PageSize == 0 ? (int)pageSize : PageSize;
            PageNumber = PageNumber == 0 ? 1 : PageNumber;

            Count = entityIQ.Count();
            TotalPages = (int)Math.Ceiling(Count / (double)PageSize);

            entityIQ = entityIQ.Skip((PageNumber - 1) * PageSize).Take(PageSize);
            return entityIQ;
        }

        public IEnumerable PageSizeDictionary => new Dictionary<int, string>() { { 10, "10" }, { 20, "20" }, { 50, "50" }, { 100, "100" } };
    }
}
