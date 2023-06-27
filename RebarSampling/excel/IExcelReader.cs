using System.Data;

namespace demo
{
    /// <summary>
    /// Excel读取接口
    /// </summary>
    public interface IExcelReader
    {
        /// <summary>
        /// 读取文件为DataTable
        /// </summary>
        /// <param name="sheet">工作表索引</param>
        /// <returns></returns>
        DataTable Read(int sheet);
    }
}
