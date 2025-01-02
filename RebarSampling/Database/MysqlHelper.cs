using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using RebarSampling.Database;

namespace RebarSampling
{
    /// <summary>
    /// MYSQL操作类
    /// </summary>
    public class MysqlHelper : IDBHelper
    {
        /// <summary>
        /// 连接对象
        /// </summary>
        private MySqlConnection conn = null;
        /// <summary>
        /// 语句执行对象
        /// </summary>
        private MySqlCommand cmd = null;
        /// <summary>
        /// 语句执行结果数据对象
        /// </summary>
        private MySqlDataReader reader = null;
        /// <summary>
        /// 连接string
        /// </summary>
        private string connectString { get; set; }

        private const string dbName = "rebarDB";
        private const string userID = "root";
        private const string password = "919121";
        private const string server = "localhost";
        private const int port = 3306;

        public MysqlHelper()
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();//直接写链接语句，比较容易出错，使用Builder写语句，实现分段
            builder.UserID = userID;//用户名
            builder.Password = password;//密码
            builder.Server = server;//服务器地址
            builder.Database = dbName;//要连接的数据库 
            builder.Pooling = false;
            builder.Port = port;
            connectString = builder.ConnectionString;

            conn = new MySqlConnection(connectString);//连接string
            //conn.Open();//打开连接
        }
        public MysqlHelper(string _pwd = password, string _server = server, string _db = dbName, string _user = userID)
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();//直接写链接语句，比较容易出错，使用Builder写语句，实现分段
            builder.UserID = _user;
            builder.Password = _pwd;
            builder.Server = _server;
            builder.Database = _db;
            builder.Pooling = false;
            builder.Port = 3306;
            connectString = builder.ConnectionString;

