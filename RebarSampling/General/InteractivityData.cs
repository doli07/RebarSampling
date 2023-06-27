using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebarSampling
{

    /// <summary>
    /// 委托类型，用于往主界面输出日志
    /// </summary>
    /// <param name="Log"></param>
    public delegate void DelegatePrint(int type,string Log);

    /// <summary>
    /// 内部数据交互类，主要用于不同线程间传递数据的委托
    /// </summary>
    public class InteractivityData
    {
            
        public DelegatePrint printlog { get; set; }

    }
}
