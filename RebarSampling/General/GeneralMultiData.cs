using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebarSampling
{
    /// <summary>
    /// 多段数据结构
    /// 示例：
    ///     375,90;11675,套;10725,90;375,0
    ///     12000,搭590;12000,搭590;11080,0
    ///     12000,套;3835,90;375,0
    ///     375,90;9155,套;12000,套;11900,丝
    ///     12000,搭590;12000*2,搭590;7520,0
    ///     0,套;12000,套;12000*2,套;3400,90;375,0
    /// 
    /// </summary>
    public class GeneralMultiData
    {
        public GeneralMultiData() 
        {
            this.length = "";
            this.num = 0;
            this.cornerMsg = "";
            this.headType = EnumMultiHeadType.NONE;
        }
        /// <summary>
        /// 长度,考虑缩尺通用，此处用string，而不是int
        /// </summary>
        public string length { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int num { get; set; }
        /// <summary>
        /// 边角处理信息，包括有：     端点应该输入接头类型或者角度，
        ///                         接头包含:"套、套 20、反套、反、丝、反丝、搭、搭 500、单、双、对、竖",
        ///                         角度包含:"90、135、180、-135(-代表反向弯勾)
        /// </summary>
        public string cornerMsg { get; set; }
        /// <summary>
        /// 端头接头类型，接头包含:"原头、弯、套、变径套、反套、丝、反丝、搭、单、双、对、竖",
        /// </summary>
        public EnumMultiHeadType headType { get; set; }
    }


    public class GeneralMultiLength 
    { 
        public GeneralMultiLength()
        {
            this.length = "";
            this.num = 0;
        }
        /// <summary>
        /// 长度，考虑缩尺通用，此处用string，而不是int
        /// </summary>
        public string length { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int num { get; set; }
    }

}
