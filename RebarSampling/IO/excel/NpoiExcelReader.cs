using System.Data;
using NPOI.SS.UserModel;
using RebarSampling;

namespace demo
{
    /// <summary>
    /// NPOI的Excel读取类的实现
    /// </summary>
    public class NpoiExcelReader : IExcelReader
    {
        private string _fileName;

        /// <summary>
        /// 工作表对象
        /// </summary>
        public NpoiExcelReader(string fileName)
        {
            _fileName = fileName;
        }
        public DataTable Read(int index)    
        {
            IWorkbook workbook = WorkBookFactory.GetWorkBook(_fileName);
            int headerIndex = 0;
            int startIndex = 1;
            try
            {
                ISheet sheet = workbook.GetSheetAt(index);
                //行数
                int rowNum = sheet.LastRowNum;
                if (rowNum == 0)
                {
                    return new DataTable();
                }
                IRow firstRow = sheet.GetRow(sheet.FirstRowNum);
                int colNum = firstRow.Cells.Count;
                //创建列
                DataTable dt = new DataTable();
                foreach (var cell in firstRow.Cells)
                {
                    dt.Columns.Add(cell.StringCellValue, typeof(string));
                }
                //读取数据行
                for (int i = startIndex; i < sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ICell cell = row.GetCell(j);
                        dr[j] = cell.ToString();
                    }
                    dt.Rows.Add(dr);
                }

                return dt;
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                workbook.Close();
            }
        }
    }
}

