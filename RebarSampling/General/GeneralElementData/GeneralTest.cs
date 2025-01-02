using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebarSampling.General
{
    /// <summary>
    /// 测试背包算法用的数据结构
    /// </summary>
    public class GeneralTest
    {
        public GeneralTest(int _seriNo,int _length, bool _ifuse = false) 
        {
            this.ifuse = _ifuse; 
            this.seriNo = _seriNo;
            this.length = _length;
        }
        public bool ifuse { get; set; }
        public int seriNo { get; set; }

        public int length { get; set;}
    }
}
