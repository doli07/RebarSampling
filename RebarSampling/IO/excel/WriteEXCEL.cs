using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebarSampling
{
    public class ExcelWriter
    {
        //private static string filepath="";
        //private FileStream fileStream = null;

        /// <summary>
        /// HSSFWorkbook : 用于读取excel2007版本以下的xls文件
        /// XSSFWorkbook : 用于读取.xlsx 文件
        /// </summary>
        public IWorkbook wb = null;

        /// <summary>
        /// 将datatale保存到excel
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="_dt"></param>
        public void SaveSheet(string filename,DataTable _dt)
        {
            wb = new XSSFWorkbook();//For .xlsx files
            //wb = new HSSFWorkbook();//For .xls files

            //create sheet
            ISheet _sheet = wb.CreateSheet("sheet1");

            //header row
            IRow _headerRow = _sheet.CreateRow(0);//第一行，列名
            for(int i=0;i< _dt.Columns.Count;i++)
            {
                _headerRow.CreateCell(i).SetCellValue(_dt.Columns[i].ColumnName);//dt的列名做sheet列名                
            }

            //fill sheet
            for(int i=0;i<_dt.Rows.Count;i++)
            {
                IRow _row = _sheet.CreateRow(i + 1);
                for (int j = 0; j < _dt.Columns.Count; j++)
                {
                    _row.CreateCell(j).SetCellValue(_dt.Rows[i][j].ToString());
                }
            }

            // Auto-size columns 
            for (int i = 0; i < _dt.Columns.Count; i++)
            {
                _sheet.AutoSizeColumn(i);
            }

            using (FileStream _stream = new FileStream(filename,FileMode.Create,FileAccess.Write))
            {
                wb.Write(_stream,false);//leaveopen，写入完成后记得要关闭数据流，这里务必选false
            }

        }


    }
}
