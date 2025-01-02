using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebarSampling.Database
{
    public  interface IDBHelper
    {

        bool ConnectDB(string _filepath, string _psd = "");
        bool CreateTable(string _tableName);
        bool AddColumn(string _tableName, string _colName, string _datatype);

        bool ExecuteSqlsTran(List<string> a_listSqls);

        bool IsTableEXIST(string tableName);
        /// <summary>
        /// 删除表中的特定行，可以逐行删除，保留表结构，但不释放空间
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        bool DeleteTable(string tablename);
        /// <summary>
        /// 删除表中的所有数据，保留表结构，释放空间
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        bool ClearTable(string tablename);

        DataTable ExecuteQuery(string _sql);

        DataTable GetDataTable(string tableName);

        bool DeleteTableSequence(string tableName);
        /// <summary>
        /// 根据DbType得到数据库表中不同的数据类型string
        /// </summary>
        /// <param name="_dbtype"></param>
        /// <returns></returns>
        string GetDataType(DbType _dbtype);
    }
}
