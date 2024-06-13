using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Web.UI.WebControls;
using static System.Windows.Forms.AxHost;
using ZXing;
using ZXing.Common;
using ZXing.QrCode.Internal;
using ZXing.QrCode;
using NPOI.POIFS.Storage;
using NPOI.HPSF;

namespace RebarSampling
{
    /// <summary>
    /// 画图类，用于绘制钢筋
    /// </summary>
    public class graphics
    {
        /// <summary>
        /// 画一根钢筋
        /// </summary>
        /// <param name="_rebarOri"></param>
        /// <returns></returns>
        public static Bitmap PaintRebar(RebarOri _rebarOri)
        {
            Bitmap bitmap = new Bitmap(650, 30);//新建一个bitmap，用于绘图

            //Graphics g = this.pictureBox1.CreateGraphics();
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);//从bitmap中建一个画图对象
            g.Clear(Color.White);

            int maxPointX = 600;
            int startY = 20;
            int _start = 0;
            int _end = 0;
            int _lengthSum = 0;


            Pen _pen;
            Point p1, p2;

            string text;
            int fontsize = 10;
            int fontX = 0;
            int fontY = 0;

            Font _font = new Font("微软雅黑", fontsize, FontStyle.Regular);
            Brush _brush = new SolidBrush(Color.Green);

