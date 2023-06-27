using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Collections.Generic;
using System.Data.Common;
using System.Security.Cryptography;
using System.Linq;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace RebarSampling
{
    /// <summary>
    /// SQLite帮助类
    /// 创建人：gyc
    /// 创建事件：2022-03-31
    /// 说明：负责Sqlite操纵，支持数据库创建、删除，表增删改，事务回滚等，已完善。
    ///       Data Source=DB\DBPlayer.sqlite3;Version=3;
    /// 使用过程中发现错误，请联系作者修改 https://blog.csdn.net/youcheng_ge
    /// </summary>
    public class SQLiteHelper
    {
        public string DB_FilePath { get; set; }
        public string DB_Password { get; set; }
        public string DB_version { get; set; }
        public string m_DBConnection { get; set; }


        /// <summary>
        /// 构造函数
        /// </summary>
        public SQLiteHelper()
        {
            //this.m_DBConnection = AppConfig.GetValue("m_DBConnection");
        }

        public bool ConnectDB(string _filepath, string _psd = "")
        {
            try
            {
                //创建数据库
                string filepath = _filepath;
                if (!File.Exists(filepath))
                {
                    CreateDBFile(filepath);//如果数据库文件不存在，则创建新的文件
                }
                CreateConnection(filepath);

                return true;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                return false;
                throw;
            }
        }

        /// <summary>
        /// 构造函数(指定数据库)
        /// </summary>
        /// <param name="datasource"></param>
        /// <param name="password"></param>
        /// <param name="version"></param>
        public void CreateConnection(string datasource, string password = "", int version = 3)
        {
            this.DB_FilePath = datasource;
            this.DB_Password = password;
            this.DB_version = version.ToString();

            //this.m_DBConnection = $"Data Source={DB_FilePath};password={DB_Password},Version={DB_version};";
            this.m_DBConnection = $"Data Source={DB_FilePath};";

        }


        /// <summary>
        /// 创建数据库文件
        /// </summary>
        /// <param name="filePath">文件名</param>
        public void CreateDBFile(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            //if (extension != "sqlite3")
            //{
            //    filePath = Path.ChangeExtension(filePath, "sqlite3");
            //}
            if (extension != "db")
            {
                filePath = Path.ChangeExtension(filePath, "db");
            }

            if (!File.Exists(filePath))
            {
                try
                {
                    SQLiteConnection.CreateFile(filePath);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// 删除数据库
        /// </summary>
        /// <param name="filePath">文件名</param>
        public void DeleteDBFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }




        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="tablename">表名称</param>
        /// <returns></returns>
        public bool DropTable(string tablename)
        {
            using (SQLiteConnection connection = new SQLiteConnection(m_DBConnection))
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    try
                    {
                        connection.Open();
                        command.CommandText = "DROP TABLE IF EXISTS " + tablename;
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                        throw ex;
                    }
                    finally { command.Dispose(); connection.Close(); }
                }
            }
        }

        public bool CreatTable(string tablename)
        {
            using (SQLiteConnection connection = new SQLiteConnection(m_DBConnection))
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    try
                    {
                        connection.Open();
                        command.CommandText = "CREATE TABLE IF NOT EXISTS " + tablename + "(id INTEGER PRIMARY KEY AUTOINCREMENT) ";//新建表，以id为主键,并且实现自增
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                        throw ex;
                    }
                    finally { command.Dispose(); connection.Close(); }
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
            using (SQLiteConnection connection = new SQLiteConnection(m_DBConnection))
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    try
                    {
                        connection.Open();
                        command.CommandText = "DELETE FROM " + tablename;
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                        throw ex;
                    }
                    finally { command.Dispose(); connection.Close(); }
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
                string sqlstr = "select * from " + tableName;
                //DataTable temptable = GeneralClass.SQLiteHelper.ExecuteQuery(sqlstr, null);
                DataTable temptable = ExecuteQuery(sqlstr, null);

                return temptable;
            }
            catch (SqlException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                return null;
            }
        }


        /// <summary>
        /// sql查找指令：select  _item  from _tablename  where _finditem=_findvalue，输出_item的值_itemvalue
        /// 如果找到了返回true，找不到返回false
        /// </summary>
        /// <returns>如果找到了返回true，找不到返回false</returns>
        public bool FindItemValue(string _tablename, string _item, out string _itemvalue, string _finditem, string _findvalue)
        {
            try
            {
                string sqlstr = "select [" + _item + "] from " + _tablename + " where [" + _finditem + "]='" + _findvalue + "'";

                SQLiteDataReader reader = ExecuteReader(sqlstr, null);

                while (reader != null && reader.Read())
                {
                    _itemvalue = reader[_item].ToString();
                    return true;
                }
                _itemvalue = "";
                return false;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("FindItemValue出错：" + ex.Message);
                _itemvalue = "";
                return false;
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
            using (SQLiteConnection connection = new SQLiteConnection(m_DBConnection))
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    try
                    {
                        connection.Open();
                        command.CommandText = "ALTER TABLE " + a_strTableName + " ADD COLUMN " + a_strCol + " " + a_strDataType;
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                        throw ex;
                    }
                    finally { command.Dispose(); connection.Close(); }
                }
            }
        }


        /// <summary>
        /// 根据行号删除数据
        /// </summary>
        /// <param name="a_strTableName">表名</param>
        /// <param name="a_intRowNum">行号</param>
        /// <returns></returns>
        public bool DeleteRowByNum(string a_strTableName, int a_intRowNum)
        {
            using (SQLiteConnection connection = new SQLiteConnection(m_DBConnection))
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    try
                    {
                        connection.Open();
                        command.CommandText = "delete from " + a_strTableName + " where rowid = (select rowid from " + a_strTableName + " Limit " + a_intRowNum + ", 1)";
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                        throw ex;
                    }
                    finally
                    {
                        command.Dispose();
                        connection.Close();
                    }
                }
            }
        }


        /// <summary>
        /// 不带参数，SQL执行语句
        /// update、delete、insert
        /// </summary>
        /// <param name="a_Sql">SQL</param>
        /// <returns></returns>
        public int Execute(string a_Sql)
        {
            SQLiteConnection connection = new SQLiteConnection(m_DBConnection);
            SQLiteCommand command = new SQLiteCommand(connection);
            try
            {
                connection.Open();
                command.CommandText = a_Sql;
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }
            finally
            {
                command.Dispose();
                connection.Close();
            }
        }





        /// <summary>
        /// 带参数，执行脚本
        /// insert,update,delete
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="parameters">可变参数，目的是省略了手动构造数组的过程，直接指定对象，编译器会帮助我们构造数组，并将对象加入数组中，传递过来</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, params SQLiteParameter[] parameters)
        {
            using (SQLiteConnection connection = new SQLiteConnection(m_DBConnection))
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    try
                    {
                        connection.Open();
                        command.CommandText = sql;
                        if (parameters != null && parameters.Length > 0)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        return command.ExecuteNonQuery();
                    }
                    catch (Exception) { throw; }
                    finally { command.Dispose(); connection.Close(); }
                }
            }
        }


        /// <summary>
        /// 执行查询语句，并返回第一个结果。
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, params SQLiteParameter[] parameters)
        {
            using (SQLiteConnection connection = new SQLiteConnection(m_DBConnection))
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    try
                    {
                        connection.Open();
                        command.CommandText = sql;
                        if (parameters.Length != 0)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        return command.ExecuteScalar();
                    }
                    catch (Exception) { throw; }
                    finally { command.Dispose(); connection.Close(); }

                }
            }
        }



        /// <summary>
        /// 执行一个查询语句，返回一个包含查询结果的DataTable。 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string sql, params SQLiteParameter[] parameters)
        {
            using (SQLiteConnection connection = new SQLiteConnection(m_DBConnection))
            {
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    if (parameters != null && parameters.Length != 0)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable data = new DataTable();
                    try
                    {
                        adapter.Fill(data);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally { command.Dispose(); connection.Close(); }

                    return data;
                }
            }
        }


        /// <summary>
        /// 执行一个查询语句，返回一个关联的SQLiteDataReader实例。 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public SQLiteDataReader ExecuteReader(string sql, params SQLiteParameter[] parameters)
        {
            SQLiteConnection connection = new SQLiteConnection(m_DBConnection);
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            try
            {
                if (parameters != null && parameters.Length != 0)
                {
                    command.Parameters.AddRange(parameters);
                }
                connection.Open();
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception) { throw; }
            finally { command.Dispose(); connection.Close(); }
        }


        /// <summary>
        /// 带事务回滚批量执行脚本，启用事务，提升sqlite读写效率
        /// </summary>
        /// <param name="a_listSqls">SQL脚本</param>
        /// <returns></returns>
        public int ExecuteNonQuery(List<string> a_listSqls)
        {
            try
            {

                using (SQLiteConnection connection = new SQLiteConnection(m_DBConnection))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(connection))
                    {
                        //using (DbTransaction trans = connection.BeginTransaction())     //启动事务
                        {
                            try
                            {
                                connection.Open();
                                DbTransaction trans = connection.BeginTransaction();//启动事务,大幅提高读写速率，注意connection要先open

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

                                return 1;
                            }
                            catch (Exception)
                            {
                                return 0;
                            }
                            finally
                            {
                                //trans.Dispose();
                                cmd.Dispose();
                                connection.Close();
                            }


                        }
                    }
                }

            }
            catch (Exception ex) { MessageBox.Show("ExecuteNonQuery(list) error:" + ex.Message);return 0; }

        }


        /// <summary>
        /// 当columnname=iteminfo，删掉一行记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="itemInfo"></param>
        public void DeleteOneItem(string tableName, string columnName, string itemInfo)
        {
            try
            {
                string sqlstr = "delete from " + tableName + " where " + columnName + "='" + itemInfo.ToString() + "'";
                //int rt = GeneralClass.SQLiteHelper.ExecuteNonQuery(sqlstr, null);
                int rt = ExecuteNonQuery(sqlstr, null);

            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 增加一条记录
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">列名</param>
        /// <param name="itemInfo">信息</param>
        /// <returns></returns>
        public bool InsertOneItem(string tableName, string columnName, string itemInfo)
        {
            try
            {
                string sqlstr = "insert into " + tableName + "(" + columnName + ")" + "values(" + itemInfo + ")";
                //int rt = GeneralClass.SQLiteHelper.ExecuteNonQuery(sqlstr, null);
                int rt = ExecuteNonQuery(sqlstr, null);

                return true;
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 删除整个表格的数据
        /// </summary>
        /// <param name="tableName">数据表名</param>
        public bool DeleteTableData(string tableName)
        {
            try
            {
                string sqlstr = "delete from " + tableName;
                //int rt = GeneralClass.SQLiteHelper.ExecuteNonQuery(sqlstr, null);
                int rt = ExecuteNonQuery(sqlstr, null);

                return true;
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return false;
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
                string sqlstr = "UPDATE sqlite_sequence SET seq = 0 WHERE name='" + tableName + "' ";

                int rt = ExecuteNonQuery(sqlstr, null);

                return true;
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 检索table是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool IsTableEXIST(string tableName)
        {
            try
            {
                string sqlstr = "SELECT count(*) FROM sqlite_master WHERE type='table' AND name = @tableName ";

                object ob = ExecuteScalar(sqlstr, new SQLiteParameter[] { new SQLiteParameter("tableName", tableName) });
                if (Convert.ToInt16(ob) == 1)
                {
                    return true;
                }
                else { return false; }

            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return false;
            }
        }

    }
}