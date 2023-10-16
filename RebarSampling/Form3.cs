//using Etable;
//using Grand;
using NPOI.SS.Formula.Functions;
using RebarSampling.General;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RebarSampling
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();

            InitDataGridView();
            InitTreeView1();
            InitTreeView();
            InitCheckBox();

            GeneralClass.interactivityData.showAssembly += ShowAllElement;
        }

        private void InitTreeView1()
        {
            treeView1.Nodes.Clear();
            treeView1.LabelEdit = true;
            treeView1.ExpandAll();
            treeView1.CheckBoxes = true;//节点的勾选框

        }

        private void InitTreeView()
        {
            treeView2.Nodes.Clear();
            treeView2.LabelEdit = true;
            treeView2.ExpandAll();
            treeView2.CheckBoxes = true;//节点的勾选框

            treeView3.Nodes.Clear();
            treeView3.LabelEdit = true;
            treeView3.ExpandAll();
            treeView3.CheckBoxes = true;//节点的勾选框

            treeView4.Nodes.Clear();
            treeView4.LabelEdit = true;
            treeView4.ExpandAll();
            treeView4.CheckBoxes = true;//节点的勾选框

        }


        private void InitCheckBox()
        {
            checkBox1.Checked = true;//棒材
            checkBox2.Checked = false;//线材

            checkBox3.Checked = false;//详细
            checkBox4.Checked = true;//去零

            checkBox5.Checked = true;//弯曲
            checkBox6.Checked = true;//不弯曲
            checkBox7.Checked = true;//套丝
            checkBox8.Checked = true;//不套丝

            checkBox22.Checked = false;//标化
            checkBox23.Checked = true;//非标

            //直径种类
            checkBox9.Checked = true;
            checkBox10.Checked = true;
            checkBox11.Checked = true;
            checkBox12.Checked = true;
            checkBox13.Checked = true;
            checkBox14.Checked = true;
            checkBox15.Checked = true;
            checkBox16.Checked = true;
            checkBox17.Checked = true;

            //数量区间
            checkBox18.Checked = true;
            checkBox19.Checked = true;
            checkBox20.Checked = true;
            checkBox21.Checked = true;



        }
        private void InitDataGridView()
        {
            Form2.InitDGV(dataGridView1);
            Form2.InitDGV(dataGridView4);
            Form2.InitDGV(dataGridView9);
        }


        private string _selectproject = "";
        private string _selectassembly = "";

        private List<ElementData> _elements = new List<ElementData>();
        private void ShowAllElement(string _project, string _assembly)
        {
            InitTreeView1();

            _selectproject = _project;
            _selectassembly = _assembly;

            ////先获取钢筋总表
            //List<RebarData> _allList = GeneralClass.SQLiteOpt.GetAllRebarList(GeneralClass.AllRebarTableName);

            ////查询所有被选中的钢筋
            //List<RebarData> _newlist = new List<RebarData>();
            //foreach (RebarData _data in _allList)
            //{
            //    if (_data.ProjectName == _project && _data.MainAssemblyName == _assembly)
            //    {
            //        _newlist.Add(_data);
            //    }
            //}

            _elements = GeneralClass.SQLiteOpt.GetAllElementList(GeneralClass.AllRebarTableName, _project, _assembly);
            TreeNode tn = new TreeNode();
            TreeNode tn1 = new TreeNode();
            foreach (var item in _elements)
            {
                tn1 = new TreeNode();
                tn1.Text = item.elementName;
                tn.Nodes.Add(tn1);
            }
            tn.Text = _assembly;
            treeView1.Nodes.Add(tn);
            treeView1.ExpandAll();

            //if (_newlist.Count != 0 && dataGridView1 != null)
            //{
            //    Form2.FillDGVWithRebarList(_newlist, dataGridView1);
            //}

        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // 获取当前勾选节点
            TreeNode currentNode = e.Node;

            // 设置以当前节点为根节点的所有子节点的勾选状态等于当前节点的勾选状态
            Treeview_SetChildNodesCheckedState(currentNode, currentNode.Checked);

            // 设置所有父节点的勾选状态
            //Treeview_SetParentNodesCheckedState(currentNode);

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

            if (e.Action == TreeViewAction.ByMouse || e.Action == TreeViewAction.ByKeyboard)
            {
                if (e.Node.Parent != null)
                {
                    //List<ElementData> _list = GeneralClass.SQLiteOpt.GetAllElementList(GeneralClass.AllRebarTableName, this._selectproject, this._selectassembly);

                    foreach (var item in this._elements)
                    {
                        //if(item.elementName == e.Node.Text)
                        if (item.elementIndex == e.Node.Index)//因为子构件名不唯一，改用index索引
                        {
                            if (item.rebarlist.Count != 0 && dataGridView1 != null)
                            {
                                ////选择是否显示线材
                                //var newlist = item.rebarlist.Where(t => (checkBox2.Checked ? (t.Diameter > 0) : (t.Diameter >= 16))).ToList();

                                var newlist = PickNewList(item.rebarlist);

                                Form2.FillDGVWithRebarList(newlist, dataGridView1);

                                //ShowElementAddData(item.rebarlist);
                                ShowElementAddData(newlist);

                                dataGridView1.Columns[5].Visible = checkBox3.Checked;

                            }
                        }
                    }

                }
            }
        }

        private void Treeview_SetChildNodesCheckedState(TreeNode node, bool ischecked)
        {
            foreach (TreeNode child in node.Nodes)
            {
                child.Checked = ischecked;
                //GeneralClass.interactivityData?.printlog(1, child.Text + "设置为" + ischecked.ToString());

                if (node.Nodes.Count != 1)//用于停止递归
                {
                    Treeview_SetChildNodesCheckedState(child, ischecked);//递归设置，保证勾选某个节点后，其所有层级节点勾选状态保持一致
                }
            }
        }

        private void Treeview_SetParentNodesCheckedState(TreeNode node)
        {
            TreeNode parentnode = node.Parent;
            if (parentnode == null) return;

            bool isChecked = true;
            foreach (TreeNode child in parentnode.Nodes)
            {
                if (!child.Checked)
                {
                    isChecked = false;//如果有子节点不为选中，则父节点设为不选中
                    break;
                }
            }

            parentnode.Checked = isChecked;
            Treeview_SetParentNodesCheckedState(parentnode);
        }
        /// <summary>
        /// 显示单个构件包按照直径规格的汇总信息
        /// </summary>
        /// <param name="_list"></param>
        private void ShowElementAddData(List<RebarData> _list)
        {
            DataTable dt_z = new DataTable();
            dt_z.Columns.Add("直径", typeof(string));
            dt_z.Columns.Add("总长度(m)", typeof(double));
            dt_z.Columns.Add("总数量(根)", typeof(int));
            dt_z.Columns.Add("总重量(kg)", typeof(double));

            List<GroupbyDiameterListWithLength> _grouplist = GeneralClass.SQLiteOpt.QueryAllListByDiameterWithLength(_list);

            foreach (var item in _grouplist)
            {
                if (checkBox1.Checked ? (item._diameter >= 16) : false)
                {
                    dt_z.Rows.Add("Φ" + item._diameter.ToString(), item._totallength / 1000, item._totalnum, item._totalweight);
                }
                if (checkBox2.Checked ? (item._diameter <= 14) : false)
                {
                    dt_z.Rows.Add("Φ" + item._diameter.ToString(), item._totallength / 1000, item._totalnum, item._totalweight);
                }
            }
            dataGridView2.DataSource = dt_z;
            dataGridView2.Columns[1].DefaultCellStyle.Format = "0.000";        //
            //dataGridView2.Columns[2].DefaultCellStyle.Format = "0.0";        //
            dataGridView2.Columns[3].DefaultCellStyle.Format = "0.0";          //
        }

        private void ShowElementAddData(List<ElementDataFB> _fblist, DataGridView _dgv)
        {
            DataTable dt_z = new DataTable();
            dt_z.Columns.Add("直径", typeof(string));
            dt_z.Columns.Add("总长度(m)", typeof(double));
            dt_z.Columns.Add("总数量(根)", typeof(int));
            dt_z.Columns.Add("总重量(kg)", typeof(double));

            List<GroupbyDiameterListWithLength> _group = new List<GroupbyDiameterListWithLength>();

            foreach (var item in _fblist)
            {
                foreach (var ttt in item.diameterGroup)
                {
                    _group.Add(ttt);
                }
            }

            var _newgroup = _group.GroupBy(x => x._diameter).Select(
                y => new
                {
                    _diameter = y.Key,
                    _totallength = y.Sum(x => x._totallength),
                    _maxlength = y.Max(x => x._maxlength),
                    _minlength = y.Min(x => x._minlength),
                    _totalnum = y.Sum(x => x._totalnum),
                    _totalweight = y.Sum(x => x._totalweight),
                    //_datalist = y.ToList(x => x._datalist),         //此处代码不会写
                    _datalist = y.Select(x => x._datalist).ToList(),
                }
                ).ToList();

            _newgroup = _newgroup.OrderBy(x => x._diameter).ToList();//排序一下

            foreach (var item in _newgroup)
            {
                dt_z.Rows.Add("Φ" + item._diameter.ToString(), (double)item._totallength / 1000, item._totalnum, item._totalweight);
            }


            _dgv.DataSource = dt_z;
            _dgv.Columns[1].DefaultCellStyle.Format = "0.0";        //
            //_dgv.Columns[2].DefaultCellStyle.Format = "0.0";        //
            _dgv.Columns[3].DefaultCellStyle.Format = "0.0";          //
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Columns[5].Visible = checkBox3.Checked;
            dataGridView4.Columns[5].Visible = checkBox3.Checked;
            dataGridView9.Columns[5].Visible = checkBox3.Checked;

        }

        private void button1_Click(object sender, EventArgs e)
        {

            //解析钢筋总表的所有构件

            GeneralClass.interactivityData?.printlog(1, "开始解析所有料单的构件包");

            GeneralClass.AllElementList = GeneralClass.SQLiteOpt.GetAllElementList(GeneralClass.AllRebarTableName);

            DataTable dt_z = new DataTable();
            dt_z.Columns.Add("索引", typeof(int));
            dt_z.Columns.Add("db序号", typeof(int));
            dt_z.Columns.Add("项目名称", typeof(string));
            dt_z.Columns.Add("主构件名", typeof(string));
            dt_z.Columns.Add("子构件名", typeof(string));
            dt_z.Columns.Add("总数量", typeof(int));
            dt_z.Columns.Add("总重量(kg)", typeof(double));
            dt_z.Columns.Add("直径种类", typeof(int));
            dt_z.Columns.Add("直径组合(Φ)", typeof(string));
            dt_z.Columns.Add("最大长度", typeof(int));
            dt_z.Columns.Add("最小长度", typeof(int));


            int _num = 0;
            double _weight = 0;
            int _maxlength, _minlength = 0;
            int _index = 0;
            foreach (ElementData item in GeneralClass.AllElementList)
            {
                var _banglist = PickNewList(item.rebarlist);

                _num = _banglist.Sum(t => t.TotalPieceNum);
                _weight = _banglist.Sum(t => t.TotalWeight);

                if (checkBox4.Checked ? (_num != 0) : true)//去零显示
                {
                    //找最大最小长度值
                    int _length = 0;
                    if (_num != 0)
                    {
                        _maxlength = _banglist.Max(t => (int.TryParse(t.Length, out _length) ? _length : ((Convert.ToInt32(t.Length.Split('~')[0]) + Convert.ToInt32(t.Length.Split('~')[1])) / 2)));
                        _minlength = _banglist.Min(t => (int.TryParse(t.Length, out _length) ? _length : ((Convert.ToInt32(t.Length.Split('~')[0]) + Convert.ToInt32(t.Length.Split('~')[1])) / 2)));
                    }
                    else
                    {
                        _maxlength = 0;
                        _minlength = 0;
                    }

                    //找一个构件包中直径种类
                    var _group = GeneralClass.SQLiteOpt.QueryAllListByDiameter(_banglist);

                    var _newgroup = _group.OrderBy(t => t._diameter).ToList();//按照直径升序排列
                    string sss = "";
                    foreach (var ttt in _newgroup)
                    {
                        sss += ttt._diameter.ToString() + ",";
                    }
                    sss.TrimEnd(',');

                    //根据直径种类选择和数量区间选择来选择构件包
                    if (PickDiameter(_group.Count) && PickNum(_banglist))//Count：直径种类的数量
                    {
                        dt_z.Rows.Add(_index, item.elementIndex, item.projectName, item.assemblyName, item.elementName, _num, _weight, _group.Count, sss, _maxlength, _minlength);
                        _index++;
                    }

                }

            }
            dataGridView3.DataSource = dt_z;
            //dataGridView3.Columns[3].DefaultCellStyle.Format = "0.000";        //
            dataGridView3.Columns[6].DefaultCellStyle.Format = "0.00";          //

            GeneralClass.interactivityData?.printlog(1, "所有构件包解析完成");

        }
        /// <summary>
        /// 按照checkbox选项组的勾选情况，选择不同直径种类的
        /// </summary>
        /// <param name="_diametertype"></param>
        /// <returns></returns>
        private bool PickDiameter(int _diametertype)
        {
            switch (_diametertype)
            {
                case 1:
                    return checkBox9.Checked ? true : false;
                case 2:
                    return checkBox10.Checked ? true : false;
                case 3:
                    return checkBox11.Checked ? true : false;
                case 4:
                    return checkBox12.Checked ? true : false;
                case 5:
                    return checkBox13.Checked ? true : false;
                case 6:
                    return checkBox14.Checked ? true : false;
                case 7:
                    return checkBox15.Checked ? true : false;
                case 8:
                    return checkBox16.Checked ? true : false;
                case 9:
                    return checkBox17.Checked ? true : false;
                default: return false;
            }

        }
        /// <summary>
        /// 筛选棒材,根据checkbox状态筛选是否弯曲，是否套丝，是否标化非标
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        private List<RebarData> PickNewList(List<RebarData> _list)
        {
            List<RebarData> _newlist = new List<RebarData>();

            _newlist = _list.Where(t =>
           ((checkBox2.Checked) ? (t.Diameter > 0) : (t.Diameter >= 16)) &&
           (((checkBox5.Checked) ? (t.IfBend) : false) || ((checkBox6.Checked) ? (!t.IfBend) : false)) &&
           (((checkBox7.Checked) ? (t.IfTao) : false) || ((checkBox8.Checked) ? (!t.IfTao) : false)) &&
           (((checkBox22.Checked) ? (t.Length == "9000" || t.Length == "12000" || t.Length == "3000" || t.Length == "4000" || t.Length == "5000" || t.Length == "6000" || t.Length == "7000") : false)
                || ((checkBox23.Checked) ? (t.Length != "9000" && t.Length != "12000" && t.Length != "3000" && t.Length != "4000" && t.Length != "5000" && t.Length != "6000" && t.Length != "7000") : false))
           ).ToList();//筛选棒材

            return _newlist;
        }
        /// <summary>
        /// 根据构件包的钢筋数量区间进行筛选
        /// </summary>
        /// <returns></returns>
        private bool PickNum(List<RebarData> _list)
        {
            int _num = _list.Sum(t => t.TotalPieceNum);

            bool _flag = (checkBox18.Checked ? (_num <= GeneralClass.wareArea[0]) : false)
                        || (checkBox19.Checked ? (_num > GeneralClass.wareArea[0] && _num <= GeneralClass.wareArea[1]) : false)
                        || (checkBox20.Checked ? (_num > GeneralClass.wareArea[1] && _num <= GeneralClass.wareArea[2]) : false)
                        || (checkBox21.Checked ? (_num > GeneralClass.wareArea[2]) : false);

            return _flag;
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                //点击dgv3每个构件包时，在dgv4中显示其详细信息
                if (e.RowIndex > -1)
                {
                    int _index = Convert.ToInt32(dataGridView3.Rows[e.RowIndex].Cells[1].Value.ToString());
                    string _project = dataGridView3.Rows[e.RowIndex].Cells[2].Value.ToString();
                    string _assembly = dataGridView3.Rows[e.RowIndex].Cells[3].Value.ToString();
                    string _element = dataGridView3.Rows[e.RowIndex].Cells[4].Value.ToString();

                    foreach (var item in GeneralClass.AllElementList)
                    {
                        if (item.projectName == _project && item.assemblyName == _assembly && item.elementName == _element && item.elementIndex == _index)
                        {
                            //var newlist = item.rebarlist.Where(t => (checkBox2.Checked ? (t.Diameter > 0) : (t.Diameter >= 16))).ToList();//是否显示线材
                            var newlist = PickNewList(item.rebarlist);

                            Form2.FillDGVWithRebarList(newlist, dataGridView4);

                            dataGridView4.Columns[5].Visible = checkBox3.Checked;

                        }
                    }

                }
            }
            catch (Exception ex) { MessageBox.Show("dataGridView3_CellClick error:" + ex.Message); }

        }

        private void CreatWorkBill(List<List<ElementDataFB>>[] _elementlist)
        {
            WorkBillMsg wbMsg = new WorkBillMsg();      //工单信息
            //SingleRebarMsg srMsg = new SingleRebarMsg();//单段钢筋信息
            List<RebarData> _list = new List<RebarData>();
            List<GroupbyDiameterListWithLength> _group = new List<GroupbyDiameterListWithLength>();

            for (int i = 0; i < (int)EnumWareNumGroup.maxNum; i++)/*仓*/
            {
                foreach (var item in _elementlist[i])/*批次*/
                {
                    _group = new List<GroupbyDiameterListWithLength>();

                    foreach (var iii in item)/*构件包*/
                    {
                        foreach (var ttt in iii.diameterGroup)/*直径分组*/
                        {
                            _group.Add(ttt);//汇总一起
                        }
                    }

                    var _newgroup = _group.GroupBy(x => x._diameter).ToList();//在一个批次内部，按照直径分类
                    foreach (var eee in _newgroup)       //不同直径
                    {
                        var _grouplist = eee.ToList();

                        _list.Clear();
                        foreach (var mmm in _grouplist)
                        {
                            _list.AddRange(mmm._datalist);//准备好rebardata的list，准备进行长度套料
                        }
                        foreach(var kkk in _list)//修改curChildBatch
                        {
                            BatchMsg batchmsg = new BatchMsg();
                            batchmsg = kkk.BatchMsg;
                            batchmsg.curChildBatch = _newgroup.IndexOf(eee);//修改完，再存回去
                            kkk.BatchMsg = batchmsg;
                        }
                        int totallength = 0;
                        /*长度套料后生成的钢筋原材list*/
                        var _newlist = Algorithm.Taoliao(_list, out totallength);//套料时顺便算一下总长度

                        foreach (var aaa in _newlist)
                        {
                            wbMsg.shift = 1;
                            wbMsg.projectName = "光谷国际社区";
                            wbMsg.block = "A";
                            wbMsg.building = "06D";
                            wbMsg.floor = "01F";
                            wbMsg.level = "C";//钢筋级别
                            wbMsg.brand = "鄂钢";//厂商
                            wbMsg.specification = "HRB400";//规格型号
                            wbMsg.originLength = GeneralClass.OriginalLength2;
                            wbMsg.totalOriginal = _newlist.Count;//总的原材根数
                            wbMsg.curOriginal = _newlist.IndexOf(aaa);//当前的原材流水号

                            string jsonstr = GeneralClass.WorkBillOpt.CreateWorkBill(wbMsg, aaa);
                            GeneralClass.jsonList.Add(jsonstr);
                        }
                        //更新dgv11
                        string diameterStr = "";
                        if (item[0].diameterList.Count > 2)
                        {
                            diameterStr = "多直径";
                        }
                        else if (item[0].diameterList.Count == 2)
                        {
                            diameterStr = "两直径";
                        }
                        else
                        {
                            diameterStr = "单直径";
                        }
                        //直径种类，仓位，批次，直径，总数量，总长度，利用率
                        dt_wb.Rows.Add(diameterStr,
                            GeneralClass.wareNum[i],
                            _elementlist[i].IndexOf(item),
                            eee.Key,
                            _newlist.Count,
                            (double)totallength / 1000,
                            (double)totallength / (double)(_newlist.Count * GeneralClass.OriginalLength2));


                    }


                }
            }

        }

        /// <summary>
        /// 工单datatable
        /// </summary>
        private DataTable dt_wb = new DataTable();

        private void button2_Click(object sender, EventArgs e)
        {
            if (_multiWorklist[0].Count == 0 && _oneWorklist[0].Count == 0 && _twoWorklist[0].Count == 0)
            {
                MessageBox.Show("工单数据未准备好，请先点击【组合匹配】");
                return;
            }
            GeneralClass.interactivityData?.printlog(1, "开始创建工单");

            dt_wb = new DataTable();
            dt_wb.Columns.Add("直径种类", typeof(string));
            dt_wb.Columns.Add("仓位", typeof(int));
            dt_wb.Columns.Add("批次", typeof(int));
            dt_wb.Columns.Add("直径", typeof(int));
            dt_wb.Columns.Add("原材数量", typeof(int));
            dt_wb.Columns.Add("总长度(m)", typeof(double));
            dt_wb.Columns.Add("利用率(%)", typeof(double));


            GeneralClass.jsonList.Clear();
            CreatWorkBill(_oneWorklist);//单直径worklist
            CreatWorkBill(_twoWorklist);//双直径worklist
            CreatWorkBill(_multiWorklist);//多直径worklist

            GeneralClass.interactivityData?.printlog(1, "创建工单完成");


            dataGridView11.DataSource = dt_wb;
            //dataGridView11.Columns[1].DefaultCellStyle.Format = "0.000";        //
            //dataGridView11.Columns[2].DefaultCellStyle.Format = "0.0";        //
            //dataGridView11.Columns[3].DefaultCellStyle.Format = "0.0";          //
            dataGridView11.Columns[6].DefaultCellStyle.Format = "P1";          //

            int totalpiece = Convert.ToInt32(dt_wb.Compute("sum(原材数量)", ""));
            double totallength = Convert.ToInt32(dt_wb.Compute("sum([总长度(m)])", ""));

            textBox2.Text = totalpiece.ToString();
            textBox1.Text = totallength.ToString("F2");
            textBox3.Text = (totallength / ((double)GeneralClass.OriginalLength2 / 1000 * (double)totalpiece)).ToString("P1");

        }


        List<int> _randomlist = new List<int>();
        /// <summary>
        /// 已经经过降序排列的数据list
        /// </summary>
        List<GeneralTest> _sortlist = new List<GeneralTest>();
        private void button3_Click(object sender, EventArgs e)
        {
            _randomlist.Clear();

            int count = 1000;
            Random rnd = new Random();
            while (count > 0)
            {
                _randomlist.Add(rnd.Next(100, 12000));

                count--;
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("索引", typeof(int));
            dt.Columns.Add("长度", typeof(int));

            for (int i = 0; i < _randomlist.Count; i++)
            {
                dt.Rows.Add(i, _randomlist[i]);
            }

            dataGridView5.DataSource = dt;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<int> _sortlist = _randomlist.OrderByDescending(item => item).ToList();

            this._sortlist.Clear();

            DataTable dt = new DataTable();
            dt.Columns.Add("索引", typeof(int));
            dt.Columns.Add("长度", typeof(int));

            for (int i = 0; i < _sortlist.Count; i++)
            {
                dt.Rows.Add(i, _sortlist[i]);

                this._sortlist.Add(new GeneralTest(i, _sortlist[i]));
            }

            dataGridView6.DataSource = dt;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("索引", typeof(int));
            dt.Columns.Add("长度1", typeof(int));
            dt.Columns.Add("索引", typeof(int));
            dt.Columns.Add("长度2", typeof(int));
            dt.Columns.Add("索引", typeof(int));
            dt.Columns.Add("长度3", typeof(int));

            List<List<GeneralTest>> _algogroup = new List<List<GeneralTest>>();

            for (int i = 0; i < _sortlist.Count; i++)
            {
                if (!_sortlist[i].ifuse)//未使用过
                {
                    //GeneralClass.OriginalLength1 -;


                }

            }


            dataGridView7.DataSource = dt;
        }

        /// <summary>
        /// 多直径种类构件包分组后的工单，分三个维度：[]维度代表仓位区间，list(外)维度代表直径种类，List(内)维度代表所包含的构件包list
        /// </summary>
        List<List<ElementDataFB>>[] _multiWorklist = new List<List<ElementDataFB>>[(int)EnumWareNumGroup.maxNum] {
            new List<List<ElementDataFB>>(),
            new List<List<ElementDataFB>>(),
            new List<List<ElementDataFB>>(),
            new List<List<ElementDataFB>>()};//分组后的构件包，按照数量仓位分组
        /// <summary>
        /// 单直径种类构件包分组后的工单
        /// </summary>
        List<List<ElementDataFB>>[] _oneWorklist = new List<List<ElementDataFB>>[(int)EnumWareNumGroup.maxNum] {
            new List<List<ElementDataFB>>(),
            new List<List<ElementDataFB>>(),
            new List<List<ElementDataFB>>(),
            new List<List<ElementDataFB>>()};//分组后的构件包，按照数量仓位分组
        /// <summary>
        /// 两直径种类构件包分组后的工单
        /// </summary>
        List<List<ElementDataFB>>[] _twoWorklist = new List<List<ElementDataFB>>[(int)EnumWareNumGroup.maxNum] {
            new List<List<ElementDataFB>>(),
            new List<List<ElementDataFB>>(),
            new List<List<ElementDataFB>>(),
            new List<List<ElementDataFB>>()};//分组后的构件包，按照数量仓位分组


        private void button6_Click(object sender, EventArgs e)
        {
            GeneralClass.interactivityData?.printlog(1, "开始进行构件包匹配");

            GeneralClass.AllElementList = GeneralClass.SQLiteOpt.GetAllElementList(GeneralClass.AllRebarTableName);

            List<ElementDataFB> _fblist = new List<ElementDataFB>();

            foreach (var item in GeneralClass.AllElementList)
            {
                item.Group();//先进行各个构件包的分组处理

                _fblist.Add(item.elementDataFB);//单独存入构件包非标list
            }

            //行：直径种类，1种，2种，3种，4种，5种，。。。10种
            //列：数量仓位,EIGHT:1~10(8仓)，FOUR:11~50(4仓)，TWO:51~100(2仓)，ONE:100~(1仓)
            List<ElementDataFB>[,] fb_diameterGroup = new List<ElementDataFB>[(int)EnumRebarBang.maxRebarBangNum, (int)EnumWareNumGroup.maxNum];//待分组的构件包

            for (int i = 0; i < (int)EnumRebarBang.maxRebarBangNum; i++)
            {
                for (int j = 0; j < (int)EnumWareNumGroup.maxNum; j++)
                {
                    fb_diameterGroup[i, j] = _fblist.Where(t => t.diameterType == i + 1 && t.numGroup == (EnumWareNumGroup)j).ToList();
                    fb_diameterGroup[i, j] = fb_diameterGroup[i, j].OrderBy(t => t.diameterStr).ToList();
                }
            }


            foreach (var item in _multiWorklist)
            { item.Clear(); }
            foreach (var item in _oneWorklist)
            { item.Clear(); }
            foreach (var item in _twoWorklist)
            { item.Clear(); }

            //先组合多直径构件包
            for (int j = 0; j < (int)EnumWareNumGroup.maxNum; j++)
            {
                #region 先处理多直径种类的                
                //先处理多直径种类的
                for (int i = (int)EnumRebarBang.maxRebarBangNum - 1; i > 1; i--)//倒序，从直径种类多的开始分组,
                {
                    if (fb_diameterGroup[i, j].Count == 0) continue;//直径种类太多的一般没有，例如5种以上的

                    for (int k = 0; k < fb_diameterGroup[i, j].Count; k++)
                    {
                        if (_multiWorklist[j].Count == 0)//刚开始没有元素
                        {
                            List<ElementDataFB> temp = new List<ElementDataFB> { fb_diameterGroup[i, j][0] };
                            _multiWorklist[j].Add(temp);//将第一个构件包存入，建立第一个小批次
                        }
                        else
                        {
                            bool bUse = false;
                            for (int m = 0; m < _multiWorklist[j].Count; m++)//是否能够匹配现有的小批次
                            {
                                //条件一：至少包含一种直径，条件二：螺距类型一致，条件三：仓位未满
                                if (fb_diameterGroup[i, j][k].IfIncludeby(_multiWorklist[j][m][0]) &&
                                    _multiWorklist[j][m].Count < GeneralClass.wareNum[j] &&
                                     fb_diameterGroup[i, j][k].diameterPitchType == _multiWorklist[j][m][0].diameterPitchType)//根据直径种类的包含关系分组，同时考虑仓位是否满仓
                                {
                                    _multiWorklist[j][m].Add(fb_diameterGroup[i, j][k]);
                                    bUse = true;
                                }
                            }
                            if (!bUse)//如果匹配不了现有的批次，则建立新的小批次
                            {
                                List<ElementDataFB> temp = new List<ElementDataFB> { fb_diameterGroup[i, j][k] };
                                _multiWorklist[j].Add(temp);
                            }
                        }
                    }
                }
                #endregion
                #region 再处理单直径的
                for (int k = 0; k < fb_diameterGroup[0, j].Count; k++)//0为1种直径的
                {
                    if (_oneWorklist[j].Count == 0)//刚开始没有元素
                    {
                        List<ElementDataFB> temp = new List<ElementDataFB> { fb_diameterGroup[0, j][0] };
                        _oneWorklist[j].Add(temp);//将第一个构件包存入，建立第一个小批次
                    }
                    else
                    {
                        bool bUse = false;
                        for (int m = 0; m < _oneWorklist[j].Count; m++)//是否能够匹配现有的小批次
                        {
                            //条件一：直径一致，条件二：螺距类型一致，条件三：仓位未满
                            if (fb_diameterGroup[0, j][k].IfIncludeby(_oneWorklist[j][m][0])
                                && _oneWorklist[j][m].Count < GeneralClass.wareNum[j]
                                && fb_diameterGroup[0, j][k].diameterPitchType == _oneWorklist[j][m][0].diameterPitchType)//根据直径种类的包含关系分组，同时考虑仓位是否满仓
                            {
                                _oneWorklist[j][m].Add(fb_diameterGroup[0, j][k]);
                                bUse = true;
                            }
                        }
                        if (!bUse)//如果匹配不了现有的批次，则建立新的小批次
                        {
                            List<ElementDataFB> temp = new List<ElementDataFB> { fb_diameterGroup[0, j][k] };
                            _oneWorklist[j].Add(temp);
                        }
                    }
                }
                #endregion

                #region 再处理两种直径的
                for (int k = 0; k < fb_diameterGroup[1, j].Count; k++)//1为2种直径的
                {
                    if (_twoWorklist[j].Count == 0)//刚开始没有元素
                    {
                        List<ElementDataFB> temp = new List<ElementDataFB> { fb_diameterGroup[1, j][0] };
                        _twoWorklist[j].Add(temp);//将第一个构件包存入，建立第一个小批次
                    }
                    else
                    {
                        bool bUse = false;
                        for (int m = 0; m < _twoWorklist[j].Count; m++)//是否能够匹配现有的小批次
                        {
                            //条件一：至少包含一种直径，条件二：螺距类型一致，条件三：仓位未满
                            if (fb_diameterGroup[1, j][k].IfIncludeby(_twoWorklist[j][m][0])
                                && _twoWorklist[j][m].Count < GeneralClass.wareNum[j]
                                && fb_diameterGroup[1, j][k].diameterPitchType == _twoWorklist[j][m][0].diameterPitchType)//根据直径种类的包含关系分组，同时考虑仓位是否满仓
                            {
                                _twoWorklist[j][m].Add(fb_diameterGroup[1, j][k]);
                                bUse = true;
                            }
                        }
                        if (!bUse)//如果匹配不了现有的批次，则建立新的小批次
                        {
                            List<ElementDataFB> temp = new List<ElementDataFB> { fb_diameterGroup[1, j][k] };
                            _twoWorklist[j].Add(temp);
                        }
                    }
                }
                #endregion

            }

            FillTreeView(_multiWorklist, treeView2);
            FillTreeView(_oneWorklist, treeView3);
            FillTreeView(_twoWorklist, treeView4);

            AddWareMsg(ref _oneWorklist);
            AddWareMsg(ref _twoWorklist);
            AddWareMsg(ref _multiWorklist);

            AddBatchMsg(ref _oneWorklist, ref _twoWorklist, ref _multiWorklist);

            GeneralClass.interactivityData?.printlog(1, "构件包匹配完成");

        }
        private void AddBatchMsg(ref List<List<ElementDataFB>>[] _onelist, ref List<List<ElementDataFB>>[] _twolist, ref List<List<ElementDataFB>>[] _multilist)
        {
            BatchMsg _msg = new BatchMsg();
            _msg.totalBatch = 0;
            _msg.curBatch = 0;
            _msg.totalchildBatch = 0;//子批次,按照直径来分
            _msg.curChildBatch = 0;

            int _totalbatch = 0;
            int _batchNo = 0;
            int _totalChildBatch = 0;
            int _chilBatchNo = 0;

            List<GroupbyDiameterListWithLength> _dlist= new List<GroupbyDiameterListWithLength>();

            //先统计总共多少个批次
            for (int i = 0; i < (int)EnumWareNumGroup.maxNum; i++)/*仓*/
            {
                foreach (var item in _onelist[i])/*批次*/
                {
                    _totalbatch++;                    
                }
            }
            for (int i = 0; i < (int)EnumWareNumGroup.maxNum; i++)/*仓*/
            {
                foreach (var item in _twolist[i])/*批次*/
                {
                    _totalbatch++;
                }
            }
            for (int i = 0; i < (int)EnumWareNumGroup.maxNum; i++)/*仓*/
            {
                foreach (var item in _multilist[i])/*批次*/
                {
                    _totalbatch++;
                }
            }

            //更新批次信息
            for (int i = 0; i < (int)EnumWareNumGroup.maxNum; i++)/*仓*/
            {
                foreach (var item in _onelist[i])/*批次*/
                {
                    _batchNo++;
                    _msg.totalchildBatch = item.Max(t => t.diameterType);//直径种类的最大值为子批次总数
                    foreach (var iii in item)/*构件包*/
                    {
                        foreach (var ttt in iii.diameterGroup)/*直径分组*/
                        {
                            foreach (var eee in ttt._datalist)/*钢筋*/
                            {
                                _msg.totalBatch = _totalbatch;
                                _msg.curBatch = _batchNo;
                                eee.BatchMsg = _msg;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < (int)EnumWareNumGroup.maxNum; i++)/*仓*/
            {
                foreach (var item in _twolist[i])/*批次*/
                {
                    _batchNo++;
                    _msg.totalchildBatch = item.Max(t => t.diameterType);//直径种类的最大值为子批次总数

                    foreach (var iii in item)/*构件包*/
                    {
                        foreach (var ttt in iii.diameterGroup)/*直径分组*/
                        {
                            foreach (var eee in ttt._datalist)/*钢筋*/
                            {
                                _msg.totalBatch = _totalbatch;
                                _msg.curBatch = _batchNo;
                                eee.BatchMsg = _msg;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < (int)EnumWareNumGroup.maxNum; i++)/*仓*/
            {
                foreach (var item in _multilist[i])/*批次*/
                {
                    _batchNo++;
                    _msg.totalchildBatch = item.Max(t => t.diameterType);//直径种类的最大值为子批次总数

                    foreach (var iii in item)/*构件包*/
                    {
                        foreach (var ttt in iii.diameterGroup)/*直径分组*/
                        {
                            foreach (var eee in ttt._datalist)/*钢筋*/
                            {
                                _msg.totalBatch = _totalbatch;
                                _msg.curBatch = _batchNo;
                                eee.BatchMsg = _msg;
                            }
                        }
                    }
                }
            }

        }
        /// <summary>
        /// 为三个worklist添加waremsg(仓储信息)到rebardata里面
        /// 仓储信息规则如下：
        /// 1、总共四条通道，通道1和通道2负责弯曲的，通道3和通道4负责非弯曲的；
        /// 2、每条通道按照8421仓来进行切换
        /// 3、
        /// </summary>
        /// <param name="_worklist"></param>
        private void AddWareMsg(ref List<List<ElementDataFB>>[] _worklist)
        {
            WareMsg _msg = new WareMsg();
            _msg.channel = 1;
            _msg.totalware = EnumWareNumGroup.NONE;
            _msg.wareno = 1;

            for (int i = 0; i < (int)EnumWareNumGroup.maxNum; i++)/*仓*/
            {
                _msg.totalware = (EnumWareNumGroup)i;

                foreach (var item in _worklist[i])/*批次*/
                {
                    foreach (var iii in item)/*构件包*/
                    {
                        int _index = item.IndexOf(iii);//第几个构件包就是第几个仓位
                        _msg.wareno = _index % GeneralClass.wareNum[i] + 1;

                        foreach (var ttt in iii.diameterGroup)/*直径分组*/
                        {
                            foreach (var eee in ttt._datalist)/*钢筋*/
                            {
                                if (eee.IfBend)
                                {
                                    _msg.channel = _index / GeneralClass.wareNum[i] + 1;
                                }
                                else
                                {
                                    _msg.channel = _index / GeneralClass.wareNum[i] + 2;
                                }
                                eee.WareMsg = _msg;//重新赋值仓储信息
                            }
                        }
                    }
                }

            }



        }

        private void FillTreeView(List<List<ElementDataFB>>[] _list, TreeView _tv)
        {
            TreeNode tn1 = null;
            TreeNode tn2 = null;
            TreeNode tn3 = null;

            _tv.Nodes.Clear();
            for (int i = 0; i < _list.Count(); i++)
            {
                tn1 = new TreeNode();
                tn1.Text = GeneralClass.wareNum[i].ToString() + "仓";
                for (int j = 0; j < _list[i].Count; j++)
                {
                    tn2 = new TreeNode();
                    tn2.Text = "批次" + j.ToString();

                    for (int k = 0; k < _list[i][j].Count; k++)
                    {
                        tn3 = new TreeNode();
                        tn3.Text = "构件包" + k.ToString();
                        tn2.Nodes.Add(tn3);
                    }
                    tn1.Nodes.Add(tn2);
                }
                _tv.Nodes.Add(tn1);
            }
        }


        private void dataGridView8_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                //点击dgv8每个构件包时，在dgv9中显示其详细信息
                if (e.RowIndex > -1)
                {
                    int _index = Convert.ToInt32(dataGridView8.Rows[e.RowIndex].Cells[1].Value.ToString());
                    string _project = dataGridView8.Rows[e.RowIndex].Cells[2].Value.ToString();
                    string _assembly = dataGridView8.Rows[e.RowIndex].Cells[3].Value.ToString();
                    string _element = dataGridView8.Rows[e.RowIndex].Cells[4].Value.ToString();

                    if (tabControl2.SelectedIndex == 0)
                    {
                        foreach (var item in _multiShowlist)
                        {
                            if (item.projectName == _project && item.assemblyName == _assembly && item.elementName == _element && item.elementIndex == _index)
                            {
                                List<RebarData> newlist = new List<RebarData>();

                                foreach (var ttt in item.diameterGroup)
                                {
                                    newlist.AddRange(ttt._datalist);
                                }

                                Form2.FillDGVWithRebarList(newlist, dataGridView9);
                                dataGridView9.Columns[5].Visible = checkBox3.Checked;

                            }
                        }
                    }
                    else if (tabControl2.SelectedIndex == 1)
                    {
                        foreach (var item in _oneShowlist)
                        {
                            if (item.projectName == _project && item.assemblyName == _assembly && item.elementName == _element && item.elementIndex == _index)
                            {
                                List<RebarData> newlist = new List<RebarData>();

                                foreach (var ttt in item.diameterGroup)
                                {
                                    newlist.AddRange(ttt._datalist);
                                }

                                Form2.FillDGVWithRebarList(newlist, dataGridView9);
                                dataGridView9.Columns[5].Visible = checkBox3.Checked;

                            }
                        }
                    }
                    else
                    {
                        foreach (var item in _twoShowlist)
                        {
                            if (item.projectName == _project && item.assemblyName == _assembly && item.elementName == _element && item.elementIndex == _index)
                            {
                                List<RebarData> newlist = new List<RebarData>();

                                foreach (var ttt in item.diameterGroup)
                                {
                                    newlist.AddRange(ttt._datalist);
                                }

                                Form2.FillDGVWithRebarList(newlist, dataGridView9);
                                dataGridView9.Columns[5].Visible = checkBox3.Checked;

                            }
                        }
                    }


                }
            }
            catch (Exception ex) { MessageBox.Show("dataGridView8_CellClick error:" + ex.Message); }

        }


        List<ElementDataFB> _multiShowlist = new List<ElementDataFB>();
        List<ElementDataFB> _oneShowlist = new List<ElementDataFB>();
        List<ElementDataFB> _twoShowlist = new List<ElementDataFB>();


        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse || e.Action == TreeViewAction.ByKeyboard)
            {

                _multiShowlist = new List<ElementDataFB>();

                if (e.Node.Level == 0)
                {
                    for (int i = 0; i < _multiWorklist[e.Node.Index].Count; i++)
                    {
                        for (int j = 0; j < _multiWorklist[e.Node.Index][i].Count; j++)
                        {
                            _multiShowlist.Add(_multiWorklist[e.Node.Index][i][j]);
                        }
                    }
                    ShowElementAddData(_multiShowlist, dataGridView10);
                }
                else if (e.Node.Level == 1)
                {
                    for (int i = 0; i < _multiWorklist[e.Node.Parent.Index][e.Node.Index].Count; i++)
                    {
                        _multiShowlist.Add(_multiWorklist[e.Node.Parent.Index][e.Node.Index][i]);

                    }
                    ShowElementAddData(_multiShowlist, dataGridView10);
                }
                else
                {
                    _multiShowlist.Add(_multiWorklist[e.Node.Parent.Parent.Index][e.Node.Parent.Index][e.Node.Index]);
                    ShowElementAddData(_multiShowlist, dataGridView10);
                }


                DataTable dt_z = new DataTable();
                dt_z.Columns.Add("索引", typeof(int));
                dt_z.Columns.Add("db序号", typeof(int));
                dt_z.Columns.Add("项目名称", typeof(string));
                dt_z.Columns.Add("主构件名", typeof(string));
                dt_z.Columns.Add("子构件名", typeof(string));
                dt_z.Columns.Add("总数量", typeof(int));
                dt_z.Columns.Add("总重量(kg)", typeof(double));
                dt_z.Columns.Add("直径种类", typeof(int));
                dt_z.Columns.Add("直径组合(Φ)", typeof(string));
                //dt_z.Columns.Add("最大长度", typeof(int));
                //dt_z.Columns.Add("最小长度", typeof(int));

                int _index = 0;
                foreach (var item in _multiShowlist)
                {
                    var _newgroup = item.diameterList.OrderBy(x => x).ToList();//按照直径升序排列
                    string sss = "";
                    foreach (var ttt in _newgroup)
                    {
                        sss += ttt.ToString().Substring(6, 2) + ",";
                    }
                    sss.TrimEnd(',');

                    dt_z.Rows.Add(_index, item.elementIndex, item.projectName, item.assemblyName, item.elementName, item.totalNum, item.totalweight, item.diameterType, sss);
                    _index++;
                }

                dataGridView8.DataSource = dt_z;
                dataGridView8.Columns[6].DefaultCellStyle.Format = "0.00";          //

            }
        }

        private void treeView3_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse || e.Action == TreeViewAction.ByKeyboard)
            {

                _oneShowlist = new List<ElementDataFB>();

                if (e.Node.Level == 0)
                {
                    for (int i = 0; i < _oneWorklist[e.Node.Index].Count; i++)
                    {
                        for (int j = 0; j < _oneWorklist[e.Node.Index][i].Count; j++)
                        {
                            _oneShowlist.Add(_oneWorklist[e.Node.Index][i][j]);
                        }
                    }
                    ShowElementAddData(_oneShowlist, dataGridView10);
                }
                else if (e.Node.Level == 1)
                {
                    for (int i = 0; i < _oneWorklist[e.Node.Parent.Index][e.Node.Index].Count; i++)
                    {
                        _oneShowlist.Add(_oneWorklist[e.Node.Parent.Index][e.Node.Index][i]);

                    }
                    ShowElementAddData(_oneShowlist, dataGridView10);
                }
                else
                {
                    _oneShowlist.Add(_oneWorklist[e.Node.Parent.Parent.Index][e.Node.Parent.Index][e.Node.Index]);
                    ShowElementAddData(_oneShowlist, dataGridView10);
                }


                DataTable dt_z = new DataTable();
                dt_z.Columns.Add("索引", typeof(int));
                dt_z.Columns.Add("db序号", typeof(int));
                dt_z.Columns.Add("项目名称", typeof(string));
                dt_z.Columns.Add("主构件名", typeof(string));
                dt_z.Columns.Add("子构件名", typeof(string));
                dt_z.Columns.Add("总数量", typeof(int));
                dt_z.Columns.Add("总重量(kg)", typeof(double));
                dt_z.Columns.Add("直径种类", typeof(int));
                dt_z.Columns.Add("直径组合(Φ)", typeof(string));
                //dt_z.Columns.Add("最大长度", typeof(int));
                //dt_z.Columns.Add("最小长度", typeof(int));

                int _index = 0;
                foreach (var item in _oneShowlist)
                {
                    var _newgroup = item.diameterList.OrderBy(x => x).ToList();//按照直径升序排列
                    string sss = "";
                    foreach (var ttt in _newgroup)
                    {
                        sss += ttt.ToString().Substring(6, 2) + ",";
                    }
                    sss.TrimEnd(',');

                    dt_z.Rows.Add(_index, item.elementIndex, item.projectName, item.assemblyName, item.elementName, item.totalNum, item.totalweight, item.diameterType, sss);
                    _index++;
                }

                dataGridView8.DataSource = dt_z;
                dataGridView8.Columns[6].DefaultCellStyle.Format = "0.00";          //

            }

        }

        private void treeView4_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse || e.Action == TreeViewAction.ByKeyboard)
            {

                _twoShowlist = new List<ElementDataFB>();

                if (e.Node.Level == 0)
                {
                    for (int i = 0; i < _twoWorklist[e.Node.Index].Count; i++)
                    {
                        for (int j = 0; j < _twoWorklist[e.Node.Index][i].Count; j++)
                        {
                            _twoShowlist.Add(_twoWorklist[e.Node.Index][i][j]);
                        }
                    }
                    ShowElementAddData(_twoShowlist, dataGridView10);
                }
                else if (e.Node.Level == 1)
                {
                    for (int i = 0; i < _twoWorklist[e.Node.Parent.Index][e.Node.Index].Count; i++)
                    {
                        _twoShowlist.Add(_twoWorklist[e.Node.Parent.Index][e.Node.Index][i]);

                    }
                    ShowElementAddData(_twoShowlist, dataGridView10);
                }
                else
                {
                    _twoShowlist.Add(_twoWorklist[e.Node.Parent.Parent.Index][e.Node.Parent.Index][e.Node.Index]);
                    ShowElementAddData(_twoShowlist, dataGridView10);
                }


                DataTable dt_z = new DataTable();
                dt_z.Columns.Add("索引", typeof(int));
                dt_z.Columns.Add("db序号", typeof(int));
                dt_z.Columns.Add("项目名称", typeof(string));
                dt_z.Columns.Add("主构件名", typeof(string));
                dt_z.Columns.Add("子构件名", typeof(string));
                dt_z.Columns.Add("总数量", typeof(int));
                dt_z.Columns.Add("总重量(kg)", typeof(double));
                dt_z.Columns.Add("直径种类", typeof(int));
                dt_z.Columns.Add("直径组合(Φ)", typeof(string));
                //dt_z.Columns.Add("最大长度", typeof(int));
                //dt_z.Columns.Add("最小长度", typeof(int));

                int _index = 0;
                foreach (var item in _twoShowlist)
                {
                    var _newgroup = item.diameterList.OrderBy(x => x).ToList();//按照直径升序排列
                    string sss = "";
                    foreach (var ttt in _newgroup)
                    {
                        sss += ttt.ToString().Substring(6, 2) + ",";
                    }
                    sss.TrimEnd(',');

                    dt_z.Rows.Add(_index, item.elementIndex, item.projectName, item.assemblyName, item.elementName, item.totalNum, item.totalweight, item.diameterType, sss);
                    _index++;
                }

                dataGridView8.DataSource = dt_z;
                dataGridView8.Columns[6].DefaultCellStyle.Format = "0.00";          //

            }

        }
    }
}
