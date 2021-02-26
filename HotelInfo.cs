using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerLy
{
    public class HotelInfo
    {
        //序号
        public int Id { get; set; }
        //酒店名
        public string HotelName { get; set; }
        //酒店
        public string HotelId { get; set; }

        //日期
        public string Date { get; set; }

        //房间类型
        public string Bed { get; set; }

        //价格
        public string Price { get; set; }
        //早餐
        public string Breakfast { get; set; }

        //库存
        public string Count { get; set; }
        //爬取时间
        public string CrawlerTime { get; set; }
    }
}
