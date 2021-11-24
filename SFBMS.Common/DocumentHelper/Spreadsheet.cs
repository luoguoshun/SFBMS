using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SFBMS.Common.DocumentHelper
{
    public class Spreadsheet
    {
        private readonly ILogger<Spreadsheet> _logger;
        public Spreadsheet(ILogger<Spreadsheet> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 从文本流中打开
        /// </summary>
        /// <typeparam name="TClass"></typeparam>
        /// <param name="file"></param>
        /// <returns></returns>
        public (List<TClass>, string) GetDataListFromExcel<TClass>(Stream file) where TClass : new()
        {
            using var document = SpreadsheetDocument.Open(file, false);
            return GetDataList<TClass>(document);
        }
        /// <summary>
        /// 根据文件路径打开
        /// </summary>
        /// <typeparam name="TClass"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public (List<TClass>, string) GetDataListFromExcel<TClass>(string filePath) where TClass : new()
        {
            using var document = SpreadsheetDocument.Open(filePath, false);
            return GetDataList<TClass>(document);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <typeparam name="TClasas"></typeparam>
        /// <param name="document"></param>
        /// <returns></returns>
        protected (List<TClasas>, string) GetDataList<TClasas>(SpreadsheetDocument document) where TClasas : new()
        {
            try
            {
                List<TClasas> data = new List<TClasas>();
                Sheet sheet = document.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>().FirstOrDefault();
                if (sheet is null)
                {
                    return (null, "找不到文件");
                }
                WorksheetPart worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(sheet.Id.Value);
                Worksheet worksheet = worksheetPart.Worksheet;
                //获取所有行
                List<Row> rows = worksheet.Descendants<SheetData>().FirstOrDefault().Descendants<Row>().ToList();
                for (int i = 1; i < rows.Count; i++)
                {
                    int cellIndex = 0;
                    TClasas baseC = new TClasas();
                    PropertyInfo[] properties = baseC.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    foreach (Cell cell in rows[i].Take(properties.Length))
                    {
                        if (cellIndex + 1 > properties.Length) break;
                        string text = string.Empty;
                        //单元格不为空值
                        if (cell.CellValue != null)
                        {
                            // 判断单元格的类型是否为共享字符串(对于数字和日期类型， DataType 属性的值为 null)
                            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
                            {
                                SharedStringTablePart shareStringTablePart = document.WorkbookPart.SharedStringTablePart;
                                if (shareStringTablePart is null)
                                {
                                    return (null, "找不到共享字符串");
                                }
                                int shareStringId = Convert.ToInt32(cell.CellValue.Text);
                                SharedStringItem item = shareStringTablePart.SharedStringTable
                                                        .Elements<SharedStringItem>()
                                                        .ElementAt(shareStringId);
                                text = item.InnerText;
                            }
                            else
                            {
                                text = cell.CellValue.Text;
                            }
                        }
                        else
                        {
                            Type propertyType = properties[cellIndex].PropertyType;
                            switch (propertyType.Name)
                            {
                                case "String":
                                    text = "暂无信息";
                                    break;
                                case "Int32":
                                    text = "0";
                                    break;
                                case "DateTime":
                                    text = "2001/01/01 00:00:00";
                                    break;
                            }
                        }
                        object value = Convert.ChangeType(text, properties[cellIndex].PropertyType);                       
                        properties[cellIndex].SetValue(baseC, value, null);
                        cellIndex++;
                    }
                    data.Add(baseC);
                }
                return (data, "ok");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return (null, ex.Message);
            }
        }

    }
}
