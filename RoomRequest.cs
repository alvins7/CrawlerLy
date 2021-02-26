using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerLy
{
    //房间
    class RoomRequest
    {
        public string Status { get; set; }
        public string trace_token { get; set; }
        public List<Room> RoomList { get; set; }
    }
}
