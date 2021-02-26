using CrawlerLy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Web;

namespace CrawlerLy
{
    class Program
    {
        //定时时间
        static int Ticktime = 6000;
        //获取此程序线程数
        static int TheadNum = Convert.ToInt32(ConfigurationManager.AppSettings.Get("TheadNum"));
        //获取此程序开始日期
        static int StartNum = Convert.ToInt32(ConfigurationManager.AppSettings.Get("StartNum"));
        //获取此程序开始日期
        static int CrawlerNum = Convert.ToInt32(ConfigurationManager.AppSettings.Get("CrawlerNum"));
        //获取此程序爬取阶梯
        static int AppNum = Convert.ToInt32(ConfigurationManager.AppSettings.Get("AppNum"));
        //获取代理延迟
        static int SleepTime = Convert.ToInt32(ConfigurationManager.AppSettings.Get("SleepTime"));
        //数据路径
        static string shujupath = ConfigurationManager.AppSettings.Get("shujupath");
        //代理Api
        static string ProxyUrl = ConfigurationManager.AppSettings.Get("ProxyUrl");
        //列表cookie
        static string cookieStr = "GnHotelData=%7B%22CnHotelComeDate%22%3A%222020-07-13%22%2C%22CnHotelLeaveDate%22%3A%222020-07-14%22%2C%22CnHotelCityName%22%3A%22%25E5%258C%2597%25E4%25BA%25AC%22%2C%22CnHotelCityId%22%3A%2253%22%2C%22CnHotelParCityId%22%3A%220%22%2C%22Keyword%22%3A%22%22%2C%22KeywordId%22%3A%220%22%2C%22KeywordType%22%3A%220%22%2C%22Longitude%22%3A%22%22%2C%22Latitude%22%3A%22%22%7D; 17uCNRefId=RefId=0&SEFrom=baidu&SEKeyWords=; TicketSEInfo=RefId=0&SEFrom=baidu&SEKeyWords=; CNSEInfo=RefId=0&tcbdkeyid=&SEFrom=baidu&SEKeyWords=&RefUrl=https%3A%2F%2Fwww.baidu.com%2Flink%3Furl%3DlmVtyY3_O3rc2HtPELeKOSiMM7LIcKqQ-z3IRn2b3oC%26wd%3D%26eqid%3D9b71586e00007cb0000000065f0c1021; qdid=-9999; __tctma=144323752.1594626084143849.1594626084634.1594626084634.1594626084634.1; __tctmu=144323752.0.0; longKey=1594626084143849; __tctrack=0; route=aa2f4a4e5099ff74807cc36c6b779467; wangba=1594626084171; firsttime=1594626084958; __tctmc=144323752.261808523; __tctmz=144323752.1594626902846.1.2.utmccn=(referral)|utmcsr=v2ex.com|utmcct=t/687280|utmcmd=referral; Hm_lvt_c6a93e2a75a5b1ef9fb5d4553a2226e5=1594626084,1594626902,1594627046; Hm_lvt_f97c1b2277f4163d4974e7b5c8aa1e96=1594626084,1594626903,1594627047; Hm_lpvt_c6a93e2a75a5b1ef9fb5d4553a2226e5=1594627914; Hm_lpvt_f97c1b2277f4163d4974e7b5c8aa1e96=1594627915; trace_extend={'deviceid':'1594626084143849','appid':'1','userid':'1594626084143849','orderfromid':'57000','sessionid':'b55c-5b0c-25af-76e9-59dc-6cff','pvid':'82da46aa'}; __tctmd=144323752.737325; __tctmb=144323752.757100920287544.1594626902846.1594627915063.4; lasttime=1594628999951";
        //详情cookie
        static string newcookieStr = "Hm_lvt_c6a93e2a75a5b1ef9fb5d4553a2226e5=1594627120,1594730040; __tctma=144323752.159462712161991.1594627121383.1594730042297.1594734014725.9; Hm_lvt_f97c1b2277f4163d4974e7b5c8aa1e96=1594627121,1594730041; firsttime=1594627122783; lasttime=1594730571591; _tcudid_v2=vXbaVJUFqZL76Y2YU2bT2GvOjM9HtABx5rVu-HFaNhQ; Hm_lpvt_c6a93e2a75a5b1ef9fb5d4553a2226e5=1594734014; qdid=-9999; 17uCNRefId=RefId=0&SEFrom=&SEKeyWords=; TicketSEInfo=RefId=0&SEFrom=&SEKeyWords=; CNSEInfo=RefId=0&tcbdkeyid=&SEFrom=&SEKeyWords=&RefUrl=…1594734014; trace_extend={'deviceid':'159462712161991','appid':'1','userid':'159462712161991','orderfromid':'57000','sessionid':'cb88-2338-d009-aa2d-b74d-6d6b','pvid':'604e612b'}; wangba=1594730041644; trace_token=%7C*%7CcityId%3A101%7C*%7CqId%3A08d9dd45-e884-42cd-b572-43515aa5c96c%7C*%7Cst%3Acity%7C*%7CsId%3A101%7C*%7Cpos%3A4%7C*%7ChId%3A40601023%7C*%7CTp%3AtalRec%7C*%7C; __tccgd=144323752.0; __tctmb=144323752.2909192084196903.1594730569353.1594730569353.1; User-Ref-SessionId=cb88-2338-d009-aa2d-b74d-6d6b}";
        //订房cookie
        static string rnewcookieStr = "firsttime={firsttime}; lasttime={lasttime}; qdid=-9999; 17uCNRefId=RefId=0&SEFrom=&SEKeyWords=; TicketSEInfo=RefId=0&SEFrom=&SEKeyWords=; CNSEInfo=RefId=0&tcbdkeyid=&SEFrom=&SEKeyWords=&RefUrl=; wangba=1598429002541; User-Ref-SessionId=fe1f-2a4c-8f9b-dabc-c06f-02bc; trace_extend={'deviceid':'1598429003494571','appid':'1','userid':'1598429003494571','orderfromid':'57000','sessionid':'fe1f-2a4c-8f9b-dabc-c06f-02bc','pvid':'8b783dcc'}; __tctmc=144323752.210388243; __tctmd=144323752.105597701; __tctma=144323752.1598429003494571.1598429003800.1598429003800.1598429003800.1; __tctmb=144323752.3259225622842662.1598429005037.1598429482153.3; __tctmu=144323752.0.0; __tctmz=144323752.1598429003800.1.1.utmccn=(direct)|utmcsr=(direct)|utmcmd=(none); longKey={longkey}; __tctrack=0; route=aa2f4a4e5099ff74807cc36c6b779467; passport_login_state={pageurl}; _tcudid_v2=B3xpvitnXA0hoXexJt2gfzL-j73upLYNes9MJR6mBbg";
        //列表url
        static string _url = "https://www.ly.com/hotel/api/tmapi/search/hotellist/";
        //订房cookieurl
        static string page_url = "pageurl=http://www.ly.com/book1.html?HotelId={HotelId}&RoomTypeId={RoomTypeId}&ComeDate={ComeDate}&LeaveDate={LeaveDate}&PolicyId={PolicyId}&ptuse=1&SRId={SRId}";
        //详情url
        static string child_url = "https://www.ly.com/hotel/api/tmapi/roomlist/?HotelId={HotelId}&ComeDate={ComeDate}&LeaveDate={LeaveDate}&antitoken={antitoken}&ptUse=1&sug_act_info=&trace_token={trace_token}&search_entrance_id=tpc_home&_=159464989223{i}";
        //订房url
        static string room_url = "http://www.ly.com/hotel/api/tmapi/book/checkroomstatus/?HotelId={HotelId}&RoomTypeId={RoomTypeId}&PolicyId={PolicyId}&RoomCount=1&ComeDate={ComeDate}&LeaveDate={LeaveDate}&ShadowId={ShadowId}&ptuse=1&antitoken=573a6329cd01a9332ba363a085e47ef9&traceId=77c3fb13-016d-47f2-ba72-4c4f035bb242";
        //路径
        static string path = ConfigurationManager.AppSettings.Get("path");
        static string logpath = ConfigurationManager.AppSettings.Get("logpath");
        static string HotelId = "";
        //详情rfer
        static string rfer = "https://www.ly.com/HotelInfo-{HotelId}.html";
        //订房rfer
        static string newrfer = "http://www.ly.com/book1.html?HotelId={HotelId}&RoomTypeId={RoomTypeId}&ComeDate={ComeDate}&LeaveDate={LeaveDate}&PolicyId={PolicyId}&ptuse=1&SRId={SRId}";
        //代理ip
        static string proxyip = "";
        //端口
        static int proxyport = 0;
        //第一代理时间
        static double proxytime = 0;
        public static int Time
        {
            get;
            set;
        }
        //定时器
        public static System.Timers.Timer MT { get; set; }
        //全局参数
        public static Unit _ent = new Unit();
        //错误ID
        static string eid = "";
        //爬取轮数
        static int num = 0;
        //是否直接开始
        static string isgo = "0";
        //超时次数
        static int errnum = 0;
        //超时次数
        static int hotelnum = 0;
        static List<HotelInfo> HotelInfoList1=new List<HotelInfo>();

