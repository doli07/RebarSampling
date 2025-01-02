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
    public class MaterialOri
    {
        /// <summary>
        /// 原材库数据结构的构造函数
        /// </summary>
        public MaterialOri()
        {
            this._level = "C";//默认三级钢
            this._diameter = EnumDiaBang.NONE;
            this._length = 0;
            this._num = 0;
        }
        /// <summary>
        /// 构造函数，构造指定规格的原材
        /// </summary>
        /// <param name="m_diameter">直径</param>
        /// <param name="m_length">长度</param>
        /// <param name="m_num">数量</param>
        /// <param name="m_level">级别</param>
        public MaterialOri(EnumDiaBang m_diameter, int m_length, int m_num,string m_level="C")
        {
            this._level = m_level;
            this._diameter = m_diameter;
            this._length = m_length;
            this._num = m_num;
        }
        /// <summary>
        /// 级别
        /// </summary>
        public string _level { get; set; }
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
