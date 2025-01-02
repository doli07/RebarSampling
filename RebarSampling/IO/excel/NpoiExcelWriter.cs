using NPOI.SS.UserModel;
using RebarSampling;
using System.Data;
using System.IO;

namespace demo
{
    /// <summary>
    /// NPOI 基础实现类
    /// </summary>
    public class NpoiExcelWriter : IExcelWriter
    {
        /// <summary>
        /// 工作薄类型
        /// </summary>
        WorkBookType _type;
        /// <summary>
        /// 工作薄对象
        /// </summary>
        IWorkbook _workbook;
        /// <summary>
        /// 工作表对象
        /// </summary>
        ISheet _sheet;
        /// <summary>
        /// 当前的行号
        /// </summary>
        volatile int _rownum;
        public NpoiExcelWriter(WorkBookType type)
        {
            _type = type;
            _workbook = WorkBookFactory.Create(type);
            _sheet = _workbook.CreateSheet();
        }

        public void SaveAs(string file)
        {
            using (FileStream stream = File.Create(file, 1024))
            {
                _workbook?.Write(stream, false);
                _workbook?.Close();
            }
        }

        public IWorkbook Write(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row = _sheet.CreateRow(_rownum);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ICell cell = row.CreateCell(j);
                    string value = dt.Rows[i][j]?.ToString();
                    cell.SetCellValue(value);
                }
                _rownum++;
            }
            return _workbook;
        }
        /// <summary>
        /// 追加一行数据
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public IWorkbook AppendRow(params object[] values)
        {
            if (values == null)
            {
                return _workbook;
            }
            var row = _sheet.CreateRow(_rownum);
            for (int j = 0; j < values.Length; j++)
            {
                ICell cell = row.CreateCell(j);
                cell.SetCellValue(values[j].ToString());
            }
            _rownum++;
            return _workbook;
        }
    }
}

