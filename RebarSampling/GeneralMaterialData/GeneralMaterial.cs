using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebarSampling
{
    /// <summary>
    /// 定义原材库数据结构。
    /// 规定除9m、12m的定尺原材外，3m、4m、5m、6m、7m等非定尺材也作为原材，一起存入原材库
    /// </summary>
    public class GeneralMaterial
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GeneralMaterial()
        {
            this._diameter = EnumDiaBang.NONE;
            this._length = 0;
            this._num = 0;
        }
        /// <summary>
        /// 构造函数，构造指定规格的原材
        /// </summary>
        /// <param name="m_diameter"></param>
        /// <param name="m_length"></param>
        /// <param name="m_num"></param>
        public GeneralMaterial(EnumDiaBang m_diameter, int m_length, int m_num)
        {
            this._diameter = m_diameter;
            this._length = m_length;
            this._num = m_num;
        }
        /// <summary>
        /// 直径
        /// </summary>
        public EnumDiaBang _diameter { get; set; }
        /// <summary>
        /// 长度（mm）
        /// </summary>
        public int _length { get; set; }
        /// <summary>
        /// 数量（根）
        /// </summary>
        public int _num { get; set; }
    }
}
