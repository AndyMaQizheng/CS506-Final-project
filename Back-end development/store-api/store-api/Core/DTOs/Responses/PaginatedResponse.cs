using System;

namespace store_api.Core.DTOs.Responses
{
    public class PaginatedResponse
    {
        public PaginatedResponse()
        {
        }
        public PageInfo Page { get; set; }
        public dynamic Items { get; set; }
    }
    public class PageInfo
    {
        public long Total { get; set; }
        public int Size { get; set; }
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PreviousPage { get; set; }
        public int NextPage { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int TotalSkipped { get; set; }

    }
}