            conn = new MySqlConnection(builder.ConnectionString);//连接string
        }

        public MysqlHelper(string _connectString)
        {
            connectString = _connectString;
            conn = new MySqlConnection(connectString);
        }
        public bool ConnectDB(string _filepath, string _psd = "")
        {
            return true;//mysql的这个连接数据库接口没什么用，构造里面自动连了
        }
        /// <summary>
        /// 查询连接状态
        /// </summary>
        /// <returns></returns>
        public bool CheckConnectState()
        {
            using (conn = new MySqlConnection(connectString))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        return true;
                    }
                    else { return false; }
                }
                catch (Exception ex)
                {
                    return false;
                    throw ex;
                }
                finally { conn.Close(); }
            }
        }
        ///// <summary>
        ///// 用于增、删、改操作方法
        ///// </summary>
        ///// <param name="sql"></param>
        ///// <returns></returns>
        //public int CmdExecute(string sql)
        //{
        //    using (conn = new MySqlConnection(connectString))
        //    {
        //        using (cmd = new MySqlCommand(sql, conn))
        //        {
        //            int _ret = -1;
        //            try
        //            {
        //                conn.Open();
        //                _ret = cmd.ExecuteNonQuery();

        //                return _ret;
        //            }
        //            catch (Exception ex)
        //            {
        //                return _ret;
        //                throw ex;
        //            }
        //            finally { cmd.Dispose(); conn.Close(); }
        //        }
        //    }
        //}





        /// <summary>
        /// 创建新表
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public bool CreateTable(string _tableName)
        {
            using (conn = new MySqlConnection(connectString))
            {
                string _cmdstring = "CREATE TABLE IF NOT EXISTS " + _tableName + "(id INTEGER PRIMARY KEY AUTO_INCREMENT) ";//新建表，以id为主键,并且实现自增

                using (cmd = new MySqlCommand(_cmdstring, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                        return false;

                    }
                    finally { cmd.Dispose(); conn.Close(); }
                }
            }
        }

        /// <summary>
        /// 删除表数据
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        public bool DeleteTable(string tablename)
        {
            using (conn = new MySqlConnection(connectString))
            {
                string _cmdstring = "DELETE FROM " + tablename;//删除表数据

                using (cmd = new MySqlCommand(_cmdstring, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                        throw ex;
                    }
                    finally { cmd.Dispose(); conn.Close(); }
                }
            }
        }

        public bool ClearTable(string tablename)
        {
            using (conn = new MySqlConnection(connectString))
            {
                string _cmdstring = "TRUNCATE TABLE " + tablename;//清空表数据

                using (cmd = new MySqlCommand(_cmdstring, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                        throw ex;
                    }
                    finally { cmd.Dispose(); conn.Close(); }
                }
            }
        }

        /// <summary>
        /// 对于有自增列的表，删除整表数据时清空sqlite_sequence
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool DeleteTableSequence(string tableName)
        {
            try
            {
                string sqlstr = "TRUNCATE TABLE " + tableName ;//当你仍要保留该表、仅删除所有数据表内容时，用truncate

                int rt = ExecuteCmd(sqlstr);

                return true;
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="tablename">表名称</param>
        /// <returns></returns>
        public bool DropTable(string tablename)
        {
            using (conn = new MySqlConnection(connectString))
            {
                string _cmdstring = "DROP TABLE IF EXISTS " + tablename;

                using (cmd = new MySqlCommand(_cmdstring, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                        throw ex;
                    }
                    finally { cmd.Dispose(); conn.Close(); }
                }
            }
        }

        /// <summary>
        /// 在指定表中添加列
        /// </summary>
        /// <param name="a_strTableName">表名称</param>
        /// <param name="a_strCol">要添加的列名称</param>
        /// <param name="a_strDataType">列数据类型</param>
        /// <returns></returns>
        public bool AddColumn(string a_strTableName, string a_strCol, string a_strDataType)
        {
            string _cmdstring = "ALTER TABLE " + a_strTableName + " ADD COLUMN " + a_strCol + " " + a_strDataType;

            using (conn = new MySqlConnection(connectString))
            {
                using (cmd = new MySqlCommand(_cmdstring, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                        throw ex;
                    }
                    finally { cmd.Dispose(); conn.Close(); }
                }
            }
        }

        /// <summary>
        /// 带事务回滚批量执行脚本，启用事务，提升sqlite读写效率
        /// </summary>
        /// <param name="a_listSqls">SQL脚本</param>
        /// <returns></returns>
        public bool ExecuteSqlsTran(List<string> a_listSqls)
        {

            using (conn = new MySqlConnection(connectString))
            {
                using (cmd = new MySqlCommand(string.Empty, conn))//先给个空的cmdtext
                {
                    //using (DbTransaction trans = connection.BeginTransaction())     //启动事务
                    {
                        try
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();//启动事务,大幅提高读写速率，注意connection要先open

                            try
                            {
                                //connection.Open();
                                //DbTransaction trans = connection.BeginTransaction();
                                foreach (string sql in a_listSqls)
                                {
                                    cmd.CommandText = sql;
                                    cmd.ExecuteNonQuery();
                                }
                                trans.Commit();                         //事务提交
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("ExecuteNonQuery error:" + e.Message);
                                trans.Rollback();                   //事务回滚
                            }

                            return true;
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                        finally
                        {
                            //trans.Dispose();
                            cmd.Dispose();
                            conn.Close();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="_sql"></param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string _sql)
        {
            DataTable dt = null;
            using (conn = new MySqlConnection(connectString))
            {
                using (cmd = new MySqlCommand(_sql, conn))
                {
                    try
                    {
                        conn.Open();

                        dt = new DataTable();

                        using (reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            dt.Load(reader);
                        }
                        return dt;
                    }
                    catch (Exception ex)
                    {
                        return dt;
                        throw ex;
                    }
                    finally { cmd.Dispose(); conn.Close(); }
                }
            }
        }

        /// <summary>
        /// 根据tableName获取DataTable数据
        /// </summary>
        /// <param name="tableName">传入表格名或者带表格名的语句</param>
        /// <returns></returns>
        public DataTable GetDataTable(string tableName)
        {
            try
            {
                if (!this.IsTableEXIST(tableName))
                {
                    return null;
                }
                string sqlstr = "select * from " + tableName;
                DataTable temptable = ExecuteQuery(sqlstr);

                return temptable;
            }
            catch (SqlException e)
            {
                //System.Windows.Forms.MessageBox.Show(e.Message);
                return null;
                throw e;
            }
        }
        /// <summary>
        /// 增、删、改公共方法，不含查询的方法
        /// </summary>
        /// <param name="_sql"></param>
        /// <returns></returns>
        public int ExecuteCmd(string _sql)
        {
            int ret = -1;
            using (conn = new MySqlConnection(connectString))
            {
                using (cmd = new MySqlCommand(_sql, conn))
                {
                    try
                    {
                        conn.Open();
                        ret = cmd.ExecuteNonQuery();
                        return ret;
                    }
                    catch (Exception ex)
                    {
                        return ret;
                        throw ex;
                    }
                    finally { cmd.Dispose(); conn.Close(); }
                }
            }
        }

        /// <summary>
        /// 检索table是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool IsTableEXIST(string tableName)
        {
            using (conn = new MySqlConnection(connectString))
            {
                // --dbName 为数据库名字 tablename 为你需要查询的表名称，注意使用 TEMPORARY 关键字创建出来的临时表无法查询出来。
                //string sqlstr = "select TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA =‘" + dbName + "’ and TABLE_NAME =‘" + tableName + "'";

                string sqlstr = $"SHOW TABLES LIKE '{tableName}'";

                using (cmd = new MySqlCommand(sqlstr, conn))
                {
                    try
                    {
                        conn.Open();
                        //object ob = cmd.ExecuteScalar();
                        //if (ob.ToString().Equals(tableName))//如果返回的值就是表名，则存在
                        //{
                        //    return true;
                        //}
                        //else { return false; }

                        using (reader = cmd.ExecuteReader()) { return reader.Read(); }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                        return false;
                    }
                    finally { cmd.Dispose(); conn.Close(); }
                }
            }
            //try
            //{
            //   // --dbName 为数据库名字 tablename 为你需要查询的表名称，注意使用 TEMPORARY 关键字创建出来的临时表无法查询出来。
            //    string sqlstr = "select TABLE_NAME from INFORMATION_SCHEMA.TABLES whereTABLE_SCHEMA =‘"+dbName+"’ and TABLE_NAME =‘"+ tableName + "'";
            //    object ob = ExecuteScalar(sqlstr, new SQLiteParameter[] { new SQLiteParameter("tableName", tableName) });
            //    if (Convert.ToInt16(ob) == 1)
            //    {
            //        return true;
            //    }
            //    else { return false; }
            //}
            //catch (System.Exception ex)
            //{
            //    System.Windows.Forms.MessageBox.Show(ex.Message);
            //    return false;
            //}
        }



        /// <summary>
        /// 根据DbType得到数据库表中不同的数据类型string
        /// </summary>
        /// <param name="_dbtype"></param>
        /// <returns></returns>
        public string GetDataType(DbType _dbtype)
        {
            switch (_dbtype)
            {
                case DbType.String:
                    return "TEXT";
                case DbType.Int32:
                    return "INT";
                case DbType.Boolean:
                    return "TINYINT";
                case DbType.Double:
                    return "DOUBLE";
                case DbType.Date:
                    return "DATE";
                case DbType.DateTime:
                    return "DATETIME";
                default:
                    return string.Empty;
            }
        }

    }
}
