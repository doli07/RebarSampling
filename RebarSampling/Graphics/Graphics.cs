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
    public class PaintPic:System.Windows.Forms.Control
    {
        private Graphics p;
        public PaintPic()
        {
            Graphics p = this.CreateGraphics();
        }
        public void DrawLine()
        {
            Pen pen = new Pen(new SolidBrush(Color.Red), 2);//线条粗细
            Point p1= new Point(0, 0);
            Point p2= new Point(100, 100);
            p.DrawLine(pen, p1, p2);

        }
    }
}
