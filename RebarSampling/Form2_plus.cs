using MathNet.Numerics.Optimization;
using NPOI.OpenXmlFormats.Dml;
//using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RebarSampling
{
    /******
     * 本类为form2的分支，处理鼠标拖动实现套料结果手动组合、变更的功能
     *  
     * 
     * ******/
    public partial class Form2 : Form
    {

        /// <summary>
        /// 用于批量锯切自动套料的list
        /// </summary>
        private List<RebarOri> _piAutoTaoTargetList = new List<RebarOri>();
        /// <summary>
        /// 起点rebarOri
        /// </summary>
        private List<KeyValuePair<int, RebarOri>> _sourceOri = new List<KeyValuePair<int, RebarOri>>();
        /// <summary>
        /// 目标rebarOri
        /// </summary>
        private KeyValuePair<int, RebarOri> _targetOri = new KeyValuePair<int, RebarOri>();



        /// <summary>
        /// 用于批量锯切手动套料的list
        /// </summary>
        private List<RebarOri> _piManulTaoTargetList = new List<RebarOri>();
        /// <summary>
        /// 用于手动套料的数据源
        /// </summary>
        private List<Rebar> _piManualTaoSourceList = new List<Rebar>();
        /// <summary>
        /// 手动套料是否允许
        /// </summary>
        private bool _manualEnabled = false;
        /// <summary>
        /// 正在拖动中的rebar
        /// </summary>
        private List<Rebar> _flyingRebar = new List<Rebar>();


        private void dataGridView37_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                // 获取鼠标在DataGridView中的位置（行和列索引）  
                int rowIndex = dataGridView37.HitTest(e.X, e.Y).RowIndex;
                int colIndex = dataGridView37.HitTest(e.X, e.Y).ColumnIndex;

                // 检查是否确实点击在了一个单元格上  
                if (rowIndex >= 0 && colIndex >= 1)//确保鼠标点击在图片列
                {

                    Rectangle rectCurCell = dataGridView37.GetCellDisplayRectangle(colIndex, rowIndex, true);//获取当前单元格相对dgv控件的坐标

                    //Point _leftTop = new Point(dataGridView37.Rows[rowIndex].Cells[colIndex].ContentBounds.Left + rectCurCell.Left,
                    //    dataGridView37.Rows[rowIndex].Cells[colIndex].ContentBounds.Top + rectCurCell.Top);

                    Rectangle rectPic = new Rectangle(rectCurCell.Location, GeneralClass.taoPicSize);//取单元格的左上角坐标和套料显示图片的尺寸

                    if (rectPic.Contains(e.X, e.Y))//鼠标位置是否在图片区域内
                    {
                        Point _p = new Point(e.X - rectCurCell.X, e.Y - rectCurCell.Y);//建立一个相对坐标点，为鼠标位置相对图片左上角的坐标

                        RebarOri _rebarOri = _piAutoTaoTargetList[rowIndex];
                        Modifylist(_p, ref _rebarOri, true);

                        dataGridView37.Rows[rowIndex].Cells[colIndex].Value = graphics.PaintRebar(_rebarOri);

                        _sourceOri?.Clear();//清空
                        _sourceOri.Add(new KeyValuePair<int, RebarOri>(rowIndex, _rebarOri));//将本rebarOri存入source，等待拖拽
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("dataGridView37_MouseDown error:" + ex.Message); }

        }

        private void dataGridView37_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                // 获取鼠标在DataGridView中的位置（行和列索引）  
                int rowIndex = dataGridView37.HitTest(e.X, e.Y).RowIndex;
                int colIndex = dataGridView37.HitTest(e.X, e.Y).ColumnIndex;

                bool _haveSelect = false;
                // 检查是否确实点击在了一个单元格上  
                if (rowIndex >= 0 && colIndex >= 1)//确保鼠标点击在图片列
                {
                    foreach (var item in _piAutoTaoTargetList[rowIndex]._list)
                    {
                        if (item.PickUsed)
                        {
                            _haveSelect = true;//有被选中的
                            break;
                        }
                    }
                    if (_haveSelect)//分别执行不同的操作，如果有选中的，就只是取消选中，如果没有选中的就要完成整个拖拽动作，并重新绘制起点跟目标rebarOri
                    {
                        //有选中的，执行取消选中
                        Rectangle rectCurCell = dataGridView37.GetCellDisplayRectangle(colIndex, rowIndex, true);//获取当前单元格相对dgv控件的坐标

                        Rectangle rectPic = new Rectangle(rectCurCell.Location, GeneralClass.taoPicSize);//取单元格的左上角坐标和套料显示图片的尺寸

                        if (rectPic.Contains(e.X, e.Y))//鼠标位置是否在图片区域内
                        {
                            Point _p = new Point(e.X - rectCurCell.X, e.Y - rectCurCell.Y);//建立一个相对坐标点，为鼠标位置相对图片左上角的坐标

                            RebarOri _rebarOri = _piAutoTaoTargetList[rowIndex];
                            Modifylist(_p, ref _rebarOri, false);//修改list元素的pick状态
                            dataGridView37.Rows[rowIndex].Cells[colIndex].Value = graphics.PaintRebar(_rebarOri);//重绘套料显示图
                        }
                        _sourceOri?.Clear();//如果只是取消选中，则清空待拖拽list

                    }
                    else
                    {
                        //没有选中的,执行拖拽，sourceOri的list去除选中Ori，targetOri的list增加选中Ori
                        _targetOri = new KeyValuePair<int, RebarOri>(rowIndex, _piAutoTaoTargetList[rowIndex]);//目标位置rebarOri
                        if (_sourceOri != null && _sourceOri.Count != 0)
                        {
                            int _sourceIndex = _sourceOri[0].Key;
                            RebarOri temp = _sourceOri[0].Value;

                            for (int i = _piAutoTaoTargetList[_sourceIndex]._list.Count - 1; i >= 0; i--)
                            {
                                if (_piAutoTaoTargetList[_sourceIndex]._list[i].PickUsed)
                                {
                                    _piAutoTaoTargetList[rowIndex]._list.Add(_piAutoTaoTargetList[_sourceIndex]._list[i]);//targetOri的list增加
                                    _piAutoTaoTargetList[_sourceIndex]._list.RemoveAt(i);//sourceOri的list去除
                                    foreach (var tt in _piAutoTaoTargetList[rowIndex]._list)
                                    {
                                        tt.PickUsed = false;//复位pick状态
                                    }
                                    break;
                                }
                            }

                            dataGridView37.Rows[_sourceIndex].Cells[colIndex].Value = graphics.PaintRebar(_piAutoTaoTargetList[_sourceIndex]);//重绘sourceOri
                            dataGridView37.Rows[rowIndex].Cells[colIndex].Value = graphics.PaintRebar(_piAutoTaoTargetList[rowIndex]);//重绘targetOri
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("dataGridView37_MouseUp error:" + ex.Message); }

        }

        /// <summary>
        /// 根据鼠标位置，修改rebarOri的list中的pickUsed状态，为后面PaintRebarOri做准备，_modify：true:置位，false：复位
        /// </summary>
        /// <param name="_p">鼠标位置</param>
        /// <param name="_rebarOri"></param>
        /// <param name="_modify">true:置位，false：复位</param>
        private void Modifylist(Point _p, ref RebarOri _rebarOri, bool _modify)
        {
            if (_rebarOri._list != null && _rebarOri._list.Count != 0)
            {
                int _lengAdd = 0;
                List<int> _endlist = new List<int>();//list所有rebar段的终点列表
                _endlist.Add(0);//增加一个0起点
                foreach (var item in _rebarOri._list)
                {
                    _lengAdd += item.length;
                    _endlist.Add((int)((double)(_lengAdd) / (double)GeneralClass.OriginalLength * 600));
                }
                _endlist.Add(600);//增加一个足尺寸终点

                for (int i = 0; i < _endlist.Count - 2; i++)
                {
                    if (_p.X > _endlist[i] && _p.X <= _endlist[i + 1])
                    {
                        _rebarOri._list[i].PickUsed = _modify;

                        return;
                    }

                }
            }
        }
        //判断dgv2的Alt是否触发，跟dgv2的鼠标左键点击形成组合键，用于添加多个flyingRebar
        private bool isShiftDown = false;

        /// <summary>
        /// 长度统计表拖拽至手动套料表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView2_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!_manualEnabled) return;

                //FillDGV_Pi_Tao_Manual(_piManulTaoTargetList);//先把右边设为不折叠状态

                // 获取鼠标在DataGridView中的位置（行和列索引）  
                int rowIndex = dataGridView2.HitTest(e.X, e.Y).RowIndex;
                int colIndex = dataGridView2.HitTest(e.X, e.Y).ColumnIndex;

                DataTable dt = dataGridView2.DataSource as DataTable;

                // 检查是否确实点击在了一个单元格上  
                if (rowIndex >= 0 && colIndex >= 0)//
                {
                    int _length = (int)(dt.Rows[rowIndex][1]);
                    int _num = (int)(dt.Rows[rowIndex][2]);

                    _flyingRebar.Clear();
                    var templist = _piManualTaoSourceList.Where(t => t.length == _length).ToList();
                    if (isShiftDown)//如果Alt键也按下，则为组合，此时可多放几个rebar
                    {
                        int _multi = GeneralClass.OriginalLength / _length;
                        _flyingRebar.AddRange(templist.Take(Math.Min(_multi, templist.Count)));//取多个作为flying，此处数量取剩余根数和整长段数的较小值
                        isShiftDown = false;//复位
                    }
                    else
                    {
                        _flyingRebar.AddRange(templist.Take(1));//将符合长度要求的rebar，取第一个作为flying

                    }

                }
            }
            catch (Exception ex) { MessageBox.Show("dataGridView2_MouseDown error:" + ex.Message); }

        }

        private void dataGridView2_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_manualEnabled) return;

        }

        //private bool isDoubleClicked = false;

        /// <summary>
        /// 手动套料表左键双击，自动增加一根
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView38_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                /***20240524
                 * 因双击事件与单击事件冲突，暂时关闭***/


                //if (!_manualEnabled) return;

                //if (e.Button == MouseButtons.Left)
                //{
                //    isDoubleClicked = true;//双击标志置为true

                //    // 获取鼠标在DataGridView中的位置（行和列索引）  
                //    int rowIndex = dataGridView38.HitTest(e.X, e.Y).RowIndex;
                //    int colIndex = dataGridView38.HitTest(e.X, e.Y).ColumnIndex;

                //    // 检查是否确实点击在了一个单元格上  
                //    if (rowIndex >= 0 && colIndex >= 1 && rowIndex < _piManulTaoTargetList.Count)//确保鼠标点击在图片列
                //    {

                //        Rectangle rectCurCell = dataGridView38.GetCellDisplayRectangle(colIndex, rowIndex, true);//获取当前单元格相对dgv控件的坐标

                //        Rectangle rectPic = new Rectangle(rectCurCell.Location, GeneralClass.taoPicSize);//取单元格的左上角坐标和套料显示图片的尺寸

                //        if (rectPic.Contains(e.X, e.Y))//鼠标位置是否在图片区域内
                //        {
                //            Point _p = new Point(e.X - rectCurCell.X, e.Y - rectCurCell.Y);//建立一个相对坐标点，为鼠标位置相对图片左上角的坐标

                //            RebarOri _rebarOri = _piManulTaoTargetList[rowIndex];
                //            Modifylist(_p, ref _rebarOri, true);

                //            var ttt = _rebarOri._list.Where(t => t.PickUsed == true).ToList();//选中的rebar

                //            _flyingRebar.Clear();
                //            var templist = _piManualTaoSourceList.Where(t => t.length == ttt[0].length).ToList();
                //            _flyingRebar.AddRange(templist.Take(1));//将符合长度要求的rebar，取第一个作为flying

                //            _rebarOri._list.AddRange(_flyingRebar);//添加一个rebar
                //            foreach (var item in _flyingRebar)
                //            {
                //                _piManualTaoSourceList.Remove(item);//sourcelist中去除掉已经手动套料的rebar
                //            }
                //            Modifylist(_p, ref _rebarOri, false);//复位pickused

                //            RefreshUI();

                //        }
                //    }

                //    isDoubleClicked = false;
                //}

            }
            catch (Exception ex) { MessageBox.Show("dataGridView38_MouseDoubleClick error:" + ex.Message); }
        }
        /// <summary>
        /// 手动套料表右键取消选中，返回sourcelist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView38_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!_manualEnabled) return;

                if (e.Button == MouseButtons.Right)
                {
                    // 获取鼠标在DataGridView中的位置（行和列索引）  
                    int rowIndex = dataGridView38.HitTest(e.X, e.Y).RowIndex;
                    int colIndex = dataGridView38.HitTest(e.X, e.Y).ColumnIndex;

                    // 检查是否确实点击在了一个单元格上  
                    if (rowIndex >= 0 && colIndex >= 0 && rowIndex < _piManulTaoTargetList.Count)//确保鼠标点击在图片列
                    {

                        Rectangle rectCurCell = dataGridView38.GetCellDisplayRectangle(colIndex, rowIndex, true);//获取当前单元格相对dgv控件的坐标

                        Rectangle rectPic = new Rectangle(rectCurCell.Location, GeneralClass.taoPicSize);//取单元格的左上角坐标和套料显示图片的尺寸

                        if (rectPic.Contains(e.X, e.Y))//鼠标位置是否在图片区域内
                        {
                            Point _p = new Point(e.X - rectCurCell.X, e.Y - rectCurCell.Y);//建立一个相对坐标点，为鼠标位置相对图片左上角的坐标

                            RebarOri _rebarOri = _piManulTaoTargetList[rowIndex];
                            Modifylist(_p, ref _rebarOri, true);

                            _piManualTaoSourceList.AddRange(_rebarOri._list.Where(t => t.PickUsed == true).ToList());//将选中的元素返回sourcelist
                            _rebarOri._list.RemoveAll(t => t.PickUsed == true);//删掉右键选中的元素

                            dataGridView38.Rows[rowIndex].Cells[colIndex].Value = graphics.PaintRebar(_rebarOri);

                            foreach (var item in _piManualTaoSourceList)
                            {
                                item.PickUsed = false;//复位
                            }
                            RefreshUI();

                            //_sourceOri?.Clear();//清空
                            //_sourceOri.Add(new KeyValuePair<int, RebarOri>(rowIndex, _rebarOri));//将本rebarOri存入source，等待拖拽
                        }
                    }
                }

            }
            catch (Exception ex) { MessageBox.Show("dataGridView38_MouseDown error:" + ex.Message); }


        }

        private void dataGridView38_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                //if(isDoubleClicked)
                //{
                //    return;
                //}

                // 获取鼠标在DataGridView中的位置（行和列索引）  
                int rowIndex = dataGridView38.HitTest(e.X, e.Y).RowIndex;
                int colIndex = dataGridView38.HitTest(e.X, e.Y).ColumnIndex;

                //bool _haveSelect = false;

                // 检查是否确实点击在了一个单元格上  
                if (rowIndex >= 0 && colIndex >= 0)//
                {
                    #region 手动套料
                    if (!_manualEnabled) return;

                    if (_flyingRebar != null && _flyingRebar.Count != 0)
                    {
                        if (_piManulTaoTargetList.Count < rowIndex + 1)//如果选中的行数超过list大小，则新增rebarOri
                        {
                            RebarOri temp = new RebarOri();
                            _piManulTaoTargetList.Add(temp);
                        }
                        //刷新手动套料表
                        _piManulTaoTargetList[rowIndex]._list.AddRange(_flyingRebar);
                        //FillDGV_Pi_Tao_Manual(_piManulTaoList);

                        //刷新长度统计表
                        foreach (var item in _flyingRebar)
                        {
                            _piManualTaoSourceList.Remove(item);//sourcelist中去除掉已经手动套料的rebar
                        }
                        //int _curIndex = dataGridView2.CurrentRow.Index;
                        //FillDGV_Length(_piManualTaoSourceList);
                        //dataGridView2.Rows[_curIndex].Selected = true;
                        RefreshUI();
                    }
                    _flyingRebar.Clear();//拖动完成清空
                    #endregion


                    #region 手动拖拽
                    //foreach (var item in _piAutoTaoList[rowIndex]._list)
                    //{
                    //    if (item.PickUsed)
                    //    {
                    //        _haveSelect = true;//有被选中的
                    //        break;
                    //    }
                    //}
                    //if (_haveSelect)//分别执行不同的操作，如果有选中的，就只是取消选中，如果没有选中的就要完成整个拖拽动作，并重新绘制起点跟目标rebarOri
                    //{
                    //    //有选中的，执行取消选中
                    //    Rectangle rectCurCell = dataGridView38.GetCellDisplayRectangle(colIndex, rowIndex, true);//获取当前单元格相对dgv控件的坐标

                    //    Rectangle rectPic = new Rectangle(rectCurCell.Location, GeneralClass.taoPicSize);//取单元格的左上角坐标和套料显示图片的尺寸

                    //    if (rectPic.Contains(e.X, e.Y))//鼠标位置是否在图片区域内
                    //    {
                    //        Point _p = new Point(e.X - rectCurCell.X, e.Y - rectCurCell.Y);//建立一个相对坐标点，为鼠标位置相对图片左上角的坐标

                    //        RebarOri _rebarOri = _piAutoTaoList[rowIndex];
                    //        Modifylist(_p, ref _rebarOri, false);//修改list元素的pick状态
                    //        dataGridView38.Rows[rowIndex].Cells[colIndex].Value = graphics.PaintRebar(_rebarOri);//重绘套料显示图
                    //    }
                    //    _sourceOri?.Clear();//如果只是取消选中，则清空待拖拽list

                    //}
                    //else
                    //{
                    //    //没有选中的,执行拖拽，sourceOri的list去除选中Ori，targetOri的list增加选中Ori
                    //    _targetOri = new KeyValuePair<int, RebarOri>(rowIndex, _piAutoTaoList[rowIndex]);//目标位置rebarOri
                    //    if (_sourceOri != null && _sourceOri.Count != 0)
                    //    {
                    //        int _sourceIndex = _sourceOri[0].Key;
                    //        RebarOri temp = _sourceOri[0].Value;

                    //        for (int i = _piAutoTaoList[_sourceIndex]._list.Count - 1; i >= 0; i--)
                    //        {
                    //            if (_piAutoTaoList[_sourceIndex]._list[i].PickUsed)
                    //            {
                    //                _piAutoTaoList[rowIndex]._list.Add(_piAutoTaoList[_sourceIndex]._list[i]);//targetOri的list增加
                    //                _piAutoTaoList[_sourceIndex]._list.RemoveAt(i);//sourceOri的list去除
                    //                foreach (var tt in _piAutoTaoList[rowIndex]._list)
                    //                {
                    //                    tt.PickUsed = false;//复位pick状态
                    //                }
                    //                break;
                    //            }
                    //        }

                    //        dataGridView38.Rows[_sourceIndex].Cells[colIndex].Value = graphics.PaintRebar(_piAutoTaoList[_sourceIndex]);//重绘sourceOri
                    //        dataGridView38.Rows[rowIndex].Cells[colIndex].Value = graphics.PaintRebar(_piAutoTaoList[rowIndex]);//重绘targetOri
                    //    }
                    //}
                    #endregion


                }

            }
            catch (Exception ex) { MessageBox.Show("dataGridView38_MouseUp error:" + ex.Message); }
        }

        /// <summary>
        /// 待复制的rebarOri
        /// </summary>
        private RebarOri _copyRebarOri = null;
        private void dataGridView38_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                if (e.Control && e.KeyCode == Keys.C)//ctrl+c,复制
                {
                    _copyRebarOri = new RebarOri();

                    int _curIndex = dataGridView38.CurrentRow.Index;
                    _copyRebarOri = _piManulTaoTargetList[_curIndex];
                }
                if (e.Control && e.KeyCode == Keys.V)//ctrl+v,粘贴
                {
                    while (true)
                    {
                        //先判断sourcelist中的rebar够不够，不够就要return,20240520添加
                        var _gg = _copyRebarOri._list.GroupBy(t => t.length).ToList();
                        foreach (var iii in _gg)
                        {
                            var _have = _piManualTaoSourceList.Where(t => t.length == iii.Key).ToList();

                            if (_have.Count < iii.ToList().Count)//数量不够了
                            {
                                MessageBox.Show("长度为:"+iii.Key.ToString()+" 的钢筋，数量不够！");
                                RefreshUI();//数量不够的时候刷新一次界面，再退出
                                return;
                            }
                        }

                        //手动从sourcelist中取rebar
                        RebarOri temp = new RebarOri();
                        foreach (var item in _copyRebarOri._list)
                        {
                            var tttlist = _piManualTaoSourceList.Where(t => t.length == item.length).ToList();//从source中取长度一致的一个rebar
                            temp._list.Add(tttlist[0]);//将符合长度要求的rebar，取第一个

                            _piManualTaoSourceList.Remove(tttlist[0]);//sourcelist中去除掉已经手动套料的rebar
                        }
                        _piManulTaoTargetList.Add(temp);

                        //RefreshUI();

                        Thread.Sleep(1);
                    }


                }
            }
            catch (Exception ex) { MessageBox.Show("dataGridView38_KeyDown error:" + ex.Message); }

        }
        private void RefreshUI()
        {
            try
            {
                //刷新手动套料表
                int m_curIndex = dataGridView38.FirstDisplayedScrollingRowIndex;//保持当前滚动显示的第一行不变
                FillDGV_Pi_Tao_Manual(_piManulTaoTargetList);
                dataGridView38.FirstDisplayedScrollingRowIndex = m_curIndex;

                //刷新长度统计表
                int l_curIndex = (int)(dataGridView2.CurrentRow?.Index);//保持选中行不变
                FillDGV_Length(_piManualTaoSourceList);
                dataGridView2.Rows[l_curIndex].Selected = true;
            }
            catch (Exception ex) { MessageBox.Show("RefreshUI error:" + ex.Message); }
        }

        /// <summary>
        /// 棒材的批量锯切统计，主要是按照长度统计
        /// </summary>
        /// <param name="_sonlist"></param>
        private void FillDGV_Length(List<Rebar> _sonlist, List<Rebar> _mumlist = null)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(" ", typeof(bool));
            dt.Columns.Add("长度(mm)", typeof(int));
            dt.Columns.Add("数量(根)", typeof(int));
            dt.Columns.Add("数量(%)", typeof(double));
            dt.Columns.Add("重量(kg)", typeof(double));
            dt.Columns.Add("重量(%)", typeof(double));

            //List<GroupbyLengthDatalist> _sonalllist = GeneralClass.SQLiteOpt.QueryAllListByLength(_sonlist);
            //List<GroupbyLengthDatalist> _mumalllist = GeneralClass.SQLiteOpt.QueryAllListByLength(_mumlist);
            var _sonalllist = _sonlist.GroupBy(x => x.length).Select(
                            y => new
                            {
                                _length = y.Key,
                                _totalnum = y.Sum(item => item.TotalPieceNum),
                                _totalweight = y.Sum(item => item.TotalWeight),
                                _datalist = y.ToList()
                            }).OrderByDescending(t => t._length).ToList();//按长度降序排列
            var _mumalllist = ((_mumlist == null) ? _sonlist : _mumlist).GroupBy(x => x.length).Select(
                y => new
                {
                    _length = y.Key,
                    _totalnum = y.Sum(item => item.TotalPieceNum),
                    _totalweight = y.Sum(item => item.TotalWeight),
                    _datalist = y.ToList()
                }).OrderByDescending(t => t._length).ToList();

            double total_num = 0;
            double total_weight = 0.0;

            foreach (var item in _mumalllist)//使用分母list的统计数据，作为分母
            {
                total_num += item._totalnum;
                total_weight += item._totalweight;
            }
            //int ilength = 0;
            foreach (var item in _sonalllist)
            {
                //if (!int.TryParse(item._length, out ilength))//针对缩尺~
                //{
                //    string[] tt = item._length.Split('~');
                //    ilength = (Convert.ToInt32(tt[0]) + Convert.ToInt32(tt[1])) / 2;
                //}

                dt.Rows.Add(false, /*ilength*/item._length, item._totalnum, (double)item._totalnum / total_num, item._totalweight, item._totalweight / total_weight);
            }

            dataGridView2.DataSource = dt;
            //dataGridView2.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns[3].DefaultCellStyle.Format = "P2";
            dataGridView2.Columns[4].DefaultCellStyle.Format = "0.00";
            dataGridView2.Columns[5].DefaultCellStyle.Format = "P2";

        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (!CheckDiameterSinglePick())
            {
                MessageBox.Show("操作无效，请先确保只选择一种直径规格的钢筋！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _piManulTaoTargetList = new List<RebarOri>();
            FillDGV_Pi_Tao_Manual(_piManulTaoTargetList);//清空dt


            List<RebarData> _sonlist = new List<RebarData>();//添加直径筛选后的数据源
            int _threshold = (GeneralClass.m_typeC12) ? 12 : ((GeneralClass.m_typeC14) ? 14 : 16);//先看12是否为棒材，再看14是否为棒材
            foreach (RebarData _dd in GeneralClass.AllRebarList)
            {
                if (_dd.Diameter >= _threshold && _dd.TotalPieceNum != 0
                        && CheckLengthType(_dd, false)
                        && CheckWorkType(_dd, false)
                        && CheckDiameter(_dd, false))
                {
                    _sonlist.Add(_dd);
                }
            }

            _piManualTaoSourceList = Algorithm.ListExpand(_sonlist);

            InitManualTao();//初始化combobox1



            _manualEnabled = true;//手动使能开启
            button17.BackColor = Color.DarkSalmon;

        }
    }
}
