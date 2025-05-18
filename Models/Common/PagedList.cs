using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bocchiwebbackend.Models.Common
{
    public class PagedList<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
    }
}