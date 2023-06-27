
using System;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace demo
{
    /// <summary>
    /// 工作薄类型
    /// </summary>
    public enum WorkBookType
    {
        /// <summary>
        /// xls
        /// </summary>
        xls,
        /// <summary>
        /// xlsx
        /// </summary>
        xlsx
    }

    /// <summary>
    /// 工作薄工厂类,根据类型创建IWorkbook的实例
    /// </summary>
    public class WorkBookFactory
    {
        /// <summary>
        /// 创建一个工作簿对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IWorkbook Create(WorkBookType type)
        {
            IWorkbook workbook = null;
            switch (type)
            {
                case WorkBookType.xls:
                    workbook = new HSSFWorkbook();
                    break;
                case WorkBookType.xlsx:
                    workbook = new XSSFWorkbook();
                    break;
                default:
                    break;
            }
            return workbook;
        }
        /// <summary>
        /// 根据文件名获取工作簿对象
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static IWorkbook GetWorkBook(string file)
        {
            string ext = Path.GetExtension(file).ToLower();
            if (ext != ".xls" && ext != ".xlsx")
            {
                throw new Exception("excel文件格式不正确");
            }
            using (FileStream readStram = File.OpenRead(file))
            {
                IWorkbook workbook = null;
                if (ext == ".xls")
                {
                    workbook = new HSSFWorkbook(readStram);
                }
                else
                {
                    workbook = new XSSFWorkbook(readStram);
                }
                return workbook;
            }
        }
    }
}


