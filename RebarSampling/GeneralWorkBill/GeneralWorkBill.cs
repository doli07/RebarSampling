using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebarSampling
{
    /// <summary>
    /// 套料之后的钢筋rebar列表
    /// </summary>
    public class RebarTaoLiao
    {
        public RebarTaoLiao() 
        {
            DiameterType = EnumDiameterType.NONE;
            WareNumType = EnumWareNumGroup.NONE;
            BatchNo = 0;
            Diameter = 0;
            _rebarlist=new List<RebarOri>();
        }
        public EnumDiameterType DiameterType { get; set; }

        public EnumWareNumGroup WareNumType { get; set; }

        public int BatchNo { get; set; } 

        public int Diameter { get; set; }

        public List<RebarOri> _rebarlist { get; set; }
    }

    public struct BatchMsg
    {
        /// <summary>
        /// 总批次
        /// </summary>
        public int totalBatch { get; set; }
        /// <summary>
        /// 当前批次
        /// </summary>
        public int curBatch { get; set; }
        /// <summary>
        /// 子批次总数，按照直径种类的数量来分
        /// </summary>
        public int totalchildBatch { get; set; }
        /// <summary>
        /// 当前子批次
        /// </summary>
        public int curChildBatch { get; set; }
    }

    /// <summary>
    /// 成品仓储信息
    /// </summary>
    public struct WareMsg
    {
        /// <summary>
        /// 成品仓通道编号
        /// </summary>
        public int channel { get; set; }
        /// <summary>
        /// 仓位总数,原则：EIGHT:1~15(8仓)，FOUR:16~50(4仓)，TWO:51~100(2仓)，ONE:100~(1仓)
        /// </summary>
        public EnumWareNumGroup totalware { get; set; }
        /// <summary>
        /// 仓位编号
        /// </summary>
        public int wareno { get; set; }
    }
    public struct SingleRebarMsg
    {
        /// <summary>
        /// 班次
        /// </summary>
        public int shift { get; set; }
        /// <summary>
        /// 批次信息
        /// </summary>
        public BatchMsg BatchMsg { get; set; }
        /// <summary>
        /// 总的原材根数
        /// </summary>
        public int totalOriginal { get; set; }
        /// <summary>
        /// 当前的原材流水号
        /// </summary>
        public int curOriginal { get; set; }
        /// <summary>
        /// 当前的小段的编号
        /// </summary>
        public int curSingle { get; set; }
        /// <summary>
        /// 仓储信息
        /// </summary>
        public WareMsg wareMsg { get; set; }

    }
    /// <summary>
    /// 单段钢筋的信息数据格式，一根原材上可能有几段钢筋
    /// </summary>
    public class SingleRebarData
    {
        /// <summary>
        /// 20230628_1_098_003_123_065_01
        /// 字段代表含义：
        /// 日期_班次_批次总数_批次流水号_当前批钢筋原材总数_钢筋原材流水号_当前小段的流水号
        /// </summary>
        public string SeriNo { get; set; }
        /// <summary>
        /// 项目名称，对应的excel中的文件名
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 主构件名称，对应的excel中的sheetname
        /// </summary>
        public string AssemblyName { get; set; }
        /// <summary>
        /// 子构件名称，对应excel表中的构件名称
        /// </summary>
        public string ElementName { get; set; }
        /// <summary>
        /// 此钢筋对应的仓库编号: 2_4_1        
        /// 字段含义：
        /// 通道编号_当前通道总仓位数_仓位号
        /// </summary>
        public string WareInfo { get; set; }
        /// <summary>
        /// 加工图形编号
        /// </summary>
        public string PicNo { get; set; }
        /// <summary>
        /// 钢筋级别
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 直径
        /// </summary>
        public int Diameter { get; set; }
        /// <summary>
        /// 下料长度
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public double Weight { get; set; }
        /// <summary>
        /// 边角结构,
        /// </summary>
        public string CornerMsg { get; set; }

        /// <summary>
        /// 数据库索引ID，
        /// 1、激光打标使用此参数进行打标；
        /// 2、弯曲线视觉扫码时，读取此信息进入数据库进行检索，调出对应的弯曲加工信息
        /// </summary>
        public string IndexCode { get; set; }
    }


    public struct WorkBillMsg
    {
        /// <summary>
        /// 班次
        /// </summary>
        public int shift { get; set; }

        public BatchMsg BatchMsg { get; set; }  
        /// <summary>
        /// 总的原材根数
        /// </summary>
        public int totalOriginal { get; set; }
        /// <summary>
        /// 当前的原材流水号
        /// </summary>
        public int curOriginal { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string projectName { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public string block { get; set; }
        /// <summary>
        /// 楼栋
        /// </summary>
        public string building { get; set; }
        /// <summary>
        /// 楼层
        /// </summary>
        public string floor { get; set; }
        /// <summary>
        /// 钢筋级别
        /// </summary>
        public string level { get; set; }
        /// <summary>
        /// 厂商
        /// </summary>
        public string brand { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        public string specification { get; set; }
        /// <summary>
        /// 原材长度，一般为9米或12米
        /// </summary>
        public int originLength { get; set; }
    }

    /// <summary>
    /// 生产工单数据格式，以单根原材为数据单元
    /// </summary>
    public class WorkBill
    {
        public WorkBill()
        {
            this.Msgtype = 1;
            this.BillNo = "";
            this.SteelbarNo = "";
            this.ProjectName = "";
            this.Block = "";
            this.Building = "";
            this.Floor = "";
            this.Level = "";
            this.Diameter = 0;
            this.Brand = "";
            this.Specification = "";
            this.OriginalLength = 0;
            this.SteelbarList = new List<SingleRebarData>();
        }
        /// <summary>
        /// 数据类型：
        /// 1:请求工单
        /// 2:工单;
        /// 3:完成信号;
        /// </summary>
        public int Msgtype { get; set; }
        /// <summary>
        /// 工单号:20230628_1_098_003
        /// 字段代表含义：
        /// 日期_班次_批次总数_批次流水号
        /// </summary>
        public string BillNo { get; set; }
        /// <summary>
        /// 20230628_1_098_003_123_065
        ///字段代表含义：
        ///日期_班次_批次总数_批次流水号_当前批钢筋原材总数_钢筋原材流水号
        /// </summary>
        public string SteelbarNo { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 分区，A区，B区等等
        /// </summary>
        public string Block { get; set; }
        /// <summary>
        /// 楼栋号
        /// </summary>
        public string Building { get; set; }
        /// <summary>
        /// 楼层号
        /// </summary>
        public string Floor { get; set; }
        /// <summary>
        /// 钢筋级别
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 直径
        /// </summary>
        public int Diameter { get; set; }
        /// <summary>
        /// 钢筋厂商
        /// </summary>
        public string Brand { get; set; }
        /// <summary>
        /// 钢筋规格
        /// </summary>
        public string Specification { get; set; }
        /// <summary>
        /// 原材长度，一般9米或12米两种
        /// </summary>
        public int OriginalLength { get; set; }


        public List<SingleRebarData> SteelbarList { get; set; }
    }
}