            try
            {
                //画原材里面的每一段钢筋
                foreach (var item in _rebarOri._list)
                {
                    //_start = (int)((double)_lengthSum / (double)_rebarlist._totalLength * (double)maxPointX);
                    //_end = (int)((double)(_lengthSum + item.length) / (double)_rebarlist._totalLength * (double)maxPointX);
                    _start = (int)((double)_lengthSum / (double)GeneralClass.OriginalLength * (double)maxPointX);
                    _end = (int)((double)(_lengthSum + item.length) / (double)GeneralClass.OriginalLength * (double)maxPointX);

                    _pen = new Pen(new SolidBrush(Color.Green), item.PickUsed?6:3);//看rebar是否处于拖拽选中状态，如果是则加粗
                    p1 = new Point(_start, startY);
                    p2 = new Point(_end, startY);
                    g.DrawLine(_pen, p1, p2);//画绿线，钢筋小段

                    _pen = new Pen(new SolidBrush(Color.Black), 3);
                    p1 = new Point(_start, startY);
                    p2 = new Point(_start, startY - 5);
                    g.DrawLine(_pen, p1, p2);//画竖向的小黑线，分隔线

                    text = item.Length;
                    fontX = (_start + _end) / 2;
                    fontY = startY - 20;
                    g.DrawString(text, _font, _brush, fontX, fontY);//写线段长度，文本标注

                    //拆解corner信息,准备绘制两端套丝和弯曲标识
                    List<GeneralMultiData> _MultiData = SQLiteOpt.GetMultiData(item.CornerMessage, item.Diameter);
                    if (_MultiData != null && _MultiData.Count != 0)//可以画弯曲套丝的就画，画不了的就算了
                    {
                        if (_MultiData.First().type == 2)//端头套丝
                        {
                            _pen = new Pen(new SolidBrush(Color.Black), 6);
                            p1 = new Point(_start, startY);
                            p2 = new Point(_start + 10, startY);
                            g.DrawLine(_pen, p1, p2);//画黑色加粗线，标识套丝
                        }
                        if (_MultiData.Last().type == 2)//端尾套丝
                        {
                            _pen = new Pen(new SolidBrush(Color.Black), 6);
                            p1 = new Point(_end - 10, startY);
                            p2 = new Point(_end, startY);
                            g.DrawLine(_pen, p1, p2);//画黑色加粗线，标识套丝
                        }
                        int _bendlength = 0;
                        foreach (var ttt in _MultiData)
                        {
                            _bendlength += ttt.ilength;
                            if (ttt.type == 1)//画弯曲
                            {
                                int _bendpos = (int)((double)(_lengthSum + _bendlength) / (double)GeneralClass.OriginalLength * (double)maxPointX);
                                _pen = new Pen(new SolidBrush(Color.Black), 2);
                                p1 = new Point(_bendpos - 5, startY + 7);
                                p2 = new Point(_bendpos, startY + 2);
                                g.DrawLine(_pen, p1, p2);
                                p1 = new Point(_bendpos, startY + 2);
                                p2 = new Point(_bendpos + 5, startY + 7);
                                g.DrawLine(_pen, p1, p2);//画黑色折线，标识弯曲
                            }
                        }
                    }

                    _lengthSum += item.length;//计算累加长度
                }

                //画余料和废料
                if (_lengthSum < _rebarOri._totalLength)
                {
                    //画二次利用的长度
                    if (_rebarOri._lengthSecondUsed != 0)
                    {
                        foreach(var item in _rebarOri._secondUsedList)//二次利用的可能有多段
                        {
                            _start = (int)((double)_lengthSum / (double)GeneralClass.OriginalLength * (double)maxPointX);
                            _end = (int)((double)(_lengthSum + item) / (double)GeneralClass.OriginalLength * (double)maxPointX);

                            _pen = new Pen(new SolidBrush(Color.Yellow), 3);
                            p1 = new Point(_start, startY);
                            p2 = new Point(_end, startY);
                            g.DrawLine(_pen, p1, p2);//画黄色线

                            _pen = new Pen(new SolidBrush(Color.Black), 3);
                            p1 = new Point(_start, startY);
                            p2 = new Point(_start, startY - 5);
                            g.DrawLine(_pen, p1, p2);//画竖向的小黑线

                            text = item.ToString();
                            fontX = (_start + _end) / 2;
                            fontY = startY - 20;
                            _brush = new SolidBrush(Color.Green);
                            g.DrawString(text, _font, _brush, fontX, fontY);//写线段长度

                            _lengthSum += item;//计算累加长度
                        }
                    }

                    if ((/*GeneralClass.OriginalLength*/ _rebarOri._totalLength - _lengthSum) <= 300)//小于300，当废料处理，用红色线
                    {
                        //_start = (int)((double)_lengthSum / (double)_rebarlist._totalLength * (double)maxPointX);
                        //_end = (int)((double)(_rebarlist._totalLength) / (double)_rebarlist._totalLength * (double)maxPointX);
                        _start = (int)((double)_lengthSum / (double)GeneralClass.OriginalLength * (double)maxPointX);
                        _end = (int)((double)(_rebarOri._totalLength/*GeneralClass.OriginalLength*/) / (double)GeneralClass.OriginalLength * (double)maxPointX);

                        _pen = new Pen(new SolidBrush(Color.Red), 3);
                        p1 = new Point(_start, startY);
                        p2 = new Point(_end, startY);
                        g.DrawLine(_pen, p1, p2);//画红线

                        _pen = new Pen(new SolidBrush(Color.Black), 3);
                        p1 = new Point(_start, startY);
                        p2 = new Point(_start, startY - 5);
                        g.DrawLine(_pen, p1, p2);//画竖向的小黑线

                        text = (_rebarOri._totalLength - _lengthSum).ToString();
                        fontX = (_start + _end) / 2;
                        fontY = startY - 20;
                        _brush = new SolidBrush(Color.Red);
                        g.DrawString(text, _font, _brush, fontX, fontY);//写线段长度
                    }
                    else//大于等于300，当余料处理，用蓝色线
                    {
                        _start = (int)((double)_lengthSum / (double)GeneralClass.OriginalLength * (double)maxPointX);
                        _end = (int)((double)(_rebarOri._totalLength/*GeneralClass.OriginalLength*/) / (double)GeneralClass.OriginalLength * (double)maxPointX);

                        _pen = new Pen(new SolidBrush(Color.Blue), 3);
                        p1 = new Point(_start, startY);
                        p2 = new Point(_end, startY);
                        g.DrawLine(_pen, p1, p2);//画蓝线

                        _pen = new Pen(new SolidBrush(Color.Black), 3);
                        p1 = new Point(_start, startY);
                        p2 = new Point(_start, startY - 5);
                        g.DrawLine(_pen, p1, p2);//画竖向的小黑线

                        text = (/*GeneralClass.OriginalLength*/ _rebarOri._totalLength - _lengthSum).ToString();
                        fontX = (_start + _end) / 2;
                        fontY = startY - 20;
                        _brush = new SolidBrush(Color.Blue);
                        g.DrawString(text, _font, _brush, fontX, fontY);//写线段长度

                    }


                }

            }
            catch (Exception ex) { MessageBox.Show("PaintRebar error:" + ex.Message); }

