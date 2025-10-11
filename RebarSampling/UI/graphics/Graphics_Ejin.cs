using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RebarSampling
{
    public partial class graphics
    {
        /// <summary>
        /// 使用e筋自带的图形信息进行画图
        /// 示例：
        ///     箍筋：33/P2 108 5 73 5 73 43 108 43 108 5/L150 90+0 5+16 0 1,570 73+-14 24+6 0 1,150 90+0 43+0 0 1,570 108+4 24+6 0 0/D-135,-90,-90,-90,-135/M3,4,0,0,0/SG
        ///     拉勾：33/P2 64 23 117 23/L170 90+0 23+0 0 1/D90,135/SL
        ///     主筋两头带弯钩：33/P2 38 32 38 15 144 15 144 32/L180 38+-1 23+8 0 2,6380 91+0 15+0 0 1,180 144+3 23+8 0 0/D0,90,90,0
        ///     带圆弧：33/P2 32 45 32 23 51 3 131 3 150 24 150 45/L900 32+0 34+0 -90 9,950 36+-15 8+-9 -46 9,4200 91+0 3+0 0 1,950 145+8 9+-14 47 9,900 150+0 34+0 270 9/D0,45,45,45,45,0/H0,0.44,0,0.44,0,0
        /// </summary>
        /// <param name="_rebar"></param>
        /// <returns></returns>
        public static Bitmap PaintRebarPicUsePicMsg(Rebar _rebar, out int _realheight)
        {
            if (_rebar.RebarShapeType == EnumRebarShapeType.SHAPE_GJ) { _realheight = 40; return PaintRebarPic_GJ(_rebar); }//箍筋还是用老版的画法
            if (_rebar.RebarShapeType == EnumRebarShapeType.SHAPE_LG) { _realheight = 40; return PaintRebarPic_LG(_rebar); }//拉勾还是用老版的画法

            string picMsg = _rebar.PicMessage;//图形信息
            string cornerMsg = _rebar.CornerMessage;//边角信息

            EjinPicMsg _pic = new EjinPicMsg();//建一个ejin数据结构

            string[] msgs = picMsg.Split('/');//每组指令之间以"/"分隔

            foreach (var item in msgs)
            {
                if (item.IndexOf('P') == 0) { ExecuteP(item, ref _pic); }
                if (item.IndexOf('L') == 0) { ExecuteL(item, ref _pic); }
                if (item.IndexOf('M') == 0) { ExecuteM(item, ref _pic); }
                if (item.IndexOf('D') == 0) { ExecuteD(item, ref _pic); }
                if (item.IndexOf('H') == 0) { ExecuteH(item, ref _pic); }
                if (item.IndexOf('S') == 0) { ExecuteS(item, ref _pic); }
            }

            ////20250725，解决拆解多段钢筋的画图问题，不可以直接读【图形信息】进行画图，用之前 的画图方式
            //if(_rebar.CornerMessage!=_rebar.CornerMessageBK)//如果cornermsg和cornermsgBk不一样，则判定为多段
            //{
            //    if (!_rebar.IfBend)
            //    {
            //        _realheight = 40;
            //        return PaintRebarPic_10000(_rebar);
            //    }
            //    else
            //    {
            //        _realheight = 40;
            //        return PaintRebarPic_20000(_rebar);
            //    }
            //}


            //正式开始画图
            _realheight = _pic.realHeight;//实际画图高度
            Bitmap bitmap = new Bitmap(185, _realheight);//新建一个bitmap用于绘图，

            int _bias = _pic.bias;//Y向整体偏移值

            Graphics g = System.Drawing.Graphics.FromImage(bitmap);//从bitmap中建一个画图对象
            g.Clear(Color.White);
            Font _font = new Font("微软雅黑", 10, FontStyle.Regular);
            Brush _brush = new SolidBrush(Color.Black);
            Pen _pen = new Pen(new SolidBrush(Color.Black), 2);//粗细3

            //画直线段或者圆弧
            for (int i = 0; i < _pic.PlinePoints.Count - 1; i++)
            {
                Point _start = new Point(_pic.PlinePoints[i].X, _pic.PlinePoints[i].Y + _bias);//画线段要把整体Y偏移值加上
                Point _end = new Point(_pic.PlinePoints[i + 1].X, _pic.PlinePoints[i + 1].Y + _bias);

                if (_pic.HArc.Count != 0 && _pic.HArc[i] != 0)//画圆弧
                {
                    Point _base = new Point(Math.Min(_start.X, _end.X), Math.Min(_start.Y, _end.Y));//确定圆弧外矩形框的画图基点，即左上角
                    int _radius = (Math.Max(Math.Abs(_end.X - _start.X), Math.Abs(_end.Y - _start.Y))
                        + Math.Min(Math.Abs(_end.X - _start.X), Math.Abs(_end.Y - _start.Y))) / 2;//取起点终点xy差值的平均值做半径，外矩形边长==直径，

                    //设定圆弧的起始角度（以度为单位），从椭圆外接矩形的X轴正方向（3点钟方向）顺时针计算。
                    float startangle = 0F;
                    if (_end.X < _start.X && _end.Y > _start.Y)//四象限
                    {
                        startangle = 0F; _base.X -= _radius; _base.Y -= _radius;//调整基点坐标
                    }
                    else if (_end.X < _start.X && _end.Y < _start.Y) //三象限
                    {
                        startangle = 90F; _base.Y -= _radius;
                    }
                    else if (_end.X > _start.X && _end.Y < _start.Y)
                    {
                        startangle = 180F;
                    }
                    else if (_end.X > _start.X && _end.Y > _start.Y)
                    {
                        startangle = 270F; _base.X -= _radius;
                    }
                    else { startangle = 0F; }
                    g.DrawArc(_pen, new Rectangle(_base, new Size(_radius * 2, _radius * 2)), startangle, 90F);//设定矩形框，起始角度（从3点钟方向开始，顺时针），扫掠角度
                    //g.DrawArc(_pen, new Rectangle(_start - 20, startY, 40, 40), 180F, 90F);//设定矩形框，起始角度（从3点钟方向开始，顺时针），扫掠角度

                }
                else
                {
                    g.DrawLine(_pen, _start, _end);//画直线段
                }
            }

            #region 画端点-old


            ////画端点
            ////示例1 画图信息：33 / P2 34 23 146 23 / L3000 90 + 0 23 + 0 0 1 / D套,丝
            ////           边角信息：   0,套;3000,丝
            ////示例2 画图信息：33/P2 39 33 39 15 91 15 143 15/L150 39+0 24+0 0 6,3650 65+0 15+0 0 1,6000 117 15 0 1/D0,90,搭,0
            ////          边角信息：150,90;3650,搭500;6000,0
            //List<GeneralMultiData> _MultiData = GeneralClass.LDOpt.ldhelper.GetMultiData(_rebar.CornerMessageBK, _rebar.Diameter);//获取加工信息，进行对应
            //if (_pic.Dtype[0] == "0")
            //{
            //    _MultiData.Insert(0, new GeneralMultiData());//因为当dtype第一个为0时，边角信息会少一个，补齐一个空白GeneralMultiData
            //}
            //for (int i = 0; i < _pic.PlinePoints.Count; i++)
            //{
            //    Point _last = new Point(), _next = new Point();
            //    if (i != 0) { _last = new Point(_pic.PlinePoints[i - 1].X, _pic.PlinePoints[i - 1].Y + _bias); }//取前一点
            //    if (i != _pic.PlinePoints.Count - 1) { _next = new Point(_pic.PlinePoints[i + 1].X, _pic.PlinePoints[i + 1].Y + _bias); }//取后一点
            //    Point _cur = new Point(_pic.PlinePoints[i].X, _pic.PlinePoints[i].Y + _bias);//取当前点坐标

            //    if (_MultiData[i].headType == EnumMultiHeadType.TAO_P ||
            //        _MultiData[i].headType == EnumMultiHeadType.TAO_N ||
            //        _MultiData[i].headType == EnumMultiHeadType.TAO_V ||
            //        _MultiData[i].headType == EnumMultiHeadType.SI_P ||
            //        _MultiData[i].headType == EnumMultiHeadType.SI_N)//需要套丝的
            //    {
            //        _pen = new Pen(new SolidBrush(Color.Black), 4);//加粗4
            //        Point _start, _end;
            //        if (i == 0)//起点
            //        {
            //             _start = new Point(_cur.X, _cur.Y);
            //             _end = CalculateTaoPoint(_cur, _next, 10, true);
            //            g.DrawLine(_pen, _start, _end);//画套丝
            //        }
            //        else
            //        {
            //             _start = CalculateTaoPoint(_last, _cur, 10, false);
            //             _end = new Point(_cur.X, _cur.Y);
            //            g.DrawLine(_pen, _start, _end);//画套丝
            //        }

            //        g.DrawString(_MultiData[i].msg_second, _font, _brush, _cur);//标注套丝

            //        //if (_start.Y==_end.Y)//横向的套丝
            //        //{
            //        //    g.DrawString(_MultiData[i].msg_second, _font, _brush, _cur);//标注套丝
            //        //}
            //        //else//竖向的套丝
            //        //{
            //        //    Point newStart = CalculateBiggerY(_start, _end);//取y值大点的做起点
            //        //    Point newEnd = CalculateSmallerY(_start, _end); //取y值小点 的做终点
            //        //    int _angle = CalculateAngle(newStart,newEnd );
            //        //    Matrix matrix = new Matrix();
            //        //    matrix.RotateAt(Math.Abs(_angle), newStart);//设置旋转中心，以解析的原始基准加上偏移
            //        //    g.Transform = matrix;//旋转设置
            //        //    g.DrawString(_MultiData[i].msg_second, _font, _brush, _cur);//标注套丝
            //        //    matrix.RotateAt(-Math.Abs(_angle), newStart);//设置负角度，复原
            //        //    g.Transform = matrix;//
            //        //}

            //        //Matrix matrix = new Matrix();
            //        //matrix.RotateAt(_pic.LengthMsg[i].angle, new Point(_pos.X, _pos.Y + _bias));//设置旋转中心，以解析的原始基准加上偏移
            //        //g.Transform = matrix;//旋转设置
            //        //g.DrawString(_MultiData[i].msg_second, _font, _brush, _cur);//标注套丝

            //    }
            //    else if (_MultiData[i].headType == EnumMultiHeadType.DA)//需要搭接
            //    {
            //        _pen = new Pen(new SolidBrush(Color.Black), 2);//粗细2
            //        Point _start = new Point(_cur.X-2, _cur.Y - 5);
            //        Point _end = new Point(_cur.X + 8, _cur.Y + 5);
            //        g.DrawLine(_pen, _start, _end);//画搭接标识1
            //         _start = new Point(_cur.X-8, _cur.Y - 5);
            //         _end = new Point(_cur.X +2, _cur.Y + 5);
            //        g.DrawLine(_pen, _start, _end);//画搭接标识2

            //        g.DrawString(_MultiData[i].msg_second, _font, _brush, _cur);//标注搭接
            //    }
            //}
            #endregion

            //画端点
            //示例1 画图信息：33 / P2 34 23 146 23 / L3000 90 + 0 23 + 0 0 1 / D套,丝
            //           边角信息：   0,套;3000,丝
            //示例2 画图信息：33/P2 39 33 39 15 91 15 143 15/L150 39+0 24+0 0 6,3650 65+0 15+0 0 1,6000 117 15 0 1/D0,90,搭,0
            //          边角信息：150,90;3650,搭500;6000,0

            for (int i = 0; i < _pic.PlinePoints.Count; i++)
            {
                Point _last = new Point(), _next = new Point();
                if (i != 0) { _last = new Point(_pic.PlinePoints[i - 1].X, _pic.PlinePoints[i - 1].Y + _bias); }//取前一点
                if (i != _pic.PlinePoints.Count - 1) { _next = new Point(_pic.PlinePoints[i + 1].X, _pic.PlinePoints[i + 1].Y + _bias); }//取后一点
                Point _cur = new Point(_pic.PlinePoints[i].X, _pic.PlinePoints[i].Y + _bias);//取当前点坐标

                if (_pic.Dtype[i].IndexOf('套') > -1 || _pic.Dtype[i].IndexOf('丝') > -1 || _pic.Dtype[i].IndexOf('反') > -1)//需要套丝的，包括反丝
                {
                    _pen = new Pen(new SolidBrush(Color.Black), 4);//加粗4
                    Point _start, _end;
                    if (i == 0)//起点
                    {
                        _start = new Point(_cur.X, _cur.Y);
                        _end = CalculateTaoPoint(_cur, _next, 10, true);
                        g.DrawLine(_pen, _start, _end);//画套丝
                    }
                    else
                    {
                        _start = CalculateTaoPoint(_last, _cur, 10, false);
                        _end = new Point(_cur.X, _cur.Y);
                        g.DrawLine(_pen, _start, _end);//画套丝
                    }
                    g.DrawString(_pic.Dtype[i], _font, _brush, _cur);//标注套丝
                }
                else if (_pic.Dtype[i].IndexOf('搭') > -1)//需要搭接
                {
                    _pen = new Pen(new SolidBrush(Color.Black), 2);//粗细2
                    Point _start = new Point(_cur.X - 2, _cur.Y - 5);
                    Point _end = new Point(_cur.X + 8, _cur.Y + 5);
                    g.DrawLine(_pen, _start, _end);//画搭接标识1
                    _start = new Point(_cur.X - 8, _cur.Y - 5);
                    _end = new Point(_cur.X + 2, _cur.Y + 5);
                    g.DrawLine(_pen, _start, _end);//画搭接标识2

                    g.DrawString(_pic.Dtype[i], _font, _brush, _cur);//标注搭接
                }
            }

            //画长度标注，先处理一下缩尺，缩尺的实际长度已经进行拆解，原缩尺信息存储在cornermsgBk里面，20250719
            List<GeneralMultiData> _MultiData = GeneralClass.LDOpt.ldhelper.GetMultiData(_rebar.CornerMessage, _rebar.Diameter);//获取加工信息，进行对应
            _MultiData.RemoveAll(t => t.ilength == 0);//移除所有长度为0的multidata

            for (int i = 0; i < _pic.LengthMsg.Count; i++)
            {
                string _length="";
                if (_pic.LengthMsg[i].length.IndexOf('~')>-1&&_rebar.TotalPieceNum!=0)//数量不为0的缩尺，为拆解后的有效数据，其他非有效数据仅观看
                {
                    _length = _MultiData[i].ilength.ToString();//如果是缩尺，则用边角结构拆解的长度，前提是边角结构和图形信息的multidata顺序要一致，20250719
                }
                else
                {
                    _length = _pic.LengthMsg[i].length;
                }

                SizeF _size = g.MeasureString(_length, _font);
                _pic.LengthMsg[i].lengthStrWidth = Convert.ToInt32(_size.Width);//要先赋值字符串宽度、高度，再获取其画图位置基准点
                _pic.LengthMsg[i].lengthStrHeight = Convert.ToInt32(_size.Height);//要先赋值字符串宽度、高度，再获取其画图位置基准点

                Point _pos = _pic.LengthMsg[i].pos;
                Point _paintPos = _pic.LengthMsg[i].paintPos;//画图位置基准点

                Matrix matrix = new Matrix();
                matrix.RotateAt(_pic.LengthMsg[i].angle, new Point(_pos.X, _pos.Y + _bias));//设置旋转中心，以解析的原始基准加上偏移
                g.Transform = matrix;//旋转设置

                g.DrawString(_length, _font, _brush, new Point(_paintPos.X, _paintPos.Y + _bias));//标注长度

            }

            return bitmap;
        }
        private static Point CalculateSmallerY(Point p1, Point p2)
        {
            if (p1.Y < p2.Y)
            {
                return p1;
            }
            else if (p1.Y > p2.Y)
            {
                return p2;
            }
            else
            {
                return (p1.X < p2.X) ? p1 : p2;//如果Y值相等，取x值小的
            }
        }
        private static Point CalculateBiggerY(Point p1, Point p2)
        {
            if (p1.Y > p2.Y)
            {
                return p1;
            }
            else if (p1.Y < p2.Y)
            {
                return p2;
            }
            else
            {
                return (p1.X > p2.X) ? p1 : p2;//如果Y值相等，取x值大的
            }
        }
        /// <summary>
        /// 已知起点终点，求与坐标系夹角
        /// </summary>
        /// <param name="_start"></param>
        /// <param name="_end"></param>
        /// <returns></returns>
        private static int CalculateAngle(Point _start, Point _end)
        {
            int deltaX = _end.X - _start.X;
            int deltaY = _end.Y - _start.Y;

            double angleRadians = Math.Atan2(deltaY, deltaX);
            double angleDegrees = angleRadians * (180 / Math.PI);

            int rtAngle = (int)angleDegrees;
            //rtAngle = (rtAngle + 90 * 10) % 90;
            return rtAngle;
        }
        /// <summary>
        /// 通过三角函数，已知起点、终点，画指定距离的点的坐标，注意是在起点附近画，还是终点附近画
        /// </summary>
        /// <param name="_start">起点</param>
        /// <param name="_end">终点</param>
        /// <param name="_length">距离</param>
        /// <param name="sORe">是在起点画，还是终点画，true起点，false终点</param>
        private static Point CalculateTaoPoint(Point _start, Point _end, int _length, bool sORe)
        {
            try
            {
                Point _cur = new Point();

                double L = Math.Sqrt((double)((_end.X - _start.X) * (_end.X - _start.X) + (_end.Y - _start.Y) * (_end.Y - _start.Y)));

                if (sORe)//起点附近画
                {
                    int _x = Convert.ToInt16(_start.X + _length / L * (_end.X - _start.X));
                    int _y = Convert.ToInt16(_start.Y + _length / L * (_end.Y - _start.Y));
                    _cur = new Point(_x, _y);
                }
                else
                {
                    int _x = Convert.ToInt16(_end.X - _length / L * (_end.X - _start.X));
                    int _y = Convert.ToInt16(_end.Y - _length / L * (_end.Y - _start.Y));
                    _cur = new Point(_x, _y);
                }
                return _cur;
            }
            catch (Exception ex) { MessageBox.Show("CalculatePoint error :" + ex.Message); return new Point(); }
        }

        /// <summary>
        /// 解析P指令，示例：P2 108 5 73 5 73 43 108 43 108 5
        /// </summary>
        /// <param name="cmd">P字段指令</param>
        /// <param name="_picmsg"></param>
        /// <returns></returns>
        public static bool ExecuteP(string cmd, ref EjinPicMsg _picmsg)
        {
            string[] sss = cmd.Split(' ');//用空格间隔开
            if (sss.Length % 2 == 0)
            {
                MessageBox.Show("executeP error :P指令长度不为奇数，解析失败");
                return false;
            }

            try
            {
                _picmsg.PlineType = sss[0].Substring(1);//P2去掉前面的"P",仅留后面的线形，

                for (int i = 1; i < sss.Length;)
                {
                    Point tmp = new Point(Convert.ToInt16(sss[i]), Convert.ToInt16(sss[i + 1]));
                    i += 2;
                    _picmsg.PlinePoints.Add(tmp);
                }

                //if(_picmsg.PlinePoints.Min(t=>t.Y)<0)
                //{
                //    int _bias = 2 - _picmsg.PlinePoints.Min(t => t.Y);//如果最小Y值小于0，则全体Y值做一个补偿偏移，保证所有的Y值均>0

                //    for(int i =0;i<_picmsg.PlinePoints.Count;i++)
                //    {
                //        _picmsg.PlinePoints[i] =new Point(_picmsg.PlinePoints[i].X, _picmsg.PlinePoints[i].Y+_bias);
                //    }
                //}

                return true;
            }
            catch (Exception ex) { MessageBox.Show("ExecuteP error:" + ex.Message); return false; }
        }
        /// <summary>
        /// 解析L指令，示例：L180 38+-1 23+8 0 2,6380 91+0 15+0 0 1,180 144+3 23+8 0 0
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="_picmsg"></param>
        /// <returns></returns>
        public static bool ExecuteL(string cmd, ref EjinPicMsg _picmsg)
        {
            try
            {
                string[] sss = cmd.Substring(1).Split(',');//以','隔开
                foreach (var item in sss)
                {
                    LengthMsg tmp = new LengthMsg();

                    string[] ss = item.Split(' ');//以空格隔开

                    tmp.length = ss[0];
                    List<int> xlist = new List<int>();
                    List<int> ylist = new List<int>();
                    foreach (var iii in ss[1].Split('+'))//处理180 38+-1 23+8 0 2
                    {
                        xlist.Add(Convert.ToInt16(iii));
                    }
                    foreach (var ttt in ss[2].Split('+'))
                    {
                        ylist.Add(Convert.ToInt16(ttt));
                    }
                    int x = xlist.Sum();//将38+-1理解为38加上-1
                    int y = ylist.Sum();//将23+8理解为23加上8
                    tmp.pos = new Point(x, y);
                    tmp.angle = Convert.ToInt16(ss[3]);
                    tmp.align = Convert.ToInt16(ss[4]);

                    _picmsg.LengthMsg.Add(tmp);
                }
                return true;
            }
            catch (Exception ex) { MessageBox.Show("ExecuteL error:" + ex.Message); return false; }
        }
        /// <summary>
        /// 解析M指令，示例：M3,4,0,0,0
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="_picmsg"></param>
        /// <returns></returns>
        public static bool ExecuteM(string cmd, ref EjinPicMsg _picmsg)
        {
            try
            {
                string[] sss = cmd.Substring(1).Split(',');//以','隔开
                foreach (var item in sss)
                {
                    _picmsg.Match.Add(item);
                }
                return true;
            }
            catch (Exception ex) { MessageBox.Show("ExecuteM error:" + ex.Message); return false; }
        }
        /// <summary>
        ///  解析D指令，示例：D0,90,90,0
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="_picmsg"></param>
        /// <returns></returns>
        public static bool ExecuteD(string cmd, ref EjinPicMsg _picmsg)
        {
            try
            {
                string[] sss = cmd.Substring(1).Split(',');//以','隔开
                foreach (var item in sss)
                {
                    _picmsg.Dtype.Add(item);
                }
                return true;
            }
            catch (Exception ex) { MessageBox.Show("ExecuteD error:" + ex.Message); return false; }
        }
        /// <summary>
        /// 解析H指令，示例：H0,0.44,0,0.44,0,0
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="_picmsg"></param>
        /// <returns></returns>
        public static bool ExecuteH(string cmd, ref EjinPicMsg _picmsg)
        {
            try
            {
                string[] sss = cmd.Substring(1).Split(',');//以','隔开
                foreach (var item in sss)
                {
                    _picmsg.HArc.Add(Convert.ToSingle(item));
                }
                return true;
            }
            catch (Exception ex) { MessageBox.Show("ExecuteH error:" + ex.Message); return false; }
        }

        public static bool ExecuteS(string cmd, ref EjinPicMsg _picmsg)
        {
            try
            {
                if (cmd.Contains("SG")) { _picmsg.Stype = EnumRebarShapeType.SHAPE_GJ; }//箍筋
                if (cmd.Contains("SL")) { _picmsg.Stype = EnumRebarShapeType.SHAPE_LG; }//拉勾
                if (cmd.Contains("SC")) { _picmsg.Stype = EnumRebarShapeType.SHAPE_MD; }//马凳

                return true;
            }
            catch (Exception ex) { MessageBox.Show("ExecuteS error:" + ex.Message); return false; }
        }


    }
}
