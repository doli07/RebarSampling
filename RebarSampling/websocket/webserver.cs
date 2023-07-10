using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperWebSocket;
using System.Web;

namespace RebarSampling
{
    public class WebServer
    {
        private WebSocketServer ws = null;

        /// <summary>
        /// session列表
        /// </summary>
        private List<WebSocketSession> sessionList = new List<WebSocketSession>();
        /// <summary>
        /// 启动websocketserver，根据ip和port
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="Port"></param>
        /// <returns>设置服务监听失败：-2；启动服务监听失败：-1；启动服务监听成功：0</returns>
        public int Start(string IP, string Port)
        {
            ws = new WebSocketServer();
            ws.NewSessionConnected += Ws_NewSessionConnected;
            ws.SessionClosed += Ws_SessionClosed;
            ws.NewMessageReceived += Ws_NewMessageReceived;

            if (!ws.Setup(IP, Convert.ToInt32(Port)))
            {
                GeneralClass.interactivityData?.printlog(1, "设置服务监听失败");
                return -2;
            }
            if (!ws.Start())
            {
                GeneralClass.interactivityData?.printlog(1, "启动服务监听失败");
                return -1;
            }
            else
            {
                GeneralClass.interactivityData?.printlog(1, "启动服务监听成功");
                return 0;
            }
        }

        /// <summary>
        /// 停止websocketserver
        /// </summary>
        public void Stop()
        {
            if (ws != null)
            {
                GeneralClass.interactivityData?.printlog(1, "停止服务监听");
                sessionList.Clear();
                ws.Stop();
                ws.Dispose();
                ws= null;
            }
        }

        private void Ws_NewMessageReceived(WebSocketSession session, string value)
        {
            string msg = GetSessionName(session) + "发送数据:" + value;
            GeneralClass.interactivityData?.printlog(1, msg);
            GeneralClass.interactivityData?.servermsg(value);
            //SendToAll(session, msg);
            //throw new NotImplementedException();
        }

        private void Ws_SessionClosed(WebSocketSession session, SuperSocket.SocketBase.CloseReason value)
        {
            string msg = GetSessionName(session) + "关闭连接：" + value.ToString();
            GeneralClass.interactivityData?.printlog(1, msg);
            sessionList.Remove(session);
            SendToAll(session, msg);
            //throw new NotImplementedException();
        }

        private void Ws_NewSessionConnected(WebSocketSession session)
        {
            string msg = GetSessionName(session) + "加入连接";
            sessionList.Add(session);
            GeneralClass.interactivityData?.printlog(1, msg);

            SendToAll(session, msg);
            //throw new NotImplementedException();
        }

        public string GetSessionName(WebSocketSession session)
        {
            return HttpUtility.UrlDecode(session.SessionID);
        }
        private void SendToAll(WebSocketSession session, string msg)
        {
            foreach (var sendsession in session.AppServer.GetAllSessions())
            {
                sendsession.Send(msg);
            }
        }

        public void SendMsg(string msg)
        {
            foreach(WebSocketSession session in sessionList)
            {
                SendToAll(session, msg);
            }
        }
    }
}
