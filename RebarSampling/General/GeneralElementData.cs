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
            this.projectName = "";
            this.assemblyName = "";
            this.elementIndex = 0;
            this.elementName = "";
            this.diameterType = 0;
            this.diameterList = new List<int>();
            this.rebarlist = new List<RebarData>();
        }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string projectName { get; set; }
        /// <summary>
        /// 主构件名称
        /// </summary>
        public string assemblyName { get; set; }
        /// <summary>
        /// 因为elementname并非唯一，所以需要elementindex来索引，指示在主构件中的索引位置
        /// </summary>
        public int elementIndex { get; set; }
        /// <summary>
        /// 构件名称
        /// </summary>
        public string elementName { get; set; }

        /// <summary>
        /// 直径种类
        /// </summary>
        public int diameterType { get; set; }
        /// <summary>
        /// 所包含的直径规格，Φ16，Φ18，Φ20，Φ22，Φ25，Φ28，Φ32，Φ36，Φ40
        /// </summary>
        public List<int> diameterList { get; set; }

        /// <summary>
        /// 构件中所有的钢筋列表
        /// </summary>
        public List<RebarData> rebarlist { get; set; }
    }
}
