using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebarSampling
{

    public static partial class GeneralClass
    {
        /// <summary>
        /// 操作读写excel文件
        /// </summary>
        public static ExcelReader readEXCEL = new ExcelReader();

        /// <summary>
        /// 操作json序列化与反序列化
        /// </summary>
        public static json JsonOpt = new json();
        /// <summary>
        /// 操作sqlite文件读取
        /// </summary>
        public static SQLiteOpt SQLiteOpt = new SQLiteOpt();

        /// <summary>
        /// 所有钢筋数据总的list
        /// </summary>
        public static List<RebarData> AllRebarList = new List<RebarData>();

        /// <summary>
        /// detail分析用的三维数组，尺寸*工艺*分析项
        /// </summary>
        public static object[,,] AllDetailData = new object[(int)EnumDetailTableColName.ONLY_CUT, (int)EnumDetailTableRowName.maxRowNum, (int)EnumDetailItem.maxItemNum];//先处理三个原材的
        //public static object[,,] AllDetailData = new object[(int)EnumDetailTableRowName.maxRowNum, (int)EnumDetailTableColName.ONLY_CUT, (int)EnumDetailItem.maxItemNum];//先处理三个原材的

        /// <summary>
        /// detail分析除了原材以外的所有图形
        /// </summary>
        public static List<KeyValuePair<EnumRebarPicType, GeneralDetailData>>[] AllDetailOtherData = new List<KeyValuePair<EnumRebarPicType, GeneralDetailData>>[(int)EnumDetailTableRowName.maxRowNum];
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
        public static string AllRebarBKTableName = "allsheet_bk";
        /// <summary>
        /// 经过筛选后的钢筋总表的tablename
        /// </summary>
        public static string AllRebarTableName = "allsheet";

        /// <summary>
        /// 钢筋总数据库的名称
        /// </summary>
        public static string AllRebarDBfileName = "aa";

        public static string[] sRebarAssemblyTypeName = new string[(int)EnumRebarAssemblyType.maxAssemblyNum]
            {
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

        };


        public static string[] sAllPicColumnName = new string[(int)EnumAllPicTableColName.maxAllPicColNum]
        {
            "图形编号",
            "钢筋简图",
            "数量",
            "重量",
            "备注"
        };

        public static string[] sDetailTableRowName = new string[(int)EnumDetailTableRowName.maxRowNum]
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
