using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using iText.Kernel.Pdf;
//using iText.Layout;
//using iText.Layout.Element;
//using iText.IO.Image;
using iTextSharp.text;
using iTextSharp.text.pdf;
//using System.Drawing;



namespace RebarSampling.labelPrint
{
    public class ImageTopdf
    {
        //public static void ConvertImageToPdf(string imagePath, string pdfPath)
        //{
        //    // 创建一个PdfWriter对象，指定输出文件路径
        //    using (PdfWriter writer = new PdfWriter(pdfPath))
        //    {
        //        // 创建一个PdfDocument对象
        //        using (PdfDocument pdf = new PdfDocument(writer))
        //        {
        //            // 创建一个Document对象
        //            Document document = new Document(pdf);

        //            // 加载图像
        //            ImageData imageData = ImageDataFactory.Create(imagePath);
        //            iText.Layout.Element.Image pdfImage = new iText.Layout.Element.Image(imageData);

        //            // 将图像添加到PDF文档中
        //            document.Add(pdfImage);

        //            // 关闭文档
        //            document.Close();
        //        }
        //    }
        //}

        public static void ConvertImageToPdf(string imagePath, string pdfPath)
        {
            using (FileStream stream = new FileStream(pdfPath, FileMode.Create))
            {
                Document pdfDoc = new Document();
                PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();

                //using (Image img = Image.FromFile(imagePath))
                //{
                iTextSharp.text.Image pdfImg = iTextSharp.text.Image.GetInstance(imagePath);
                pdfImg.ScaleToFit(pdfDoc.PageSize.Width - pdfDoc.LeftMargin - pdfDoc.RightMargin, pdfDoc.PageSize.Height - pdfDoc.TopMargin - pdfDoc.BottomMargin);
                pdfImg.Alignment = Image.ALIGN_CENTER;
                pdfDoc.Add(pdfImg);
                //}

                pdfDoc.Close();
            }
        }
    }
}
