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
        /// 索引号，为当前钢筋在原材钢筋中的排序索引
        /// </summary>
        public int SeriNo { get; set; }
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
        /// 此钢筋对应的仓库编号，1~6
        /// </summary>
        public int WareNo { get; set; }
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

    /// <summary>
    /// 生产工单数据格式
    /// </summary>
    public class WorkBill
    {
        public WorkBill() 
        {
            this.Msgtype = 1;
            this.BillNo = "";
            this.TotalNum = 0;
            this.SteelbarNo = 0;
            this.ProjectName = "";
            this.Block = "";
            this.Building = "";
            this.Floor = "";
            this.Level = "";
            this.Diameter = 0;
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
        /// 工单号
        /// </summary>
        public string BillNo { get; set; }
        /// <summary>
        /// 原材钢筋总数，表示一个流水工单里面有多少根原材钢筋
        /// </summary>
        public int TotalNum { get; set; }
        /// <summary>
        /// 原材钢筋流水号，表示当前钢筋在流水工单里面的流水号，最大不超过totalNum
        /// </summary>
        public int SteelbarNo { get; set; }
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


        public List<SingleRebarData> SteelbarList { get; set; }
    }
}
