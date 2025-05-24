using System.Collections.Generic;

namespace ApiECommerce.DTOs
{
    public class PaginatedResponse<T>
    {
        public List<T> Data { get; set; } = new List<T>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)System.Math.Ceiling(TotalCount / (double)PageSize);
    }
}
