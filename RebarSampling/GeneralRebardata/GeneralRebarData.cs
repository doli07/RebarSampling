using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    /// 加工组合方案完全一致的rebarOri放在一起的数据结构，包含其组合方案和数量，
    /// 注意：
    /// 1、与rebarPiOri区分开
    /// 2、加工组合方案一致不代表来自同一个构件，构件属性信息可能不一致
    /// </summary>
    public class RebarOriPi
    {
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
        /// <summary>
        /// 数量
        /// </summary>
        public int num { get; set; }
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
        /// 构造函数，定尺原材
        /// </summary>
        public RebarPiOri()
        {
            this._totalLength = GeneralClass.OriginalLength;
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
        public RebarOri()
        {
            this._totalLength = GeneralClass.OriginalLength;
            this._list = new List<Rebar>();
        }
        /// <summary>
        /// 重载构造函数，考虑某些加工原材不一定是用的9m或12m定尺原材，也有非定尺原材
        /// </summary>
        /// <param name="_materialLength"></param>
        public RebarOri(int _materialLength)
        {
            this._totalLength = _materialLength;
            this._list = new List<Rebar>();
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
        /// 直径
        /// </summary>
        public int _diameter
        {
            get
            {
                if (_list != null && _list.Count != 0)
                {
                    return this._list[0].Diameter;
                }
                return 0;
            }
        }
        /// <summary>
        /// 钢筋小段的list
        /// </summary>
        public List<Rebar> _list { get; set; }
    }

    /// <summary>
    /// 定义批量加工的rebarPi的概念，用来包含长度一致的rebar序列
    /// </summary>
    public class RebarPi
    {
        /// <summary>
        /// 长度
        /// </summary>
        public int length { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int num
        {
            get
            {
                return this._rebarList.Count;
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

    }
    /// <summary>
    /// 钢筋数据结构
    /// </summary>
    public class RebarData
    {
        public RebarData()
        {
            this.RebarConstructionPos = new RebarConstructionPos();
            this.RebarAssemblyType = EnumRebarAssemblyType.None;
            this.IndexNo = 0;
            this.ProjectName = "";
            this.MainAssemblyName = "";
            this.ElementName = "";
            this.PicTypeNum = "";
            this.Level = "";
            this.Diameter = 0;
            this.RebarPic = "";
            this.PicMessage = "";
            this.CornerMessage = "";
            this.Length = "";
            this.IsMulti = false;
            this.PieceNumUnitNum = "";
            this.TotalPieceNum = 0;
            this.TotalWeight = 0;
            this.Description = "";
            this.SerialNum = 0;
            //this.IsOriginal = false;
            //this.IfTao = false;
            //this.IfBend = false;
            //this.IfCut = false;
            //this.IfBendTwice = false;
            this.WareMsg = new WareMsg();
            this.BatchMsg = new BatchMsg();
        }
        public void init()
        {
            this.RebarConstructionPos = new RebarConstructionPos();
            this.RebarAssemblyType = EnumRebarAssemblyType.None;
            this.IndexNo = 0;
            this.ProjectName = "";
            this.MainAssemblyName = "";
            this.ElementName = "";
            this.PicTypeNum = "";
            this.Level = "";
            this.Diameter = 0;
            this.RebarPic = "";
            this.PicMessage = "";
            this.CornerMessage = "";
            this.Length = "";
            this.IsMulti = false;
            this.PieceNumUnitNum = "";
            this.TotalPieceNum = 0;
            this.TotalWeight = 0;
            this.Description = "";
            this.SerialNum = 0;
            //this.IsOriginal = false;
            //this.IfTao = false;
            //this.IfBend = false;
            //this.IfCut = false;
            //this.IfBendTwice = false;
            this.WareMsg = new WareMsg();
            this.BatchMsg = new BatchMsg();
        }
        public void Copy(RebarData _data)
        {
            this.RebarConstructionPos = _data.RebarConstructionPos;
            this.RebarAssemblyType = _data.RebarAssemblyType;
            this.IndexNo = _data.IndexNo;
            this.ProjectName = _data.ProjectName;
            this.MainAssemblyName = _data.MainAssemblyName;
            this.ElementName = _data.ElementName;
            this.PicTypeNum = _data.PicTypeNum;
            this.Level = _data.Level;
            this.Diameter = _data.Diameter;
            this.RebarPic = _data.RebarPic;
            this.PicMessage = _data.PicMessage;
            this.CornerMessage = _data.CornerMessage;
            this.Length = _data.Length;
            this.IsMulti = _data.IsMulti;
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
            this.WareMsg = _data.WareMsg;
            this.BatchMsg = _data.BatchMsg;
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
        /// 在数据库中的索引id
        /// </summary>
        public int IndexNo { get; set; }
        /// <summary>
        /// 项目名称，对应的excel中的文件名
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 主构件名称，对应的excel中的sheetname
        /// </summary>
        public string MainAssemblyName { get; set; }
        /// <summary>
        /// 子构件名称，对应excel表中的构件名称
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
        /// 钢筋下料长度，单位：mm，有多段的情况，其length字段中通过\n隔开多段的长度值
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
        /// 是否多段
        /// </summary>
        public bool IsMulti { get; set; }
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
        public int SerialNum { get; set; }

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
        /// 是否需要弯曲
        /// </summary>
        public bool IfBend
        {
            get
            {
                //拆解cornerMsg,如果存在bend类型的multidata，则需要弯曲,20230907修改bug
                bool _ifbend = false;
                List<GeneralMultiData> _MultiData = SQLiteOpt.GetMultiData(this.CornerMessage);
                if (_MultiData != null)
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
        /// 单段钢筋的成品仓位信息
        /// </summary>
        public WareMsg WareMsg { get; set; }
        /// <summary>
        /// 批次信息
        /// </summary>
        public BatchMsg BatchMsg { get; set; }
    }






}
