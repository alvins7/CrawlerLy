using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerLy
{
    //库存
    class rsp
    {
        public string BookMaxRoomCount { get; set; }
        public List<bookDayPriceList> bookDayPriceList { get; set; }

        public string rspStatus { get; set; }
    }
}
