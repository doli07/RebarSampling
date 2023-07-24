using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebarSampling
{
    /// <summary>
    /// 构件包的数据结构，里面包含有多个钢筋
    /// </summary>
    public class ElementData
    {
        public ElementData() 
        { 
            this.elementName = "";
            this.rebarlist = new List<RebarData>();
        }
        /// <summary>
        /// 构件名称
        /// </summary>
        public string elementName { get; set; }
        /// <summary>
        /// 构件中所有的钢筋列表
        /// </summary>
        public List<RebarData> rebarlist { get; set; }
    }
}
