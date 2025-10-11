using NPOI.OpenXmlFormats.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace RebarSampling
{
    /// <summary>
    /// 此类为E筋图形信息库的数据结构类，用以表征其图形信息
    /// 示例：
    ///     箍筋：33/P2 108 5 73 5 73 43 108 43 108 5/L150 90+0 5+16 0 1,570 73+-14 24+6 0 1,150 90+0 43+0 0 1,570 108+4 24+6 0 0/D-135,-90,-90,-90,-135/M3,4,0,0,0/SG
    ///     拉勾：33/P2 64 23 117 23/L170 90+0 23+0 0 1/D90,135/SL
    ///     主筋两头带弯钩：33/P2 38 32 38 15 144 15 144 32/L180 38+-1 23+8 0 2,6380 91+0 15+0 0 1,180 144+3 23+8 0 0/D0,90,90,0
    ///     带圆弧：33/P2 32 45 32 23 51 3 131 3 150 24 150 45/L900 32+0 34+0 -90 9,950 36+-15 8+-9 -46 9,4200 91+0 3+0 0 1,950 145+8 9+-14 47 9,900 150+0 34+0 270 9/D0,45,45,45,45,0/H0,0.44,0,0.44,0,0
    /// </summary>
    public class EjinPicMsg
    {
        public EjinPicMsg()
        {
            this.versionNo = "";
            this.PlineType = "";
            this.PlinePoints = new List<Point>();
            this.LengthMsg = new List<LengthMsg>();
            this.Match = new List<string>();
            this.Dtype = new List<string>();
            this.HArc = new List<float>();
            //this.Stype=EnumRebarShapeType.NONE;
        }

        /// <summary>
        /// 版本号
        /// </summary>
        public string versionNo { get; set; }
        /// <summary>
        /// 钢筋线坐标，P开头，线形：1 细线、2 加粗、3 细虚线、4 粗虚线
        /// </summary>
        public string PlineType { get; set; }
        /// <summary>
        /// 钢筋线坐标，P开头，各个端点的坐标
        /// </summary>
        public List<Point> PlinePoints { get; set; }
        /// <summary>
        /// 长度信息标注，L开头，标注各个线段的长度
        /// </summary>
        public List<LengthMsg> LengthMsg { get; set; }

        /// <summary>
        /// Y向整体偏移值，如果yLimit的最小值小于0，则需要Y向整体偏移
        /// </summary>
        public int bias { get { return (this.yLimit.Item1 < 0) ? (0 - this.yLimit.Item1) : 0; } }
        /// <summary>
        /// 根据ylimit值的最大值-最小值，并与40对比，取较大值，得到图片实际画图高度范围
        /// </summary>
        public int realHeight { get { return Math.Max(40, this.yLimit.Item2 - this.yLimit.Item1) + 10; } }
        /// <summary>
        /// 通过判断PlinePoints各个端点的y正负限，和LengthMsg标注位置的y正负限，调整整体偏移值，输出pair为Y值的[负极限，正极限]
        /// </summary>
        public Tuple<int, int> yLimit
        {
            get
            {
                //int _min = Math.Min(this.PlinePoints.Min(t => t.Y), this.LengthMsg.Min(t => t.paintPos.Y)) - 15;//从plinepoints的y值中取最小，从lengthmsg的paintpos的y值取最小，比较两个较小者
                //int _max = Math.Max(this.PlinePoints.Max(t => t.Y), this.LengthMsg.Max(t => t.paintPos.Y) + 20/*RebarSampling.LengthMsg.lengthStrHeight*/);//从plinepoints的y值中取最大，从lengthmsg的paintpos的y值取最大，比较两个较大者
                int _min = Math.Min(this.PlinePoints.Min(t => t.Y), this.LengthMsg.Min(t => t.pos.Y)) - 15;//从plinepoints的y值中取最小，从lengthmsg的paintpos的y值取最小，比较两个较小者
                int _max = Math.Max(this.PlinePoints.Max(t => t.Y), this.LengthMsg.Max(t => t.pos.Y) + 15/*RebarSampling.LengthMsg.lengthStrHeight*/);//从plinepoints的y值中取最大，从lengthmsg的paintpos的y值取最大，比较两个较大者

                return new Tuple<int, int>(_min, _max + 2/*+LengthMsg.lengthStrHeight*/);   //最大值=最大的基准点y值+字符高度
            }
        }
        /// <summary>
        /// 匹配的对应边，M开头，示例：M3,4,0,0,0
        /// </summary>
        public List<string> Match { get; set; }
        /// <summary>
        /// 端头信息，D开头，
        /// 接头类型或者角度
        /// 接头包含:"套、反、长、搭、单、双、对、竖、机、丝、反丝、长丝(仅用于端部)",
        /// 角度包含:"90、135、180、-135(-代表反向弯勾)"
        /// </summary>
        public List<string> Dtype { get; set; }
        /// <summary>
        /// 圆弧的凸度，H开头
        /// </summary>
        public List<float> HArc { get; set; }
        /// <summary>
        /// 形状类型说明，SG,SL,SC，sg表示箍筋，sl表示拉勾，sc表示马凳
        /// </summary>
        public EnumRebarShapeType Stype { get; set; }

    }

    /// <summary>
    /// 长度信息
    /// </summary>
    public class LengthMsg
    {
        public LengthMsg()
        {
            this.lengthStrWidth = 0;
            this.lengthStrHeight = 0;
            this.length = "";
            this.pos = new Point(0, 0);
            this.angle = 0;
            this.align = 1;
        }



        /// <summary>
        /// 获取/设定长度标注字符的高度
        /// </summary>
        public int lengthStrHeight { get; set; }
        /// <summary>
        /// 获取/设定长度标注字符的宽度
        /// </summary>
        public int lengthStrWidth { get; set; }
        //public static int lengthStrHeight = 18;//设定长度标注字符的高度
        //public static int lengthStrWidth = 28;//设定长度标注字符的宽度


        /// <summary>
        /// 长度
        /// </summary>
        public string length { get; set; }
        /// <summary>
        /// 长度标注的位置坐标
        /// </summary>
        public Point pos { get; set; }
        /// <summary>
        /// 长度标注的实际画图坐标，即左上角基准坐标，需要在原始解析的位置坐标上按照对齐方式做调整
        /// 水平对齐方式，0：左对齐，1：居中对齐，2：右对齐
        /// </summary>
        public Point paintPos
        {
            get
            {

                if (this.align == 0)//左对齐，位于右侧
                {
                    return new Point(this.pos.X, this.pos.Y - lengthStrHeight);//原始解析坐标是标注的左下角
                }
                else if (this.align == 1)//居中对齐
                {
                    return new Point(this.pos.X - lengthStrWidth / 2 , this.pos.Y - lengthStrHeight);//原始解析坐标是标注的中下部，临时减去5
                }
                else if (this.align == 2)//右对齐，位于左侧
                {
                    return new Point(this.pos.X - lengthStrWidth , this.pos.Y - lengthStrHeight);//原始解析坐标是标注的右下角
                }
                else if (this.align == 4)
                {
                    return new Point(this.pos.X, this.pos.Y - lengthStrHeight / 2);//原始解析坐标是标注的左中部
                }
                else if (this.align == 6)//右对齐，位于左侧
                {
                    return new Point(this.pos.X - lengthStrWidth, this.pos.Y - lengthStrHeight / 2);//原始解析坐标是标注的右中部
                }
                else if (this.align == 9)
                {
                    return new Point(this.pos.X - lengthStrWidth / 2, this.pos.Y);//原始解析坐标是标注的中上部
                }
                else
                {
                    return this.pos;
                }
            }
        }
        /// <summary>
        /// 旋转角度
        /// </summary>
        public int angle { get; set; }
        /// <summary>
        /// 对齐,文字水平对齐方式：0 居左，1 居中，2 居右
        /// </summary>
        public int align { get; set; }
    }

}
