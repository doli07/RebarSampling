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
using static Org.BouncyCastle.Crypto.Digests.SkeinEngine;
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
    public class DBOpt
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
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.ELEMENT_NAME], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TABLE_NAME], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TABLE_SHEET_NAME], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PIC_NO], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.LEVEL], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.DIAMETER], dbHelper.GetDataType(DbType.Int32));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.REBAR_PIC], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PIC_MESSAGE], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.CORNER_MESSAGE], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.LENGTH], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.ISMULTI], dbHelper.GetDataType(DbType.Boolean));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PIECE_NUM_UNIT_NUM], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TOTAL_PIECE_NUM], dbHelper.GetDataType(DbType.Int32));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TOTAL_WEIGHT], dbHelper.GetDataType(DbType.Double));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.DESCRIPTION], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.SERIALNUM], dbHelper.GetDataType(DbType.Int32));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.ISORIGINAL], dbHelper.GetDataType(DbType.Boolean));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFTAO], dbHelper.GetDataType(DbType.Boolean));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFBEND], dbHelper.GetDataType(DbType.Boolean));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFCUT], dbHelper.GetDataType(DbType.Boolean));
                dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFBENDTWICE], dbHelper.GetDataType(DbType.Boolean));


                //        dbHelper.AddColumn(tableName, GeneralClass.RebarColumnName[i], rebarData.ElementName.GetType().Name);
            }
            catch (Exception ex) { throw ex; }

        }
        public string InsertRowElementData(string tableName, string projectName, string assemblyName, string elementName)
        {
            try
            {
                //示例：string sqlstr = "insert into " + tableName + "(" + columnName + ")" + "values(" + itemInfo + ")";

                string sqlstr = "insert into " + tableName + "(";

                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PROJECT_NAME] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.MAIN_ASSEMBLY_NAME] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.ELEMENT_NAME] + ",";

                sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);//去掉尾部的逗号
                sqlstr += ")" + "values(";

                sqlstr += "'" + projectName + "'" + ",";
                sqlstr += "'" + assemblyName + "'" + ",";
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
        public string InsertRowPiCut(string tableName,string _level, int _diameter, int _length, int _num, string _cornerMsg, string _pictypeNum,string _projectName,string _assemblyName,string _elementName)
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
                sqlstr += "'" + _pictypeNum + "'"+",";
                sqlstr += "'" + _projectName + "'" + ",";
                sqlstr += "'" + _assemblyName + "'" + ",";
                sqlstr += "'" + _elementName + "'"+",";

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
                sqlstr += "'" + _element.elementName + "'";

                //sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);//去掉尾部的逗号
                sqlstr += ")";

                return sqlstr;
            }
            catch (Exception e) { throw e; }
        }
        public string InsertRowPickData(string tableName, string projectName, string assemblyName, bool ifpick)
        {
            try
            {
                //示例：string sqlstr = "insert into " + tableName + "(" + columnName + ")" + "values(" + itemInfo + ")";

                string sqlstr = "insert into " + tableName + "(";

                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PROJECT_NAME] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.MAIN_ASSEMBLY_NAME] + ",";
                sqlstr += GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFPICK] + ",";

                sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);//去掉尾部的逗号
                sqlstr += ")" + "values(";

                sqlstr += "'" + projectName + "'" + ",";
                sqlstr += "'" + assemblyName + "'" + ",";
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

                            for (int i = 0; i <= (int)EnumAllRebarTableColName.IFBENDTWICE; i++)
                            {
                                sqlstr += GeneralClass.sRebarColumnName[i] + ",";
                            }
                            sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);//去掉尾部的逗号
                            sqlstr += ")" + "values(";

                            //for (int j = 0; j < (int)EnumAllRebarTableColName.maxRebarColNum; j++)
                            //{
                            //    sqlstr += "@" + GeneralClass.sRebarColumnName[j] + ",";
                            //}
                            sqlstr += "'" + rowdata.ProjectName + "'" + ",";
                            sqlstr += "'" + rowdata.MainAssemblyName + "'" + ",";
                            sqlstr += "'" + rowdata.ElementName + "'" + ",";
                            sqlstr += "'" + rowdata.TableName + "'" + ",";
                            sqlstr += "'" + rowdata.TableSheetName + "'" + ",";
                            sqlstr += "'" + rowdata.PicTypeNum + "'" + ",";
                            sqlstr += "'" + rowdata.Level + "'" + ",";
                            sqlstr += rowdata.Diameter.ToString() + ",";
                            sqlstr += "'" + rowdata.RebarPic + "'" + ",";
                            sqlstr += "'" + rowdata.PicMessage + "'" + ",";
                            sqlstr += "'" + rowdata.CornerMessage + "'" + ",";
                            sqlstr += "'" + rowdata.Length + "'" + ",";
                            sqlstr += rowdata.IsMulti.ToString() + ",";
                            sqlstr += "'" + rowdata.PieceNumUnitNum + "'" + ",";
                            sqlstr += rowdata.TotalPieceNum.ToString() + ",";
                            sqlstr += rowdata.TotalWeight.ToString() + ",";
                            sqlstr += "'" + rowdata.Description + "'" + ",";
                            sqlstr += rowdata.SerialNum.ToString() + ",";
                            sqlstr += rowdata.IsOriginal.ToString() + ",";
                            sqlstr += rowdata.IfTao.ToString() + ",";
                            sqlstr += rowdata.IfBend.ToString() + ",";
                            sqlstr += rowdata.IfCut.ToString() + ",";
                            sqlstr += rowdata.IfBendTwice.ToString();

                            //sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);//去掉尾部的逗号
                            sqlstr += ")";

                            return sqlstr;

                        }
                        catch (Exception) { throw; }
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
        public static void SaveAllPiCutToDB(bool _ifInit,List<List<Rebar>> _list)
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
                string _projectName=item[0].ProjectName;
                string _assemblyName = item[0].MainAssemblyName;
                string _elementName = item[0].ElementName;
                sqls.Add(GeneralClass.DBOpt.InsertRowPiCut(GeneralClass.TableName_PiCutBatch, _level,_diameter, _length, _num, _cornermsg, _picTypeNum,
                    _projectName,_assemblyName,_elementName));
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

            List<Tuple<string, string, DataTable>> _dtlist = GeneralClass.ExcelOpt?.GetAllSheet(filename);

            for (int i = 0; i < _dtlist.Count; i++)
            {
                DataTable dt = _dtlist[i].Item3;
                string projectName=_dtlist[i].Item1;//项目名称
                string assemblyName = _dtlist[i].Item2;//构件部位
                sheetname = dt.TableName;

                //检查是否有“序号”这一列
                bool _haveSeri = (dt.Columns[1].ColumnName == "序号") ? true : false;

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    DataRow dr = dt.Rows[j];
                    if (dr == null) continue;

                    int startindex = _haveSeri ? 2 : 1;

                    //存入数据库
                    if (dr["编号"].ToString() == "" || dr["边角结构"].ToString() == "")//以是否有钢筋类型编号作为此行是否为有效数据的判定条件
                    {
                        continue;
                    }

                    rebarData = new RebarData();//构建新的钢筋对象

                    rebarData.TableName = System.IO.Path.GetFileNameWithoutExtension(filename); //获取excel文件名作为根节点名称,不带后缀名;//将filename赋予钢筋的料表名称
                    rebarData.TableSheetName = sheetname;//将sheetname赋予钢筋的料表sheet名称
                    rebarData.ProjectName = projectName; //
                    rebarData.MainAssemblyName = assemblyName;//

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
                        rebarData.IsMulti = (sss.Length > 1) ? true : false;
                    }

                    rebarData.PieceNumUnitNum = dr[startindex + 6].ToString();//根数*件数

                    int.TryParse(dr[startindex + 7].ToString(), out tt_i);//总根数
                    rebarData.TotalPieceNum = tt_i;

                    double.TryParse(dr[startindex + 8].ToString(), out tt_d);//总重量，kg
                    rebarData.TotalWeight = tt_d;

                    rebarData.Description = dr[startindex + 9].ToString();//备注说明

                    //int.TryParse(dr[startindex + 10].ToString(), out tt_i);
                    //rebarData.SerialNum = tt_i;


                    //ModifyRebarData(ref rebarData);

                    //InsertRowData(tablename, rebarData);
                    sqls.Add(InsertRowRebarData(tablename, rebarData));
                }

            }

            dbHelper.ExecuteSqlsTran(sqls);//批量存入


        }

        /// <summary>
        /// 修改钢筋的图形编号，主要是拆分多段钢筋时使用，修改图形编号
        /// </summary>
        /// <param name="_data"></param>
        private void ModifyRebarPicNum(ref RebarData _data)
        {
            try
            {

                List<GeneralMultiData> _multidata = GetMultiData(_data.CornerMessage);

                if (_multidata.Count == 1 || (_multidata.Count == 2 && _multidata[0].ilength == 0))//只有一段multidata，一般都是10000的图形编号，或者有两段，第一段长度为0，一般是【0,套】的情况
                {
                    _data.PicTypeNum = "10000";
                }
                else if (_multidata.Count == 2 || (_multidata.Count == 3 && _multidata[0].ilength == 0))//有两段multidata的，分几种情况
                {
                    int _index = (_multidata.Count == 2) ? 0 : 1;//起始index
                    if (_multidata[_index].ilength <= _multidata[_index + 1].ilength && _multidata[_index].angle > 0)//第一段长度小于第二段，且弯曲角度大于0，一般为20100图形编号
                    {
                        _data.PicTypeNum = "20100";
                    }
                    else if (_multidata[_index].ilength <= _multidata[_index + 1].ilength && _multidata[_index].angle < 0)//第一段长度小于第二段，且弯曲角度大于0，一般为20100图形编号
                    {
                        _data.PicTypeNum = "20200";
                    }
                    else if (_multidata[_index].ilength > _multidata[_index + 1].ilength && _multidata[_index].angle > 0)//第一段长度小于第二段，且弯曲角度大于0，一般为20100图形编号
                    {
                        _data.PicTypeNum = "20001";
                    }
                    else if (_multidata[_index].ilength > _multidata[_index + 1].ilength && _multidata[_index].angle < 0)//第一段长度小于第二段，且弯曲角度大于0，一般为20100图形编号
                    {
                        _data.PicTypeNum = "20002";
                    }
                }
                else if (_multidata.Count == 3 || (_multidata.Count == 4 && _multidata[0].ilength == 0))
                {
                    int _index = (_multidata.Count == 3) ? 0 : 1;//起始index

                    if (_multidata[_index].angle > 0 && _multidata[_index + 1].angle < 0)//第一段角度大于0，第二段小于0，为30102图形
                    {
                        _data.PicTypeNum = "30102";
                    }
                    else if (_multidata[_index].angle < 0 && _multidata[_index + 1].angle > 0)
                    {
                        _data.PicTypeNum = "30201";
                    }
                    else if (_multidata[_index].angle > 0 && _multidata[_index + 1].angle > 0)
                    {
                        _data.PicTypeNum = "30101";
                    }
                    else if (_multidata[_index].angle < 0 && _multidata[_index + 1].angle < 0)
                    {
                        _data.PicTypeNum = "30202";
                    }

                }
                else
                {
                    GeneralClass.interactivityData?.printlog(1, "拆分多段钢筋,修改图形编号," + "picTypeNum==" + _data.PicTypeNum + " 未涉及，CornerMessage==" + _data.CornerMessage);
                    //MessageBox.Show("初始picTypeNum=="+_data.PicTypeNum+" 未涉及，CornerMessage==" + _data.CornerMessage);
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show("ModifyRebarPicNum error:" + ex.Message);
                GeneralClass.interactivityData?.printlog(1, "ModifyRebarPicNum error:" + ex.Message + "CornerMessage==" + _data.CornerMessage);
                return;
            }

        }

        ///// <summary>
        ///// 详细处理
        ///// </summary>
        ///// <param name="_data"></param>
        //private void ModifyRebarData(ref RebarData _data)
        //{
        //    try
        //    {
        //        //详细处理
        //        _data.IsOriginal = (_data.Length == "9000" || _data.Length == "12000") ? true : false;//标注是否为原材，长度为9000或者12000，为原材

        //        _data.IfTao = (_data.CornerMessage.IndexOf("套") > -1
        //            || _data.CornerMessage.IndexOf("丝") > -1
        //            || _data.CornerMessage.IndexOf("反") > -1) ? true : false;//如果边角结构信息中含有“套”或者“丝”或者“反”，则认为其需要套丝


        //        //_data.IfBend = (_data.TypeNum.Substring(0, 1) == "1") ? false : true;//如果图形编号是1开头的，则不用弯，其他都需要弯
        //        bool _ifbend = false;
        //        List<GeneralMultiData> _MultiData = GetMultiData(_data.CornerMessage);//拆解corner信息,如果存在bend类型的multidata，则需要弯曲,20230907修改bug
        //        if (_MultiData != null)
        //        {
        //            foreach (var item in _MultiData)
        //            {
        //                if (item.headType == EnumMultiHeadType.BEND) _ifbend = true;
        //            }
        //        }
        //        _data.IfBend = _ifbend;

        //        _data.IfCut = (_data.Length == "9000" || _data.Length == "12000") ? false : true;//标注是否需要切断，原材以外的都需要切断

        //        _data.IfBendTwice = (_data.PicTypeNum.Substring(0, 1) == "1"
        //            || _data.PicTypeNum.Substring(0, 1) == "2"
        //            || _data.PicTypeNum.Substring(0, 1) == "3") ? false : true;//1、2、3开头的图形编号为需要弯折两次以下的，其他的需要弯折2次以上

        //    }
        //    catch (Exception ex) { MessageBox.Show("ModifyRebarData error:" + ex.Message); return; }

        //}
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
        /// <summary>
        /// 根据不同的钢筋尺寸_diameter，和分析项_item，进行各个细分项的数据分析，并返回根据不同钢筋尺寸的数据list
        /// </summary>
        /// <param name="_rebarDataList"></param>
        /// <param name="_detail"></param>
        /// <param name="_return"></param>
        public object[,,] DetailAnalysis(List<RebarData> _rebarDataList)
        {
            try
            {
                //定义三维数组，尺寸*工艺*分析项
                //object[,,] _alldata = new object[(int)EnumDetailTableRowName.maxRowNum, (int)EnumDetailTableColName.ONLY_CUT, (int)EnumDetailItem.maxItemNum];//先处理三个原材的
                object[,,] _alldata = new object[(int)EnumDetailTableColName.ONLY_CUT, (int)EnumBangOrXian.maxRowNum, (int)EnumDetailItem.maxItemNum];//先处理三个原材的

                GeneralDetailData[] _data = new GeneralDetailData[(int)EnumDetailTableColName.ONLY_CUT];//先处理三个原材的

                string _level = "";
                int _diameter = 0;

                for (int i = (int)EnumBangOrXian.XIAN_A6; i < (int)EnumBangOrXian.maxRowNum; i++)
                {
                    string ss = ((EnumBangOrXian)i).ToString().Split('_')[1];//例如：BANG_C14，截取后一段
                    _level = ss.Substring(0, 1);
                    _diameter = Convert.ToInt32(ss.Substring(1, ss.Length - 1));

                    //第一步，先根据棒材直径分类
                    List<RebarData> _bangList = new List<RebarData>();
                    foreach (RebarData _rebar in _rebarDataList)
                    {
                        if (_rebar.Diameter == _diameter && _rebar.Level == _level)
                        {
                            _bangList.Add(_rebar);
                        }
                    }

                    if (_bangList.Count != 0)
                    {
                        _data = Detail_diameter(_bangList);


                        for (int j = (int)EnumDetailTableColName.ORIGINAL; j < (int)EnumDetailTableColName.ONLY_CUT; j++)
                        {
                            //_alldata[i, j, (int)EnumDetailItem.TOTAL_PIECE] = _data[j].TotalPieceNum;
                            //_alldata[i, j, (int)EnumDetailItem.TOTAL_LENGTH] = _data[j].TotalLength;
                            //_alldata[i, j, (int)EnumDetailItem.TOTAL_WEIGHT] = _data[j].TotalWeight;
                            //_alldata[i, j, (int)EnumDetailItem.TAO_SI_NUM] = _data[j].TaosiNum;
                            //_alldata[i, j, (int)EnumDetailItem.TAO_TONG_NUM] = _data[j].TaotongNum;
                            //_alldata[i, j, (int)EnumDetailItem.ZHENG_SI_TAO_TONG] = _data[j].TaotongNum_P;
                            //_alldata[i, j, (int)EnumDetailItem.FAN_SI_TAO_TONG] = _data[j].TaotongNum_N;
                            //_alldata[i, j, (int)EnumDetailItem.BIAN_JING_TAO_TONG] = _data[j].TaotongNum_V;
                            //_alldata[i, j, (int)EnumDetailItem.CUT_NUM] = _data[j].CutNum;
                            //_alldata[i, j, (int)EnumDetailItem.BEND_NUM] = _data[j].BendNum;
                            //_alldata[i, j, (int)EnumDetailItem.ZHI_NUM] = _data[j].StraightenedNum;
                            _alldata[j, i, (int)EnumDetailItem.TOTAL_PIECE] = _data[j].TotalPieceNum;
                            _alldata[j, i, (int)EnumDetailItem.TOTAL_LENGTH] = _data[j].TotalLength;
                            _alldata[j, i, (int)EnumDetailItem.TOTAL_WEIGHT] = _data[j].TotalWeight;
                            _alldata[j, i, (int)EnumDetailItem.TAO_SI_NUM] = _data[j].TaosiNum;
                            _alldata[j, i, (int)EnumDetailItem.TAO_TONG_NUM] = _data[j].TaotongNum;
                            _alldata[j, i, (int)EnumDetailItem.ZHENG_SI_TAO_TONG] = _data[j].TaotongNum_P;
                            _alldata[j, i, (int)EnumDetailItem.FAN_SI_TAO_TONG] = _data[j].TaotongNum_N;
                            _alldata[j, i, (int)EnumDetailItem.BIAN_JING_TAO_TONG] = _data[j].TaotongNum_V;
                            _alldata[j, i, (int)EnumDetailItem.CUT_NUM] = _data[j].CutNum;
                            _alldata[j, i, (int)EnumDetailItem.BEND_NUM] = _data[j].BendNum;
                            _alldata[j, i, (int)EnumDetailItem.ZHI_NUM] = _data[j].StraightenedNum;

                        }
                    }
                }

                return _alldata;




            }
            catch (Exception ex) { MessageBox.Show("DetailAnalysis error:" + ex.Message); return null; }

        }

        /// <summary>
        /// 在指定的钢筋直径,根据不同筛选项，返回不同工艺下的数据
        /// </summary>
        /// <param name="_rebarDataList"></param>
        /// <param name="_item"></param>
        /// <returns></returns>
        public GeneralDetailData[] Detail_diameter(List<RebarData> _rebarDataList)
        {
            GeneralDetailData[] data = new GeneralDetailData[(int)EnumDetailTableColName.ONLY_CUT];//先处理三个原材的

            for (int _item = 0; _item < (int)EnumDetailTableColName.ONLY_CUT; _item++)
            //for (int _item = 0; _item < (int)EnumDetailTableColName.maxColNum; _item++)
            {
                //
                switch (_item)
                {
                    case (int)EnumDetailTableColName.ORIGINAL://统计原材
                        {
                            data[(int)EnumDetailTableColName.ORIGINAL] = Detail_Original(_rebarDataList);
                            break;
                        }
                    case (int)EnumDetailTableColName.ONLY_TAO://统计原材仅套丝
                        {
                            data[(int)EnumDetailTableColName.ONLY_TAO] = Detail_OnlyTao(_rebarDataList);
                            break;
                        }
                    case (int)EnumDetailTableColName.ONLY_BEND://统计原材仅弯曲
                        {
                            data[(int)EnumDetailTableColName.ONLY_BEND] = Detail_OnlyBend(_rebarDataList);
                            break;
                        }
                        //case (int)EnumDetailTableColName.ONLY_CUT://统计仅切断
                        //    {
                        //        data[(int)EnumDetailTableColName.ONLY_CUT] = Detail_OnlyCut(_rebarDataList);
                        //        break;
                        //    }
                        //case (int)EnumDetailTableColName.CUT_TAO://统计切断+套丝
                        //    {
                        //        data[(int)EnumDetailTableColName.CUT_TAO] = Detail_CutTao(_rebarDataList);
                        //        break;
                        //    }
                        //case (int)EnumDetailTableColName.CUT_BEND_NOT_2://统计切断+弯曲不超过2次
                        //    {
                        //        data[(int)EnumDetailTableColName.CUT_BEND_NOT_2] = Detail_CutBendNot2(_rebarDataList);
                        //        break;
                        //    }
                        //case (int)EnumDetailTableColName.CUT_BEND_OVER_2://统计切断+弯曲超过2次
                        //    {
                        //        data[(int)EnumDetailTableColName.CUT_BEND_OVER_2] = Detail_CutBendOver2(_rebarDataList);
                        //        break;
                        //    }
                        //case (int)EnumDetailTableColName.CUT_TAO_BEND://统计切断+弯曲+套丝
                        //    {
                        //        data[(int)EnumDetailTableColName.CUT_TAO_BEND] = Detail_CutBendTao(_rebarDataList);
                        //        break;
                        //    }


                }
            }


            return data;

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
           var  _grouplist = GeneralClass.AllRebarList.GroupBy(x => new { x.TableName, x.TableSheetName }).Select(
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
                    _element.assemblyName = _rebarlist[i].MainAssemblyName;
                    _element.elementName = (_rebarlist[i].ElementName != "") ? _rebarlist[i].ElementName : "default";//极少数情况，会出现首行没有构件名，用缺省名代替                                 
                    _element.rebarlist.Add(_rebarlist[i]);

                    continue;
                }
                //GeneralClass.interactivityData?.printlog(1, "3.1");

                //int curIndex = _rebarlist.IndexOf(item);//经测试，indexof会占用大量时间

                //GeneralClass.interactivityData?.printlog(1, "3.2");

                if (i != 0 && _rebarlist[i].ElementName != "" && _rebarlist[i].ElementName != _rebarlist[i - 1].ElementName)//构件名不为空，且跟上一个元素的构件名不一样，则新建构件
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
                    _element.assemblyName = _rebarlist[i].MainAssemblyName;
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

            //GeneralClass.interactivityData?.printlog(1, "4");

            //给每个子构件一个index索引号
            foreach (var item in _elementList)
            {
                item.elementIndex = _elementList.IndexOf(item);
            }
            //GeneralClass.interactivityData?.printlog(1, "5");

            return _elementList;
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
                        //rebarData.ElementName = row[(int)EnumAllRebarTableColName.ELEMENT_NAME + 1].ToString();
                        rebarData.ElementName = String.Join("", row[(int)EnumAllRebarTableColName.ELEMENT_NAME + 1].ToString().Split('\n'));

                        rebarData.TableName = row[(int)EnumAllRebarTableColName.TABLE_NAME + 1].ToString();
                        //rebarData.TableName = String.Join("", row[(int)EnumAllRebarTableColName.TABLE_NAME + 1].ToString().Split('\n'));
                        rebarData.TableSheetName = row[(int)EnumAllRebarTableColName.TABLE_SHEET_NAME + 1].ToString();

                        rebarData.PicTypeNum = row[(int)EnumAllRebarTableColName.PIC_NO + 1].ToString();
                        rebarData.Level = row[(int)EnumAllRebarTableColName.LEVEL + 1].ToString();
                        rebarData.Diameter = Convert.ToInt32(row[(int)EnumAllRebarTableColName.DIAMETER + 1].ToString());
                        rebarData.RebarPic = row[(int)EnumAllRebarTableColName.REBAR_PIC + 1].ToString();
                        rebarData.PicMessage = row[(int)EnumAllRebarTableColName.PIC_MESSAGE + 1].ToString();
                        rebarData.CornerMessage = row[(int)EnumAllRebarTableColName.CORNER_MESSAGE + 1].ToString();
                        rebarData.Length = row[(int)EnumAllRebarTableColName.LENGTH + 1].ToString();

                        if (GeneralClass.CfgData.DatabaseType == EnumDatabaseType.SQLITE)
                        {
                            rebarData.IsMulti = Convert.ToBoolean(row[(int)EnumAllRebarTableColName.ISMULTI + 1].ToString());
                        }
                        if (GeneralClass.CfgData.DatabaseType == EnumDatabaseType.MYSQL)
                        {
                            rebarData.IsMulti = Convert.ToInt16(row[(int)EnumAllRebarTableColName.ISMULTI + 1].ToString()) == 1 ? true : false;
                        }


                        rebarData.PieceNumUnitNum = row[(int)EnumAllRebarTableColName.PIECE_NUM_UNIT_NUM + 1].ToString();
                        rebarData.TotalPieceNum = Convert.ToInt32(row[(int)EnumAllRebarTableColName.TOTAL_PIECE_NUM + 1].ToString());
                        rebarData.TotalWeight = Convert.ToDouble(row[(int)EnumAllRebarTableColName.TOTAL_WEIGHT + 1].ToString());
                        rebarData.Description = row[(int)EnumAllRebarTableColName.DESCRIPTION + 1].ToString();
                        rebarData.SerialNum = Convert.ToInt32(row[(int)EnumAllRebarTableColName.SERIALNUM + 1].ToString());
                        //rebarData.IsOriginal = Convert.ToBoolean(row[(int)EnumAllRebarTableColName.ISORIGINAL + 1].ToString());
                        //rebarData.IfTao = Convert.ToBoolean(row[(int)EnumAllRebarTableColName.IFTAO + 1].ToString());
                        //rebarData.IfBend = Convert.ToBoolean(row[(int)EnumAllRebarTableColName.IFBEND + 1].ToString());
                        //rebarData.IfCut = Convert.ToBoolean(row[(int)EnumAllRebarTableColName.IFCUT + 1].ToString());
                        //rebarData.IfBendTwice = Convert.ToBoolean(row[(int)EnumAllRebarTableColName.IFBENDTWICE + 1].ToString());

                        if (rebarData.TotalPieceNum != 0)
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
        /// 用于多段钢筋中，拆分其边角信息
        /// 示例：
        /// 12000,套;3835,90;375,0
        /// 0,套;12000,套;12000*2,套;3400,90;375,0
        /// 375,90;11675,套;10725,90;375,0
        /// 12000,搭590;12000,搭590;11080,0
        /// 375,90;9155,套;12000,套;11900,丝
        /// 12000,搭590;12000*2,搭590;7520,0
        /// 特殊情况：马镫筋：230+220+350*2+230-8d*FC
        /// </summary>
        /// <param name="_cornerMsg">边角信息</param>
        /// <param name="_diameter">直径，缺省=1，因某些长度信息与直径相关</param>
        /// <returns></returns>
        public static List<GeneralMultiData> GetMultiData(string _cornerMsg, int _diameter = 1)
        {
            try
            {
                if (_cornerMsg == "")
                {
                    GeneralClass.interactivityData?.printlog(1, "cornerMsg为null");
                    return null;
                }

                List<GeneralMultiData> datalist = new List<GeneralMultiData>();
                GeneralMultiData _data = null;

                //string[] ssss = _cornerMsg.Split(';');
                string[] ssss = _cornerMsg.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);//去掉split后为空的值

                foreach (string sss in ssss)
                {
                    _data = new GeneralMultiData();

                    _data.diameter = _diameter;//直径

                    string[] ss = sss.Split(',');
                    _data.cornerMsg = sss;

                    if (ss.Length != 2) break;  //某些cornermessage不能解析的，直接退出 ，例如：1120*PI+408+15d&FG

                    //#region _data.headType
                    ////if (ss[1].Equals("0") || ss[1].Equals("0&D"))
                    //if (ss[1].IndexOf("0") == 0)
                    //{
                    //    _data.headType = EnumMultiHeadType.ORG;
                    //}
                    ////else if (ss[1].Equals("套"))//套
                    //else if (ss[1].IndexOf("套") == 0)//套
                    //{
                    //    _data.headType = EnumMultiHeadType.TAO_P;
                    //}
                    //else if (ss[1].IndexOf("套") > 0 && ss[1].IndexOf("反") == -1)//25套，变径套筒，不含“反”字
                    //{
                    //    _data.headType = EnumMultiHeadType.TAO_V;
                    //}
                    //else if (RegexIsNumeric(ss[1])) //90，是否为整数，整数则为弯曲角度
                    //{
                    //    _data.headType = EnumMultiHeadType.BEND;
                    //}
                    ////else if (ss[1].Equals("反套"))
                    //else if (ss[1].IndexOf("反套") == 0)
                    //{
                    //    _data.headType = EnumMultiHeadType.TAO_N;
                    //}
                    //else if (ss[1].IndexOf("反") >= 0)//反丝，包括变径反丝
                    //{
                    //    _data.headType = EnumMultiHeadType.SI_N;
                    //}
                    //else if (ss[1].IndexOf("丝") == 0)//特例：【7380,套;12000,丝&D】
                    //{
                    //    _data.headType = EnumMultiHeadType.SI_P;
                    //}
                    //else if (ss[1].IndexOf("搭") > -1)//含有“搭”
                    //{
                    //    _data.headType = EnumMultiHeadType.DA;
                    //}
                    //#endregion

                    string[] s = ss[0].Split('*');
                    _data.num = (s.Length > 1) ? Convert.ToInt32(s[1]) : 1;//
                    _data.length = s[0];//

                    datalist.Add(_data);
                }

                if (datalist.Count == 0)//特殊情况：马镫筋：300+280+750*2+300-8d&FC，这种情况，datalist的计数为0
                {
                    GeneralClass.interactivityData?.printlog(1, "GetMultiData error,CornerMsg==" + _cornerMsg);
                }
                return datalist;
            }
            catch (Exception ex) { MessageBox.Show("GetMultiData error:" + ex.Message); return null; }

        }


        //示例：12000\n
        //      12000*3\n
        //      7300
        /// <summary>
        /// 将多段通长筋的length数据进行拆解
        ///示例：12000\n
        ///      12000*3\n
        ///      7300
        ///缩尺1：7010
        ///          ~5370
        ///缩尺2：8020
        ///       4707~4608
        /// </summary>
        /// <returns></returns>
        public List<GeneralMultiLength> GetMultiLength(string strlength)
        {
            try
            {
                //缩尺符号~前面如果是'\n',则需要先去掉'\n'
                if (strlength.IndexOf('~') > -1 && strlength[strlength.IndexOf('~') - 1] == '\n')
                {
                    strlength = strlength.Remove(strlength.IndexOf('~') - 1, 1);//去掉~前面的那个'\n'                    
                }

                List<GeneralMultiLength> _lengthlist = new List<GeneralMultiLength>();
                GeneralMultiLength _length = new GeneralMultiLength();

                string[] sss = strlength.Split('\n');
                foreach (string ss in sss)
                {
                    _length = new GeneralMultiLength();
                    string[] s = ss.Split('*');
                    if (s.Length > 1)
                    {
                        //_length.length = Convert.ToInt32(s[0]);
                        _length.length = s[0];
                        _length.num = Convert.ToInt32(s[1]);
                    }
                    else
                    {
                        //_length.length = Convert.ToInt32(s[0]);
                        _length.length = s[0];
                        _length.num = 1;
                    }
                    _lengthlist.Add(_length);
                }
                return _lengthlist;
            }
            catch (Exception ex) { MessageBox.Show("GetMultiLength error:" + ex.Message); return null; }
        }
        /// <summary>
        /// 计算多段通长筋的总长度,兼顾考虑缩尺的情况，缩尺按照平均长度处理
        /// 示例：12000\n
        //          12000*3\n
        //          7300
        //缩尺1：7010
        //      ~5370
        //缩尺2：8020
        //       4707~4608
        /// </summary>
        /// <param name="strlength"></param>
        /// <returns></returns>
        public int GetMultiTotalLength(string strlength)
        {
            try
            {
                int _totallength = 0;
                int _max, _min = 0;

                List<GeneralMultiLength> _multilength = GetMultiLength(strlength);


                for (int i = 0; i < _multilength.Count; i++)
                {
                    if (_multilength[i].length.IndexOf('~') > -1)//如果含有~，为缩尺
                    {
                        string[] ttt = _multilength[i].length.Split('~');
                        _max = Convert.ToInt32(ttt[0]);
                        _min = Convert.ToInt32(ttt[1]);

                        _totallength += (int)(_max + _min) / 2 * _multilength[i].num;
                    }
                    else
                    {
                        _totallength += Convert.ToInt32(_multilength[i].length) * _multilength[i].num;
                    }

                }
                return _totallength;

                ////缩尺符号~前面如果是'\n',则需要先去掉'\n'
                //if (strlength.IndexOf('~') > -1 && strlength[strlength.IndexOf('~') - 1] == '\n')
                //{
                //    strlength = strlength.Remove(strlength.IndexOf('~') - 1, 1);//去掉~前面的那个'\n'                    
                //}

                //int _length = 0;

                //string[] sss = strlength.Split('\n');
                //foreach (string ss in sss)
                //{
                //    string[] s = ss.Split('*');
                //    if (s.Length > 1)
                //    {
                //        _length += Convert.ToInt32(s[0]) * Convert.ToInt32(s[1]);
                //    }
                //    else
                //    {
                //        _length += Convert.ToInt32(s[0]);

                //    }
                //}

                //return _length;

            }
            catch (Exception ex) { MessageBox.Show("GetMultiTotalLength error:" + ex.Message); return 0; }

        }
        /// <summary>
        /// 原材
        /// </summary>
        /// <param name="_rebarDataList"></param>
        /// <returns></returns>
        public GeneralDetailData Detail_Original(List<RebarData> _rebarDataList)
        {
            GeneralDetailData _data = new GeneralDetailData();

            foreach (RebarData _dd in _rebarDataList)
            {
                if (_dd.PicTypeNum == "70000") { continue; }//7000的图形为异类，排除掉
                if (_dd.PicTypeNum == "74201" && _dd.IsMulti) { continue; }
                if (_dd.IsOriginal && !_dd.IfCut && !_dd.IfBend && !_dd.IfTao && !_dd.IfBendTwice)//第一种情况，原本就是原材的，数量直接加
                {
                    _data.TotalPieceNum += _dd.TotalPieceNum;
                    _data.TotalLength += Convert.ToInt32(_dd.Length) * _dd.TotalPieceNum;    //总长度需要乘以数量
                    _data.TotalWeight += _dd.TotalWeight;   //数据源中的重量已经做了汇总
                }
                //搭接的钢筋
                if (_dd.IsMulti && !_dd.IfBend && !_dd.IfBendTwice && !_dd.IfTao)//第二种情况，多段中的原材，需分开处理，此种情况一般为搭接钢筋
                {
                    //示例：12000\n
                    //      12000*3\n
                    //      7300
                    string[] sss = _dd.Length.Split('\n');  //下料长度可能会出现多段的情况，此时需按照'\n'进行拆分字符串
                    int ori_length = 0;//累计计算原材长度
                    int all_length = GetMultiTotalLength(_dd.Length);//累计总长
                    foreach (string ss in sss)
                    {
                        string[] s = ss.Split('*');
                        if (s.Length > 1 && (s[0] == "12000" || s[0] == "9000")) //12000*3的情况
                        {
                            _data.TotalPieceNum += _dd.TotalPieceNum * Convert.ToInt32(s[1]);
                            _data.TotalLength += Convert.ToInt32(s[0]) * _dd.TotalPieceNum * Convert.ToInt32(s[1]);    //总长度需要乘以数量
                            ori_length += Convert.ToInt32(s[0]) * _dd.TotalPieceNum * Convert.ToInt32(s[1]);
                            //all_length += Convert.ToInt32(s[0]) * _dd.TotalPieceNum * Convert.ToInt32(s[1]);
                        }
                        else if (s.Length == 1 && (s[0] == "12000" || s[0] == "9000")) //12000的情况
                        {
                            _data.TotalPieceNum += _dd.TotalPieceNum;
                            _data.TotalLength += Convert.ToInt32(s[0]) * _dd.TotalPieceNum;    //总长度需要乘以数量
                            ori_length += Convert.ToInt32(s[0]) * _dd.TotalPieceNum;
                            //all_length += Convert.ToInt32(s[0]) * _dd.TotalPieceNum;
                        }
                        else//7300的情况
                        {
                            //all_length += Convert.ToInt32(s[0]) * _dd.TotalPieceNum;
                        }
                    }
                    _data.TotalWeight += _dd.TotalWeight * ori_length / all_length;   //数据源中的重量已经做了汇总，此处需做分割，只统计原材部分的重量

                }
            }

            return _data;
        }

        /// <summary>
        /// 原材仅套丝
        /// </summary>
        /// <param name="_rebarDataList"></param>
        /// <returns></returns>
        public GeneralDetailData Detail_OnlyTao(List<RebarData> _rebarDataList)
        {
            try
            {
                GeneralDetailData _data = new GeneralDetailData();

                List<GeneralMultiData> _multidata = new List<GeneralMultiData>();

                foreach (RebarData _dd in _rebarDataList)
                {
                    if (_dd.PicTypeNum == "70000") { continue; }//7000的图形为异类，排除掉
                    if (_dd.PicTypeNum == "74201" && _dd.IsMulti) { continue; }

                    if (_dd.IsOriginal && _dd.IfTao && !_dd.IfCut && !_dd.IfBend && !_dd.IfBendTwice)//第一种情况，原本就是原材,仅需套丝
                    {
                        _data.TotalPieceNum += _dd.TotalPieceNum;
                        _data.TotalLength += Convert.ToInt32(_dd.Length) * _dd.TotalPieceNum;    //总长度需要乘以数量
                        _data.TotalWeight += _dd.TotalWeight;   //数据源中的重量已经做了汇总

                        //示例：0,套;12000,丝
                        //string[] ttt = _dd.CornerMessage.Split(';');
                        string[] ttt = _dd.CornerMessage.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);//去掉空的

                        foreach (string tt in ttt)
                        {
                            string[] t = tt.Split(',');
                            if (t[1].Equals("反套"))//“反套”
                            {
                                _data.TaosiNum += 2;
                                _data.TaotongNum += 1;
                                _data.TaotongNum_N += 1;
                            }
                            else if (t[1].Equals("反丝"))//“反丝”
                            {
                                _data.TaosiNum += 1;
                            }
                            else if (t[1].IndexOf("套") > 0)   //变径套筒，比如“25套”
                            {
                                _data.TaosiNum += 2;
                                _data.TaotongNum_V += 1;
                            }
                            else if (t[1].IndexOf("套") == 0)//“套”
                            {
                                _data.TaosiNum += 2;
                                _data.TaotongNum += 1;
                                _data.TaotongNum_P += 1;
                            }
                            else if (t[1].IndexOf("丝") == 0)//“丝”
                            {
                                _data.TaosiNum += 1;
                            }
                        }
                    }
                    if (_dd.IsMulti && _dd.IfTao && !_dd.IfBend && !_dd.IfBendTwice)//第二种情况，多段中的原材，需分开处理，
                    {
                        //示例：2700,套;12000*2,套;12000,丝
                        //      0,25套; 7900,套; 12000,丝
                        //      12000,套;2265,0
                        _multidata = GetMultiData(_dd.CornerMessage);
                        int tao_length = 0;//累计计算套丝长度
                        int all_length = GetMultiTotalLength(_dd.Length);//累计总长

                        foreach (GeneralMultiData _mm in _multidata)
                        {
                            if ((_mm.length == "12000" || _mm.length == "9000")
                                && _mm.headType != EnumMultiHeadType.DA)//原材，不为搭接端头
                            {
                                _data.TotalPieceNum += _dd.TotalPieceNum * _mm.num;
                                _data.TotalLength += Convert.ToInt32(_mm.length) * _dd.TotalPieceNum * _mm.num;    //总长度需要乘以数量
                                                                                                                   //_data.TotalWeight += _dd.TotalWeight;   //数据源中的重量已经做了汇总
                                tao_length += Convert.ToInt32(_mm.length) * _dd.TotalPieceNum * _mm.num;
                                //all_length += _mm.length * _dd.TotalPieceNum * _mm.num;

                                if (_mm.headType == EnumMultiHeadType.TAO_P)
                                {
                                    _data.TaosiNum += 2;
                                    _data.TaotongNum += 1;
                                    _data.TaotongNum_P += 1;
                                }
                                else if (_mm.headType == EnumMultiHeadType.TAO_N)
                                {
                                    _data.TaosiNum += 2;
                                    _data.TaotongNum += 1;
                                    _data.TaotongNum_N += 1;
                                }
                                else if (_mm.headType == EnumMultiHeadType.TAO_V)
                                {
                                    _data.TaosiNum += 2;
                                    _data.TaotongNum += 1;
                                    _data.TaotongNum_V += 1;
                                }
                                else if (_mm.headType == EnumMultiHeadType.SI_P || _mm.headType == EnumMultiHeadType.SI_N)
                                {
                                    _data.TaosiNum += 1;
                                }
                            }
                            //all_length += _mm.length * _dd.TotalPieceNum * _mm.num;
                            _data.TotalWeight += _dd.TotalWeight * tao_length / all_length;   //数据源中的重量已经做了汇总，此处需做分割，只统计原材部分的重量

                        }
                    }
                }

                return _data;
            }
            catch (Exception ex) { MessageBox.Show("Detail_OnlyTao error:" + ex.Message); return null; }

        }

        public GeneralDetailData Detail_OnlyBend(List<RebarData> _rebarDataList)
        {
            try
            {
                //375,90;11675,套;7725,丝	            12000\n 7730
                GeneralDetailData _data = new GeneralDetailData();

                List<GeneralMultiData> _multidata = new List<GeneralMultiData>();

                foreach (RebarData _dd in _rebarDataList)
                {
                    if (_dd.PicTypeNum == "70000") { continue; }//70000的图形为异类，排除掉
                    if (_dd.PicTypeNum == "74201" && _dd.IsMulti) { continue; }

                    if (_dd.IsOriginal && !_dd.IfTao && !_dd.IfCut && (_dd.IfBend || _dd.IfBendTwice))//第一种情况，原本就是原材,仅需弯曲
                    {
                        _data.TotalPieceNum += _dd.TotalPieceNum;
                        _data.TotalLength += Convert.ToInt32(_dd.Length) * _dd.TotalPieceNum;    //总长度需要乘以数量
                        _data.TotalWeight += _dd.TotalWeight;   //数据源中的重量已经做了汇总

                        _data.BendNum += 1;
                    }
                    if (_dd.IsMulti && (_dd.IfBend || _dd.IfBendTwice))//第二种情况，多段中的原材，需分开处理，
                    {
                        int bend_length = 0;//累计计算套丝长度
                        int all_length = GetMultiTotalLength(_dd.Length);//累计总长

                        //示例：
                        _multidata = GetMultiData(_dd.CornerMessage);

                        for (int i = 0; i < _multidata.Count - 1; i++)
                        {
                            int l_i = Convert.ToInt32(_multidata[i].length);
                            int l_i1 = Convert.ToInt32(_multidata[i + 1].length);
                            if (_multidata[i].headType == EnumMultiHeadType.BEND
                                && ((l_i + l_i1) <= 12100 && (l_i + l_i1) > 12000) ||
                                ((l_i + l_i1) <= 9100 && (l_i + l_i1) > 9000))
                            {
                                _data.TotalPieceNum += _dd.TotalPieceNum;
                                _data.TotalLength += (l_i + l_i1) * _dd.TotalPieceNum;    //总长度需要乘以数量
                                                                                          //_data.TotalWeight += _dd.TotalWeight   //数据源中的重量已经做了汇总
                                bend_length += (l_i + l_i1);

                                _data.BendNum += 1;
                            }
                        }
                        _data.TotalWeight += _dd.TotalWeight * bend_length / all_length;   //数据源中的重量已经做了汇总，此处需做分割，只统计原材部分的重量


                    }
                }

                return _data;
            }
            catch (Exception ex) { MessageBox.Show("Detail_OnlyBend error:" + ex.Message); return null; }

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

        /// <summary>
        /// 分解通长多段筋
        ///示例：   9000               9000,搭390;9000*2,搭390;8070,0
        ///         9000*2
        ///         8070
        ///         
        ///示例：  12000               0,套;12000,套;2550,0
        ///         2550
        ///         
        ///示例：  9480              375,90;9155,套;12000,套;11900,丝
        ///         12000
        ///         11900
        ///         
        ///     375,90;11675,套;10725,90;375,0
        ///     12000,搭590;12000,搭590;11080,0
        ///     12000,套;3835,90;375,0
        ///     375,90;9155,套;12000,套;11900,丝
        ///     12000,搭590;12000*2,搭590;7520,0
        ///     0,套;12000,套;12000*2,套;3400,90;375,0
        /// </summary>
        /// <param name="_data"></param>
        public List<RebarData> SplitMultiRebar(RebarData _data)
        {
            try
            {
                List<RebarData> _list = new List<RebarData>();

                List<GeneralMultiData> _MultiData = GetMultiData(_data.CornerMessage);//拆解corner信息
                List<GeneralMultiLength> _MultiLength = GetMultiLength(_data.Length);//拆解length信息

                if (_MultiData == null)
                {
                    GeneralClass.interactivityData?.printlog(1, "cornermsg数据有异常，GetMultiData拆解失败");
                    return null;
                }
                if (_MultiLength == null)
                {
                    GeneralClass.interactivityData?.printlog(1, "Length数据有异常，GetMultiLength拆解失败");
                    return null;
                }
                List<List<GeneralMultiData>> m_listgroup = new List<List<GeneralMultiData>>();//只要是有套丝的就要分开成多根钢筋
                List<GeneralMultiData> m_list = new List<GeneralMultiData>();

                //先拆分多段数据
                for (int i = 0; i < _MultiData.Count; i++)
                {
                    //特例：【0,套;12000,套;12000,单&D】
                    //特例：【0,套;12000,套;12000,套;5550,0&D】，最后一段识别为none
                    //if (_MultiData[i].headType != EnumMultiHeadType.NONE
                    //    && _MultiData[i].headType != EnumMultiHeadType.BEND
                    //    && _MultiData[i].length != "0")
                    //特例：130,90;1010,套;7340,-3;950R950T0.44,-7;900,0
                    if (_MultiData[i].headType != EnumMultiHeadType.BEND &&
                        _MultiData[i].headType != EnumMultiHeadType.ARC &&
                        _MultiData[i].length != "0")//端头类型不是弯曲,不是圆弧，且长度不为0，一般是套丝、搭接等，需要拆分，20241115加上圆弧判断
                    {
                        _MultiData[i].cornerMsg = _MultiData[i].msg_first.Split('*')[0]+","+_MultiData[i].msg_second;//20240909修改，去掉【6000*2,90】后面的【*2】
                        m_list.Add(_MultiData[i]);
                        m_listgroup.Add(m_list);

                        m_list = new List<GeneralMultiData>();//清空重新添加
                    }
                    else//其他情况，则认为还在一根钢筋里面
                    {
                        m_list.Add(_MultiData[i]);      //【375,90;9155,套;】或者【0,套;12000,套;】的情况
                    }
                }

                //添加一个0长度的端头
                GeneralMultiData temp = new GeneralMultiData();
                if (m_listgroup.Count != 0)        //对于端头为套丝的，分割后，要加个【0，丝】或者【0，反丝】端头进去
                {
                    for (int j = 1; j < m_listgroup.Count; j++)//从1开始
                    {
                        if (m_listgroup[j - 1].Last().headType == EnumMultiHeadType.TAO_P
                            || m_listgroup[j - 1].Last().headType == EnumMultiHeadType.TAO_V)//前一个是正套或者变径套，则添加正丝端头进去
                        {
                            temp.length = "0";
                            temp.num = m_listgroup[j].First().num;
                            temp.cornerMsg = "0,丝";
                            //temp.headType = EnumMultiHeadType.SI_P;

                            m_listgroup[j].Insert(0, temp);//插入到第一个
                        }
                        if (m_listgroup[j - 1].Last().headType == EnumMultiHeadType.TAO_N)//反丝
                        {
                            temp.length = "0";
                            temp.num = m_listgroup[j].First().num;
                            temp.cornerMsg = "0,反丝";
                            //temp.headType = EnumMultiHeadType.SI_N;

                            m_listgroup[j].Insert(0, temp);//插入到第一个
                        }
                    }
                }

                //比对_MultiLength和m_listgroup，如果数量对不上，需要报警提示
                if (_MultiLength.Count != m_listgroup.Count)
                {
                    GeneralClass.interactivityData?.printlog(1, "多段通长筋拆解有误，length数量与cornermsg数量不一致");
                    return null;
                }

                //组合新的钢筋
                RebarData _tempdata = new RebarData();
                string ss = "";
                for (int k = 0; k < m_listgroup.Count; k++)
                {
                    _tempdata = new RebarData();
                    //_tempdata = _data;      //先复制一份
                    _tempdata.Copy(_data);//先复制一份，主要是复制其各种信息

                    ss = "";
                    for (int h = 0; h < m_listgroup[k].Count; h++)
                    {
                        ss += m_listgroup[k][h].cornerMsg + ";";//多个multidata的cornermsg拼接起来
                    }
                    _tempdata.CornerMessage = ss;
                    _tempdata.Length = _MultiLength[k].length;
                    //_tempdata.PieceNumUnitNum = (_MultiLength[k].num == 1) ? _data.TotalPieceNum.ToString() : (_MultiLength[k].num.ToString() + "*" + _data.TotalPieceNum.ToString());
                    string[] _num = _data.PieceNumUnitNum.Split('*');
                    if (_num.Length > 1)
                    {
                        //_tempdata.PieceNumUnitNum = _MultiLength[k].num.ToString() + "*" + (Convert.ToInt32(_num[0]) * Convert.ToInt32(_num[1])).ToString();
                        _tempdata.PieceNumUnitNum = (_MultiLength[k].num * Convert.ToInt32(_num[0])).ToString() + "*" + Convert.ToInt32(_num[1]).ToString();
                    }
                    else
                    {
                        //_tempdata.PieceNumUnitNum = _MultiLength[k].num.ToString() + "*" + _num[0].ToString();
                        _tempdata.PieceNumUnitNum = (_MultiLength[k].num * Convert.ToInt32(_num[0])).ToString();
                    }
                    _tempdata.TotalPieceNum = _MultiLength[k].num * _data.TotalPieceNum;
                    double tt;
                    double.TryParse(_MultiLength[k].length, out tt);//可能有缩尺
                    _tempdata.TotalWeight = _data.TotalWeight * tt * _MultiLength[k].num / (double)GetMultiTotalLength(_data.Length);//20240813，解决bug，注意要乘以数量
                    _tempdata.TableName=_data.TableName;//料表名
                    _tempdata.TableSheetName=_data.TableSheetName;//料表sheet名

                    //ModifyRebarData(ref _tempdata);//修改其他的项
                    ModifyRebarPicNum(ref _tempdata);//修改图形编号

                    _list.Add(_tempdata);
                }
                return _list;
            }
            catch (Exception ex) { MessageBox.Show("SplitMultiRebar error:" + ex.Message); return null; }

        }
        /// <summary>
        /// 分解缩尺筋,注意：分解缩尺筋最好是在分解多段之后再处理，此处分解缩尺筋仅考虑单个数据【7010~5370】这种
        ///缩尺1：7010
        ///      ~5370
        ///缩尺2：8020
        ///       4707~4608
        ///       
        /// 缩尺3：1420~870,25;750,0
        /// 750,35;1420~870,0
        /// 
        /// 缩尺4： 2060
        ///         ~2660                        10d,135;300~600,90;640,90;300~600,90;640,135;10d,0&G               
        ///         
        /// 缩尺的备注信息：△64mm,总长13230@210，注意需要用到此处的△64mm，此即为缩尺间距
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        public List<RebarData> SplitSuoChiRebar(RebarData _data)
        {
            try
            {
                if (_data.Length.IndexOf('~') <= -1)
                {
                    GeneralClass.interactivityData?.printlog(1, "缩尺数据格式不正确，请检查！");
                    return null;
                }

                int _num = 0;//根数
                int _piece = 0;//件数
                string[] _numStr = _data.PieceNumUnitNum.Split('*');//例如：根数*件数=5*2，取5为根数，2为件数
                _num = Convert.ToInt32(_numStr[0]);
                _piece = (_numStr.Length > 1) ? Convert.ToInt32(_numStr[1]) : 1;

                ////示例：△64mm，截取缩尺间距的数值
                //int _ss = _data.Description.IndexOf('△');
                //int _ee = _data.Description.IndexOf('m');
                //int _offset = Convert.ToInt32(_data.Description.Substring(_ss, _ee));

                List<RebarData> _list = new List<RebarData>();
                RebarData _newdata = new RebarData();

                ////注意：边角信息里面的缩尺数值跟下料长度中的缩尺数值是不一样的，要分开计算
                List<int> _lengthlist = SuoChiDeal(_data.Length, _num);

                List<string> _cornerlist = SuoChiDealCornerMsg(_data.CornerMessage, _num);

                for (int i = 0; i < _num; i++)
                {
                    _newdata = new RebarData();
                    _newdata.Copy(_data);//先复制原本的rebardata
                    _newdata.Length = _lengthlist[i].ToString();
                    _newdata.CornerMessage = _cornerlist[i];
                    _newdata.PieceNumUnitNum = _piece != 1 ? ("1*" + _piece) : "1";
                    _newdata.TotalPieceNum = 1 * _piece;

                    _newdata.TotalWeight = _data.TotalWeight * (double)_lengthlist[i] / (double)_lengthlist.Sum();

                    ModifyRebarPicNum(ref _newdata);//修改图形编号，20240902修改bug

                    _list.Add(_newdata);
                }


                return _list;
            }
            catch (Exception ex) { MessageBox.Show("SplitSuoChiRebar error:" + ex.Message); return null; }

        }
        /// <summary>
        /// 处理缩尺信息，
        /// </summary>
        /// <param name="_suochi">缩尺长度信息，例如：1420~870</param>
        /// <param name="_num">总共多少根缩尺筋，间距数量-1</param>
        /// <returns>返回处理好的长度数值的list</returns>
        public List<int> SuoChiDeal(string _suochi, int _num)
        {
            try
            {
                List<int> _retlist = new List<int>();

                string[] _length = _suochi.Split('~');
                int _startlength = Convert.ToInt32(_length[0]);//缩尺的起始长度，不一定最大，有可能是最小
                int _endlength = Convert.ToInt32(_length[1]);//缩尺的终止长度，不一定最小，有可能是最大

                int _maxlength = Math.Max(_startlength, _endlength);
                int _minlength = Math.Min(_startlength, _endlength);

                for (int i = 0; i < _num; i++)//从最短的开始
                {
                    if (_num == 1)
                    {
                        GeneralClass.interactivityData?.printlog(1, "缩尺信息:" + _suochi + "，数量:" + _num + "，数据不正确，请检查！");
                    }
                    int temp = _minlength + i * (_maxlength - _minlength) / ((_num - 1) != 0 ? (_num - 1) : 1);
                    _retlist.Add(temp);
                }

                return _retlist;
            }
            catch (Exception ex) { MessageBox.Show("SuoChiDeal error:" + ex.Message); return null; }
        }
        /// <summary>
        /// 处理缩尺的边角信息，返回新的边角信息序列,从短到长排列
        /// </summary>
        /// <param name="_cornerMsg"></param>
        /// <param name="_num"></param>
        /// <returns></returns>
        public List<string> SuoChiDealCornerMsg(string _cornerMsg, int _num)
        {
            try
            {

                List<string> _retlist = new List<string>();

                List<GeneralMultiData> _MultiData = GetMultiData(_cornerMsg);//拆解corner信息

                for (int i = 0; i < _num; i++)
                {
                    string _newCornerMsg = "";
                    foreach (var _multi in _MultiData)
                    {
                        if (_multi.cornerMsg.IndexOf('~') > -1)
                        {
                            string _msg = _multi.cornerMsg.Split(',')[0];
                            List<int> temp = SuoChiDeal(_msg, _num);

                            string _newmulti = temp[i] + "," + _multi.cornerMsg.Split(',')[1] + ";";
                            _newCornerMsg += _newmulti;
                        }
                        else
                        {
                            _newCornerMsg += _multi.cornerMsg + ";";
                        }
                    }
                    _retlist.Add(_newCornerMsg);
                }

                return _retlist;

            }
            catch (Exception ex) { MessageBox.Show("SuoChiDealCornerMsg error:" + ex.Message); return null; }

        }
    }
}
