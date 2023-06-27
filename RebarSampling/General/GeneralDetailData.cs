using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebarSampling
{
    /// <summary>
    /// 统计数据结构
    /// </summary>
    public class GeneralDetailData
    {
        public GeneralDetailData() 
        { 
            this.TotalPieceNum = 0;
            this.TotalWeight = 0;
            this.TotalLength = 0;
            this.TaosiNum = 0;
            this.TaotongNum = 0;
            this.TaotongNum_P = 0;
            this.TaotongNum_N = 0;
            this.TaotongNum_V = 0;
            this.CutNum = 0;
            this.BendNum = 0;
            this.StraightenedNum = 0;
        }
        public void Init()
        {
            this.TotalPieceNum = 0;
            this.TotalWeight = 0;
            this.TotalLength = 0;
            this.TaosiNum = 0;
            this.TaotongNum = 0;
            this.TaotongNum_P = 0;
            this.TaotongNum_N = 0;
            this.TaotongNum_V = 0;
            this.CutNum = 0;
            this.BendNum = 0;
            this.StraightenedNum = 0;
        }
        /// <summary>
        /// 总数量，根数
        /// </summary>
        public int TotalPieceNum { get; set; }
        /// <summary>
        /// 总重量，kg
        /// </summary>
        public double TotalWeight { get; set; }
        /// <summary>
        /// 总长度，mm
        /// </summary>
        public int TotalLength { get; set; }
        /// <summary>
        /// 套丝个数
        /// </summary>
        public int TaosiNum { get; set; }
        /// <summary>
        /// 套筒个数
        /// </summary>
        public int TaotongNum { get; set; }
        /// <summary>
        /// 正丝套筒个数
        /// </summary>
        public int TaotongNum_P { get; set; }
        /// <summary>
        /// 反丝套筒个数
        /// </summary>
        public int TaotongNum_N { get; set; }
        /// <summary>
        /// 变径套筒个数
        /// </summary>
        public int TaotongNum_V { get; set; }
        /// <summary>
        /// 切断次数
        /// </summary>
        public int CutNum { get; set; }
        /// <summary>
        /// 弯曲次数
        /// </summary>
        public int BendNum { get; set; }
        /// <summary>
        /// 调直长度，mm
        /// </summary>
        public int StraightenedNum { get; set; }
    }
}
