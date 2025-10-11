using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NPOI.HSSF.Util.HSSFColor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
//using NPOI.HSSF.UserModel;
//using NPOI.SS.UserModel;
//using NPOI.XSSF.UserModel;

namespace RebarSampling
{
    public partial class FormManualTaoQZ : Form
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
        /// <summary>
        /// 判断dgv1的ctrl是否触发，跟dgv1的鼠标左键点击形成组合键，用于添加多个flyingRebar
        /// </summary>
        private bool isCtrlDown = false;
        /// <summary>
        /// 待复制的rebarOri
        /// </summary>
        private RebarOri _copyRebarOri = null;
        /// <summary>
        /// 是否折叠
        /// </summary>
        private bool _fold = true;
        /// <summary>
        /// 所有直径在完成套料后，形成的完整工单
        /// </summary>
        private RebarOriPiAllDiameter m_allDiameterWorkBill = new RebarOriPiAllDiameter();

        public FormManualTaoQZ()
        {
            InitializeComponent();

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            dataGridView4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            InitRadiobtn();
            InitCheckbox();
        }
        private void InitRadiobtn()
        {
            radioButton1.Checked = true;
            radioButton2.Checked = false;
        }

        private void InitCheckbox()
        {
            checkBox13.Checked = true;
            checkBox14.Checked = true;
            checkBox15.Checked = true;
            checkBox16.Checked = true;
            checkBox17.Checked = true;
            checkBox18.Checked = true;
            checkBox19.Checked = true;

        }
        private List<RebarData> AllRebarListAfterPick = new List<RebarData>();
        /// <summary>
        /// 从form3界面获取料单选择状态，依次为：棒材、线材、箍筋、拉钩、马凳、主筋
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        private List<RebarData> Pick(List<RebarData> _data)
        {
            List<RebarData> _ret = new List<RebarData>();

            Tuple<bool, bool, bool, bool, bool, bool, bool> _sts = new Tuple<bool, bool, bool, bool, bool, bool, bool>(false, false, false, false, false, false, false);//init
            GeneralClass.interactivityData?.askForPickStatus(out _sts);//从form3界面获取料单选择状态，依次为：棒材、线材、箍筋、拉钩、马凳、端头、主筋

            //筛选棒材、线材，筛选箍筋、拉钩、马凳、端头、主筋
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
            try
            {
                GeneralClass.interactivityData?.printlog(1, "开始手动套料，数据准备");

                //Tuple<bool, bool, bool, bool, bool, bool> _sts = new Tuple<bool, bool, bool, bool, bool, bool>(false, false, false, false, false, false);//init
                //GeneralClass.interactivityData?.askForPickStatus(out _sts);//从form3界面获取料单选择状态，依次为：棒材、线材、箍筋、拉钩、马凳、主筋

                GeneralClass.AllRebarList = GeneralClass.DBOpt.GetAllRebarList(GeneralClass.TableName_AllRebar);//取得所有的钢筋数据list

                this.AllRebarListAfterPick = Pick(GeneralClass.AllRebarList);//先筛选棒材/线材，箍筋/拉钩/马凳/端头/主筋

                List<RebarData> _sonData = new List<RebarData>();//
                foreach (RebarData _dd in AllRebarListAfterPick)
                {
                    if (_dd.TotalPieceNum != 0 && CheckDiameter(_dd) && CheckTaoPN(_dd) && CheckIfTao(_dd) && CheckBend(_dd))//筛选直径、套丝、弯曲
                    {
                        _sonData.Add(_dd);
                    }
                }


                _piManualTaoSourceList = Algorithm.ListExpand(_sonData);//20250804



                //长度
                FillDGV_Length(Algorithm.ListExpand(_sonData));

                ////保存至批量锯切数据库
                //SavePiCut(_sonData);


                //pictureBox1.Image = graphics.PaintRuler();//画一把尺子

                FillDGV_ruler();


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void SavePiCut(List<RebarData> _list)
        {
            var _rebarlist = Algorithm.ListExpand(_list);//先展开成rebar

            var _sonalllist = _rebarlist.GroupBy(x => new { x.length, x.CornerMessage }).Select(
                y => new
                {
                    _length = y.Key.length,
                    _cornerMessage = y.Key.CornerMessage,
                    //_totalnum = y.Sum(item => item.TotalPieceNum),
                    //_totalweight = y.Sum(item => item.TotalWeight),
                    _datalist = y.ToList()
                }).OrderByDescending(t => t._length).ToList();//根据长度和边角信息来分组，并按长度降序排列

            List<List<Rebar>> _newlist = new List<List<Rebar>>();//组合成新的list
            foreach (var item in _sonalllist)
            {
                _newlist.Add(item._datalist);
            }

            DBOpt.SaveAllPiCutToDB(true, _newlist);//保存至数据库

        }
        private void FillDGV_ruler()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("长度(mm)", typeof(int));

            for (int i = 1; i <= 20; i++)
            {
                //dt.Rows.Add((int)((double)GeneralClass.OriginalLength / i));//什么狗屁的标尺，不要，删了
            }

            dataGridView3.DataSource = dt;
            //dataGridView3.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dataGridView3.Columns[3].DefaultCellStyle.Format = "P2";
        }
        /// <summary>
        /// 根据勾选项判断rebardata是否被选择
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        private bool CheckDiameter(RebarData _data)
        {
            bool _rt = ((checkBox1.Checked && GeneralClass.m_typeC12) ? (_data.Diameter == 12) : false) ||
                    ((checkBox2.Checked && GeneralClass.m_typeC14) ? (_data.Diameter == 14) : false) ||
                                (checkBox3.Checked ? (_data.Diameter == 16) : false) ||
                                (checkBox4.Checked ? (_data.Diameter == 18) : false) ||
                                (checkBox5.Checked ? (_data.Diameter == 20) : false) ||
                                (checkBox6.Checked ? (_data.Diameter == 22) : false) ||
                                (checkBox7.Checked ? (_data.Diameter == 25) : false) ||
                                (checkBox8.Checked ? (_data.Diameter == 28) : false) ||
                                (checkBox9.Checked ? (_data.Diameter == 32) : false) ||
                                (checkBox10.Checked ? (_data.Diameter == 36) : false) ||
                                (checkBox11.Checked ? (_data.Diameter == 40) : false);

            return _rt;
        }
        private bool CheckTaoPN(RebarData _data)
        {
            bool rt = (checkBox15.Checked ? (_data.TaosiType == 0) : false) ||//不套丝
                 (checkBox18.Checked ? (_data.IfTaoZheng) : false) ||//正丝
                 (checkBox19.Checked ? (_data.IfTaoFan) : false);//反丝

            return rt;
        }
        private bool CheckIfTao(RebarData _data)
        {
            bool rt = (checkBox15.Checked ? (_data.TaosiType == 0) : false) ||//不套丝
               (checkBox16.Checked ? (_data.TaosiType == 1 || _data.TaosiType == 2 || _data.TaosiType == 3 || _data.TaosiType == 4) : false) ||//单头丝
               (checkBox17.Checked ? (_data.TaosiType == 5 || _data.TaosiType == 6 || _data.TaosiType == 7) : false); //双头丝

            return rt;
        }
        /// <summary>
        /// 筛选是否弯曲
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        private bool CheckBend(RebarData _data)
        {
            bool rt = (checkBox13.Checked ? _data.IfBend : false) || (checkBox14.Checked ? (!_data.IfBend) : false);
            return rt;
        }
        /// <summary>
        /// 获取当前checklist选中了哪个直径
        /// </summary>
        /// <returns></returns>
        private EnumDiaBang GetDiameterChecked()
        {
            if (!CheckDiameterSinglePick())
            {
                MessageBox.Show("操作无效，请先确保只选择一种直径规格的钢筋！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return EnumDiaBang.NONE;
            }

            if (checkBox1.Checked) { return EnumDiaBang.BANG_C12; }
            if (checkBox2.Checked) { return EnumDiaBang.BANG_C14; }
            if (checkBox3.Checked) { return EnumDiaBang.BANG_C16; }
            if (checkBox4.Checked) { return EnumDiaBang.BANG_C18; }
            if (checkBox5.Checked) { return EnumDiaBang.BANG_C20; }
            if (checkBox6.Checked) { return EnumDiaBang.BANG_C22; }
            if (checkBox7.Checked) { return EnumDiaBang.BANG_C25; }
            if (checkBox8.Checked) { return EnumDiaBang.BANG_C28; }
            if (checkBox9.Checked) { return EnumDiaBang.BANG_C32; }
            if (checkBox10.Checked) { return EnumDiaBang.BANG_C36; }
            if (checkBox11.Checked) { return EnumDiaBang.BANG_C40; }

            return EnumDiaBang.NONE;
        }
        /// <summary>
        /// 手动批量套料的dgv显示，_fold决定是否折叠
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="_fold"></param>
        private void FillDGV_Pi_Tao_Manual(List<RebarOri> _list, bool _fold = false)
        {

            if (_fold)//折叠
            {
                DataTable dt = new DataTable();

                //dt.Columns.Add("序号", typeof(int));
                dt.Columns.Add("数量", typeof(int));
                dt.Columns.Add("钢筋原材分段", typeof(Image));

                ////注意此处的泛型委托func写法，func<输入类型，输出类型>
                //Func<List<Rebar>, string> msglist = x =>
                //{
                //    string sss = "";
                //    foreach (var ttt in x)
                //    {
                //        sss += ttt.CornerMessage;//拼接cornerMessage
                //    }
                //    return sss;
                //};

                ////此处使用func委托类型创建一个复杂的键选择器，目的是根据rebarOri中所有rebar边角信息的拼接来进行筛选
                //var temp = _list.GroupBy(p => new { cornerMsgkey = msglist(p._list) }).Select(
                //                y => new
                //                {
                //                    cornerMsgList = y.Key,
                //                    num = y.Count(),
                //                    datalist = y.ToList()
                //                }
                //                ).ToList();

                var temp = Algorithm.ExchangeRebarOri(_list);

                foreach (var item in temp)
                {
                    //var str = item.cornerMsgList;
                    //int _index = temp.IndexOf(item);
                    //dt.Rows.Add(_index, item.num, graphics.PaintRebar(item.datalist[0]));
                    dt.Rows.Add(item.num, graphics.PaintRebar(item._list[0]));
                }

                dataGridView2.DataSource = dt;
            }
            else
            {
                DataTable dt = new DataTable();

                //dt.Columns.Add("序号", typeof(int));
                //dt.Columns.Add("数量", typeof(int));
                dt.Columns.Add("钢筋原材分段", typeof(Image));

                foreach (var item in _list)
                {
                    //int _index = _list.IndexOf(item);
                    //dt.Rows.Add(_index, graphics.PaintRebar(item));
                    dt.Rows.Add(graphics.PaintRebar(item));

                }
                dataGridView2.DataSource = dt;
            }

            if (radioButton1.Checked)
            {
                textBox2.Text = _list.Count.ToString();//原材根数
                int _real = _list.Sum(t => t._lengthListUsed);
                int _use = _list.Sum(t => t._totalLength);
                textBox1.Text = ((double)_real / (double)_use).ToString("P1");

            }

        }

        /// <summary>
        /// 棒材的批量锯切统计，主要是按照长度统计
        /// </summary>
        /// <param name="_sonlist"></param>
        private void FillDGV_Length(List<Rebar> _sonlist, List<Rebar> _mumlist = null)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(" ", typeof(bool));
            //dt.Columns.Add("序号", typeof(string));
            dt.Columns.Add("序号", typeof(int));

            dt.Columns.Add("长度(mm)", typeof(int));
            dt.Columns.Add("数量(根)", typeof(int));
            //dt.Columns.Add("数量(%)", typeof(double));
            dt.Columns.Add("重量(kg)", typeof(double));
            //dt.Columns.Add("重量(%)", typeof(double));
            dt.Columns.Add("边角信息", typeof(string));


            var _sonalllist = _sonlist.GroupBy(x => new { x.length, x.iSerialNum, x.CornerMessage }).Select(
                            y => new
                            {
                                _serinum = y.Key.iSerialNum,
                                _length = y.Key.length,
                                _cornerMessage = y.Key.CornerMessage,
                                _totalnum = y.Sum(item => item.TotalPieceNum),
                                _totalweight = y.Sum(item => item.TotalWeight),
                                _datalist = y.ToList()
                            })./*OrderByDescending(t => t._length)*/OrderBy(t => t._serinum).ToList();//按长度降序排列，20250812改为按序号排序
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
                dt.Rows.Add(false, item._serinum, item._length, item._totalnum, /*(double)item._totalnum / total_num,*/
                    item._totalweight, /*item._totalweight / total_weight,*/ item._cornerMessage);
            }

            dataGridView1.DataSource = dt;
            //dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dataGridView1.Columns[3].DefaultCellStyle.Format = "P2";
            dataGridView1.Columns[4].DefaultCellStyle.Format = "0.00";
            //dataGridView1.Columns[5].DefaultCellStyle.Format = "P2";

        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!_manualEnabled) return;

