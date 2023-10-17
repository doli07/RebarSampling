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

    public class GroupbyDiameterListWithLength
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
    public class GroupbyDiameterlist
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
        public string _length { get; set; }

        public int _totalnum { get; set; }

        public double _totalweight { get; set; }

        public List<RebarData> _datalist { get; set; }
    }




    /// <summary>
    /// 一根钢筋
    /// </summary>
    public class Rebar : RebarData
    {
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
                int.TryParse(this.Length, out _length);
                return _length;
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
            this.TypeNum = "";
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
            this.IsOriginal = false;
            this.IfTao = false;
            this.IfBend = false;
            this.IfCut = false;
            this.IfBendTwice = false;
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
            this.TypeNum = "";
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
            this.IsOriginal = false;
            this.IfTao = false;
            this.IfBend = false;
            this.IfCut = false;
            this.IfBendTwice = false;
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
            this.TypeNum = _data.TypeNum;
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
            this.IsOriginal = _data.IsOriginal;
            this.IfTao = _data.IfTao;
            this.IfBend = _data.IfBend;
            this.IfCut = _data.IfCut;
            this.IfBendTwice = _data.IfBendTwice;
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
        /// 构件类型编号
        /// </summary>
        public string TypeNum { get; set; }
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
        public bool IsOriginal { get; set; }
        /// <summary>
        /// 是否需要套丝
        /// </summary>
        public bool IfTao { get; set; }
        /// <summary>
        /// 是否需要弯曲
        /// </summary>
        public bool IfBend { get; set; }
        /// <summary>
        /// 是否需要切断
        /// </summary>
        public bool IfCut { get; set; }
        /// <summary>
        /// 是否需要弯曲两次以上
        /// </summary>
        public bool IfBendTwice { get; set; }
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
