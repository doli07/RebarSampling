using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.IO;
using System.Data;
using NPOI.SS.Formula.Functions;

namespace RebarSampling
{
    public class ExcelReader
    {

        //private static string filepath="";
        private FileStream fileStream = null;
        /// <summary>
        /// HSSFWorkbook : 用于读取excel2007版本以下的xls文件
        /// XSSFWorkbook : 用于读取.xlsx 文件
        /// </summary>
        public IWorkbook wb = null;


        /// <summary>
        /// .xls是2003版Office Microsoft Office Excel 工作表的格式
        /// .xlsx是2007版Office Microsoft Office Excel 工作表的格式
        /// </summary>
        /// <param name="filename"></param>
        public void OpenFile(string filename)
        {
            // 加载 Excel 文件
            fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);

            string ext = Path.GetExtension(filename).ToLower();//获取文件名的扩展名，以选择用哪种workbook

            if (ext != ".xls" && ext != ".xlsx")
            {
                throw new Exception("excel文件格式不正确");
            }
            if (ext == ".xls")
            {
                wb = new HSSFWorkbook(fileStream);
            }
            else
            {
                wb = new XSSFWorkbook(fileStream);
            }
        }

        public void CloseFile()
        {
            if (fileStream != null)
            {
                fileStream.Close();
            }
        }
        public List<DataTable> GetAllSheets(string filename)
        {
            try
            {
                OpenFile(filename);

                List<DataTable> _dtlist = new List<DataTable>();

                for (int k = 0; k < wb?.NumberOfSheets; k++)//
                {
                    ISheet sheet = wb.GetSheetAt(k);
                    //行数
                    int rowNum = sheet.LastRowNum;
                    if (rowNum == 0)
                    {
                        continue;
                    }
                    IRow firstRow = sheet.GetRow(sheet.FirstRowNum);//
                    int colNum = firstRow.Cells.Count;
                    //创建列
                    DataTable dt = new DataTable();
                    dt.TableName = sheet.SheetName;//获取sheetname
                    foreach (var cell in firstRow.Cells)
                    {
                        dt.Columns.Add(cell.StringCellValue, typeof(string));//以firstrow的名称作为datatable列名
                    }

                    //读取数据行
                    for (int i = 0; i <= sheet.LastRowNum; i++)//注意此处为<=，sheet.lastRowNum从0开始，20240517解决bug
                    {
                        IRow row = sheet.GetRow(i);
                        DataRow dr = dt.NewRow();
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            ICell cell = row?.GetCell(j);
                            dr[j] = cell?.ToString();
                        }
                        dt.Rows.Add(dr);
                    }
                    _dtlist.Add(dt);

                }

                return _dtlist;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { CloseFile(); }
        }
        /// <summary>
        /// 获取excel的单个sheet，并获取到项目名称和构件部位，注意每张sheet对应有自己的项目名称和构件部位，所以此处返回值用tuple元组
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Tuple<string,string,DataTable>> GetAllSheet(string filename)
        {
            try
            {
                OpenFile(filename);

                List<Tuple<string, string, DataTable>> _dtlist = new List<Tuple<string, string, DataTable>>();

                string _lastname = wb?.GetSheetName(wb.NumberOfSheets - 1);//获取最后一个sheet的名字，如果是“封面”，那就不取这个sheet的数据
                int _totalnum= _lastname.Equals("封面")? wb.NumberOfSheets - 1:wb.NumberOfSheets;

                for (int k = 0; k < _totalnum; k++)//去掉最后一个sheet的统计表
                {
                    ISheet sheet = wb.GetSheetAt(k);

                    //获取项目名称和构件部位
                    string project=sheet.GetRow(1).GetCell(0).ToString();
                    string assembly=sheet.GetRow(1).GetCell(6).ToString();
                    project = project.Split('：')[1];//例如：项目名称：汉韵公馆地库
                    assembly = assembly.Split('：')[1];//例如：构件：地下室承台 1#楼北侧

                    //行数
                    int rowNum = sheet.LastRowNum;
                    if (rowNum == 0)
                    {
                        continue;
                    }
                    IRow firstRow = sheet.GetRow(sheet.FirstRowNum + 2);//从第三行开始
                    int colNum = firstRow.Cells.Count;
                    //创建列
                    DataTable dt = new DataTable();
                    dt.TableName = sheet.SheetName;//获取sheetname
                    foreach (var cell in firstRow.Cells)
                    {
                        dt.Columns.Add(cell.StringCellValue, typeof(string));//以firstrow的名称作为datatable列名
                    }
                    dt.Columns.Add("序号",typeof(string));//最后一列是流水号，但没有列名

                    int startIndex = 3;//从第三行开始
                    //读取数据行
                    for (int i = startIndex; i <= sheet.LastRowNum; i++)//注意此处为<=，sheet.lastRowNum从0开始，20240517解决bug
                    {
                        IRow row = sheet.GetRow(i);
                        DataRow dr = dt.NewRow();
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            ICell cell = row?.GetCell(j);
                            dr[j] = cell?.ToString();
                        }
                        dt.Rows.Add(dr);
                    }
                    _dtlist.Add(new Tuple<string, string, DataTable>(project,assembly,dt));

                }

                return _dtlist;


            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally { CloseFile(); }



        }

        //根据cell数据的不同类型，分别进行解析
        public string getCellStringValueAllCase(NPOI.SS.UserModel.ICell tCell)
        {
            string tempValue = "";
            switch (tCell.CellType)
            {
                case NPOI.SS.UserModel.CellType.Blank://空值
                    break;
                case NPOI.SS.UserModel.CellType.Boolean://bool型
                    tempValue = tCell.BooleanCellValue.ToString();
                    break;
                case NPOI.SS.UserModel.CellType.Error://错误
                    break;
                case NPOI.SS.UserModel.CellType.Formula://公式型
                    NPOI.SS.UserModel.IFormulaEvaluator fe = NPOI.SS.UserModel.WorkbookFactory.CreateFormulaEvaluator(tCell.Sheet.Workbook);
                    var cellValue = fe.Evaluate(tCell);
                    switch (cellValue.CellType)
                    {
                        case NPOI.SS.UserModel.CellType.Blank:
                            break;
                        case NPOI.SS.UserModel.CellType.Boolean:
                            tempValue = cellValue.BooleanValue.ToString();
                            break;
                        case NPOI.SS.UserModel.CellType.Error:
                            break;
                        case NPOI.SS.UserModel.CellType.Formula:
                            break;
                        case NPOI.SS.UserModel.CellType.Numeric:
                            tempValue = cellValue.NumberValue.ToString();
                            break;
                        case NPOI.SS.UserModel.CellType.String:
                            tempValue = cellValue.StringValue.ToString();
                            break;
                        case NPOI.SS.UserModel.CellType.Unknown:
                            break;
                        default:
                            break;
                    }
                    break;
                case NPOI.SS.UserModel.CellType.Numeric://数值型

                    if (NPOI.SS.UserModel.DateUtil.IsCellDateFormatted(tCell))
                    {
                        tempValue = tCell.DateCellValue?.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        tempValue = tCell.NumericCellValue.ToString();
                    }
                    break;
                case NPOI.SS.UserModel.CellType.String://字符串型
                    tempValue = tCell.StringCellValue.Trim();
                    break;
                case NPOI.SS.UserModel.CellType.Unknown://未知类型-1
                    break;
                default:
                    break;
            }
            return tempValue;
        }









    }
}
