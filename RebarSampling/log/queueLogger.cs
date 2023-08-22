using NPOI.HSSF.Record;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RebarSampling.log
{

    public enum LogLevel
    {
        debug,
        info,
        warning,
        error,
        fatal
    }

    public class LogMsg
    {
        public string Time { get; set; }
        public LogLevel Level { get; set; }
        public string Message { get; set; }
    }

    public class queueLogger
    {
        public static string AppPath = Application.StartupPath;

        private ConcurrentQueue<LogMsg> _que;

        private ManualResetEvent _cmd;

        private static queueLogger _queuelog = new queueLogger();

        //private StreamWriter debugwriter;
        //private StreamWriter infowriter;
        //private StreamWriter warningwriter;
        //private StreamWriter errorwriter;
        //private StreamWriter fatalwriter;

        //private logger debugLogger;
        //private logger infoLogger;
        //private logger warningLogger;
        //private logger errorLogger;
        //private logger fatalLogger;


        public queueLogger()
        {
            _que = new ConcurrentQueue<LogMsg>();
            _cmd = new ManualResetEvent(false);

            //string recordDate = DateTime.Now.ToString("yyyy-MM-dd");
            //string recordTime = DateTime.Now.ToString("HH:mm:ss.fff");

            //string filePath;
            //filePath = AppPath + @"\logfile\历史数据\" + recordDate + ".txt";
            //debugLogger = new logger(filePath);
            //debugLogger.Open();


            //filePath = AppPath + @"\logfile\操作记录\" + recordDate + ".txt";
            //infoLogger = new logger(filePath);
            //infoLogger.Open();

            //Instance().Register();
        }

        ~queueLogger()
        {
            //debugLogger.Close();
            //infoLogger.Close();
        }
        public static queueLogger Instance()//单例
        {
            return _queuelog;
        }
        public void Register()
        {
            Thread thread = new Thread(new ThreadStart(writelog));
            thread.IsBackground = true;
            thread.Start();
        }

        private void writelog()
        {

            while (true)
            {
                _cmd.WaitOne();

                LogMsg _msg = new LogMsg();
                while (_que.Count > 0 && _que.TryDequeue(out _msg))
                {
                    switch (_msg.Level)
                    {
                        case LogLevel.debug:
                            //debugLogger.Write(_msg.Message);
                            break;
                        case LogLevel.info:
                            Logfile.SaveLog(_msg.Message, 1);
                            //infoLogger.Write(_msg.Message);
                            break;
                        case LogLevel.warning:
                            //warningLogger.Write( _msg.Message);
                            break;
                        case LogLevel.error:
                            //errorLogger.Write( _msg.Message);
                            break;
                        case LogLevel.fatal:
                            //fatalLogger.Write( _msg.Message);
                            break;
                    }


                }

                _cmd.Reset();//重置信号
                Thread.Sleep(1);
            }

        }


        private void EnqueueMsg(string msg, LogLevel _level)
        {
            _que.Enqueue(new LogMsg
            {
                Time = DateTime.Now.ToString("HH:mm:ss.fff"),
                Level = _level,
                Message = msg
            });
            _cmd.Set();//通知线程队列中有数据，需要写log
        }
        public static void Debug(string msg) { Instance().EnqueueMsg(msg, LogLevel.debug); }
        public static void Info(string msg) { Instance().EnqueueMsg(msg, LogLevel.info); }
        public static void Warn(string msg) { Instance().EnqueueMsg(msg, LogLevel.warning); }
        public static void Error(string msg) { Instance().EnqueueMsg(msg, LogLevel.error); }
        public static void Fatal(string msg) { Instance().EnqueueMsg(msg, LogLevel.fatal); }

    }
}
