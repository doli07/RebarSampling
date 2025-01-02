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
        private static workflow _instance = null;
        private workflow()
        {
            GeneralClass.interactivityData.servermsg += ServerMsgProcess;
        }

        public static workflow Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new workflow();
                }
                return _instance;
            }
        }

        public bool _sendflag_LB = false;
        public bool _sendflag_CX = false;
        public bool _sendflag_PiCut = false;
        public bool _sendflag_QZ = false;
        public bool _sendflag_BEND = false;

        private bool _threadflag = false;
        /// <summary>
        /// 发送工单数据，依次为梁板线、磁吸上料、批量锯切、墙柱线、弯曲机器人
        /// </summary>
        /// <param name="_jsonlist">工单json</param>
        /// <param name="timestep">间隔时间，ms</param>
        public void SendWorkBill(Tuple<List<string>, string, string, string, List<string>> _json, int timestep)
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
                                if (_sendflag_LB)
                                {
                                    _step = 2;
                                }
                                if (_sendflag_CX)
                                {
                                    _step = 3;
                                }
                                if (_sendflag_PiCut)
                                {
                                    _step = 4;
                                }
                                if (_sendflag_QZ)
                                {
                                    _step = 5;
                                }
                                if (_sendflag_BEND)
                                {
                                    _step = 6;
                                }
                            }
                            break;
                        case 2://发送梁板线json工单
                            {
                                foreach (string item in _json.Item1)
                                {
                                    GeneralClass.webServer.SendMsg(item);
                                    Thread.Sleep(timestep);
                                }
                                _step = 1;//回到step1，继续等待发送信号
                                _sendflag_LB = false;//flag复位
                            }
                            break;
                        case 3://发送磁吸上料json工单
                            {
                                //foreach (string item in _json.Item2)
                                //{
                                //    GeneralClass.webServer.SendMsg(item);
                                //    Thread.Sleep(timestep);
                                //}
                                GeneralClass.webServer.SendMsg(_json.Item2);

                                _step = 1;//回到step1，继续等待发送信号
                                _sendflag_CX = false;//flag复位
                            }
                            break;
                        case 4://发送批量锯切json工单
                            {
                                //foreach (string item in _json.Item3)
                                //{
                                GeneralClass.webServer.SendMsg(_json.Item3);
                                //Thread.Sleep(timestep);
                                //}
                                _step = 1;//回到step1，继续等待发送信号
                                _sendflag_PiCut = false;//flag复位
                            }
                            break;
                        case 5://发送墙柱线json工单
                            {
                                //foreach (string item in _json.Item4)
                                //{
                                GeneralClass.webServer.SendMsg(_json.Item4);
                                //Thread.Sleep(timestep);
                                //}
                                _step = 1;//回到step1，继续等待发送信号
                                _sendflag_QZ = false;//flag复位
                            }
                            break;
                        case 6://发送墙柱线json工单
                            {
                                foreach (var item in _json.Item5)
                                {
                                    GeneralClass.webServer.SendMsg(item);
                                    Thread.Sleep(timestep);
                                }
                                _step = 1;//回到step1，继续等待发送信号
                                _sendflag_BEND = false;//flag复位
                            }
                            break;

                    }
                    Thread.Sleep(1);
                }

                GeneralClass.webServer.Stop();

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
                    _sendflag_LB = true;
                    GeneralClass.interactivityData?.printlog(1, "收到梁板线pcs工单请求信号");
                }
                if (_json.Msgtype == 9)
                {
                    _sendflag_CX = true;
                    GeneralClass.interactivityData?.printlog(1, "收到磁吸上料pcs工单请求信号");
                }
                if (_json.Msgtype == 5)
                {
                    _sendflag_PiCut = true;
                    GeneralClass.interactivityData?.printlog(1, "收到批量锯切pcs工单请求信号");
                }
                if (_json.Msgtype == 3)
                {
                    _sendflag_QZ = true;
                    GeneralClass.interactivityData?.printlog(1, "收到墙柱线pcs工单请求信号");
                }
                if (_json.Msgtype == 7)
                {
                    _sendflag_BEND = true;
                    GeneralClass.interactivityData?.printlog(1, "收到弯曲机器人pcs工单请求信号");
                }
            }
        }
    }
}
