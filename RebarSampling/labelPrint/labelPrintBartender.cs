using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Seagull.BarTender.Print;

namespace RebarSampling.labelPrint
{
    public class LabelPrintBartender
    {
        private Engine _engine = null;//bartender print engine
        private LabelFormatDocument _format = null;//current open format

        private string _printerName = "Gprinter GP-1834T";

        private string _jobName1 = "QZ label print";
        private string _jobName2 = "LB label print";

        //public static string filepath = Directory.GetCurrentDirectory() + @"\configfile\config.json";

        private string _lbfilepath = Directory.GetCurrentDirectory() + @"\labelfile\" + "构件包标签模板.btw";
        private string _qzfilepath = Directory.GetCurrentDirectory() + @"\labelfile\" + "批量包标签模板.btw";

        private string _saveThumbnailpath = Directory.GetCurrentDirectory() + @"\labelfile\" + "thumbnail.jpg";//缩略图保存路径

        public LabelPrintBartender()
        {
            try
            {
                _engine = new Engine(true);//true表示启动引擎
            }
            catch (PrintEngineException ex)
            {
                MessageBox.Show("labelprintBartender error:" + ex.Message);
            }
        }

        ~LabelPrintBartender()
        {
            if (_engine != null)
            {
                _engine.Stop(SaveOptions.DoNotSaveChanges);//关闭引擎，不保存改变
                _engine.Dispose();//注意要回收资源
            }
        }

        private bool hasPrinter(string printer)
        {
            if (printer == "") return true;
            int n = PrinterSettings.InstalledPrinters.Count;
            for (int i = 0; i < n; i++)
            {
                if (printer == PrinterSettings.InstalledPrinters[i])
                {
                    return true;
                }
            }
            return false;
        }
        public void print(EnumLabelType _labeltype)
        {
            lock (_engine)
            //using (_engine = new Engine(true))//true表示启动引擎
            {
                if (_format != null)
                {
                    _format.Close(SaveOptions.DoNotSaveChanges);
                }
                if (_labeltype == EnumLabelType.QZ_LABEL)
                {
                    _format = _engine.Documents.Open(_qzfilepath);//open file
                }
                else if (_labeltype == EnumLabelType.LB_LABEL)
                {
                    _format = _engine.Documents.Open(_lbfilepath);//open file
                }

                _format.PrintSetup.IdenticalCopiesOfLabel = 1;//Identical Copies must be an integer greater than or equal to 1
                _format.PrintSetup.NumberOfSerializedLabels = 1;//Serialized Copies must be an integer greater than or equal to 1

                _format.PrintSetup.PrinterName = _printerName;//set printerName

                GeneralClass.interactivityData?.printlog(1, "label print start...");

                Messages msgs;
                int timeout = 10000;
                Result _result = _format.Print(_jobName1, timeout, out msgs);//打印，超时时间10s

                string Msg = "";
                foreach (Seagull.BarTender.Print.Message item in msgs)
                {
                    Msg += item.ToString();
                }

                if (_result == Result.Failure)
                {
                    MessageBox.Show("label print failed:" + Msg, _jobName1, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //MessageBox.Show("label print success!"+Msg, _jobName1);
                    GeneralClass.interactivityData?.printlog(1, "label print success!" + Msg);
                }

                //_engine.Stop(SaveOptions.DoNotSaveChanges);//关闭引擎，不保存改变

            }
        }
        /// <summary>
        /// 获取标签缩略图
        /// </summary>
        /// <param name="_labeltype"></param>
        /// <returns></returns>
        public System.Drawing.Bitmap showThumbnail(EnumLabelType _labeltype)
        {
            #region method1
            //System.Drawing.Image _image = null;

            //if (_labeltype == EnumLabelType.LB_LABEL)
            //{
            //    _image = LabelFormatThumbnail.Create(_lbfilepath, Color.Gray, 600, 600);//直接由format生成缩略图

            //}
            //else if (_labeltype == EnumLabelType.QZ_LABEL)
            //{
            //    _image = LabelFormatThumbnail.Create(_qzfilepath, Color.Gray, 600, 600);//直接由format生成缩略图
            //}

            //Bitmap _returnBmp = new Bitmap(_image);
            //return _returnBmp;
            #endregion


            #region method2

            Bitmap bmp = null;

            //using (_engine = new Engine(true))
            {
                //打开对应的标签文件
                if (_format != null)
                {
                    _format.Close(SaveOptions.DoNotSaveChanges);
                }
                if (_labeltype == EnumLabelType.QZ_LABEL)
                {
                    _format = _engine.Documents.Open(_qzfilepath);//open file
                }
                else if (_labeltype == EnumLabelType.LB_LABEL)
                {
                    _format = _engine.Documents.Open(_lbfilepath);//open file
                }

                //导出缩略图到jpg图片
                if (_format != null)
                {
                    _format.ExportImageToFile(_saveThumbnailpath, ImageType.BMP,
                        Seagull.BarTender.Print.ColorDepth.ColorDepth24bit, new Resolution(500, 500), OverwriteOptions.Overwrite);//导出缩略图到jpg图片文件

                    System.Drawing.Image _image = System.Drawing.Image.FromFile(_saveThumbnailpath);//从jpg图片文件导入成图片bitmap
                    bmp = new Bitmap(_image);
                }

                //_engine.Stop();
            }

            return bmp;

            #endregion


        }


    }




}

