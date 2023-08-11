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
    /// 委托类型，用于操作统计界面初始化dgv
    /// </summary>
    public delegate void DelegateInitStatisticsDGV();

    /// <summary>
    /// 委托类型，用于操作统计界面显示assembly的料单内容
    /// </summary>
    public delegate void DelegateShowAssembly(string _projectName,string _assemblyName);
    /// <summary>
    /// 委托类型，用于操作套料界面显示element构件内容
    /// </summary>
    public delegate void DelegateShowElement();

    /// <summary>
    /// 委托类型，从主界面查询该钢筋数据是否被选中
    /// </summary>
    /// <param name="_data"></param>
    /// <returns></returns>
    public delegate bool DelegateIfRebarChecked(RebarData _data);

    /// <summary>
    /// 委托类型，从主界面查询该钢筋数据是否点选，注意点选和选中的区别
    /// </summary>
    /// <param name="_data"></param>
    /// <returns></returns>
    public delegate bool DelegateIfRebarSelected(RebarData _data);

    public delegate void DelegateWebServerMsg(string msg);
    public delegate void DelegateWebClientMsg(string msg);

    public delegate void DelegateMqttPublishMsg(string msg);
    public delegate void DelegateMqttSubscribMsg(string msg);

    /// <summary>
    /// 委托类型，从form2中根据_type搜寻对应的image图片
    /// </summary>
    /// <param name="_type">钢筋图形</param>
    /// <param name="_image">输出图片</param>
    /// <returns>true：找到了，false：未找到</returns>
    public delegate bool DelegateFindImage(string _type, out object _image);

    /// <summary>
    /// 委托类型，发送jsonlist,以timestep为间隔发送时间
    /// </summary>
    /// <param name="_list"></param>
    /// <param name="timeStep"></param>
    /// <returns></returns>
    public delegate void DelegateSendWorkBill(List<string> _jsonlist,int timeStep);
        
    /// <summary>
    /// 内部数据交互类，主要用于不同线程间、不同窗口间传递数据的委托
    /// </summary>
    public class InteractivityData
    {
            
        public DelegatePrint printlog { get; set; }

        public DelegateInitStatisticsDGV initStatisticsDGV { get; set; }

        public DelegateShowAssembly showAssembly { get; set; }

        public DelegateShowElement showElement { get; set; }

        public DelegateIfRebarChecked ifRebarChecked { get; set; }

        public DelegateIfRebarSelected ifRebarSelected { get; set; }

        public DelegateWebServerMsg servermsg { get; set; }

        public DelegateWebClientMsg clientmsg { get; set; }

        public DelegateMqttPublishMsg mqttpublishmsg { get; set; }
        public DelegateMqttSubscribMsg mqttsubscribmsg { get; set; }

        public DelegateFindImage ifFindImage { get; set; }

        public DelegateSendWorkBill sendworkbill { get; set; }
    }
}
