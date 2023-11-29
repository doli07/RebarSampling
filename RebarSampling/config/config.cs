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
        /// 可锯切的最短长度
        /// </summary>
        public int MinLength { get; set; }

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
