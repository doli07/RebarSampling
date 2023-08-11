using Etable;
using Grand;
using NPOI.SS.Formula.Functions;
using System;
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
        private void InitCheckBox()
        {
            checkBox1.Checked = true;
            checkBox2.Checked = false;

            checkBox3.Checked = false;
            checkBox4.Checked = true;
        }
        private void InitDataGridView()
        {
            Form2.InitDGV(dataGridView1);
            Form2.InitDGV(dataGridView4);
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
                                //选择是否显示线材
                                var newlist = item.rebarlist.Where(t => (checkBox2.Checked ? (t.Diameter > 0) : (t.Diameter >= 16))).ToList();
                                //Form2.FillDGVWithRebarList(item.rebarlist, dataGridView1);
                                Form2.FillDGVWithRebarList(newlist, dataGridView1);
                                ShowElementAddData(item.rebarlist);
                                checkBox3.Checked = false;
                                dataGridView1.Columns[5].Visible = false;

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
            dt_z.Columns.Add("总数量", typeof(int));
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

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Columns[5].Visible = checkBox3.Checked;
            dataGridView4.Columns[5].Visible = checkBox3.Checked;

        }

        private void button1_Click(object sender, EventArgs e)
        {

            //解析钢筋总表的所有构件

            GeneralClass.interactivityData?.printlog(1, "开始解析所有料单的构件包");

            GeneralClass.AllElementList = GeneralClass.SQLiteOpt.GetAllElementList(GeneralClass.AllRebarTableName);

            DataTable dt_z = new DataTable();
            dt_z.Columns.Add("索引", typeof(int));
            dt_z.Columns.Add("项目名称", typeof(string));
            dt_z.Columns.Add("主构件名", typeof(string));
            dt_z.Columns.Add("子构件名", typeof(string));
            dt_z.Columns.Add("总数量", typeof(int));
            dt_z.Columns.Add("总重量(kg)", typeof(double));
            dt_z.Columns.Add("直径种类", typeof(int));
            dt_z.Columns.Add("最大长度", typeof(int));
            dt_z.Columns.Add("最小长度", typeof(int));


            int _num = 0;
            double _weight = 0;
            int _maxlength, _minlength = 0;
            foreach (var item in GeneralClass.AllElementList)
            {
                var _banglist = item.rebarlist.Where(t => t.Diameter >= 16).ToList();//筛选棒材

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

                    dt_z.Rows.Add(item.elementIndex, item.projectName, item.assemblyName, item.elementName, _num, _weight, _group.Count, _maxlength, _minlength);

                }

            }
            dataGridView3.DataSource = dt_z;
            //dataGridView3.Columns[3].DefaultCellStyle.Format = "0.000";        //
            dataGridView3.Columns[5].DefaultCellStyle.Format = "0.00";          //

            GeneralClass.interactivityData?.printlog(1, "所有构件包解析完成");

        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                //点击dgv3每个构件包时，在dgv4中显示其详细信息
                if (e.RowIndex > -1)
                {
                    string _project = dataGridView3.Rows[e.RowIndex].Cells[1].Value.ToString();
                    string _assembly = dataGridView3.Rows[e.RowIndex].Cells[2].Value.ToString();
                    string _element = dataGridView3.Rows[e.RowIndex].Cells[3].Value.ToString();
                    int _index = Convert.ToInt32(dataGridView3.Rows[e.RowIndex].Cells[0].Value.ToString());

                    foreach (var item in GeneralClass.AllElementList)
                    {
                        if (item.projectName == _project && item.assemblyName == _assembly && item.elementName == _element && item.elementIndex == _index)
                        {
                            var newlist = item.rebarlist.Where(t => (checkBox2.Checked ? (t.Diameter > 0) : (t.Diameter >= 16))).ToList();//是否显示线材
                                                                                                                                          //Form2.FillDGVWithRebarList(item.rebarlist, dataGridView4);
                            Form2.FillDGVWithRebarList(newlist, dataGridView4);

                            dataGridView4.Columns[5].Visible = false;

                        }
                    }

                }
            }
            catch (Exception ex) { MessageBox.Show("dataGridView3_CellClick error:" + ex.Message); }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<ElementData> _elements = new List<ElementData>();

            ElementData _elem = new ElementData();

            GeneralClass.interactivityData?.printlog(1, "1");

            for (int i = 0; i < 200000; i++)
            {
                _elem = new ElementData();
                _elements.Add(_elem);

            }
            GeneralClass.interactivityData?.printlog(1, "2");

            List<KeyValuePair<int, ElementData>> _pairs = new List<KeyValuePair<int, ElementData>>();

            int _index = 0;
            //foreach(var item in _elements)
            //{
            //    _pairs.Add(new KeyValuePair<int, ElementData>(_index, item));
            //    _index++;
            //}
            for (int i = 0; i < _elements.Count; i++)
            {
                _pairs.Add(new KeyValuePair<int, ElementData>(i, _elements[i]));

            }
            GeneralClass.interactivityData?.printlog(1, "3");

            for (int i = 0; i < _pairs.Count; i++)
            {
                ElementData tt = _pairs[i].Value;
            }
            //foreach(var item in _elements)
            //{
            //    item.elementIndex = _elements.IndexOf(item);
            //}
            GeneralClass.interactivityData?.printlog(1, "4");


        }
    }
}
