using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Data.SQLite;
using NPOI.SS.UserModel;
using System.Windows.Forms;
using NPOI.OpenXmlFormats.Shared;
using NPOI.OpenXmlFormats.Spreadsheet;
using System.Threading;
//using static Org.BouncyCastle.Crypto.Digests.SkeinEngine;
using static NPOI.HSSF.Util.HSSFColor;
using System.Text.RegularExpressions;
using NPOI.OpenXmlFormats.Dml;
using NPOI.XWPF.UserModel;
using NPOI.Util;
using System.ComponentModel;
using NPOI.SS.Formula.Functions;
using RebarSampling.Database;
using System.Reflection;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;

namespace RebarSampling
{

    /// <summary>
    /// DBOpt数据库操作类，用于对数据库文件的复杂操作
    /// </summary>
    public partial class DBOpt
    {

        //public IDBHelper dbHelper = new MysqlHelper();
        //public IDBHelper dbHelper = new SQLiteHelper();
        public IDBHelper dbHelper;

        public DBOpt()
        {
            //ExistRebarPicTypeList.Clear();
        }

        public DBOpt(EnumDatabaseType databaseType)
        {
            if (databaseType == EnumDatabaseType.SQLITE)
            {
                dbHelper = new SQLiteHelper();
            }
            if (databaseType == EnumDatabaseType.MYSQL)
            {
                dbHelper = new MysqlHelper();
            }
        }


        ///// <summary>
        ///// 读excel时，存储已存在的rebarType，放入list中
        ///// </summary>
        //public List<EnumAllRebarPicType> ExistRebarPicTypeList = new List<EnumAllRebarPicType>();





        //public bool CreatTable(string tableName)
        //{
        //    return dbHelper.CreateTable(tableName);
        //}
        //public bool AddColumn(string _tableName,string _col,string _datatype)
        //{
        //}

