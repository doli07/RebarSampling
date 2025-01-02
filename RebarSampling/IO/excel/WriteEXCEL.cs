using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebarSampling
{
    public class WriteEXCEL
    {
        //private static string filepath="";
        private FileStream fileStream = null;
        /// <summary>
        /// HSSFWorkbook : 用于读取excel2007版本以下的xls文件
        /// XSSFWorkbook : 用于读取.xlsx 文件
        /// </summary>
        public IWorkbook wb = null;

        public void SaveFile(string filename)
        {
            //wb.sa
        }


    }
}
