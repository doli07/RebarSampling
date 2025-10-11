using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RebarSampling
{

    /// <summary>
    /// 钢筋施工位置，按照“xx分区xx楼xx层”标准来区分
    /// </summary>
    public struct RebarConstructionPos
    {
        /// <summary>
        /// 分区
        /// </summary>
        public string Zone { get; set; }
        /// <summary>
        /// 楼栋
        /// </summary>
        public string Building { get; set; }
        /// <summary>
        /// 楼层
        /// </summary>
        public string Floor { get; set; }

    }
    public class GroupbyProjectAssemblyList
    {
        public string _projectName { get; set; }
        public string _assemblyName { get; set; }
        public List<RebarData> _datalist { get; set; }
    }
    /// <summary>
    /// 根据直径进行分组的rebardata的list
    /// </summary>
    public class GroupbyDiaWithLength
    {
        public int _diameter { get; set; }
        public int _totallength { get; set; }
        public int _maxlength { get; set; }
        public int _minlength { get; set; }
        public int _totalnum { get; set; }
        public double _totalweight { get; set; }
        public List<RebarData> _datalist { get; set; }
    }

    /// <summary>
    /// 按照钢筋直径进行检索的数据list，自动统计数量、重量
    /// </summary>
    public class GroupbyDia
    {
        public int _diameter { get; set; }
        public int _totalnum { get; set; }
        public double _totalweight { get; set; }
        public List<RebarData> _datalist { get; set; }

    }
    /// <summary>
    /// 按照工艺是否套丝是否弯曲进行分类的数据list，自动统计数量、重量
    /// </summary>

    public class GroupbyTaoBendDatalist
    {
        public bool _iftao { get; set; }
        public bool _ifbend { get; set; }
        public int _totalnum { get; set; }
        public double _totalweight { get; set; }

        public List<RebarData> _datalist { get; set; }
    }
    /// <summary>
    /// 按照钢筋长度进行检索的数据list，自动统计数量、重量
    /// </summary>

    public class GroupbyLengthDatalist
    {
        /// <summary>
        /// 长度，mm
        /// </summary>
        public string _length { get; set; }

        public int _totalnum { get; set; }

        public double _totalweight { get; set; }

        public List<RebarData> _datalist { get; set; }
    }
    /// <summary>
    /// 对应某一直径的rebarOriPi的list
    /// </summary>
    public class RebarOriPiWithDiameter
    {
        public RebarOriPiWithDiameter()
        {
            this.BatchSeri = "";
            this._level = "C";
            this._diameter = 0;
            this.totalBatchNo = 0;
            this.curBatchNo = 0;
            this._rebarOriPiList = new List<RebarOriPi>();
        }
        /// <summary>
        /// 批次序号
        /// </summary>
        public string BatchSeri { get; set; }
        /// <summary>
        /// 原材长度
        /// </summary>
        public int OrignalLength
        {
            get
            {
                return this._rebarOriPiList.Count != 0 ? this._rebarOriPiList.First().OrignalLength : 0;//20250712
            }
        }
        //{ 
        //    get 
        //    { 
        //       return  GeneralClass.OriginalLength(this._level, this._diameter);
        //    }
        //    set { }
        //}

        /// <summary>
        /// 钢筋级别
        /// </summary>
        public string _level { get; set; }
        /// <summary>
        /// 直径
        /// </summary>
        public int _diameter { get; set; }
        /// <summary>
        /// 总批次数
        /// </summary>
        public int totalBatchNo { get; set; }
        /// <summary>
        /// 当前批次数
        /// </summary>
        public int curBatchNo { get; set; }

        public int OriginalNum { get { return this._rebarOriPiList.Sum(t => t.OriginalNum); } }
        public int SteelbarNum { get { return this._rebarOriPiList.Sum(t => t.SteelbarNum); } }
        public double OriginalWeight { get { return this._rebarOriPiList.Sum(t => t.OriginalWeight); } }
        public double SteelbarWeight { get { return this._rebarOriPiList.Sum(t => t.SteelbarWeight); } }
        /// <summary>
        /// 直径对应的rebarOriPi的list
        /// </summary>
        public List<RebarOriPi> _rebarOriPiList { get; set; }
        /// <summary>
        /// 通过二维数组将rebarOriPi转换为rebarPiOri
        /// </summary>
        public List<RebarPiOri> _rebarPiOriList
        {
            get
            {
                try
                {
                    if (this._rebarOriPiList == null || this._rebarOriPiList.Count == 0) { return null; }

                    RebarPi _rebarpi = new RebarPi();
                    //List<RebarPi> _rebarpiList = new List<RebarPi>();
                    //RebarPiOri _rebarpiOri = new RebarPiOri(this._level, this._diameter);
                    RebarPiOri _rebarpiOri = new RebarPiOri(this._rebarOriPiList.First().OrignalLength);

                    List<RebarPiOri> _rebarpiOrilist = new List<RebarPiOri>();

                    foreach (RebarOriPi _oripi in _rebarOriPiList)
                    {
                        Rebar[,] rebars = new Rebar[_oripi.num, _oripi._list[0]._list.Count];

                        for (int i = 0; i < _oripi.num; i++)
                        {
                            for (int j = 0; j < _oripi._list[i]._list.Count; j++)
                            {
                                rebars[i, j] = _oripi._list[i]._list[j];//通过二维数组将rebarOriPi转换为rebarPiOri
                            }
                        }

                        _rebarpiOri._list = new List<RebarPi>();
                        for (int j = 0; j < rebars.GetLength(1); j++)//获取二维数组的列数
                        {
                            _rebarpi = new RebarPi();

                            for (int i = 0; i < rebars.GetLength(0); i++)//获取二维数组的行数
                            {
                                _rebarpi._rebarList.Add(rebars[i, j]);
                            }
                            _rebarpiOri._list.Add(_rebarpi);
                        }
                        _rebarPiOriList.Add(_rebarpiOri);

                    }

                    return _rebarPiOriList;
                }
                catch (Exception ex) { throw ex; }
            }
        }
    }



    /// <summary>
    /// 所有
    /// </summary>
    public class RebarOriPiAllDiameter
    {
        public RebarOriPiAllDiameter()
        {
            this._list = new List<RebarOriPiWithDiameter>();
        }
        /// <summary>
        /// 总批次
        /// </summary>
        public int totalBatchNo { get; set; }
        /// <summary>
        /// 当前批次
        /// </summary>
        public int curBatchNo { get; set; }
        /// <summary>
        /// 原材长度，基于单一直径的原材批的长度
        /// </summary>
        public int OriginalLength { get { return this._list.Count != 0 ? this._list.First().OrignalLength : 0; } }

        public int OriginalNum { get { return this._list.Sum(t => t.OriginalNum); } }
        public int SteelbarNum { get { return this._list.Sum(t => t.SteelbarNum); } }
        public double OriginalWeight { get { return this._list.Sum(t => t.OriginalWeight); } }
        public double SteelbarWeight { get { return this._list.Sum(t => t.SteelbarWeight); } }

        public List<RebarOriPiWithDiameter> _list { get; set; }
    }




    /// <summary>
    /// 加工组合方案完全一致的rebarOri放在一起的数据结构，包含其组合方案和数量，
    /// 注意：
    /// 1、与rebarPiOri区分开
    /// 2、加工组合方案一致不代表来自同一个构件，构件属性信息可能不一致
    /// </summary>
    public class RebarOriPi
    {
        public RebarOriPi()
        {
            this.totalBatchNo = 0;
            this.curBatchNo = 0;
            this._list = new List<RebarOri>();
        }
        public int totalBatchNo { get; set; }
        public int curBatchNo { get; set; }
        /// <summary>
        /// 按顺序各根原材的list的cornerMsg串联所成，可以认为是一种组合方案，考虑各段钢筋的边角信息，即使同样长度的rebar也会属于不同的组合方案
        /// </summary>
        public string CornerMsgSolution
        {
            get
            {
                string str = "";
                foreach (var item in this._list[0]._list)
                {
                    str += item.CornerMessage;//组合边角信息
                }
                return str;
            }

        }
        public List<string> strCornerMsgList
        {
            get
            {
                return (_list != null && _list.Count != 0) ? _list[0].strCornerMsglist : null;
            }
        }
        /// <summary>
        /// 数量
        /// </summary>
        public int num
        {
            get
            {
                return _list.Count;
            }
        }
        /// <summary>
        /// 原材长度
        /// </summary>
        public int OrignalLength
        {
            get
            {
                return _list.Count != 0 ? _list.First()._totalLength : 0;
            }
        }
        public int OriginalNum { get { return this.num; } }
        public int SteelbarNum { get { return this._list.Sum(t => t.SteelbarNum); } }
        public double OriginalWeight { get { return this._list.Sum(t => t.OriginalWeight); } }
        public double SteelbarWeight { get { return this._list.Sum(t => t.SteelbarWeight); } }
        /// <summary>
        /// rebarOri的集合
        /// </summary>
        public List<RebarOri> _list { get; set; }
    }

    /// <summary>
    /// 可以理解为包含了长度和数量二维数据的rebarOri，用于二维套料
    /// </summary>
    public class RebarPiOri
    {
        /// <summary>
        /// 构造函数，定尺原材，20241121修改，根据钢筋级别直径取其原材长度，构建rebarPiOri
        /// </summary>
        public RebarPiOri(string _level, int _diameter)
        {
            this._totalLength = GeneralClass.OriginalLength(_level, _diameter);
            this._list = new List<RebarPi>();

        }
        /// <summary>
        /// 重载构造函数，考虑某些加工原材不一定是用的9m或12m定尺原材，也有非定尺原材
        /// </summary>
        /// <param name="_materialLength"></param>
        public RebarPiOri(int _materialLength)
        {
            this._totalLength = _materialLength;
            this._list = new List<RebarPi>();
        }
        /// <summary>
        /// 当前加工原材的总长度，如果是定尺原材，即为9m或12m，如果是非定尺原材，则在构造时指定长度
        /// </summary>
        public int _totalLength { get; set; }

        /// <summary>
        /// 一次利用的剩余长度
        /// </summary>
        public int _lengthFirstLeft
        {
            get
            {
                return this._totalLength - this._lengthListUsed;
            }
        }
        /// <summary>
        /// 当前已使用的小段总长
        /// </summary>
        public int _lengthListUsed
        {
            get
            {
                return this._list.Sum(t => t.length);
            }
        }
        /// <summary>
        /// 数量，根数
        /// </summary>
        public int _num
        {
            get
            {
                return (this._list.Count != 0) ? _list[0].num : 0;
            }
        }
        public List<RebarPi> _list { get; set; }
    }
    /// <summary>
    /// 用来加工的钢筋原材，由多个小段rebar组成，其原材长度可以是9m或12m的定尺原材，也可以是3m、4m、5m、6m、7m等非定尺原材，20240224
    /// </summary>
    public class RebarOri
    {
        /// <summary>
        /// 构造函数，定尺原材
        /// </summary>
        public RebarOri(string _level, int _diameter)
        {
            this._level = _level;
            this._diameter = _diameter;
            this._totalLength = GeneralClass.OriginalLength(_level, _diameter);
            this._list = new List<Rebar>();
        }
        /// <summary>
        /// 重载构造函数，考虑某些加工原材不一定是用的9m或12m定尺原材，也有非定尺原材
        /// </summary>
        /// <param name="_materialLength"></param>
        public RebarOri(int _materialLength, string _level, int _diameter)
        {
            this._level = _level;
            this._diameter = _diameter;
            this._totalLength = _materialLength;
            this._list = new List<Rebar>();
        }
        public int OriginalNum { get { return 1; } }//原材数量
        public int SteelbarNum { get { return this._list.Count; } }//钢筋数量
        /// <summary>
        /// 计算原材重量，0.001963为钢筋密度，0.001963*钢筋端面截面积=单位米重，再*长度即为原材重量
        /// </summary>
        public double OriginalWeight { get { return Math.PI * this._diameter * this._diameter * 0.001963115 * this._totalLength / 1000; } }
        public double SteelbarWeight { get { return this._list.Sum(t => t.weight); } }//钢筋重量
        /// <summary>
        /// 当前加工原材的总长度，如果是定尺原材，即为9m或12m，如果是非定尺原材，则在构造时指定长度
        /// </summary>
        public int _totalLength { get; set; }

        /// <summary>
        /// 一次利用的剩余长度
        /// </summary>
        public int _lengthFirstLeft
        {
            get
            {
                return this._totalLength - this._lengthListUsed;
                //if(this._list.Count!=0)
                //{
                //    return this._totalLength - this._lengthUsed;
                //}
                //else { return -1; }
            }
        }
        /// <summary>
        /// 一次利用的长度，即当前已使用的所有list小段总长
        /// </summary>
        public int _lengthListUsed
        {
            get
            {
                return this._list.Sum(t => t.length);
            }
        }

        /// <summary>
        /// 余料二次利用的list，目前把1500 和1200 视为可以二次利用的长度
        /// </summary>
        public List<int> _secondUsedList
        {
            get
            {
                List<int> _temp = new List<int>();
                _temp.Add(GeneralClass.CfgData.MatPoolYuliao1);
                _temp.Add(GeneralClass.CfgData.MatPoolYuliao2);

                List<int> _yuliao = new List<int>();
                if (this._lengthFirstLeft >= _temp.Sum())//取两个值
                {
                    _yuliao.AddRange(_temp);
                    return _yuliao;
                }
                else if (this._lengthFirstLeft >= _temp.Max() && this._lengthFirstLeft < _temp.Sum())//取较大值
                {
                    _yuliao.Add(_temp.Max());
                    return _yuliao;
                }
                else if (this._lengthFirstLeft >= _temp.Min() && this._lengthFirstLeft < _temp.Max())//取较小值
                {
                    _yuliao.Add(_temp.Min());
                    return _yuliao;
                }
                else
                {
                    return _yuliao;//空
                }

            }
        }

        /// <summary>
        /// 余料二次利用的长度，目前把1500 和1200 视为可以二次利用的长度,单位：mm
        /// </summary>
        public int _lengthSecondUsed
        {
            get
            {
                if (this._secondUsedList != null && this._secondUsedList.Count != 0)
                {
                    return this._secondUsedList.Sum();
                }
                else { return 0; }
            }
        }

        /// <summary>
        /// 二次利用后的剩余长度
        /// </summary>
        public int _lengthSecondLeft
        {
            get
            {
                return this._totalLength - this._lengthListUsed - this._lengthSecondUsed;
            }
        }
        /// <summary>
        /// 钢筋级别
        /// </summary>
        public string _level
        {
            get; set;
            //get
            //{
            //    if (_list != null && _list.Count != 0)
            //    {
            //        return this._list[0].Level;
            //    }
            //    return "C";
            //}
        }
        /// <summary>
        /// 直径
        /// </summary>
        public int _diameter
        {
            get; set;
            //get
            //{
            //    if (_list != null && _list.Count != 0)
            //    {
            //        return this._list[0].Diameter;
            //    }
            //    return 0;
            //}
        }
        public List<string> strCornerMsglist
        {
            get
            {
                List<string> strlist = new List<string>();
                foreach (var item in this._list)
                {
                    strlist.Add(item.CornerMessage);
                }
                return strlist;
            }
        }

        /// <summary>
        /// 所有钢筋小段的工序数量和
        /// </summary>
        public int _caseCount
        {
            get
            {
                return this._list.Sum(t => t.caseCount);
            }
        }

        /// <summary>
        /// 钢筋小段的list
        /// </summary>
        public List<Rebar> _list { get; set; }
    }

    /// <summary>
    /// 定义批量加工的rebarPi的概念，用来包含长度一致的rebar序列，即为一捆钢筋小段
    /// </summary>
    public class RebarPi
    {
        public RebarPi()
        {
            this._batchseri = string.Empty;
            this._rebarList = new List<Rebar>();
        }
        /// <summary>
        /// 批号流水号
        /// </summary>
        public string _batchseri { get; set; }
        /// <summary>
        /// 直径
        /// </summary>
        public int _diameter
        {
            get
            {
                return (this._rebarList != null && this._rebarList.Count != 0) ? this._rebarList[0].Diameter : 0;
            }
        }
        /// <summary>
        /// 定义rebarPi，要求归于一个rebarPi的边角信息必须一致
        /// </summary>
        public string _cornerMsg
        {
            get
            {
                return (this._rebarList != null && this._rebarList.Count != 0) ? this._rebarList[0].CornerMessage : "";
            }

        }
        /// <summary>
        /// 钢筋简图类型编号
        /// </summary>
        public string _picTypeNum
        {
            get
            {
                return (this._rebarList != null && this._rebarList.Count != 0) ? this._rebarList[0].PicTypeNum : "";
            }
        }
        /// <summary>
        /// 长度
        /// </summary>
        public int length
        {
            get
            {
                return (this._rebarList != null && this._rebarList.Count != 0) ? this._rebarList[0].length : 0;
            }

        }
        /// <summary>
        /// 数量
        /// </summary>
        public int num
        {
            get
            {
                return (this._rebarList != null && this._rebarList.Count != 0) ? this._rebarList.Count : 0;
            }
        }

        public List<Rebar> _rebarList { get; set; }
    }

    /// <summary>
    /// 一根钢筋,在原材上排布的一小段钢筋
    /// </summary>
    public class Rebar : RebarData
    {
        public Rebar()
        {
            this.TaoUsed = false;
            this.PickUsed = false;
            this.seriNo = 0;
            this.length = 0;
        }
        /// <summary>
        /// 构造一个指定长度的虚拟rebar
        /// </summary>
        /// <param name="_length"></param>
        public Rebar(int _length)
        {
            this.TaoUsed = false;
            this.PickUsed = false;
            this.seriNo = 0;
            this.length = _length;
        }
        /// <summary>
        /// 标识钢筋在套料时是否被使用，20240314
        /// </summary>
        public bool TaoUsed { get; set; }
        /// <summary>
        /// 拖拽时是否被选中
        /// </summary>
        public bool PickUsed { get; set; }
        /// <summary>
        /// 一个rebar中包含有多根，seriNo为多根中的序号
        /// </summary>
        public int seriNo { get; set; }
        /// <summary>
        /// 一根钢筋的重量
        /// </summary>
        public double weight
        {
            get
            {
                return this.TotalWeight / this.TotalPieceNum;
            }
        }
        /// <summary>
        /// 转成int的length，
        /// </summary>
        public int length
        {
            get
            {
                int _length = 0;
                if (!int.TryParse(this.Length, out _length))//处理缩尺xx~xx
                {
                    string[] tt = this.Length.Split('~');
                    _length = (Convert.ToInt32(tt[0]) + Convert.ToInt32(tt[1])) / 2;
                    return _length;
                }
                else
                {
                    return _length;
                }

                //int _length = 0;
                //int.TryParse(this.Length, out _length);
                //return _length;
            }
            set
            {
                this.Length = value.ToString();
            }

        }

        /// <summary>
        /// 计算一根钢筋小段的工序数量，用积分制，定义此钢筋段的加工难易度，最后用来进行工艺节拍优化
        /// 1、锯切0分，套丝、弯曲各1分，
        /// 2、左套丝1分，右套丝1分
        /// 
        /// </summary>
        public int caseCount
        {
            get
            {
                int _caseCount = 0;
                if (this.IfTao) _caseCount++;//如果套丝，+1
                if (this.TaosiType == 3) _caseCount++;//如果两头套丝，再+1
                if (this.IfBend) _caseCount++;//如果弯曲，+1
                return _caseCount;
            }
        }

    }
    /// <summary>
    /// 钢筋数据结构
    /// </summary>
    public class RebarData
    {
        public RebarData()
        {
            this.RebarConstructionPos = new RebarConstructionPos();
            this.RebarAssemblyType = EnumRebarAssemblyType.NONE;
            this.BarType = "";
            this.FabricationType = "";
            this.IndexNo = 0;
            this.TableNo = "";
            this.TableName = "";
            this.TableSheetName = "";
            this.ProjectName = "";
            this.MainAssemblyName = "";
            this.ChildAssemblyName = "";
            this.ElementName = "";
            this.PicTypeNum = "";
            this.Level = "";
            this.Diameter = 0;
            this.RebarPic = "";
            this.PicMessage = "";
            this.CornerMessage = "";
            this.CornerMessageBK = "";
            this.Length = "";
            //this.IsMulti = false;
            this.PieceNumUnitNum = "";
            this.TotalPieceNum = 0;
            this.TotalWeight = 0;
            this.Description = "";
            this.SerialNum = "";
            //this.IsOriginal = false;
            //this.IfTao = false;
            //this.IfBend = false;
            //this.IfCut = false;
            //this.IfBendTwice = false;
            this._wareMsg = new WareMsg();
            this._batchMsg = new BatchMsg();
            this.IfMakeInLine = false;
            this.IfPickInBill = false;
            this.IfSendToPCS = false;
            this.IfMakeDone = false;
        }
        public void init()
        {
            this.RebarConstructionPos = new RebarConstructionPos();
            this.RebarAssemblyType = EnumRebarAssemblyType.NONE;
            this.BarType = "";
            this.FabricationType = "";
            this.IndexNo = 0;
            this.TableNo = "";
            this.TableName = "";
            this.TableSheetName = "";
            this.ProjectName = "";
            this.MainAssemblyName = "";
            this.ChildAssemblyName = "";
            this.ElementName = "";
            this.PicTypeNum = "";
            this.Level = "";
            this.Diameter = 0;
            this.RebarPic = "";
            this.PicMessage = "";
            this.CornerMessage = "";
            this.CornerMessageBK = "";
            this.Length = "";
            //this.IsMulti = false;
            this.PieceNumUnitNum = "";
            this.TotalPieceNum = 0;
            this.TotalWeight = 0;
            this.Description = "";
            this.SerialNum = "";
            //this.IsOriginal = false;
            //this.IfTao = false;
            //this.IfBend = false;
            //this.IfCut = false;
            //this.IfBendTwice = false;
            this.IfMakeInLine = false;
            this.IfPickInBill = false;
            this.IfSendToPCS = false;
            this.IfMakeDone = false;
            this._wareMsg = new WareMsg();
            this._batchMsg = new BatchMsg();
        }
        public void Copy(RebarData _data)
        {
            this.RebarConstructionPos = _data.RebarConstructionPos;
            this.RebarAssemblyType = _data.RebarAssemblyType;
            this.BarType = _data.BarType;
            this.FabricationType = _data.FabricationType;
            this.IndexNo = _data.IndexNo;
            this.TableNo = _data.TableNo;
            this.TableName = _data.TableName;
            this.TableSheetName = _data.TableSheetName;
            this.ProjectName = _data.ProjectName;
            this.MainAssemblyName = _data.MainAssemblyName;
            this.ChildAssemblyName = _data.ChildAssemblyName;
            this.ElementName = _data.ElementName;
            this.PicTypeNum = _data.PicTypeNum;
            this.Level = _data.Level;
            this.Diameter = _data.Diameter;
            this.RebarPic = _data.RebarPic;
            this.PicMessage = _data.PicMessage;
            this.CornerMessage = _data.CornerMessage;
            this.CornerMessageBK = _data.CornerMessageBK;
            this.Length = _data.Length;
            //this.IsMulti = _data.IsMulti;
            this.PieceNumUnitNum = _data.PieceNumUnitNum;
            this.TotalPieceNum = _data.TotalPieceNum;
            this.TotalWeight = _data.TotalWeight;
            this.Description = _data.Description;
            this.SerialNum = _data.SerialNum;
            //this.IsOriginal = _data.IsOriginal;
            //this.IfTao = _data.IfTao;
            //this.IfBend = _data.IfBend;
            //this.IfCut = _data.IfCut;
            //this.IfBendTwice = _data.IfBendTwice;
            this.IfMakeInLine = _data.IfMakeInLine;
            this.IfPickInBill = _data.IfPickInBill;
            this.IfSendToPCS = _data.IfSendToPCS;
            this.IfMakeDone = _data.IfMakeDone;
            this._wareMsg = _data._wareMsg;
            this._batchMsg = _data._batchMsg;
        }
        /// <summary>
        /// 施工部位解析
        /// </summary>
        public RebarConstructionPos RebarConstructionPos { get; set; }
        /// <summary>
        /// 钢筋所属构件类型，分为墙、柱、梁、板、楼梯
        /// </summary>
        public EnumRebarAssemblyType RebarAssemblyType { get; set; }
        /// <summary>
        /// 钢筋的形状类型，分为箍筋、拉勾、马凳、端头、主筋
        /// </summary>
        public EnumRebarShapeType RebarShapeType
        {
            get
            {
                if (GeneralClass.CfgData.MaterialBill == EnumMaterialBill.EJIN)//e筋
                {
                    string _msg = this.CornerMessage;

                    if (_msg.Contains("&C") || _msg.Contains("&FC"))
                    {
                        return EnumRebarShapeType.SHAPE_MD;
                    }
                    else if (_msg.Contains("&G"))
                    {
                        return EnumRebarShapeType.SHAPE_GJ;
                    }
                    else if (_msg.Contains("&L"))
                    {
                        return EnumRebarShapeType.SHAPE_LG;
                    }
                    else if (_msg.Contains("&D"))
                    {
                        return EnumRebarShapeType.SHAPE_DT;
                    }
                    else
                    {
                        return EnumRebarShapeType.SHAPE_ZJ;
                    }
                }
                else//广联达
                {
                    if (this.BarType == "直筋")//直筋即为主筋
                    {
                        return EnumRebarShapeType.SHAPE_ZJ;
                    }
                    else //广联达的非直筋一般就是箍筋
                    {
                        if (this.FabricationType == "拉筋")
                        {
                            return EnumRebarShapeType.SHAPE_LG;//拉勾
                        }
                        else
                        {
                            return EnumRebarShapeType.SHAPE_GJ;//箍筋
                        }
                    }

                }

            }
        }
        /// <summary>
        /// 钢筋类型，主要区分：直筋和箍筋
        /// 广联达料单特有
        /// </summary>
        public string BarType { get; set; }
        /// <summary>
        /// 钢筋制作类型（绑扎类型），直筋分为：插筋、接筋、水平筋、垂直筋；箍筋分为：外箍、内箍、拉筋。
        /// 广联达料单特有
        /// </summary>
        public string FabricationType { get; set; }
        /// <summary>
        /// 如果是箍筋，计算出其矩形长宽，tuple的item1为宽，item2为高，如果不是箍筋，返回<0,0>
        /// 示例：
        ///         10d,135;750,90;150,90;750,90;150,135;10d,0&G
        ///         10d,135;450,90;200,90;450,90;200,135;10d,0&G
        /// </summary>
        public Tuple<int, int> GJsize
        {
            get
            {
                Tuple<int, int> _ret = new Tuple<int, int>(0, 0);
                if (this.RebarShapeType == EnumRebarShapeType.SHAPE_GJ)//如果是箍筋
                {
                    List<GeneralMultiData> _MultiData = GeneralClass.LDOpt.ldhelper.GetMultiData(this.CornerMessage);//拆解边角信息，提取矩形边长
                    _ret = new Tuple<int, int>(_MultiData[1].ilength, _MultiData[2].ilength);//取第二段长度为箍筋的宽度，取第三段长度为箍筋的高度
                }
                return _ret;
            }
        }
        /// <summary>
        /// 钢筋的尺寸类型，分为棒材线材
        /// </summary>
        public EnumRebarSizeType RebarSizeType
        {
            get
            {
                int _bangThreshold = GeneralClass.m_typeC12 ? 12 : (GeneralClass.m_typeC14 ? 14 : 16);//区分线材棒材的阈值
                return (this.Diameter >= _bangThreshold) ? EnumRebarSizeType.BANG : EnumRebarSizeType.XIAN;
            }
        }
        /// <summary>
        /// 在数据库中的索引id
        /// </summary>
        public int IndexNo { get; set; }
        /// <summary>
        /// 料表编号
        /// </summary>
        public string TableNo { get; set; }
        /// <summary>
        /// 料表名称
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 料表sheet名称
        /// </summary>
        public string TableSheetName { get; set; }
        /// <summary>
        /// 项目名称，对应的excel中的文件名
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 主构件部位名称，对应的excel中的sheetname
        /// </summary>
        public string MainAssemblyName { get; set; }
        /// <summary>
        /// 子构件部位名称，对应的excel表中空白行的名称，一般作为子构件部位名称，后续的所有子构件均隶属于该子构件部位
        /// </summary>
        public string ChildAssemblyName { get; set; }
        /// <summary>
        /// 子构件名称，对应excel表中的构件名称，
        /// </summary>
        public string ElementName { get; set; }
        /// <summary>
        /// 钢筋简图类型编号
        /// </summary>
        public string PicTypeNum { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 直径
        /// </summary>
        public int Diameter { get; set; }
        /// <summary>
        /// 钢筋简图
        /// </summary>
        public string RebarPic { get; set; }
        /// <summary>
        /// 图片信息
        /// </summary>
        public string PicMessage { get; set; }
        /// <summary>
        /// 边角结构信息
        /// </summary>
        public string CornerMessage { get; set; }
        /// <summary>
        /// 备份一下边角结构信息，用于多段线后续会拆解cornerMessage，怕找不到原始边角信息
        /// </summary>
        public string CornerMessageBK { get; set; }
        /// <summary>
        /// 钢筋下料长度，单位：mm，有多段的情况，其length字段中通过‘\n’隔开多段的长度值，广联达料单以“/”分隔
        /// </summary>
        public string Length { get; set; }
        /// <summary>
        /// 转成int的length，
        /// </summary>
        public int iLength
        {
            get
            {
                //if (!int.TryParse(item._length, out ilength))//处理缩尺xx~xx
                //{
                //    string[] tt = item._length.Split('~');
                //    ilength = (Convert.ToInt32(tt[0]) + Convert.ToInt32(tt[1])) / 2;
                //}
                int _length = 0;
                int.TryParse(this.Length, out _length);
                return _length;
            }
            set { }

        }
        /// <summary>
        /// 是否多段，注意，E筋中多段钢筋的下料长度用"\n"隔开，而广联达用"/"隔开
        /// </summary>
        public bool IsMulti
        {
            get
            {
                if (GeneralClass.CfgData.MaterialBill == EnumMaterialBill.EJIN)//e筋
                {
                    string[] sss = this.Length.Split('\n');
                    return (sss.Length > 1) ? true : false;

                    //string[] sss = _length.Split('\n');  //下料长度可能会出现多段的情况，此时需按照'\n'进行拆分字符串
                    //rebarData.Length = _length;
                    ////rebarData.IsMulti = (sss.Length > 1) ? true : false;

                }
                else//广联达
                {
                    if (this.Length.IndexOf('/') > -1)//注意，E筋中多段钢筋的下料长度用"\n"隔开，而广联达用"/"隔开
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
        }

        public bool IsSuoChi
        {
            get
            {
                //return (this.CornerMessage.IndexOf('~') > -1||this.CornerMessageBK.IndexOf('~')>-1) ? true : false;
                return (this.CornerMessage.IndexOf('~') > -1 ) ? true : false;//20250725，边角结构备份是完整的信息，边角结构是拆分过后的 信息

            }
        }
        /// <summary>
        /// 根数*件数
        /// </summary>
        public string PieceNumUnitNum { get; set; }
        /// <summary>
        /// 总根数
        /// </summary>
        public int TotalPieceNum { get; set; }
        /// <summary>
        /// 总重量，kg
        /// </summary>
        public double TotalWeight { get; set; }
        /// <summary>
        /// 说明备注
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 标注序号，注意excel文件中，有效钢筋记录每行的最后一个数字为标注序号
        /// </summary>
        public string SerialNum { get; set; }
        /// <summary>
        /// 标注序号可能经过分解，会带了-1-2等后缀，此处需提取int型序号
        /// </summary>
        public int iSerialNum
        {
            get
            {
                if(SerialNum!=string.Empty)
                {
                    string _seri = SerialNum.Split('-')[0];
                    return Convert.ToInt32(_seri);
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 是否原材，长度为9000或者12000，即为原材
        /// </summary>
        public bool IsOriginal
        {
            get
            {
                return (this.Length == "9000" || this.Length == "12000") ? true : false;//标注是否为原材，长度为9000或者12000，为原材
            }
        }
        /// <summary>
        /// 是否含有圆弧
        /// </summary>
        public bool IfHaveARC
        {
            get
            {
                List<GeneralMultiData> _MultiData = GeneralClass.LDOpt.ldhelper.GetMultiData(this.CornerMessage, this.Diameter);
                if (_MultiData.Exists(t => t.headType == EnumMultiHeadType.ARC))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
        /// <summary>
        /// 是否需要套丝
        /// </summary>
        public bool IfTao
        {
            get
            {
                //如果边角结构信息中含有“套”或者“丝”或者“反”，则认为其需要套丝
                return (this.CornerMessage.IndexOf("套") > -1 || this.CornerMessage.IndexOf("丝") > -1 || this.CornerMessage.IndexOf("反") > -1) ? true : false;
            }
        }
        /// <summary>
        /// 套丝类型:
        /// 0：不套丝；
        /// 1：仅端头正丝；
        /// 2：仅端头反丝，
        /// 3：仅端尾正丝；
        /// 4：仅端尾反丝；
        /// 5：两头正丝；
        /// 6：头正尾反；
        /// 7：头反尾正；
        /// </summary>
        public int TaosiType
        {
            get
            {
                if (!this.IfTao) return 0;//不套丝
                List<GeneralMultiData> _list = GeneralClass.LDOpt.ldhelper.GetMultiData(this.CornerMessage, this.Diameter);
                if (_list != null && _list.Count != 0)
                {
                    if (_list.First().ilength == 0 && _list.First().type == 3 && _list.Last().type == 2)//头反尾正，
                    {
                        return 7;
                    }
                    else if (_list.First().ilength == 0 && _list.First().type == 2 && _list.Last().type == 3)//头正尾反
                    {
                        return 6;
                    }
                    else if (_list.First().ilength == 0 && _list.First().type == 2 && _list.Last().type == 2)//两头正丝
                    {
                        return 5;
                    }
                    else if (_list.First().ilength != 0 && _list.Last().type == 3)//仅端尾反丝
                    {
                        return 4;
                    }
                    else if (_list.First().ilength != 0 && _list.Last().type == 2)//仅端尾正丝
                    {
                        return 3;
                    }
                    else if (_list.First().ilength == 0 && _list.First().type == 3 && _list.Last().type != 2 && _list.Last().type != 3)//仅端头反丝
                    {
                        return 2;
                    }
                    else if (_list.First().ilength == 0 && _list.First().type == 2 && _list.Last().type != 2 && _list.Last().type != 3)//仅端头正丝
                    {
                        return 1;
                    }
                    else { return 0; }//不套丝
                }
                else { return 0; }
            }
        }
        /// <summary>
        /// 反丝
        /// </summary>
        public bool IfTaoFan
        {
            get
            {
                //如果边角结构信息中含有“反”，则认为其需要反丝
                return (this.CornerMessage.IndexOf("反") > -1) ? true : false;
            }
        }
        /// <summary>
        /// 正丝
        /// </summary>
        public bool IfTaoZheng
        {
            get
            {
                //如果边角结构信息中含有“反”，则认为其需要反丝
                return (this.CornerMessage.IndexOf("套") > -1 || this.CornerMessage.IndexOf("丝") > -1) ? true : false;
            }
        }
        /// <summary>
        /// 是否需要弯曲，此标志位判断该钢筋真正是否需要弯曲，注意与ifbend的区别
        /// </summary>
        public bool IfBendOri
        {
            get
            {
                bool _ifbend = false;
                List<GeneralMultiData> _MultiData = GeneralClass.LDOpt.ldhelper.GetMultiData(this.CornerMessage);
                if (_MultiData != null && _MultiData.Count != 0)
                {
                    foreach (var item in _MultiData)
                    {
                        if (item.headType == EnumMultiHeadType.BEND) _ifbend = true;
                    }
                }
                return _ifbend;
            }
        }


        /// <summary>
        /// 是否需要弯曲，此标志位用于产线加工时判断是否需要弯曲，考虑产线无法加工某些长的弯拐
        /// </summary>
        public bool IfBend
        {
            get
            {
                //拆解cornerMsg,如果存在bend类型的multidata，则需要弯曲,20230907修改bug
                bool _ifbend = false;
                List<GeneralMultiData> _MultiData = GeneralClass.LDOpt.ldhelper.GetMultiData(this.CornerMessage);
                if (_MultiData != null&&_MultiData.Count!=0)
                {
                    //20251010增加是否弯曲的规则
                    //规则五 角度小于10°的不弯曲
                    foreach (var item in _MultiData)
                    {
                        if (item.headType == EnumMultiHeadType.BEND && Math.Abs(item.angle) <= GeneralClass.CfgData.BelowAngleAutoNoBend)
                        {
                            _ifbend = false;
                            return _ifbend;
                        }
                    }

                    //20250712修改是否弯曲的规则，
                    //规则一 中段两侧的如有两段弯曲的不弯
                    //规则二 当前段和后一段长度至少有一个少于1800，负角度可以放宽到2000，正角度限制在1800
                    //规则三 当前段和后一段长度和不能少于1800   
                    //规则四 当前段和后一段长度至少 有一个大于1400                    
                    int _middleLength = _MultiData.Max(t => t.ilength);//取最长的一段作为中段
                    int _midIndex = 0;
                    foreach (var item in _MultiData)
                    {
                        if (item.ilength == _middleLength) { _midIndex = _MultiData.IndexOf(item); break; }//先找到中段长度和中段的index
                    }

                    int _bendCount1 = 0, _bendCount2 = 0;
                    for (int i = 0; i < _midIndex ; i++)
                    {
                        if (_MultiData[i].headType == EnumMultiHeadType.BEND) { _bendCount1++; }
                    }
                    for(int i=_midIndex;i<_MultiData.Count;i++)
                    {
                        if (_MultiData[i].headType == EnumMultiHeadType.BEND) { _bendCount2++; }
                    }
                    if(_bendCount1>1||_bendCount2>1) //中段两侧的如有两段弯曲的不弯
                    {
                        _ifbend = false;
                        return _ifbend;
                    }
                    //foreach(var item in _MultiData)
                    //{
                    //    if (item.headType == EnumMultiHeadType.BEND) { _bendCount++; }
                    //}
                    //if(_bendCount>1) //规则一 两段弯曲的不弯
                    //{
                    //    _ifbend = false;
                    //    return _ifbend;
                    //}


                    int _bendCount = 0;//弯曲计数
                    int _bendPermission = 0;//弯曲许可计数
                    //当前段和后一段长度至少有一个少于1800，当前段和后一段长度和不能少于1800 ，当前段和后一段长度至少 有一个大于1400
                    for (int i = 0; i < _MultiData.Count; i++) 
                    {
                        if (_MultiData[i].headType==EnumMultiHeadType.BEND)
                        {
                            _bendCount++;
                        }
                        else
                        {
                            continue;//如果不弯，则continue下一个循环
                        }
                        //负角度可以放宽到2000，正角度限制在1800
                        int _threshold = _MultiData[i].angle > 0 ? GeneralClass.CfgData.P_AngleMaxLength : GeneralClass.CfgData.N_AngleMaxLength;
                        //最短中段
                        int _minMiddle = GeneralClass.CfgData.MinMiddleLength;

                        if (
                            (_MultiData[i].ilength <= _threshold || _MultiData[i + 1].ilength <= _threshold) &&
                            //(_MultiData[i].ilength + _MultiData[i + 1].ilength) > 1800 &&
                            (_MultiData[i].ilength >= _minMiddle || _MultiData[i + 1].ilength >= _minMiddle)
                            )
                        {
                            _bendPermission++;           //允许弯曲的计数                 
                        }
                    }

                    if(_bendCount>0&&_bendCount==_bendPermission)//必须是有需要弯曲的计数，并且许可弯曲的计数与需要弯曲的计数相等，才能弯曲，否则不弯曲
                    {
                        _ifbend = true;
                    }
                    else
                    {
                        _ifbend=false;  
                    }
                }
                return _ifbend;
            }
        }
        /// <summary>
        /// 是否需要切断
        /// </summary>
        public bool IfCut
        {
            get
            {
                //标注是否需要切断，原材以外的都需要切断
                return (this.Length == "9000" || this.Length == "12000") ? false : true;
            }
        }
        /// <summary>
        /// 是否需要弯曲两次以上
        /// </summary>
        public bool IfBendTwice
        {
            get
            {
                //1、2、3开头的图形编号为需要弯折两次以下的，其他的需要弯折2次以上
                return (this.PicTypeNum.Substring(0, 1) == "1" || this.PicTypeNum.Substring(0, 1) == "2" || this.PicTypeNum.Substring(0, 1) == "3") ? false : true;
            }
        }
        /// <summary>
        /// 是否在产线生产
        /// </summary>
        public bool IfMakeInLine { get; set; }
        /// <summary>
        /// 是否勾选进入加工单
        /// </summary>
        public bool IfPickInBill { get; set; }
        /// <summary>
        /// 是否下发pcs
        /// </summary>
        public bool IfSendToPCS { get; set; }
        /// <summary>
        /// 是否加工完成
        /// </summary>
        public bool IfMakeDone { get; set; }

        /// <summary>
        /// 单段钢筋的成品仓位信息
        /// </summary>
        public WareMsg _wareMsg { get; set; }
        /// <summary>
        /// 批次信息
        /// </summary>
        public BatchMsg _batchMsg { get; set; }
    }

    ///// <summary>
    ///// 多段钢筋，rebardata分为四类：【箍筋】、【拉勾】、【马凳】、【多段】
    ///// </summary>
    //public class  RebarData_DD:RebarData
    //{

    //}
    ///// <summary>
    ///// 箍筋，rebardata分为四类：【箍筋】、【拉勾】、【马凳】、【多段】
    ///// </summary>
    //public class RebarData_GJ : RebarData
    //{

    //}
    ///// <summary>
    ///// 拉勾，rebardata分为四类：【箍筋】、【拉勾】、【马凳】、【多段】
    ///// </summary>
    //public class RebarData_LG : RebarData
    //{ 

    //}
    ///// <summary>
    ///// 马凳，rebardata分为四类：【箍筋】、【拉勾】、【马凳】、【多段】
    ///// </summary>
    //public class RebarData_MD : RebarData 
    //{ 

    //}

}