            return bitmap;

        }
        /// <summary>
        /// 画线材
        /// </summary>
        /// <param name="_rebar"></param>
        /// <returns></returns>
        public static Bitmap PaintRebarXian(Rebar _rebar)
        {
            Bitmap bitmap = new Bitmap(GeneralClass.taoPicSize.Width,GeneralClass.taoPicSize.Height);//新建一个bitmap，用于绘图

            Graphics g = System.Drawing.Graphics.FromImage(bitmap);//从bitmap中建一个画图对象
            g.Clear(Color.White);

            int maxPointX = 600;
            int startY = 20;
            int _start = 0;
            int _end = 0;
            int _lengthSum = 0;

            Pen _pen;
            Point p1, p2;

            string text;
            int fontsize = 10;
            int fontX = 0;
            int fontY = 0;

            Font _font = new Font("微软雅黑", fontsize, FontStyle.Regular);
            Brush _brush = new SolidBrush(Color.Green);

            try
            {
                _start = (int)((double)_lengthSum / (double)GeneralClass.OriginalLength * (double)maxPointX);
                _end = (int)((double)(_lengthSum + _rebar.length) / (double)GeneralClass.OriginalLength * (double)maxPointX);

                _pen = new Pen(new SolidBrush(Color.Green), 3);
                p1 = new Point(_start, startY);
                p2 = new Point(_end, startY);
                g.DrawLine(_pen, p1, p2);//画绿线，钢筋小段

                _pen = new Pen(new SolidBrush(Color.Black), 3);
                p1 = new Point(_start, startY);
                p2 = new Point(_start, startY - 5);
                g.DrawLine(_pen, p1, p2);//画竖向的小黑线，分隔线

                text = _rebar.Length;
                fontX = (_start + _end) / 2;
                fontY = startY - 20;
                g.DrawString(text, _font, _brush, fontX, fontY);//写线段长度，文本标注

                //拆解corner信息,准备绘制两端套丝和弯曲标识
                List<GeneralMultiData> _MultiData = SQLiteOpt.GetMultiData(_rebar.CornerMessage, _rebar.Diameter);
                if (_MultiData != null && _MultiData.Count != 0)//可以画弯曲套丝的就画，画不了的就算了
                {
                    if (_MultiData.First().type == 2)//端头套丝
                    {
                        _pen = new Pen(new SolidBrush(Color.Black), 6);
                        p1 = new Point(_start, startY);
                        p2 = new Point(_start + 10, startY);
                        g.DrawLine(_pen, p1, p2);//画黑色加粗线，标识套丝
                    }
                    if (_MultiData.Last().type == 2)//端尾套丝
                    {
                        _pen = new Pen(new SolidBrush(Color.Black), 6);
                        p1 = new Point(_end - 10, startY);
                        p2 = new Point(_end, startY);
                        g.DrawLine(_pen, p1, p2);//画黑色加粗线，标识套丝
                    }
                    int _bendlength = 0;
                    foreach (var ttt in _MultiData)
                    {
                        _bendlength += ttt.ilength;
                        if (ttt.type == 1)//画弯曲
                        {
                            int _bendpos = (int)((double)(_lengthSum + _bendlength) / (double)GeneralClass.OriginalLength * (double)maxPointX);
                            _pen = new Pen(new SolidBrush(Color.Black), 2);
                            p1 = new Point(_bendpos - 5, startY + 7);
                            p2 = new Point(_bendpos, startY + 2);
                            g.DrawLine(_pen, p1, p2);
                            p1 = new Point(_bendpos, startY + 2);
                            p2 = new Point(_bendpos + 5, startY + 7);
                            g.DrawLine(_pen, p1, p2);//画黑色折线，标识弯曲
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("PaintRebarXian error:" + ex.Message+",cornerMsg:"+_rebar.CornerMessage); }

            return bitmap;

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
        /// <summary>
        /// 打印构件的标签
        /// </summary>
        /// <returns></returns>
        public static Bitmap PaintElementLabel(ElementRebarFB _element)
        {
            Bitmap bitmap = new Bitmap(mm2inch(50), mm2inch(100));//新建一个bitmap，用于绘图

            Graphics g = System.Drawing.Graphics.FromImage(bitmap);//从bitmap中建一个画图对象
            g.Clear(Color.White);            
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;// 设置高质量插值法              
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;// 设置高质量平滑度  

            int fontsize = 0;
            //Font _font;
            Brush _brush;

            Pen _pen;
            Point p1, p2;

            fontsize = 15;
            PaintString(ref g, "构件标签", 50, 2, fontsize, FontStyle.Bold);

            //画绿线
            _brush = new SolidBrush(Color.Green);
            _pen = new Pen(new SolidBrush(Color.Green), 3);
            p1 = new Point(0, 30);
            p2 = new Point(350, 30);
            g.DrawLine(_pen, p1, p2);

            PaintString(ref g, "项目名称：", 10, 40);//项目名称
            PaintString(ref g, "杨春湖华侨城", 80, 40);//杨春湖华侨城
            PaintString(ref g, "楼栋号：", 10, 60);//项目名称
            PaintString(ref g, "12#楼", 80, 60);//杨春湖华侨城
            PaintString(ref g, "楼层部位：", 10, 80);//项目名称
            PaintString(ref g, "十七层柱接筋", 80, 80);//杨春湖华侨城
            PaintString(ref g, "构件名称：", 10, 100);//项目名称
            PaintString(ref g, "3#GBZ2", 80, 100);//杨春湖华侨城

            PaintString(ref g, "根数：", 10, 120);//项目名称
            PaintString(ref g, "34", 80, 120);//杨春湖华侨城
            PaintString(ref g, "重量(Kg)：", 10, 140);//项目名称
            PaintString(ref g, "57.4", 80, 140);//杨春湖华侨城

            PaintString(ref g, "加工厂：", 10, 180);//项目名称
            PaintString(ref g, "庙岭加工基地", 80, 180);//杨春湖华侨城
            PaintString(ref g, "加工日期：", 10, 200);//项目名称
            PaintString(ref g, "2024-04-22", 80, 200);//杨春湖华侨城
            PaintString(ref g, "需求日期：", 10, 220);//项目名称
            PaintString(ref g, "2024-04-25", 80, 220);//杨春湖华侨城


            Bitmap _qrCode = CreateQRCode("中国建筑先进技术研究院");
            g.DrawImage(_qrCode, new System.Drawing.Point(80, 240));

            //绘制先进院logo
            Bitmap _sourceImage = global::RebarSampling.Properties.Resources.cscec_xjy;//获取先进院logo
            int _width = 168;
            int _height = 24;            
            double xRatio = (double)_width / _sourceImage.Width;// 计算缩略图的缩放比例  
            double yRatio = (double)_height / _sourceImage.Height;
            double ratio = Math.Min(xRatio, yRatio);            
            g.DrawImage(_sourceImage, 10, 320, (int)(_sourceImage.Width * ratio), (int)(_sourceImage.Height * ratio));// 绘制缩略图  

            return bitmap;
        }

        private static void PaintString(ref Graphics _g, string _text, int _startX, int _startY, int _fontsize = 10, FontStyle _style = FontStyle.Regular)
        {
            Brush _brush;
            Font _font;

            _font = new Font("微软雅黑", _fontsize, _style);
            _brush = new SolidBrush(Color.Black);
            _g.DrawString(_text, _font, _brush, _startX, _startY);
        }

        /// <summary>
        /// 二维码方法
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public static Bitmap CreateQRCode(string asset)
        {
            EncodingOptions options = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8", //编码
                Width = 80,             //宽度
                Height = 80             //高度
            };
            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            writer.Options = options;
            return writer.Write(asset);
        }

    }
}