        /// <summary>
        /// 创建钢筋总表的构件表
        /// </summary>
        /// <param name="tableName"></param>
        public void CreateElementTable(string tableName)
        {
            try
            {
                dbHelper.CreateTable(tableName);
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PROJECT_NAME], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.MAIN_ASSEMBLY_NAME], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.CHILD_ASSEMBLY_NAME], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.ELEMENT_NAME], dbHelper.GetDataType(DbType.String));
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 创建料单管理表
        /// </summary>
        /// <param name="tableName"></param>
        public void CreateBillManageTable(string tableName)
        {
            try
            {
                dbHelper.CreateTable(tableName);
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TABLE_NAME], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.MAIN_ASSEMBLY_NAME], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.CHILD_ASSEMBLY_NAME], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.ELEMENT_NAME], dbHelper.GetDataType(DbType.String));
            }
            catch (Exception ex) { throw ex; }

        }
        /// <summary>
        /// 创建梁板线的构件批的表，列名依次为：生产批号、料仓号、仓位号、仓位设置、临时编号、项目名称、主构件名称、子构件名称
        /// </summary>
        /// <param name="tableName"></param>
        public void CreateElementBatch_LB_Table(string tableName)
        {
            try
            {
                dbHelper.CreateTable(tableName);
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.WORK_LB__ELEMENTBATCH_SERI], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.WAREHOUSE_NO], dbHelper.GetDataType(DbType.Int32));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.WARE_NO], dbHelper.GetDataType(DbType.Int32));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.WAREHOUSE_SET], dbHelper.GetDataType(DbType.Int32));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TEMP_ELEMENT_SERI], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TAOSI_SET], dbHelper.GetDataType(DbType.String));

                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PROJECT_NAME], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.MAIN_ASSEMBLY_NAME], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.CHILD_ASSEMBLY_NAME], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.ELEMENT_NAME], dbHelper.GetDataType(DbType.String));
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 创建墙柱线批量锯切的加工批的表，列名依次为：生产批号(按直径划分)，临时编号、直径、长度、数量、边角信息、是否弯曲、是否套丝，套丝机设置
        /// </summary>
        /// <param name="tableName"></param>
        public void CreateElementBatch_QZ_Table(string tableName)
        {
            try
            {
                dbHelper.CreateTable(tableName);
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.WORK_QZ_PICUT_SERI], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TEMP_NO], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.DIAMETER], dbHelper.GetDataType(DbType.Int32));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.LENGTH], dbHelper.GetDataType(DbType.Int32));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.NUM], dbHelper.GetDataType(DbType.Int32));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.CORNER_MESSAGE], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFBEND], dbHelper.GetDataType(DbType.Boolean));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFTAO], dbHelper.GetDataType(DbType.Boolean));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TAOSI_SET], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PIC_NO], dbHelper.GetDataType(DbType.String));


                //dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PROJECT_NAME], dbHelper.GetDataType(DbType.String));
                //dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.MAIN_ASSEMBLY_NAME], dbHelper.GetDataType(DbType.String));
                //dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.ELEMENT_NAME], dbHelper.GetDataType(DbType.String));
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 创建批量锯切的加工批的表，列名依次为：直径、长度、数量、边角信息、简图编号，项目名称，构件名称
        /// </summary>
        /// <param name="tableName"></param>
        public void CreateElementBatch_PiCut_Table(string tableName)
        {
            try
            {
                dbHelper.CreateTable(tableName);
                //dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.WORK_QZ_PICUT_SERI], dbHelper.GetDataType(DbType.String));
                //dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TEMP_NO], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.LEVEL], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.DIAMETER], dbHelper.GetDataType(DbType.Int32));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.LENGTH], dbHelper.GetDataType(DbType.Int32));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.NUM], dbHelper.GetDataType(DbType.Int32));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.CORNER_MESSAGE], dbHelper.GetDataType(DbType.String));
                //dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFBEND], dbHelper.GetDataType(DbType.Boolean));
                //dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFTAO], dbHelper.GetDataType(DbType.Boolean));
                //dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TAOSI_SET], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PIC_NO], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PROJECT_NAME], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.MAIN_ASSEMBLY_NAME], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.ELEMENT_NAME], dbHelper.GetDataType(DbType.String));



                //dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PROJECT_NAME], dbHelper.GetDataType(DbType.String));
                //dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.MAIN_ASSEMBLY_NAME], dbHelper.GetDataType(DbType.String));
                //dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.ELEMENT_NAME], dbHelper.GetDataType(DbType.String));
            }
            catch (Exception ex) { throw ex; }
        }

        public void CreatePrintBuff_LB_Table(string tableName)
        {
            try
            {
                dbHelper.CreateTable(tableName);
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.WORK_LB__ELEMENTBATCH_SERI], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.WAREHOUSE_NO], dbHelper.GetDataType(DbType.Int32));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.WARE_NO], dbHelper.GetDataType(DbType.Int32));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.WAREHOUSE_SET], dbHelper.GetDataType(DbType.Int32));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TEMP_ELEMENT_SERI], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TAOSI_SET], dbHelper.GetDataType(DbType.String));

                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PROJECT_NAME], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.MAIN_ASSEMBLY_NAME], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.ELEMENT_NAME], dbHelper.GetDataType(DbType.String));
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 创建钢筋总表的筛选表
        /// </summary>
        /// <param name="tableName"></param>
        public void CreatePickTable(string tableName)
        {
            try
            {
                dbHelper.CreateTable(tableName);
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PROJECT_NAME], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.MAIN_ASSEMBLY_NAME], dbHelper.GetDataType(DbType.String));
                //dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.CHILD_ASSEMBLY_NAME], dbHelper.GetDataType(DbType.String));

                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFPICK], dbHelper.GetDataType(DbType.Boolean));
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 创建钢筋总表，添加列
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="_data"></param>
        public void CreateRebarTable(string tableName)
        {
            try
            {
                dbHelper.CreateTable(tableName);
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PROJECT_NAME], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.MAIN_ASSEMBLY_NAME], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.CHILD_ASSEMBLY_NAME], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.ELEMENT_NAME], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TABLE_NO], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TABLE_NAME], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TABLE_SHEET_NAME], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PIC_NO], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.LEVEL], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.DIAMETER], dbHelper.GetDataType(DbType.Int32));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.REBAR_PIC], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PIC_MESSAGE], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.CORNER_MESSAGE], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.CORNER_MESSAGE_BK], dbHelper.GetDataType(DbType.String));

                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.LENGTH], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.ISMULTI], dbHelper.GetDataType(DbType.Boolean));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PIECE_NUM_UNIT_NUM], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TOTAL_PIECE_NUM], dbHelper.GetDataType(DbType.Int32));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TOTAL_WEIGHT], dbHelper.GetDataType(DbType.Double));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.DESCRIPTION], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.SERIALNUM], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.ISORIGINAL], dbHelper.GetDataType(DbType.Boolean));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFTAO], dbHelper.GetDataType(DbType.Boolean));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFBEND], dbHelper.GetDataType(DbType.Boolean));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFCUT], dbHelper.GetDataType(DbType.Boolean));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFBENDTWICE], dbHelper.GetDataType(DbType.Boolean));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.BAR_TYPE], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.FABRICATION_TYPE], dbHelper.GetDataType(DbType.String));

                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFMAKE_IN_LINE], dbHelper.GetDataType(DbType.Boolean));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFPICK_IN_BILL], dbHelper.GetDataType(DbType.Boolean));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFSEND_TO_PCS], dbHelper.GetDataType(DbType.Boolean));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFMAKE_DONE], dbHelper.GetDataType(DbType.Boolean));


                //        dbHelper.AddColumn(tableName, GeneralClass.RebarColumnName[i], rebarData.ElementName.GetType().Name);
            }
            catch (Exception ex) { throw ex; }

        }
        public string InsertRowElementData(string tableName, string projectName, string assemblyName, string childAssemblyName, string elementName)
        {
            try
            {
                //示例：string sqlstr = "insert into " + tableName + "(" + columnName + ")" + "values(" + itemInfo + ")";


                string sqlstr = "insert into " + tableName + "(";

                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PROJECT_NAME] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.MAIN_ASSEMBLY_NAME] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.CHILD_ASSEMBLY_NAME] + ",";

                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.ELEMENT_NAME] + ",";

                sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);//去掉尾部的逗号
                sqlstr += ")" + "values(";

                sqlstr += "'" + projectName + "'" + ",";
                sqlstr += "'" + assemblyName + "'" + ",";
                sqlstr += "'" + childAssemblyName + "'" + ",";

                sqlstr += "'" + elementName + "'";

                //sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);//去掉尾部的逗号
                sqlstr += ")";

                return sqlstr;
            }
            catch (Exception e) { throw e; }
        }
        /// <summary>
        /// 添加一行到梁板线的printbuff表，用于bartender打印
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="_workseri"></param>
        /// <param name="_ware">依次为料仓号、仓位号、料仓设置</param>
        /// <param name="tempNo"></param>
        /// <param name="_element">依次为项目名称、主构件名称、子构件名称</param>
        /// <param name="_taosiSet"></param>
        /// <returns></returns>
        public string InsertRowPrintBuff_LB(string tableName, string _workseri, Tuple<int, int, int> _ware, string tempNo, Tuple<string, string, string> _element, string _taosiSet)
        {
            try
            {
                //示例：string sqlstr = "insert into " + tableName + "(" + columnName + ")" + "values(" + itemInfo + ")";

                string sqlstr = "insert into " + tableName + "(";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.WORK_LB__ELEMENTBATCH_SERI] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.WAREHOUSE_NO] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.WARE_NO] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.WAREHOUSE_SET] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TEMP_ELEMENT_SERI] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TAOSI_SET] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PROJECT_NAME] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.MAIN_ASSEMBLY_NAME] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.ELEMENT_NAME] + ",";

                sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);//去掉尾部的逗号
                sqlstr += ")" + "values(";

                sqlstr += "'" + _workseri + "'" + ",";
                sqlstr += "'" + _ware.Item1 + "'" + ",";
                sqlstr += "'" + _ware.Item2 + "'" + ",";
                sqlstr += "'" + _ware.Item3 + "'" + ",";
                sqlstr += "'" + tempNo + "'" + ",";
                sqlstr += "'" + _taosiSet + "'" + ",";
                sqlstr += "'" + _element.Item1 + "'" + ",";
                sqlstr += "'" + _element.Item2 + "'" + ",";
                sqlstr += "'" + _element.Item3 + "'";

                //sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);//去掉尾部的逗号
                sqlstr += ")";

                return sqlstr;
            }
            catch (Exception e) { throw e; }
        }
        public string InsertRowPiCut(string tableName, string _level, int _diameter, int _length, int _num, string _cornerMsg, string _pictypeNum, string _projectName, string _assemblyName, string _elementName)
        {
            try
            {
                //示例：string sqlstr = "insert into " + tableName + "(" + columnName + ")" + "values(" + itemInfo + ")";

                string sqlstr = "insert into " + tableName + "(";
                //sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.WORK_QZ_PICUT_SERI] + ",";
                //sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TEMP_NO] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.LEVEL] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.DIAMETER] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.LENGTH] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.NUM] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.CORNER_MESSAGE] + ",";
                //sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFBEND] + ",";
                //sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFTAO] + ",";
                //sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TAOSI_SET] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PIC_NO] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PROJECT_NAME] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.MAIN_ASSEMBLY_NAME] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.ELEMENT_NAME] + ",";


                sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);//去掉尾部的逗号
                sqlstr += ")" + "values(";

                //sqlstr += "'" + _workseri + "'" + ",";
                //sqlstr += "'" + _tempno + "'" + ",";
                sqlstr += "'" + _level + "'" + ",";
                sqlstr += "'" + _diameter + "'" + ",";
                sqlstr += "'" + _length + "'" + ",";
                sqlstr += "'" + _num + "'" + ",";
                sqlstr += "'" + _cornerMsg + "'" + ",";
                //sqlstr += _ifbend.ToString() + ",";
                //sqlstr += _iftao.ToString() + ",";
                //sqlstr += "'" + _taosiSet + "'" + ",";
                sqlstr += "'" + _pictypeNum + "'" + ",";
                sqlstr += "'" + _projectName + "'" + ",";
                sqlstr += "'" + _assemblyName + "'" + ",";
                sqlstr += "'" + _elementName + "'" + ",";

                sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);//去掉尾部的逗号
                sqlstr += ")";

                return sqlstr;
            }
            catch (Exception e) { throw e; }
        }
        public string InsertRowPiCut_QZ(string tableName, string _workseri, string _tempno, int _diameter, int _length, int _num, string _cornerMsg, bool _ifbend, bool _iftao, string _taosiSet, string _pictypeNum)
        {
            try
            {
                //示例：string sqlstr = "insert into " + tableName + "(" + columnName + ")" + "values(" + itemInfo + ")";

                string sqlstr = "insert into " + tableName + "(";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.WORK_QZ_PICUT_SERI] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TEMP_NO] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.DIAMETER] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.LENGTH] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.NUM] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.CORNER_MESSAGE] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFBEND] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFTAO] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TAOSI_SET] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PIC_NO] + ",";


                sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);//去掉尾部的逗号
                sqlstr += ")" + "values(";

                sqlstr += "'" + _workseri + "'" + ",";
                sqlstr += "'" + _tempno + "'" + ",";
                sqlstr += "'" + _diameter + "'" + ",";
                sqlstr += "'" + _length + "'" + ",";
                sqlstr += "'" + _num + "'" + ",";
                sqlstr += "'" + _cornerMsg + "'" + ",";
                sqlstr += _ifbend.ToString() + ",";
                sqlstr += _iftao.ToString() + ",";
                sqlstr += "'" + _taosiSet + "'" + ",";
                sqlstr += "'" + _pictypeNum + "'";

                //sqlstr += "'" + _element.projectName + "'" + ",";
                //sqlstr += "'" + _element.assemblyName + "'" + ",";
                //sqlstr += "'" + _element.elementName + "'";

                //sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);//去掉尾部的逗号
                sqlstr += ")";

                return sqlstr;
            }
            catch (Exception e) { throw e; }
        }
        /// <summary>
        /// 在梁板线的构件批的表插入一行，列名依次为：生产批号、料仓号、仓位号、仓位设置、临时编号、项目名称、主构件名称、子构件名称
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="_workseri"></param>
        /// <param name="_warehouseNo"></param>
        /// <param name="_wareno"></param>
        /// <param name="_wareset"></param>
        /// <param name="tempNo"></param>
        /// <param name="_element"></param>
        /// <param name="_taosiSet"></param>
        /// <returns></returns>
        public string InsertRowElementBatch_LB(string tableName, string _workseri, int _warehouseNo, int _wareno, int _wareset, string tempNo, ElementRebarFB _element, string _taosiSet)
        {
            try
            {
                //示例：string sqlstr = "insert into " + tableName + "(" + columnName + ")" + "values(" + itemInfo + ")";

                string sqlstr = "insert into " + tableName + "(";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.WORK_LB__ELEMENTBATCH_SERI] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.WAREHOUSE_NO] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.WARE_NO] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.WAREHOUSE_SET] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TEMP_ELEMENT_SERI] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TAOSI_SET] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PROJECT_NAME] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.MAIN_ASSEMBLY_NAME] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.CHILD_ASSEMBLY_NAME] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.ELEMENT_NAME] + ",";

                sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);//去掉尾部的逗号
                sqlstr += ")" + "values(";

                sqlstr += "'" + _workseri + "'" + ",";
                sqlstr += "'" + _warehouseNo + "'" + ",";
                sqlstr += "'" + _wareno + "'" + ",";
                sqlstr += "'" + _wareset + "'" + ",";
                sqlstr += "'" + tempNo + "'" + ",";
                sqlstr += "'" + _taosiSet + "'" + ",";
                sqlstr += "'" + _element.projectName + "'" + ",";
                sqlstr += "'" + _element.assemblyName + "'" + ",";
                sqlstr += "'" + _element.childAssemblyName + "'" + ",";
                sqlstr += "'" + _element.elementName + "'";

                //sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);//去掉尾部的逗号
                sqlstr += ")";

                return sqlstr;
            }
            catch (Exception e) { throw e; }
        }
        public string InsertRowPickData(string tableName, string projectName, string assemblyName,/*string childAssemblyName,*/ bool ifpick)
        {
            try
            {
                //示例：string sqlstr = "insert into " + tableName + "(" + columnName + ")" + "values(" + itemInfo + ")";

                string sqlstr = "insert into " + tableName + "(";

                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PROJECT_NAME] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.MAIN_ASSEMBLY_NAME] + ",";
                //sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.CHILD_ASSEMBLY_NAME] + ",";

                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFPICK] + ",";

                sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);//去掉尾部的逗号
                sqlstr += ")" + "values(";

                sqlstr += "'" + projectName + "'" + ",";
                sqlstr += "'" + assemblyName + "'" + ",";
                //sqlstr += "'" + childAssemblyName + "'" + ",";

                sqlstr += ifpick.ToString();

                //sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);//去掉尾部的逗号
                sqlstr += ")";

                return sqlstr;
            }
            catch (Exception e) { throw e; }
        }

        /// <summary>
        /// 插入一行rebardata数据，注意需要整理rowdata的list
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="rowdata"></param>
        public string InsertRowRebarData(string tableName, RebarData rowdata)
        {
            //lock (m_DBConnection)//线程安全，互锁一下
            {

                //using (SQLiteConnection connection = new SQLiteConnection(m_DBConnection))
                {
                    //using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        try
                        {

                            //string sqlstr = "insert into " + tableName + "(" + columnName + ")" + "values(" + itemInfo + ")";

                            string sqlstr = "insert into " + tableName + "(";

                            for (int i = 0; i <= (int)EnumAllRebarTableColName.FABRICATION_TYPE; i++)
                            {
                                sqlstr += GeneralClass.sRebarColumnName[i] + ",";
                            }
                            sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFMAKE_IN_LINE] + ",";
                            sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFPICK_IN_BILL] + ",";
                            sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFSEND_TO_PCS] + ",";
                            sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFMAKE_DONE] + ",";

                            sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);//去掉尾部的逗号
                            sqlstr += ")" + "values(";

                            //for (int j = 0; j < (int)EnumAllRebarTableColName.maxRebarColNum; j++)
                            //{
                            //    sqlstr += "@" + GeneralClass.sRebarColumnName[j] + ",";
                            //}
                            sqlstr += "'" + rowdata.ProjectName + "'" + ",";
                            sqlstr += "'" + rowdata.MainAssemblyName + "'" + ",";
                            sqlstr += "'" + rowdata.ChildAssemblyName + "'" + ",";
                            sqlstr += "'" + rowdata.ElementName + "'" + ",";
                            sqlstr += "'" + rowdata.TableNo + "'" + ",";
                            sqlstr += "'" + rowdata.TableName + "'" + ",";
                            sqlstr += "'" + rowdata.TableSheetName + "'" + ",";
                            sqlstr += "'" + rowdata.PicTypeNum + "'" + ",";
                            sqlstr += "'" + rowdata.Level + "'" + ",";
                            sqlstr += rowdata.Diameter.ToString() + ",";
                            sqlstr += "'" + rowdata.RebarPic + "'" + ",";
                            sqlstr += "'" + rowdata.PicMessage + "'" + ",";
                            sqlstr += "'" + rowdata.CornerMessage + "'" + ",";
                            sqlstr += "'" + rowdata.CornerMessageBK + "'" + ",";
                            sqlstr += "'" + rowdata.Length + "'" + ",";
                            sqlstr += rowdata.IsMulti.ToString() + ",";
                            sqlstr += "'" + rowdata.PieceNumUnitNum + "'" + ",";
                            sqlstr += rowdata.TotalPieceNum.ToString() + ",";
                            sqlstr += rowdata.TotalWeight.ToString() + ",";
                            sqlstr += "'" + rowdata.Description + "'" + ",";
                            //sqlstr += rowdata.SerialNum.ToString() + ",";
                            sqlstr += "'" + rowdata.SerialNum + "'" + ",";
                            sqlstr += rowdata.IsOriginal.ToString() + ",";
                            sqlstr += rowdata.IfTao.ToString() + ",";
                            sqlstr += rowdata.IfBend.ToString() + ",";
                            sqlstr += rowdata.IfCut.ToString() + ",";
                            sqlstr += rowdata.IfBendTwice.ToString() + ",";
                            sqlstr += "'" + rowdata.BarType + "'" + ",";
                            sqlstr += "'" + rowdata.FabricationType + "'" + ",";
                            sqlstr += rowdata.IfMakeInLine.ToString() + ",";
                            sqlstr += rowdata.IfPickInBill.ToString() + ",";
                            sqlstr += rowdata.IfSendToPCS.ToString() + ",";
                            sqlstr += rowdata.IfMakeDone.ToString();




                            //sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);//去掉尾部的逗号
                            sqlstr += ")";

                            return sqlstr;

                        }
                        catch (Exception ) { throw ; }
                        finally { /*command.Dispose(); connection.Close();*/ }
                    }
                }
            }
        }

        /// <summary>
        /// 批量锯切存入数据库，_ifInit为是否初始化数据库
        /// </summary>
        /// <param name="_ifInit"></param>
        /// <param name="_list"></param>
        public static void SaveAllPiCutToDB(bool _ifInit, List<List<Rebar>> _list)
        {
            if (_ifInit)
            {
                GeneralClass.DBOpt.InitPiCutDB(GeneralClass.TableName_PiCutBatch);
            }
            List<string> sqls = new List<string>();

            foreach (var item in _list)
            {
                string _level = item[0].Level;
                int _diameter = item[0].Diameter;
                int _length = item[0].length;
                int _num = item.Count;
                string _cornermsg = item[0].CornerMessage;
                string _picTypeNum = item[0].PicTypeNum;
                string _projectName = item[0].ProjectName;
                string _assemblyName = item[0].MainAssemblyName;
                string _elementName = item[0].ElementName;
                sqls.Add(GeneralClass.DBOpt.InsertRowPiCut(GeneralClass.TableName_PiCutBatch, _level, _diameter, _length, _num, _cornermsg, _picTypeNum,
                    _projectName, _assemblyName, _elementName));
            }

            GeneralClass.DBOpt.dbHelper.ExecuteSqlsTran(sqls);//批量存入

        }
        public static void SaveAllPiCutBatchToDB(RebarOriPiAllDiameter _batchlist)
        {
            //创建墙柱线的批量锯切的表
            GeneralClass.DBOpt.InitPiCutDB_QZ(GeneralClass.TableName_PiCutBatch_QZ);
            List<string> sqls = new List<string>();

            int _tempNo = 0;
            //添加sqls，以每个零件包为单位进行添加
            foreach (RebarOriPiWithDiameter _batch in _batchlist._list)
            {
                int _diameter = _batch._diameter;
                foreach (RebarOriPi _oripi in _batch._rebarOriPiList)
                {
                    int _num = _oripi.num;//数量
                    foreach (Rebar _rebar in _oripi._list[0]._list)//rebarlist
                    {
                        _tempNo++;
                        int _index = _oripi._list[0]._list.IndexOf(_rebar);
                        string _batchseri = "B_" + DateTime.Now.ToString("yyyyMMdd") + "_"
                                + "1" + "_"
                                + "001" + "/"
                                + "001" + "_"
                                + _batch.totalBatchNo.ToString().PadLeft(3, '0') + "/"
                                + (_batch.curBatchNo + 1).ToString().PadLeft(3, '0') + "_"
                                + _oripi.totalBatchNo.ToString().PadLeft(3, '0') + "/"
                                + (_oripi.curBatchNo + 1).ToString().PadLeft(3, '0') + "_"
                                + _oripi._list[0]._list.Count.ToString().PadLeft(3, '0') + "/"
                                + (_index + 1).ToString().PadLeft(3, '0');

                        sqls.Add(GeneralClass.DBOpt.InsertRowPiCut_QZ(GeneralClass.TableName_PiCutBatch_QZ,
                            _batchseri,
                            _tempNo.ToString(),
                            _diameter,
                            _rebar.length,
                            _num,
                            _rebar.CornerMessage,
                            _rebar.IfBend,
                            _rebar.IfTao,
                            "",
                            _rebar.PicTypeNum));

                        //GeneralClass.EnumWareSetToInt(_piori.wareNumSet),
                        // "A" + _piori.elementIndex,
                        // _piori,
                        // _batch.TaosiSetting));
                    }

                }
            }

            GeneralClass.DBOpt.dbHelper.ExecuteSqlsTran(sqls);//批量存入
        }
        /// <summary>
        /// 保存所有rebardata存入数据库
        /// </summary>
        /// <param name="_rebarlist"></param>
        public static void SaveAllRebarToDB(List<RebarData> _rebarlist)
        {
            GeneralClass.DBOpt.InitRebarDB(GeneralClass.TableName_AllRebar);
            List<string> sqls = new List<string>();

            foreach (var item in _rebarlist)
            {
                sqls.Add(GeneralClass.DBOpt.InsertRowRebarData(GeneralClass.TableName_AllRebar, item));
            }
            GeneralClass.DBOpt.dbHelper.ExecuteSqlsTran(sqls);//批量存入
        }
        /// <summary>
        /// 将所有的构件生产批存入数据库
        /// </summary>
        public static void SaveAllElementBatchToDB(List<ElementBatch> _batchlist)
        {
            //创建梁板线的构件批的表
            GeneralClass.DBOpt.InitElementBatchDB_LB(GeneralClass.TableName_ElementBatch_LB);
            List<string> sqls = new List<string>();

            //添加sqls，以每个构件为单位进行添加
            foreach (ElementBatch _batch in _batchlist)
            {
                foreach (ElementRebarFB item in _batch.elementData)
                {
                    sqls.Add(GeneralClass.DBOpt.InsertRowElementBatch_LB(GeneralClass.TableName_ElementBatch_LB,
                        _batch.BatchSeri,
                        item.warehouseNo,
                        item.wareNo,
                       GeneralClass.EnumWareSetToInt(item.wareNumSet),
                        "A" + item.elementIndex,
                        item,
                        _batch.TaosiSetting));
                }
            }

            GeneralClass.DBOpt.dbHelper.ExecuteSqlsTran(sqls);//批量存入
        }
        /// <summary>
        /// 图形汇总中，根据钢筋不同钢筋图形进行查询,罗列出colNameList这几项的内容
        /// /// SELECT column1,columnN FROM table_name WHERE[condition1] AND[condition2]...AND[conditionN];
        ///         =	    等于
        ///         <>	    不等于
        ///         >	    大于
        ///         <       小于
        ///         >=	    大于等于
        ///         <=	    小于等于
        ///         between 在某个范围内
        ///         like    搜索某种模式
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="colNameList"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public DataTable FindAllPic(string tablename, List<string> colNameList, EnumRebarSizeType _rebarType, EnumRebarPicType _rebarPicType)
        {
            try
            {
                if (colNameList.Count == 0) { return null; }

                string sqlstr = "select ";

                foreach (string col in colNameList)
                {
                    sqlstr += col + ",";
                }
                sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);//去掉尾部逗号

                sqlstr += " from " + tablename + " where ";

                if (_rebarType == EnumRebarSizeType.BANG)//棒材
                {
                    sqlstr += "(" + GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.DIAMETER] + " between 14 AND 40 )";  //棒材钢筋
                }
                else
                {
                    sqlstr += "(" + GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.DIAMETER] + " between 6 AND 12 )";   //线材钢筋
                }
                sqlstr += "AND";

                string tt = _rebarPicType.ToString();
                sqlstr += "(" + GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PIC_NO] + " = '" + tt.Substring(2, tt.Length - 2) + "')";


                DataTable dt = dbHelper.ExecuteQuery(sqlstr);

                return dt;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("FindItemValue出错：" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 初始化DB文件，实现功能：
        /// 1、如果不存在则创建
        /// 2、清空表单，准备存入新的数据
        /// </summary>
        /// <param name="tableName"></param>
        public void InitRebarDB(string tablename)
        {
            try
            {
                if (!dbHelper.IsTableEXIST(tablename))    //如果table不存在，则创建新表
                {
                    CreateRebarTable(tablename);
                }
                dbHelper.DeleteTable(tablename);         //清空表
                dbHelper.DeleteTableSequence(tablename); //清空表的自增列序号
                //GeneralClass.ExistRebarPicTypeList.Clear();//清空已存在的钢筋图形list
            }
            catch (Exception ex) { MessageBox.Show("InitRebarDB error:" + ex.Message); }
        }
        /// <summary>
        /// 初始化DB文件，实现功能：
        /// 1、如果不存在则创建
        /// 2、清空表单，准备存入新的数据
        /// </summary>
        /// <param name="tableName"></param>
        public void InitPickDB(string tablename)
        {
            try
            {

                if (!dbHelper.IsTableEXIST(tablename))    //如果table不存在，则创建新表
                {
                    CreatePickTable(tablename);
                }
                dbHelper.DeleteTable(tablename);         //清空表
                dbHelper.DeleteTableSequence(tablename); //清空表的自增列序号
                //GeneralClass.ExistRebarPicTypeList.Clear();//清空已存在的钢筋图形list
            }
            catch (Exception ex) { MessageBox.Show("InitPickDB error:" + ex.Message); }
        }
        /// <summary>
        /// 初始化DB文件，实现功能：
        /// 1、如果不存在则创建
        /// 2、清空表单，准备存入新的数据
        /// </summary>
        /// <param name="tableName"></param>
        public void InitElementDB(string tablename)
        {
            try
            {
                if (!dbHelper.IsTableEXIST(tablename))    //如果table不存在，则创建新表
                {
                    CreateElementTable(tablename);
                }
                dbHelper.DeleteTable(tablename);         //清空表
                dbHelper.DeleteTableSequence(tablename); //清空表的自增列序号
                //GeneralClass.ExistRebarPicTypeList.Clear();//清空已存在的钢筋图形list
            }
            catch (Exception ex) { MessageBox.Show("InitElementDB error:" + ex.Message); }
        }
        /// <summary>
        /// 初始化料单管理表，如果不存在该表，则创建，如果有，不需要操作清空
        /// </summary>
        /// <param name="tablename"></param>
        public void InitBillManageDB(string tablename)
        {
            try
            {
                if (!dbHelper.IsTableEXIST(tablename))    //如果table不存在，则创建新表
                {
                    CreateBillManageTable(tablename);
                }
                //dbHelper.DeleteTable(tablename);         //清空表
                //dbHelper.DeleteTableSequence(tablename); //清空表的自增列序号
            }
            catch (Exception ex) { MessageBox.Show("InitElementDB error:" + ex.Message); }

        }
        public void InitPiCutDB_QZ(string tablename)
        {
            try
            {
                if (!dbHelper.IsTableEXIST(tablename))    //如果table不存在，则创建新表
                {
                    CreateElementBatch_QZ_Table(tablename);
                }
                dbHelper.DeleteTable(tablename);         //清空表
                dbHelper.DeleteTableSequence(tablename); //清空表的自增列序号
            }
            catch (Exception ex) { MessageBox.Show("InitPiCutDB_QZ error:" + ex.Message); }
        }

        public void InitPiCutDB(string tablename)
        {
            try
            {
                if (!dbHelper.IsTableEXIST(tablename))    //如果table不存在，则创建新表
                {
                    CreateElementBatch_PiCut_Table(tablename);
                }
                dbHelper.DeleteTable(tablename);         //清空表
                dbHelper.DeleteTableSequence(tablename); //清空表的自增列序号
            }
            catch (Exception ex) { MessageBox.Show("InitPiCutDB error:" + ex.Message); }

        }

        /// <summary>
        /// 创建梁板线的构件生产批的表
        /// </summary>
        /// <param name="tablename"></param>
        public void InitElementBatchDB_LB(string tablename)
        {
            try
            {
                if (!dbHelper.IsTableEXIST(tablename))    //如果table不存在，则创建新表
                {
                    CreateElementBatch_LB_Table(tablename);
                }
                dbHelper.DeleteTable(tablename);         //清空表
                dbHelper.DeleteTableSequence(tablename); //清空表的自增列序号
            }
            catch (Exception ex) { MessageBox.Show("InitElementBatchDB_LB error:" + ex.Message); }
        }

        public void InitPrintBuff_LB(string tablename)
        {
            try
            {
                if (!dbHelper.IsTableEXIST(tablename))    //如果table不存在，则创建新表
                {
                    CreatePrintBuff_LB_Table(tablename);
                }
                dbHelper.DeleteTable(tablename);         //清空表
                dbHelper.DeleteTableSequence(tablename); //清空表的自增列序号
            }
            catch (Exception ex) { MessageBox.Show("InitPrintBuff_LB error:" + ex.Message); }
        }
        /// <summary>
        /// excel文件存入数据库
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="DBtableName"></param>
        public void ExcelToDB(string filename, string DBtableName)
        {
            string sheetname = null;
            string tablename = DBtableName;

            List<string> sqls = new List<string>();

            double tt_d;
            int tt_i;

            RebarData rebarData;

            List<Tuple<string, string, DataTable>> _dtlist = GeneralClass.ExcelReadOpt?.GetAllSheet(filename);

            for (int i = 0; i < _dtlist.Count; i++)
            {
                DataTable dt = _dtlist[i].Item3;
                string projectName = _dtlist[i].Item1;//项目名称
                string assemblyName = _dtlist[i].Item2;//构件部位
                sheetname = dt.TableName;

                //检查是否有“序号”这一列
                bool _haveSeri = (dt.Columns[1].ColumnName == "序号") ? true : false;

                string childAssembly = "";//20250624 某些料单有子构件部位的标识，

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    DataRow dr = dt.Rows[j];
                    if (dr == null) continue;

                    int startindex = _haveSeri ? 2 : 1;
                    bool _haveTongji = dt.Columns[startindex + 10].ColumnName == "统计\n说明" ? true : false;


                    if (dr["构件名称"].ToString() != "" && dr["边角结构"].ToString() == "")//20250624如果边角结构是空的，但构件名称不为空，说明此行为子构件部位说明行，后续构件均使用此子构件部位名称
                    {
                        childAssembly = dr["构件名称"].ToString();
                        continue;
                    }
                    //存入数据库
                    if (dr["编号"].ToString() == "" || dr["边角结构"].ToString() == "")//以是否有钢筋类型编号作为此行是否为有效数据的判定条件
                    {
                        continue;
                    }

                    rebarData = new RebarData();//构建新的钢筋对象

                    rebarData.TableName = System.IO.Path.GetFileNameWithoutExtension(filename); //获取excel文件名作为根节点名称,不带后缀名;//将filename赋予钢筋的料表名称
                    rebarData.TableSheetName = sheetname;//将sheetname赋予钢筋的料表sheet名称
                    rebarData.ProjectName = projectName; //项目名称
                    rebarData.MainAssemblyName = assemblyName;//主构件部位名称
                    rebarData.ChildAssemblyName = childAssembly.Replace("%", "");//子构件部位名称，去掉某些特殊字符，例如%

                    rebarData.ElementName = dr[0].ToString();
                    rebarData.PicTypeNum = dr[startindex].ToString();

                    string level_diameter = dr[startindex + 1].ToString();
                    if (level_diameter != "" && level_diameter.Length >= 2)//将级别直径（如C14）拆分开
                    {
                        rebarData.Level = level_diameter.Substring(0, 1);                        //级别
                        rebarData.Diameter = Convert.ToInt32(level_diameter.Substring(1, level_diameter.Length - 1));    //直径
                    }
                    else
                    {
                        rebarData.Level = "";
                        rebarData.Diameter = 0;
                    }


                    rebarData.RebarPic = dr[startindex + 2].ToString();//钢筋简图,暂为空

                    rebarData.PicMessage = dr[startindex + 3].ToString();//图形信息
                    rebarData.CornerMessage = dr[startindex + 4].ToString();//边角结构信息
                    rebarData.CornerMessageBK = dr[startindex + 4].ToString();//边角结构信息备份

                    string _length = dr[startindex + 5].ToString();//下料长度
                    if (_length != "")
                    {
                        //缩尺符号~前面如果是'\n',则需要先去掉'\n'
                        if (_length.IndexOf('~') > -1 && _length[_length.IndexOf('~') - 1] == '\n')
                        {
                            _length = _length.Remove(_length.IndexOf('~') - 1, 1);//去掉~前面的那个'\n'                    
                        }
                        string[] sss = _length.Split('\n');  //下料长度可能会出现多段的情况，此时需按照'\n'进行拆分字符串
                        rebarData.Length = _length;
                        //rebarData.IsMulti = (sss.Length > 1) ? true : false;
                    }

                    rebarData.PieceNumUnitNum = dr[startindex + 6].ToString();//根数*件数

                    int.TryParse(dr[startindex + 7].ToString(), out tt_i);//总根数
                    rebarData.TotalPieceNum = tt_i;

                    double.TryParse(dr[startindex + 8].ToString(), out tt_d);//总重量，kg
                    rebarData.TotalWeight = tt_d;

                    rebarData.Description = dr[startindex + 9].ToString();//备注说明

                    //int.TryParse(dr[startindex + 10].ToString(), out tt_i);
                    //rebarData.SerialNum = tt_i;     //料单中钢筋流水号
                    rebarData.SerialNum = _haveTongji ? dr[startindex + 11].ToString() : dr[startindex + 10].ToString();     //料单中钢筋流水号


                    //ModifyRebarData(ref rebarData);

                    //InsertRowData(tablename, rebarData);
                    sqls.Add(InsertRowRebarData(tablename, rebarData));
                }

            }

            dbHelper.ExecuteSqlsTran(sqls);//批量存入


        }



        /// <summary>
        /// 判断字符串中是否含有中文
        /// </summary>
        /// <param name="_cornerMessage"></param>
        /// <returns></returns>
        private bool RegexHaveChinese(string _cornerMessage)
        {
            return Regex.IsMatch(_cornerMessage, @"[\u4e00-\u9fa5]");//是否含有中文的UNICODE码范围
        }

        public List<KeyValuePair<EnumRebarPicType, GeneralDetailData>> Detail_other(List<RebarData> _rebarDataList, bool _iffilter, bool _ifcut, bool _ifbend, bool _iftao)
        {
            try
            {
                if (_rebarDataList == null || _rebarDataList.Count == 0) { return null; }

                List<KeyValuePair<EnumRebarPicType, GeneralDetailData>> _data = new List<KeyValuePair<EnumRebarPicType, GeneralDetailData>>();
                //List<GeneralDetailData> _data = new List<GeneralDetailData>();
                KeyValuePair<EnumRebarPicType, GeneralDetailData> _pair = new KeyValuePair<EnumRebarPicType, GeneralDetailData>();

                GeneralDetailData _detaildata = new GeneralDetailData();

                //List<RebarData> _newdatalist = new List<RebarData>();
                ////先把rebardata列表里面筛出原材和多段的
                //foreach (RebarData _dd in _rebarDataList)
                //{
                //    if (!_dd.IsMulti && !_dd.IsOriginal)
                //    {
                //        _newdatalist.Add(_dd);
                //    }
                //}

                List<EnumRebarPicType> _typelist = GetExistedRebarTypeList(_rebarDataList/*_newdatalist*/);//得到列表中包含的钢筋图形编号列表

                foreach (EnumRebarPicType _type in _typelist)
                {
                    _detaildata = new GeneralDetailData();
                    foreach (RebarData _ddd in _rebarDataList /*_newdatalist*/)
                    {
                        //把TotalPieceNum==0的多段排除出去
                        if (_ddd.PicTypeNum == _type.ToString().Substring(2, 5) && (_ddd.TotalPieceNum != 0) &&
                           (_iffilter ? (_ddd.IfCut == _ifcut && _ddd.IfBend == _ifbend && _ddd.IfTao == _iftao) : true))
                        {
                            _detaildata.TotalPieceNum += _ddd.TotalPieceNum;
                            _detaildata.TotalWeight += _ddd.TotalWeight;

                            #region 处理长度数值例外的情况,临时做法，如果长度数值有换行符号，则给0
                            //_detaildata.TotalLength += Convert.ToInt32(_ddd.Length);
                            int _llll = 0;
                            if (int.TryParse(_ddd.Length, out _llll))
                            {
                                _detaildata.TotalLength += _llll;
                            }
                            #endregion

                            //解析套丝数据，
                            //示例：0,套;2250,-90;300,0&D
                            //      0,32套; 4000,丝
                            //string[] sss = _ddd.CornerMessage.Split(';');
                            string[] sss = _ddd.CornerMessage.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                            foreach (string ss in sss)
                            {
                                string[] s = ss.Split(',');
                                if (s.Length > 1)
                                {
                                    if (s[1].Equals("套"))
                                    {
                                        _detaildata.TaosiNum += 2;
                                        _detaildata.TaotongNum += 1;
                                        _detaildata.TaotongNum_P += 1;
                                    }
                                    else if (s[1].Equals("丝"))
                                    {
                                        _detaildata.TaosiNum += 1;
                                    }
                                    else if (s[1].Equals("反套"))
                                    {
                                        _detaildata.TaosiNum += 2;
                                        _detaildata.TaotongNum += 1;
                                        _detaildata.TaotongNum_N += 1;
                                    }
                                    else if (s[1].Equals("反丝"))
                                    {
                                        _detaildata.TaosiNum += 1;
                                    }
                                    else if (s[1].IndexOf("套") > 0)   //套字不在前面，则为变径套筒
                                    {
                                        _detaildata.TaosiNum += 2;
                                        _detaildata.TaotongNum += 1;
                                        _detaildata.TaotongNum_V += 1;
                                    }
                                }

                            }

                            _detaildata.CutNum += 1;
                            //计算弯曲数量，切断数量
                            if (_ddd.PicTypeNum.Substring(0, 1) == "2")
                            {
                                _detaildata.BendNum += 1;
                            }
                            else if (_ddd.PicTypeNum.Substring(0, 1) == "3")
                            {
                                _detaildata.BendNum += 2;
                            }
                            else if (_ddd.PicTypeNum.Substring(0, 1) == "4")
                            {
                                _detaildata.BendNum += 3;
                            }
                            else if (_ddd.PicTypeNum.Substring(0, 1) == "5")
                            {
                                _detaildata.BendNum += 4;
                            }
                            else if (_ddd.PicTypeNum.Substring(0, 1) == "6")
                            {
                                _detaildata.BendNum += 6;//不定，平均等于6吧
                            }
                            else if (_ddd.PicTypeNum.Substring(0, 1) == "7")
                            {
                                _detaildata.BendNum += 6;
                            }


                        }
                    }

                    _pair = new KeyValuePair<EnumRebarPicType, GeneralDetailData>(_type, _detaildata);
                    _data.Add(_pair);
                    //_data.Add(_detaildata);
                }

                return _data;
            }
            catch (Exception ex) { MessageBox.Show("Detail_other error:" + ex.Message); return null; }

        }
        public List<KeyValuePair<EnumRebarPicType, GeneralDetailData>>[] DetailAnalysis2(List<RebarData> _rebarDataList, bool _iffilter, bool _ifcut, bool _ifbend, bool _iftao)
        {
            try
            {
                //List<GeneralDetailData>[] _allList=new List<GeneralDetailData>[(int)EnumDetailTableRowName.maxRowNum];
                List<KeyValuePair<EnumRebarPicType, GeneralDetailData>>[] _allList = new List<KeyValuePair<EnumRebarPicType, GeneralDetailData>>[(int)EnumBangOrXian.maxRowNum];

                //List<GeneralDetailData> _list = new List<GeneralDetailData>();
                List<KeyValuePair<EnumRebarPicType, GeneralDetailData>> _list = new List<KeyValuePair<EnumRebarPicType, GeneralDetailData>>();
                string _level = "";
                int _diameter = 0;

                for (int i = (int)EnumBangOrXian.XIAN_A6; i < (int)EnumBangOrXian.maxRowNum; i++)
                {
                    //_list.Clear();//注意要先清掉
                    string ss = ((EnumBangOrXian)i).ToString().Split('_')[1];//例如：BANG_C14，截取后一段
                    _level = ss.Substring(0, 1);
                    _diameter = Convert.ToInt32(ss.Substring(1, ss.Length - 1));

                    //第一步，先根据棒材直径分类
                    List<RebarData> _bangList = new List<RebarData>();
                    foreach (RebarData _rebar in _rebarDataList)
                    {
                        if (_rebar.Diameter == _diameter && _rebar.Level == _level)
                        //if (_rebar.Diameter == _diameter )//麻威说的，先不考虑级别，d级钢跟c级钢一样对待
                        {
                            _bangList.Add(_rebar);
                        }
                    }

                    _list = new List<KeyValuePair<EnumRebarPicType, GeneralDetailData>>();
                    if (_bangList.Count != 0)
                    {
                        _list = Detail_other(_bangList, _iffilter, _ifcut, _ifbend, _iftao);

                    }
                    _allList[i] = _list;
                }
                return _allList;

            }
            catch (Exception ex) { MessageBox.Show("DetailAnalysis2 error:" + ex.Message); return null; }

        }

        //public List<GroupbyProjectAssemblyList> QueryAllListByProjectAssembly(List<RebarData> _rebardatalist)
        //{
        //    List<GroupbyProjectAssemblyList> returnlist = new List<GroupbyProjectAssemblyList>();
        //    returnlist = _rebardatalist.GroupBy(x => new { x.ProjectName, x.MainAssemblyName }).Select(
        //        y => new GroupbyProjectAssemblyList
        //        {
        //            _projectName = y.Key.ProjectName,
        //            _assemblyName = y.Key.MainAssemblyName,
        //            _datalist = y.ToList()
        //        }).ToList();
        //    return returnlist;
        //}
        public List<GroupbyDiaWithLength> QueryAllListByDiameterWithLength(List<RebarData> _rebardatalist)
        {
            try
            {
                int _length = 0;

                List<GroupbyDiaWithLength> returnlist = new List<GroupbyDiaWithLength>();
                returnlist = _rebardatalist.GroupBy(x => x.Diameter).Select(
                    y => new GroupbyDiaWithLength
                    {
                        _diameter = y.Key,
                        //_totallength = y.Sum(item => Convert.ToInt32(item.Length) * item.TotalPieceNum),
                        //_totallength = y.Sum(item => (double.TryParse(item.Length, out _length) ? _length : 0) * item.TotalPieceNum),
                        _totallength = y.Sum(item => (int.TryParse(item.Length, out _length) ? _length : ((Convert.ToInt32(item.Length.Split('~')[0]) + Convert.ToInt32(item.Length.Split('~')[1])) / 2)) * item.TotalPieceNum),
                        _maxlength = y.Max(item => (int.TryParse(item.Length, out _length) ? _length : ((Convert.ToInt32(item.Length.Split('~')[0]) + Convert.ToInt32(item.Length.Split('~')[1])) / 2))),
                        _minlength = y.Min(item => (int.TryParse(item.Length, out _length) ? _length : ((Convert.ToInt32(item.Length.Split('~')[0]) + Convert.ToInt32(item.Length.Split('~')[1])) / 2))),

                        _totalnum = y.Sum(item => item.TotalPieceNum),
                        _totalweight = y.Sum(item => item.TotalWeight),
                        _datalist = y.ToList()
                    }
                    ).ToList();

                return returnlist;
            }
            catch (Exception ex) { MessageBox.Show("QueryAllListByDiameterWithLength error:" + ex.Message); return null; }

        }

        public List<GroupbyDia> QueryAllListByDiameter(List<RebarData> _rebardatalist)
        {
            List<GroupbyDia> returnlist = new List<GroupbyDia>();
            returnlist = _rebardatalist.GroupBy(x => x.Diameter).Select(
                y => new GroupbyDia
                {
                    _diameter = y.Key,
                    _totalnum = y.Sum(item => item.TotalPieceNum),
                    _totalweight = y.Sum(item => item.TotalWeight),
                    _datalist = y.ToList()
                }).ToList();

            return returnlist;
        }
        public List<GroupbyTaoBendDatalist> QueryAllListByTaoBend(List<RebarData> _rebardatalist)
        {
            List<GroupbyTaoBendDatalist> returnlist = new List<GroupbyTaoBendDatalist>();

            returnlist = _rebardatalist.GroupBy(x => new { x.IfTao, x.IfBend }).Select(
                y => new GroupbyTaoBendDatalist
                {
                    _iftao = y.Key.IfTao,
                    _ifbend = y.Key.IfBend,
                    _totalnum = y.Sum(item => item.TotalPieceNum),
                    _totalweight = y.Sum(item => item.TotalWeight),
                    _datalist = y.ToList()
                }).ToList();

            return returnlist;
        }

        public List<GroupbyLengthDatalist> QueryAllListByLength(List<RebarData> _rebardatalist)
        {
            List<GroupbyLengthDatalist> returnlist = new List<GroupbyLengthDatalist>();

            returnlist = _rebardatalist.GroupBy(x => x.Length).Select(
                y => new GroupbyLengthDatalist
                {
                    _length = y.Key,
                    _totalnum = y.Sum(item => item.TotalPieceNum),
                    _totalweight = y.Sum(item => item.TotalWeight),
                    _datalist = y.ToList()
                }).ToList();



            ////******* 对集合按Name属于进行分组GroupBy查询 ********  
            ////结果中包括的字段：  
            ////1、分组的关键字：Name = g.Key  
            ////2、每个分组的数量：count = g.Count()  
            ////3、每个分组的年龄总和：ageC = g.Sum(item => item.Age)  
            ////4、每个分组的收入总和：moneyC = g.Sum(item => item.Money)  

            ////写法1：Lambda 表达式写法（推荐）  
            //var ls = persons1.GroupBy(a => a.Name).Select(g => (new { 
            //    name = g.Key, 
            //    count = g.Count(),
            //    ageC = g.Sum(item => item.Age), 
            //    moneyC = g.Sum(item => item.Money) 
            //}));
            ////写法2：类SQL语言写法 最终编译器会把它转化为lamda表达式  
            //var ls2 = from ps in persons1
            //          group ps by ps.Name
            //             into g
            //          select new { name = g.Key, count = g.Count(), ageC = g.Sum(item => item.Age), moneyC = g.Sum(item => item.Money) };



            return returnlist;

        }
        /// <summary>
        /// 从钢筋总表中汇总出所有的钢筋图形
        /// </summary>
        /// <param name="_rebardatalist"></param>
        /// <returns></returns>
        public List<EnumRebarPicType> GetExistedRebarTypeList(List<RebarData> _rebardatalist)
        {
            List<EnumRebarPicType> _typelist = new List<EnumRebarPicType>();

            try
            {
                var item = _rebardatalist.GroupBy(t => t.PicTypeNum).ToList();

                foreach (var ttt in item)
                {
                    string s = "T_" + ttt.Key;
                    EnumRebarPicType _enum /*= (EnumRebarPicType)Enum.Parse(typeof(EnumRebarPicType), s, true)*/;

                    if (Enum.TryParse(s, out _enum))
                    {
                        _typelist.Add(_enum);
                    }
                    else
                    {
                        MessageBox.Show("GetExistedRebarTypeList error:" + s + " No Found!");
                    }

                }

                return _typelist;
            }
            catch (Exception ex) { MessageBox.Show("GetExistedRebarTypeList error:" + ex.Message); return _typelist; }

        }
        /// <summary>
        /// 根据钢筋总表名称、项目名称、主构件名称，得到所有的构件包
        /// </summary>
        /// <param name="_tableName">钢筋总表名称，allsheet</param>
        /// <param name="_excelName">料表名称，为excel文件名</param>
        /// <param name="_excelsheetName">料表sheet名称，为excel中的sheet名</param>
        /// <returns>返回构件包list</returns>
        public List<ElementData> GetAllElementList(string _tableName, string _excelName = "", string _excelsheetName = "")
        {
            List<ElementData> _elementList = new List<ElementData>();
            ElementData _element = null;

            //GeneralClass.interactivityData?.printlog(1, "1");

            //List<RebarData> _rebarlist = GetAllRebarList(_tableName);
            List<RebarData> _rebarlist = new List<RebarData>();

            GeneralClass.AllRebarList = GeneralClass.DBOpt.GetAllRebarList(GeneralClass.TableName_AllRebar);//取得所有的钢筋数据list
                                                                                                            //GeneralClass.interactivityData?.printlog(1, "2");

            //根据项目名称、主构件名称获取所有的钢筋list
            //List<GroupbyProjectAssemblyList> _grouplist = QueryAllListByProjectAssembly(GeneralClass.AllRebarList);
            var _grouplist = GeneralClass.AllRebarList.GroupBy(x => new { x.TableName, x.TableSheetName }).Select(
                 y => new
                 {
                     _excelName = y.Key.TableName,
                     _excelsheetName = y.Key.TableSheetName,
                     _datalist = y.ToList()
                 }).ToList();

            //判断是否输入有效的_excelName和_excelsheetName
            if (_excelName == "" && _excelsheetName == "")
            {
                //_rebarlist = GeneralClass.AllRebarList.Select(t => new RebarData().Copy(t)).ToList();
                foreach (var item in GeneralClass.AllRebarList)
                {
                    RebarData temp = new RebarData();
                    temp.Copy(item);                //注意：此处应采用copy方法，进行深度拷贝，浅拷贝会改变原数据，容易出错
                    _rebarlist.Add(temp);
                }
            }
            else
            {
                foreach (var item in _grouplist)
                {
                    if (item._excelName == _excelName && item._excelsheetName == _excelsheetName)
                    {
                        foreach (var tt in item._datalist)
                        {
                            RebarData temp = new RebarData();
                            temp.Copy(tt);
                            _rebarlist.Add(temp);
                        }
                    }
                }

            }
            //GeneralClass.interactivityData?.printlog(1, "3");


            /*** 筛选组建真正的子构件包，
             * 策略：以【前一个不为空】且【连续两行不一致】的子构件名分组，生成一个构件包，并将子构件名作为构件包名称
             * 注意有很多特殊情况：
             * 1、某些翻样人员不按规范，将构件包的注释加在子构件名下面，造成构件包识别时，拆分开了
             *      解决方案：增加规则，前一行子构件名含“#”字符为子构件名，不含则判定为注释说明**/
            for (int i = 0; i < _rebarlist.Count; i++)
            {
                //首行先创建构件
                if (i == 0)
                {
                    _element = new ElementData();
                    _element.projectName = _rebarlist[i].ProjectName;
                    _element.mainAssemblyName = _rebarlist[i].MainAssemblyName;
                    _element.childAssemblyName = _rebarlist[i].ChildAssemblyName;
                    _element.elementName = (_rebarlist[i].ElementName != "") ? _rebarlist[i].ElementName : "default";//极少数情况，会出现首行没有构件名，用缺省名代替                                 
                    _element.rebarlist.Add(_rebarlist[i]);

                    if (_rebarlist.Count==1) { _elementList.Add(_element);break; }//如果只有一行，则第一行就是一个单独构件，20250926

                    continue;
                }
                //GeneralClass.interactivityData?.printlog(1, "3.1");

                //int curIndex = _rebarlist.IndexOf(item);//经测试，indexof会占用大量时间

                //GeneralClass.interactivityData?.printlog(1, "3.2");

                if (i != 0 && _rebarlist[i].ElementName != "" &&
                   (_rebarlist[i].ElementName != _rebarlist[i - 1].ElementName ||
                    _rebarlist[i].ChildAssemblyName != _rebarlist[i - 1].ChildAssemblyName))//构件名不为空，且跟上一个rebardata的构件名或子构件部位名有一个不一样，则新建构件
                {
                    ////if (_rebarlist[i - 1].ElementName.IndexOf('#') > -1 && _rebarlist[i].ElementName.IndexOf('#') == -1)//前一行含有“#”，本行不含“#”，则表示本行为注释
                    //if (_rebarlist[i].ElementName.IndexOf('#') == -1)//本行不含“#”，则表示本行为注释，20240517修改
                    //{
                    //    _element.rebarlist.Add(_rebarlist[i]);//本行为注释的，纳入同一个构件包
                    //    if (_rebarlist[i].ElementName != _rebarlist[i - 1].ElementName)
                    //    {
                    //        _element.elementName += _rebarlist[i].ElementName;  //将本行的注释也加入到构件名
                    //    }
                    //    //_rebarlist[i - 1].ElementName += _rebarlist[i].ElementName;//修改前一行的子构件名
                    //    //_rebarlist[i].ElementName = _rebarlist[i - 1].ElementName;//更新当前行的子构件名
                    //}
                    //else
                    //{
                    if (_element != null)
                    {
                        _elementList.Add(_element);
                    }
                    _element = new ElementData();
                    _element.projectName = _rebarlist[i].ProjectName;
                    _element.mainAssemblyName = _rebarlist[i].MainAssemblyName;
                    _element.childAssemblyName = _rebarlist[i].ChildAssemblyName;
                    _element.elementName = _rebarlist[i].ElementName;
                    _element.rebarlist.Add(_rebarlist[i]);
                    //}


                }
                else
                {
                    _element.rebarlist.Add(_rebarlist[i]);//子构件名称为空的，默认为跟前一个不为空的是同一个构件的

                }

                if (i == _rebarlist.Count - 1)//最后一行保存最后一个构件
                {
                    _elementList.Add(_element);
                }
            }


            ////20250624，处理一下构件队列，按照重量对构件，以及构件里面的零件进行拆分
            //List<ElementData> rt_elementList = new List<ElementData>();
            //foreach (var item in _elementList)
            //{
            //    rt_elementList.AddRange(SplitElement(item));
            //}

            ////给每个子构件一个index索引号
            //foreach (var item in rt_elementList)
            //{
            //    item.elementIndex = rt_elementList.IndexOf(item);//构件index
            //    item.elementTotalNum = rt_elementList.Count;//总构件数量
            //    item.elementName = item.elementName.Replace("\r", "").Replace("\n", "");//20250311修改信息化系统的bug，去除掉构件名称中的"\r"或者"\n"之类的换行符
            //}

            //return rt_elementList;

            //给每个子构件一个index索引号
            foreach (var item in _elementList)
            {
                item.elementIndex = _elementList.IndexOf(item);//构件index
                item.elementTotalNum = _elementList.Count;//总构件数量
                item.elementName = item.elementName.Replace("\r", "").Replace("\n", "");//20250311修改信息化系统的bug，去除掉构件名称中的"\r"或者"\n"之类的换行符
            }

            return _elementList;
        }

        private int GetMaxNumByDiameter(EnumDiaBang _diameter)
        {
            int _maxNum = 0;
            switch (_diameter)//先根据直径，确认按哪个数量来拆解
            {
                case EnumDiaBang.BANG_C16: { _maxNum = GeneralClass.CfgData.SplitNumThresholdWithBend_16; break; }
                case EnumDiaBang.BANG_C18: { _maxNum = GeneralClass.CfgData.SplitNumThresholdWithBend_18; break; }
                case EnumDiaBang.BANG_C20: { _maxNum = GeneralClass.CfgData.SplitNumThresholdWithBend_20; break; }
                case EnumDiaBang.BANG_C22: { _maxNum = GeneralClass.CfgData.SplitNumThresholdWithBend_22; break; }
                case EnumDiaBang.BANG_C25: { _maxNum = GeneralClass.CfgData.SplitNumThresholdWithBend_25; break; }
                case EnumDiaBang.BANG_C28: { _maxNum = GeneralClass.CfgData.SplitNumThresholdWithBend_28; break; }
                case EnumDiaBang.BANG_C32: { _maxNum = GeneralClass.CfgData.SplitNumThresholdWithBend_32; break; }
                default: { _maxNum = 50; break; }
            }

            return _maxNum;
        }
        /// <summary>
        /// 按数量分解零件，注意不同直径的分解数量不一样
        /// </summary>
        /// <param name="_rebardata"></param>
        /// <returns></returns>
        public List<RebarData> SplitRebarDataByNum(RebarData _rebardata)
        {
            try
            {


                List<RebarData> newRebarlist = new List<RebarData>();
                RebarData newRebar = new RebarData();


                int _maxNum = GetMaxNumByDiameter(GeneralClass.IntToEnumDiameter(_rebardata.Diameter));
                //按数量，先确定拆成几个包
                //int _count = (_rebardata.TotalPieceNum % _maxNum == 0) ? _rebardata.TotalPieceNum / _maxNum : (_rebardata.TotalPieceNum / _maxNum + 1);
                int _count = _rebardata.TotalPieceNum / _maxNum;

                double _singleweight = _rebardata.TotalWeight / _rebardata.TotalPieceNum;//单根重量

                for (int i = 0; i < _count; i++)
                {
                    newRebar = new RebarData();
                    newRebar.Copy(_rebardata);
                    newRebar.TotalPieceNum = _maxNum;
                    newRebar.TotalWeight = newRebar.TotalPieceNum * _singleweight;


                    newRebar.SerialNum = _rebardata.SerialNum + "-" + (i + 1).ToString();
                    newRebarlist.Add(newRebar);
                }

                if (_rebardata.TotalPieceNum % _maxNum != 0)//如果有余数，则最后一个rebardata加上余数
                {
                    newRebar = new RebarData();
                    newRebar.Copy(_rebardata);
                    newRebar.TotalPieceNum = _rebardata.TotalPieceNum % _maxNum;
                    newRebar.TotalWeight = newRebar.TotalPieceNum * _singleweight;
                    newRebar.SerialNum = _rebardata.SerialNum + "-" + (_count + 1).ToString();
                    newRebarlist.Add(newRebar);
                }

                return newRebarlist;
            }
            catch (Exception ex) { MessageBox.Show("SplitRebarDataByNum error:" + ex.Message); return new List<RebarData>(); }

        }
        /// <summary>
        /// 按重量将零件拆解，
        /// </summary>
        /// <param name="_maxWeight">重量拆解阈值</param>
        /// <param name="_rebardata">待拆解的rebardata</param>
        /// <returns></returns>
        public List<RebarData> SplitRebarDataByWeight(double _maxWeight, RebarData _rebardata)
        {
            try
            {

                List<RebarData> newRebarlist = new List<RebarData>();
                RebarData newRebar = new RebarData();

                //按重量，先确定拆成几个包
                int _count = ((int)_rebardata.TotalWeight % (int)_maxWeight == 0) ? (int)_rebardata.TotalWeight / (int)_maxWeight : ((int)_rebardata.TotalWeight / (int)_maxWeight + 1);
                double _singleweight = _rebardata.TotalWeight / _rebardata.TotalPieceNum;//单根重量

                for (int i = 0; i < _count; i++)
                {
                    newRebar = new RebarData();
                    newRebar.Copy(_rebardata);
                    newRebar.TotalPieceNum = _rebardata.TotalPieceNum / _count;
                    newRebar.TotalWeight = newRebar.TotalPieceNum * _singleweight;

                    if (i == _count - 1)//最后一个rebardata加上余数
                    {
                        newRebar.TotalPieceNum += _rebardata.TotalPieceNum % _count;
                        newRebar.TotalWeight = newRebar.TotalPieceNum * _singleweight;
                    }
                    newRebar.SerialNum = _rebardata.SerialNum + "-" + (i + 1).ToString();
                    newRebarlist.Add(newRebar);
                }

                return newRebarlist;
            }
            catch (Exception ex) { MessageBox.Show("SplitRebarDataByWeight error:" + ex.Message); return new List<RebarData>(); }

        }
        /// <summary>
        /// 按照重量和数量综合拆分
        /// </summary>
        /// <param name="_maxWeight"></param>
        /// <param name="_rebardata"></param>
        /// <returns></returns>
        public List<RebarData> SplitRebarDataByWeightAndNum(double _maxWeight, RebarData _rebardata)
        {
            try
            {
                List<RebarData> newRebarlist = new List<RebarData>();
                RebarData newRebar = new RebarData();

                int _count = 1;//思路：count逐渐增大，
                while(true)
                {
                    double _weight = _rebardata.TotalWeight/_count;//
                    int _num = _rebardata.TotalPieceNum/_count;

                    if (_weight>_maxWeight||
                        (_rebardata.IfBendOri &&
                           GeneralClass.IntToEnumDiameter(_rebardata.Diameter) >= EnumDiaBang.BANG_C16 &&
                            GeneralClass.IntToEnumDiameter(_rebardata.Diameter) <= EnumDiaBang.BANG_C32 &&
                            _num > GetMaxNumByDiameter(GeneralClass.IntToEnumDiameter(_rebardata.Diameter))))//如果超重，或者超数量了，则增加count，继续拆分
                    {
                        _count++;
                        continue;
                    }
                    else
                    {
                        if (_count==1)//如果不用拆解，则直接返回
                        {
                            newRebar = new RebarData();
                            newRebar.Copy(_rebardata);
                            newRebarlist.Add(_rebardata);
                            break;
                        }

                        double _singleweight = _rebardata.TotalWeight / _rebardata.TotalPieceNum;//单根重量

                        for (int i = 0; i < _count; i++)
                        {
                            newRebar = new RebarData();
                            newRebar.Copy(_rebardata);
                            newRebar.TotalPieceNum = _rebardata.TotalPieceNum / _count;
                            newRebar.TotalWeight = newRebar.TotalPieceNum * _singleweight;

                            if (i == _count - 1)//最后一个rebardata加上余数
                            {
                                newRebar.TotalPieceNum += _rebardata.TotalPieceNum % _count;
                                newRebar.TotalWeight = newRebar.TotalPieceNum * _singleweight;
                            }
                            newRebar.SerialNum = _rebardata.SerialNum + "-" + (i + 1).ToString() + "/" + _count.ToString();//20250908增加【/count】
                            newRebarlist.Add(newRebar);
                        }
                        break;
                    }

                }

                return newRebarlist;


            }
            catch (Exception ex) { MessageBox.Show("SplitRebarDataByWeightAndNum error:" + ex.Message); return new List<RebarData>(); }
        }

        /// <summary>
        /// 零件拆分策略：
        ///         原则1：单一rebardata的重量如果超过了3吨，需要拆解成多个
        /// </summary>
        /// <param name="_rebarlist"></param>
        /// <returns></returns>
        public List<RebarData> SplitRebarData(List<RebarData> _rebarlist)
        {
            try
            {
                double _maxWeight = GeneralClass.CfgData.SplitWeightThreshold;//单包最大重量

                List<RebarData> newRebarlist = new List<RebarData>();

                foreach(var item in _rebarlist)
                {
                    newRebarlist.AddRange(SplitRebarDataByWeightAndNum(_maxWeight, item));
                }
                return newRebarlist;

                #region old
                //List<RebarData> newRebarlist_1 = new List<RebarData>();
                //List<RebarData> newRebarlist_2 = new List<RebarData>();

                //RebarData newRebar = new RebarData();

                //if (GeneralClass.CfgData.SplitIfWeightFirst)//重量优先
                //{
                //    //先过一遍，按重量分解
                //    foreach (var item in _rebarlist)
                //    {
                //        if (item.TotalWeight > _maxWeight)//如果单一rebardata超重，则打散
                //        {
                //            newRebarlist_1.AddRange(SplitRebarDataByWeight(_maxWeight, item));
                //        }
                //        else
                //        {
                //            newRebar = new RebarData();
                //            newRebar.Copy(item);
                //            newRebarlist_1.Add(newRebar);
                //        }
                //    }

                //    //再过一遍rebarlist，如果有数量超标的，先分解，原则是满足：弯曲、直径16~32、数量大于阈值
                //    foreach (var item in newRebarlist_1)
                //    {
                //        if (item.IfBendOri &&
                //           GeneralClass.IntToEnumDiameter(item.Diameter) >= EnumDiaBang.BANG_C16 &&
                //            GeneralClass.IntToEnumDiameter(item.Diameter) <= EnumDiaBang.BANG_C32 &&
                //            item.TotalPieceNum > GetMaxNumByDiameter(GeneralClass.IntToEnumDiameter(item.Diameter)))//弯曲、直径16~32、数量大于阈值
                //        {
                //            newRebarlist_2.AddRange(SplitRebarDataByNum(item));
                //        }
                //        else //没有超过数量的，则保留
                //        {
                //            newRebar = new RebarData();
                //            newRebar.Copy(item);
                //            newRebarlist_2.Add(newRebar);
                //        }
                //    }
                //}
                //else//数量优先
                //{
                //    //先过一遍rebarlist，如果有数量超标的，先分解，原则是满足：弯曲、直径16~32、数量大于阈值
                //    foreach (var item in _rebarlist)
                //    {
                //        if (item.IfBendOri &&
                //           GeneralClass.IntToEnumDiameter(item.Diameter) >= EnumDiaBang.BANG_C16 &&
                //            GeneralClass.IntToEnumDiameter(item.Diameter) <= EnumDiaBang.BANG_C32 &&
                //            item.TotalPieceNum > GetMaxNumByDiameter(GeneralClass.IntToEnumDiameter(item.Diameter)))//弯曲、直径16~32、数量大于阈值
                //        {
                //            newRebarlist_1.AddRange(SplitRebarDataByNum(item));
                //        }
                //        else //没有超过数量的，则保留
                //        {
                //            newRebar = new RebarData();
                //            newRebar.Copy(item);
                //            newRebarlist_1.Add(newRebar);
                //        }
                //    }

                //    //再过一遍，按重量分解
                //    foreach (var item in newRebarlist_1)
                //    {
                //        if (item.TotalWeight > _maxWeight)//如果单一rebardata超重，则打散
                //        {
                //            newRebarlist_2.AddRange(SplitRebarDataByWeight(_maxWeight, item));
                //        }
                //        else
                //        {
                //            newRebar = new RebarData();
                //            newRebar.Copy(item);
                //            newRebarlist_2.Add(newRebar);
                //        }
                //    }
                //}

                //return newRebarlist_2;
                #endregion




            }
            catch (Exception ex) { MessageBox.Show("SplitRebarData error:" + ex.Message); return new List<RebarData>(); }

        }
        /// <summary>
        ///  构件拆分策略：
        ///             原则1：单一rebardata的重量如果超过了3吨，需要单拎出来
        ///             原则2：单一构件的rebardata太多，超过30个，则需要拆为多个构件，名称后加“-1/-2/-3”等
        ///             原则3：单一rebardata的重量如果超过了3吨，需要拆为多个构件，名称后加“-1/-2/-3”等
        /// </summary>
        /// <param name="_element"></param>
        /// <returns></returns>
        public List<ElementData> SplitElement(ElementData _element)
        {
            double _maxWeight = 3000;//单包最大重量

            List<ElementData> elementList = new List<ElementData>();
            ElementData newElement = new ElementData();

            List<RebarData> newRebarlist = new List<RebarData>();
            RebarData newRebar = new RebarData();

            //先过一遍rebarlist，如果有单一rebardata重量超过3吨，先将其打散
            foreach (var item in _element.rebarlist)
            {
                if (item.TotalWeight > _maxWeight)//如果单一rebardata超重，则打散
                {
                    //按重量，先确定拆成几个包
                    int _count = ((int)item.TotalWeight % (int)_maxWeight == 0) ? (int)item.TotalWeight / (int)_maxWeight : ((int)item.TotalWeight / (int)_maxWeight + 1);
                    double _singleweight = item.TotalWeight / item.TotalPieceNum;//单根重量

                    for (int i = 0; i < _count; i++)
                    {
                        newRebar = new RebarData();
                        newRebar.Copy(item);
                        newRebar.TotalPieceNum = item.TotalPieceNum / _count;
                        newRebar.TotalWeight = newRebar.TotalPieceNum * _singleweight;

                        if (i == _count - 1)//最后一个rebardata加上余数
                        {
                            newRebar.TotalPieceNum += item.TotalPieceNum % _count;
                            newRebar.TotalWeight = newRebar.TotalPieceNum * _singleweight;
                        }
                        newRebar.SerialNum = item.SerialNum + "-" + (i + 1).ToString();
                        newRebarlist.Add(newRebar);
                    }

                }
                else //没有超重，则保留
                {
                    newRebar = new RebarData();
                    newRebar.Copy(item);
                    newRebarlist.Add(newRebar);
                }
            }

            newElement.Copy(_element);
            newElement.rebarlist.Clear();
            foreach (var item in newRebarlist)
            {
                newElement.rebarlist.Add(item);
            }

            elementList.Add(newElement);

            return elementList;
        }
        /// <summary>
        /// 从数据库中取出所有的钢筋数据
        /// </summary>
        /// <param name="_tableName"></param>
        /// <returns>返回所有的钢筋list</returns>
        public List<RebarData> GetAllRebarList(string _tableName)
        {
            try
            {


                List<RebarData> allRebarList = new List<RebarData>();   //钢筋总表的list
                RebarData rebarData = new RebarData();              //新建一个对象

                DataTable dt = dbHelper.GetDataTable(_tableName);

                if (dt != null)
                {
                    //从db中取出钢筋总表list
                    foreach (DataRow row in dt.Rows)
                    {
                        rebarData = new RebarData();
                        rebarData.IndexNo = int.Parse(row[0].ToString());//数据库索引
                        rebarData.ProjectName = row[(int)EnumAllRebarTableColName.PROJECT_NAME + 1].ToString();
                        rebarData.MainAssemblyName = row[(int)EnumAllRebarTableColName.MAIN_ASSEMBLY_NAME + 1].ToString();
                        rebarData.ChildAssemblyName = row[(int)EnumAllRebarTableColName.CHILD_ASSEMBLY_NAME + 1].ToString();
                        //rebarData.ElementName = row[(int)EnumAllRebarTableColName.ELEMENT_NAME + 1].ToString();
                        rebarData.ElementName = String.Join("", row[(int)EnumAllRebarTableColName.ELEMENT_NAME + 1].ToString().Split('\n'));

                        rebarData.TableNo = row[(int)EnumAllRebarTableColName.TABLE_NO + 1].ToString();
                        rebarData.TableName = row[(int)EnumAllRebarTableColName.TABLE_NAME + 1].ToString();
                        //rebarData.TableName = String.Join("", row[(int)EnumAllRebarTableColName.TABLE_NAME + 1].ToString().Split('\n'));
                        rebarData.TableSheetName = row[(int)EnumAllRebarTableColName.TABLE_SHEET_NAME + 1].ToString();

                        rebarData.PicTypeNum = row[(int)EnumAllRebarTableColName.PIC_NO + 1].ToString();
                        rebarData.Level = row[(int)EnumAllRebarTableColName.LEVEL + 1].ToString();
                        rebarData.Diameter = Convert.ToInt32(row[(int)EnumAllRebarTableColName.DIAMETER + 1].ToString());
                        rebarData.RebarPic = row[(int)EnumAllRebarTableColName.REBAR_PIC + 1].ToString();
                        rebarData.PicMessage = row[(int)EnumAllRebarTableColName.PIC_MESSAGE + 1].ToString();
                        rebarData.CornerMessage = row[(int)EnumAllRebarTableColName.CORNER_MESSAGE + 1].ToString();
                        rebarData.CornerMessageBK = row[(int)EnumAllRebarTableColName.CORNER_MESSAGE_BK + 1].ToString();//备份一下cornerMessage
                        rebarData.Length = row[(int)EnumAllRebarTableColName.LENGTH + 1].ToString();

                        //20250205关闭，因修改ismulti属性
                        //if (GeneralClass.CfgData.DatabaseType == EnumDatabaseType.SQLITE)
                        //{
                        //    rebarData.IsMulti = Convert.ToBoolean(row[(int)EnumAllRebarTableColName.ISMULTI + 1].ToString());
                        //}
                        //if (GeneralClass.CfgData.DatabaseType == EnumDatabaseType.MYSQL)
                        //{
                        //    rebarData.IsMulti = Convert.ToInt16(row[(int)EnumAllRebarTableColName.ISMULTI + 1].ToString()) == 1 ? true : false;
                        //}


                        rebarData.PieceNumUnitNum = row[(int)EnumAllRebarTableColName.PIECE_NUM_UNIT_NUM + 1].ToString();
                        rebarData.TotalPieceNum = Convert.ToInt32(row[(int)EnumAllRebarTableColName.TOTAL_PIECE_NUM + 1].ToString());
                        rebarData.TotalWeight = Convert.ToDouble(row[(int)EnumAllRebarTableColName.TOTAL_WEIGHT + 1].ToString());
                        rebarData.Description = row[(int)EnumAllRebarTableColName.DESCRIPTION + 1].ToString();
                        //rebarData.SerialNum = Convert.ToInt32(row[(int)EnumAllRebarTableColName.SERIALNUM + 1].ToString());
                        rebarData.SerialNum = row[(int)EnumAllRebarTableColName.SERIALNUM + 1].ToString();

                        //rebarData.IsOriginal = Convert.ToBoolean(row[(int)EnumAllRebarTableColName.ISORIGINAL + 1].ToString());
                        //rebarData.IfTao = Convert.ToBoolean(row[(int)EnumAllRebarTableColName.IFTAO + 1].ToString());
                        //rebarData.IfBend = Convert.ToBoolean(row[(int)EnumAllRebarTableColName.IFBEND + 1].ToString());
                        //rebarData.IfCut = Convert.ToBoolean(row[(int)EnumAllRebarTableColName.IFCUT + 1].ToString());
                        //rebarData.IfBendTwice = Convert.ToBoolean(row[(int)EnumAllRebarTableColName.IFBENDTWICE + 1].ToString());
                        rebarData.BarType = row[(int)EnumAllRebarTableColName.BAR_TYPE + 1].ToString();
                        rebarData.FabricationType = row[(int)EnumAllRebarTableColName.FABRICATION_TYPE + 1].ToString();

                        if (rebarData.TotalPieceNum != 0)//暂不去掉总根数为0的钢筋，20250719
                        {
                            allRebarList.Add(rebarData);
                        }
                    }
                }

                return allRebarList;
            }
            catch (Exception ex) { MessageBox.Show("GetAllRebarList error:" + ex.Message); return null; }

        }
        /// <summary>
        /// 判断字符串是否为整数，包括负整数
        /// </summary>
        /// <param name="_Value"></param>
        /// <returns></returns>
        public static bool RegexIsNumeric(string _Value)
        {
            try
            {
                Regex regex = new System.Text.RegularExpressions.Regex("^-?\\d+$");
                //Regex regex = new System.Text.RegularExpressions.Regex("^(-?[0-9]*[.]*[0-9]{0,3})$");

                if (regex.IsMatch(_Value)) return true;
                else return false;
            }
            catch (Exception)
            {
                return false;
            }

        }





        /// <summary>
        /// 将rebardata转化成List<KeyValuePair<string,object>>，用于插入一行数据到数据库
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        public List<KeyValuePair<string, object>> RebardataToList(RebarData _data)
        {
            try
            {
                List<KeyValuePair<string, object>> keyValuePairs = new List<KeyValuePair<string, object>>();

                for (int pp = 0; pp <= (int)EnumAllRebarTableColName.IFBENDTWICE; pp++)
                {
                    KeyValuePair<string, object> pair;
                    switch (pp)
                    {
                        case (int)EnumAllRebarTableColName.MAIN_ASSEMBLY_NAME:
                            pair = new KeyValuePair<string, object>(GeneralClass.sRebarColumnName[pp], _data.MainAssemblyName);
                            break;
                        case (int)EnumAllRebarTableColName.ELEMENT_NAME:
                            pair = new KeyValuePair<string, object>(GeneralClass.sRebarColumnName[pp], _data.ElementName);
                            break;
                        case (int)EnumAllRebarTableColName.PIC_NO:
                            pair = new KeyValuePair<string, object>(GeneralClass.sRebarColumnName[pp], _data.PicTypeNum);
                            break;
                        case (int)EnumAllRebarTableColName.LEVEL:
                            pair = new KeyValuePair<string, object>(GeneralClass.sRebarColumnName[pp], _data.Level);
                            break;
                        case (int)EnumAllRebarTableColName.DIAMETER:
                            pair = new KeyValuePair<string, object>(GeneralClass.sRebarColumnName[pp], _data.Diameter);
                            break;
                        case (int)EnumAllRebarTableColName.REBAR_PIC:
                            pair = new KeyValuePair<string, object>(GeneralClass.sRebarColumnName[pp], _data.RebarPic);
                            break;
                        case (int)EnumAllRebarTableColName.PIC_MESSAGE:
                            pair = new KeyValuePair<string, object>(GeneralClass.sRebarColumnName[pp], _data.PicMessage);
                            break;
                        case (int)EnumAllRebarTableColName.CORNER_MESSAGE:
                            pair = new KeyValuePair<string, object>(GeneralClass.sRebarColumnName[pp], _data.CornerMessage);
                            break;
                        case (int)EnumAllRebarTableColName.LENGTH:
                            pair = new KeyValuePair<string, object>(GeneralClass.sRebarColumnName[pp], _data.Length);
                            break;
                        case (int)EnumAllRebarTableColName.PIECE_NUM_UNIT_NUM:
                            pair = new KeyValuePair<string, object>(GeneralClass.sRebarColumnName[pp], _data.PieceNumUnitNum);
                            break;
                        case (int)EnumAllRebarTableColName.TOTAL_PIECE_NUM:
                            pair = new KeyValuePair<string, object>(GeneralClass.sRebarColumnName[pp], _data.TotalPieceNum);
                            break;
                        case (int)EnumAllRebarTableColName.TOTAL_WEIGHT:
                            pair = new KeyValuePair<string, object>(GeneralClass.sRebarColumnName[pp], _data.TotalWeight);
                            break;
                        case (int)EnumAllRebarTableColName.DESCRIPTION:
                            pair = new KeyValuePair<string, object>(GeneralClass.sRebarColumnName[pp], _data.Description);
                            break;
                        case (int)EnumAllRebarTableColName.SERIALNUM:
                            pair = new KeyValuePair<string, object>(GeneralClass.sRebarColumnName[pp], _data.SerialNum);
                            break;
                        default:
                            pair = new KeyValuePair<string, object>("", "");
                            break;
                    }

                    keyValuePairs.Add(pair);
                }

                return keyValuePairs;

            }
            catch (Exception ex) { MessageBox.Show("RebardataToList error:" + ex.Message); return null; }

        }



    }
}
