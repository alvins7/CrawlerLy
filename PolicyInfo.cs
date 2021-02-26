using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerLy
{
    //房间信息
    class PolicyInfo
    {
        public string PolicyId { get; set; }
        public string RoomName { get; set; }
        public string AvgPrice { get; set; }
        
        //1=担保
        public string BookType { get; set; }
        public string Breakfast { get; set; }
        public string SRId { get; set; }
        public string IsCanYuding { get; set; }

        // public List<PriceList> PriceList { get; set; }
    }
}
