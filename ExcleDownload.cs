
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerLy
{
    public class ExcleDownload
    {

        #region 处理数据
        /// <summary>
        /// 导出矿山列表
        /// </summary>
        /// <returns></returns>
        /// <param name="isAll">是否导出全部数据</param>
        public static void GetExportList(List<HotelInfo> HotelInfo,string path)
        {
            try {
                var dateTime = DateTime.Now.ToString("yyyy-MM-dd");
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "同程酒店" + dateTime;
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 25;
                excelconfig.IsAllSizeColumn = true;
                excelconfig.FileName = path + "同程酒店"+ dateTime + ".xlsx";
                //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "Id", ExcelColumn = "序号" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "HotelName", ExcelColumn = "酒店名称" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "HotelId", ExcelColumn = "酒店ID" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "Date", ExcelColumn = "日期" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "Bed", ExcelColumn = "房型名称" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "Price", ExcelColumn = "含税价" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "Breakfast", ExcelColumn = "早餐", });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "Count", ExcelColumn = "库存" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "CrawlerTime", ExcelColumn = "爬取时间", });
                DataTable dtSource = FillDataTable(HotelInfo);
                //调用导出方法
                ExcelHelper.MemoryStream(dtSource, excelconfig);
            }
            catch (Exception ex) {
                string log = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ex.ToString();
                Console.WriteLine(log);
            }
        }

        #endregion
        public static DataTable FillDataTable<T>(List<T> modelList)
        {
            if (modelList == null || modelList.Count == 0)
            { return null; }
            DataTable dt = CreatTable(modelList[0]);
            foreach (T model in modelList)
            {
              DataRow dr = dt.NewRow();
              foreach (PropertyInfo p in typeof(T).GetProperties())
              {
                    dr[p.Name] = p.GetValue(model, null);
              }
                dt.Rows.Add(dr);
            }
           return dt;
        }
        private static DataTable CreatTable<T>(T model)
        {
            DataTable dt = new DataTable(typeof(T).Name);
            foreach (PropertyInfo p in typeof(T).GetProperties())
            {
                dt.Columns.Add(new DataColumn(p.Name, p.PropertyType));
            }
            return dt;
        }
    }

}
