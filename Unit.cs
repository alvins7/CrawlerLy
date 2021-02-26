using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerLy
{
    class Unit
    {
        public int CityId { get; set; }
        public int BizSectionId { get; set; }
        public int SectionId { get; set; }
        public string Word { get; set; }
        public string PriceRegion { get; set; }
        public string Range { get; set; }
        public string HotelStar { get; set; }
        public string ChainId { get; set; }
        public string HasStandBack { get; set; }
        public string Facilities { get; set; }
        public string BreakFast { get; set; }
        public string PayType { get; set; }
        public string SortType { get; set; }
        public string Instant { get; set; }
        public string LabelId { get; set; }
        public string WordType { get; set; }
        public string ThemeId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public DateTime ComeDate { get; set; }
        public DateTime LeaveDate { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public string antitoken { get; set; }
        public string IsSeo { get; set; }
        public string iid { get; set; }

        public string HotelType { get; set; }
        public string TraceId { get; set; }
        public string trace_token { get; set; }
        public string search_entrance_id { get; set; }
    }
}
