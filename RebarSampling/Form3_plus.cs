using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace RebarSampling
{
    /******
     * 本类为form3的分支，处理鼠标拖动实现套料结果手动组合、变更的功能
     *  
     * 
     * ******/
    public partial class Form3 : Form
    {
        /// <summary>
        /// 起点rebarOri
        /// </summary>
        private List<KeyValuePair<int, RebarOri>> _sourceOri=new List<KeyValuePair<int, RebarOri>>();
        /// <summary>
        /// 目标rebarOri
        /// </summary>
        private KeyValuePair<int, RebarOri> _targetOri=new KeyValuePair<int, RebarOri>();
        private void dataGridView12_MouseDown(object sender, MouseEventArgs e)
        {
            // 获取鼠标在DataGridView中的位置（行和列索引）  
            int rowIndex = dataGridView12.HitTest(e.X, e.Y).RowIndex;
            int colIndex = dataGridView12.HitTest(e.X, e.Y).ColumnIndex;

            // 检查是否确实点击在了一个单元格上  
            if (rowIndex >= 0 && colIndex >= 0)//确保鼠标点击在图片列
            {
                Rectangle rectCurCell = dataGridView12.GetCellDisplayRectangle(colIndex, rowIndex, true);//获取当前单元格相对dgv控件的坐标

                //Point _leftTop = new Point(dataGridView12.Rows[rowIndex].Cells[colIndex].ContentBounds.Left + rectCurCell.Left,
                //    dataGridView12.Rows[rowIndex].Cells[colIndex].ContentBounds.Top + rectCurCell.Top);

                Rectangle rectPic = new Rectangle(rectCurCell.Location, GeneralClass.taoPicSize);//取单元格的左上角坐标和套料显示图片的尺寸

                if (rectPic.Contains(e.X, e.Y))//鼠标位置是否在图片区域内
                {
                    Point _p = new Point(e.X - rectCurCell.X, e.Y - rectCurCell.Y);//建立一个相对坐标点，为鼠标位置相对图片左上角的坐标

                    RebarOri _rebarOri = _SelectedRebarOriList[rowIndex];
                    Modifylist(_p, ref _rebarOri, true);

                    dataGridView12.Rows[rowIndex].Cells[colIndex].Value = graphics.PaintRebar(_rebarOri);

                    _sourceOri?.Clear();//清空
                    _sourceOri.Add(new KeyValuePair<int, RebarOri>(rowIndex, _rebarOri));//将本rebarOri存入source，等待拖拽
                }
            }
        }
        private void dataGridView12_MouseUp(object sender, MouseEventArgs e)
        {
            // 获取鼠标在DataGridView中的位置（行和列索引）  
            int rowIndex = dataGridView12.HitTest(e.X, e.Y).RowIndex;
            int colIndex = dataGridView12.HitTest(e.X, e.Y).ColumnIndex;

            bool _haveSelect = false;
            // 检查是否确实点击在了一个单元格上  
            if (rowIndex >= 0 && colIndex >= 0)//确保鼠标点击在图片列
            {
                foreach (var item in _SelectedRebarOriList[rowIndex]._list)
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
                    Rectangle rectCurCell = dataGridView12.GetCellDisplayRectangle(colIndex, rowIndex, true);//获取当前单元格相对dgv控件的坐标

                    Rectangle rectPic = new Rectangle(rectCurCell.Location, GeneralClass.taoPicSize);//取单元格的左上角坐标和套料显示图片的尺寸

                    if (rectPic.Contains(e.X, e.Y))//鼠标位置是否在图片区域内
                    {
                        Point _p = new Point(e.X - rectCurCell.X, e.Y - rectCurCell.Y);//建立一个相对坐标点，为鼠标位置相对图片左上角的坐标

                        RebarOri _rebarOri = _SelectedRebarOriList[rowIndex];
                        Modifylist(_p, ref _rebarOri, false);//修改list元素的pick状态
                        dataGridView12.Rows[rowIndex].Cells[colIndex].Value = graphics.PaintRebar(_rebarOri);//重绘套料显示图
                    }
                    _sourceOri?.Clear();//如果只是取消选中，则清空待拖拽list

                }
                else
                {
                    //没有选中的,执行拖拽，sourceOri的list去除选中Ori，targetOri的list增加选中Ori
                    _targetOri = new KeyValuePair<int, RebarOri>(rowIndex, _SelectedRebarOriList[rowIndex]);//目标位置rebarOri
                    if(_sourceOri!=null&&_sourceOri.Count!=0)
                    {
                        int _sourceIndex = _sourceOri[0].Key;
                        RebarOri temp = _sourceOri[0].Value;

                        for(int i= _SelectedRebarOriList[_sourceIndex]._list.Count-1;i>=0;i--)
                        {
                            if (_SelectedRebarOriList[_sourceIndex]._list[i].PickUsed)
                            {
                                _SelectedRebarOriList[rowIndex]._list.Add(_SelectedRebarOriList[_sourceIndex]._list[i]);//targetOri的list增加
                                _SelectedRebarOriList[_sourceIndex]._list.RemoveAt(i);//sourceOri的list去除
                                foreach(var tt in _SelectedRebarOriList[rowIndex]._list)
                                {
                                    tt.PickUsed = false;//复位pick状态
                                }
                                break;
                            }
                        }

                        dataGridView12.Rows[_sourceIndex].Cells[colIndex].Value = graphics.PaintRebar(_SelectedRebarOriList[_sourceIndex]);//重绘sourceOri

                        dataGridView12.Rows[rowIndex].Cells[colIndex].Value = graphics.PaintRebar(_SelectedRebarOriList[rowIndex]);//重绘targetOri



                    }
                }

            }
        }

        /// <summary>
        /// 根据鼠标位置，修改rebarOri的list中的pickUsed状态，为后面PaintRebarOri做准备
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
                    _endlist.Add((int)((double)(_lengAdd) / (double)GeneralClass.OriginalLength(item.Level, item.Diameter) * 600));
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

    }
}
