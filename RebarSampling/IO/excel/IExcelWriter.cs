using NPOI.SS.UserModel;
using System;
using System.Data;

namespace demo
{

    /// <summary>
    /// Excel文件写入接口
    /// </summary>
    public interface IExcelWriter
    {
        /// <summary>
        /// 把DataTable的数据写入Workbook
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        IWorkbook Write(DataTable dt);
        /// <summary>
        /// 追加一行数据
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        IWorkbook AppendRow(params Object[] values);
        /// <summary>
        /// 保存到文件中
        /// </summary>
        /// <param name="file"></param>
        void SaveAs(string file);
    }
}

