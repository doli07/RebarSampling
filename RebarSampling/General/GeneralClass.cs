using RebarSampling.GeneralWorkBill;
using RebarSampling.labelPrint;
using RebarSampling.log;
//using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RebarSampling
{

    public static partial class GeneralClass
    {
        /// <summary>
        /// 操作读写excel文件
        /// </summary>
        public static ExcelReader ExcelOpt = new ExcelReader();
        /// <summary>
        /// 操作解析E筋格式文件
        /// 通过E筋公司提供的ETable.dll实现
        /// </summary>
        //public static EjinReader readEjin = new EjinReader();
        /// <summary>
        /// 操作json序列化与反序列化
        /// </summary>
        //public static json JsonOpt = new json();
        public static NewtonJson JsonOpt = new NewtonJson();
        /// <summary>
        /// 操作数据库文件读写，20240805修改，不在此处初始化，而通过配置文件，确定用哪种数据库进行初始化
        /// </summary>
        public static DBOpt DBOpt /*= new DBOpt()*/;
        /// <summary>
        /// 操作工单
        /// </summary>
        public static WorkBillOpt WorkBillOpt = new WorkBillOpt();
        /// <summary>
        /// 打印标签的宽度
        /// </summary>
        public static int LabelPrintSizeWidth = 50;
        /// <summary>
        /// 打印标签的高度
        /// </summary>
        public static int LabelPrintSizeHeight = 130;

        /// <summary>
        /// bartender标签打印
        /// </summary>
        //public static LabelPrintBartender LabelPrintOpt = new LabelPrintBartender();

        /// <summary>
        /// 弯曲机器人所需的json工单
        /// </summary>
        public static List<string> jsonList_bend = new List<string>();//
        /// <summary>
        /// 梁板线，json格式的钢筋数据的list
        /// </summary>
        public static List<string> jsonList_LB = new List<string>();//生成的jsonstr的集合
        /// <summary>
        /// 梁板线磁吸上料的json数据
        /// </summary>
        public static string json_CX;
        /// <summary>
        /// 墙柱线的json格式数据
        /// </summary>
        public static string jsonList_QZ;
        /// <summary>
        /// 批量锯切的json格式数据
        /// </summary>
        public static string jsonList_PiCut;
        /// <summary>
        /// 使用queue队列写日志
        /// </summary>
        //public static queueLogger quelogger = new queueLogger();

        /// <summary>
        /// 所有钢筋数据总的list
        /// </summary>
        public static List<RebarData> AllRebarList = new List<RebarData>();
        /// <summary>
        /// 所有构件数据总的list
        /// </summary>
        public static List<ElementData> AllElementList = new List<ElementData>();

        public static WebServer webServer = new WebServer();
        public static WebClient webClient = new WebClient();

        public static MqttServerOpt mqttServer = new MqttServerOpt();
        public static MqttClientOpt mqttClient = new MqttClientOpt();
        /// <summary>
        /// detail分析用的三维数组，尺寸*工艺*分析项
        /// </summary>
        public static object[,,] AllDetailData = new object[(int)EnumDetailTableColName.ONLY_CUT, (int)EnumBangOrXian.maxRowNum, (int)EnumDetailItem.maxItemNum];//先处理三个原材的
        //public static object[,,] AllDetailData = new object[(int)EnumDetailTableRowName.maxRowNum, (int)EnumDetailTableColName.ONLY_CUT, (int)EnumDetailItem.maxItemNum];//先处理三个原材的

        /// <summary>
        /// detail分析除了原材以外的所有图形
        /// </summary>
        public static List<KeyValuePair<EnumRebarPicType, GeneralDetailData>>[] AllDetailOtherData = new List<KeyValuePair<EnumRebarPicType, GeneralDetailData>>[(int)EnumBangOrXian.maxRowNum];
        /// <summary>
        /// 读excel时，存储已存在的rebarType，放入list中
        /// </summary>
        public static List<EnumRebarPicType> ExistRebarPicTypeList = new List<EnumRebarPicType>();

        /// <summary>
        /// 内部数据交互对象
        /// </summary>
        public static InteractivityData interactivityData = new InteractivityData();

        /// <summary>
        /// 导入所有excel后的钢筋总表，用作数据备份
        /// </summary>
        public static string TableName_AllRebarBK = "allsheet_bk";
        /// <summary>
        /// 经过筛选后的钢筋总表
        /// </summary>
        public static string TableName_AllRebar = "allsheet";
        /// <summary>
        /// 钢筋总表的筛选指引表
        /// </summary>
        public static string TableName_Pick = "allsheet_pick";
        /// <summary>
        /// 拆解到构件级别的钢筋总表
        /// </summary>
        public static string TableName_Element = "allsheet_element";
        /// <summary>
        /// 梁板线构件生产批的表
        /// </summary>
        public static string TableName_ElementBatch_LB = "allsheet_elementBatchLB";
        /// <summary>
        /// 墙柱线批量包生产批的表
        /// </summary>
        public static string TableName_PiCutBatch_QZ = "allsheet_PiCutBatchQZ";

        /// <summary>
        /// 批量锯切生产批的表
        /// </summary>
        public static string TableName_PiCutBatch = "allsheet_PiCutBatch";

        ///// <summary>
        ///// 梁板线构件包的打印临时缓存区表，绑定bartender
        ///// </summary>
        //public static string TableName_elementPrintBuff_LB = "printBuff_elementLB";
        ///// <summary>
        ///// 墙柱线零件包的打印临时缓存区表，绑定bartender
        ///// </summary>
        //public static string TableName_PiCutPrint_QZ = "printBuff_PiCutQZ";




        ///// <summary>
        ///// 二次利用的长度
        ///// </summary>
        ////public const int SecondUseLength = 1500;

        /// <summary>
        /// 9米原材
        /// </summary>
        private const int OriginalLength1 = 9000;
        /// <summary>
        /// 12米原材
        /// </summary>
        private const int OriginalLength2 = 12000;
        /// <summary>
        /// 根据钢筋级别和直径，取得其原材长度
        /// </summary>
        /// <param name="_level"></param>
        /// <param name="_diameter"></param>
        /// <returns></returns>
        public static int OriginalLength(string _level, int _diameter)
        {
            EnumDiaBang _dia = IntToEnumDiameter(_diameter);//int先转成EnumDiaBang

            if (_level == "C")//三级钢
            {
                var temp = CfgData.MaterialOriPool_3.Find(t => t._level == "C" && t._diameter == _dia);
                if (temp != null)
                {
                    int _orilength = temp._length;
                    int cuthead = CfgData.IfCutHead ? CfgData.CutHeadLength : 0;
                    return _orilength - cuthead;
                }
                else { return 0; }
            }
            else// if(_level=="D")，四级钢
            {
                var temp = CfgData.MaterialOriPool_4.Find(t => t._level == "D" && t._diameter == _dia);
                if (temp != null)
                {
                    int _orilength = temp._length;
                    int cuthead = CfgData.IfCutHead ? CfgData.CutHeadLength : 0;
                    return _orilength - cuthead;
                }
                else { return 0; }
            }
        }
        /// <summary>
        /// 重载
        /// </summary>
        /// <param name="_level"></param>
        /// <param name="_diameter"></param>
        /// <returns></returns>
        public static int OriginalLength(string _level, EnumDiaBang _diameter)
        {
            if (_level == "C")//三级钢
            {
                int _orilength = CfgData.MaterialOriPool_3.Find(t => t._level == "C" && t._diameter == _diameter)._length;
                int cuthead = CfgData.IfCutHead ? CfgData.CutHeadLength : 0;
                return _orilength - cuthead;
            }
            else// if(_level=="D")，四级钢
            {
                int _orilength = CfgData.MaterialOriPool_4.Find(t => t._level == "D" && t._diameter == _diameter)._length;
                int cuthead = CfgData.IfCutHead ? CfgData.CutHeadLength : 0;
                return _orilength - cuthead;
            }
        }
        //public static int OriginalLength
        //{
        //    get

        //    {
        //        int cuthead = CfgData.IfCutHead ? CfgData.CutHeadLength : 0;
        //        return (CfgData.OriginType == EnumOriType.ORI_9) ? OriginalLength1 - cuthead : OriginalLength2 - cuthead;
        //    }
        //}

        /// <summary>
        /// 添加默认的原材库
        /// </summary>
        public static void AddDefaultMaterial()
        {
            MaterialOri _material = new MaterialOri();
            m_MaterialPool.Clear();//先清空

            for (int i = (int)EnumDiaBang.BANG_C12; i <= (int)EnumDiaBang.BANG_C40; i++)
            {
                m_MaterialPool.Add(new MaterialOri((EnumDiaBang)i, GeneralClass.CfgData.MatPoolYuliao1, 9999, "C"));//余料利用1200
                m_MaterialPool.Add(new MaterialOri((EnumDiaBang)i, GeneralClass.CfgData.MatPoolYuliao2, 9999, "C"));//余料利用1500
                m_MaterialPool.Add(new MaterialOri((EnumDiaBang)i, GeneralClass.CfgData.MatPoolYuliao1, 9999, "D"));//余料利用1200
                m_MaterialPool.Add(new MaterialOri((EnumDiaBang)i, GeneralClass.CfgData.MatPoolYuliao2, 9999, "D"));//余料利用1500

                if (GeneralClass.CfgData.MatPoolHave3)
                {
                    m_MaterialPool.Add(new MaterialOri((EnumDiaBang)i, 3000, 9999, "C"));//3000
                    m_MaterialPool.Add(new MaterialOri((EnumDiaBang)i, 3000, 9999, "D"));//3000
                }

                if (GeneralClass.CfgData.MatPoolHave6)
                {
                    m_MaterialPool.Add(new MaterialOri((EnumDiaBang)i, 6000, 9999, "C"));//6000
                    m_MaterialPool.Add(new MaterialOri((EnumDiaBang)i, 6000, 9999, "D"));//6000
                }
            }

            m_MaterialPool.AddRange(CfgData.MaterialOriPool_3);
            m_MaterialPool.AddRange(CfgData.MaterialOriPool_4);

            //if (GeneralClass.CfgData.OriginType == EnumOriType.ORI_9)
            //{
            //    for (int i = (int)EnumDiaBang.BANG_C12; i <= (int)EnumDiaBang.BANG_C40; i++)
            //    {
            //        if (GeneralClass.CfgData.MatPoolSetType == EnumMatPoolSetType.INTEGER)//整数倍模数
            //        {
            //            _material = new MaterialOri((EnumDiaBang)i, 1800, 9999);
            //            if (!m_MaterialPool.Contains(_material)) m_MaterialPool.Add(_material);
            //            _material = new MaterialOri((EnumDiaBang)i, 2250, 9999);
            //            if (!m_MaterialPool.Contains(_material)) m_MaterialPool.Add(_material);
            //            _material = new MaterialOri((EnumDiaBang)i, 3000, 9999);
            //            if (!m_MaterialPool.Contains(_material)) m_MaterialPool.Add(_material);
            //            _material = new MaterialOri((EnumDiaBang)i, 4500, 9999);
            //            if (!m_MaterialPool.Contains(_material)) m_MaterialPool.Add(_material);
            //            _material = new MaterialOri((EnumDiaBang)i, 6000, 9999);
            //            if (!m_MaterialPool.Contains(_material)) m_MaterialPool.Add(_material);
            //            _material = new MaterialOri((EnumDiaBang)i, 7200, 9999);
            //            if (!m_MaterialPool.Contains(_material)) m_MaterialPool.Add(_material);
            //            _material = new MaterialOri((EnumDiaBang)i, 9000, 9999);
            //            if (!m_MaterialPool.Contains(_material)) m_MaterialPool.Add(_material);

            //        }
            //        else//等间距模数
            //        {
            //            _material = new MaterialOri((EnumDiaBang)i, GeneralClass.CfgData.MatPoolYuliao1, 9999);            //余料利用1200
            //            if (!m_MaterialPool.Contains(_material)) m_MaterialPool.Add(_material);
            //            _material = new MaterialOri((EnumDiaBang)i, GeneralClass.CfgData.MatPoolYuliao2, 9999);            //余料利用1500
            //            if (!m_MaterialPool.Contains(_material)) m_MaterialPool.Add(_material);

            //            if (GeneralClass.CfgData.MatPoolHave3)
            //            {
            //                _material = new MaterialOri((EnumDiaBang)i, 3000, 9999);
            //                if (!m_MaterialPool.Contains(_material)) m_MaterialPool.Add(_material);
            //            }

            //            if (GeneralClass.CfgData.MatPoolHave6)
            //            {
            //                _material = new MaterialOri((EnumDiaBang)i, 6000, 9999);
            //                if (!m_MaterialPool.Contains(_material)) m_MaterialPool.Add(_material);
            //            }

            //            _material = new MaterialOri((EnumDiaBang)i, 9000, 9999);
            //            if (!m_MaterialPool.Contains(_material)) m_MaterialPool.Add(_material);
            //        }
            //    }
            //}
            //else     //12米
            //{
            //    for (int i = (int)EnumDiaBang.BANG_C12; i <= (int)EnumDiaBang.BANG_C40; i++)
            //    {
            //        _material = new MaterialOri((EnumDiaBang)i, GeneralClass.CfgData.MatPoolYuliao1, 9999);            //余料利用长度1
            //        if (!m_MaterialPool.Contains(_material)) m_MaterialPool.Add(_material);
            //        _material = new MaterialOri((EnumDiaBang)i, GeneralClass.CfgData.MatPoolYuliao2, 9999);            //余料利用长度2
            //        if (!m_MaterialPool.Contains(_material)) m_MaterialPool.Add(_material);

            //        if (GeneralClass.CfgData.MatPoolHave3)
            //        {
            //            _material = new MaterialOri((EnumDiaBang)i, 3000, 9999);
            //            if (!m_MaterialPool.Contains(_material)) m_MaterialPool.Add(_material);
            //        }
            //        if (GeneralClass.CfgData.MatPoolHave6)
            //        {
            //            _material = new MaterialOri((EnumDiaBang)i, 6000, 9999);
            //            if (!m_MaterialPool.Contains(_material)) m_MaterialPool.Add(_material);
            //        }

            //        _material = new MaterialOri((EnumDiaBang)i, 12000, 9999);
            //        if (!m_MaterialPool.Contains(_material)) m_MaterialPool.Add(_material);


            //    }
            //}
            //return m_MaterialPool;
        }
        /// <summary>
        /// 原材库，包含3m、4m、5m、6m、7m等非定尺材，以及9m和12m定尺原材
        /// 默认的几个长度:
        /// 9米原材包括：1米、1.8米、2.25米、3米、4.5米、6米、7.2米、8米、9米
        /// 12米原材包括：2米、3米、4米、5米、6米、7米、8米、9米、10米、12米
        /// </summary>
        public static List<MaterialOri> m_MaterialPool = new List<MaterialOri>();




        /// <summary>
        /// 直径enum转为int
        /// </summary>
        /// <param name="_bang"></param>
        /// <returns></returns>
        public static int EnumDiameterToInt(EnumDiaBang _bang)
        {
            return Convert.ToInt16(_bang.ToString().Substring(6, 2));
        }
        /// <summary>
        /// 直径int转为enum
        /// </summary>
        /// <param name="_diameter"></param>
        /// <returns></returns>
        public static EnumDiaBang IntToEnumDiameter(int _diameter)
        {
            try
            {
                return (EnumDiaBang)System.Enum.Parse(typeof(EnumDiaBang), "BANG_C" + _diameter.ToString());//将直径转为enum
            }
            catch (Exception e)
            {
                //MessageBox.Show("IntToEnumDiameter error :" + e.Message);
                GeneralClass.interactivityData?.printlog(1, "IntToEnumDiameter error :" + e.Message);
                return EnumDiaBang.NONE;
            }
        }
        /// <summary>
        /// 智能料仓设置enum转为int
        /// </summary>
        /// <param name="_wareset"></param>
        /// <returns></returns>
        public static int EnumWareSetToInt(EnumWareNumSet _wareset)
        {
            if (_wareset != EnumWareNumSet.NONE)
            {
                return Convert.ToInt32(_wareset.ToString().Last().ToString());
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 智能料仓设置的int转为enum
        /// </summary>
        /// <param name="_wareset"></param>
        /// <returns></returns>
        public static EnumWareNumSet IntToEnumWareSet(int _wareset)
        {
            return (EnumWareNumSet)System.Enum.Parse(typeof(EnumWareNumSet), "WARESET_" + _wareset.ToString());//将直径转为enum
        }
        //public static int[] wareNum = new int[(int)EnumWareNumGroup.maxNum] { 48, 12, 6, 3 };

        private static int[] warenum = new int[(int)EnumWareNumSet.maxNum] /*{ 48,12,6,3}*/;


        /// <summary>
        /// 仓位数,按照4条通道配置
        /// </summary>
        public static int[] wareNum
        {
            get
            {
                switch (m_factoryType)
                {
                    case EnumFactoryType.LowConfig:
                        warenum[(int)EnumWareNumSet.WARESET_8] = 8 * 2;
                        warenum[(int)EnumWareNumSet.WARESET_4] = 4 * 2;
                        warenum[(int)EnumWareNumSet.WARESET_2] = 2 * 2;
                        warenum[(int)EnumWareNumSet.WARESET_1] = 1 * 2;
                        break;
                    case EnumFactoryType.HighConfig:
                        warenum[(int)EnumWareNumSet.WARESET_8] = 8 * GeneralClass.CfgData.WareHouseChannels;
                        warenum[(int)EnumWareNumSet.WARESET_4] = 4 * GeneralClass.CfgData.WareHouseChannels;
                        warenum[(int)EnumWareNumSet.WARESET_2] = 2 * GeneralClass.CfgData.WareHouseChannels;
                        warenum[(int)EnumWareNumSet.WARESET_1] = 1 * GeneralClass.CfgData.WareHouseChannels;
                        break;
                    case EnumFactoryType.Experiment:
                        warenum[(int)EnumWareNumSet.WARESET_8] = 8 * 1;
                        warenum[(int)EnumWareNumSet.WARESET_4] = 4 * 1;
                        warenum[(int)EnumWareNumSet.WARESET_2] = 2 * 1;
                        warenum[(int)EnumWareNumSet.WARESET_1] = 1 * 1;
                        break;
                }
                return warenum;
            }
            set { warenum = value; }
        }

        /// <summary>
        /// 直径种类的中文描述，"1种直径", "2种直径", "3种直径", "4种直径", "1~4种直径", "5~种直径"
        /// </summary>
        public static readonly string[] m_DiaType = new string[(int)EnumDiaGroupType.maxDiameterType] { "1~4种直径", "5~种直径", "1种直径", "2种直径", "3种直径", "4种直径" };
        /// <summary>
        /// 多直径分组包含直径种类数量，0(不考虑包含关系),1，2，100(全包含)
        /// </summary>
        public static readonly int m_inclueNum = 0;
        /// <summary>
        /// 分组时，是否判定螺距类型
        /// </summary>
        public static readonly bool m_checkPitchType = false;
        /// <summary>
        /// 仓位划分的数量区间，四种仓位，三个节点:25,50,100
        /// </summary>
        public static int[] wareArea
        {
            get
            {
                return new int[3] { CfgData.WareAreaSet1, CfgData.WareAreaSet2, CfgData.WareAreaSet3 };
            }
        }
        //public static int[] wareArea = new int[3] { 10, 50, 100 };


        /// <summary>
        /// 钢筋总数据库的名称
        /// </summary>
        public static string AllRebarDBfileName = "aa";
        /// <summary>
        /// 找不到图形编号的图形是否用默认图片代替
        /// </summary>
        public static bool m_showNoFindPic = true;

        /// <summary>
        /// 套料显示图片的尺寸
        /// </summary>
        public static System.Drawing.Size taoPicSize = new System.Drawing.Size(620, 30);

        /*************配置文件**************/
        public static ConfigData CfgData { get; set; }

        /// <summary>
        /// 工厂类型，分为低配、高配、实验
        /// </summary>
        public static EnumFactoryType m_factoryType { get { return CfgData.Factorytype; } set { m_factoryType = value; } }
        /// <summary>
        /// Φ12直径钢筋归类于线材还是棒材，false为线材，true为棒材
        /// </summary>
        public static bool m_typeC12 { get { return CfgData.TypeC12; } set { m_typeC12 = value; } }
        /// <summary>
        /// Φ14直径钢筋归类于线材还是棒材，false为线材，true为棒材
        /// </summary>
        public static bool m_typeC14 { get { return CfgData.TypeC14; } set { m_typeC14 = value; } }
        /// <summary>
        /// 棒材线材分隔阈值，取决于Φ12直径钢筋和Φ14直径钢筋归属
        /// </summary>
        public static int m_threshold_BX { get { return (GeneralClass.m_typeC12) ? 12 : ((GeneralClass.m_typeC14) ? 14 : 16); } }
        /*************配置文件**************/



        public static string[] sRebarAssemblyTypeName = new string[(int)EnumRebarAssemblyType.maxAssemblyNum]
            {
                "基础",
                "墙",
                "柱",
                "梁",
                "板",
                "梯"

            };

        /// <summary>
        /// 钢筋表的列名
        /// </summary>
        public static string[] sRebarColumnName = new string[(int)EnumAllRebarTableColName.maxRebarColNum]
        {
            "项目名称",
            "主构件名称",
           "子构件名称",
           "料表名称",
           "料表页名",
            "图形编号",
            "级别",
            "直径",
            "钢筋简图",
            "图形信息",
            "边角结构",
            "下料长度",
            "是否多段",
            "根数件数",
            "总根数",
            "重量",
            "备注",
            "标注序号",
            "是否原材",
            "是否套丝",
            "是否弯曲",
            "是否切断",
            "是否弯曲两次以上",

            "是否选择",

            "生产批号",
            "料仓号",
            "仓位号",
            "料仓设置",
            "料仓临时编号",
            "套丝机设置",

            "生产批号",
            "数量",
            "临时编号"

        };


        public static string[] sAllPicColumnName = new string[(int)EnumAllPicTableColName.maxAllPicColNum]
        {
            "图形编号",
            "钢筋简图",
            "数量",
            "重量",
            "备注"
        };

        public static string[] sDetailTableRowName = new string[(int)EnumBangOrXian.maxRowNum]
        {
            "A6",
            "A8",
            "C6",
            "C8",
            "C10",
            "C12",
            "C14",
            "C16",
            "C18",
            "C20",
            "C22",
            "C25",
            "C28",
            "C32",
            "C36",
            "C40",
        };

        public static string[] sDetailTableColName = new string[(int)EnumDetailTableColName.maxColNum]
        {
            "原材",
            "原材仅套丝",
            "原材仅弯曲",
            "仅切断",
            "切断套丝",
            "切断弯曲小于2次",
            "切断弯曲大于2次",
            "切断弯曲套丝"

        };

        public static string[] sDetailTableItemName = new string[(int)EnumDetailItem.maxItemNum]
        {
            "总数量(根)",
            "总重量(kg)",
            "总长度(m)",
            "套丝数量(个)",
            "套筒数量(个)",
            "正丝套筒数量(个)",
            "反丝套筒数量(个)",
            "变径套筒数量(个)",
            "切断总次数(次)",
            "弯曲次数(次)",
            "调直长度(m)"
        };


    }
}
