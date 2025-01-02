using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebarSampling
{
    /// <summary>
    /// 表示一组等待加工的钢筋数据实体
    /// </summary>
    public class MOrderDtls
    {
        /// <summary>
        /// 直径-直径的值（单位：mm）(必填)
        /// </summary>
        public float diameter { get; set; }

        /// <summary>
        ///理重（单位：kg）（必填） 
        /// </summary>
        public double cusliweight { get; set; }

        /// <summary>
        /// 总长度（单位：mm）（必填）
        /// </summary>
        public string cuslong { get; set; }

        /// <summary>
        /// 大样图号
        /// </summary>
        public string cusno { get; set; }

        /// <summary>
        /// 规格-HPB300（必填）
        /// </summary>
        public string diaspec { get; set; }

        /// <summary>
        /// 构件名称
        /// </summary>
        public string goujianname { get; set; }

        /// <summary>
        /// 构件位置
        /// </summary>
        public string goujianplace { get; set; }

        /// <summary>
        /// 数量（必填）
        /// </summary>
        public int neednum { get; set; }

        /// <summary>
        /// 流水号，该图在料单中的序号
        /// </summary>
        public int orderindexno { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 生产参数（必填）
        /// 边，角；边，角；边，角；
        /// 如110,90;200,90;110,0;
        /// </summary>
        public string makeparam { get; set; }

        /// <summary>
        /// 图形画图参数（必填）
        /// 易筋本身格式
        /// </summary>
        public string chartparam { get; set; }

    }
    /// <summary>
    /// 表示一个工程的主信息
    /// </summary>
    public class Morder
    {
        public Morder() 
        {
            levelName = new string[5] { "","","","",""};
        }
        public string[] levelName { get; set; }
        //public Morder() 
        //{
        //    levelName_1 = "";
        //    levelName_2 = "";
        //    levelName_3 = "";
        //    levelName_4 = "";
        //    levelName_5 = "";
        //}
        ///// <summary>
        ///// 第一层级名称，一般为项目名称
        ///// </summary>
        //public string levelName_1 { get; set; }
        ///// <summary>
        ///// 第二层级名称，一般为楼栋名称
        ///// </summary>
        //public string levelName_2 { get; set;}
        ///// <summary>
        ///// 第三层级名称，一般为楼层名称
        ///// </summary>
        //public string levelName_3 { get; set;} 
        ///// <summary>
        ///// 第四层级名称，一般为片区名称
        ///// </summary>
        //public string levelName_4 { get; set;}
        ///// <summary>
        ///// 第五层级名称，一般为构件类型名
        ///// </summary>
        //public string levelName_5 { get; set;}

    }

    ///// <summary>
    ///// 表示一个工程
    ///// </summary>
    //public class Morder
    //{
    //    /// <summary>
    //    /// 客户提供的单据号（必填）
    //    /// </summary>
    //    public string cusbillno { get; set; }
    //    /// <summary>
    //    /// 工程名称
    //    /// </summary>
    //    public string projectname { get; set; }
    //    /// <summary>
    //    /// 项目名称
    //    /// </summary>
    //    public string xiangmuname { get; set; }
    //    /// <summary>
    //    /// 客户联系人
    //    /// </summary>
    //    public string cuslinker { get; set; }
    //    /// <summary>
    //    /// 联系电话
    //    /// </summary>
    //    public string linktel { get; set; }
    //    /// <summary>
    //    /// 发货客户地址
    //    /// </summary>
    //    public string cusaddress { get; set; }
    //    /// <summary>
    //    /// 交货日期，字符串格式，年月日时分秒，具体参考json案例（必填）
    //    /// </summary>
    //    public string jiaohuodate { get; set; }
    //    /// <summary>
    //    /// 总重量
    //    /// </summary>
    //    public double weight { get; set; }
    //    /// <summary>
    //    /// 备注
    //    /// </summary>
    //    public string remark { get; set; }
    //    /// <summary>
    //    /// 制单人主键，由建科提供固定的值（必填）
    //    /// </summary>
    //    public int creator { get; set; } = 1;
    //    /// <summary>
    //    /// 客户名称（必填）
    //    /// XXX工程单位
    //    /// </summary>
    //    public string customername { get; set; }
    //}




    /// <summary>
    /// 管理等待上传的工程数据和料单数据
    /// </summary>
    public class BookHelperArray
    {
        /// <summary>
        /// 料单数据实体集合
        /// </summary>
        public List<MOrderDtls> mOrderDtls { get; set; }

        /// <summary>
        /// 工程数据实体
        /// </summary>
        public Morder morder { get; set; }

        public BookHelperArray()
        {
            morder = new Morder();
            mOrderDtls = new List<MOrderDtls>();
        }

        public void Clear()
        {
            morder = new Morder();
            mOrderDtls.Clear();
        }

    }


}
