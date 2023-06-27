using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebarSampling
{

    /// <summary>
    /// 钢筋施工位置，按照“xx分区xx楼xx层”标准来区分
    /// </summary>
    public struct RebarConstructionPos
    {
        /// <summary>
        /// 分区
        /// </summary>
        public string Zone { get; set; }
        /// <summary>
        /// 楼栋
        /// </summary>
        public string Building { get; set; }
        /// <summary>
        /// 楼层
        /// </summary>
        public string Floor { get; set; }

    }



    /// <summary>
    /// 钢筋数据结构
    /// </summary>
    public class RebarData
    {
        public RebarData()
        {
            this.RebarConstructionPos = new RebarConstructionPos();
            this.RebarAssemblyType = EnumRebarAssemblyType.None;
            this.ProjectName = "";
            this.MainAssemblyName = "";
            this.ElementName = "";
            this.TypeNum = "";
            this.Level = "";
            this.Diameter = 0;
            this.RebarPic = "";
            this.PicMessage = "";
            this.CornerMessage = "";
            this.Length = "";
            this.IsMulti = false;
            this.PieceNumUnitNum = "";
            this.TotalPieceNum = 0;
            this.TotalWeight = 0;
            this.Description = "";
            this.SerialNum = 0;
            this.IsOriginal = false;
            this.IfTao = false;
            this.IfBend = false;
            this.IfCut = false;
            this.IfBendTwice = false;
        }
        public void init()
        {
            this.RebarConstructionPos = new RebarConstructionPos();
            this.RebarAssemblyType = EnumRebarAssemblyType.None;
            this.ProjectName = "";
            this.MainAssemblyName = "";
            this.ElementName = "";
            this.TypeNum = "";
            this.Level = "";
            this.Diameter = 0;
            this.RebarPic = "";
            this.PicMessage = "";
            this.CornerMessage = "";
            this.Length = "";
            this.IsMulti = false;
            this.PieceNumUnitNum = "";
            this.TotalPieceNum = 0;
            this.TotalWeight = 0;
            this.Description = "";
            this.SerialNum = 0;
            this.IsOriginal = false;
            this.IfTao = false;
            this.IfBend = false;
            this.IfCut = false;
            this.IfBendTwice = false;
        }
        public void Copy(RebarData _data)
        {
            this.RebarConstructionPos = _data.RebarConstructionPos;
            this.RebarAssemblyType = _data.RebarAssemblyType;
            this.ProjectName = _data.ProjectName;
            this.MainAssemblyName = _data.MainAssemblyName;
            this.ElementName = _data.ElementName;
            this.TypeNum = _data.TypeNum;
            this.Level = _data.Level;
            this.Diameter = _data.Diameter;
            this.RebarPic = _data.RebarPic;
            this.PicMessage = _data.PicMessage;
            this.CornerMessage = _data.CornerMessage;
            this.Length = _data.Length;
            this.IsMulti = _data.IsMulti;
            this.PieceNumUnitNum = _data.PieceNumUnitNum;
            this.TotalPieceNum = _data.TotalPieceNum;
            this.TotalWeight = _data.TotalWeight;
            this.Description = _data.Description;
            this.SerialNum = _data.SerialNum;
            this.IsOriginal = _data.IsOriginal;
            this.IfTao = _data.IfTao;
            this.IfBend = _data.IfBend;
            this.IfCut = _data.IfCut;
            this.IfBendTwice = _data.IfBendTwice;
        }
        /// <summary>
        /// 施工部位解析
        /// </summary>
        public RebarConstructionPos RebarConstructionPos { get; set; }
        /// <summary>
        /// 钢筋所属构件类型，分为墙、柱、梁、板、楼梯
        /// </summary>
        public EnumRebarAssemblyType RebarAssemblyType { get; set; }
        /// <summary>
        /// 项目名称，对应的excel中的文件名
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 主构件名称，对应的excel中的sheetname
        /// </summary>
        public string MainAssemblyName { get; set; }
        /// <summary>
        /// 子构件名称，对应excel表中的构件名称
        /// </summary>
        public string ElementName { get; set; }
        /// <summary>
        /// 构件类型编号
        /// </summary>
        public string TypeNum { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 直径
        /// </summary>
        public int Diameter { get; set; }
        /// <summary>
        /// 钢筋简图
        /// </summary>
        public string RebarPic { get; set; }
        /// <summary>
        /// 图片信息
        /// </summary>
        public string PicMessage { get; set; }
        /// <summary>
        /// 边角结构信息
        /// </summary>
        public string CornerMessage { get; set; }
        /// <summary>
        /// 钢筋下料长度，单位：mm，有多段的情况，其length字段中通过\n隔开多段的长度值
        /// </summary>
        public string Length { get; set; }
        /// <summary>
        /// 是否多段
        /// </summary>
        public bool IsMulti { get; set; }
        /// <summary>
        /// 根数*件数
        /// </summary>
        public string PieceNumUnitNum { get; set; }
        /// <summary>
        /// 总根数
        /// </summary>
        public int TotalPieceNum { get; set; }
        /// <summary>
        /// 总重量，kg
        /// </summary>
        public double TotalWeight { get; set; }
        /// <summary>
        /// 说明备注
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 标注序号，注意excel文件中，有效钢筋记录每行的最后一个数字为标注序号
        /// </summary>
        public int SerialNum { get; set; }

        /// <summary>
        /// 是否原材，长度为9000或者12000，即为原材
        /// </summary>
        public bool IsOriginal { get; set; }
        /// <summary>
        /// 是否需要套丝
        /// </summary>
        public bool IfTao { get; set; }
        /// <summary>
        /// 是否需要弯曲
        /// </summary>
        public bool IfBend { get; set; }
        /// <summary>
        /// 是否需要切断
        /// </summary>
        public bool IfCut { get; set; }
        /// <summary>
        /// 是否需要弯曲两次以上
        /// </summary>
        public bool IfBendTwice { get; set; }
    }





}
