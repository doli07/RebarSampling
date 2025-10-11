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

//using PdfiumViewer;

using RebarSampling.labelPrint;
//using iTextSharp.text.pdf;
//using iText.Kernel.Pdf.Canvas;
//using iText.Kernel.Pdf;
//using iText.Kernel.Pdf.Canvas.Parser;
//using iText.Kernel.Pdf.Xobject;
//using iText.Kernel.Pdf.Canvas;
//using iText.Kernel.Pdf;
//using iText.Kernel.Pdf.Canvas.Parser;
//using iText.Kernel.Pdf.Canvas.Parser.Listener;
//using iTextSharp;
//using PdfiumViewer;

//using System.Windows.Forms;
//using O2S.Components.PDFRender4NET;
//using Seagull.BarTender.Print;

namespace RebarSampling
{


    public class LabelPrint
    {
        static int printNum = 0;//多页打印

        static string printerName = "Gprinter GP-1834T";//打印机名称

        private static string _lbfilepath = Directory.GetCurrentDirectory() + @"\labelfile\" + "构件包标签模板.bmp";
        private static string _qzfilepath = Directory.GetCurrentDirectory() + @"\labelfile\" + "批量包标签模板.bmp";

        private static string _pdfpath = Directory.GetCurrentDirectory() + @"\labelfile\" + "temp.pdf";// 输出的PDF文件路径

        static System.Drawing.Image pic = null;
        public static void print(System.Drawing.Image _image, EnumLabelType _labeltype)
        {
            //图片保存到文件路径中
            if (_labeltype == EnumLabelType.LB_LABEL||_labeltype==EnumLabelType.ELEMENT_LABEL)
            {
                _image.Save(_lbfilepath, System.Drawing.Imaging.ImageFormat.Bmp);
                //_image.Save(_lbfilepath, System.Drawing.Imaging.ImageFormat.Jpeg);

                ImageTopdf.ConvertImageToPdf(_lbfilepath, _pdfpath);// 将图像转换为PDF
            }
            else if (_labeltype == EnumLabelType.QZ_LABEL)
            {
                _image.Save(_qzfilepath, System.Drawing.Imaging.ImageFormat.Bmp);
                //_image.Save(_qzfilepath, System.Drawing.Imaging.ImageFormat.Jpeg);

                ImageTopdf.ConvertImageToPdf(_qzfilepath, _pdfpath);// 将图像转换为PDF
            }

            pic = _image;

            if (hasPrinter(printerName))
            {
                using (PrintDocument pd = new PrintDocument())
                {
                    pd.PrintPage += new PrintPageEventHandler(PrintElement);
                    //pd.PrintPage += (sender, e) =>
                    //{
                    //};

                    pd.DefaultPageSettings.PrinterSettings.PrinterName = printerName;       //打印机名称
                    //pd.DefaultPageSettings.PaperSize = new PaperSize("钢筋", mm2inch(GeneralClass.LabelPrintSizeWidth), mm2inch(GeneralClass.LabelPrintSizeHeight));
                    pd.DefaultPageSettings.PaperSize = new PaperSize("钢筋", mm2inch(GeneralClass.LabelPrintSizeWidth), _image.Height + 150);//以标签的实际大小来设置打印区域大小
                                                                                                                                           //pd.DefaultPageSettings.Landscape = true;  //设置横向打印，不设置默认是纵向的
                    pd.PrintController = new System.Drawing.Printing.StandardPrintController();
                    pd.Print();
                }

                //using (PDFFile file = PDFFile.Open(_pdfpath))
                //{
                //    PrinterSettings settings = new PrinterSettings();
                //    if (printerName != "")
                //    {
                //        settings.PrinterName = printerName;
                //        settings.PrintToFile = false;
                //    }
                //    O2S.Components.PDFRender4NET.Printing.PDFPrintSettings pdfPrintSettings = new O2S.Components.PDFRender4NET.Printing.PDFPrintSettings(settings);
                //    pdfPrintSettings.PaperSize = new PaperSize("BIM标签", mm2inch(GeneralClass.LabelPrintSizeWidth), mm2inch(GeneralClass.LabelPrintSizeHeight));
                //    pdfPrintSettings.PageScaling = O2S.Components.PDFRender4NET.Printing.PageScaling.FitToPrinterMarginsProportional;
                //    pdfPrintSettings.PrinterSettings.Copies = 1;
                //    file.Print(pdfPrintSettings);
                //}



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

            System.Drawing.Rectangle r = e.PageBounds;
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
