using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RebarSampling
{
    /// <summary>
    /// 加工线程，用于与pcs系统交互工单信息
    /// </summary>
    public class workflow
    {
        private static workflow _instance=null;
        private workflow() 
        {
            GeneralClass.interactivityData.servermsg += ServerMsgProcess;
        }

        public static workflow Instance
        {
            get
            {
                if(_instance==null)
                {
                    _instance = new workflow();                    
                }
                return _instance;
            }
        }

        public bool _sendflag = false;

        private bool _threadflag = false;
        /// <summary>
        /// 发送工单数据
        /// </summary>
        /// <param name="_jsonlist">工单json</param>
        /// <param name="timestep">间隔时间，ms</param>
        public void SendWorkBill(List<string> _jsonlist, int timestep)
        {
            Thread sendthread = new Thread(() =>
            {
                 _threadflag = true;
                int _step = 0;

                while (_threadflag)
                {

                    switch (_step)
                    {
                        case 0://先开启webserver
                            {
                                if (GeneralClass.webServer.Start(GeneralClass.CfgData.webserverIP, GeneralClass.CfgData.webserverPort) == 0)
                                {
                                    _step++;
                                }
                                else
                                {
                                    GeneralClass.webServer.Stop();
                                    Thread.Sleep(3000);
                                }
                            }
                            break;
                        case 1://等待工单发送信号
                            {
                                if (_sendflag)
                                {
                                    _step++;
                                }
                            }
                            break;
                        case 2://发送json工单
                            {
                                foreach (string item in _jsonlist)
                                {
                                    GeneralClass.webServer.SendMsg(item);
                                    Thread.Sleep(timestep);
                                }
                                _step = 1;//回到step1，继续等待发送信号
                                _sendflag = false;//flag复位
                            }
                            break;
                    }
                    Thread.Sleep(1);
                }

            }
                );
            sendthread.IsBackground = true;
            sendthread.Start();

        }

        public void StopWorkBill()
        {
            _threadflag = false;
        }
        /// <summary>
        /// 解析webserver接收到的数据
        /// </summary>
        /// <param name="msg"></param>
        private void ServerMsgProcess(string msg)
        {
            if (msg != "")
            {
                WorkBillRequest _json = NewtonJson.Deserializer<WorkBillRequest>(msg);// 将JSON字符串转换为对象
                if (_json.Msgtype == 1)
                {
                    _sendflag = true;
                    GeneralClass.interactivityData?.printlog(1, "收到pcs工单请求信号");
                }
            }
        }
    }
}
