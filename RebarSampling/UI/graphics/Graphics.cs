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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using NPOI.SS.Formula.Functions;
using System.Web.SessionState;
using RebarSampling.Database;

namespace RebarSampling
{
    /// <summary>
    /// 画图类，用于绘制钢筋
    /// </summary>
    public class graphics
    {
        /// <summary>
        /// 画一根钢筋原材，全直的
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
                    _start = (int)((double)_lengthSum / (double)GeneralClass.OriginalLength(_rebarOri._level, _rebarOri._diameter) * (double)maxPointX);
                    _end = (int)((double)(_lengthSum + item.length) / (double)GeneralClass.OriginalLength(_rebarOri._level, _rebarOri._diameter) * (double)maxPointX);

                    _pen = new Pen(new SolidBrush(Color.Green), item.PickUsed ? 6 : 3);//看rebar是否处于拖拽选中状态，如果是则加粗
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
                    //List<GeneralMultiData> _MultiData = DBOpt.GetMultiData(item.CornerMessage, item.Diameter);
                    List<GeneralMultiData> _MultiData = GeneralClass.LDOpt.ldhelper.GetMultiData(item.CornerMessage, item.Diameter);
                    if (_MultiData != null && _MultiData.Count != 0)//可以画弯曲套丝的就画，画不了的就算了
                    {
                        if (_MultiData.First().ilength == 0 && _MultiData.First().type == 2)//端头套丝，20241113修改增加长度判断，例如“6000，套”是单头套，非双头套
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
                                int _bendpos = (int)((double)(_lengthSum + _bendlength) / (double)GeneralClass.OriginalLength(_rebarOri._level, _rebarOri._diameter) * (double)maxPointX);
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
                        foreach (var item in _rebarOri._secondUsedList)//二次利用的可能有多段
                        {
                            _start = (int)((double)_lengthSum / (double)GeneralClass.OriginalLength(_rebarOri._level, _rebarOri._diameter) * (double)maxPointX);
                            _end = (int)((double)(_lengthSum + item) / (double)GeneralClass.OriginalLength(_rebarOri._level, _rebarOri._diameter) * (double)maxPointX);

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
                        _start = (int)((double)_lengthSum / (double)GeneralClass.OriginalLength(_rebarOri._level, _rebarOri._diameter) * (double)maxPointX);
                        _end = (int)((double)(_rebarOri._totalLength/*GeneralClass.OriginalLength*/) / (double)GeneralClass.OriginalLength(_rebarOri._level, _rebarOri._diameter) * (double)maxPointX);

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
                        _start = (int)((double)_lengthSum / (double)GeneralClass.OriginalLength(_rebarOri._level, _rebarOri._diameter) * (double)maxPointX);
                        _end = (int)((double)(_rebarOri._totalLength/*GeneralClass.OriginalLength*/) / (double)GeneralClass.OriginalLength(_rebarOri._level, _rebarOri._diameter) * (double)maxPointX);

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

        public static Bitmap PaintRebarPic(RebarData _rebar)
        {
            try
            {
                List<Rebar> _list = Algorithm.ListExpand(new List<RebarData> { _rebar });

                return PaintRebarPic(_list[0]);
                //return (_list.Count > 0) ? PaintRebarPic(_list[0]) : null;//存在_list数量=0的情况，一般是料单有错误
            }
            catch (Exception ex) { MessageBox.Show("PaintRebarPic error :" + ex.Message); return null; }

        }
        /// <summary>
        /// 画钢筋的简图，真正的形状（带弯曲的）,此处根据钢筋简图编号做分拣，实际用各自的图形
        /// </summary>
        /// <param name="_rebar"></param>
        /// <returns></returns>
        public static Bitmap PaintRebarPic(Rebar _rebar)
        {
            Bitmap bitmap = null;

            if (_rebar.IfHaveARC)//有圆弧的先处理
            {
                bitmap = PaintRebarPic_ARC(_rebar);
                return bitmap;
            }

            if (_rebar.PicTypeNum == "10000"
                || _rebar.PicTypeNum == "00000"
                || _rebar.PicTypeNum == "20000")//直条
            {
                bitmap = PaintRebarPic_10000(_rebar);
            }

            if (_rebar.RebarShapeType == EnumRebarShapeType.SHAPE_GJ)//箍筋
            {
                bitmap = PaintRebarPic_GJ(_rebar);
            }
            if (_rebar.RebarShapeType == EnumRebarShapeType.SHAPE_LG)//拉勾
            {
                bitmap = PaintRebarPic_LG(_rebar);

            }
            if (_rebar.PicTypeNum == "10100"
                || _rebar.PicTypeNum == "10200"
                || _rebar.PicTypeNum == "20001"
                || _rebar.PicTypeNum == "20002"
                || _rebar.PicTypeNum == "20100"
                || _rebar.PicTypeNum == "20200")//两段的以2开头
            {
                bitmap = PaintRebarPic_20000(_rebar);
            }

            if (_rebar.PicTypeNum == "20101"
                || _rebar.PicTypeNum == "20202"
                || _rebar.PicTypeNum == "30101"
                || _rebar.PicTypeNum == "30202")//三段的，两边弯锚
            {
                bitmap = PaintRebarPic_30000(_rebar);
            }
            if (_rebar.PicTypeNum == "30102" || _rebar.PicTypeNum == "30201")//三段的，两边反向弯锚
            {
                bitmap = PaintRebarPic_30102(_rebar);
            }
            return bitmap;
        }


        private static Bitmap PaintRebarPic_ARC(Rebar _rebar)
        {
            Bitmap bitmap = new Bitmap(180, 40);//新建一个bitmap，用于绘图

            Graphics g = System.Drawing.Graphics.FromImage(bitmap);//从bitmap中建一个画图对象
            g.Clear(Color.White);

            int startY = 5;
            int _start = 40;
            int _end = 140;

            Pen _pen;
            Point p1, p2;

            string text;
            int fontsize = 10;
            int fontX = 0;
            int fontY = 0;

            Font _font = new Font("微软雅黑", fontsize, FontStyle.Regular);
            Brush _brush = new SolidBrush(Color.Black);

            try
            {
                _pen = new Pen(new SolidBrush(Color.Black), 3);//粗细3
                p1 = new Point(_start, startY);
                p2 = new Point(_end, startY);
                g.DrawLine(_pen, p1, p2);//画黑线，钢筋中段


                //拆解corner信息,准备绘制两端套丝和弯曲标识
                //List<GeneralMultiData> _MultiData = DBOpt.GetMultiData(_rebar.CornerMessage, _rebar.Diameter);
                List<GeneralMultiData> _MultiData = GeneralClass.LDOpt.ldhelper.GetMultiData(_rebar.CornerMessage, _rebar.Diameter);

                if (_MultiData != null && _MultiData.Count != 0)//
                {
                    int _lengthmax = _MultiData.FindAll(t => t.headType != EnumMultiHeadType.ARC).Max(t => t.ilength);//找到不为arc的且长度最长的multidata，其长度即为中段长度
                    int _indexmax = _MultiData.FindIndex(t => t.ilength == _lengthmax);//查找其索引
                    fontX = (_start + _end) / 2 - 20;
                    fontY = startY + 2;
                    g.DrawString(_lengthmax.ToString(), _font, _brush, fontX, fontY);//写中段长度，文本标注

                    if (_indexmax - 1 >= 0)//从中段往前一段
                    {
                        if (_MultiData[_indexmax - 1].headType == EnumMultiHeadType.ARC)//画圆弧端头
                        {
                            //_pen = new Pen(new SolidBrush(Color.Black), 3);
                            g.DrawArc(_pen, new Rectangle(_start - 20, startY, 40, 40), 180F, 90F);//设定矩形框，起始角度（从3点钟方向开始，顺时针），扫掠角度

                            text = _MultiData[_indexmax - 1].ilength.ToString();//圆弧长度
                            fontX = _start - 10;
                            fontY = startY + 2;
                            g.DrawString(text, _font, _brush, fontX, fontY);//写线段长度，文本标注
                        }
                        else//非圆弧的，一般为直线段
                        {
                            p1 = new Point(_start, startY);
                            p2 = new Point(_start, startY - 10);
                            g.DrawLine(_pen, p1, p2);//画黑线
                            text = _MultiData[_indexmax - 1].ilength.ToString();//直线段长度
                            fontX = _start - 10;
                            fontY = startY + 2;
                            g.DrawString(text, _font, _brush, fontX, fontY);//写线段长度，文本标注
                        }
                    }

                    if (_indexmax - 2 >= 0)//从中段往前两段
                    {
                        if (_MultiData[_indexmax - 2].headType != EnumMultiHeadType.ARC)
                        {
                            p1 = new Point(_start - 20, startY + 20);
                            p2 = new Point(_start - 20, startY + 35);
                            g.DrawLine(_pen, p1, p2);//画黑线
                            text = _MultiData[_indexmax - 2].ilength.ToString();//直线段长度
                            fontX = _start - 15;
                            fontY = startY + 20;
                            g.DrawString(text, _font, _brush, fontX, fontY);//写线段长度，文本标注
                        }
                    }


                    if (_indexmax + 1 <= _MultiData.Count - 1)//从中段往后一段
                    {
                        if (_MultiData[_indexmax + 1].headType == EnumMultiHeadType.ARC)//画圆弧端尾
                        {
                            //_pen = new Pen(new SolidBrush(Color.Black), 3);
                            g.DrawArc(_pen, new Rectangle(_end - 20, startY, 40, 40), 270F, 90F);//设定矩形框，起始角度（从3点钟方向开始，顺时针），扫掠角度

                            text = _MultiData[_indexmax + 1].ilength.ToString();//圆弧长度
                            fontX = _end - 15;
                            fontY = startY + 2;
                            g.DrawString(text, _font, _brush, fontX, fontY);//写线段长度，文本标注
                        }
                        else//非圆弧的，一般为直线段
                        {
                            p1 = new Point(_end, startY);
                            p2 = new Point(_end, startY + 10);
                            g.DrawLine(_pen, p1, p2);//画黑线
                            text = _MultiData[_indexmax + 1].ilength.ToString();//直线段长度
                            fontX = _end - 10;
                            fontY = startY + 2;
                            g.DrawString(text, _font, _brush, fontX, fontY);//写线段长度，文本标注
                        }
                    }
                    if (_indexmax + 2 <= _MultiData.Count - 1)//从中段往后两段
                    {
                        if (_MultiData[_indexmax + 2].headType != EnumMultiHeadType.ARC)
                        {
                            p1 = new Point(_end + 20, startY + 20);
                            p2 = new Point(_end + 20, startY + 35);
                            g.DrawLine(_pen, p1, p2);//画黑线
                            text = _MultiData[_indexmax + 2].ilength.ToString();//直线段长度
                            fontX = _end - 10;
                            fontY = startY + 20;
                            g.DrawString(text, _font, _brush, fontX, fontY);//写线段长度，文本标注
                        }
                    }
                    //if (_MultiData.First().type == 2 && _MultiData.First().ilength == 0)//端头套丝的一般标为【0,丝】
                    //{
                    //    _pen = new Pen(new SolidBrush(Color.Black), 6);
                    //    p1 = new Point(_start, startY);
                    //    p2 = new Point(_start + 10, startY);
                    //    g.DrawLine(_pen, p1, p2);//画黑色加粗线，标识套丝
                    //}
                    //if (_MultiData.First().headType == EnumMultiHeadType.ARC)//画圆弧端头
                    //{
                    //    _pen = new Pen(new SolidBrush(Color.Black), 3);
                    //    g.DrawArc(_pen, new Rectangle(_start - 20, startY, 40, 40), 180F, 90F);//设定矩形框，起始角度（从3点钟方向开始，顺时针），扫掠角度

                    //    text = _MultiData.First().ilength.ToString();//圆弧长度
                    //    fontX = _start - 10;
                    //    fontY = startY + 2;
                    //    g.DrawString(text, _font, _brush, fontX, fontY);//写线段长度，文本标注
                    //}
                    //if (_MultiData.First().ilength == 0 &&
                    //    _MultiData.First().headType != EnumMultiHeadType.BEND &&
                    //    _MultiData.First().headType != EnumMultiHeadType.ORG)//原始端头和弯曲的不用写端头标识
                    //{
                    //    _pen = new Pen(new SolidBrush(Color.Black), 3);
                    //    fontX = _start;
                    //    fontY = startY + 2;
                    //    g.DrawString(_MultiData.First().msg_second, _font, _brush, fontX, fontY);//画端头信息string
                    //}

                    ////端尾
                    //if (_MultiData.Last().type == 2)//端尾套丝
                    //{
                    //    _pen = new Pen(new SolidBrush(Color.Black), 6);
                    //    p1 = new Point(_end - 10, startY);
                    //    p2 = new Point(_end, startY);
                    //    g.DrawLine(_pen, p1, p2);//画黑色加粗线，标识套丝
                    //}
                    //if (_MultiData.Last().headType == EnumMultiHeadType.ARC)//画圆弧端尾
                    //{
                    //    _pen = new Pen(new SolidBrush(Color.Black), 3);
                    //    g.DrawArc(_pen, new Rectangle(_end - 20, startY, 40, 40), 270F, 90F);//设定矩形框，起始角度（从3点钟方向开始，顺时针），扫掠角度

                    //    text = _MultiData.Last().ilength.ToString();//圆弧长度
                    //    fontX = _end - 30;
                    //    fontY = startY + 2;
                    //    g.DrawString(text, _font, _brush, fontX, fontY);//写线段长度，文本标注
                    //}
                    //if (_MultiData.Last().headType != EnumMultiHeadType.BEND &&
                    // _MultiData.Last().headType != EnumMultiHeadType.ORG &&
                    // _MultiData.Last().headType != EnumMultiHeadType.ARC)//原始端尾、弯曲、圆弧的的不用写端尾标识
                    //{
                    //    _pen = new Pen(new SolidBrush(Color.Black), 3);
                    //    fontX = _end - 30;
                    //    fontY = startY + 2;
                    //    g.DrawString(_MultiData.Last().msg_second, _font, _brush, fontX, fontY);//画端头信息string
                    //}
                }

                ////20241114修改，有圆弧端头和端尾的长度不是按照下料长度来，
                //if (_MultiData.First().headType == EnumMultiHeadType.ARC || _MultiData.Last().headType == EnumMultiHeadType.ARC)
                //{
                //    text = _MultiData.Find(t => t.headType != EnumMultiHeadType.ARC).ilength.ToString();//找到不为arc的multidata，其长度即为中段长度
                //    fontX = (_start + _end) / 2 - 20;
                //    fontY = startY - 20;
                //    g.DrawString(text, _font, _brush, fontX, fontY);//写线段长度，文本标注
                //}
                //else
                //{
                //    text = _rebar.length.ToString();//长度
                //    fontX = (_start + _end) / 2 - 20;
                //    fontY = startY - 20;
                //    g.DrawString(text, _font, _brush, fontX, fontY);//写线段长度，文本标注
                //}

            }
            catch (Exception ex) { MessageBox.Show("PaintRebar_ARC error:" + ex.Message); }

            return bitmap;

        }
        /// <summary>
        /// 直条钢筋，简图编号：10000、20000
        /// 注意特例：圆弧端头的直条钢筋，
        ///                 圆弧端头示例：
        //      1200R1200T0.44,2;4200,3;1200R1200T0.44,0
        //      900R900T0.44,5;4250,5;900R900T0.44,0
        /// </summary>
        /// <param name="_rebar">单根钢筋</param>
        /// <returns></returns>
        private static Bitmap PaintRebarPic_10000(Rebar _rebar)
        {
            Bitmap bitmap = new Bitmap(180, 40);//新建一个bitmap，用于绘图

            Graphics g = System.Drawing.Graphics.FromImage(bitmap);//从bitmap中建一个画图对象
            g.Clear(Color.White);

            int startY = 20;
            int _start = 40;
            int _end = 140;

            Pen _pen;
            Point p1, p2;

            string text;
            int fontsize = 10;
            int fontX = 0;
            int fontY = 0;

            Font _font = new Font("微软雅黑", fontsize, FontStyle.Regular);
            Brush _brush = new SolidBrush(Color.Black);

            try
            {
                _pen = new Pen(new SolidBrush(Color.Black), 3);//粗细3
                p1 = new Point(_start, startY);
                p2 = new Point(_end, startY);
                g.DrawLine(_pen, p1, p2);//画黑线，钢筋小段



                //拆解corner信息,准备绘制两端套丝和弯曲标识
                //List<GeneralMultiData> _MultiData = DBOpt.GetMultiData(_rebar.CornerMessage, _rebar.Diameter);
                List<GeneralMultiData> _MultiData = GeneralClass.LDOpt.ldhelper.GetMultiData(_rebar.CornerMessage, _rebar.Diameter);

                if (_MultiData != null && _MultiData.Count != 0)//
                {
                    if (_MultiData.First().type == 2 && _MultiData.First().ilength == 0)//端头套丝的一般标为【0,丝】
                    {
                        _pen = new Pen(new SolidBrush(Color.Black), 6);
                        p1 = new Point(_start, startY);
                        p2 = new Point(_start + 10, startY);
                        g.DrawLine(_pen, p1, p2);//画黑色加粗线，标识套丝
                    }
                    if (_MultiData.First().headType == EnumMultiHeadType.ARC)//画圆弧端头
                    {
                        _pen = new Pen(new SolidBrush(Color.Black), 3);
                        g.DrawArc(_pen, new Rectangle(_start - 20, startY, 40, 40), 180F, 90F);//设定矩形框，起始角度（从3点钟方向开始，顺时针），扫掠角度

                        text = _MultiData.First().ilength.ToString();//圆弧长度
                        fontX = _start - 10;
                        fontY = startY + 2;
                        g.DrawString(text, _font, _brush, fontX, fontY);//写线段长度，文本标注
                    }
                    if (_MultiData.First().ilength == 0 &&
                        _MultiData.First().headType != EnumMultiHeadType.BEND &&
                        _MultiData.First().headType != EnumMultiHeadType.ORG)//原始端头和弯曲的不用写端头标识
                    {
                        _pen = new Pen(new SolidBrush(Color.Black), 3);
                        fontX = _start;
                        fontY = startY + 2;
                        g.DrawString(_MultiData.First().msg_second, _font, _brush, fontX, fontY);//画端头信息string
                    }

                    //端尾
                    if (_MultiData.Last().type == 2)//端尾套丝
                    {
                        _pen = new Pen(new SolidBrush(Color.Black), 6);
                        p1 = new Point(_end - 10, startY);
                        p2 = new Point(_end, startY);
                        g.DrawLine(_pen, p1, p2);//画黑色加粗线，标识套丝
                    }
                    if (_MultiData.Last().headType == EnumMultiHeadType.ARC)//画圆弧端尾
                    {
                        _pen = new Pen(new SolidBrush(Color.Black), 3);
                        g.DrawArc(_pen, new Rectangle(_end - 20, startY, 40, 40), 270F, 90F);//设定矩形框，起始角度（从3点钟方向开始，顺时针），扫掠角度

                        text = _MultiData.Last().ilength.ToString();//圆弧长度
                        fontX = _end - 30;
                        fontY = startY + 2;
                        g.DrawString(text, _font, _brush, fontX, fontY);//写线段长度，文本标注
                    }
                    if (_MultiData.Last().headType != EnumMultiHeadType.BEND &&
                     _MultiData.Last().headType != EnumMultiHeadType.ORG &&
                     _MultiData.Last().headType != EnumMultiHeadType.ARC)//原始端尾、弯曲、圆弧的的不用写端尾标识
                    {
                        _pen = new Pen(new SolidBrush(Color.Black), 3);
                        fontX = _end - 30;
                        fontY = startY + 2;
                        g.DrawString(_MultiData.Last().msg_second, _font, _brush, fontX, fontY);//画端头信息string
                    }
                }

                //20241114修改，有圆弧端头和端尾的长度不是按照下料长度来，
                if (_MultiData.First().headType == EnumMultiHeadType.ARC || _MultiData.Last().headType == EnumMultiHeadType.ARC)
                {
                    text = _MultiData.Find(t => t.headType != EnumMultiHeadType.ARC).ilength.ToString();//找到不为arc的multidata，其长度即为中段长度
                    fontX = (_start + _end) / 2 - 20;
                    fontY = startY - 20;
                    g.DrawString(text, _font, _brush, fontX, fontY);//写线段长度，文本标注
                }
                else
                {
                    text = _rebar.length.ToString();//长度
                    fontX = (_start + _end) / 2 - 20;
                    fontY = startY - 20;
                    g.DrawString(text, _font, _brush, fontX, fontY);//写线段长度，文本标注
                }

            }
            catch (Exception ex) { MessageBox.Show("PaintRebar_10000 error:" + ex.Message); }

            return bitmap;
        }

        /// <summary>
        /// 单头弯锚，简图编号：10100、10200、20001、20002、20100、20200
        /// </summary>
        /// <param name="_rebar">单根钢筋</param>
        /// <returns></returns>
        private static Bitmap PaintRebarPic_20000(Rebar _rebar)
        {
            Bitmap bitmap = new Bitmap(180, 40);//新建一个bitmap，用于绘图，图片的尺寸为180*40

            Graphics g = System.Drawing.Graphics.FromImage(bitmap);//从bitmap中建一个画图对象
            g.Clear(Color.White);

            //int startY = 20;
            //int _start = 40;
            //int _end = 140;

            Point p1, p2;

            int fontsize = 10;
            int fontX = 0;
            int fontY = 0;

            Font _font = new Font("微软雅黑", fontsize, FontStyle.Regular);
            Brush _brush = new SolidBrush(Color.Black);
            Pen _pen = new Pen(new SolidBrush(Color.Black), 3);//粗细3

            try
            {
                //拆解corner信息,
                //List<GeneralMultiData> _MultiData = DBOpt.GetMultiData(_rebar.CornerMessage, _rebar.Diameter);
                List<GeneralMultiData> _MultiData = GeneralClass.LDOpt.ldhelper.GetMultiData(_rebar.CornerMessage, _rebar.Diameter);

                //if (_MultiData.Count != 2) { GeneralClass.interactivityData?.printlog(1, "PaintRebarPic_20000，GetMultiData num error!"); return bitmap; }//段数不对

                int _index_s = 0, _index_e = 0;//起始终止序号

                if (_MultiData[0].ilength == 0)//如果第一段长度为0，则为端头，
                { _index_s = 1; _index_e = 2; }
                else { _index_s = 0; _index_e = 1; }

                bool _side = (_MultiData[_index_s].ilength > _MultiData[_index_e].ilength) ? true : false;//前面的长，则为右侧

                if (_side)//前面的长，则为右侧弯钩
                {
                    g.DrawLine(_pen, new Point(50, 20), new Point(130, 20));//横线
                    g.DrawLine(_pen, new Point(130, 20), new Point(130, 40));//竖线弯钩

                    g.DrawString(_MultiData[_index_s].ilength.ToString(), _font, _brush, new Point(80, 0));//标注长度
                    g.DrawString(_MultiData[_index_e].ilength.ToString(), _font, _brush, new Point(132, 22));//标注长度
                }
                else//后面的长，则为左侧弯钩
                {
                    g.DrawLine(_pen, new Point(50, 20), new Point(130, 20));//横线
                    g.DrawLine(_pen, new Point(50, 20), new Point(50, 40));//竖线弯钩

                    g.DrawString(_MultiData[_index_s].ilength.ToString(), _font, _brush, new Point(52, 22));//标注长度
                    g.DrawString(_MultiData[_index_e].ilength.ToString(), _font, _brush, new Point(80, 0));//标注长度
                }


                //准备绘制两端套丝和弯曲标识
                if (_MultiData.First().type == 2)//端头套丝
                {
                    _pen = new Pen(new SolidBrush(Color.Black), 6);//画黑色加粗线，标识套丝
                    if (_side)//右侧弯钩
                    { g.DrawLine(_pen, new Point(50, 20), new Point(60, 20)); }
                    else //左侧弯钩
                    { g.DrawLine(_pen, new Point(50, 30), new Point(50, 40)); }
                }
                if (_MultiData.First().ilength == 0 &&
                    _MultiData.First().headType != EnumMultiHeadType.BEND &&
                    _MultiData.First().headType != EnumMultiHeadType.ORG)//原始端头和弯曲的不用写端头标识
                {
                    _pen = new Pen(new SolidBrush(Color.Black), 3);
                    g.DrawString(_MultiData.First().msg_second, _font, _brush, new Point(30, 20));//画端头信息string
                }
                if (_MultiData.Last().type == 2)//端尾套丝
                {
                    _pen = new Pen(new SolidBrush(Color.Black), 6);//画黑色加粗线，标识套丝

                    if (_side)//右侧弯钩
                    { g.DrawLine(_pen, new Point(130, 30), new Point(130, 40)); }
                    else
                    { g.DrawLine(_pen, new Point(120, 20), new Point(130, 20)); }
                }
                if (_MultiData.Last().headType != EnumMultiHeadType.BEND &&
                 _MultiData.Last().headType != EnumMultiHeadType.ORG)//原始端头和弯曲的不用写端头标识
                {
                    _pen = new Pen(new SolidBrush(Color.Black), 3);
                    g.DrawString(_MultiData.Last().msg_second, _font, _brush, new Point(110, 20));//画端头信息string
                }

            }
            catch (Exception ex) { MessageBox.Show("PaintRebar_20000 error:" + ex.Message); }

            return bitmap;

        }
        /// <summary>
        /// 双头弯锚，简图编号：20101、20202、30101、30202
        /// </summary>
        /// <param name="_rebar"></param>
        /// <returns></returns>
        private static Bitmap PaintRebarPic_30000(Rebar _rebar)
        {
            Bitmap bitmap = new Bitmap(180, 40);//新建一个bitmap，用于绘图，图片的尺寸为180*40

            Graphics g = System.Drawing.Graphics.FromImage(bitmap);//从bitmap中建一个画图对象
            g.Clear(Color.White);

            Font _font = new Font("微软雅黑", 10, FontStyle.Regular);
            Brush _brush = new SolidBrush(Color.Black);
            Pen _pen = new Pen(new SolidBrush(Color.Black), 3);//粗细3

            try
            {
                //拆解corner信息,
                //List<GeneralMultiData> _MultiData = DBOpt.GetMultiData(_rebar.CornerMessage, _rebar.Diameter);
                List<GeneralMultiData> _MultiData = GeneralClass.LDOpt.ldhelper.GetMultiData(_rebar.CornerMessage, _rebar.Diameter);

                int _index_s = (_MultiData[0].ilength == 0) ? 1 : 0;//起始序号，如果第一段长度为0，则为端头，

                g.DrawLine(_pen, new Point(50, 20), new Point(130, 20));//横线
                g.DrawLine(_pen, new Point(130, 20), new Point(130, 40));//竖线右侧弯钩
                g.DrawLine(_pen, new Point(50, 20), new Point(50, 40));//竖线左侧弯钩

                g.DrawString(_MultiData[_index_s].ilength.ToString(), _font, _brush, new Point(52, 22));//标注长度
                g.DrawString(_MultiData[_index_s + 1].ilength.ToString(), _font, _brush, new Point(80, 0));//标注长度
                g.DrawString(_MultiData[_index_s + 2].ilength.ToString(), _font, _brush, new Point(132, 22));//标注长度

                //准备绘制两端套丝和弯曲标识
                if (_MultiData != null && _MultiData.Count != 0)//
                {
                    if (_MultiData.First().type == 2)//端头套丝
                    {
                        _pen = new Pen(new SolidBrush(Color.Black), 6);
                        g.DrawLine(_pen, new Point(50, 30), new Point(50, 40));//画黑色加粗线，标识套丝
                    }
                    if (_MultiData.First().ilength == 0 &&
                        _MultiData.First().headType != EnumMultiHeadType.BEND &&
                        _MultiData.First().headType != EnumMultiHeadType.ORG)//原始端头和弯曲的不用写端头标识
                    {
                        _pen = new Pen(new SolidBrush(Color.Black), 3);
                        g.DrawString(_MultiData.First().msg_second, _font, _brush, new Point(30, 20));//画端头信息string
                    }
                    if (_MultiData.Last().type == 2)//端尾套丝
                    {
                        _pen = new Pen(new SolidBrush(Color.Black), 6);
                        g.DrawLine(_pen, new Point(130, 30), new Point(130, 40));//画黑色加粗线，标识套丝
                    }
                    if (_MultiData.Last().headType != EnumMultiHeadType.BEND &&
                     _MultiData.Last().headType != EnumMultiHeadType.ORG)//原始端头和弯曲的不用写端头标识
                    {
                        _pen = new Pen(new SolidBrush(Color.Black), 3);
                        g.DrawString(_MultiData.Last().msg_second, _font, _brush, new Point(110, 20));//画端头信息string
                    }

                }

            }
            catch (Exception ex) { MessageBox.Show("PaintRebar_30000 error:" + ex.Message); }

            return bitmap;
        }

        /// <summary>
        /// 双头反向弯锚，简图编号：30102、30201
        /// </summary>
        /// <param name="_rebar"></param>
        /// <returns></returns>
        private static Bitmap PaintRebarPic_30102(Rebar _rebar)
        {
            Bitmap bitmap = new Bitmap(180, 40);//新建一个bitmap，用于绘图，图片的尺寸为180*40

            Graphics g = System.Drawing.Graphics.FromImage(bitmap);//从bitmap中建一个画图对象
            g.Clear(Color.White);

            Font _font = new Font("微软雅黑", 10, FontStyle.Regular);
            Brush _brush = new SolidBrush(Color.Black);
            Pen _pen = new Pen(new SolidBrush(Color.Black), 3);//粗细3

            try
            {
                //拆解corner信息,
                List<GeneralMultiData> _MultiData = GeneralClass.LDOpt.ldhelper.GetMultiData(_rebar.CornerMessage, _rebar.Diameter);

                int _index_s = (_MultiData[0].ilength == 0) ? 1 : 0;//起始序号，如果第一段长度为0，则为端头，

                g.DrawLine(_pen, new Point(50, 20), new Point(130, 20));//横线
                g.DrawString(_MultiData[_index_s + 1].ilength.ToString(), _font, _brush, new Point(80, 0));//标注横线长度

                bool _side = (_MultiData[_index_s].angle < 0 && _MultiData[_index_s + 1].angle > 0) ? true : false;//左-90，右90
                if (_side)//左-90，右90
                {
                    g.DrawLine(_pen, new Point(50, 20), new Point(50, 0));//竖线左侧弯钩
                    g.DrawLine(_pen, new Point(130, 20), new Point(130, 40));//竖线右侧弯钩

                    g.DrawString(_MultiData[_index_s].ilength.ToString(), _font, _brush, new Point(10, 2));//标注左侧长度
                    g.DrawString(_MultiData[_index_s + 2].ilength.ToString(), _font, _brush, new Point(132, 22));//标注右侧长度
                }
                else//左90，右-90
                {
                    g.DrawLine(_pen, new Point(50, 20), new Point(50, 40));//竖线左侧弯钩
                    g.DrawLine(_pen, new Point(130, 20), new Point(130, 0));//竖线右侧弯钩

                    g.DrawString(_MultiData[_index_s].ilength.ToString(), _font, _brush, new Point(52, 22));//标注左侧长度
                    g.DrawString(_MultiData[_index_s + 2].ilength.ToString(), _font, _brush, new Point(132, 2));//标注右侧长度

                }


                //准备绘制两端套丝和弯曲标识
                if (_MultiData.First().type == 2)//端头套丝
                {
                    if (_side)//左-90，右90
                    {
                        _pen = new Pen(new SolidBrush(Color.Black), 6);
                        g.DrawLine(_pen, new Point(50, 0), new Point(50, 10));//画黑色加粗线，标识套丝
                    }
                    else
                    {
                        _pen = new Pen(new SolidBrush(Color.Black), 6);
                        g.DrawLine(_pen, new Point(50, 30), new Point(50, 40));//画黑色加粗线，标识套丝

                    }
                }
                if (_MultiData.First().ilength == 0 &&
                    _MultiData.First().headType != EnumMultiHeadType.BEND &&
                    _MultiData.First().headType != EnumMultiHeadType.ORG)//原始端头和弯曲的不用写端头标识
                {
                    _pen = new Pen(new SolidBrush(Color.Black), 3);
                    g.DrawString(_MultiData.First().msg_second, _font, _brush, new Point(40, 20));//画端头信息string
                }
                if (_MultiData.Last().type == 2)//端尾套丝
                {
                    if (_side)//左-90，右90
                    {
                        _pen = new Pen(new SolidBrush(Color.Black), 6);
                        g.DrawLine(_pen, new Point(130, 30), new Point(130, 40));//画黑色加粗线，标识套丝
                    }
                    else
                    {
                        _pen = new Pen(new SolidBrush(Color.Black), 6);
                        g.DrawLine(_pen, new Point(130, 0), new Point(130, 10));//画黑色加粗线，标识套丝
                    }
                }
                if (_MultiData.Last().headType != EnumMultiHeadType.BEND &&
                 _MultiData.Last().headType != EnumMultiHeadType.ORG)//原始端头和弯曲的不用写端头标识
                {
                    _pen = new Pen(new SolidBrush(Color.Black), 3);
                    g.DrawString(_MultiData.Last().msg_second, _font, _brush, new Point(120, 20));//画端头信息string
                }



            }
            catch (Exception ex) { MessageBox.Show("PaintRebar_30102 error:" + ex.Message); }

            return bitmap;
        }
        /// <summary>
        /// 画拉勾，示例：【10d,90;460,135;10d,0&L】
        /// </summary>
        /// <param name="_rebar"></param>
        /// <returns></returns>
        private static Bitmap PaintRebarPic_LG(Rebar _rebar)
        {
            Bitmap bitmap = new Bitmap(180, 40);//新建一个bitmap，用于绘图，图片的尺寸为180*40

            Graphics g = System.Drawing.Graphics.FromImage(bitmap);//从bitmap中建一个画图对象
            g.Clear(Color.White);

            Font _font = new Font("微软雅黑", 10, FontStyle.Regular);
            Brush _brush = new SolidBrush(Color.Black);
            Pen _pen = new Pen(new SolidBrush(Color.Black), 3);//粗细3

            try
            {
                //拆解corner信息,
                List<GeneralMultiData> _MultiData = GeneralClass.LDOpt.ldhelper.GetMultiData(_rebar.CornerMessage, _rebar.Diameter);

                int _index_s = (_MultiData[0].ilength == 0) ? 1 : 0;//起始序号，如果第一段长度为0，则为端头，

                g.DrawLine(_pen, new Point(60, 20), new Point(120, 20));//横线
                g.DrawLine(_pen, new Point(120, 20), new Point(120, 25));//竖线右侧短竖线
                g.DrawLine(_pen, new Point(120, 25), new Point(105, 40));//竖线右侧弯钩
                g.DrawLine(_pen, new Point(60, 20), new Point(60, 40));//竖线左侧竖线

                //g.DrawString(_MultiData[_index_s].ilength.ToString(), _font, _brush, new Point(52, 22));//标注长度
                g.DrawString(_MultiData[_index_s + 1].ilength.ToString(), _font, _brush, new Point(80, 0));//标注长度
                //g.DrawString(_MultiData[_index_s + 2].ilength.ToString(), _font, _brush, new Point(132, 22));//标注长度

                ////准备绘制两端套丝和弯曲标识
                //if (_MultiData != null && _MultiData.Count != 0)//
                //{
                //    if (_MultiData.First().type == 2)//端头套丝
                //    {
                //        _pen = new Pen(new SolidBrush(Color.Black), 6);
                //        g.DrawLine(_pen, new Point(50, 30), new Point(50, 40));//画黑色加粗线，标识套丝
                //    }
                //    if (_MultiData.First().ilength == 0 &&
                //        _MultiData.First().headType != EnumMultiHeadType.BEND &&
                //        _MultiData.First().headType != EnumMultiHeadType.ORG)//原始端头和弯曲的不用写端头标识
                //    {
                //        _pen = new Pen(new SolidBrush(Color.Black), 3);
                //        g.DrawString(_MultiData.First().msg_second, _font, _brush, new Point(30, 20));//画端头信息string
                //    }
                //    if (_MultiData.Last().type == 2)//端尾套丝
                //    {
                //        _pen = new Pen(new SolidBrush(Color.Black), 6);
                //        g.DrawLine(_pen, new Point(130, 30), new Point(130, 40));//画黑色加粗线，标识套丝
                //    }
                //    if (_MultiData.Last().headType != EnumMultiHeadType.BEND &&
                //     _MultiData.Last().headType != EnumMultiHeadType.ORG)//原始端头和弯曲的不用写端头标识
                //    {
                //        _pen = new Pen(new SolidBrush(Color.Black), 3);
                //        g.DrawString(_MultiData.Last().msg_second, _font, _brush, new Point(110, 20));//画端头信息string
                //    }

                //}

            }
            catch (Exception ex) { MessageBox.Show("PaintRebar_LG error:" + ex.Message); }

            return bitmap;
        }

        /// <summary>
        /// 画箍筋钢筋的简图，
        /// 注意有半箍的情况，500,90;150,90;500,0&G
        /// </summary>
        /// <param name="_rebar">单根钢筋</param>
        /// <returns></returns>
        private static Bitmap PaintRebarPic_GJ(Rebar _rebar)
        {
            int _gjtype = 0;//箍筋类型，1为全箍，2为半箍
            //先判断是否有半箍的情况
            List<GeneralMultiData> _MultiData = GeneralClass.LDOpt.ldhelper.GetMultiData(_rebar.CornerMessage);//拆解边角信息，提取矩形边长
            //_ret = new Tuple<int, int>(_MultiData[1].ilength, _MultiData[2].ilength);//取第二段长度为箍筋的宽度，取第三段长度为箍筋的高度
            if (_MultiData.Count == 6) { _gjtype = 1; }//全箍
            else if (_MultiData.Count == 3) { _gjtype = 2; }//半箍
            else { _gjtype = 0; }//其他

            Bitmap bitmap = new Bitmap(180, 40);//新建一个bitmap，用于绘图

            Graphics g = System.Drawing.Graphics.FromImage(bitmap);//从bitmap中建一个画图对象
            g.Clear(Color.White);

            int _startX = 50, _startY = 3;

            Pen _pen;
            Point p1, p2;

            string text;
            int fontsize = 10;
            int fontX = 0;
            int fontY = 0;

            int width = 80;//矩形宽度
            int height = 35;//矩形高度

            Font _font = new Font("微软雅黑", fontsize, FontStyle.Regular);
            Brush _brush = new SolidBrush(Color.Black);

            try
            {
                if(_gjtype==1)//全箍
                {
                    //画圆角矩形框
                    _pen = new Pen(new SolidBrush(Color.Black), 3);//粗细3
                    g.DrawArc(_pen, _startX + 0, _startY + 0, 8, 8, 180, 90);
                    g.DrawLine(_pen, _startX + 4, _startY + 0, _startX + width - 5, _startY + 0);
                    g.DrawArc(_pen, _startX + width - 9, _startY + 0, 8, 8, 270, 90);
                    g.DrawLine(_pen, _startX + width - 1, _startY + 4, _startX + width - 1, _startY + height - 5);
                    g.DrawArc(_pen, _startX + width - 9, _startY + height - 9, 8, 8, 0, 90);
                    g.DrawLine(_pen, _startX + 4, _startY + height - 1, _startX + width - 5, _startY + height - 1);
                    g.DrawArc(_pen, _startX + 0, _startY + height - 9, 8, 8, 90, 90);
                    g.DrawLine(_pen, _startX + 0, _startY + 4, _startX + 0, _startY + height - 5);

                    //画右上角的箍筋端头
                    int _delta = 6;
                    g.DrawLine(_pen, _startX + width - 5, _startY + 0, _startX + width - 5 - _delta, _startY + _delta);
                    g.DrawLine(_pen, _startX + width - 1, _startY + 4, _startX + width - 1 - _delta, _startY + 4 + _delta);

                    //画长度标识
                    text = _rebar.GJsize.Item1.ToString();//箍筋宽度
                    fontX = _startX + width / 2 - 15;
                    fontY = _startY + height - 20;
                    g.DrawString(text, _font, _brush, fontX, fontY);//写线段长度，文本标注

                    text = _rebar.GJsize.Item2.ToString();//箍筋高度
                    fontX = _startX + width;
                    fontY = _startY + height / 2 - 10;
                    g.DrawString(text, _font, _brush, fontX, fontY);//写线段长度，文本标注


                }
                else//半箍
                {
                    //画圆角半矩形框
                    _pen = new Pen(new SolidBrush(Color.Black), 3);//粗细3
                    g.DrawArc(_pen, _startX + 0, _startY + 0, 8, 8, 180, 90);
                    g.DrawLine(_pen, _startX + 4, _startY + 0, _startX + width - 5, _startY + 0);
                    //g.DrawArc(_pen, _startX + width - 9, _startY + 0, 8, 8, 270, 90);
                    //g.DrawLine(_pen, _startX + width - 1, _startY + 4, _startX + width - 1, _startY + height - 5);
                    //g.DrawArc(_pen, _startX + width - 9, _startY + height - 9, 8, 8, 0, 90);
                    g.DrawLine(_pen, _startX + 4, _startY + height - 1, _startX + width - 5, _startY + height - 1);
                    g.DrawArc(_pen, _startX + 0, _startY + height - 9, 8, 8, 90, 90);
                    g.DrawLine(_pen, _startX + 0, _startY + 4, _startX + 0, _startY + height - 5);

                    //画长度标识
                    text = _rebar.GJsize.Item2.ToString();//箍筋宽度
                    fontX = _startX + width / 2 - 15;
                    fontY = _startY + height - 20;
                    g.DrawString(text, _font, _brush, fontX, fontY);//写线段长度，文本标注

                    text = _rebar.GJsize.Item1.ToString();//箍筋高度
                    fontX = _startX -30;
                    fontY = _startY + height / 2 - 10;
                    g.DrawString(text, _font, _brush, fontX, fontY);//写线段长度，文本标注

                }









            }
            catch (Exception ex) { MessageBox.Show("PaintRebar_GJ error:" + ex.Message); }

            return bitmap;

        }

        /// <summary>
        /// 画标尺
        /// </summary>
        /// <returns></returns>
        public static Bitmap PaintRuler()
        {
            Bitmap bitmap = new Bitmap(650, 150);//新建一个bitmap，用于绘图

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
                for (int i = 10; i > 0; i--)
                {
                    _start = 0;
                    _end = (int)(((double)GeneralClass.OriginalLength("C", 12) / i) / (double)GeneralClass.OriginalLength("C", 12) * (double)maxPointX);

                    _pen = new Pen(new SolidBrush(Color.Green), 3);
                    p1 = new Point(_start, startY);
                    p2 = new Point(_end, startY);
                    g.DrawLine(_pen, p1, p2);//画绿线，钢筋小段

                    _pen = new Pen(new SolidBrush(Color.Black), 3);
                    p1 = new Point(_end, startY);
                    p2 = new Point(_end, startY - 5);
                    g.DrawLine(_pen, p1, p2);//画竖向的小黑线，分隔线

                    text = ((int)((double)GeneralClass.OriginalLength("C", 12) / i)).ToString() + "*" + i.ToString();
                    fontX = _end - 20;
                    fontY = startY + 5 + (i - 1) * 10;
                    g.DrawString(text, _font, _brush, fontX, fontY);//写线段长度，文本标注

                }

            }
            catch (Exception ex) { MessageBox.Show("PaintRuler error:" + ex.Message); }

            return bitmap;
        }
        /// <summary>
        /// 画线材，全拉直的
        /// </summary>
        /// <param name="_rebar"></param>
        /// <returns></returns>
        public static Bitmap PaintRebarXian(Rebar _rebar)
        {
            Bitmap bitmap = new Bitmap(GeneralClass.taoPicSize.Width, GeneralClass.taoPicSize.Height);//新建一个bitmap，用于绘图

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
                _start = (int)((double)_lengthSum / (double)GeneralClass.OriginalLength("C", 12) * (double)maxPointX);//线材是没有原材长度这个概念的，这里只是为了画图，用C12原材长度
                _end = (int)((double)(_lengthSum + _rebar.length) / (double)GeneralClass.OriginalLength("C", 12) * (double)maxPointX);

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
                List<GeneralMultiData> _MultiData = GeneralClass.LDOpt.ldhelper.GetMultiData(_rebar.CornerMessage, _rebar.Diameter);
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
                            int _bendpos = (int)((double)(_lengthSum + _bendlength) / (double)GeneralClass.OriginalLength("C", 12) * (double)maxPointX);
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
            catch (Exception ex) { MessageBox.Show("PaintRebarXian error:" + ex.Message + ",cornerMsg:" + _rebar.CornerMessage); }

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
        /// 打印构件标签，并且根据棒材线材的选择情况，分别打印
        /// </summary>
        /// <param name="_element">待打印的构件</param>
        /// <param name="_bangPicked">是否选择棒材</param>
        /// <param name="_xianPicked">是否选择线材</param>
        /// <param name="_gjlgPicked">是否选择箍筋拉勾</param>
        /// <param name="_No">项目编号、区域编号和部位编号组合成的元组</param>
        /// <returns></returns>
        public static Bitmap PaintElementLabel(ElementData _element, Tuple<string, string, string> _No, bool _bangPicked = true, bool _xianPicked = true, bool _gjlgPicked = true)
        {
            Bitmap bitmap = new Bitmap(mm2inch(GeneralClass.LabelPrintSizeWidth), mm2inch(GeneralClass.LabelPrintSizeHeight * 100));//新建一个bitmap，用于绘图

            int _totalHeight = 0;//图片实际总高度，用于重绘图片


            Graphics g = System.Drawing.Graphics.FromImage(bitmap);//从bitmap中建一个画图对象
            g.Clear(Color.White);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;// 设置高质量插值法              
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;// 设置高质量平滑度  

            Pen _pen = new Pen(new SolidBrush(Color.Black), 3);
            Point _start = new Point(10, 10);//起始点，可以调整整个页面的偏置
            Point _end = new Point(10, 10);//结束点，可以调整每行的偏置

            PaintString(ref g, "中建壹品*汉韵公馆", _start.X, _start.Y, 15, FontStyle.Bold);//项目名称
            _end.Y += _start.Y + 16;
            //20250303关闭，领导要求去掉矩形框和料仓编号
            //g.DrawRectangle(_pen, new Rectangle(_start.X, _start.Y + 30, 180, 40));//画矩形
            //PaintString(ref g, "料仓编号：", _start.X + 10, _start.Y + 32);//
            //PaintString(ref g, _element.warehouseNo + "-" + _element.wareNo, _start.X + 80, _start.Y + 32);//料仓编号
            //PaintString(ref g, _element.batchSeri, _start.X + 2, _start.Y + 50);//批次流水号

            g.DrawLine(_pen, new Point(_start.X, _end.Y + 2), new Point(_start.X + 180, _end.Y + 2));//画黑线
            _end.Y += 2;


            //PaintString(ref g, _element.projectName, _start.X, _start.Y + 80, 12, FontStyle.Regular);//项目名称
            //PaintString(ref g, _element.assemblyName, _start.X, _start.Y + 110, 12, FontStyle.Regular);//构件部位
            //PaintString(ref g, _element.elementName, _start.X, _start.Y + 150, 16, FontStyle.Bold);//构件名称，20号字体，加粗

            PaintString(ref g, _element.projectName.Replace("汉韵公馆", "") + _element.assemblyName, _start.X, _end.Y + 2, 12, FontStyle.Regular);//项目名称，注意去掉真正的项目名称
            PaintString(ref g, _element.elementName, _start.X, _end.Y + 32, 16, FontStyle.Bold);//构件名称，20号字体，加粗
            _end.Y += 72;

            _pen = new Pen(new SolidBrush(Color.Black), 2);
            _pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;//画虚线、点线

            ////区分棒材、线材
            //int _Threshold = (GeneralClass.m_typeC12) ? 12 : ((GeneralClass.m_typeC14) ? 14 : 16);//先看12是否为棒材，再看14是否为棒材


            int _height = 108;
            int _count = 0;
            for (int i = 0; i < _element.rebarlist.Count; i++)
            {
                //如果没有选棒材，当前又是棒材；或者没有选线材，当前又是线材；或者是没有选择箍筋拉勾马凳，当前又是箍筋拉勾马凳，只有这三种情况，跳过至下一个循环
                if ((!_bangPicked && _element.rebarlist[i].RebarSizeType == EnumRebarSizeType.BANG) ||
                    (!_xianPicked && _element.rebarlist[i].RebarSizeType == EnumRebarSizeType.XIAN) ||
                    (!_gjlgPicked && (_element.rebarlist[i].RebarShapeType == EnumRebarShapeType.SHAPE_GJ ||
                                            _element.rebarlist[i].RebarShapeType == EnumRebarShapeType.SHAPE_LG ||
                                            _element.rebarlist[i].RebarShapeType == EnumRebarShapeType.SHAPE_MD))
                    ) continue;

                //g.DrawLine(_pen, new Point(_start.X, _start.Y + 210 + _height * _count), new Point(_start.X + 180, _start.Y + 210 + _height * _count));//画黑线
                //PaintRebarString(ref g, _element.rebarlist[i].Level, _start.X + 0, _start.Y + 214 + _height * _count, 16, FontStyle.Regular);//钢筋直径符号
                //PaintString(ref g, _element.rebarlist[i].Diameter.ToString(), _start.X + 15, _start.Y + 212 + _height * _count, 14, FontStyle.Regular);//直径
                //PaintString(ref g, _element.rebarlist[i].iLength.ToString(), _start.X + 60, _start.Y + 212 + _height * _count, 14, FontStyle.Regular);//长度
                //PaintString(ref g, _element.rebarlist[i].TotalPieceNum.ToString() + "根", _start.X + 130, _start.Y + 212 + _height * _count, 14, FontStyle.Regular);//数量

                //Bitmap pic = PaintRebarPic(_element.rebarlist[i]);//直接画钢筋简图
                //PaintImage(ref g, pic, new Point(_start.X, _start.Y + 240 + _height * _count), 180, 40);
                //PaintString(ref g, _element.rebarlist[i].Description.ToString(), _start.X + 10, _start.Y + 280 + _height * _count, 9, FontStyle.Regular);//备注

                g.DrawLine(_pen, new Point(_start.X, _end.Y + _height * _count), new Point(_start.X + 180, _end.Y + _height * _count));//画黑线

                PaintRebarString(ref g, _element.rebarlist[i].Level, _start.X + 0, _end.Y + 4 + _height * _count, 17, FontStyle.Regular);//钢筋直径符号
                PaintString(ref g, _element.rebarlist[i].Diameter.ToString(), _start.X + 15, _end.Y + 2 + _height * _count, 13, FontStyle.Regular);//直径
                PaintString(ref g, _element.rebarlist[i].iLength.ToString(), _start.X + 60, _end.Y + 2 + _height * _count, 13, FontStyle.Regular);//长度
                PaintString(ref g, _element.rebarlist[i].TotalPieceNum.ToString() + "根", _start.X + 130, _end.Y + 2 + _height * _count, 13, FontStyle.Regular);//数量

                Bitmap pic = PaintRebarPic(_element.rebarlist[i]);//直接画钢筋简图
                PaintImage(ref g, pic, new Point(_start.X, _end.Y + 30 + _height * _count), 180, 40);

                PaintString(ref g, _element.rebarlist[i].Description.ToString(), _start.X + 10, _end.Y + 70 + _height * _count, 12, FontStyle.Bold, true);//备注

                _count++;//自增计数
            }
            _end.Y += _height * _count;//


            int lineheight = 30;
            //g.DrawLine(_pen, new Point(_start.X, _start.Y + 315 + _height * (_count - 1)),
            //    new Point(_start.X + 180, _start.Y + 315 + _height * (_count - 1)));//画黑线
            g.DrawLine(_pen, new Point(_start.X, _end.Y + 2), new Point(_start.X + 180, _end.Y + 2));//画黑线
            _end.Y += 3;

            //PaintString(ref g, "加工单元:", _start.X, _start.Y + 315 + _height * (_count - 1), 12, FontStyle.Regular);//加工单元
            //g.DrawLine(_pen, new Point(_start.X + 70, _start.Y + 315 + lineheight - 2 + _height * (_count - 1)),
            //            new Point(_start.X + 180, _start.Y + 315 + lineheight - 2 + _height * (_count - 1)));//画黑线

            //PaintString(ref g, "加工人员:", _start.X, _start.Y + 315 + lineheight + _height * (_count - 1), 12, FontStyle.Regular);//加工单元
            //g.DrawLine(_pen, new Point(_start.X + 70, _start.Y + 315 + lineheight * 2 - 2 + _height * (_count - 1)),
            //            new Point(_start.X + 180, _start.Y + 315 + lineheight * 2 - 2 + _height * (_count - 1)));//画黑线

            //PaintString(ref g, "质检员:", _start.X, _start.Y + 315 + lineheight * 2 + _height * (_count - 1), 12, FontStyle.Regular);//加工单元
            //g.DrawLine(_pen, new Point(_start.X + 70, _start.Y + 315 + lineheight * 3 - 2 + _height * (_count - 1)),
            //            new Point(_start.X + 180, _start.Y + 315 + lineheight * 3 - 2 + _height * (_count - 1)));//画黑线


            string _elementSeriNo = _No.Item1 + "_" + _No.Item2 + "_" + _No.Item3 + "_" + String.Join("", _element.elementName.Split('\n'));
            Bitmap _qrCode = CreateQRCode(_elementSeriNo, 100, 100);//打印构件的流水号，形成二维码
            //PaintImage(ref g, _qrCode, new Point(_start.X + 10, _start.Y + 315 + lineheight * 3 - 2 + _height * (_count - 1)), 160, 160);
            PaintImage(ref g, _qrCode, new Point(_start.X + 10, _end.Y), 160, 160);
            _end.Y += 160;

            //绘制先进院logo
            Bitmap _sourceImage = global::RebarSampling.Properties.Resources.logo;//获取先进院logo
            //PaintImage(ref g, _sourceImage, new Point(_start.X, _start.Y + 475 + lineheight * 3 + _height * (_count - 1)), 180, 30);
            PaintImage(ref g, _sourceImage, new Point(_start.X, _end.Y), 180, 30);
            _end.Y += 30;

            //_totalHeight = _start.Y + 475 + lineheight * 3 + _height * (_count - 1) + 30;
            _totalHeight = _end.Y;

            g.Dispose();//回收资源



            //新建一个bitmap，根据图片实际大小重绘图片
            Bitmap _newbitmap = new Bitmap(mm2inch(GeneralClass.LabelPrintSizeWidth), _totalHeight);

            using (Graphics gg = System.Drawing.Graphics.FromImage(_newbitmap))//从_newbitmap中建一个画图对象
            {
                gg.DrawImage(bitmap, 0, 0);
            }

            return _newbitmap;


            #region old                        
            //Bitmap bitmap = new Bitmap(mm2inch(GeneralClass.LabelPrintSizeWidth), mm2inch(GeneralClass.LabelPrintSizeHeight));//新建一个bitmap，用于绘图

            //Graphics g = System.Drawing.Graphics.FromImage(bitmap);//从bitmap中建一个画图对象
            //g.Clear(Color.White);
            //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;// 设置高质量插值法              
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;// 设置高质量平滑度  

            //int fontsize = 0;
            ////Font _font;
            //Brush _brush;

            //Pen _pen;
            //Point _start = new Point(15, 10);

            //fontsize = 15;
            //PaintString(ref g, _element.projectName, _start.X, _start.Y/*, fontsize, FontStyle.Bold*/);//项目名称

            ////画矩形
            //_brush = new SolidBrush(Color.Green);
            //_pen = new Pen(new SolidBrush(Color.Green), 3);
            //g.DrawRectangle(_pen, new Rectangle(_start.X, _start.Y + 20, 170, 60));//画矩形

            //PaintString(ref g, "构件部位：", _start.X, _start.Y + 90);//
            //PaintString(ref g, _element.assemblyName, _start.X + 30, _start.Y + 110);//构件部位

            //PaintString(ref g, "构件名称：", _start.X, _start.Y + 130);//
            //PaintString(ref g, _element.elementName, _start.X + 30, _start.Y + 150);//构件名称
            //PaintString(ref g, "料仓编号：", _start.X, _start.Y + 190);//
            //PaintString(ref g, _element.warehouseNo + "-" + _element.wareNo, _start.X + 30, _start.Y + 210);//料仓编号

            //PaintString(ref g, "钢筋形状：", _start.X, _start.Y + 230);//

            //Bitmap _qrCode = CreateQRCode(_element.batchSeri == string.Empty ? "0" : _element.batchSeri, 180, 180);//打印构件的流水号，形成二维码
            //PaintImage(ref g, _qrCode, new Point(_start.X, _start.Y + 250), 180, 180);

            ////绘制先进院logo
            //Bitmap _sourceImage = global::RebarSampling.Properties.Resources.logo;//获取先进院logo
            //PaintImage(ref g, _sourceImage, new Point(_start.X, _start.Y + 430), 180, 30);

            //return bitmap;
            #endregion
        }
        public static Bitmap PaintCode(Tuple<string, string, string, string> _strs)
        {
            Bitmap bitmap = new Bitmap(mm2inch(GeneralClass.LabelPrintSizeWidth), mm2inch(77));//新建一个bitmap，用于绘图

            Graphics g = System.Drawing.Graphics.FromImage(bitmap);//从bitmap中建一个画图对象
            g.Clear(Color.White);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;// 设置高质量插值法              
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;// 设置高质量平滑度  

            int fontsize = 12;
            Pen _pen = new Pen(new SolidBrush(Color.Black), 3);
            Point _start = new Point(10, 0);

            PaintString(ref g, _strs.Item1, _start.X, _start.Y + 10, fontsize, FontStyle.Bold);//
            PaintString(ref g, _strs.Item2, _start.X, _start.Y + fontsize * 1 + 20, fontsize, FontStyle.Bold);//
            PaintString(ref g, _strs.Item3, _start.X, _start.Y + fontsize * 2 + 30, fontsize, FontStyle.Bold);//
            PaintString(ref g, _strs.Item4, _start.X, _start.Y + fontsize * 3 + 40, fontsize - 2, FontStyle.Regular);//

            Bitmap _qrCode = CreateQRCode(_strs.Item4, 160, 160);//打印构件的流水号，形成二维码
            PaintImage(ref g, _qrCode, new Point(_start.X + 10, _start.Y + fontsize * 4 + 60), 160, 160);

            //绘制先进院logo
            Bitmap _sourceImage = global::RebarSampling.Properties.Resources.logo;//获取先进院logo
            PaintImage(ref g, _sourceImage, new Point(_start.X, _start.Y + fontsize * 4 + 220), 180, 30);

            return bitmap;
        }
        /// <summary>
        /// 画批量锯切标签
        /// </summary>
        /// <param name="_element"></param>
        /// <returns></returns>
        public static Bitmap PaintPiCutLabel(RebarPi _pi)
        {
            Bitmap bitmap = new Bitmap(mm2inch(GeneralClass.LabelPrintSizeWidth), mm2inch(GeneralClass.LabelPrintSizeHeight));//新建一个bitmap，用于绘图

            Graphics g = System.Drawing.Graphics.FromImage(bitmap);//从bitmap中建一个画图对象
            g.Clear(Color.White);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;// 设置高质量插值法              
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;// 设置高质量平滑度  

            int fontsize = 0;
            //Font _font;
            //Brush _brush;

            Pen _pen;
            Point _start = new Point(10, 10);

            fontsize = 15;
            //PaintString(ref g, _element.projectName, _start.X, _start.Y/*, fontsize, FontStyle.Bold*/);//项目名称

            //画矩形
            //_brush = new SolidBrush(Color.Green);
            _pen = new Pen(new SolidBrush(Color.Green), 3);
            //p1 = new Point(0, 30);
            //p2 = new Point(350, 30);
            //g.DrawLine(_pen, p1, p2);
            PaintString(ref g, "中建壹品*汉韵公馆", _start.X, _start.Y, 14);//项目名称

            g.DrawRectangle(_pen, new Rectangle(_start.X, _start.Y + 30, 160, 40));//画矩形

            //PaintString(ref g, "构件部位：", _start.X, _start.Y + 90);//
            //PaintString(ref g, _element.assemblyName, _start.X + 70, _start.Y + 90);//构件部位
            //PaintString(ref g, "构件名称：", _start.X, _start.Y + 110);//
            //PaintString(ref g, _element.elementName, _start.X + 70, _start.Y + 110);//构件名称
            //PaintString(ref g, "料仓编号：", _start.X, _start.Y + 130);//
            //PaintString(ref g, _element.warehouseNo + "-" + _element.wareNo, _start.X + 70, _start.Y + 130);//料仓编号

            PaintString(ref g, _pi._rebarList[0].ProjectName, _start.X, _start.Y + 80, 12, FontStyle.Regular);//项目名称
            PaintString(ref g, _pi._rebarList[0].MainAssemblyName, _start.X, _start.Y + 110, 12, FontStyle.Regular);//项目名称

            //PaintString(ref g, _pi._rebarList[0].MainAssemblyName, _start.X + 70, _start.Y + 90);//构件部位名称
            //PaintString(ref g, _pi._rebarList[0].ElementName, _start.X + 10, _start.Y + 45,20,FontStyle.Bold);//构件名称，20号字体，加粗





            PaintString(ref g, "直径：", _start.X, _start.Y + 140);//
            //PaintRebarString(ref g, "C", _start.X + 70, _start.Y + 140, 18, FontStyle.Bold);//钢筋直径符号
            PaintRebarString(ref g, _pi._rebarList[0].Level, _start.X + 70, _start.Y + 140, 18, FontStyle.Regular);//钢筋直径符号
            PaintString(ref g, _pi._diameter.ToString(), _start.X + 85, _start.Y + 140, 16, FontStyle.Regular);//直径

            PaintString(ref g, "长度：", _start.X, _start.Y + 170);//
            PaintString(ref g, _pi.length.ToString(), _start.X + 70, _start.Y + 170, 16, FontStyle.Regular);//长度
            PaintString(ref g, "数量：", _start.X, _start.Y + 200);//
            PaintString(ref g, _pi.num.ToString() + "根", _start.X + 70, _start.Y + 200, 16, FontStyle.Regular);//数量
            PaintString(ref g, "加工信息：", _start.X, _start.Y + 230);//
            PaintString(ref g, _pi._cornerMsg, _start.X, _start.Y + 245, 10, FontStyle.Regular);//数量


            PaintString(ref g, "钢筋形状：", _start.X, _start.Y + 295);//
            //Bitmap pic = (Bitmap)GeneralClass.interactivityData?.getImageUsePicNum(_pi._picTypeNum);
            Bitmap pic = PaintRebarPic(_pi._rebarList[0]);//直接画钢筋简图
            PaintImage(ref g, pic, new Point(_start.X, _start.Y + 310), 180, 40);


            //PaintString(ref g, "加工厂：", 10, 180);//项目名称
            //PaintString(ref g, "庙岭加工基地", 80, 180);//杨春湖华侨城
            //PaintString(ref g, "加工日期：", 10, 200);//项目名称
            //PaintString(ref g, "2024-04-22", 80, 200);//杨春湖华侨城
            //PaintString(ref g, "需求日期：", 10, 220);//项目名称
            //PaintString(ref g, "2024-04-25", 80, 220);//杨春湖华侨城


            Bitmap _qrCode = CreateQRCode(_pi._batchseri == string.Empty ? "0" : _pi._batchseri, 120, 120);//打印构件的流水号，形成二维码
            //g.DrawImage(_qrCode, new System.Drawing.Point(80, 240));
            PaintImage(ref g, _qrCode, new Point(_start.X + 40, _start.Y + 350), 120, 120);

            //绘制先进院logo
            Bitmap _sourceImage = global::RebarSampling.Properties.Resources.logo;//获取先进院logo
            PaintImage(ref g, _sourceImage, new Point(_start.X, _start.Y + 470), 180, 30);
            //int _width = 168;
            //int _height = 24;
            //double xRatio = (double)_width / _sourceImage.Width;// 计算缩略图的缩放比例  
            //double yRatio = (double)_height / _sourceImage.Height;
            //double ratio = Math.Min(xRatio, yRatio);
            //g.DrawImage(_sourceImage, 10, 320, (int)(_sourceImage.Width * ratio), (int)(_sourceImage.Height * ratio));// 绘制缩略图  

            return bitmap;
        }

        /// <summary>
        /// 按比例缩放显示图片
        /// </summary>
        /// <param name="_g"></param>
        /// <param name="_bmp"></param>
        /// <param name="_start"></param>
        /// <param name="_width"></param>
        /// <param name="_height"></param>
        private static void PaintImage(ref Graphics _g, Bitmap _bmp, Point _start, int _width, int _height)
        {
            if (_bmp != null)
            {
                double xRatio = (double)_width / _bmp.Width;// 计算缩略图的缩放比例  
                double yRatio = (double)_height / _bmp.Height;
                double ratio = Math.Min(xRatio, yRatio);
                _g.DrawImage(_bmp, _start.X, _start.Y, (int)(_bmp.Width * ratio), (int)(_bmp.Height * ratio));// 绘制缩略图  
            }
        }
        /// <summary>
        /// 画字符串
        /// </summary>
        /// <param name="_g"></param>
        /// <param name="_text"></param>
        /// <param name="_startX"></param>
        /// <param name="_startY"></param>
        /// <param name="_fontsize"></param>
        /// <param name="_style"></param>
        /// <param name="_multi">区分是否画多行，如果false，则只显示一行</param>
        private static void PaintString(ref Graphics _g, string _text, int _startX, int _startY, int _fontsize = 10, FontStyle _style = FontStyle.Regular, bool _multi = false)
        {
            Brush _brush;
            Font _font;

            _font = new Font("微软雅黑", _fontsize, _style);
            _brush = new SolidBrush(Color.Black);

            if (_multi)//画多行
            {
                RectangleF descRect = new RectangleF();//一个可变大小的矩形区域，用于显示多行字符串
                descRect.Location = new Point(_startX, _startY);
                descRect.Size = new Size(180, ((int)_g.MeasureString(_text, _font, 180, StringFormat.GenericTypographic).Height));

                _g.DrawString(_text, _font, _brush, descRect);
            }
            else
            {
                _g.DrawString(_text, _font, _brush, new Point(_startX, _startY));
            }



            //string txtDescription = "这是一段非常长的字符串";
            //RectangleF descRect = new RectangleF();
            ////using (Font useFont = new Font("SimSun", 28, FontStyle.Bold))
            //{
            //    descRect.Location = new Point(30, 105);
            //    descRect.Size = new Size(600, ((int)e.Graphics.MeasureString(txtDescription, useFont, 600, StringFormat.GenericTypographic).Height));
            //    e.Graphics.DrawString(txtDescription, useFont, Brushes.Black, descRect);
            //}



            //StringFormat sf = new StringFormat();
            //sf.Alignment = StringAlignment.Center;//水平居中
            //sf.LineAlignment = StringAlignment.Center;//垂直居中

        }
        /// <summary>
        /// 画钢筋直径符号专用字体，使用GJFH字体库（SJQY）
        /// </summary>
        /// <param name="_g"></param>
        /// <param name="_level">钢筋级别</param>
        /// <param name="_startX"></param>
        /// <param name="_startY"></param>
        /// <param name="_fontsize"></param>
        /// <param name="_style"></param>
        private static void PaintRebarString(ref Graphics _g, string _text, int _startX, int _startY, int _fontsize = 10, FontStyle _style = FontStyle.Regular)
        {
            Brush _brush;
            Font _font;

            //_font = new Font("SJQY", _fontsize, _style);//SJQY字体仅含有ABCDE
            _font = new Font("GJFH", _fontsize, _style);//GJFH字体含有ABCDEFGHI

            _brush = new SolidBrush(Color.Black);
            _g.DrawString(_text, _font, _brush, _startX, _startY);
        }
        /// <summary>
        /// 二维码方法
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public static Bitmap CreateQRCode(string asset, int _width, int _height)
        {
            try
            {
                EncodingOptions options = new QrCodeEncodingOptions
                {
                    DisableECI = true,
                    CharacterSet = "UTF-8", //编码
                    Width = _width,             //宽度
                    Height = _height             //高度
                };
                BarcodeWriter writer = new BarcodeWriter();
                writer.Format = BarcodeFormat.QR_CODE;//QR码
                                                      //writer.Format = BarcodeFormat.DATA_MATRIX;//DATA_MATRIX码
                                                      //writer.Format = BarcodeFormat.CODE_39;//39码
                                                      //writer.Format = BarcodeFormat.CODE_128;//128码

                writer.Options = options;
                return writer.Write(asset);
            }
            catch (Exception e) { MessageBox.Show("CreateQRCode error:" + e.Message); return null; }

        }

    }
}