        public delegate bool ControlCtrlDelegate(int CtrlType);
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ControlCtrlDelegate HandlerRoutine, bool Add);
        private static ControlCtrlDelegate cancelHandler = new ControlCtrlDelegate(HandlerRoutine);
        public static bool HandlerRoutine(int CtrlType)
        {
            string IsDownload = Console.ReadLine();
            if (string.IsNullOrEmpty(IsDownload)|| HotelInfoList1.Count > 2000)
            {
                DownList(HotelInfoList1);
            }
            return false;
        }
        /// <summary>
        /// 主程序开始
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            System.AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            SetConsoleCtrlHandler(cancelHandler, true);
            MT = new System.Timers.Timer(5000);
            MT.Start();
            MT.Elapsed += StartThread;
            MT.Enabled = true;
            Console.WriteLine(" 等待程序的执行．．．．．．");
            Console.ReadLine();
        }

       
        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            string log = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + e.ExceptionObject.ToString();
            Console.WriteLine(log);
            Console.ReadLine();
            Environment.Exit(1);
        }
        /// <summary>
        /// 开启多线程
        /// </summary>
        public static void StartThread(object source, System.Timers.ElapsedEventArgs _e)
        {
             _ent.ComeDate = DateTime.Now;
            HotelInfoList1 = new List<HotelInfo>();
            //定时器早上九点-晚上十点
            if ((8 <= _ent.ComeDate.Hour && _ent.ComeDate.Hour <= 22))
            {
                if (MT.Interval == 5000)//如果是第一次执行
                {
                    Time = (60 - _ent.ComeDate.Minute) * 1000 * Ticktime;
                    MT.Interval = Time;//设置Interval到整点。
                }
                else if (MT.Interval == Time)
                {
                    MT.Interval = 60 * 60 * 1000;//设置Interval为一个小时。
                }
                num++;
                _ent.LeaveDate = _ent.ComeDate.AddDays(1);
                //获取代理
                //GetProxy();
                Console.WriteLine("正在爬取第．．．．．．．．．．．．．．．．．．．．．．．．．．" + num + "轮    " + _ent.ComeDate.ToString());
                for (int i = (AppNum-1)* CrawlerNum; i < AppNum*CrawlerNum; i++)
                {
                    abAsync.Post(i);
                }
            }
        }
        /// <summary>
        /// dataflow开始
        /// </summary>
        public static ActionBlock<int> abAsync = new ActionBlock<int>((i) =>
        {
            Thread.Sleep(1000);
            Console.WriteLine(i + " ThreadId:" + Thread.CurrentThread.ManagedThreadId + " Execute Time:" + DateTime.Now);
            try
            {
                var Today_date = DateTime.Now;

                if (Today_date.Day > StartNum)
                {
                    Environment.Exit(1);
                }

                GetPriceList(_url, _ent, i, Thread.CurrentThread.ManagedThreadId);
            }
            catch (Exception ex)
            {
                string log = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ex.ToString();
                Console.WriteLine(log);
            }
        }
       , new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = TheadNum });
        /// <summary>
        /// 获取酒店价格
        /// </summary>
        /// <param name="url"></param>
        /// <param name="ent"></param>
        public static void GetPriceList(string url, Unit ent,object BeginNum,int flowId)
        {
            hotelnum++;
            //临时ID
            string _UID = "";
            //清空
            eid = "";
            //线程状态
            bool Issthread = true;
            try
            {
                DateTime hote_date = ent.ComeDate;
                //参数
                ent.antitoken = "722b1952bc9ceedbc0b04eb863cdc9ae";
                ent.iid = "0.16545553576668293";
                ent.TraceId = "007df812-1536-4ede-a0cd-3076dc576c0e";
                ent.trace_token = "|*|cityId:101|*|qId:08d9dd45-e884-42cd-b572-43515aa5c96c|*|st:city|*|sId:101|*|pos:4|*|hId:40601023|*|Tp:talRec|*|";
                ent.search_entrance_id = "tpc_home";
                List<HotelInfo>  HotelInfoList2 = new List<HotelInfo>();
                //读取需求列表
                StreamReader sr = new StreamReader(@shujupath, Encoding.GetEncoding("utf-8"));
                string[] upStr = File.ReadAllLines(@shujupath, Encoding.GetEncoding("utf-8"));
                sr.Close();
                int _id = 0;
                int HotelCount = upStr.Length;
                Console.WriteLine("共．．．．．．．．．" + HotelCount + "家酒店");
                if (Issthread)
                {
                    try
                    {
                        //请求是否出错
                        bool iserr = false;
                        Console.WriteLine("第"+ flowId + "条线程正在爬取第．．．．．．．．．" + hotelnum+ "家酒店,还有" + (300 - hotelnum) + "家");
                        string[] newStr = upStr[(int)BeginNum].Split('\t');
                        HotelId = newStr[1];
                        _UID = newStr[1];
                        //填充房间URL
                        var new_url = child_url.Replace("{HotelId}", newStr[1]);
                        new_url = new_url.Replace("{ComeDate}", ent.ComeDate.ToString("yyyy-MM-dd"));
                        new_url = new_url.Replace("{LeaveDate}", ent.LeaveDate.ToString("yyyy-MM-dd"));
                        new_url = new_url.Replace("{antitoken}", ent.antitoken);
                        new_url = new_url.Replace("{trace_token}", ent.trace_token.ToString());
                        new_url = new_url.Replace("{i}", 1.ToString());
                        var rerferer = rfer.Replace("{HotelId}", newStr[1]);
                            
                        //获取房间信息
                        RoomRequest RoomRequestlist = JsonConvert.DeserializeObject(HttpGet(new_url, rerferer, newcookieStr), typeof(RoomRequest)) as RoomRequest;
                        if (RoomRequestlist == null)
                        {

                            iserr = true;
                        }
                        else if (RoomRequestlist.Status == "400" || RoomRequestlist.RoomList == null)
                        {
                            iserr = true;
                        }
                        if (iserr)
                        {
                            Console.WriteLine("获取"+ newStr[0] + "房间信息失败XXXXXXXX");
                            HotelInfo hotel = SetHotel(newStr[1], newStr[0], "", _id, hote_date.ToString("yyyy-MM-dd"));
                            HotelInfoList2.Add(hotel);
                            return;
                        }
                        Console.WriteLine("成功获取" + newStr[0] + "房间信息√√√√√√√√√√");

                        //循环房型
                        foreach (var room_ent in RoomRequestlist.RoomList)
                        {

                            //循环付款类型
                            foreach (var Pulic_ent in room_ent.PolicyInfo)
                            {
                                //BookType：2 在线付
                                if (Pulic_ent.BookType == "2")
                                {
                                    for (var d = 0; d < 7; d++)
                                    {
                                        //填充cookie的pageurl
                                        string _pageurl = page_url.Replace("{HotelId}", newStr[1]);
                                        _pageurl = _pageurl.Replace("{RoomTypeId}", room_ent.RoomId);
                                        _pageurl = _pageurl.Replace("{ComeDate}", ent.ComeDate.AddDays(d).ToString("yyyy-MM-dd"));
                                        _pageurl = _pageurl.Replace("{LeaveDate}", ent.LeaveDate.AddDays(d).ToString("yyyy-MM-dd"));
                                        _pageurl = _pageurl.Replace("{PolicyId}", Pulic_ent.PolicyId);
                                        _pageurl = _pageurl.Replace("{SRId}", Pulic_ent.SRId);
                                        //设置新的cookie
                                        TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                                        string new_coo = rnewcookieStr.Replace("{pageurl}", _pageurl);
                                        new_coo = new_coo.Replace("{firsttime}", Convert.ToInt64(ts.TotalSeconds).ToString());
                                        new_coo = new_coo.Replace("{lasttime}", Convert.ToInt64(ts.TotalSeconds+10000).ToString());
                                        new_coo = new_coo.Replace("{longkey}", Convert.ToInt64(ts.TotalSeconds + 10001).ToString());
                                        //填充订单URL
                                        var new_room_url = room_url.Replace("{HotelId}", newStr[1]);
                                        new_room_url = new_room_url.Replace("{RoomTypeId}", room_ent.RoomId);
                                        new_room_url = new_room_url.Replace("{PolicyId}", Pulic_ent.PolicyId);
                                        new_room_url = new_room_url.Replace("{ShadowId}", Pulic_ent.SRId);
                                        new_room_url = new_room_url.Replace("{ComeDate}", ent.ComeDate.AddDays(d).ToString("yyyy-MM-dd"));
                                        new_room_url = new_room_url.Replace("{LeaveDate}", ent.LeaveDate.AddDays(d).ToString("yyyy-MM-dd"));
                                        new_room_url = new_room_url.Replace("{antitoken}", ent.antitoken);
                                        //填充订单referer
                                        var new_referer = newrfer.Replace("{HotelId}", newStr[1]);
                                        new_referer = new_referer.Replace("{RoomTypeId}", room_ent.RoomId);
                                        new_referer = new_referer.Replace("{ComeDate}", ent.ComeDate.AddDays(d).ToString("yyyy-MM-dd"));
                                        new_referer = new_referer.Replace("{LeaveDate}", ent.LeaveDate.AddDays(d).ToString("yyyy-MM-dd"));
                                        new_referer = new_referer.Replace("{PolicyId}", Pulic_ent.PolicyId);
                                        new_referer = new_referer.Replace("{SRId}", Pulic_ent.SRId);

                                        bool isrspent = false;
                                        string txt = "";
                                        HotelInfo hotel = new HotelInfo();
                                        //4分钟获取一次代理
                                        if ((DateTime.Now.TimeOfDay.TotalMinutes - proxytime) > 4)
                                        {
                                            //GetProxy();
                                        }
                                        //获取订单信息
                                        rsp rspent = JsonConvert.DeserializeObject(HttpNewGet(new_room_url, new_referer, new_coo,0).Result, typeof(rsp)) as rsp;
                                        //如果没有返回值
                                        if (rspent == null)
                                        {
                                            isrspent = true;
                                            txt = "请求失败";
                                        }
                                        else if (string.IsNullOrEmpty(rspent.BookMaxRoomCount))
                                        {
                                            isrspent = true;
                                            txt = "没有库存";
                                        }
                                        else
                                        {
                                            Console.WriteLine("成功获取" + newStr[0] + "，"+room_ent.RoomName + "订单信息√√√√√√√√√√");
                                            var PriceList = rspent.bookDayPriceList;
                                            hotel.HotelId = newStr[1];
                                            hotel.HotelName = newStr[0];
                                            hotel.Bed = room_ent.RoomName;
                                            hotel.Id = _id;
                                            hotel.Price = PriceList[0] .RealTimePrice == "满房" ? "0" : PriceList[0].RealTimePrice;
                                            hotel.Breakfast = PriceList[0].Breakfast == "" ? "无早" : PriceList[0].Breakfast;
                                            hotel.Date = PriceList[0].Time.ToString("yyyy-MM-dd");
                                            hotel.Count = rspent.BookMaxRoomCount == null ? "0" : rspent.BookMaxRoomCount;
                                        }
                                        hotel.CrawlerTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                        if (isrspent)
                                        {
                                            Console.WriteLine("获取" + newStr[0] + "，" + room_ent.RoomName + "订单信息失败XXXXXXX"+ txt);
                                            hotel = SetHotel(newStr[1], newStr[0], room_ent.RoomName, _id, hote_date.AddDays(d).ToString("yyyy-MM-dd"));
                                        }
                                        HotelInfoList2.Add(hotel);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        eid += _UID;
                        string log = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "酒店ID：" + _UID + ex.ToString();
                        Console.WriteLine(log);
                        return;
                    }
                    Thread.Sleep(1000);
                    //下载
                    Console.WriteLine("第" + flowId + "条线程已爬取共．．．．．．．．．" + HotelInfoList2.Count + "条数据");
                     DownList(HotelInfoList2);
                }
            }
            catch (Exception ex)
            {
                string log = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "酒店ID：" + _UID + ex.ToString();
                Console.WriteLine(log);
                WriteTxt(log, logpath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ".txt");
            }

        }

        public static void DownList(List<HotelInfo> HotelInfoList)
        {
            Console.WriteLine("正在下载↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓第" + num + "轮数据");
            ExcleDownload.GetExportList(HotelInfoList, path + "第" + num + "轮");
            WriteTxt(eid, logpath + "第" + num + "轮没爬取到的酒店清单" + ".txt");
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        private static async Task<string> HttpNewGet(string Url, string Referer, string cookieS,int erNum)
        {
            try {
                HttpClientHandler handler = new HttpClientHandler()
                {
                    //Proxy = new WebProxy(proxyip, proxyport),
                    //UseProxy = true,
                    UseCookies = false,
                };
                HttpClient client = new HttpClient(handler);
                client = AddDefaultHeaders(client, Referer, cookieS);
                client.Timeout = new TimeSpan(0,0,5);
                var response = await client.GetStreamAsync(Url);
                string responseBody = "";
                StreamReader myStreamReader = new StreamReader(response, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                Thread.Sleep(1000);
                return responseBody;
            }
            catch (Exception ex)
            {
                erNum++;
                Thread.Sleep(2000);
                string exlog = ex.ToString();
                Console.WriteLine("访问===》" + Url + "======>" + ex.ToString());
                //连续错误5次跳过
                if ((exlog.Contains("无法连接") || exlog.Contains("超时") || exlog.Contains("403")) && erNum < 5)
                {
                    Console.WriteLine("正在重新请求=========>" + Url);
                    //请求出错3次重新请求代理
                    if (erNum > 3) 
                    {
                        //GetProxy();
                    }
                    string s = await HttpNewGet(Url, Referer, cookieS, erNum);
                    Console.WriteLine("重新请求成功√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√");
                    return s;
                }
                return "";
            }
        }
        /// <summary>
        /// 添加头部信息
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="Referer"></param>
        /// <param name="cookieS"></param>
        /// <returns></returns>
        private static HttpClient AddDefaultHeaders(HttpClient httpClient,string Referer, string cookieS)
        {
            httpClient.DefaultRequestHeaders.Add("Host", "www.ly.com");
            httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.8,zh-TW;q=0.7,zh-HK;q=0.5,en-US;q=0.3,en;q=0.2");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
            httpClient.DefaultRequestHeaders.Add("Referer", Referer);
            httpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            httpClient.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("Cookie", cookieS);
            return httpClient;
        }
        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static string HttpGet(string Url, string Referer, string cookieS)
        {
            try
            {
                System.GC.Collect();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.ServicePoint.ConnectionLimit = 65500;
                //str为IP地址 port为端口号 代理类
                WebProxy proxyObject = new WebProxy(proxyip, proxyport);
                //设置代理
                request.Proxy = proxyObject;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36";
                request.Method = "GET";
                //写入Cookie
                request.Headers.Add("Cookie", cookieS);
                request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                request.KeepAlive = true;
                request.Host = "www.ly.com";
                request.Referer = Referer;
                request.Timeout = 5000;
                request.ReadWriteTimeout = 5000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                response.Close();
                request.Abort();
                myStreamReader.Close();
                myResponseStream.Close();
                Thread.Sleep(1000);
                return retString;
            }
            catch (Exception ex)
            {
                errnum++;
                string exlog = ex.ToString();
                Thread.Sleep(2000);
                Console.WriteLine("访问===》" + Url + "======>" + ex.ToString());
                if ((exlog.Contains("无法连接") || exlog.Contains("超时") || exlog.Contains("403")) && errnum < 3)
                {
                    Console.WriteLine("正在重新请求=========>" + Url);
                    //GetProxy();
                    string s = HttpGet(Url, Referer, cookieS);
                    errnum = 0;
                    Console.WriteLine("重新请求成功√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√");
                    return s;
                }
                errnum = 0;
                return "";
            }
        }
        /// <summary>
        /// Get代理
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static void GetProxy()
        {
            try
            {
                Console.WriteLine("正在获取===============》代理");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ProxyUrl);
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                if (string.IsNullOrEmpty(retString))
                {
                    Console.WriteLine("生成代理出错");
                    Thread.Sleep(1000);
                    GetProxy();
                }
                string[] ProxyResult = retString.Split(':');
                proxyip = ProxyResult[0];
                proxyport = Convert.ToInt32(ProxyResult[1]);
                proxytime = DateTime.Now.TimeOfDay.TotalMinutes;
                Console.WriteLine("获取代理成功√√√√√√√√√√√√√√√√√√√√√");
            }
            catch (Exception ex)
            {
                Console.WriteLine("生成代理出错,休眠2S===========》" + ex.ToString());
                Thread.Sleep(2000);
                GetProxy();
            }
        }
        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="ent"></param>
        /// <returns></returns>
        public static string HttpPost(string Url, Unit ent)
        {
            try
            {

                UTF8Encoding encoding = new UTF8Encoding();
                CookieContainer cookie = new CookieContainer();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Accept = "application/json, text/javascript, */*; q=0.01";
                request.Headers.Add("Cookie", cookieStr);
                request.Method = "POST";
                request.Host = "www.ly.com";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36";
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                request.Referer = "https://www.ly.com/searchlist.html?cityid=53&sectionid=0&comedate=2020-07-05&leavedate=2020-07-06&word=&wordid=0&wordtype=0&spm0=10002.2001.1.0.1.1.2";
                // 写入参数
                IDictionary<string, object> para = new Dictionary<string, object>();
                para.Add("CityId", ent.CityId);
                para.Add("BizSectionId", "0");
                para.Add("SectionId", "0");
                para.Add("Word", "");
                para.Add("PriceRegion", "");
                para.Add("Range", "");
                para.Add("HotelStar", "");
                para.Add("ChainId", "");
                para.Add("HasStandBack", "0");
                para.Add("Facilities", "");
                para.Add("BreakFast", "");
                para.Add("PayType", "");
                para.Add("SortType", "0");
                para.Add("Instant", "");
                para.Add("LabelId", "0");
                para.Add("WordType", "0");
                para.Add("ThemeId", "");
                para.Add("Latitude", "");
                para.Add("Longitude", "");
                para.Add("ComeDate", ent.ComeDate);
                para.Add("LeaveDate", ent.LeaveDate);
                para.Add("PageSize", "20");
                para.Add("Page", ent.Page);
                para.Add("antitoken", "84c9d06ad64029787ccd685d61c74bd6");
                para.Add("IsSeo", "0");
                para.Add("iid", "0.30335013793425203");
                para.Add("TraceId", "06d2d94c-e607-424c-b4dc-69fa34b165a6");
                para.Add("HotelType", "0");
                para.Add("trace_token", "|*|cityId:101|*|qId:aae41d9d-a25a-4ebe-9c54-41a0e0248d1b|*|st:city|*|sId:101|*|k:含早餐|*|tp:早餐|*|");
                para.Add("search_entrance_id", "tpc_home");
                StringBuilder builder = new StringBuilder();
                int i = 0;
                foreach (var item in para)
                {
                    if (i > 0)
                        builder.Append("&");
                    builder.AppendFormat("{0}={1}", item.Key, item.Value);
                    i++;
                }
                byte[] data = Encoding.UTF8.GetBytes(builder.ToString());
                request.ContentLength = data.Length;
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Close();
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                response.Cookies = cookie.GetCookies(response.ResponseUri);
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                return retString;

            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("系统错误：{0}", e.Message));
                return string.Empty;
            }
        }
        public static void WriteTxt(string txt, string newpath)
        {
            //创建写入文件 
            FileStream fs1 = new FileStream(newpath, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs1);
            //开始写入值
            sw.WriteLine(txt);
            sw.Close();
            fs1.Close();
        }

        public static void WriteEXL()
        {
            string str = "";
            DirectoryInfo thisOne = new DirectoryInfo("e:\\Doc\\Hotel\\3nd\\");
            //获取当前的目录的文件
            FileInfo[] fileInfos = thisOne.GetFiles();
            foreach (var file in fileInfos)
            {
                StreamReader sr = new StreamReader(file.FullName, Encoding.GetEncoding("utf-8"));
                string upStr = File.ReadAllText(file.FullName, Encoding.GetEncoding("utf-8"));
                str += upStr.Replace("酒店ID：", "").Replace("酒店名称：", "/").Replace("价格：", "/").Replace("类型：", "/").Replace("早餐：", "/").Replace("库存：", "/").Replace("日期：", "/").Replace("爬取时间：", "/") + "\r\n";
            }
            WriteTxt(str, "e:\\Doc\\tongji.txt");

        }
        /// <summary>
        /// 填充错误数据
        /// </summary>
        public static HotelInfo SetHotel(string HotelId, string HotelName, string RoomName, int _id, string date)
        {
            HotelInfo hotel = new HotelInfo();
            hotel.HotelId = HotelId;
            hotel.HotelName = HotelName;
            hotel.Bed = RoomName;
            hotel.Id = _id;
            hotel.Price = "0";
            hotel.Breakfast = "";
            hotel.Date = date;
            hotel.Count = "0";
            hotel.CrawlerTime = DateTime.Now.ToString("yyyy-MM-dd-HHmmss");
            return hotel;
        }
    }
}
