using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using WebBiHuGadget.Models;
namespace WebBiHuGadget.Helpers
{
    public class OperateExcelWps
    {
        public List<XlsModel> ExcelToDSWps(string path)
        {
            List<XlsModel> list = new List<XlsModel>();
            Type type;
            type = Type.GetTypeFromProgID("ET.Application");
            if (type == null)
            {
                type = Type.GetTypeFromProgID("Ket.Application");
                if (type == null)
                {
                    type = Type.GetTypeFromProgID("EXCEL.Application");
                    if (type == null)
                    {
                        return null;
                    }
                }
            }
            string fileName = Path.GetFileNameWithoutExtension(path);
            dynamic app = Activator.CreateInstance(type);
            app.Visible = false;
            dynamic workbook = app.Workbooks.Open(path);
            dynamic worksheet = workbook.Worksheets[fileName];
            for (int i = 2; i < 10000; i++)
            {
                var name = worksheet.Range["A" + i].Text.ToString();
                var value = worksheet.Range["B" + i].Text.ToString();
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
                {
                    break;
                }
                list.Add(new XlsModel
                {
                    UserName = name,
                    PunchCardTime = value
                });
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
            workbook.Close();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            app.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
            return list;
        }
        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public List<XlsModel> ExcelToDataTable(string path, bool isFirstRowColumn)
        {
            string fileName = Path.GetFileNameWithoutExtension(path);
            ISheet sheet = null;
            DataTable data = new DataTable();
            IWorkbook workbook = null;
            FileStream fs = null;
            int startRow = 0;
            try
            {
                fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                if (path.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (path.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);

                if (fileName != null)
                {
                    sheet = workbook.GetSheet(fileName);
                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }
                List<XlsModel> list = new List<XlsModel>();
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    string name = data.Rows[i][0].ToString();
                    string value = data.Rows[i][1].ToString();
                    list.Add(new XlsModel
                    {
                        UserName = name,
                        PunchCardTime = value
                    });
                }
                return list;
            }
            catch (Exception ex)
            {
                LogCollectHelper.ErrorLog("读取考勤数据：" + ex.ToString());
            }
            return null;
        }
    }
}