using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace RebarSampling
{
    public class Logfile
    {
        public static string AppPath = Application.StartupPath;


        static ReaderWriterLockSlim LogWriteLock = new ReaderWriterLockSlim();

        /// <summary>
        /// 保存本地log 
        /// type:  1-操作记录    2-上传记录   3-历史数据    4-标签内容   5-流水号内容    6-标签模板csv
        /// record：数据内容
        /// item：保存标签模板csv用的字段名
        /// </summary>
        /// <param name="record"></param>
        /// <param name="type"></param>
        public static void SaveLog(string record, int type, string item = "")
        {
            try
            {
                LogWriteLock.EnterWriteLock();

                string recordDate = DateTime.Now.ToString("yyyy-MM-dd");
                string recordTime = DateTime.Now.ToString("HH:mm:ss.fff");
                string filePath = "";

                switch (type)
                {
                    #region case 1:操作记录
                    case 1:
                        {
                            filePath = AppPath + @"\logfile\操作记录\" + recordDate + ".txt";

                            if (!File.Exists(filePath))
                            {
                                File.Create(filePath).Dispose();
                            }
                            if (File.Exists(filePath))
                            {
                                FileStream file;
                                file = new FileStream(filePath, FileMode.Append);
                                StreamWriter writeFile = new StreamWriter(file);
                                writeFile.WriteLine(recordTime + "-->" + record);
                                writeFile.Flush();
                                writeFile.Close();
                                file.Close();
                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("文件路径:" + filePath + "不存在!");
                            }
                        }
                        break;
                    #endregion
                    #region case 2:上传记录
                    case 2:
                        {
                            filePath = AppPath + @"\logfile\上传记录\" + recordDate + ".txt";

                        }
                        break;
                    #endregion
                    #region case 3:历史数据
                    case 3:
                        {
                            filePath = AppPath + @"\logfile\历史数据\" + recordDate + ".txt";

                            if (!File.Exists(filePath))
                            {
                                File.Create(filePath).Dispose();
                            }
                            if (File.Exists(filePath))
                            {
                                FileStream file;
                                file = new FileStream(filePath, FileMode.Append);
                                StreamWriter writeFile = new StreamWriter(file);
                                writeFile.WriteLine(recordTime + "-->" + record);
                                writeFile.WriteLine("------------------------------------------------");
                                writeFile.Flush();
                                writeFile.Close();
                                file.Close();
                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("文件路径:" + filePath + "不存在!");
                            }
                        }
                        break;
                    #endregion
                    #region case 4:标签内容
                    case 4:
                        {
                            filePath = AppPath + @"\logfile\标签内容\" + recordDate + ".txt";

                        }
                        break;
                    #endregion
                    #region case 5:流水号内容
                    case 5:
                        {
                            filePath = AppPath + @"\logfile\流水号内容\" + recordDate + ".csv";

                        }
                        break;
                    #endregion
                    //#region case 6:标签模板csv
                    //case 6:
                    //    {
                    //        filePath = AppPath + @"\logfile\标签模板\标签模板.csv";

                    //        if (!File.Exists(filePath))
                    //        {
                    //            File.Create(filePath).Dispose();
                    //            WriteCSV(filePath, item);

                    //        }
                    //        if (File.Exists(filePath))
                    //        {
                    //            WriteCSV(filePath, record);
                    //            //FileStream file;
                    //            //file = new FileStream(filePath, FileMode.Append);
                    //            //StreamWriter writeFile = new StreamWriter(file);
                    //            //writeFile.WriteLine(recordTime + "-->" + record);
                    //            //writeFile.Flush();
                    //            //writeFile.Close();
                    //            //file.Close();
                    //        }
                    //        else
                    //        {
                    //            System.Windows.Forms.MessageBox.Show("文件路径:" + filePath + "不存在!");
                    //        }
                    //    }
                    //    break;
                    //#endregion

                }




            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
            finally
            {
                LogWriteLock.ExitWriteLock();
            }

        }


        ///// <summary>
        ///// 保存记录
        ///// </summary>
        ///// <param name="type">保存信息的类别 1:报警代码 2:操作记录(修改参数:马达,坐标,换料,) 
        /////                                   3:产量统计 4:运行记录(软件开启时间,关闭时间,软件异常发生情况)</param>
        ///// <param name="str"></param>
        //public static void WriteRecord(int type, string str)
        //{
        //    // object obj = new object();
        //    //  lock (obj)
        //    try
        //    {
        //        LogWriteLock.EnterWriteLock();
        //        //所有文件按日期进行保存,不然放在一起会很大,打开时会很慢
        //        StringBuilder filename = new StringBuilder();
        //        string recorddate = string.Format("{0:D4}年{1:D2}月{2:D2}日", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        string recordtime = string.Format("{0:D2}:{1:D2}:{2:D2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        switch (type)
        //        {
        //            case 1:
        //                filename.Append(GlobClass.AppPath + @"\Data\报警记录\" + recorddate + ".txt");
        //                if (!File.Exists(filename.ToString()))
        //                {
        //                    File.Create(filename.ToString()).Dispose();
        //                }
        //                break;
        //            case 2:
        //                filename.Append(GlobClass.AppPath + @"\Data\操作记录\" + recorddate + ".txt");

        //                if (!File.Exists(filename.ToString()))
        //                {
        //                    File.Create(filename.ToString()).Dispose();
        //                    // filename.Append(GlobClass.AppPath + @"\Data\操作记录\" + recorddate + ".txt");
        //                }
        //                break;
        //            case 3:
        //                filename.Append(GlobClass.AppPath + @"\Data\产量统计\" + recorddate + ".csv");

        //                if (!File.Exists(filename.ToString()))
        //                {
        //                    File.Create(filename.ToString()).Dispose();
        //                    List<string> temp = new List<string>() { "时间", "IMEM码", "二维码", "状态", "详细" };
        //                    WriteCSV(filename.ToString(), temp);
        //                }
        //                else
        //                {
        //                    //List<string> temp = new List<string>() {productinfo.ProductTime,productinfo.ProductImem)
        //                    //productinfo.ProductQR, productinfo.ProductNG, productinfo.ProductDetal };
        //                    // WriteCSV(filename.ToString(), temp);
        //                }
        //                return;
        //            case 4:
        //                filename.Append(GlobClass.AppPath + @"\Data\运行记录\" + recorddate + ".txt");
        //                break;
        //        }
        //        using (StreamWriter file = new StreamWriter(filename.ToString(), true))
        //        {
        //            file.WriteLine(recordtime + "     " + str);
        //        }
        //    }
        //    catch (Exception m)
        //    {
        //        MessageBox.Show(m.Message);
        //    }
        //    finally
        //    {
        //        LogWriteLock.ExitWriteLock();
        //    }
        //}




        /// <summary>
        /// 写入csv
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        public static bool WriteCSV(string filePath, string record)
        {
            try
            {
                using (StreamWriter fileWriter = new StreamWriter(filePath, true, Encoding.Default))
                {
                    fileWriter.WriteLine(record);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 读csv
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<String[]> ReadCSV(string filePath)
        {
            List<String[]> ls = new List<String[]>();
            StreamReader fileReader = new StreamReader(filePath);
            string strLine = "";
            while (strLine != null)
            {
                strLine = fileReader.ReadLine();
                if (strLine != null && strLine.Length > 0)
                {
                    ls.Add(strLine.Split(','));
                }
            }
            fileReader.Close();
            return ls;
        }





    }
}
