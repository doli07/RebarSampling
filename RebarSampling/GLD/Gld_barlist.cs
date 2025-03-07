using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebarSampling.GLD
{
    /// <summary>
    /// 广联达料单的barlist，猜测应该是钢筋list
    /// </summary>
    public class Gld_barlist
    {

        public List<BarItem> BarItems { get; set; }
    }

    public class BarItem
    {
        /// <summary>
        /// 钢筋图形表达式
        /// </summary>
        public string BarDataExpression {  get; set; }
        /// <summary>
        /// 钢筋在构件里面的流水编号
        /// </summary>
        public string BarIndex {  get; set; }
        /// <summary>
        /// 钢筋图形类型编号
        /// </summary>
        public string BarShapeTypeID {  get; set; }
        /// <summary>
        /// 钢筋简图保存路径
        /// </summary>
        public string BarSketchFile {  get; set; }
        /// <summary>
        /// 钢筋种类：直筋、箍筋、拉勾、马凳
        /// </summary>
        public string BarType {  get; set; }
        /// <summary>
        /// 断料长度
        /// </summary>
        public string BreakLength {  get; set; }
        /// <summary>
        /// 施工区域段编号
        /// </summary>
        public string ConstructionSectionID {  get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count {  get; set; }
        /// <summary>
        /// 直径
        /// </summary>
        public string Diameter {  get; set; }
        /// <summary>
        /// 制作类型，插筋、接筋、拉筋、外箍、内箍
        /// </summary>
        public string FabricationType {  get; set; }
        /// <summary>
        /// 计算式
        /// </summary>
        public string Formula {  get; set; }
        /// <summary>
        /// 钢筋在构件里面的流水编号，与BarIndex相同
        /// </summary>
        public int Index {  get; set; }
        /// <summary>
        /// 构件编号
        /// </summary>
        public string InstanceID {  get; set; }
        /// <summary>
        /// 构件流水编号
        /// </summary>
        public string InstanceIndex {  get; set; }
        /// <summary>
        /// 长度，与BreakLength相同
        /// </summary>
        public int Length {  get; set; }
        /// <summary>
        /// 钢筋级别，牌号
        /// </summary>
        public string LevelName {  get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 间距，一般是箍筋的摆放间距
        /// </summary>
        public double Space { get; set; }
        /// <summary>
        /// 不知道
        /// </summary>
        public string UsedType { get; set; }
        /// <summary>
        /// 重量，注意是单根的重量，汇总计算时，应该乘以数量
        /// </summary>
        public double Weight { get; set; }



    }
}
