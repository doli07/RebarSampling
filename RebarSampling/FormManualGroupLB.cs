using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RebarSampling
{
    public partial class FormManualGroupLB : Form
    {
        public FormManualGroupLB()
        {
            InitializeComponent();

            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            dataGridView5.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView6.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView7.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView8.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            GeneralClass.interactivityData.getTaosiSetting += GetTaosiSetting;

        }
        /// <summary>
        /// 拖拽中的list
        /// </summary>
        private List<ElementRebarFB> _flyingElement = new List<ElementRebarFB>();
        /// <summary>
        /// 套丝机设置
        /// </summary>
        private string _taosiSetting = "";
        /// <summary>
        /// 构件批的list
        /// </summary>
        //private List<ElementDataBatch> _ElementBatchList = new List<ElementDataBatch>();
        private List<ElementBatch> _ElementBatchList = new List<ElementBatch>();

        /// <summary>
        /// 手动排程sourcelist
        /// </summary>
        private List<ElementRebarFB> _ElementSourceList = new List<ElementRebarFB>();
        //private List<ElementData> _ElementSourceList = new List<ElementData>();

        ///// <summary>
        ///// 手动排程targetlist
        ///// </summary>
        //private List<ElementData> _ElementTargetList = new List<ElementData>();
        private List<ElementRebarFB> _ElementTargetList_1 = new List<ElementRebarFB>();//1#料仓
        private List<ElementRebarFB> _ElementTargetList_2 = new List<ElementRebarFB>();//2#料仓
        private List<ElementRebarFB> _ElementTargetList_3 = new List<ElementRebarFB>();//3#料仓
        private List<ElementRebarFB> _ElementTargetList_4 = new List<ElementRebarFB>();//4#料仓

        //private List<ElementData> _ElementTargetList_1 = new List<ElementData>();//1#料仓
        //private List<ElementData> _ElementTargetList_2 = new List<ElementData>();//2#料仓
        //private List<ElementData> _ElementTargetList_3 = new List<ElementData>();//3#料仓
        //private List<ElementData> _ElementTargetList_4 = new List<ElementData>();//4#料仓

        /// <summary>
        /// 从form3界面获取料单选择状态，依次为：棒材、线材、箍筋、拉钩、马凳、主筋
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        private List<RebarData> Pick(List<RebarData> _data)
        {
            List<RebarData> _ret = new List<RebarData>();

            Tuple<bool, bool, bool, bool, bool, bool,bool> _sts = new Tuple<bool, bool, bool, bool, bool, bool,bool>(false, false, false, false, false, false,false);//init
            GeneralClass.interactivityData?.askForPickStatus(out _sts);//从form3界面获取料单选择状态，依次为：棒材、线材、箍筋、拉钩、马凳、端头、主筋

            //筛选棒材、线材，筛选箍筋、拉钩、马凳、主筋
            _ret = _data.Where(t =>
                   ((_sts.Item1 ? (t.RebarSizeType == EnumRebarSizeType.BANG) : false) ||
                   (_sts.Item2 ? (t.RebarSizeType == EnumRebarSizeType.XIAN) : false))
                   &&
                    ((_sts.Item3 ? (t.RebarShapeType == EnumRebarShapeType.SHAPE_GJ) : false) ||
                     (_sts.Item4 ? (t.RebarShapeType == EnumRebarShapeType.SHAPE_LG) : false) ||
                      (_sts.Item5 ? (t.RebarShapeType == EnumRebarShapeType.SHAPE_MD) : false) ||
                      (_sts.Item6 ? (t.RebarShapeType == EnumRebarShapeType.SHAPE_DT) : false) ||
                       (_sts.Item7 ? (t.RebarShapeType == EnumRebarShapeType.SHAPE_ZJ) : false))
                ).ToList();

            return _ret;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            GeneralClass.interactivityData?.printlog(1, "开始对所有构件包进行手动分组排程");

            GeneralClass.AllElementList = GeneralClass.DBOpt.GetAllElementList(GeneralClass.TableName_AllRebar);
            if (GeneralClass.AllElementList.Count == 0) { GeneralClass.interactivityData?.printlog(1, "构件包列表为空"); return; }

            _ElementSourceList.Clear();

            ElementData elementAfterPick;//经过条件筛选后的构件

            List<RebarData> temp = new List<RebarData>();
            foreach (var item in GeneralClass.AllElementList)
            {
                elementAfterPick = new ElementData();
                elementAfterPick.Copy(item);

                temp = Pick(item.rebarlist);
                elementAfterPick.rebarlist.Clear();
                foreach (var ttt in temp)//复制筛选后的rebarlist
                {
                    elementAfterPick.rebarlist.Add(ttt);
                }

                _ElementSourceList.Add(elementAfterPick.elementDataFB);
            }




            FillDGV_Elements_source_show(_ElementSourceList, ref dataGridView1);

            _ElementBatchList.Clear();
            FillDGV_batch_show(_ElementBatchList, ref dataGridView2);

            ClearAllElementTarget();

            button1.BackColor = Color.DarkSalmon;
            tabControl1.SelectedIndex = 0;
        }

        private void FillDGV_Elements_source_show(List<ElementRebarFB> _elements, ref DataGridView _dgv)
        {
            DataTable dt_z = new DataTable();
            dt_z.Columns.Add("构件编号", typeof(string));
            dt_z.Columns.Add("总数量", typeof(string));
            //dt_z.Columns.Add("直径种类", typeof(int));
            //dt_z.Columns.Add("直径组合(Φ)", typeof(string));
            //dt_z.Columns.Add("总重量(kg)", typeof(double));

            dt_z.Columns.Add("Φ12", typeof(string));
            dt_z.Columns.Add("Φ14", typeof(string));
            dt_z.Columns.Add("Φ16", typeof(string));
            dt_z.Columns.Add("Φ18", typeof(string));
            dt_z.Columns.Add("Φ20", typeof(string));
            dt_z.Columns.Add("Φ22", typeof(string));
            dt_z.Columns.Add("Φ25", typeof(string));
            dt_z.Columns.Add("Φ28", typeof(string));
            dt_z.Columns.Add("Φ32", typeof(string));

            dt_z.Columns.Add("项目名称", typeof(string));
            dt_z.Columns.Add("主构件名", typeof(string));
            dt_z.Columns.Add("构件名", typeof(string));

            int[] numSum = new int[(int)EnumDiaBang.maxRebarBangNum];//分直径的总计
            int[] numSumTao = new int[(int)EnumDiaBang.maxRebarBangNum];//分直径的总计
            int[] numSumTaoF = new int[(int)EnumDiaBang.maxRebarBangNum];//分直径的总计

            int totalSum = 0;//总计
            int totalSumTao = 0;
            int totalSumTaoF = 0;

            foreach (var item in _elements)
            {
                //List<RebarData> _list = SettingCheck(item);
                List<RebarData> _list = item.rebarlist_bc;
                List<RebarData> _taolist = item.rebarlist_bc_tao;
                List<RebarData> _taoFlist = item.rebarlist_bc_tao_fan;


                int[] num = new int[(int)EnumDiaBang.maxRebarBangNum];//各个直径的棒材总数量
                int[] numTao = new int[(int)EnumDiaBang.maxRebarBangNum];//各个直径的棒材套丝总数量
                int[] numTaoF = new int[(int)EnumDiaBang.maxRebarBangNum];//各个直径的棒材套丝反丝总数量


                for (EnumDiaBang i = EnumDiaBang.BANG_C12; i < EnumDiaBang.maxRebarBangNum; i++)//根据不同直径进行统计
                {
                    num[(int)i] = _list.Where(t => t.Diameter == Convert.ToInt32(i.ToString().Substring(6, 2))).Sum(t => t.TotalPieceNum);
                    numTao[(int)i] = _taolist.Where(t => t.Diameter == Convert.ToInt32(i.ToString().Substring(6, 2))).Sum(t => t.TotalPieceNum);
                    numTaoF[(int)i] = _taoFlist.Where(t => t.Diameter == Convert.ToInt32(i.ToString().Substring(6, 2))).Sum(t => t.TotalPieceNum);

                    numSum[(int)i] += num[(int)i];
                    numSumTao[(int)i] += numTao[(int)i];
                    numSumTaoF[(int)i] += numTaoF[(int)i];
                }

                int sum_bc = _list.Sum(t => t.TotalPieceNum);//棒材总数量
                int sum_bc_tao = _taolist.Sum(t => t.TotalPieceNum);//棒材套丝总数量
                int sum_bc_taoF = _taoFlist.Sum(f => f.TotalPieceNum);//棒材套丝反丝总数量

                totalSum += sum_bc;//总计
                totalSumTao += sum_bc_tao;
                totalSumTaoF += sum_bc_taoF;

                dt_z.Rows.Add("A-" + item.elementIndex.ToString(), sum_bc.ToString() + (sum_bc_tao == 0 ? "" : ("(" + sum_bc_tao + ")")) + (sum_bc_taoF == 0 ? "" : ("(" + sum_bc_taoF + "*)")),
                    ((num[0] == 0) ? "" : num[0].ToString()) + ((numTao[0] == 0) ? "" : ("(" + numTao[0].ToString() + ")")) + ((numTaoF[0] == 0) ? "" : ("(" + numTaoF[0].ToString() + "*)")),
                    ((num[1] == 0) ? "" : num[1].ToString()) + ((numTao[1] == 0) ? "" : ("(" + numTao[1].ToString() + ")")) + ((numTaoF[1] == 0) ? "" : ("(" + numTaoF[1].ToString() + "*)")),
                    ((num[2] == 0) ? "" : num[2].ToString()) + ((numTao[2] == 0) ? "" : ("(" + numTao[2].ToString() + ")")) + ((numTaoF[2] == 0) ? "" : ("(" + numTaoF[2].ToString() + "*)")),
                    ((num[3] == 0) ? "" : num[3].ToString()) + ((numTao[3] == 0) ? "" : ("(" + numTao[3].ToString() + ")")) + ((numTaoF[3] == 0) ? "" : ("(" + numTaoF[3].ToString() + "*)")),
                    ((num[4] == 0) ? "" : num[4].ToString()) + ((numTao[4] == 0) ? "" : ("(" + numTao[4].ToString() + ")")) + ((numTaoF[4] == 0) ? "" : ("(" + numTaoF[4].ToString() + "*)")),
                    ((num[5] == 0) ? "" : num[5].ToString()) + ((numTao[5] == 0) ? "" : ("(" + numTao[5].ToString() + ")")) + ((numTaoF[5] == 0) ? "" : ("(" + numTaoF[5].ToString() + "*)")),
                    ((num[6] == 0) ? "" : num[6].ToString()) + ((numTao[6] == 0) ? "" : ("(" + numTao[6].ToString() + ")")) + ((numTaoF[6] == 0) ? "" : ("(" + numTaoF[6].ToString() + "*)")),
                    ((num[7] == 0) ? "" : num[7].ToString()) + ((numTao[7] == 0) ? "" : ("(" + numTao[7].ToString() + ")")) + ((numTaoF[7] == 0) ? "" : ("(" + numTaoF[7].ToString() + "*)")),
                    ((num[8] == 0) ? "" : num[8].ToString()) + ((numTao[8] == 0) ? "" : ("(" + numTao[8].ToString() + ")")) + ((numTaoF[8] == 0) ? "" : ("(" + numTaoF[8].ToString() + "*)")),
                    item.projectName, item.assemblyName, item.elementName);
            }

            //再加一个总计
            dt_z.Rows.Add("总计", totalSum + (totalSumTao == 0 ? "" : ("(" + totalSumTao + ")")) + (totalSumTaoF == 0 ? "" : ("(" + totalSumTaoF + "*)")),
                    ((numSum[0] == 0) ? "" : numSum[0].ToString()) + ((numSumTao[0] == 0) ? "" : ("(" + numSumTao[0].ToString() + ")")) + ((numSumTaoF[0] == 0) ? "" : ("(" + numSumTaoF[0].ToString() + "*)")),
                    ((numSum[1] == 0) ? "" : numSum[1].ToString()) + ((numSumTao[1] == 0) ? "" : ("(" + numSumTao[1].ToString() + ")")) + ((numSumTaoF[1] == 0) ? "" : ("(" + numSumTaoF[1].ToString() + "*)")),
                    ((numSum[2] == 0) ? "" : numSum[2].ToString()) + ((numSumTao[2] == 0) ? "" : ("(" + numSumTao[2].ToString() + ")")) + ((numSumTaoF[2] == 0) ? "" : ("(" + numSumTaoF[2].ToString() + "*)")),
                    ((numSum[3] == 0) ? "" : numSum[3].ToString()) + ((numSumTao[3] == 0) ? "" : ("(" + numSumTao[3].ToString() + ")")) + ((numSumTaoF[3] == 0) ? "" : ("(" + numSumTaoF[3].ToString() + "*)")),
                    ((numSum[4] == 0) ? "" : numSum[4].ToString()) + ((numSumTao[4] == 0) ? "" : ("(" + numSumTao[4].ToString() + ")")) + ((numSumTaoF[4] == 0) ? "" : ("(" + numSumTaoF[4].ToString() + "*)")),
                    ((numSum[5] == 0) ? "" : numSum[5].ToString()) + ((numSumTao[5] == 0) ? "" : ("(" + numSumTao[5].ToString() + ")")) + ((numSumTaoF[5] == 0) ? "" : ("(" + numSumTaoF[5].ToString() + "*)")),
                    ((numSum[6] == 0) ? "" : numSum[6].ToString()) + ((numSumTao[6] == 0) ? "" : ("(" + numSumTao[6].ToString() + ")")) + ((numSumTaoF[6] == 0) ? "" : ("(" + numSumTaoF[6].ToString() + "*)")),
                    ((numSum[7] == 0) ? "" : numSum[7].ToString()) + ((numSumTao[7] == 0) ? "" : ("(" + numSumTao[7].ToString() + ")")) + ((numSumTaoF[7] == 0) ? "" : ("(" + numSumTaoF[7].ToString() + "*)")),
                    ((numSum[8] == 0) ? "" : numSum[8].ToString()) + ((numSumTao[8] == 0) ? "" : ("(" + numSumTao[8].ToString() + ")")) + ((numSumTaoF[8] == 0) ? "" : ("(" + numSumTaoF[8].ToString() + "*)")),
                    "", "", "");
            _dgv.DataSource = dt_z;
            //_dgv.Columns[7].DefaultCellStyle.Format = "0.00";          //

            //对所有的数量cell进行判别，包含（）的为需要套丝，标注蓝色；包含*的为反丝，标注红色
            if (dt_z.Rows.Count > 1)
            {
                for (int i = 0; i < _dgv.RowCount - 1; i++)//所有行
                {
                    for (int j = 1; j < _dgv.ColumnCount - 3; j++)//只有数量列
                    {
                        string _cell = (string)_dgv.Rows[i].Cells[j].Value;
                        if (_cell.IndexOf('(') > -1)
                        {
                            _dgv.Rows[i].Cells[j].Style.BackColor = (_cell.IndexOf('*') > -1) ? Color.Red : Color.LightGreen;//如果含有“*”则为反丝，不包含则标注绿色
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 清空四个手动分组target表
        /// </summary>
        private void ClearAllElementTarget()
        {
            _ElementTargetList_1.Clear();
            FillDGV_Elements_target_show(_ElementTargetList_1, ref dataGridView5);
            _ElementTargetList_2.Clear();
            FillDGV_Elements_target_show(_ElementTargetList_2, ref dataGridView6);
            _ElementTargetList_3.Clear();
            FillDGV_Elements_target_show(_ElementTargetList_3, ref dataGridView7);
            _ElementTargetList_4.Clear();
            FillDGV_Elements_target_show(_ElementTargetList_4, ref dataGridView8);

        }
        private void FillDGV_Elements_target_show(List<ElementRebarFB> _elements, ref DataGridView _dgv)
        {
            DataTable dt_z = new DataTable();
            dt_z.Columns.Add("构件", typeof(string));
            dt_z.Columns.Add("数量", typeof(int));

            dt_z.Columns.Add("仓位设置", typeof(int));
            dt_z.Columns.Add("仓位序号", typeof(int));

            dt_z.Columns.Add("项目名称", typeof(string));
            dt_z.Columns.Add("主构件名", typeof(string));
            dt_z.Columns.Add("构件名", typeof(string));

            foreach (var item in _elements)
            {
                //int _wareSet = Convert.ToInt32(item.wareNumSet.ToString().Substring(8, 1));
                int _wareSet = GeneralClass.EnumWareSetToInt(item.wareNumSet);

                dt_z.Rows.Add("A-" + item.elementIndex.ToString(),
                    item.totalNum,
                   _wareSet,
                   0,

                    item.projectName, item.assemblyName, item.elementName);
            }

            _dgv.DataSource = dt_z;
            //_dgv.Columns[7].DefaultCellStyle.Format = "0.00";          //

        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                // 获取鼠标在DataGridView中的位置（行和列索引）  
                int rowIndex = dataGridView1.HitTest(e.X, e.Y).RowIndex;
                int colIndex = dataGridView1.HitTest(e.X, e.Y).ColumnIndex;

                DataTable dt = dataGridView1.DataSource as DataTable;

                // 检查是否确实点击在了一个单元格上  
                if (rowIndex >= 0 && colIndex >= 0)//
                {
                    int _index = Convert.ToInt32(dt.Rows[rowIndex][0].ToString().Substring(2));

                    _flyingElement.Clear();
                    var temp = _ElementSourceList.Where(t => t.elementIndex == _index).ToList();

                    _flyingElement.AddRange(temp);
                }
            }
            catch (Exception ex) { MessageBox.Show("dataGridView1_MouseDown error:" + ex.Message); }

        }


        private void RefreshDGV()
        {
            int m_curIndex = dataGridView1.FirstDisplayedScrollingRowIndex;//保持当前滚动显示的第一行不变
            //int l_curIndex = (int)(dataGridView13.CurrentRow?.Index);//保持选中行不变
            FillDGV_Elements_source_show(_ElementSourceList, ref dataGridView1);
            //dataGridView13.Rows[l_curIndex].Selected = true;
            dataGridView1.FirstDisplayedScrollingRowIndex = m_curIndex;

            FillDGV_Elements_target_show(_ElementTargetList_1, ref dataGridView5);
            FillDGV_Elements_target_show(_ElementTargetList_2, ref dataGridView6);
            FillDGV_Elements_target_show(_ElementTargetList_3, ref dataGridView7);
            FillDGV_Elements_target_show(_ElementTargetList_4, ref dataGridView8);

            FillDGV_Elements_Realtime();
        }



        private void dataGridView5_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    // 获取鼠标在DataGridView中的位置（行和列索引）  
                    int rowIndex = dataGridView5.HitTest(e.X, e.Y).RowIndex;
                    int colIndex = dataGridView5.HitTest(e.X, e.Y).ColumnIndex;

                    DataTable dt = dataGridView5.DataSource as DataTable;

                    // 检查是否确实点击在了一个单元格上  
                    if (rowIndex >= 0 && colIndex >= 0 && rowIndex < _ElementTargetList_1.Count)//确保鼠标点击在有效行
                    {
                        int _index = Convert.ToInt32(dt.Rows[rowIndex][0].ToString().Substring(2));//找到选中行的index
                        var temp = _ElementTargetList_1.Find(t => t.elementIndex == _index);

                        _ElementSourceList.Add(temp);
                        _ElementTargetList_1.Remove(temp);

                        RefreshDGV();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("dataGridView5_MouseDown error:" + ex.Message); }

        }

        private void dataGridView5_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (_flyingElement == null || _flyingElement.Count == 0) return;

                //如果数量分组不匹配，则不允许放一起
                if (_ElementTargetList_1.Count != 0)
                {
                    if (_flyingElement[0].wareNumSet > _ElementTargetList_1[0].wareNumSet)//大仓可以放小包，小仓不能放大包，所以此处用“>”
                    {
                        MessageBox.Show("仓位设置不一致，手动分组失败！");
                        return;
                    }

                    int _num = Convert.ToInt32(_ElementTargetList_1[0].wareNumSet.ToString().Substring(8, 1));//取仓位设置enum的数字部分
                    if ((_ElementTargetList_1.Count + _flyingElement.Count) > _num)
                    {
                        MessageBox.Show("构件包数量太多，手动分组失败！");
                        return;
                    }
                }

                _ElementTargetList_1.AddRange(_flyingElement);
                foreach (var item in _flyingElement)
                {
                    _ElementSourceList.Remove(item);
                }
                RefreshDGV();
                _flyingElement.Clear();//拖动完成清空

            }
            catch (Exception ex) { MessageBox.Show("dataGridView5_MouseUp error:" + ex.Message); }

        }

        private void dataGridView6_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    // 获取鼠标在DataGridView中的位置（行和列索引）  
                    int rowIndex = dataGridView6.HitTest(e.X, e.Y).RowIndex;
                    int colIndex = dataGridView6.HitTest(e.X, e.Y).ColumnIndex;

                    DataTable dt = dataGridView6.DataSource as DataTable;

                    // 检查是否确实点击在了一个单元格上  
                    if (rowIndex >= 0 && colIndex >= 0 && rowIndex < _ElementTargetList_2.Count)//确保鼠标点击在有效行
                    {
                        int _index = Convert.ToInt32(dt.Rows[rowIndex][0].ToString().Substring(2));//找到选中行的index
                        var temp = _ElementTargetList_2.Find(t => t.elementIndex == _index);

                        _ElementSourceList.Add(temp);
                        _ElementTargetList_2.Remove(temp);

                        RefreshDGV();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("dataGridView6_MouseDown error:" + ex.Message); }

        }

        private void dataGridView6_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (_flyingElement == null || _flyingElement.Count == 0) return;

                //如果数量分组不匹配，则不允许放一起
                if (_ElementTargetList_2.Count != 0)
                {
                    if (_flyingElement[0].wareNumSet > _ElementTargetList_2[0].wareNumSet)//大仓可以放小包，小仓不能放大包，所以此处用“>”
                    {
                        MessageBox.Show("仓位设置不一致，手动分组失败！");
                        return;
                    }

                    int _num = Convert.ToInt32(_ElementTargetList_2[0].wareNumSet.ToString().Substring(8, 1));//取仓位设置enum的数字部分
                    if ((_ElementTargetList_2.Count + _flyingElement.Count) > _num)
                    {
                        MessageBox.Show("构件包数量太多，手动分组失败！");
                        return;
                    }
                }

                _ElementTargetList_2.AddRange(_flyingElement);
                foreach (var item in _flyingElement)
                {
                    _ElementSourceList.Remove(item);
                }
                RefreshDGV();
                _flyingElement.Clear();//拖动完成清空

            }
            catch (Exception ex) { MessageBox.Show("dataGridView6_MouseUp error:" + ex.Message); }

        }

        private void dataGridView7_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    // 获取鼠标在DataGridView中的位置（行和列索引）  
                    int rowIndex = dataGridView7.HitTest(e.X, e.Y).RowIndex;
                    int colIndex = dataGridView7.HitTest(e.X, e.Y).ColumnIndex;

                    DataTable dt = dataGridView7.DataSource as DataTable;

                    // 检查是否确实点击在了一个单元格上  
                    if (rowIndex >= 0 && colIndex >= 0 && rowIndex < _ElementTargetList_3.Count)//确保鼠标点击在有效行
                    {
                        int _index = Convert.ToInt32(dt.Rows[rowIndex][0].ToString().Substring(2));//找到选中行的index
                        var temp = _ElementTargetList_3.Find(t => t.elementIndex == _index);

                        _ElementSourceList.Add(temp);
                        _ElementTargetList_3.Remove(temp);

                        RefreshDGV();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("dataGridView7_MouseDown error:" + ex.Message); }

        }

        private void dataGridView7_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (_flyingElement == null || _flyingElement.Count == 0) return;

                //如果数量分组不匹配，则不允许放一起
                if (_ElementTargetList_3.Count != 0)
                {
                    if (_flyingElement[0].wareNumSet > _ElementTargetList_3[0].wareNumSet)//大仓可以放小包，小仓不能放大包，所以此处用“>”
                    {
                        MessageBox.Show("仓位设置不一致，手动分组失败！");
                        return;
                    }

                    int _num = Convert.ToInt32(_ElementTargetList_3[0].wareNumSet.ToString().Substring(8, 1));//取仓位设置enum的数字部分
                    if ((_ElementTargetList_3.Count + _flyingElement.Count) > _num)
                    {
                        MessageBox.Show("构件包数量太多，手动分组失败！");
                        return;
                    }
                }

                _ElementTargetList_3.AddRange(_flyingElement);
                foreach (var item in _flyingElement)
                {
                    _ElementSourceList.Remove(item);
                }
                RefreshDGV();
                _flyingElement.Clear();//拖动完成清空

            }
            catch (Exception ex) { MessageBox.Show("dataGridView7_MouseUp error:" + ex.Message); }

        }

        private void dataGridView8_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    // 获取鼠标在DataGridView中的位置（行和列索引）  
                    int rowIndex = dataGridView8.HitTest(e.X, e.Y).RowIndex;
                    int colIndex = dataGridView8.HitTest(e.X, e.Y).ColumnIndex;

                    DataTable dt = dataGridView8.DataSource as DataTable;

                    // 检查是否确实点击在了一个单元格上  
                    if (rowIndex >= 0 && colIndex >= 0 && rowIndex < _ElementTargetList_4.Count)//确保鼠标点击在有效行
                    {
                        int _index = Convert.ToInt32(dt.Rows[rowIndex][0].ToString().Substring(2));//找到选中行的index
                        var temp = _ElementTargetList_4.Find(t => t.elementIndex == _index);

                        _ElementSourceList.Add(temp);
                        _ElementTargetList_4.Remove(temp);

                        RefreshDGV();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("dataGridView8_MouseDown error:" + ex.Message); }

        }

        private void dataGridView8_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (_flyingElement == null || _flyingElement.Count == 0) return;

                //如果数量分组不匹配，则不允许放一起
                if (_ElementTargetList_4.Count != 0)
                {
                    if (_flyingElement[0].wareNumSet > _ElementTargetList_4[0].wareNumSet)//大仓可以放小包，小仓不能放大包，所以此处用“>”
                    {
                        MessageBox.Show("仓位设置不一致，手动分组失败！");
                        return;
                    }

                    int _num = Convert.ToInt32(_ElementTargetList_4[0].wareNumSet.ToString().Substring(8, 1));//取仓位设置enum的数字部分
                    if ((_ElementTargetList_4.Count + _flyingElement.Count) > _num)
                    {
                        MessageBox.Show("构件包数量太多，手动分组失败！");
                        return;
                    }
                }

                _ElementTargetList_4.AddRange(_flyingElement);
                foreach (var item in _flyingElement)
                {
                    _ElementSourceList.Remove(item);
                }
                RefreshDGV();
                _flyingElement.Clear();//拖动完成清空
            }
            catch (Exception ex) { MessageBox.Show("dataGridView8_MouseUp error:" + ex.Message); }

        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //自动编号，与数据无关
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
               e.RowBounds.Location.Y,
               dataGridView1.RowHeadersWidth - 4,
               e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics,
                  (e.RowIndex + 1).ToString(),
                   dataGridView1.RowHeadersDefaultCellStyle.Font,
                   rectangle,
                   dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                   TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void dataGridView5_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //自动编号，与数据无关
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
               e.RowBounds.Location.Y,
               dataGridView5.RowHeadersWidth - 4,
               e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics,
                  (e.RowIndex + 1).ToString(),
                   dataGridView5.RowHeadersDefaultCellStyle.Font,
                   rectangle,
                   dataGridView5.RowHeadersDefaultCellStyle.ForeColor,
                   TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void dataGridView6_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //自动编号，与数据无关
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
               e.RowBounds.Location.Y,
               dataGridView6.RowHeadersWidth - 4,
               e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics,
                  (e.RowIndex + 1).ToString(),
                   dataGridView6.RowHeadersDefaultCellStyle.Font,
                   rectangle,
                   dataGridView6.RowHeadersDefaultCellStyle.ForeColor,
                   TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void dataGridView7_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //自动编号，与数据无关
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
               e.RowBounds.Location.Y,
               dataGridView7.RowHeadersWidth - 4,
               e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics,
                  (e.RowIndex + 1).ToString(),
                   dataGridView7.RowHeadersDefaultCellStyle.Font,
                   rectangle,
                   dataGridView7.RowHeadersDefaultCellStyle.ForeColor,
                   TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void dataGridView8_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //自动编号，与数据无关
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
               e.RowBounds.Location.Y,
               dataGridView8.RowHeadersWidth - 4,
               e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics,
                  (e.RowIndex + 1).ToString(),
                   dataGridView8.RowHeadersDefaultCellStyle.Font,
                   rectangle,
                   dataGridView8.RowHeadersDefaultCellStyle.ForeColor,
                   TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        /// <summary>
        /// 尚未形成加工批之前，构件组合的实时数据显示，ifclear如果为true，则清空
        /// </summary>
        private void FillDGV_Elements_Realtime(bool ifclear = false)
        {
            if (ifclear)//如果清除
            {
                dataGridView3.DataSource = new DataTable();
                dataGridView4.DataSource = new DataTable();
                return;
            }
            List<ElementRebarFB> _elements = new List<ElementRebarFB>();

            _elements.AddRange(_ElementTargetList_1);
            _elements.AddRange(_ElementTargetList_2);
            _elements.AddRange(_ElementTargetList_3);
            _elements.AddRange(_ElementTargetList_4);

            List<RebarData> _list = new List<RebarData>();
            List<RebarData> _taolist = new List<RebarData>();
            List<RebarData> _taoFlist = new List<RebarData>();

            foreach (var item in _elements)
            {
                _list.AddRange(item.rebarlist_bc);
                _taolist.AddRange(item.rebarlist_bc_tao_zheng);
                _taoFlist.AddRange(item.rebarlist_bc_tao_fan);
            }

            //直径(Φ)
            DataTable dt_z = new DataTable();
            dt_z.Columns.Add("直径", typeof(string));
            dt_z.Columns.Add("数量", typeof(int));

            var temp_1 = _list.GroupBy(t => t.Diameter).Select(
                y => new
                {
                    _dia = y.Key,
                    _num = y.Sum(t => t.TotalPieceNum),
                }).ToList();

            foreach (var item in temp_1)
            {
                dt_z.Rows.Add("Φ" + item._dia, item._num);
            }
            dataGridView3.DataSource = dt_z;
            //dataGridView3.Columns[5].DefaultCellStyle.Format = "0.00";          //




            //套丝直径(Φ)
            DataTable dt_tz = new DataTable();
            dt_tz.Columns.Add("套丝直径", typeof(string));
            dt_tz.Columns.Add("数量", typeof(int));

            var temp_2 = _taolist.GroupBy(t => t.Diameter).Select(
                            y => new
                            {
                                _dia = y.Key,
                                _num = y.Sum(t => t.TotalPieceNum),
                            }).ToList();

            foreach (var item in temp_2)
            {
                dt_tz.Rows.Add("Φ" + item._dia, item._num);//正丝
            }

            var temp_3 = _taoFlist.GroupBy(t => t.Diameter).Select(
                y => new
                {
                    _dia = y.Key,
                    _num = y.Sum(t => t.TotalPieceNum),
                }).ToList();

            foreach (var item in temp_3)
            {
                dt_tz.Rows.Add("Φ" + item._dia + "*", item._num);//反丝
            }
            dataGridView4.DataSource = dt_tz;
            //dataGridView4.Columns[5].DefaultCellStyle.Format = "0.00";          //

        }

        private void FillDGV_batch_show(List<ElementBatch> _batchlist, ref DataGridView _dgv)
        {
            DataTable dt_z = new DataTable();
            dt_z.Columns.Add("生产批号", typeof(string));
            //dt_z.Columns.Add("当前批次", typeof(int));
            dt_z.Columns.Add("直径组合(Φ)", typeof(string));
            dt_z.Columns.Add("套丝直径组合(Φ)", typeof(string));
            dt_z.Columns.Add("套丝机设置(Φ)", typeof(string));

            dt_z.Columns.Add("总数量", typeof(int));
            dt_z.Columns.Add("总重量(kg)", typeof(double));

            foreach (var item in _batchlist)
            {
                //string _batchNo = "A-" + DateTime.Now.ToString("yyyyMMdd") + "-" + item.totalBatch.ToString("D3") + "-" + item.curBatch.ToString("D3");
                dt_z.Rows.Add(/*_batchNo,*/item.BatchSeri, item.diameterStr_bc, item.diameterStr_bc_tao, item.TaosiSetting, item.totalNum_bc, item.totalWeight_bc);
            }

            _dgv.DataSource = dt_z;
            _dgv.Columns[5].DefaultCellStyle.Format = "0.00";          //
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView4.DataSource as DataTable;

            if (dt == null)
            {
                return;
            }
            List<string> list = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                list.Add(dr[0].ToString());
            }

            FormTaosiSetting _form = new FormTaosiSetting(this._taosiSetting, list);//

            if (_form.ShowDialog() == DialogResult.OK)
            {
                label1.Text = this._taosiSetting;
            }
        }
        private void GetTaosiSetting(string taosiSetting)
        {
            this._taosiSetting = taosiSetting;
        }
        /// <summary>
        /// 将已分组好的构件进行打包，创建新的构件生产批
        /// </summary>
        private void AddNewElementBatch()
        {
            //先分配四个料仓的料仓号和仓位号
            foreach (var item in _ElementTargetList_1)
            {
                item.warehouseNo = 1;//料仓号
                item.wareNo = _ElementTargetList_1.IndexOf(item) + 1;//仓位号
            }
            foreach (var item in _ElementTargetList_2)
            {
                item.warehouseNo = 2;//料仓号
                item.wareNo = _ElementTargetList_2.IndexOf(item) + 1;//仓位号
            }
            foreach (var item in _ElementTargetList_3)
            {
                item.warehouseNo = 3;//料仓号
                item.wareNo = _ElementTargetList_3.IndexOf(item) + 1;//仓位号
            }
            foreach (var item in _ElementTargetList_4)
            {
                item.warehouseNo = 4;//料仓号
                item.wareNo = _ElementTargetList_4.IndexOf(item) + 1;//仓位号
            }

            //添加新的构件生产批
            ElementBatch _batch = new ElementBatch();

            _batch.elementData.AddRange(_ElementTargetList_1);
            _batch.elementData.AddRange(_ElementTargetList_2);
            _batch.elementData.AddRange(_ElementTargetList_3);
            _batch.elementData.AddRange(_ElementTargetList_4);
            _batch.TaosiSetting = _taosiSetting;//添加套丝设置

            _ElementBatchList.Add(_batch);

            //重新梳理一下构件批列表里面每个生产批的总批次，当前批次，以及序列号等信息
            foreach (var item in _ElementBatchList)
            {
                item.totalBatch = _ElementBatchList.Count;
                item.curBatch = _ElementBatchList.IndexOf(item) + 1;
                item.BatchSeri = "A-" + DateTime.Now.ToString("yyyyMMdd") + "-" + item.totalBatch.ToString("D3") + "-" + item.curBatch.ToString("D3");
            }


        }

        /// <summary>
        /// 从数据库中取出所有构件生产批
        /// </summary>
        private void LoadElementBatchListFromDB()
        {
            try
            {

                GeneralClass.AllElementList = GeneralClass.DBOpt.GetAllElementList(GeneralClass.TableName_AllRebar);//取出所有构件

                DataTable dt = GeneralClass.DBOpt.dbHelper.GetDataTable(GeneralClass.TableName_ElementBatch_LB);


                //先用tuple结果存储db中的数据，依次为【构件批编号】、【构件数据】、【套丝设置】
                List<Tuple<string, ElementRebarFB, string>> templist = new List<Tuple<string, ElementRebarFB, string>>();
                foreach (DataRow row in dt.Rows)
                {
                    string _batchseri = row[1].ToString();
                    int _warehouseNo = Convert.ToInt32(row[2].ToString());
                    int _wareNo = Convert.ToInt32(row[3].ToString());
                    int _wareset = Convert.ToInt32(row[4].ToString());
                    string _tempNo = row[5].ToString();
                    string _taosisetting = row[6].ToString();
                    string _project = row[7].ToString();
                    string _assembly = row[8].ToString();
                    string _element = row[9].ToString();

                    ElementRebarFB temp = GeneralClass.AllElementList.Find(t => t.projectName == _project &&
                                                                                                    t.mainAssemblyName == _assembly &&
                                                                                                    t.elementName == _element).elementDataFB;
                    temp.warehouseNo = _warehouseNo;
                    temp.wareNo = _wareNo;
                    temp.batchSeri = _batchseri;

                    templist.Add(new Tuple<string, ElementRebarFB, string>(_batchseri, temp, _taosisetting));
                }

                var _group = templist.GroupBy(t => t.Item1).ToList();//按照生产批编号来分组

                _ElementBatchList.Clear();

                foreach (var item in _group)
                {
                    ElementBatch _newbatch = new ElementBatch();
                    _newbatch.BatchSeri = item.Key;

                    foreach (var ttt in item.ToList())
                    {
                        _newbatch.elementData.Add(ttt.Item2);
                    }
                    _newbatch.TaosiSetting = item.ToList().First().Item3;//默认套丝设置跟构件批编号是唯一对应的
                    _newbatch.totalBatch = Convert.ToInt32(item.Key.Split('-')[2]);//A-20240806-004-001，取分隔后的第二个元素
                    _newbatch.curBatch = Convert.ToInt32(item.Key.Split('-')[3]);//A-20240806-004-001，取分隔后的第三个元素

                    _ElementBatchList.Add(_newbatch);
                }

                FillDGV_batch_show(_ElementBatchList, ref dataGridView2);//将数据库中提取的elementbatchlist显示出来

            }
            catch (Exception ex) { MessageBox.Show("LoadElementBatchListFromDB error:" + ex.Message); }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (_ElementTargetList_1.Count == 0 && _ElementTargetList_2.Count == 0 && _ElementTargetList_3.Count == 0 && _ElementTargetList_4.Count == 0)//全为空，不可生成加工批
            {
                MessageBox.Show("未做构件包分组，不可建立新的加工批");
                return;
            }


            AddNewElementBatch();//添加新的构件生产批

            FillDGV_batch_show(_ElementBatchList, ref dataGridView2);

            ClearAllElementTarget();//清空
            FillDGV_Elements_Realtime(true);//清空
            this._taosiSetting = "";//清空

            tabControl1.SelectTab(0);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (_ElementBatchList.Count == 0)
            {
                MessageBox.Show("加工批为空，操作无效！");
            }
            else
            {
                //DBOpt.SaveAllElementBatchToDB(_ElementBatchList);//所有构件生产批存入数据库

                GeneralClass.interactivityData?.getManualBatchList(_ElementBatchList);//将batchlist传输给form3
                GeneralClass.interactivityData?.printlog(1, "手动分组完成，已生成所有加工批！");
                MessageBox.Show("手动分组完成，已生成所有加工批！");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //隐藏导航栏
            this.splitContainer1.Panel2Collapsed = !this.splitContainer1.Panel2Collapsed;
            //根据隐藏属性切换项目资源文件中的图片显示
            button5.Image = this.splitContainer1.Panel2Collapsed ? Properties.Resources.icons8_double_up_26 : Properties.Resources.icons8_double_down_26;

        }

        private void button6_Click(object sender, EventArgs e)
        {
            LoadElementBatchListFromDB();
            tabControl1.SelectedIndex = 1;
        }

        private void dataGridView2_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                // 获取鼠标在DataGridView中的位置（行和列索引）  
                int rowIndex = dataGridView2.HitTest(e.X, e.Y).RowIndex;
                int colIndex = dataGridView2.HitTest(e.X, e.Y).ColumnIndex;

                DataTable dt = dataGridView2.DataSource as DataTable;

                // 检查是否确实点击在了一个单元格上  
                if (rowIndex >= 0 && colIndex >= 0)//
                {
                    string _batchseri = dt.Rows[rowIndex][0].ToString();
                    ElementBatch _batch = _ElementBatchList.Find(t => t.BatchSeri.Equals(_batchseri));//根据构件批编号搜索出构件批

                    _ElementTargetList_1.Clear();
                    _ElementTargetList_2.Clear();
                    _ElementTargetList_3.Clear();
                    _ElementTargetList_4.Clear();

                    _ElementTargetList_1.AddRange(_batch.elementData.Where(t => t.warehouseNo == 1).ToList());
                    _ElementTargetList_2.AddRange(_batch.elementData.Where(t => t.warehouseNo == 2).ToList());
                    _ElementTargetList_3.AddRange(_batch.elementData.Where(t => t.warehouseNo == 3).ToList());
                    _ElementTargetList_4.AddRange(_batch.elementData.Where(t => t.warehouseNo == 4).ToList());


                    FillDGV_Elements_target_show(_ElementTargetList_1, ref dataGridView5);
                    FillDGV_Elements_target_show(_ElementTargetList_2, ref dataGridView6);
                    FillDGV_Elements_target_show(_ElementTargetList_3, ref dataGridView7);
                    FillDGV_Elements_target_show(_ElementTargetList_4, ref dataGridView8);

                    FillDGV_Elements_Realtime();
                }
            }
            catch (Exception ex) { MessageBox.Show("dataGridView2_MouseDown error:" + ex.Message); }
        }
    }
}
