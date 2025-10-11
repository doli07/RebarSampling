using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace RebarSampling
{
    /// <summary>
    /// 套料之后的钢筋原材rebarOri列表
    /// </summary>
    public class RebarTaoLiao
    {
        public RebarTaoLiao()
        {
            DiameterType = EnumDiaGroupType.NONE;
            WareNumType = EnumWareNumSet.NONE;
            BatchNo = 0;
            Diameter = 0;
            _rebarOriList = new List<RebarOri>();
        }
        /// <summary>
        /// 直径分组类型，1~4种/5种以上
        /// </summary>
        public EnumDiaGroupType DiameterType { get; set; }
        /// <summary>
        /// 料仓类型，8/4/2/1仓
        /// </summary>
        public EnumWareNumSet WareNumType { get; set; }
        /// <summary>
        /// 批次
        /// </summary>
        public int BatchNo { get; set; }
        /// <summary>
        /// 直径
        /// </summary>
        public int Diameter { get; set; }
        /// <summary>
        /// 钢筋原材list
        /// </summary>
        public List<RebarOri> _rebarOriList { get; set; }
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
        public int warehouseNo { get; set; }
        /// <summary>
        /// 仓位总数,原则：EIGHT:1~15(8仓)，FOUR:16~50(4仓)，TWO:51~100(2仓)，ONE:100~(1仓)
        /// </summary>
        public EnumWareNumSet wareSet { get; set; }
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
    public class WorkBill_LB_SingleRebar
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
        /// 套丝类型，0：不套丝，1：端头套丝，2：端尾套丝，3：两头套丝
        /// </summary>
        public int TaosiType {  get; set; }

        /// <summary>
        /// 数据库索引ID，
        /// 1、激光打标使用此参数进行打标；
        /// 2、弯曲线视觉扫码时，读取此信息进入数据库进行检索，调出对应的弯曲加工信息
        /// </summary>
        public string IndexCode { get; set; }
        /// <summary>
        /// 料单中钢筋唯一识别码
        /// </summary>
        public string UniqueCode { get; set; }
    }

    /// <summary>
    /// 工单的一些基础信息
    /// </summary>
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
        /// 任务单编号，即料单编号
        /// </summary>
        public string tasklistNo { get; set; }
        /// <summary>
        /// 任务单名称，即料单名称
        /// </summary>
        public string tasklistName { get; set; }
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
        /// <summary>
        /// 套丝设置，按照4条套丝线，各自规格直径设置
        /// </summary>
        public string taosiSetting { get; set; }
    }

    public class WorkBillRequest
    {
        public WorkBillRequest()
        {
            this.Msgtype = 0;
        }
        public int Msgtype { get; set; }
    }
    /// <summary>
    /// 磁吸上料的工单格式
    /// </summary>
    public class WorkBill_CX
    {
        public WorkBill_CX()
        {
            this.Msgtype = 1;
            this.BillNo = "";
            this.ProjectName = "";
            this.Block = "";
            this.Building = "";
            this.Floor = "";
            this.Level = "";
            this.Brand = "";
            this.Specification = "";
            this.OriginalLength = 0;
            this.LiaoCangList = new List<WorkBill_CX_LiaoCang>();
        }
        /// <summary>
        /// 数据类型：
        /// 9:请求工单
        /// 10:下发磁吸上料工单;
        /// </summary>
        public int Msgtype { get; set; }
        /// <summary>
        ///         L_20230628_1_008_003
        ///                 字段代表含义：
        ///                 工单类型_日期_班次_批次总数_批次流水号
        ///                 **** 注意：工单类型的定义如下：
        ///                 A ——梁板线；
        ///                 B ——墙柱线；
        ///                 C1——弯曲线；
        ///                 C2——五机头；
        ///                 C3——弯箍机；
        ///                 R ——人工线；
        ///                 L ——磁吸上料；
        /// </summary>
        public string BillNo { get; set; }
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
        /// 钢筋厂商
        /// </summary>
        public string Brand { get; set; }
        /// <summary>
        /// 钢筋规格，例如：HRB400
        /// </summary>
        public string Specification { get; set; }
        /// <summary>
        /// 原材长度，一般9米或12米两种
        /// </summary>
        public int OriginalLength { get; set; }

        public List<WorkBill_CX_LiaoCang> LiaoCangList { get; set; }
    }
    public class WorkBill_CX_LiaoCang
    { 
        public WorkBill_CX_LiaoCang() 
        {
        }
        /// <summary>
        /// 料仓位置，1：上层料仓，2：下层料仓
        /// </summary>
        public int WarePos {  get; set; }
        /// <summary>
        /// 料仓编号，值范围：1~15，对应每层15个料仓
        /// </summary>
        public int WareNo { get; set; }
        /// <summary>
        /// 料仓前半段，直径
        /// </summary>
        public int Part1_diameter {  get; set; }
        /// <summary>
        /// 料仓前半段，长度
        /// </summary>
        public int Part1_length { get; set; }
        /// <summary>
        /// 料仓前半段，数量
        /// </summary>
        public int Part1_num { get; set; }
        /// <summary>
        /// 料仓后半段，直径
        /// </summary>
        public int Part2_diameter { get;set; }
        /// <summary>
        /// 料仓后半段，长度
        /// </summary>
        public int Part2_length { get; set; }
        /// <summary>
        /// 料仓后半段，数量
        /// </summary>
        public int Part2_num { get; set; }
    }

    /// <summary>
    /// 梁板线的生产工单数据格式，以单根原材为数据单元
    /// </summary>
    public class WorkBill_LB
    {
        public WorkBill_LB()
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
            this.TaosiSetting = "";
            this.SteelbarList = new List<WorkBill_LB_SingleRebar>();
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
        /// 任务单编号，即料单编号
        /// </summary>
        public string TasklistNo { get; set; }
        /// <summary>
        /// 任务单名称，即料单名称
        /// </summary>
        public string TasklistName {  get; set; }   
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
        /// <summary>
        /// 套丝机直径设置
        /// </summary>
        public string TaosiSetting { get; set; }

        public List<WorkBill_LB_SingleRebar> SteelbarList { get; set; }
    }
    /// <summary>
    /// 墙柱线批量锯切的工单数据格式，第一层级
    /// </summary>
    public class WorkBill_QZ
    {
        public WorkBill_QZ()
        {
            this.Msgtype = 1;
            this.BillNo = "";
            this.TasklistNo = "";
            this.TasklistName = "";
            this.ProjectName = "";
            this.Block = "";
            this.Building = "";
            this.Floor = "";
            this.Level = "";
            this.Brand = "";
            this.Specification = "";
            this.OriginalLength = 0;
            this.CuttingList = new List<WorkBill_QZ_PiCutDiameter>();
        }
        /// <summary>
        /// 数据类型：
        /// 1:请求工单
        /// 3:墙柱线下发工单;
        /// </summary>
        public int Msgtype { get; set; }
        /// <summary>
        /// B_20230628_1_008_003
        ///         字段代表含义：
        ///         工单类型_日期_班次_批次总数_批次流水号
        ///         **** 注意：工单类型的定义如下：
        ///         A ——梁板线；
        ///         B ——墙柱线；
        ///         C1——弯曲线；
        ///         C2——五机头；
        ///         C3——弯箍机；
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 任务单号，即料单编号
        /// </summary>
        public string TasklistNo {  get; set; }
        /// <summary>
        /// 任务单名称，即料单名称
        /// </summary>
        public string TasklistName { get; set; }
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
        /// <summary>
        /// 原材数量
        /// </summary>
        public int OriginalNum { get; set; }
        /// <summary>
        /// 钢筋数量
        /// </summary>
        public int SteelbarNum { get;  set; }
        /// <summary>
        /// 原材重量
        /// </summary>
        public double OriginalWeight {  get; set; }
        /// <summary>
        /// 钢筋重量
        /// </summary>
        public double SteelbarWeight { get; set; }
        /// <summary>
        /// 锯切后用途，1：梁板线；2：墙柱线
        /// </summary>
        public int CuttingUse { get; set; }

        public List<WorkBill_QZ_PiCutDiameter> CuttingList { get; set; }

    }
    /// <summary>
    /// 子加工批（按不同直径区分）数据格式，第二层级
    /// </summary>
    public class WorkBill_QZ_PiCutDiameter
    {
        public  WorkBill_QZ_PiCutDiameter() {
            this.SeriNo = "";
            this.Level = "C";
            this.Diameter = 0;
            this.OriginalLength= 0;
            this.Num = 0;
            this.TaosiSetting = "";
            this.SolutionList = new List<WorkBill_QZ_PiCutSolution>();
        }
        /// <summary>
        /// 索引号： B_20230628_1_008_003-005_001
        /// 字段代表含义：
        /// 工单类型_日期_班次_批次总数_批次流水号-子批数_当前子批流水号
        /// **** 注意：
        /// 1、工单类型的定义如下：
        /// A ——梁板线；
        /// B ——墙柱线；
        /// C1——弯曲线；
        /// C2——五机头；
        /// C3——弯箍机；
        /// R ——人工线；
        /// 2、子批代表的是不同直径的钢筋，一种直径就是一个子批
        /// </summary>
        public string SeriNo {  get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 直径尺寸
        /// </summary>
        public int Diameter {  get; set; }
        /// <summary>
        /// 原材长度
        /// </summary>
        public int OriginalLength {  get; set; }
        /// <summary>
        /// 当前子加工批所有锯切方案需要的钢筋原材总数量之和
        /// </summary>
        public int Num {  get; set; }
        /// <summary>
        /// 原材数量
        /// </summary>
        public int OriginalNum { get; set; }
        /// <summary>
        /// 钢筋数量
        /// </summary>
        public int SteelbarNum { get; set; }
        /// <summary>
        /// 原材重量
        /// </summary>
        public double OriginalWeight { get; set; }
        /// <summary>
        /// 钢筋重量
        /// </summary>
        public double SteelbarWeight { get; set; }
        /// <summary>
        /// 18_18_18_18_20_20
        /// 代表1 ~6号套丝机分别设置为ø16直径、ø18直径、ø22直径、ø25直径；
        /// 注意：
        /// 1、一般在每一个批次生产完成后，进行套丝机的设置
        /// 2、根据批次当前直径的数量，设置套丝机的机头，例如数量较大，可将全部套丝机设置为当前直径，如果数量较小，可只设置1 ~2台
        /// </summary>
        public string TaosiSetting {  get; set; }  
        /// <summary>
        /// 所有锯切方案
        /// </summary>
        public List<WorkBill_QZ_PiCutSolution> SolutionList { get; set; }

    }
    /// <summary>
    /// 批量锯切的钢筋数据格式，第三层级
    /// </summary>
    public class WorkBill_QZ_PiCutSolution
    {
        public WorkBill_QZ_PiCutSolution(){
            this.SeriNo = "";
            this.OriginalLength = 0;
            this.Num= 0;
            this.RebarList = new List<WorkBill_QZ_PiCutRebar>();
        }
        /// <summary>
        /// B_20230628_1_001_001-004_001-005_002
        /// 字段代表含义：
        /// 工单类型_日期_班次_批次总数_批次流水号-子批总数_当前子批-锯切方案总数_当前锯切方案
        /// </summary>
        public string SeriNo { get; set; }
        /// <summary>
        /// 原材长度
        /// </summary>
        public int OriginalLength { get; set; }
        /// <summary>
        /// 原材数量
        /// </summary>
        public int OriginalNum { get; set; }
        /// <summary>
        /// 钢筋数量
        /// </summary>
        public int SteelbarNum { get; set; }
        /// <summary>
        /// 原材重量
        /// </summary>
        public double OriginalWeight { get; set; }
        /// <summary>
        /// 钢筋重量
        /// </summary>
        public double SteelbarWeight { get; set; }
        /// <summary>
        /// 将钢筋简图转化为base64编码，传输给pcs
        /// </summary>
        public string PicString { get;set; }
        /// <summary>
        /// 当前锯切方案需要的钢筋原材数量
        /// </summary>
        public int Num { get; set; }

        public List<WorkBill_QZ_PiCutRebar> RebarList { get; set; }

    }

    public class WorkBill_Bend_LB
    {
        public WorkBill_Bend_LB()
        {
            this.MsgType = 8;
            this.SteelbarNo=string.Empty;
            this.ElementNo = string.Empty;
            this.Diameter = 0;
            this.Length = 0;
            this.Weight = 0.0;
            this.IndexCode=string.Empty;
            this.PicNo = string.Empty;
            this.CornerMsg = string.Empty;
            this.Num = 0;
        }
        /// <summary>
        /// 梁板线：7:弯曲机器人请求工单；8:弯曲机器人下发工单;
        /// 墙柱线：11：墙柱成捆弯曲请求工单；12：墙柱成捆弯曲下发工单
        /// </summary>
        public int MsgType {  get; set; }
        /// <summary>
        /// 钢筋在数据库中主key唯一码
        /// </summary>
        public string SteelbarNo { get; set; }
        /// <summary>
        /// 构件包数据库中主key唯一码
        /// </summary>
        public string ElementNo {  get; set; }
        /// <summary>
        /// 直径
        /// </summary>
        public int Diameter { get; set; }
        /// <summary>
        /// 长度
        /// </summary>
        public int Length { get;set; }
        /// <summary>
        /// 重量
        /// </summary>
        public double Weight { get; set; }
        /// <summary>
        /// 激光打标编码
        /// 1、激光打标使用此参数进行打标；
        /// 2、弯曲线视觉扫码时，读取此信息进入数据库进行检索，调出对应的弯曲加工信息
        /// </summary>
        public string IndexCode { get; set; }
        /// <summary>
        /// 加工图形编号
        /// </summary>
        public string PicNo { get; set; }
        /// <summary>
        /// 边角信息
        /// </summary>
        public string CornerMsg { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
    }

    /// <summary>
    /// 批量锯切的钢筋数据格式
    /// </summary>
    public class WorkBill_QZ_PiCutRebar
    {
        public WorkBill_QZ_PiCutRebar()
        {
            this.SeriNo = "";
            this.CornerMsg = "";
            this.Length = 0;
            this.TaosiType = 0;
            this.BendType = false;
        }

        /// <summary>
        /// 20230628_1_098_003_123_065_01
        /// 字段代表含义：
        /// 日期_班次_批次总数_批次流水号_当前批钢筋原材总数_钢筋原材流水号_当前小段的流水号
        /// </summary>
        public string SeriNo { get; set; }
        /// <summary>
        /// 边角结构,
        /// </summary>
        public string CornerMsg { get; set; }

        /// <summary>
        /// 下料长度
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 钢筋重量
        /// </summary>
        public double SteelbarWeight { get; set; }

        /// <summary>
        /// 套丝类型，0：不套丝；1：单头套丝；2：双头套丝
        /// </summary>
        public int TaosiType { get; set; }
        /// <summary>
        /// 弯曲类型，false：不弯曲；true：弯曲
        /// </summary>
        public bool BendType {  get; set; }
        /// <summary>
        /// 料单上对每个钢筋零件都有唯一识别码
        /// </summary>
        public string UniqueCode { get; set; }

        ///// <summary>
        ///// 数量
        ///// </summary>
        //public int Num { get; set; }
        ///// <summary>
        ///// 重量
        ///// </summary>
        //public double Weight { get; set; }
    }




}
