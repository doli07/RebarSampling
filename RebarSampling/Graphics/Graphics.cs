using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace RebarSampling
{
    /// <summary>
    /// 画图类，用于绘制钢筋
    /// </summary>
    public class graphics
    {

        public static Bitmap PaintRebar(RebarOri _rebarlist)
        {
            Bitmap bitmap = new Bitmap(650, 30);//新建一个bitmap，用于绘图

            //Graphics g = this.pictureBox1.CreateGraphics();
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);//从bitmap中建一个画图对象
            g.Clear(Color.White);

            int maxPointX = 600;
            int startY = 25;
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

            foreach (var item in _rebarlist._list)
            {
                _start = (int)((double)_lengthSum / (double)GeneralClass.OriginalLength2 * (double)maxPointX);
                _end = (int)((double)(_lengthSum + item.length) / (double)GeneralClass.OriginalLength2 * (double)maxPointX);

                _pen = new Pen(new SolidBrush(Color.Green), 3);
                p1 = new Point(_start, startY);
                p2 = new Point(_end, startY);
                g.DrawLine(_pen, p1, p2);//画绿线

                _pen = new Pen(new SolidBrush(Color.Black), 3);
                p1 = new Point(_start, startY);
                p2 = new Point(_start, startY - 5);
                g.DrawLine(_pen, p1, p2);//画竖向的小黑线

                text = item.Length;
                fontX = (_start + _end) / 2;
                fontY = startY - 20;
                g.DrawString(text, _font, _brush, fontX, fontY);//写线段长度

                _lengthSum += item.length;
            }

            if (_lengthSum < GeneralClass.OriginalLength2)
            {
                if((GeneralClass.OriginalLength2-_lengthSum)<300)//小于300，当废料处理，用红色线
                {
                    _start = (int)((double)_lengthSum / (double)GeneralClass.OriginalLength2 * (double)maxPointX);
                    _end = (int)((double)(GeneralClass.OriginalLength2) / (double)GeneralClass.OriginalLength2 * (double)maxPointX);

                    _pen = new Pen(new SolidBrush(Color.Red), 3);
                    p1 = new Point(_start, startY);
                    p2 = new Point(_end, startY);
                    g.DrawLine(_pen, p1, p2);//画红线

                    _pen = new Pen(new SolidBrush(Color.Black), 3);
                    p1 = new Point(_start, startY);
                    p2 = new Point(_start, startY - 5);
                    g.DrawLine(_pen, p1, p2);//画竖向的小黑线

                    text = (GeneralClass.OriginalLength2 - _lengthSum).ToString();
                    fontX = (_start + _end) / 2;
                    fontY = startY - 20;
                    _brush = new SolidBrush(Color.Red);
                    g.DrawString(text, _font, _brush, fontX, fontY);//写线段长度
                }
                else//大于等于300，当余料处理，用蓝色线
                {
                    _start = (int)((double)_lengthSum / (double)GeneralClass.OriginalLength2 * (double)maxPointX);
                    _end = (int)((double)(GeneralClass.OriginalLength2) / (double)GeneralClass.OriginalLength2 * (double)maxPointX);

                    _pen = new Pen(new SolidBrush(Color.Blue), 3);
                    p1 = new Point(_start, startY);
                    p2 = new Point(_end, startY);
                    g.DrawLine(_pen, p1, p2);//画蓝线

                    _pen = new Pen(new SolidBrush(Color.Black), 3);
                    p1 = new Point(_start, startY);
                    p2 = new Point(_start, startY - 5);
                    g.DrawLine(_pen, p1, p2);//画竖向的小黑线

                    text = (GeneralClass.OriginalLength2 - _lengthSum).ToString();
                    fontX = (_start + _end) / 2;
                    fontY = startY - 20;
                    _brush = new SolidBrush(Color.Blue);
                    g.DrawString(text, _font, _brush, fontX, fontY);//写线段长度
                }


            }

            return bitmap;
        }
    }
}
