using RebarSampling;
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
            this.diameter = 1;
            this.num = 0;
            this.cornerMsg = "";
            //this.headType = EnumMultiHeadType.NONE;
        }

        /// <summary>
        /// 长度,考虑缩尺通用，此处用string，而不是int
        /// </summary>
        public string length { get; set; }
        /// <summary>
        /// 长度，int型的
        /// </summary>
        public int ilength
        {
            get
            {
                //if(this.length.IndexOf('~')>-1)         //特殊：缩尺
                //{
                //    string[] ss = this.length.Split('~');
                //    return (Convert.ToInt32(ss[0]) + Convert.ToInt32(ss[1]))/2;
                //}

                if (this.length.IndexOf('d') > -1)//"10d,90"
                {
                    string[] ss = this.length.Split('d');
                    return Convert.ToInt32(ss[0]) * this.diameter;//10d即为10倍直径
                }
                //圆弧端头示例：
                //      1200R1200T0.44,2;4200,3;1200R1200T0.44,0
                //      900R900T0.44,5;4250,5;900R900T0.44,0
                else if (this.headType == EnumMultiHeadType.ARC)//圆弧端头
                {
                    string[] ss = this.length.Split('R');
                    return Convert.ToInt32(ss[0]);
                }
                else
                {
                    return Convert.ToInt32(this.length);
                }
            }
        }
        /// <summary>
        /// 直径，因为长度中可能会关联上（如："10d,90"代表10倍直径），所以要带上直径
        /// </summary>
        public int diameter { get; set; }
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
        /// 边角信息拆分开的第一段
        /// </summary>
        public string msg_first
        {
            get
            {
                return this.cornerMsg.Split(',')[0];
            }
        }
        /// <summary>
        /// 边角信息拆分开的第二段
        /// </summary>
        public string msg_second
        {
            get
            {
                return this.cornerMsg.Split(',')[1];
            }
        }
        /// <summary>
        /// 端头接头类型，接头包含:"原头、弯、套、变径套、反套、丝、反丝、搭、单、双、对、竖",
        /// </summary>
        public EnumMultiHeadType headType
        {
            get
            {
                if (this.cornerMsg != string.Empty)//边角信息有效
                {
                    string[] ss = this.cornerMsg.Split(',');

                    //圆弧端头示例：
                    //      1200R1200T0.44,2;4200,3;1200R1200T0.44,0
                    //      900R900T0.44,5;4250,5;900R900T0.44,0
                    if (ss[0].IndexOf("R") > -1 && ss[0].IndexOf("T") > -1)//20241112添加，圆弧端头，这个优先处理
                    {
                        return EnumMultiHeadType.ARC;
                    }
                    //if (ss[1].Equals("0") || ss[1].Equals("0&D"))
                    else if (ss[1].IndexOf("0") == 0)
                    {
                        return EnumMultiHeadType.ORG;
                    }
                    //else if (ss[1].Equals("套"))//套
                    else if (ss[1].IndexOf("套") == 0)//套
                    {
                        return EnumMultiHeadType.TAO_P;
                    }
                    else if (ss[1].IndexOf("套") > 0 && ss[1].IndexOf("反") == -1)//25套，变径套筒，不含“反”字
                    {
                        return EnumMultiHeadType.TAO_V;
                    }
                    else if (DBOpt.RegexIsNumeric(ss[1])) //90，是否为整数，整数则为弯曲角度
                    {
                        return EnumMultiHeadType.BEND;
                    }
                    //else if (ss[1].Equals("反套"))
                    else if (ss[1].IndexOf("反套") == 0)
                    {
                        return EnumMultiHeadType.TAO_N;
                    }
                    else if (ss[1].IndexOf("反") >= 0)//反丝，包括变径反丝
                    {
                        return EnumMultiHeadType.SI_N;
                    }
                    else if (ss[1].IndexOf("丝") == 0)//特例：【7380,套;12000,丝&D】
                    {
                        return EnumMultiHeadType.SI_P;
                    }
                    else if (ss[1].IndexOf("搭") > -1)//含有“搭”
                    {
                        return EnumMultiHeadType.DA;
                    }

                    else
                    {
                        return EnumMultiHeadType.NONE;
                    }

                }
                else { return EnumMultiHeadType.NONE; }
            }
        }
        /// <summary>
        /// 如果是弯曲类型，则获取其弯曲角度
        /// </summary>
        public int angle
        {
            get
            {
                return (this.headType == EnumMultiHeadType.BEND) ? Convert.ToInt32(this.cornerMsg.Split(',')[1]) : 0;//获取弯曲角度
            }
        }
        /// <summary>
        /// 较为粗略的类型分类，0:原始端头，1:弯曲，2:套丝，3：其他
        /// </summary>
        public int type
        {
            get
            {

                if (this.headType == EnumMultiHeadType.ORG || this.headType == EnumMultiHeadType.DA)
                {
                    return 0;
                }
                else if (this.headType == EnumMultiHeadType.BEND)
                {
                    return 1;
                }
                else if (this.headType == EnumMultiHeadType.TAO_P ||
                    this.headType == EnumMultiHeadType.TAO_V ||
                    this.headType == EnumMultiHeadType.TAO_N ||
                    this.headType == EnumMultiHeadType.SI_P ||
                    this.headType == EnumMultiHeadType.SI_N)
                {
                    return 2;
                }
                else
                {
                    return 3;
                }
            }
        }

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
