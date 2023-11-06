using System;
using System.Collections.Generic;
using System.Linq;

namespace bfws.Helpers
{
    public class Paginator<T>
    {
        private const int DEFAULT_PAGE_SIZE = 10;
        private const int DEFAULT_PAGE = 1;

        public IEnumerable<T> Paginate(IEnumerable<T> items, int? page, int? pageSize)
        {
            var pageNum = page ?? DEFAULT_PAGE;
            var size = pageSize ?? DEFAULT_PAGE_SIZE;
            if (pageNum == 1)
            {
                // Get first <pageCount> amount of items
                return items.Take(size);
            }
            else
            {
                // Skip the amount of items that would be found on x amount of pages and then grab the following <pageCount> items
                return items.Skip((pageNum - 1) * size).Take(size);
            }
        }

        public IEnumerable<int> GetAvailablePages(IEnumerable<T> items, int? pageSize)
        {
            return Enumerable.Range(1, (int)Math.Ceiling((double)items.Count() / (pageSize ?? DEFAULT_PAGE_SIZE)));
        }

        public Dictionary<string, object> GetPageData(IEnumerable<T> items, int? page, int? pageSize)
        {
            IEnumerable<int> pages = GetAvailablePages(items, pageSize);
            // Page number too large?
            if (pages.Any())
            {
                page = Math.Min(pages.Last(), (page ?? DEFAULT_PAGE));
            }
            else
            {
                page = DEFAULT_PAGE;
            }
            IEnumerable<int> pagesToShow = Enumerable.Range(Math.Max(1, (page ?? DEFAULT_PAGE) - 2), 5);

            Dictionary<string, object> ret = new Dictionary<string, object>
            {
                ["Enabled"] = items.Count() > 0 ? true : false,
                ["TotalItemCount"] = items.Count(),
                ["ItemCount"] = Paginate(items, page, pageSize).Count(),
                ["Pages"] = pages,
                ["PagesToShow"] = pagesToShow,
                ["Page"] = page ?? DEFAULT_PAGE,
                ["Size"] = pageSize ?? DEFAULT_PAGE_SIZE,
                ["LastPage"] = pages.Count() > 1 ? pages.Last() : 1
            };
            return ret;
        }
    }
}
