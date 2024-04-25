using NPOI.OpenXmlFormats.Wordprocessing;
//using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RebarSampling
{


    public class LabelPrint
    {
        static int printNum = 0;//多页打印

        static string printerName = "Gprinter GP-1834T";//打印机名称

        static System.Drawing.Image pic = null;
        public static void print(System.Drawing.Image _image)
        {
            pic = _image;

            if(hasPrinter(printerName))
            {
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(PrintElement);

                pd.DefaultPageSettings.PrinterSettings.PrinterName = printerName;       //打印机名称
                pd.DefaultPageSettings.PaperSize = new PaperSize("钢筋", mm2inch(50), mm2inch(100));
                //pd.DefaultPageSettings.Landscape = true;  //设置横向打印，不设置默认是纵向的
                pd.PrintController = new System.Drawing.Printing.StandardPrintController();
                pd.Print();
            }
            else
            {
                GeneralClass.interactivityData?.printlog(1, "打印机" + printerName + "未连接!");
            }

        }

        /// <summary>
        /// mm转为inch
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private static int mm2inch(int length)
        {
            return (int)(length / 25.4 * 100);
        }

        static bool hasPrinter(string printer)
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
        private static void PrintElement(object sender, PrintPageEventArgs e)
        {
            //图片抗锯齿
            //e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //Stream fs = new FileStream(fileList[printNum].ToString().Trim(), FileMode.Open, FileAccess.Read);
            //System.Drawing.Image image = System.Drawing.Image.FromStream(fs);

            System.Drawing.Image image = pic;
            //int x = e.MarginBounds.X;
            //int y = e.MarginBounds.Y;
            //int width = image.Width;
            //int height = image.Height;
            //if ((width / e.MarginBounds.Width) > (height / e.MarginBounds.Height))
            //{
            //    width = e.MarginBounds.Width;
            //    height = image.Height * e.MarginBounds.Width / image.Width;
            //}
            //else
            //{
            //    height = e.MarginBounds.Height;
            //    width = image.Width * e.MarginBounds.Height / image.Height;
            //}

            Rectangle r = e.PageBounds;
            double xRatio = (double)r.Width / image.Width;// 计算缩略图的缩放比例  
            double yRatio = (double)r.Height / image.Height;
            double ratio = Math.Min(xRatio, yRatio);
            r.Width = (int)(image.Width * ratio);
            r.Height = (int)(image.Height * ratio);
            e.Graphics.DrawImage(image, r);// 绘制缩略图  





            ////DrawImage参数根据打印机和图片大小自行调整
            ////System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(x, y, width, height);
            ////System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(0, 0, width, height);
            //if (image.Height < 310)
            //{
            //    e.Graphics.DrawImage(image, 0, 30, image.Width + 20, image.Height);
            //    //    System.Drawing.Rectangle destRect1 = new System.Drawing.Rectangle(0, 30, image.Width, image.Height);
            //    //    e.Graphics.DrawImage(image, destRect1, 0, 0, image.Width, image.Height, System.Drawing.GraphicsUnit.Pixel);
            //}
            //else
            //{
            //    e.Graphics.DrawImage(image, 0, 0, image.Width + 20, image.Height);
            //    //    System.Drawing.Rectangle destRect2 = new System.Drawing.Rectangle(0, 0, image.Width, image.Height);
            //    //    e.Graphics.DrawImage(image, destRect2, 0, 0, image.Width, image.Height, System.Drawing.GraphicsUnit.Pixel);
            //}

            ////if (printNum < fileList.Count - 1)
            ////{
            ////    printNum++;
            ////    e.HasMorePages = true;//HasMorePages为true则再次运行PrintPage事件
            ////    return;
            ////}
            ////e.HasMorePages = false;


        }

    }
}
