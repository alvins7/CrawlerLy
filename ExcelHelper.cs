using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Drawing;
using NPOI.HSSF.Util;
using System;
using System.Collections.Generic;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;
using NPOI.OpenXmlFormats.Wordprocessing;
using ICell = NPOI.SS.UserModel.ICell;
using System.Threading;

namespace CrawlerLy
{
    /// <summary>
    ///描 述：NPOI Excel DataTable操作类
    public class ExcelHelper
    {
        #region Excel导出方法 ExcelDownload
        /// <summary>
        /// Excel导出下载
        /// </summary>
        /// <param name="dtSource">DataTable数据源</param>
        /// <param name="excelConfig">导出设置包含文件名、标题、列设置</param>
        public static void ExcelDownload(DataTable dtSource, ExcelConfig excelConfig)
        {
           // ExportMemoryStream(dtSource, excelConfig);
        }
        /// <summary>
        /// Excel导出下载
        /// </summary>
        /// <param name="list">数据源</param>
        /// <param name="templdateName">模板文件名</param>
        /// <param name="newFileName">文件名</param>
        //public static void ExcelDownload(List<TemplateMode> list, string templdateName, string newFileName)
        //{
        //    HttpResponse response = System.Web.HttpContext.Current.Response;
        //    response.Clear();
        //    response.Charset = "UTF-8";
        //    response.ContentType = "application/vnd-excel";//"application/vnd.ms-excel";
        //    System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + newFileName));
        //    System.Web.HttpContext.Current.Response.BinaryWrite(ExportListByTempale(list, templdateName).ToArray());
        //}
        #endregion

        #region DataTable导出到Excel文件excelConfig中FileName设置为全路径
        /// <summary>
        /// DataTable导出到Excel文件 Export()
        /// </summary>
        /// <param name="dtSource">DataTable数据源</param>
        /// <param name="excelConfig">导出设置包含文件名、标题、列设置</param>
        public static void ExcelExport(DataTable dtSource, ExcelConfig excelConfig)
        {
            //try
            //{
            //    using (MemoryStream ms = ExportMemoryStream(dtSource, excelConfig))
            //    {
            //        using (FileStream fs = new FileStream(@excelConfig.FileName, FileMode.Create, FileAccess.Write))
            //        {
            //            Console.WriteLine("下载完成！");
            //            byte[] data = ms.ToArray();
            //            fs.Write(data, 0, data.Length);
            //            fs.Flush();
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    string log = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ex.ToString();
            //    Console.WriteLine(log);
            //}
        }
        #endregion

