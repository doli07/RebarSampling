using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RebarSampling
{
    public class ConfigData
    {
        /// <summary>
        /// 工厂类型
        /// </summary>
        public EnumFactoryType Factorytype { get; set; }
        /// <summary>
        /// Φ12直径钢筋归类于线材还是棒材，false为线材，true为棒材
        /// </summary>
        public bool TypeC12 { get; set; }
        /// <summary>
        /// Φ14直径钢筋归类于线材还是棒材，false为线材，true为棒材
        /// </summary>
        public bool TypeC14 { get; set;}
        /// <summary>
        /// 工厂，高效/柔性
        /// </summary>
        public EnumFactory Factory { get; set; }
        /// <summary>
        /// 是否加载短钢筋数据（1.5米以下）
        /// </summary>
        public bool IfShortRebar { get; set; }
        /// <summary>
        /// 可锯切的最短长度
        /// </summary>
        public int MinLength { get; set; }
        /// <summary>
        /// 原材类型，9米/12米
        /// </summary>
        public EnumOriType OriginType { get; set; }
        /// <summary>
        /// 是否切端头
        /// </summary>
        public bool IfCutHead { get; set; }
        /// <summary>
        /// 切端头距离
        /// </summary>
        public int CutHeadLength { get; set; }
        /// <summary>
        /// 直径种类分组方式，顺序分组、混合分组
        /// </summary>
        public EnumDiaGroupTypeSetting DiaGroupType { get; set; }
        /// <summary>
        /// webserver的ip地址，默认为本机127.0.0.1
        /// </summary>
        public string webserverIP { get; set; }
        /// <summary>
        /// webserver的监听端口
        /// </summary>
        public string webserverPort { get; set;}

        /// <summary>
        /// 余料阈值
        /// </summary>
        public int LeftThreshold { get; set; }
        /// <summary>
        /// 余料最大阈值
        /// </summary>
        public int MaxThreshold { get; set; }
        /// <summary>
        /// 套料算法方式
        /// </summary>
        public EnumTaoType TaoType { get; set; }
        /// <summary>
        /// 原材库设置，切换整数倍模数和等间距模数
        /// </summary>
        public EnumMatPoolSetType MatPoolSetType { get; set; }
        /// <summary>
        /// 纳入原材库的余料1
        /// </summary>
        public int MatPoolYuliao1 { get; set; }
        /// <summary>
        /// 纳入原材库的余料2
        /// </summary>
        public int MatPoolYuliao2 { get; set; }
    }
    public class Config
    {
        public static string filepath = Directory.GetCurrentDirectory() + @"\configfile\config.json";

        static ReaderWriterLockSlim LogWriteLock = new ReaderWriterLockSlim();

        public static void SaveConfig(string _json)
        {
            try
            {
                LogWriteLock.EnterWriteLock();

                if (!File.Exists(filepath))
                {
                    File.Create(filepath);
                }
                if(File.Exists(filepath))
                {
                    File.WriteAllText(filepath, _json);
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
            finally { LogWriteLock.ExitWriteLock(); }
        }

        public static string LoadConfig()
        {
            try
            {
                LogWriteLock.EnterWriteLock();

                string rt = "";

                if (File.Exists(filepath))
                {
                    rt = File.ReadAllText(filepath);
                }
                return rt;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                return "";
            }
            finally { LogWriteLock.ExitWriteLock();  }


        }
    }
}
