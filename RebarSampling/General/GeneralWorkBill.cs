using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebarSampling
{
    /// <summary>
    /// 单根钢筋的信息
    /// </summary>
    public struct SingleRebarData
    {
        /// <summary>
        /// 项目名称，对应的excel中的文件名
        /// </summary>
        public string projectName { get; set; }

        /// <summary>
        /// 主构件名称，对应的excel中的sheetname
        /// </summary>
        public string assemblyName { get; set; }
        /// <summary>
        /// 子构件名称，对应excel表中的构件名称
        /// </summary>
        public string elementName { get; set; }
        /// <summary>
        /// 加工图形编号
        /// </summary>
        public string picNo { get; set; }
        /// <summary>
        /// 钢筋级别
        /// </summary>
        public string level { get; set; }
        /// <summary>
        /// 直径
        /// </summary>
        public int diameter { get; set; }
        /// <summary>
        /// 下料长度
        /// </summary>
        public int length { get; set; }
        /// <summary>
        /// 边角结构,
        /// </summary>
        public string cornerMsg { get; set; }

        /// <summary>
        /// 数据库索引ID
        /// </summary>
        public int ID { get; set; }
    }

    /// <summary>
    /// 生产工单数据格式
    /// </summary>
    public class GeneralWorkBill
    {
        public GeneralWorkBill() 
        {
            this.BillNo = "";
            this.ProjectName = "";
            this.Building = "";
            this.Floor = "";
            this.Level = "";
            this.Diameter = 0;
            this.RebarList = new List<SingleRebarData>();
            
        }
        /// <summary>
        /// 工单号
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
        /// 直径
        /// </summary>
        public int Diameter { get; set; }
        /// <summary>
        /// 原材长度，一般9米或12米两种
        /// </summary>
        public int OriginalLength { get; set; }

        public List<SingleRebarData> RebarList { get; set; }
    }
}
