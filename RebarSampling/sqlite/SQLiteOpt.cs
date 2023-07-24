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

namespace RebarSampling
{

    /// <summary>
    /// SQLiteOpt类，用于对sqlite数据库文件的复杂操作
    /// </summary>
    public class SQLiteOpt : SQLiteHelper
    {

        public SQLiteOpt()
        {
            //ExistRebarPicTypeList.Clear();
        }

        ///// <summary>
        ///// 读excel时，存储已存在的rebarType，放入list中
        ///// </summary>
        //public List<EnumAllRebarPicType> ExistRebarPicTypeList = new List<EnumAllRebarPicType>();



        /// <summary>
        /// 创建新表，并根据钢筋表要求添加对应的列
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="_data"></param>
        public void CreatRebarTable(string tableName)
        {
            try
            {
                CreatTable(tableName);
                AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PROJECT_NAME], DbType.String.ToString());
                AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.MAIN_ASSEMBLY_NAME], DbType.String.ToString());
                AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.ELEMENT_NAME], DbType.String.ToString());
                AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TYPE_NAME], DbType.String.ToString());
                AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.LEVEL], DbType.String.ToString());
                AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.DIAMETER], DbType.Int32.ToString());
                AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.REBAR_PIC], DbType.String.ToString());
                AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PIC_MESSAGE], DbType.String.ToString());
                AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.CORNER_MESSAGE], DbType.String.ToString());
                AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.LENGTH], DbType.String.ToString());
                AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.ISMULTI], DbType.Boolean.ToString());
                AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PIECE_NUM_UNIT_NUM], DbType.String.ToString());
                AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TOTAL_PIECE_NUM], DbType.Int32.ToString());
                AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TOTAL_WEIGHT], DbType.Double.ToString());
                AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.DESCRIPTION], DbType.String.ToString());
                AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.SERIALNUM], DbType.Int32.ToString());
                AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.ISORIGINAL], DbType.Boolean.ToString());
                AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFTAO], DbType.Boolean.ToString());
                AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFBEND], DbType.Boolean.ToString());
                AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFCUT], DbType.Boolean.ToString());
                AddColumn(tableName, GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.IFBENDTWICE], DbType.Boolean.ToString());


                //        AddColumn(tableName, GeneralClass.RebarColumnName[i], rebarData.ElementName.GetType().Name);
            }
            catch (Exception) { throw; }

        }




        /// <summary>
        /// 插入一行数据，注意需要整理rowdata的list
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="rowdata"></param>
        public string InsertRowData(string tableName, RebarData rowdata)
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

                            for (int i = 0; i < (int)EnumAllRebarTableColName.maxRebarColNum; i++)
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
                            sqlstr += "'" + rowdata.TypeNum + "'" + ",";
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
        public DataTable FindAllPic(string tablename, List<string> colNameList, EnumRebarType _rebarType, EnumRebarPicType _rebarPicType)
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

                if (_rebarType == EnumRebarType.BANG)//棒材
                {
                    sqlstr += "(" + GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.DIAMETER] + " between 14 AND 40 )";  //棒材钢筋
                }
                else
                {
                    sqlstr += "(" + GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.DIAMETER] + " between 6 AND 12 )";   //线材钢筋
                }
                sqlstr += "AND";

                string tt = _rebarPicType.ToString();
                sqlstr += "(" + GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TYPE_NAME] + " = '" + tt.Substring(2, tt.Length - 2) + "')";


                DataTable dt = ExecuteQuery(sqlstr, null);

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
        public void InitDB(string tablename)
        {
            try
            {

                if (!IsTableEXIST(tablename))    //如果table不存在，则创建新表
                {
                    //CreatTable(tableName);
                    CreatRebarTable(tablename);
                }
                DeleteTable(tablename);         //清空表
                DeleteTableSequence(tablename); //清空表的自增列序号
                //GeneralClass.ExistRebarPicTypeList.Clear();//清空已存在的钢筋图形list

            }
            catch (Exception ex) { MessageBox.Show("InitDB error:" + ex.Message); }

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

            List<DataTable> _dtlist = GeneralClass.readEXCEL?.GetAllSheet(filename);

            for (int i = 0; i < _dtlist.Count; i++)
            {
                DataTable dt = _dtlist[i];
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

                    rebarData.ProjectName = System.IO.Path.GetFileNameWithoutExtension(filename); //获取excel文件名作为根节点名称,不带后缀名;//将filename赋予钢筋的项目名称
                    rebarData.MainAssemblyName = sheetname;//将sheetname赋予钢筋的主构件名称

                    rebarData.ElementName = dr[0].ToString();
                    rebarData.TypeNum = dr[startindex].ToString();

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


                    ModifyRebarData(ref rebarData);

                    //InsertRowData(tablename, rebarData);
                    sqls.Add(InsertRowData(tablename, rebarData));
                }

            }
            ExecuteNonQuery(sqls);//批量存入


        }




        /// <summary>
        /// 详细处理
        /// </summary>
        /// <param name="_data"></param>
        private void ModifyRebarData(ref RebarData _data)
        {
            //详细处理
            _data.IsOriginal = (_data.Length == "9000" || _data.Length == "12000") ? true : false;//标注是否为原材，长度为9000或者12000，为原材

            _data.IfTao = (_data.CornerMessage.IndexOf("套") > -1
                || _data.CornerMessage.IndexOf("丝") > -1
                || _data.CornerMessage.IndexOf("反") > -1) ? true : false;//如果边角结构信息中含有“套”或者“丝”或者“反”，则认为其需要套丝

            _data.IfBend = (_data.TypeNum.Substring(0, 1) == "1") ? false : true;//如果图形编号是1开头的，则不用弯，其他都需要弯

            _data.IfCut = (_data.Length == "9000" || _data.Length == "12000") ? false : true;//标注是否需要切断，原材以外的都需要切断

            _data.IfBendTwice = (_data.TypeNum.Substring(0, 1) == "1"
                || _data.TypeNum.Substring(0, 1) == "2"
                || _data.TypeNum.Substring(0, 1) == "3") ? false : true;//1、2、3开头的图形编号为需要弯折两次以下的，其他的需要弯折2次以上

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
                        if (_ddd.TypeNum == _type.ToString().Substring(2, 5) &&(_ddd.TotalPieceNum!=0)&&
                           (_iffilter ? (_ddd.IfCut == _ifcut && _ddd.IfBend == _ifbend && _ddd.IfTao == _iftao) : true))
                        {
                            _detaildata.TotalPieceNum += _ddd.TotalPieceNum;
                            _detaildata.TotalWeight += _ddd.TotalWeight;

                            #region 处理长度数值例外的情况,临时做法，如果长度数值有换行符号，则给0
                            //_detaildata.TotalLength += Convert.ToInt32(_ddd.Length);
                            int _llll = 0;
                            if(int.TryParse(_ddd.Length,out _llll))
                            {
                                _detaildata.TotalLength += _llll;
                            }
                            #endregion

                            //解析套丝数据，
                            //示例：0,套;2250,-90;300,0&D
                            //      0,32套; 4000,丝
                            //string[] sss = _ddd.CornerMessage.Split(';');
                            string[] sss = _ddd.CornerMessage.Split(new char[] { ';'},StringSplitOptions.RemoveEmptyEntries);

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
                            if (_ddd.TypeNum.Substring(0, 1) == "2")
                            {
                                _detaildata.BendNum += 1;
                            }
                            else if (_ddd.TypeNum.Substring(0, 1) == "3")
                            {
                                _detaildata.BendNum += 2;
                            }
                            else if (_ddd.TypeNum.Substring(0, 1) == "4")
                            {
                                _detaildata.BendNum += 3;
                            }
                            else if (_ddd.TypeNum.Substring(0, 1) == "5")
                            {
                                _detaildata.BendNum += 4;
                            }
                            else if (_ddd.TypeNum.Substring(0, 1) == "6")
                            {
                                _detaildata.BendNum += 6;//不定，平均等于6吧
                            }
                            else if (_ddd.TypeNum.Substring(0, 1) == "7")
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
                List<KeyValuePair<EnumRebarPicType, GeneralDetailData>>[] _allList = new List<KeyValuePair<EnumRebarPicType, GeneralDetailData>>[(int)EnumDetailTableRowName.maxRowNum];

                //List<GeneralDetailData> _list = new List<GeneralDetailData>();
                List<KeyValuePair<EnumRebarPicType, GeneralDetailData>> _list = new List<KeyValuePair<EnumRebarPicType, GeneralDetailData>>();
                string _level = "";
                int _diameter = 0;

                for (int i = (int)EnumDetailTableRowName.XIAN_A6; i < (int)EnumDetailTableRowName.maxRowNum; i++)
                {
                    //_list.Clear();//注意要先清掉
                    string ss = ((EnumDetailTableRowName)i).ToString().Split('_')[1];//例如：BANG_C14，截取后一段
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
                object[,,] _alldata = new object[(int)EnumDetailTableColName.ONLY_CUT, (int)EnumDetailTableRowName.maxRowNum, (int)EnumDetailItem.maxItemNum];//先处理三个原材的

                GeneralDetailData[] _data = new GeneralDetailData[(int)EnumDetailTableColName.ONLY_CUT];//先处理三个原材的

                string _level = "";
                int _diameter = 0;

                for (int i = (int)EnumDetailTableRowName.XIAN_A6; i < (int)EnumDetailTableRowName.maxRowNum; i++)
                {
                    string ss = ((EnumDetailTableRowName)i).ToString().Split('_')[1];//例如：BANG_C14，截取后一段
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

        public List<GroupbyLengthDatalist> GetAllListByLength(List<RebarData> _rebardatalist)
        {
            List<GroupbyLengthDatalist> returnlist = new List<GroupbyLengthDatalist>();

            returnlist = _rebardatalist.GroupBy(x=>x.Length).Select(
                y=>new GroupbyLengthDatalist { 
                    _length = y.Key,
                    _totalnum = y.Sum(item=>item.TotalPieceNum),
                    _totalweight = y.Sum(item =>item.TotalWeight),
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
            try
            {
                List<EnumRebarPicType> _typelist = new List<EnumRebarPicType>();

                bool _exist = false;

                foreach (RebarData _data in _rebardatalist)
                {
                    _exist = false;
                    if (_typelist.Count != 0)
                    {
                        foreach (EnumRebarPicType _type in _typelist)
                        {
                            string str = _type.ToString();
                            if (_data.TypeNum == str.Substring(2, str.Length - 2))
                            {
                                _exist = true;
                            }
                        }

                        if (!_exist)
                        {
                            string s = "T_" + _data.TypeNum;
                            EnumRebarPicType _enum = (EnumRebarPicType)Enum.Parse(typeof(EnumRebarPicType), s, true);//从string转化为enum
                            _typelist.Add(_enum);
                        }
                    }
                    else
                    {
                        string s = "T_" + _data.TypeNum;
                        EnumRebarPicType _enum = (EnumRebarPicType)Enum.Parse(typeof(EnumRebarPicType), s, true);//从string转化为enum
                        _typelist.Add(_enum);
                    }

                }

                return _typelist;


            }
            catch (Exception ex) { MessageBox.Show("GetMultiData error:" + ex.Message); return null; }

        }
        /// <summary>
        /// 根据钢筋总表名称、项目名称、主构件名称，得到所有的构件包
        /// </summary>
        /// <param name="_tableName">钢筋总表名称，allsheet</param>
        /// <param name="_projectName">项目名称，为excel文件名</param>
        /// <param name="_assemblyName">主构件名称，为excel中的sheet名</param>
        /// <returns>返回构件包list</returns>
        public List<ElementData> GetAllElementList(string _tableName, string _projectName, string _assemblyName)
        {
            List<ElementData> _elementList = new List<ElementData>();
            ElementData _element = null;

            List<RebarData> _rebarlist = GetAllRebarList(_tableName);

            int _index = 0;

            foreach (RebarData item in _rebarlist)
            {
                if (item.ProjectName != _projectName || item.MainAssemblyName != _assemblyName)//筛选项目名和主构件名匹配的
                {
                    _index = 0;
                    continue;
                }
                else
                {
                    _index++;
                }

                //int _index = _rebarlist.IndexOf(item);
                
                //首行先创建构件
                if (_index == 1)
                {
                    _element = new ElementData();
                    _element.elementName = (item.ElementName != "") ? item.ElementName : "default";//极少数情况，会出现首行没有构件名，用缺省名代替                                 
                    _element.rebarlist.Add(item);
                }

                int curIndex= _rebarlist.IndexOf(item);
                if (_index != 1 && item.ElementName != "" && item.ElementName != _rebarlist[curIndex - 1].ElementName)//构件名不为空，且跟上一个元素的构件名不一样，则新建构件
                {
                    if (_element != null)
                    {
                        _elementList.Add(_element);
                    }
                    _element = new ElementData();
                    _element.elementName = item.ElementName;
                    _element.rebarlist.Add(item);
                }
                else
                {
                    _element.rebarlist.Add(item);//子构件名称为空的，默认为跟前一个不为空的是同一个构件的
                }

            }




            return _elementList;
        }
        /// <summary>
        /// 从数据库中取出所有的钢筋数据
        /// </summary>
        /// <param name="_tableName"></param>
        /// <returns>返回所有的钢筋list</returns>
        public List<RebarData> GetAllRebarList(string _tableName)
        {
            List<RebarData> allRebarList = new List<RebarData>();   //钢筋总表的list
            RebarData rebarData = new RebarData();              //新建一个对象

            DataTable dt = GetDataTable(_tableName);

            if (dt != null)
            {
                //从db中取出钢筋总表list
                foreach (DataRow row in dt.Rows)
                {
                    rebarData = new RebarData();
                    rebarData.ProjectName = row[(int)EnumAllRebarTableColName.PROJECT_NAME + 1].ToString();
                    rebarData.MainAssemblyName = row[(int)EnumAllRebarTableColName.MAIN_ASSEMBLY_NAME + 1].ToString();
                    rebarData.ElementName = row[(int)EnumAllRebarTableColName.ELEMENT_NAME + 1].ToString();
                    rebarData.TypeNum = row[(int)EnumAllRebarTableColName.TYPE_NAME + 1].ToString();
                    rebarData.Level = row[(int)EnumAllRebarTableColName.LEVEL + 1].ToString();
                    rebarData.Diameter = Convert.ToInt32(row[(int)EnumAllRebarTableColName.DIAMETER + 1].ToString());
                    rebarData.RebarPic = row[(int)EnumAllRebarTableColName.REBAR_PIC + 1].ToString();
                    rebarData.PicMessage = row[(int)EnumAllRebarTableColName.PIC_MESSAGE + 1].ToString();
                    rebarData.CornerMessage = row[(int)EnumAllRebarTableColName.CORNER_MESSAGE + 1].ToString();
                    rebarData.Length = row[(int)EnumAllRebarTableColName.LENGTH + 1].ToString();
                    rebarData.IsMulti = Convert.ToBoolean(row[(int)EnumAllRebarTableColName.ISMULTI + 1].ToString());
                    rebarData.PieceNumUnitNum = row[(int)EnumAllRebarTableColName.PIECE_NUM_UNIT_NUM + 1].ToString();
                    rebarData.TotalPieceNum = Convert.ToInt32(row[(int)EnumAllRebarTableColName.TOTAL_PIECE_NUM + 1].ToString());
                    rebarData.TotalWeight = Convert.ToDouble(row[(int)EnumAllRebarTableColName.TOTAL_WEIGHT + 1].ToString());
                    rebarData.Description = row[(int)EnumAllRebarTableColName.DESCRIPTION + 1].ToString();
                    rebarData.SerialNum = Convert.ToInt32(row[(int)EnumAllRebarTableColName.SERIALNUM + 1].ToString());
                    rebarData.IsOriginal = Convert.ToBoolean(row[(int)EnumAllRebarTableColName.ISORIGINAL + 1].ToString());
                    rebarData.IfTao = Convert.ToBoolean(row[(int)EnumAllRebarTableColName.IFTAO + 1].ToString());
                    rebarData.IfBend = Convert.ToBoolean(row[(int)EnumAllRebarTableColName.IFBEND + 1].ToString());
                    rebarData.IfCut = Convert.ToBoolean(row[(int)EnumAllRebarTableColName.IFCUT + 1].ToString());
                    rebarData.IfBendTwice = Convert.ToBoolean(row[(int)EnumAllRebarTableColName.IFBENDTWICE + 1].ToString());

                    allRebarList.Add(rebarData);
                }
            }

            return allRebarList;
        }
        /// <summary>
        /// 判断字符串是否为整数，包括负整数
        /// </summary>
        /// <param name="_Value"></param>
        /// <returns></returns>
        private bool RegexIsNumeric(string _Value)
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
        ///     375,90;11675,套;10725,90;375,0
        ///     12000,搭590;12000,搭590;11080,0
        ///     12000,套;3835,90;375,0
        ///     375,90;9155,套;12000,套;11900,丝
        ///     12000,搭590;12000*2,搭590;7520,0
        ///     0,套;12000,套;12000*2,套;3400,90;375,0
        /// </summary>
        /// <param name="_cornerMsg"></param>
        /// <returns></returns>
        public List<GeneralMultiData> GetMultiData(string _cornerMsg)
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
                string[] ssss = _cornerMsg.Split(new char[] { ';'},StringSplitOptions.RemoveEmptyEntries);//去掉split后为空的值

                foreach (string sss in ssss)
                {
                    _data = new GeneralMultiData();

                    string[] ss = sss.Split(',');
                    _data.cornerMsg = sss;

                    #region _data.headType
                    if (ss[1].Equals("0") || ss[1].Equals("0&D"))
                    {
                        _data.headType = EnumMultiHeadType.ORG;
                    }
                    //else if (ss[1].Equals("套"))//套
                    else if (ss[1].IndexOf("套") == 0)//套
                    {
                        _data.headType = EnumMultiHeadType.TAO_P;
                    }
                    else if (ss[1].IndexOf("套") > 0)//25套，变径套筒
                    {
                        _data.headType = EnumMultiHeadType.TAO_V;
                    }
                    else if (RegexIsNumeric(ss[1])) //90，是否为整数，整数则为弯曲角度
                    {
                        _data.headType = EnumMultiHeadType.BEND;
                    }
                    //else if (ss[1].Equals("反套"))
                    else if (ss[1].IndexOf("反套") == 0)
                    {
                        _data.headType = EnumMultiHeadType.TAO_N;
                    }
                    else if (ss[1].IndexOf("反丝") == 0)
                    {
                        _data.headType = EnumMultiHeadType.SI_N;
                    }
                    else if (ss[1].IndexOf("丝") == 0)//特例：【7380,套;12000,丝&D】
                    {
                        _data.headType = EnumMultiHeadType.SI_P;
                    }
                    else if (ss[1].IndexOf("搭") > -1)//含有“搭”
                    {
                        _data.headType = EnumMultiHeadType.DA;
                    }
                    #endregion

                    string[] s = ss[0].Split('*');
                    _data.num = (s.Length > 1) ? Convert.ToInt32(s[1]) : 1;//
                    _data.length = s[0];//

                    datalist.Add(_data);
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
                if (_dd.TypeNum == "70000") { continue; }//7000的图形为异类，排除掉
                if (_dd.TypeNum == "74201" && _dd.IsMulti) { continue; }
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
                    if (_dd.TypeNum == "70000") { continue; }//7000的图形为异类，排除掉
                    if (_dd.TypeNum == "74201" && _dd.IsMulti) { continue; }

                    if (_dd.IsOriginal && _dd.IfTao && !_dd.IfCut && !_dd.IfBend && !_dd.IfBendTwice)//第一种情况，原本就是原材,仅需套丝
                    {
                        _data.TotalPieceNum += _dd.TotalPieceNum;
                        _data.TotalLength += Convert.ToInt32(_dd.Length) * _dd.TotalPieceNum;    //总长度需要乘以数量
                        _data.TotalWeight += _dd.TotalWeight;   //数据源中的重量已经做了汇总
                        
                        //示例：0,套;12000,丝
                        //string[] ttt = _dd.CornerMessage.Split(';');
                        string[] ttt = _dd.CornerMessage.Split(new char[] {';'},StringSplitOptions.RemoveEmptyEntries);//去掉空的

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
                    if (_dd.TypeNum == "70000") { continue; }//70000的图形为异类，排除掉
                    if (_dd.TypeNum == "74201" && _dd.IsMulti) { continue; }

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

                for (int pp = 0; pp < (int)EnumAllRebarTableColName.maxRebarColNum; pp++)
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
                        case (int)EnumAllRebarTableColName.TYPE_NAME:
                            pair = new KeyValuePair<string, object>(GeneralClass.sRebarColumnName[pp], _data.TypeNum);
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
                List<List<GeneralMultiData>> m_listgroup = new List<List<GeneralMultiData>>();
                List<GeneralMultiData> m_list = new List<GeneralMultiData>();

                //先拆分多段数据
                for (int i = 0; i < _MultiData.Count; i++)
                {
                    //特例：【0,套;12000,套;12000,单&D】
                    //特例：【0,套;12000,套;12000,套;5550,0&D】，最后一段识别为none
                    //if (_MultiData[i].headType != EnumMultiHeadType.NONE
                    //    && _MultiData[i].headType != EnumMultiHeadType.BEND
                    //    && _MultiData[i].length != "0")
                    if (_MultiData[i].headType != EnumMultiHeadType.BEND && _MultiData[i].length != "0")
                    {
                        m_list.Add(_MultiData[i]);
                        m_listgroup.Add(m_list);

                        m_list = new List<GeneralMultiData>();//清空重新添加
                    }
                    else
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
                            temp.headType = EnumMultiHeadType.SI_P;

                            m_listgroup[j].Insert(0, temp);
                        }
                        if (m_listgroup[j - 1].Last().headType == EnumMultiHeadType.TAO_N)//反丝
                        {
                            temp.length = "0";
                            temp.num = m_listgroup[j].First().num;
                            temp.cornerMsg = "0,反丝";
                            temp.headType = EnumMultiHeadType.SI_N;

                            m_listgroup[j].Insert(0, temp);
                        }
                    }
                }

                //比对_MultiLength和m_listgroup，如果数量对不上，需要报警提示
                if (_MultiLength.Count != m_listgroup.Count)
                {
                    GeneralClass.interactivityData?.printlog(1, "多段通长筋拆解有误，length数量与cornermsg数量不一致");
                    return null;
                }

                RebarData _tempdata = new RebarData();

                string ss = "";
                for (int k = 0; k < m_listgroup.Count; k++)
                {
                    _tempdata = new RebarData();
                    //_tempdata = _data;      //先复制一份
                    _tempdata.Copy(_data);

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
                        //_tempdata.PieceNumUnitNum= _num[0]+"*"+ (_MultiLength[k].num*Convert.ToInt32( _num[1])).ToString();
                        _tempdata.PieceNumUnitNum = _MultiLength[k].num.ToString() + "*" + (Convert.ToInt32(_num[0]) * Convert.ToInt32(_num[1])).ToString();
                    }
                    else
                    {
                        //_tempdata.PieceNumUnitNum = _num[0] + "*" + (_MultiLength[k].num).ToString();
                        _tempdata.PieceNumUnitNum = _MultiLength[k].num.ToString() + "*" + _num[0].ToString();
                    }
                    _tempdata.TotalPieceNum = _MultiLength[k].num * _data.TotalPieceNum;
                    double tt;
                    double.TryParse(_MultiLength[k].length, out tt);//可能有缩尺
                    _tempdata.TotalWeight = _data.TotalWeight * tt / (double)GetMultiTotalLength(_data.Length);


                    ModifyRebarData(ref _tempdata);//修改其他的项

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
        /// 1420~870,25;750,0
        /// 750,35;1420~870,0
        /// 
        /// 缩尺3： 2060
        ///         ~2660                        10d,135;300~600,90;640,90;300~600,90;640,135;10d,0&G                        
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
                List<RebarData> _list = null;

                int _num = Convert.ToInt32(_data.PieceNumUnitNum.Split('*')[0]);//例如：5*2，取5

                RebarData _newdata = new RebarData();

                string[] _length = _data.Length.Split('~');

                for (int i = 0; i < _num; i++)
                {

                }


                return _list;
            }
            catch (Exception ex) { MessageBox.Show("SplitSuoChiRebar error:" + ex.Message); return null; }

        }
    }
}