        #region DataTable导出到Excel的MemoryStream
        /// <summary>
        /// DataTable导出到Excel的MemoryStream Export()
        /// </summary>
        /// <param name="dtSource">DataTable数据源</param>
        /// <param name="excelConfig">导出设置包含文件名、标题、列设置</param>
        public static void MemoryStream (DataTable dtSource, ExcelConfig excelConfig)
        {
            try {

                int colint = 0;
                bool cellValue = false;
                for (int i = 0; i < dtSource.Columns.Count;)
                {
                    DataColumn column = dtSource.Columns[i];
                    if (excelConfig.ColumnEntity[colint].Column != column.ColumnName)
                    {
                        dtSource.Columns.Remove(column.ColumnName);
                    }
                    else
                    {
                        i++;
                        colint++;
                    }
                }
                XSSFWorkbook workbook = new XSSFWorkbook();
                XSSFSheet sheet = workbook.CreateSheet() as XSSFSheet;
                int rowIndex = 0;
                //判断是否已存在文件
                if (File.Exists(excelConfig.FileName))
                {
                    FileStream fs = new FileStream(@excelConfig.FileName, FileMode.Open, FileAccess.ReadWrite);
                    workbook = new XSSFWorkbook(fs);
                    fs.Close();
                    sheet = workbook.GetSheetAt(0) as XSSFSheet;
                    cellValue = true;
                    rowIndex = sheet.LastRowNum + 1;
                }
                #region 设置标题样式
                ICellStyle headStyle = workbook.CreateCellStyle();
                int[] arrColWidth = new int[dtSource.Columns.Count];
                string[] arrColName = new string[dtSource.Columns.Count];//列名
                ICellStyle[] arryColumStyle = new ICellStyle[dtSource.Columns.Count];//样式表
                headStyle.Alignment = HorizontalAlignment.Center; // ------------------
                if (excelConfig.Background != new Color())
                {
                    if (excelConfig.Background != new Color())
                    {
                        headStyle.FillPattern = FillPattern.SolidForeground;
                        // headStyle.FillForegroundColor = GetXLColour(workbook, excelConfig.Background);
                    }
                }
                IFont font = workbook.CreateFont();
                font.FontHeightInPoints = excelConfig.TitlePoint;
                if (excelConfig.ForeColor != new Color())
                {
                    //font.Color = GetXLColour(workbook, excelConfig.ForeColor);
                }
                font.Boldweight = 700;
                headStyle.SetFont(font);
                #endregion

                #region 列头及样式
                ICellStyle cHeadStyle = workbook.CreateCellStyle();
                cHeadStyle.Alignment = HorizontalAlignment.Center; // ------------------
                IFont cfont = workbook.CreateFont();
                cfont.FontHeightInPoints = excelConfig.HeadPoint;
                cHeadStyle.SetFont(cfont);
                #endregion

                #region 设置内容单元格样式
                foreach (DataColumn item in dtSource.Columns)
                {
                    ICellStyle columnStyle = workbook.CreateCellStyle();
                    columnStyle.Alignment = HorizontalAlignment.Center;
                    arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
                    arrColName[item.Ordinal] = item.ColumnName.ToString();
                    if (excelConfig.ColumnEntity != null)
                    {
                        ColumnEntity columnentity = excelConfig.ColumnEntity.Find(t => t.Column == item.ColumnName);
                        if (columnentity != null)
                        {
                            arrColName[item.Ordinal] = columnentity.ExcelColumn;
                            if (columnentity.Width != 0)
                            {
                                arrColWidth[item.Ordinal] = columnentity.Width;
                            }
                            if (columnentity.Background != new Color())
                            {
                                if (columnentity.Background != new Color())
                                {
                                    columnStyle.FillPattern = FillPattern.SolidForeground;
                                    //columnStyle.FillForegroundColor = GetXLColour(workbook, columnentity.Background);
                                }
                            }
                            if (columnentity.Font != null || columnentity.Point != 0 || columnentity.ForeColor != new Color())
                            {
                                IFont columnFont = workbook.CreateFont();
                                columnFont.FontHeightInPoints = 10;
                                if (columnentity.Font != null)
                                {
                                    columnFont.FontName = columnentity.Font;
                                }
                                if (columnentity.Point != 0)
                                {
                                    columnFont.FontHeightInPoints = columnentity.Point;
                                }
                                if (columnentity.ForeColor != new Color())
                                {
                                    // columnFont.Color = GetXLColour(workbook, columnentity.ForeColor);
                                }
                                columnStyle.SetFont(font);
                            }
                            columnStyle.Alignment = getAlignment(columnentity.Alignment);
                        }
                    }
                    arryColumStyle[item.Ordinal] = columnStyle;
                }
                if (excelConfig.IsAllSizeColumn)
                {
                    #region 根据列中最长列的长度取得列宽
                    for (int i = 0; i < dtSource.Rows.Count; i++)
                    {
                        for (int j = 0; j < dtSource.Columns.Count; j++)
                        {
                            if (arrColWidth[j] != 0)
                            {
                                int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                                if (intTemp > arrColWidth[j])
                                {
                                    arrColWidth[j] = intTemp;
                                }
                            }

                        }
                    }
                    #endregion
                }
                #endregion

                #region 填充数据

                #endregion
                ICellStyle dateStyle = workbook.CreateCellStyle();
                IDataFormat format = workbook.CreateDataFormat();
                dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

                foreach (DataRow row in dtSource.Rows)
                {
                    #region 新建表，填充表头，填充列头，样式
                    if (rowIndex == 0)
                    {
                        if (rowIndex != 0)
                        {
                            //sheet = workbook.CreateSheet();
                        }

                        #region 表头及样式
                        {
                            if (excelConfig.Title != null)
                            {
                                IRow headerRow = sheet.CreateRow(0);
                                if (excelConfig.TitleHeight != 0)
                                {
                                    headerRow.Height = (short)(excelConfig.TitleHeight * 20);
                                }
                                headerRow.HeightInPoints = 25;
                                headerRow.CreateCell(0).SetCellValue(excelConfig.Title);
                                headerRow.GetCell(0).CellStyle = headStyle;
                                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1)); // ------------------
                            }

                        }
                        #endregion

                        #region 列头及样式
                        {
                            IRow headerRow = sheet.CreateRow(1);
                            #region 如果设置了列标题就按列标题定义列头，没定义直接按字段名输出
                            foreach (DataColumn column in dtSource.Columns)
                            {
                                headerRow.CreateCell(column.Ordinal).SetCellValue(arrColName[column.Ordinal]);
                                headerRow.GetCell(column.Ordinal).CellStyle = cHeadStyle;
                                //设置列宽
                                //sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                                if (arrColWidth[column.Ordinal] > 255)
                                {
                                    arrColWidth[column.Ordinal] = 254;
                                }
                                else
                                {
                                    sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                                }
                            }
                            #endregion
                        }
                        #endregion

                        rowIndex = 2;
                    }
                    #endregion

                    #region 填充内容
                    IRow dataRow = sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in dtSource.Columns)
                    {
                        ICell newCell = dataRow.CreateCell(column.Ordinal);
                        newCell.CellStyle = arryColumStyle[column.Ordinal];
                        string drValue = row[column].ToString();
                        //排列序号
                        if (column.ColumnName == "Id")
                        {
                            drValue = (rowIndex - 1).ToString();
                        }
                        if (drValue.Length >= 255)
                            drValue = drValue.Remove(255, drValue.Length - 255);
                        SetCell(newCell, dateStyle, column.DataType, drValue);
                    }
                    #endregion
                    rowIndex++;
                }
                sheet.ForceFormulaRecalculation = true;
                //创建或打开文件填充数据
                using (MemoryStream ms = new MemoryStream())
                {
                    workbook.Write(ms);
                    byte[] data = ms.ToArray();
                    using (FileStream fs1 = File.OpenWrite(@excelConfig.FileName))
                    {
                        fs1.Write(data, 0, data.Length);
                        fs1.Flush();
                        fs1.Close();
                        Console.WriteLine("下载完成！");
                    }
                    ms.Flush();
                }
            }
            catch (Exception  e) {
                Console.WriteLine("下载失败,休眠2S！========================>"+e.ToString());
                Thread.Sleep(2000);
                MemoryStream(dtSource, excelConfig);
            }
        }
        #endregion

        #region ListExcel导出(加载模板)
        /// <summary>
        /// List根据模板导出ExcelMemoryStream
        /// </summary>
        /// <param name="list"></param>
        /// <param name="templdateName"></param>
        public static MemoryStream ExportListByTempale(List<TemplateMode> list, string templdateName)
        {
            try
            {

                string templatePath = "";
                string templdateName1 = string.Format("{0}{1}", templatePath, templdateName);

                FileStream fileStream = new FileStream(templdateName1, FileMode.Open, FileAccess.Read);
                ISheet sheet = null;
                if (templdateName.IndexOf(".xlsx") == -1)//2003
                {
                    HSSFWorkbook hssfworkbook = new HSSFWorkbook(fileStream);
                    sheet = hssfworkbook.GetSheetAt(0);
                    SetPurchaseOrder(sheet, list);
                    sheet.ForceFormulaRecalculation = true;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        hssfworkbook.Write(ms);
                        ms.Flush();
                        return ms;
                    }
                }
                else//2007
                {
                    XSSFWorkbook xssfworkbook = new XSSFWorkbook(fileStream);
                    sheet = xssfworkbook.GetSheetAt(0);
                    SetPurchaseOrder(sheet, list);
                    sheet.ForceFormulaRecalculation = true;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        xssfworkbook.Write(ms);
                        ms.Flush();
                        return ms;
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 赋值单元格
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="list"></param>
        private static void SetPurchaseOrder(ISheet sheet, List<TemplateMode> list)
        {
            try
            {
                foreach (var item in list)
                {
                    IRow row = null;
                    ICell cell = null;
                    row = sheet.GetRow(item.row);
                    if (row == null)
                    {
                        row = sheet.CreateRow(item.row);
                    }
                    cell = row.GetCell(item.cell);
                    if (cell == null)
                    {
                        cell = row.CreateCell(item.cell);
                    }
                    cell.SetCellValue(item.value);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 设置表格内容
        private static void SetCell(ICell newCell, ICellStyle dateStyle, Type dataType, string drValue)
        {
            switch (dataType.ToString())
            {
                case "System.String"://字符串类型
                    newCell.SetCellValue(drValue);
                    break;
                case "System.DateTime"://日期类型
                    System.DateTime dateV;
                    if (System.DateTime.TryParse(drValue, out dateV))
                    {
                        newCell.SetCellValue(dateV);
                    }
                    else
                    {
                        newCell.SetCellValue("");
                    }
                    newCell.CellStyle = dateStyle;//格式化显示
                    break;
                case "System.Boolean"://布尔型
                    bool boolV = false;
                    bool.TryParse(drValue, out boolV);
                    newCell.SetCellValue(boolV);
                    break;
                case "System.Int16"://整型
                case "System.Int32":
                case "System.Int64":
                case "System.Byte":
                    int intV = 0;
                    int.TryParse(drValue, out intV);
                    newCell.SetCellValue(intV);
                    break;
                case "System.Decimal"://浮点型
                case "System.Double":
                    double doubV = 0;
                    double.TryParse(drValue, out doubV);
                    newCell.SetCellValue(doubV);
                    break;
                case "System.DBNull"://空值处理
                    newCell.SetCellValue("");
                    break;
                default:
                    newCell.SetCellValue("");
                    break;
            }
        }
        #endregion

        #region 从Excel导入
        /// <summary>
        /// 读取excel ,默认第一行为标头
        /// </summary>
        /// <param name="strFileName">excel文档路径</param>
        /// <returns></returns>
        public static DataTable ExcelImport(string strFileName)
        {
            DataTable dt = new DataTable();
            ISheet sheet = null;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                if (strFileName.IndexOf(".xlsx") == -1)//2003
                {
                    HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
                    sheet = hssfworkbook.GetSheetAt(0);
                }
                else//2007
                {
                    XSSFWorkbook xssfworkbook = new XSSFWorkbook(file);
                    sheet = xssfworkbook.GetSheetAt(0);
                }
            }

            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }

                dt.Rows.Add(dataRow);
            }
            return dt;
        }
        #endregion

        #region RGB颜色转NPOI颜色
        //private static short GetXLColour(XSSFWorkbook workbook, Color SystemColour)
        //{
        //    short s = 0;
        //    XSSFPalette XlPalette = workbook.GetCustomPalette();
        //    NPOI.HSSF.Util.HSSFColor XlColour = XlPalette.FindColor(SystemColour.R, SystemColour.G, SystemColour.B);
        //    if (XlColour == null)
        //    {
        //        if (NPOI.HSSF.Record.PaletteRecord.STANDARD_PALETTE_SIZE < 255)
        //        {
        //            XlColour = XlPalette.FindSimilarColor(SystemColour.R, SystemColour.G, SystemColour.B);
        //            s = XlColour.Indexed;
        //        }

        //    }
        //    else
        //        s = XlColour.Indexed;
        //    return s;
        //}
        #endregion

        #region 设置列的对齐方式
        /// <summary>
        /// 设置对齐方式
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        private static HorizontalAlignment getAlignment(string style)
        {
            switch (style)
            {
                case "center":
                    return HorizontalAlignment.Center;
                case "left":
                    return HorizontalAlignment.Left;
                case "right":
                    return HorizontalAlignment.Right;
                case "fill":
                    return HorizontalAlignment.Fill;
                case "justify":
                    return HorizontalAlignment.Justify;
                case "centerselection":
                    return HorizontalAlignment.CenterSelection;
                case "distributed":
                    return HorizontalAlignment.Distributed;
            }
            return NPOI.SS.UserModel.HorizontalAlignment.General;


        }

        #endregion
    }
}
