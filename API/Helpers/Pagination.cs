using System.Collections.Generic;
using API.DTO;

namespace API.Helpers
{
    public class Pagination<T> where T : class
    {

        public Pagination(int pageSize, int pageIndex, int count, IReadOnlyList<T> data)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            Count = count;
            this.Data = data;
        }

        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }
    }
}