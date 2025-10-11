using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebarSampling
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum EnumDatabaseType
    {
        /// <summary>
        /// sqlite
        /// </summary>
        SQLITE,
        /// <summary>
        /// mysql
        /// </summary>
        MYSQL,

        maxDBtype
    }

    /// <summary>
    /// 区分线材棒材钢筋
    /// </summary>
    public enum EnumRebarSizeType
    {
        /// <summary>
        /// 棒材钢筋
        /// </summary>
        BANG,
        /// <summary>
        /// 线材钢筋
        /// </summary>
        XIAN
    }
    /// <summary>
    /// 所有的钢筋构型图形编号索引
    /// </summary>
    public enum EnumRebarPicType
    {
        T_10000 = 0,
        T_10001,
        T_10002,
        T_10100,
        T_10200,
        T_10400,
        T_10500,
        T_12121,
        T_12222,
        T_13100,
        T_13300,
        T_15070,
        T_15080,

        T_20000,
        T_20001,
        T_20002,
        T_20003,
        T_20004,
        T_20007,
        T_20018,
        T_20100,
        T_20101,
        T_20105,
        T_20121,
        T_20200,
        T_20202,
        T_20300,
        T_20400,
        T_20501,
        T_20600,
        T_20808,

        T_21800,

        T_22101,
        T_22123,
        T_22125,
        T_22224,
        T_22226,
        T_22321,
        T_22422,
        T_22521,
        T_22622,

        T_25010,
        T_25020,
        T_25030,
        T_25040,
        T_25050,
        T_25060,
        T_25070,
        T_25080,
        T_25090,
        T_25100,
        T_25110,
        T_25120,
        T_25130,
        T_25140,
        T_25150,
        T_25160,
        T_25170,
        T_25180,
        T_25190,
        T_25200,
        T_25210,


        T_30500,
        T_30600,
        T_30700,
        T_30800,
        T_31100,
        T_31200,
        T_31300,
        T_31400,
        T_30005,
        T_30006,
        T_30007,
        T_30008,
        T_30011,
        T_30012,
        T_30013,
        T_30014,
        T_30101,
        T_30202,
        T_30303,
        T_30404,
        T_30102,
        T_30201,
        T_30103,
        T_30104,
        T_30203,
        T_30204,
        T_30301,
        T_30302,
        T_30304,
        T_30403,
        T_30401,
        T_30402,
        T_32323,
        T_32424,
        T_32525,
        T_32626,
        T_32324,
        T_32423,
        T_32325,
        T_32326,
        T_32523,
        T_32623,
        T_32425,
        T_32426,
        T_32524,
        T_32624,
        T_32526,
        T_32625,
        T_35010,
        T_35020,
        T_35030,
        T_35040,
        T_35050,
        T_35060,
        T_35070,
        T_35080,
        T_35090,
        T_35100,
        T_35110,
        T_35120,
        T_35130,
        T_35140,
        T_35150,
        T_35160,
        T_35170,
        T_35180,
        T_35190,
        T_35200,
        T_35210,
        T_35220,
        T_35230,
        T_35240,
        T_35250,
        T_35260,
        T_35270,
        T_35310,
        T_35290,
        T_35300,
        T_35280,
        T_35320,
        T_35330,
        T_35340,
        T_35350,
        T_35360,
        T_31818,
        T_38120,

        T_40900,
        T_41500,
        T_41600,
        T_40009,
        T_40015,
        T_40016,
        T_40061,
        T_40101,
        T_40105,
        T_40106,
        T_40107,
        T_40108,
        T_41101,
        T_40111,
        T_40205,
        T_40206,
        T_40207,
        T_40208,
        T_40501,
        T_40502,
        T_40601,
        T_40602,
        T_40305,
        T_40406,
        T_40503,
        T_40604,
        T_40701,
        T_40702,
        T_40801,
        T_40802,
        T_40412,
        T_40212,
        T_45010,
        T_45020,
        T_45030,
        T_45040,
        T_45050,
        T_45060,
        T_45070,
        T_45080,
        T_45090,
        T_45100,
        T_45110,
        T_45120,
        T_45130,
        T_45140,
        T_45150,
        T_45160,
        T_45170,
        T_45180,
        T_45190,
        T_45200,
        T_45210,
        T_45220,
        T_45230,
        T_45240,
        T_45250,
        T_45260,
        T_45270,
        T_40804,
        T_40408,
        T_40113,
        T_46100,

        T_51000,
        T_50010,
        T_50261,
        T_50505,
        T_50606,
        T_50707,
        T_50711,
        T_50808,
        T_51107,
        T_51111,
        T_51212,
        T_51313,
        T_51414,
        T_51112,
        T_50109,
        T_50901,
        T_50506,
        T_50605,
        T_50708,
        T_50807,
        T_50416,
        T_50461,
        T_50612,
        T_51208,
        T_55010,
        T_55020,
        T_55030,
        T_51700,
        T_50017,
        T_50806,
        T_50608,

        T_60909,
        T_61010,
        T_61515,
        T_61616,
        T_61001,
        T_60110,
        T_60616,
        T_60661,
        T_62020,
        T_62021,
        T_62022,
        T_62023,
        T_61717,
        T_65151,
        T_66106,
        T_66161,

        T_70000,
        T_75001,
        T_75020,
        T_74200,
        T_74201,
        T_74202,
        T_74203,
        T_74204,
        T_74205,
        T_74206,
        T_74207,
        T_74208,
        T_74209,
        T_74210,
        T_74211,
        T_74212,
        T_74213,
        T_74214,
        T_74215,
        T_74216,
        T_74220,
        T_74221,
        T_74222,
        T_74223,
        T_76006,
        T_76007,
        T_73007,
        T_73206,
        T_73008,
        T_73202,
        T_75003,
        T_75201,
        T_75004,
        T_75202,
        T_71208,
        T_71005,
        T_72206,
        T_72007,
        T_72207,
        T_72008,
        T_73207,
        T_73017,
        T_73101,
        T_73018,
        T_73208,
        T_73019,
        T_73211,
        T_73027,
        T_73203,
        T_74013,
        T_73204,
        T_74014,
        T_74101,
        T_74009,
        T_73102,
        T_73020,
        T_73103,
        T_73021,
        T_73209,
        T_73022,
        T_72208,
        T_72009,
        T_71004,
        T_71207,
        T_72005,
        T_72201,
        T_72006,
        T_72202,
        T_72203,
        T_73005,
        T_73006,
        T_75018,
        T_75019,
        T_75203,
        T_73210,
        T_75204,
        T_74217,
        T_73028,
        T_74021,
        T_74224,

        T_84221,
        T_89901,

        T_90000,
        T_95000,
        maxRebarTypeNum
    }

    /// <summary>
    /// 主构件类型
    /// </summary>
    public enum EnumRebarAssemblyType
    {
        NONE = -1,
        /// <summary>
        /// 基础，地下室
        /// </summary>
        ASSEMBLY_BASE = 0,
        /// <summary>
        /// 墙
        /// </summary>
        ASSEMBLY_WALL,
        /// <summary>
        /// 柱
        /// </summary>
        ASSEMBLY_PILLAR,
        /// <summary>
        /// 梁
        /// </summary>
        ASSEMBLY_BEAM,
        /// <summary>
        /// 板
        /// </summary>
        ASSEMBLY_FLOOR,
        /// <summary>
        /// 楼梯
        /// </summary>
        ASSEMBLY_STAIR,


        maxAssemblyNum
    }
    /// <summary>
    /// 钢筋的形状类型，包含：箍筋、拉勾、马凳、单段、多段
    /// </summary>
    public enum EnumRebarShapeType
    {
        NONE = -1,
        /// <summary>
        /// 箍筋
        /// </summary>
        SHAPE_GJ,
        /// <summary>
        /// 拉勾
        /// </summary>
        SHAPE_LG,
        /// <summary>
        /// 马凳，有两种马凳，分别以“FC”和“C”标注
        /// </summary>
        SHAPE_MD,
        /// <summary>
        /// 端头，一般为虚线显示
        /// </summary>
        SHAPE_DT,
        /// <summary>
        /// 主筋
        /// </summary>
        SHAPE_ZJ,


        maxShapeType
    }

    /// <summary>
    /// 钢筋列名枚举
    /// </summary>
    public enum EnumAllRebarTableColName
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        PROJECT_NAME = 0,
        /// <summary>
        /// 主构件部位名称
        /// </summary>
        MAIN_ASSEMBLY_NAME,   //
        /// <summary>
        /// 子构件部位名称
        /// </summary>
        CHILD_ASSEMBLY_NAME,
        /// <summary>
        /// 子构件名称
        /// </summary>
        ELEMENT_NAME,           //
        /// <summary>
        /// 料表编号
        /// </summary>
        TABLE_NO,
        /// <summary>
        /// 料表名称
        /// </summary>
        TABLE_NAME,
        /// <summary>
        /// 料表sheet名称
        /// </summary>
        TABLE_SHEET_NAME,
        /// <summary>
        /// 图形编号
        /// </summary>
        PIC_NO,              //
        /// <summary>
        /// 级别
        /// </summary>
        LEVEL,
        /// <summary>
        /// 直径
        /// </summary>
        DIAMETER,               //
        /// <summary>
        /// 钢筋简图
        /// </summary>
        REBAR_PIC,              //
        /// <summary>
        /// 图形信息
        /// </summary>
        PIC_MESSAGE,            //
        /// <summary>
        /// 边角结构
        /// </summary>
        CORNER_MESSAGE,         //
        /// <summary>
        /// 边角结构信息备份
        /// </summary>
        CORNER_MESSAGE_BK,
        /// <summary>
        /// 下料长度
        /// </summary>
        LENGTH,                 //
        /// <summary>
        /// 是否是多段
        /// </summary>
        ISMULTI,
        /// <summary>
        /// 根数件数
        /// </summary>
        PIECE_NUM_UNIT_NUM,     //
        /// <summary>
        /// 总根数
        /// </summary>
        TOTAL_PIECE_NUM,        //
        /// <summary>
        /// 重量
        /// </summary>
        TOTAL_WEIGHT,           //
        /// <summary>
        /// 备注
        /// </summary>
        DESCRIPTION,            //
        /// <summary>
        /// 标注序号
        /// </summary>
        SERIALNUM,               //
        /// <summary>
        /// 是否原材
        /// </summary>
        ISORIGINAL,
        /// <summary>
        /// 是否套丝
        /// </summary>
        IFTAO,
        /// <summary>
        /// 是否弯曲
        /// </summary>
        IFBEND,
        /// <summary>
        /// 是否切断
        /// </summary>
        IFCUT,
        /// <summary>
        /// 是否弯曲两次以上
        /// </summary>
        IFBENDTWICE,
        /// <summary>
        /// 广联达料单钢筋类型
        /// </summary>
        BAR_TYPE,
        /// <summary>
        /// 广联达料单制造类型
        /// </summary>
        FABRICATION_TYPE,

        /// <summary>
        /// 是否选择，在主界面是否选择纳入筛选
        /// </summary>
        IFPICK,

        /// <summary>
        /// 梁板线构件批的生产批号
        /// </summary>
        WORK_LB__ELEMENTBATCH_SERI,
        /// <summary>
        /// 梁板线的料仓号
        /// </summary>
        WAREHOUSE_NO,
        /// <summary>
        /// 梁板线的仓位号
        /// </summary>
        WARE_NO,
        /// <summary>
        /// 梁板线的智能料仓设置，8421四种设置
        /// </summary>
        WAREHOUSE_SET,
        /// <summary>
        /// 构件的临时编号
        /// </summary>
        TEMP_ELEMENT_SERI,
        /// <summary>
        /// 套丝机设置
        /// </summary>
        TAOSI_SET,

        /// <summary>
        /// 批量锯切的生产批号
        /// </summary>
        WORK_QZ_PICUT_SERI,
        /// <summary>
        /// 批量锯切的数量
        /// </summary>
        NUM,
        /// <summary>
        /// 批量锯切一捆的临时编号
        /// </summary>
        TEMP_NO,
        /// <summary>
        /// 是否在产线加工
        /// </summary>
        IFMAKE_IN_LINE,
        /// <summary>
        /// 是否勾选进入加工单
        /// </summary>
        IFPICK_IN_BILL,
        /// <summary>
        /// 是否下发pcs
        /// </summary>
        IFSEND_TO_PCS,
        /// <summary>
        /// 是否加工完成
        /// </summary>
        IFMAKE_DONE,

        maxRebarColNum
    }

    /// <summary>
    /// 图形汇总界面的列名枚举
    /// </summary>
    public enum EnumAllPicTableColName
    {
        /// <summary>
        /// 钢筋简图编号
        /// </summary>
        TYPE_NUM = 0,
        /// <summary>
        /// 钢筋简图
        /// </summary>
        PIC,
        /// <summary>
        /// 数量，根数
        /// </summary>
        NUM,
        /// <summary>
        /// 重量
        /// </summary>
        WEIGHT,

        ///// <summary>
        ///// 最大长度
        ///// </summary>
        //MAX_LENGTH,
        ///// <summary>
        ///// 最小长度
        ///// </summary>
        //MIN_LENGTH,

        /// <summary>
        /// 说明
        /// </summary>
        DESCRIPTION,

        maxAllPicColNum
    }

    /// <summary>
    /// 待分析的项
    /// </summary>
    public enum EnumDetailItem
    {
        /// <summary>
        /// 总数量，根数
        /// </summary>
        TOTAL_PIECE,
        /// <summary>
        /// 总重量（kg）
        /// </summary>
        TOTAL_WEIGHT,
        /// <summary>
        /// 总长度（m）
        /// </summary>
        TOTAL_LENGTH,
        /// <summary>
        /// 套丝数量，个
        /// </summary>
        TAO_SI_NUM,
        /// <summary>
        /// 套筒数量，个
        /// </summary>
        TAO_TONG_NUM,
        /// <summary>
        /// 正丝套筒数量，个
        /// </summary>
        ZHENG_SI_TAO_TONG,
        /// <summary>
        /// 反丝套筒数量，个
        /// </summary>
        FAN_SI_TAO_TONG,
        /// <summary>
        /// 变径套筒数量，个
        /// </summary>
        BIAN_JING_TAO_TONG,
        /// <summary>
        /// 切断总次数，次
        /// </summary>
        CUT_NUM,
        /// <summary>
        /// 弯曲总次数，次
        /// </summary>
        BEND_NUM,
        /// <summary>
        /// 调直长度，m
        /// </summary>
        ZHI_NUM,

        maxItemNum
    }

    /// <summary>
    /// 工艺类型
    /// </summary>
    public enum EnumWorkType
    {
        /// <summary>
        /// 不弯，不套
        /// </summary>
        NO_BEND_NO_TAO,
        /// <summary>
        /// 不弯，套丝
        /// </summary>
        NO_BEND_YES_TAO,
        /// <summary>
        /// 弯曲，不套
        /// </summary>
        YES_BEND_NO_TAO,
        /// <summary>
        /// 弯曲，套丝
        /// </summary>
        YES_BEND_YES_TAO,

        maxWorkType

    }
    /// <summary>
    /// 详细统计界面的列名枚举,加工工艺
    /// </summary>
    public enum EnumDetailTableColName
    {
        /// <summary>
        /// 原材，啥事不干
        /// </summary>
        ORIGINAL = 0,
        /// <summary>
        /// 原材仅套丝
        /// </summary>
        ONLY_TAO,
        /// <summary>
        /// 原材仅弯曲
        /// </summary>
        ONLY_BEND,
        /// <summary>
        /// 仅切断
        /// </summary>
        ONLY_CUT,
        /// <summary>
        /// 切断+套丝
        /// </summary>
        CUT_TAO,
        /// <summary>
        /// 切断+弯曲少于2次
        /// </summary>
        CUT_BEND_NOT_2,
        /// <summary>
        /// 切断+弯曲大于2次
        /// </summary>
        CUT_BEND_OVER_2,
        /// <summary>
        /// 切断+套丝+弯曲
        /// </summary>
        CUT_TAO_BEND,


        maxColNum

    }

    public enum EnumDiameter
    {
        DIAMETER_6,
        DIAMETER_8,
        DIAMETER_10,
        DIAMETER_12,
        DIAMETER_14,
        DIAMETER_16,
        DIAMETER_18,
        DIAMETER_20,
        DIAMETER_22,
        DIAMETER_25,
        DIAMETER_28,
        DIAMETER_32,
        DIAMETER_36,
        DIAMETER_40,

        maxDiameterNum

    }

    /// <summary>
    /// 详细统计界面的行名，钢筋尺寸
    /// </summary>
    public enum EnumBangOrXian
    {
        XIAN_A6,
        XIAN_A8,
        XIAN_C6,
        XIAN_C8,
        XIAN_C10,
        XIAN_C12,
        BANG_C14,
        BANG_C16,
        BANG_C18,
        BANG_C20,
        BANG_C22,
        BANG_C25,
        BANG_C28,
        BANG_C32,
        BANG_C36,
        BANG_C40,

        maxRowNum
    }
    /// <summary>
    /// 线材直径规格
    /// </summary>
    public enum EnumDiaXian
    {
        XIAN_A6,
        XIAN_A8,
        XIAN_C6,
        XIAN_C8,
        XIAN_C10,
        //XIAN_C12,

        maxRebarXianNum
    }
    /// <summary>
    /// 棒材直径规格
    /// </summary>
    public enum EnumDiaBang
    {
        NONE = -1,
        BANG_C12,
        BANG_C14,
        BANG_C16,
        BANG_C18,
        BANG_C20,
        BANG_C22,
        BANG_C25,
        BANG_C28,
        BANG_C32,
        BANG_C36,
        BANG_C40,

        maxRebarBangNum
    }


    /// <summary>
    /// 钢筋螺距区间，Φ16~Φ22用2.5螺距，Φ25~Φ32用3.0螺距，Φ36~Φ40用3.5螺距
    /// </summary>
    public enum EnumDiameterPitchType
    {
        NONE = -1,
        PITCH_1,
        PITCH_2,
        PITCH_3,
        PITCH_12,
        PITCH_13,
        PITCH_23,
        PITCH_123,

        maxPitchType
    }

    /// <summary>
    /// 钢筋数量分组，原则：WARESET_8:1~25(8仓)，WARESET_4:26~50(4仓)，WARESET_2:51~100(2仓)，WARESET_1:100~(1仓)
    /// </summary>
    public enum EnumWareNumSet
    {
        NONE = -1,
        /// <summary>
        /// 1~15(12包，1仓一包)
        /// </summary>
        WARESET_12,
        /// <summary>
        /// 16~50(6包，2仓一包)
        /// </summary>
        WARESET_6,
        /// <summary>
        /// 51~100(3包，4仓一包)
        /// </summary>
        WARESET_3,
        /// <summary>
        /// 100~(2包，6仓一包)
        /// </summary>
        WARESET_2,
        /// <summary>
        /// 100~(1包，12仓一包)
        /// </summary>
        WARESET_1,

        maxNum
    }
    /// <summary>
    /// 1~4种直径，多种直径,1种直径，2种直径，3种直径，4种直径,
    /// 注意与tabcontrol2 的tab顺序对应
    /// </summary>
    public enum EnumDiaGroupType
    {
        NONE = -1,
        /// <summary>
        /// 1~4种直径
        /// </summary>
        FEW,
        /// <summary>
        /// 多种直径
        /// </summary>
        MULTI,
        /// <summary>
        /// 一种直径
        /// </summary>
        ONE,
        /// <summary>
        /// 两种直径
        /// </summary>
        TWO,
        /// <summary>
        /// 三种直径
        /// </summary>
        THREE,
        /// <summary>
        /// 四种直径
        /// </summary>
        FOUR,


        maxDiameterType
    }


    /// <summary>
    /// 多段接头类型，接头包含:"原头、弯、套、变径套、反套、丝、反丝、搭、单、双、对、竖",
    /// </summary>
    public enum EnumMultiHeadType
    {
        NONE = -1,
        /// <summary>
        /// 原始端头
        /// </summary>
        ORG = 0,
        /// <summary>
        /// 弯
        /// </summary>
        BEND,
        /// <summary>
        /// 正套
        /// </summary>
        TAO_P,
        /// <summary>
        /// 变径套
        /// </summary>
        TAO_V,
        /// <summary>
        /// 反丝套
        /// </summary>
        TAO_N,
        /// <summary>
        /// 正丝
        /// </summary>
        SI_P,
        /// <summary>
        /// 反丝
        /// </summary>
        SI_N,
        /// <summary>
        /// 搭接
        /// </summary>
        DA,
        /// <summary>
        /// 单
        /// </summary>
        SINGLE,
        /// <summary>
        /// 双
        /// </summary>
        DOUBLE,
        /// <summary>
        /// 对
        /// </summary>
        PAIR,
        /// <summary>
        /// 竖
        /// </summary>
        SHU,
        /// <summary>
        /// 圆弧
        /// </summary>
        ARC
    }

    public enum EnumFactoryType
    {
        NONE = -1,
        /// <summary>
        /// 20240125修改：低配工厂，2条套丝线，2个辊道成品料仓
        /// **标配工厂，4条套丝线，6个辊道成品仓
        /// </summary>
        LowConfig,
        /// <summary>
        /// 20240125修改：高配工厂，4条套丝线，4条辊道成品料仓
        /// **低配工厂，2条套丝线，2个辊道成品仓
        /// </summary>
        HighConfig,
        /// <summary>
        /// 实验线，1条套丝线，1个辊道成品仓
        /// </summary>
        Experiment,

        maxFactoryType
    }

    public enum EnumLanguageType
    {
        NONE=-1,
        Chinese,
        English,

        maxLanguageType
    }
    /// <summary>
    /// 料单类型
    /// </summary>
    public enum EnumMaterialBill
    {
        NONE=-1,
        /// <summary>
        /// e筋料单
        /// </summary>
        EJIN,
        /// <summary>
        /// 广联达料单
        /// </summary>
        GLD,

        maxMaterialBillType
    }

    public enum EnumFactory
    {
        NONE = -1,
        /// <summary>
        /// 高效工厂
        /// </summary>
        GaoXiao,
        /// <summary>
        /// 柔性工厂
        /// </summary>
        RouXing,

        maxFactory
    }
    /// <summary>
    /// 原材类型，9米还是12米
    /// </summary>
    public enum EnumOriType
    {
        NONE = -1,
        /// <summary>
        /// 9米原材
        /// </summary>
        ORI_9,
        /// <summary>
        /// 12米原材
        /// </summary>
        ORI_12,

        maxOriType
    }
    /// <summary>
    /// 配置文件中的直径种类分组类型，顺序分组(1~4)、混合分组(1/2/3/4)
    /// </summary>
    public enum EnumDiaGroupTypeSetting
    {
        NONE = -1,
        /// <summary>
        /// 顺序分组
        /// </summary>
        Sequence,
        /// <summary>
        /// 混合分组
        /// </summary>
        Mix,

        maxDiaGroupType
    }

    public enum EnumTaoType
    {
        NONE = -1,
        /// <summary>
        /// 原材套料
        /// </summary>
        ORITAO,
        /// <summary>
        /// 批量锯切套料1.0
        /// </summary>
        ORIPOOLTAO,
        /// <summary>
        /// 批量锯切套料2.0
        /// </summary>
        CUTTAO_2,
        /// <summary>
        /// 批量锯切套料3.0，二叉树
        /// </summary>
        CUTTAO_3,


        maxTaoType
    }
    /// <summary>
    /// 原材库设置
    /// </summary>
    public enum EnumMatPoolSetType
    {
        NONE = -1,
        /// <summary>
        /// 整数倍模数
        /// </summary>
        INTEGER,
        /// <summary>
        /// 平均等间距模数
        /// </summary>
        AVERAGE,

        maxMatPoolSetType
    }

    public enum EnumLabelType
    {
        NONE = -1,
        /// <summary>
        /// 墙柱线标签
        /// </summary>
        QZ_LABEL,
        /// <summary>
        /// 梁板线标签
        /// </summary>
        LB_LABEL,
        /// <summary>
        /// 构件标
        /// </summary>
        ELEMENT_LABEL,
        /// <summary>
        /// 零件标，即一行钢筋一个标
        /// </summary>
        REBAR_LABEL,

        maxLabelType
    }

}
