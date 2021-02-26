using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerLy
{
    class Room
    {
        public string RoomId { get; set; }
        public string RoomName { get; set; }
        //1=担保
        public string BookType { get; set; }
        public string Breakfast { get; set; }
        public string Bed { get; set; }
        public string Date { get; set; }
        public string SRId { get; set; }
        public string trace_token { get; set; }
        public string IsCanYuding { get; set; }
        public List<PolicyInfo> PolicyInfo { get; set; }
    }
}
