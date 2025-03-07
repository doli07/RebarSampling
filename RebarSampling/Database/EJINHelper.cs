using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RebarSampling.Database
{
    /// <summary>
    /// e筋料单解析类
    /// </summary>
    public class EJINHelper:ILDHelper
    {
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
                        _MultiData[i].cornerMsg = _MultiData[i].msg_first.Split('*')[0] + "," + _MultiData[i].msg_second;//20240909修改，去掉【6000*2,90】后面的【*2】
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
                    _tempdata.TableName = _data.TableName;//料表名
                    _tempdata.TableSheetName = _data.TableSheetName;//料表sheet名

                    //ModifyRebarData(ref _tempdata);//修改其他的项
                    ModifyRebarPicNum(ref _tempdata);//修改图形编号

                    _list.Add(_tempdata);
                }
                return _list;
            }
            catch (Exception ex) { MessageBox.Show("SplitMultiRebar error:" + ex.Message); return null; }

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
        public  List<GeneralMultiData> GetMultiData(string _cornerMsg, int _diameter = 1)
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





    }
}