                m_level = _piManualTaoSourceList.First().Level;
                m_diameter = _piManualTaoSourceList.First().Diameter;

                //FillDGV_Pi_Tao_Manual(_piManulTaoTargetList);//先把右边设为不折叠状态

                // 获取鼠标在DataGridView中的位置（行和列索引）  
                int rowIndex = dataGridView1.HitTest(e.X, e.Y).RowIndex;
                int colIndex = dataGridView1.HitTest(e.X, e.Y).ColumnIndex;

                //DataTable dt = dataGridView1.DataSource as DataTable;


                // 检查是否确实点击在了一个单元格上  
                if (rowIndex >= 0 && colIndex >= 0)//
                {
                    //int _length = (int)(dt.Rows[rowIndex][2]);
                    ////string _serinum = (string)(dt.Rows[rowIndex][1]);
                    //int _serinum = (int)(dt.Rows[rowIndex][1]);
                    //string _corMsg = (string)(dt.Rows[rowIndex][5]);
                    int _length = (int)(dataGridView1.Rows[rowIndex].Cells[2].Value);
                    int _serinum = (int)(dataGridView1.Rows[rowIndex].Cells[1].Value);
                    string _corMsg = (string)(dataGridView1.Rows[rowIndex].Cells[5].Value);


                    _flyingRebar.Clear();
                    var templist = _piManualTaoSourceList.Where(t => t.length == _length && t.iSerialNum == _serinum && t.CornerMessage == _corMsg).ToList();//选择时，长度和边角信息都作为判断依据，一般只有一个元素
                    if (isCtrlDown)//如果ctrl键也按下，则为组合，此时可多放几个rebar
                    {
                        int _multi = GeneralClass.OriginalLength(templist.First().Level, templist.First().Diameter) / _length;
                        _flyingRebar.AddRange(templist.Take(Math.Min(_multi, templist.Count)));//取多个作为flying，此处数量取剩余根数和整长段数的较小值
                        isCtrlDown = false;//复位
                    }
                    else
                    {
                        _flyingRebar.AddRange(templist.Take(1));//将符合长度要求的rebar，取第一个作为flying

                    }

                }
            }
            catch (Exception ex) { MessageBox.Show("dataGridView1_MouseDown error:" + ex.Message); }

        }
        /// <summary>
        /// 主要处理右键删除的功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView2_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (!_manualEnabled) return;

                if (e.Button == MouseButtons.Right)//删掉右键选中的元素
                {
                    // 获取鼠标在DataGridView中的位置（行和列索引）  
                    int rowIndex = dataGridView2.HitTest(e.X, e.Y).RowIndex;
                    int colIndex = dataGridView2.HitTest(e.X, e.Y).ColumnIndex;

                    // 检查是否确实点击在了一个单元格上  
                    if (rowIndex >= 0 && colIndex >= 0 && rowIndex < _piManulTaoTargetList.Count)//确保鼠标点击在图片列
                    {

                        Rectangle rectCurCell = dataGridView2.GetCellDisplayRectangle(colIndex, rowIndex, true);//获取当前单元格相对dgv控件的坐标

                        Rectangle rectPic = new Rectangle(rectCurCell.Location, GeneralClass.taoPicSize);//取单元格的左上角坐标和套料显示图片的尺寸

                        if (rectPic.Contains(e.X, e.Y))//鼠标位置是否在图片区域内
                        {
                            Point _p = new Point(e.X - rectCurCell.X, e.Y - rectCurCell.Y);//建立一个相对坐标点，为鼠标位置相对图片左上角的坐标

                            RebarOri _rebarOri = _piManulTaoTargetList[rowIndex];
                            Modifylist(_p, ref _rebarOri, true);

                            _flyingRebar.Clear();//右键删除的也要列入flyingrebar
                            _flyingRebar.AddRange(_rebarOri._list.Where(t => t.PickUsed == true).ToList());

                            _piManualTaoSourceList.AddRange(_flyingRebar);//将选中的元素返回sourcelist
                            _rebarOri._list.RemoveAll(t => t.PickUsed == true);//删掉右键选中的元素

                            dataGridView2.Rows[rowIndex].Cells[colIndex].Value = graphics.PaintRebar(_rebarOri);

                            foreach (var item in _piManualTaoSourceList)
                            {
                                item.PickUsed = false;//复位
                            }
                            RefreshUI();
                            _flyingRebar.Clear();//刷新完清空flyingrebar


                            //_sourceOri?.Clear();//清空
                            //_sourceOri.Add(new KeyValuePair<int, RebarOri>(rowIndex, _rebarOri));//将本rebarOri存入source，等待拖拽
                        }
                    }
                }

            }
            catch (Exception ex) { MessageBox.Show("dataGridView2_MouseDown error:" + ex.Message); }

        }
        /// <summary>
        /// 主要处理手动套料新插入rebar的显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView2_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                //if(isDoubleClicked)
                //{
                //    return;
                //}

                // 获取鼠标在DataGridView中的位置（行和列索引）  
                int rowIndex = dataGridView2.HitTest(e.X, e.Y).RowIndex;
                int colIndex = dataGridView2.HitTest(e.X, e.Y).ColumnIndex;

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
                            //20250712,如果临时原材长度不为0，则按临时原材长度来套料
                            RebarOri temp = (m_orilength == 0) ? new RebarOri(m_level, m_diameter) : new RebarOri(m_orilength, m_level, m_diameter);
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


                }

            }
            catch (Exception ex) { MessageBox.Show("dataGridView38_MouseUp error:" + ex.Message); }

        }

        private string m_level;
        private int m_diameter;

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                if (e.Control && e.KeyCode == Keys.C)//ctrl+c,复制
                {
                    m_level = _piManualTaoSourceList.First().Level;
                    m_diameter = _piManualTaoSourceList.First().Diameter;

                    _copyRebarOri = new RebarOri(m_level, m_diameter);

                    int _curIndex = dataGridView2.CurrentRow.Index;
                    _copyRebarOri = _piManulTaoTargetList[_curIndex];
                }
                if (e.Control && e.KeyCode == Keys.V)//ctrl+v,粘贴
                {
                    while (true)
                    {
                        //先判断sourcelist中的rebar够不够，不够就要return,20240520添加
                        var _gg = _copyRebarOri._list.GroupBy(t => new { t.length, t.CornerMessage }).ToList();
                        foreach (var iii in _gg)
                        {
                            var _have = _piManualTaoSourceList.Where(t => t.length == iii.Key.length && t.CornerMessage == iii.Key.CornerMessage).ToList();

                            if (_have.Count < iii.ToList().Count)//数量不够了
                            {
                                MessageBox.Show("长度为:" + iii.Key.ToString() + " 的钢筋，数量不够！");
                                RefreshUI();//数量不够的时候刷新一次界面，再退出
                                return;
                            }
                        }

                        //手动从sourcelist中取rebar
                        RebarOri temp = new RebarOri(m_level, m_diameter);
                        foreach (var item in _copyRebarOri._list)
                        {
                            var tttlist = _piManualTaoSourceList.Where(t => t.length == item.length && t.CornerMessage == item.CornerMessage).ToList();//从source中取长度一致、边角信息一致的rebar
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
                _piManulTaoTargetList.RemoveAll(t => t._list.Count == 0);//20250801，先移除空的原材

                FillDGV_Pi_Tao_Manual(_piManulTaoTargetList);

                //int m_curIndex = dataGridView2.FirstDisplayedScrollingRowIndex;//保持当前滚动显示的第一行不变
                int m_curIndex = dataGridView2.Rows.Count;//保持当前滚动显示的第一行不变
                dataGridView2.Rows[m_curIndex - 1].Selected = true;
                dataGridView2.FirstDisplayedScrollingRowIndex = m_curIndex - 1;


                //刷新长度统计表
                int l_curIndex = (dataGridView1.CurrentRow == null ? 0 : dataGridView1.CurrentRow.Index);//保持选中行不变
                //FillDGV_Length(_piManualTaoSourceList);


                DataTable dt = dataGridView1.DataSource as DataTable;
                int _length = 0;
                int _serinum = 0;
                string _cornermsg = "";
                if (_flyingRebar != null && _flyingRebar.Count != 0)
                {
                    foreach (var tttt in _flyingRebar)
                    {
                        _length = tttt.iLength;
                        _serinum = tttt.iSerialNum;
                        _cornermsg = tttt.CornerMessage;

                        string filters = $"序号 = '{_serinum}' AND [长度(mm)] = '{_length}' AND 边角信息='{_cornermsg}'";//select语句，注意要把筛选条件独立出来，用$符号起头
                        DataRow[] _row = dt.Select(filters);
                        if (_row.Length != 0)
                        {
                            foreach (var item in _row)
                            {
                                var temp = _piManualTaoSourceList.Where(t => t.length == _length
                                                        && t.iSerialNum == _serinum
                                                        && t.CornerMessage == _cornermsg).ToList();
                                item["数量(根)"] = temp.Count;
                                item["重量(kg)"] = temp.Sum(t => t.TotalWeight);
                            }
                        }
                    }
                }





                //for (int i = 0; i < dataGridView1.Rows.Count; i++)//20241213修改，刷新长度表时不要全部刷新
                //{
                //    int _length = (int)(dataGridView1.Rows[i].Cells[2].Value);//长度
                //    int _serinum = (int)(dataGridView1.Rows[i].Cells[1].Value);//序号
                //    string _cormsg = (string)(dataGridView1.Rows[i].Cells[5].Value);//边角结构
                //    var templist = _piManualTaoSourceList.Where(t => t.length == _length
                //                                         && t.iSerialNum == _serinum
                //                                        && t.CornerMessage == _cormsg).ToList();//选择时，长度和边角信息都作为判断依据，一般只有一个元素
                //    dataGridView1.Rows[i].Cells[3].Value = templist.First().TotalPieceNum;
                //    dataGridView1.Rows[i].Cells[4].Value = templist.First().TotalWeight;
                //}

                //dataGridView1.Rows[l_curIndex].Selected = true;
            }
            catch (Exception ex) { MessageBox.Show("RefreshUI error:" + ex.Message); }
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

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = checkBox12.Checked ? true : false;
            checkBox2.Checked = checkBox12.Checked ? true : false;
            checkBox3.Checked = checkBox12.Checked ? true : false;
            checkBox4.Checked = checkBox12.Checked ? true : false;
            checkBox5.Checked = checkBox12.Checked ? true : false;
            checkBox6.Checked = checkBox12.Checked ? true : false;
            checkBox7.Checked = checkBox12.Checked ? true : false;
            checkBox8.Checked = checkBox12.Checked ? true : false;
            checkBox9.Checked = checkBox12.Checked ? true : false;
            checkBox10.Checked = checkBox12.Checked ? true : false;
            checkBox11.Checked = checkBox12.Checked ? true : false;

            checkBox12.Text = checkBox12.Checked ? "全选" : "全不选";
        }
        /// <summary>
        /// 判断直径选择是否处于单选中状态
        /// </summary>
        /// <returns></returns>
        private bool CheckDiameterSinglePick()
        {
            int _count = 0;
            if (checkBox1.Checked) { _count++; }
            if (checkBox2.Checked) { _count++; }
            if (checkBox3.Checked) { _count++; }
            if (checkBox4.Checked) { _count++; }
            if (checkBox5.Checked) { _count++; }
            if (checkBox6.Checked) { _count++; }
            if (checkBox7.Checked) { _count++; }
            if (checkBox8.Checked) { _count++; }
            if (checkBox9.Checked) { _count++; }
            if (checkBox10.Checked) { _count++; }
            if (checkBox11.Checked) { _count++; }

            if (_count == 1)
                return true;
            else
                return false;

        }

        /// <summary>
        /// 临时的原材长度，20250712，
        /// </summary>
        private int m_orilength = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            if (!CheckDiameterSinglePick())
            {
                MessageBox.Show("操作无效，请先确保只选择一种直径规格的钢筋！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            m_orilength = Convert.ToInt32(textBox3.Text);//根据原材长度设定值，来赋予临时原材长度

            _piManulTaoTargetList = new List<RebarOri>();
            FillDGV_Pi_Tao_Manual(_piManulTaoTargetList);//清空dt




            //List<RebarData> _sonlist = new List<RebarData>();//分子
            //foreach (RebarData _dd in AllRebarListAfterPick)
            //{
            //    if (_dd.TotalPieceNum != 0 && CheckDiameter(_dd))
            //    {
            //        _sonlist.Add(_dd);
            //    }
            //}
            //_piManualTaoSourceList = Algorithm.ListExpand(_sonlist);




            _manualEnabled = true;//手动使能开启
            button2.BackColor = Color.DarkSalmon;

        }


        private void button3_Click(object sender, EventArgs e)
        {
            FillDGV_Pi_Tao_Manual(_piManulTaoTargetList, _fold);
            button3.Image = _fold ? Properties.Resources.icons8_double_down_26 : Properties.Resources.icons8_double_up_26;
            button3.Text = _fold ? "展开" : "折叠";
            _fold = !_fold;

        }



        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                isCtrlDown = true;
            }
        }

        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                isCtrlDown = false;
            }
        }

        private void dataGridView3_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //自动编号，与数据无关
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
               e.RowBounds.Location.Y,
               dataGridView3.RowHeadersWidth - 4,
               e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics,
                  (e.RowIndex + 1).ToString(),
                   dataGridView3.RowHeadersDefaultCellStyle.Font,
                   rectangle,
                   dataGridView3.RowHeadersDefaultCellStyle.ForeColor,
                   TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!CheckDiameterSinglePick())
            {
                MessageBox.Show("操作无效，请先确保只选择一种直径规格的钢筋！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //List<RebarData> _sonData = new List<RebarData>();//分子
            //int _threshold = (GeneralClass.m_typeC12) ? 12 : ((GeneralClass.m_typeC14) ? 14 : 16);//先看12是否为棒材，再看14是否为棒材
            //foreach (RebarData _dd in GeneralClass.AllRebarList)
            //{
            //    if (_dd.Diameter >= _threshold && _dd.TotalPieceNum != 0 && CheckDiameter(_dd))
            //    {
            //        _sonData.Add(_dd);
            //    }
            //}

            List<RebarData> _sonData = new List<RebarData>();//分子
            foreach (RebarData _dd in AllRebarListAfterPick)
            {
                if (_dd.TotalPieceNum != 0 && CheckDiameter(_dd))
                {
                    _sonData.Add(_dd);
                }
            }

            _piAutoTaoTargetList = Algorithm.Taoliao_pi(_sonData);

            FillDGV_Pi_Tao_Auto(_piAutoTaoTargetList);
        }

        /// <summary>
        /// 显示批量锯切套料效果图
        /// </summary>
        private void FillDGV_Pi_Tao_Auto(List<RebarOri> _list, bool _fold = false)
        {
            if (_fold)//折叠
            {
                DataTable dt = new DataTable();

                //dt.Columns.Add("序号", typeof(int));
                dt.Columns.Add("数量", typeof(int));
                dt.Columns.Add("钢筋原材分段", typeof(Image));

                var temp = Algorithm.ExchangeRebarOri(_list);

                foreach (var item in temp)
                {
                    //var str = item.cornerMsgList;
                    //int _index = temp.IndexOf(item);
                    //dt.Rows.Add(_index, item.num, graphics.PaintRebar(item.datalist[0]));
                    dt.Rows.Add(item.num, graphics.PaintRebar(item._list[0]));
                }

                dataGridView4.DataSource = dt;
            }
            else
            {
                DataTable dt = new DataTable();
                //dt.Columns.Add("序号", typeof(int));
                dt.Columns.Add("钢筋原材分段", typeof(Image));

                foreach (var item in _list)
                {
                    //int _index = _list.IndexOf(item);
                    dt.Rows.Add(/*_index,*/ graphics.PaintRebar(item));
                }
                dataGridView4.DataSource = dt;
            }

            if (radioButton2.Checked)
            {
                textBox2.Text = _list.Count.ToString();//原材根数
                int _real = _list.Sum(t => t._lengthListUsed);
                int _use = _list.Sum(t => t._totalLength);
                textBox1.Text = ((double)_real / (double)_use).ToString("P1");

            }
        }

        private void dataGridView4_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                // 获取鼠标在DataGridView中的位置（行和列索引）  
                int rowIndex = dataGridView4.HitTest(e.X, e.Y).RowIndex;
                int colIndex = dataGridView4.HitTest(e.X, e.Y).ColumnIndex;

                // 检查是否确实点击在了一个单元格上  
                if (rowIndex >= 0 && colIndex >= 0)//确保鼠标点击在图片列
                {

                    Rectangle rectCurCell = dataGridView4.GetCellDisplayRectangle(colIndex, rowIndex, true);//获取当前单元格相对dgv控件的坐标

                    //Point _leftTop = new Point(dataGridView4.Rows[rowIndex].Cells[colIndex].ContentBounds.Left + rectCurCell.Left,
                    //    dataGridView4.Rows[rowIndex].Cells[colIndex].ContentBounds.Top + rectCurCell.Top);

                    Rectangle rectPic = new Rectangle(rectCurCell.Location, GeneralClass.taoPicSize);//取单元格的左上角坐标和套料显示图片的尺寸

                    if (rectPic.Contains(e.X, e.Y))//鼠标位置是否在图片区域内
                    {
                        Point _p = new Point(e.X - rectCurCell.X, e.Y - rectCurCell.Y);//建立一个相对坐标点，为鼠标位置相对图片左上角的坐标

                        RebarOri _rebarOri = _piAutoTaoTargetList[rowIndex];
                        Modifylist(_p, ref _rebarOri, true);

                        dataGridView4.Rows[rowIndex].Cells[colIndex].Value = graphics.PaintRebar(_rebarOri);

                        _sourceOri?.Clear();//清空
                        _sourceOri.Add(new KeyValuePair<int, RebarOri>(rowIndex, _rebarOri));//将本rebarOri存入source，等待拖拽
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("dataGridView4_MouseDown error:" + ex.Message); }

        }

        private void dataGridView4_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                // 获取鼠标在DataGridView中的位置（行和列索引）  
                int rowIndex = dataGridView4.HitTest(e.X, e.Y).RowIndex;
                int colIndex = dataGridView4.HitTest(e.X, e.Y).ColumnIndex;

                bool _haveSelect = false;
                // 检查是否确实点击在了一个单元格上  
                if (rowIndex >= 0 && colIndex >= 0)//确保鼠标点击在图片列
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
                        Rectangle rectCurCell = dataGridView4.GetCellDisplayRectangle(colIndex, rowIndex, true);//获取当前单元格相对dgv控件的坐标

                        Rectangle rectPic = new Rectangle(rectCurCell.Location, GeneralClass.taoPicSize);//取单元格的左上角坐标和套料显示图片的尺寸

                        if (rectPic.Contains(e.X, e.Y))//鼠标位置是否在图片区域内
                        {
                            Point _p = new Point(e.X - rectCurCell.X, e.Y - rectCurCell.Y);//建立一个相对坐标点，为鼠标位置相对图片左上角的坐标

                            RebarOri _rebarOri = _piAutoTaoTargetList[rowIndex];
                            Modifylist(_p, ref _rebarOri, false);//修改list元素的pick状态
                            dataGridView4.Rows[rowIndex].Cells[colIndex].Value = graphics.PaintRebar(_rebarOri);//重绘套料显示图
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

                            dataGridView4.Rows[_sourceIndex].Cells[colIndex].Value = graphics.PaintRebar(_piAutoTaoTargetList[_sourceIndex]);//重绘sourceOri
                            dataGridView4.Rows[rowIndex].Cells[colIndex].Value = graphics.PaintRebar(_piAutoTaoTargetList[rowIndex]);//重绘targetOri
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("dataGridView4_MouseUp error:" + ex.Message); }

        }

        private void dataGridView2_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //自动编号，与数据无关
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
               e.RowBounds.Location.Y,
               dataGridView2.RowHeadersWidth - 4,
               e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics,
                  (e.RowIndex + 1).ToString(),
                   dataGridView2.RowHeadersDefaultCellStyle.Font,
                   rectangle,
                   dataGridView2.RowHeadersDefaultCellStyle.ForeColor,
                   TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void dataGridView4_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //自动编号，与数据无关
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
               e.RowBounds.Location.Y,
               dataGridView4.RowHeadersWidth - 4,
               e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics,
                  (e.RowIndex + 1).ToString(),
                   dataGridView4.RowHeadersDefaultCellStyle.Font,
                   rectangle,
                   dataGridView4.RowHeadersDefaultCellStyle.ForeColor,
                   TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        /// <summary>
        /// rebarOri汇总转换成RebarOriPiWithDiameter
        /// </summary>
        /// <param name="_OriList"></param>
        /// <returns></returns>
        private RebarOriPiWithDiameter GroupAllRebarOri(List<RebarOri> _OriList)
        {
            RebarOriPiWithDiameter _newbill = new RebarOriPiWithDiameter();//新建RebarOriPiWithDiameter

            List<RebarOriPi> temp = Algorithm.ExchangeRebarOri(_OriList);//先把rebarOri转化成rebarOriPi

            EnumDiaBang _pick = GetDiameterChecked();//获取当前选中的直径

            _newbill._diameter = GeneralClass.EnumDiameterToInt(_pick);
            foreach (var item in temp)
            {
                _newbill._rebarOriPiList.Add(item);
            }
            //_newbill._rebarOriPiList = temp;

            return _newbill;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            //List<RebarOriPi> temp = Algorithm.ExchangeRebarOri(_piManulTaoTargetList);

            //EnumDiaBang _pick = GetDiameterChecked();

            //RebarOriPiWithDiameter _newbill = new RebarOriPiWithDiameter();

            //_newbill._diameter = GeneralClass.EnumDiameterToInt(_pick);
            //_newbill._rebarOriPiList = temp;

            //m_allDiameterWorkBill._list.Add(_newbill);

            m_allDiameterWorkBill._list.Clear();//20250713，每次只添加一个加工批，以免每次需要重开软件
            if (radioButton1.Checked)//手动
            {
                m_allDiameterWorkBill._list.Add(GroupAllRebarOri(_piManulTaoTargetList));
                //DBOpt.SaveAllPiCutBatchToDB(m_allDiameterWorkBill);
            }
            else
            {
                m_allDiameterWorkBill._list.Add(GroupAllRebarOri(_piAutoTaoTargetList));
                //DBOpt.SaveAllPiCutBatchToDB(m_allDiameterWorkBill);
            }

            FillDGV_AllWorkBill(m_allDiameterWorkBill);

            GeneralClass.interactivityData?.printlog(1, "墙柱线工单增加新的子工单！");
            tabControl2.SelectTab(1);




            //存入批量锯切数据库
            GeneralClass.AllRebarList = GeneralClass.DBOpt.GetAllRebarList(GeneralClass.TableName_AllRebar);//取得所有的钢筋数据list
            this.AllRebarListAfterPick = Pick(GeneralClass.AllRebarList);//先筛选棒材/线材，箍筋/拉钩/马凳/端头/主筋

            List<RebarData> _sonData = new List<RebarData>();//
            foreach (RebarData _dd in AllRebarListAfterPick)
            {
                if (_dd.TotalPieceNum != 0 && CheckDiameter(_dd))//筛选直径
                {
                    _sonData.Add(_dd);
                }
            }
            SavePiCut(_sonData);            //保存至批量锯切数据库

        }
        private void button6_Click(object sender, EventArgs e)
        {
            WorkBillMsg wbMsg = new WorkBillMsg();
            wbMsg.shift = 1;
            wbMsg.projectName = GeneralClass.CfgData.ProjectName;
            wbMsg.block = "A";
            wbMsg.building = "06D";
            wbMsg.floor = "01F";
            wbMsg.level = "C";//钢筋级别
            wbMsg.brand = "/";//厂商
            wbMsg.specification = "HRB400";//规格型号
            wbMsg.tasklistName = textBox4.Text;//加工单名称

            WorkBill_QZ _workbill = new WorkBill_QZ();
            GeneralClass.jsonList_QZ.Clear();
            GeneralClass.jsonList_QZ.Add(GeneralClass.WorkBillOpt.CreateWorkBill_QZ(wbMsg, m_allDiameterWorkBill, out _workbill));

            //GeneralClass.jsonList_PiCut = GeneralClass.WorkBillOpt.CreateWorkBill_PiCut(wbMsg, m_allDiameterWorkBill);
            //GeneralClass.jsonList_PiCut = GeneralClass.WorkBillOpt.CreateWorkBill_QZ(4, wbMsg, m_allDiameterWorkBill);

            DBOpt.SaveAllPiCutBatchToDB(m_allDiameterWorkBill);

            //保存json到txt文件
            string filePath = Application.StartupPath + @"\output\" + _workbill.TasklistName + ".txt";
            //GeneralClass.ExcelWriteOpt?.SaveSheet(filePath, _newdt);
            NewtonJson.SaveJsonToFile(GeneralClass.jsonList_QZ?.First(), filePath);
            GeneralClass.interactivityData?.printlog(1, "已导出加工单到路径：" + Application.StartupPath + @"\output\" + _workbill.TasklistName + ".xlsx");


            MessageBox.Show("墙柱线工单、批量式工单已生成！");
            GeneralClass.interactivityData?.printlog(1, "墙柱线工单、批量式工单已生成！");

        }

        private void FillDGV_AllWorkBill(RebarOriPiAllDiameter _workbill)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("直径(Φ)", typeof(int));
            dt.Columns.Add("锯切批次", typeof(int));

            foreach (var item in _workbill._list)
            {
                dt.Rows.Add(item._diameter, item._rebarOriPiList.Count);
            }

            dataGridView5.DataSource = dt;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                radioButton1.Checked = true;
            }
            if (tabControl1.SelectedIndex == 1)
            {
                radioButton2.Checked = true;
            }
            //if (tabControl1.SelectedTab.Text == "手动套料")
            //{
            //    radioButton1.Checked = true;
            //}
            //if (tabControl1.SelectedTab.Text == "自动套料")
            //{
            //    radioButton2.Checked = true;
            //}
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();//打开文件夹对话框
            openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openFileDialog.Filter = "excel文件(*.xlsx)|*.xlsx|excel文件(*.xls)|*.xls|所有文件(*.*)|*.*";
            openFileDialog.Title = "打开料单";
            openFileDialog.FilterIndex = 0;
            openFileDialog.Multiselect = false;//不允许多选
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filepath = openFileDialog.FileName;

                //string filename = System.IO.Path.GetFileNameWithoutExtension(filepath); //获取excel文件名作为根节点名称,不带后缀名

                //List<DataTable> _dtlist = GeneralClass.readEXCEL?.GetAllSheets(filename);//获取所有datasheet的数据
                List<DataTable> _dtlist = GeneralClass.ExcelReadOpt?.GetAllSheets(filepath);//获取所有datasheet的数据

                DataTable dt = _dtlist.Last();//注意要取最后一个sheet

                List<RebarData> _rebarlist = new List<RebarData>();


                EnumDiaBang _dia = EnumDiaBang.NONE;
                int _length = 0;
                int _num = 0;

                foreach (DataRow row in dt.Rows)
                {
                    if (row[0].ToString().Equals("直径")) { continue; }//第一行跳过

                    int _diaTemp = Convert.ToInt32(row[0].ToString().Substring(1, 2));//提取每行第一格，为直径
                    _dia = GeneralClass.IntToEnumDiameter(_diaTemp);//转成enum

                    _length = Convert.ToInt32((row[1]).ToString());
                    _num = Convert.ToInt32((row[2]).ToString());


                    RebarData _rebar = new RebarData();//凭空捏造rebardata
                    _rebar.Diameter = _diaTemp;
                    _rebar.Level = "C";
                    _rebar.TotalPieceNum = _num;
                    _rebar.CornerMessage = _rebar.Length + ",0";//先随便给个边角信息，例如：【2900，0】
                    _rebar.TotalWeight = 1;//随便给个数

                    _length = _length % GeneralClass.OriginalLength(_rebar.Level, _rebar.Diameter);//取原材长度的余数，注意此步很重要
                    _rebar.Length = _length.ToString();

                    _rebarlist.Add(_rebar);

                }
                //手动套
                _piManualTaoSourceList = new List<Rebar>();//清空
                _piManualTaoSourceList = Algorithm.ListExpand(_rebarlist);
                FillDGV_Length(_piManualTaoSourceList);

                _piManulTaoTargetList = new List<RebarOri>();
                FillDGV_Pi_Tao_Manual(_piManulTaoTargetList);//清空dt

                _manualEnabled = true;

                //自动套
                _piAutoTaoTargetList = Algorithm.Taoliao_pi(_rebarlist);
                FillDGV_Pi_Tao_Auto(_piAutoTaoTargetList);

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            FillDGV_Pi_Tao_Auto(_piAutoTaoTargetList, _fold);
            button8.Image = _fold ? Properties.Resources.icons8_double_down_26 : Properties.Resources.icons8_double_up_26;
            _fold = !_fold;
        }
        //实现复制钢筋的功能，与ctrl+c/ctrl+v一致
        private void button9_Click(object sender, EventArgs e)
        {
            //先获取待复制的rebarori
            _copyRebarOri = (m_orilength == 0) ? new RebarOri(m_level, m_diameter) : new RebarOri(m_orilength, m_level, m_diameter);

            int _curIndex = dataGridView2.CurrentRow.Index;
            _copyRebarOri = _piManulTaoTargetList[_curIndex];




            //if (e.Control && e.KeyCode == Keys.V)//ctrl+v,粘贴
            //{
            while (true)
            {
                //先判断sourcelist中的rebar够不够，不够就要return,20240520添加
                var _gg = _copyRebarOri._list.GroupBy(t => new { t.length, t.SerialNum, t.CornerMessage }).ToList();

                _flyingRebar.Clear();
                foreach (var ttt in _gg)
                {
                    _flyingRebar.AddRange(ttt.ToList());//刷新UI需要用到flyingrebar
                }

                foreach (var iii in _gg)
                {
                    var _have = _piManualTaoSourceList.Where(t => t.length == iii.Key.length
                                                            && t.SerialNum == iii.Key.SerialNum
                                                            && t.CornerMessage == iii.Key.CornerMessage).ToList();

                    if (_have.Count < iii.ToList().Count)//数量不够了
                    {
                        //MessageBox.Show("长度为:" + iii.Key.ToString() + " 的钢筋，数量不够！");
                        RefreshUI();//数量不够的时候刷新一次界面，再退出
                        _flyingRebar.Clear();//

                        return;
                    }
                }

                //手动从sourcelist中取rebar
                RebarOri temp = (m_orilength == 0) ? new RebarOri(m_level, m_diameter) : new RebarOri(m_orilength, m_level, m_diameter);
                foreach (var item in _copyRebarOri._list)
                {
                    var tttlist = _piManualTaoSourceList.Where(t => t.length == item.length
                                                                        && t.SerialNum == item.SerialNum
                                                                        && t.CornerMessage == item.CornerMessage).ToList();//从source中取长度一致、边角信息一致的rebar
                    temp._list.Add(tttlist[0]);//将符合长度要求的rebar，取第一个

                    _piManualTaoSourceList.Remove(tttlist[0]);//sourcelist中去除掉已经手动套料的rebar
                }
                _piManulTaoTargetList.Add(temp);

                //RefreshUI();

                Thread.Sleep(1);
            }


            //}
        }

        /// <summary>
        /// 打开json
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_Click(object sender, EventArgs e)
        {
            ////保存json到txt文件
            //string filePath = Application.StartupPath + @"\output\" /*+ _workbill.TasklistName*/ + "1.xls";
            //ExportDataGridViewToExcel(dataGridView2, filePath);
            //ExportToExcel(dataGridView2, filePath);


            //打开excel文件
            OpenFileDialog openFileDialog = new OpenFileDialog();//打开文件夹对话框
                                                                 //openFileDialog.InitialDirectory = Application.StartupPath + @"\excelfile\";
            openFileDialog.InitialDirectory = GeneralClass.CfgData.EJINpath;
            //openFileDialog.Filter = "excel文件(*.xls)|*.xls|excel文件(*.xlsx)|*.xlsx|所有文件(*.*)|*.*";
            openFileDialog.Filter = "txt文件(*.txt)|*.txt";
            openFileDialog.Title = "打开加工单";
            openFileDialog.FilterIndex = 0;
            openFileDialog.Multiselect = false;//不让导入多个
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                GeneralClass.jsonList_QZ.Clear();
                //foreach (string filepath in openFileDialog.FileNames)
                //{
                //    GeneralClass.jsonList_QZ.Add(NewtonJson.ReadJsonFromFile(filepath));
                //}
                GeneralClass.jsonList_QZ.Add(NewtonJson.ReadJsonFromFile(openFileDialog.FileName));//读取json

                WorkBill_QZ _bill = NewtonJson.Deserializer<WorkBill_QZ>(GeneralClass.jsonList_QZ[0]);//将json转成墙柱工单模板

                GeneralClass.WorkBillOpt.ParseWorkBill_QZ(_bill, ref _piManulTaoTargetList);

                RefreshUI();

                GeneralClass.CfgData.EJINpath = Path.GetDirectoryName(openFileDialog.FileName);//获取最新选择的路径
                string _json = NewtonJson.Serializer(GeneralClass.CfgData);//修改配置json
                Config.SaveConfig(_json);//保存配置文件




            }



        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {


            //DataTable dt = dataGridView1.DataSource as DataTable;
            //DataView _view = dt.DefaultView;
            //dataGridView1.DataSource = _view.ToTable();




            //// 获取当前绑定的DataView（已排序）
            //if (dataGridView1.DataSource is DataView sortedView)
            //{
            //    // 创建临时DataTable存储排序后的数据
            //    DataTable sortedTable = sortedView.ToTable();

            //    // 获取原始DataTable引用
            //    DataTable originalTable = sortedView.Table;

            //    // 清空原始表数据（保留结构）
            //    originalTable.Clear();

            //    // 按排序顺序导入行（保留行状态和原始值）
            //    foreach (DataRow row in sortedTable.Rows)
            //    {
            //        originalTable.ImportRow(row);
            //    }

            //    // 可选：重新绑定（通常自动更新）
            //    dataGridView1.DataSource = originalTable;
            //}

        }


        ///// <summary>
        ///// 将DataGridView保存为Excel文件，包括图片
        ///// </summary>
        ///// <param name="dgv">要导出的DataGridView</param>
        ///// <param name="filePath">保存路径</param>
        ///// <param name="isXlsx">是否保存为xlsx格式</param>
        //public void ExportToExcel(DataGridView dgv, string filePath, bool isXlsx = true)
        //{
        //    if (dgv.Rows.Count == 0)
        //    {
        //        MessageBox.Show("没有数据可导出！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return;
        //    }

        //    IWorkbook workbook;

        //    // 创建工作簿
        //    if (isXlsx)
        //    {
        //        workbook = new XSSFWorkbook();
        //    }
        //    else
        //    {
        //        workbook = new HSSFWorkbook();
        //    }

        //    // 创建工作表
        //    ISheet sheet = workbook.CreateSheet("Data");

        //    // 创建表头
        //    IRow headerRow = sheet.CreateRow(0);
        //    for (int i = 0; i < dgv.Columns.Count; i++)
        //    {
        //        if (dgv.Columns[i].Visible)
        //        {
        //            ICell cell = headerRow.CreateCell(i);
        //            cell.SetCellValue(dgv.Columns[i].HeaderText);
        //        }
        //    }

        //    // 填充数据和图片
        //    for (int rowIndex = 0; rowIndex < dgv.Rows.Count; rowIndex++)
        //    {
        //        if (dgv.Rows[rowIndex].IsNewRow) continue;

        //        IRow dataRow = sheet.CreateRow(rowIndex + 1);

        //        for (int colIndex = 0; colIndex < dgv.Columns.Count; colIndex++)
        //        {
        //            if (!dgv.Columns[colIndex].Visible) continue;

        //            // 处理图片单元格
        //            if (dgv.Rows[rowIndex].Cells[colIndex] is DataGridViewImageCell imageCell &&
        //                imageCell.Value != null && imageCell.Value != DBNull.Value)
        //            {
        //                // 获取图片
        //                Image image = (Image)imageCell.Value;

        //                // 将图片转换为字节数组
        //                byte[] imageBytes;
        //                using (MemoryStream ms = new MemoryStream())
        //                {
        //                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        //                    imageBytes = ms.ToArray();
        //                }

        //                // 添加图片到工作表
        //                int pictureIdx = workbook.AddPicture(imageBytes, PictureType.PNG);
        //                IDrawing drawing = sheet.CreateDrawingPatriarch();
        //                IClientAnchor anchor = workbook.GetCreationHelper().CreateClientAnchor();

        //                // 设置图片位置和大小
        //                anchor.Row1 = rowIndex + 1;
        //                anchor.Col1 = colIndex;

        //                // 调整行高以适应图片
        //                //sheet.SetRowHeight(rowIndex + 1, (short)(image.Height * 20));


        //                // 调整列宽以适应图片
        //                sheet.SetColumnWidth(colIndex, (int)(image.Width * 20));

        //                // 创建图片
        //                IPicture picture = drawing.CreatePicture(anchor, pictureIdx);
        //                picture.Resize(); // 调整图片大小以适应单元格
        //            }
        //            // 处理普通文本单元格
        //            else
        //            {
        //                object cellValue = dgv.Rows[rowIndex].Cells[colIndex].Value;
        //                ICell cell = dataRow.CreateCell(colIndex);

        //                if (cellValue != null && cellValue != DBNull.Value)
        //                {
        //                    cell.SetCellValue(cellValue.ToString());
        //                }
        //            }
        //        }
        //    }

        //    // 保存文件
        //    try
        //    {
        //        using (FileStream fs = new FileStream(filePath, FileMode.Create))
        //        {
        //            workbook.Write(fs,false);//leaveopen，写入完成后记得要关闭数据流，这里务必选false
        //        }
        //        MessageBox.Show($"文件已成功保存到：{filePath}", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"保存文件时出错：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}


        //public void ExportDataGridViewToExcel(DataGridView dgv, string filePath)
        //{
        //    // 设置 EPPlus 许可证上下文（免费使用）
        //    //ExcelPackage.License = OfficeOpenXml.LicenseContext.NonCommercial;
        //    //ExcelPackage.License =
        //    EPPlusLicense ePPlusLicense = new EPPlusLicense();
        //    ePPlusLicense.SetNonCommercialPersonal("  ");

        //    using (var package = new ExcelPackage())
        //    {
        //        var worksheet = package.Workbook.Worksheets.Add("Sheet1");

        //        // 导出列标题
        //        for (int col = 0; col < dgv.Columns.Count; col++)
        //        {
        //            worksheet.Cells[1, col + 1].Value = dgv.Columns[col].HeaderText;
        //        }

        //        // 导出数据行
        //        for (int row = 0; row < dgv.Rows.Count; row++)
        //        {
        //            for (int col = 0; col < dgv.Columns.Count; col++)
        //            {
        //                var cellValue = dgv.Rows[row].Cells[col].Value;
        //                var cell = dgv.Rows[row].Cells[col];

        //                // 处理图片单元格
        //                if (cell is DataGridViewImageCell imgCell && imgCell.Value is Image)
        //                {
        //                    AddImageToWorksheet(worksheet, row + 2, col + 1, (Image)imgCell.Value);
        //                }
        //                // 处理普通文本
        //                else
        //                {
        //                    worksheet.Cells[row + 2, col + 1].Value = cellValue?.ToString();
        //                }
        //            }
        //        }

        //        // 自动调整列宽
        //        for (int col = 1; col <= dgv.Columns.Count; col++)
        //        {
        //            worksheet.Column(col).AutoFit();
        //        }

        //        // 保存文件
        //        package.SaveAs(new FileInfo(filePath));
        //    }
        //}

        //private void AddImageToWorksheet(ExcelWorksheet worksheet, int row, int col, Image image)
        //{
        //    // 将图片转换为字节数组
        //    byte[] imageBytes;
        //    using (var ms = new MemoryStream())
        //    {
        //        image.Save(ms, image.RawFormat);
        //        imageBytes = ms.ToArray();
        //    }


        //    using (Stream _stream = new MemoryStream(imageBytes))
        //    {
        //        // 添加图片到工作表
        //        var picture = worksheet.Drawings.AddPicture($"img_{row}_{col}", _stream);

        //        // 设置图片位置和尺寸
        //        picture.SetPosition(row - 1, 0, col - 1, 0);
        //        picture.SetSize(50, 50);  // 固定尺寸（单位：像素）

        //        // 高级选项：动态调整图片尺寸
        //        // picture.SetSize(image.Width / 2, image.Height / 2); 
        //    }

        //}
    }
}
