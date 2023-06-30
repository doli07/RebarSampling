using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Threading;
using System.Data.SQLite;
using System.Runtime.Remoting.Messaging;
using NPOI.XWPF.UserModel;
using ICell = NPOI.SS.UserModel.ICell;
using System.Data.Common;
using NPOI.OpenXmlFormats.Spreadsheet;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Linq;
using System.Collections;

namespace RebarSampling
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //Font font = new Font(textBox1.Font.FontFamily, float.Parse(textBox1.Text), textBox1.Font.Style);
            //Font font = new Font("⿊体", 15);

            GeneralClass.interactivityData.printlog += LogAdd;
            GeneralClass.interactivityData.ifRebarSelected += IfRebarSelected;
            GeneralClass.interactivityData.ifRebarChecked += IfRebarChecked;
        }
        private void Form1_Load(object sender, EventArgs e)
        {


            InitChecklist1();
            InitTreeView1();


            string filepath = Application.StartupPath + @"\dbfile\" + GeneralClass.AllRebarDBfileName + ".db";

            GeneralClass.SQLiteOpt.ConnectDB(filepath); //无密码连接

            //imageList1.ImageSize = new Size(177, 42);
            //pictureBox1.Image = imageList1.Images[0];

        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //GeneralClass.readEXCEL.CloseFile();//关闭文件流

        }



        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //string filepath = "";

                //打开excel文件
                OpenFileDialog openFileDialog = new OpenFileDialog();//打开文件夹对话框
                openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                openFileDialog.Filter = "excel文件|*.xls;*.xlsx|所有文件|*.*";
                openFileDialog.Title = "打开料单";
                openFileDialog.FilterIndex = 0;
                openFileDialog.Multiselect = true;
                openFileDialog.CheckFileExists = true;
                openFileDialog.CheckPathExists = true;


                treeView1.Nodes.Clear();
                //InitDGV_BX();
                //InitDGV_Xian();
                //InitDGV_BangOri();
                //InitDGV_BangNoOri();
                GeneralClass.interactivityData?.initStatisticsDGV();//清空统计界面的dgv

                //GeneralClass.SQLiteOpt.InitDB(GeneralClass.AllRebarTableName);//先创建并清空db数据库表单
                GeneralClass.SQLiteOpt.InitDB(GeneralClass.AllRebarBKTableName);//先创建并清空db数据库表单

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string filepath in openFileDialog.FileNames)
                    {

                        string filename = System.IO.Path.GetFileNameWithoutExtension(filepath); //获取excel文件名作为根节点名称,不带后缀名

                        //GeneralClass.readEXCEL.OpenFile(filepath);


                        this.toolStripStatusLabel1.Text = filepath;//显示文件名称

                        string tableName = GeneralClass.AllRebarBKTableName;

                        GeneralClass.interactivityData?.printlog(1, "开始导入料单至数据库文件，请等待。。。");
                        //GeneralClass.SQLiteOpt.ExcelToDB(filename, tableName);//excel文件数据存入数据库
                        GeneralClass.SQLiteOpt.ExcelToDB(filepath, tableName);//excel文件数据存入数据库
                        GeneralClass.interactivityData?.printlog(1, "料单[" + filename + "]导入数据库文件成功！");

                        //treeView1.Nodes.Clear();
                        //列举所有的sheet名称
                        TreeNode tn = new TreeNode();
                        TreeNode tn1 = new TreeNode();
                        tn.Text = filename; //获取excel文件名作为根节点名称
                        for (int stnum = 0; stnum < GeneralClass.readEXCEL.wb?.NumberOfSheets - 1; stnum++)//因为料单一般最后一页是汇总表，格式不一致，不解析
                        {
                            ISheet sheet = GeneralClass.readEXCEL.wb?.GetSheetAt(stnum);
                            tn1 = new TreeNode();
                            tn1.Text = sheet.SheetName;
                            tn.Nodes.Add(tn1);
                        }
                        //tn.Checked = true;//设置节点为选中
                        treeView1.Nodes.Add(tn);

                        //GeneralClass.readEXCEL.CloseFile();//关闭文件流

                    }
                }

                //tabControl1.Enabled = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //GeneralClass.readEXCEL.CloseFile();//关闭文件流
            }

        }



        /// <summary>
        /// Log添加,保存信息的类别 1:操作记录 
        /// mes：日志内容
        /// </summary>
        /// <param name="type"></param>
        ///                                   
        /// <param name="MES"></param>   
        public void LogAdd(int type, string message)
        {

            try
            {
                if (this != null)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        if (type == 1)
                        {
                            DateTime time = DateTime.Now;
                            ListViewItem I_Item = new ListViewItem();

                            I_Item.Text = time.ToString("HH:mm:ss.fff");
                            I_Item.SubItems.Add(message);

                            this.listView1.Items.Insert(0, I_Item);
                            I_Item.EnsureVisible();

                            if (this.listView1.Items.Count > 500)
                            {
                                this.listView1.Items.RemoveAt(this.listView1.Items.Count - 1);
                            }
                        }

                        Logfile.SaveLog(message, type);
                    }
                        ));
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GetSheetToDGV(comboBox1.SelectedIndex);
        }

        private void InitChecklist1()
        {
            for (int i = 0; i < (int)EnumRebarAssemblyType.maxAssemblyNum; i++)
            {
                checkedListBox1.Items.Add(GeneralClass.sRebarAssemblyTypeName[i]);
            }
        }
        private void InitTreeView1()
        {
            treeView1.Nodes.Clear();
            treeView1.LabelEdit = true;
            treeView1.ExpandAll();
            treeView1.CheckBoxes = true;//节点的勾选框

        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //DataGridViewRow _temprow = new DataGridViewRow();
                //dataGridView2.Rows.Clear();
                //dataGridView3.Rows.Clear();

                //foreach (DataGridViewRow row in dataGridView1.Rows)
                //{
                //    _temprow = (DataGridViewRow)row.Clone();

                //    //先判断cell内容是否为null，再判断直径区间，( >=14 && <=40 )即为棒材钢筋,其他为线材
                //    if (row.Cells[3].Value != null && row.Cells[3].Value.ToString() != "")//cell[3]为直径
                //    {
                //        int diameter = Convert.ToInt32(row.Cells[3].Value.ToString());//直径
                //        if (diameter >= 14 && diameter <= 40)  //棒材
                //        {
                //            for (int i = 0; i < row.Cells.Count; i++)
                //            {
                //                _temprow.Cells[i].Value = row.Cells[i].Value;
                //            }
                //            dataGridView2.Rows.Add(_temprow);
                //        }
                //        else//线材
                //        {
                //            for (int i = 0; i < row.Cells.Count; i++)
                //            {
                //                _temprow.Cells[i].Value = row.Cells[i].Value;
                //            }
                //            dataGridView3.Rows.Add(_temprow);
                //        }
                //    }

                //}


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }



        }




        ///// <summary>
        ///// 从excel表的第_index个sheet中取数据，存入dgv1
        ///// </summary>
        ///// <param name="_index"></param>
        //private void GetSheetToDGV(int _index)
        //{
        //    try
        //    {
        //        ISheet sheet = GeneralClass.readEXCEL.wb.GetSheetAt(_index);

        //        dataGridView1.Rows.Clear();//清空
        //        DataGridViewRow dgvRow = new DataGridViewRow();
        //        DataGridViewCell dgvCell = new DataGridViewTextBoxCell();
        //        DataGridViewImageCell dgvImageCell = new DataGridViewImageCell();

        //        RebarData rebarData = new RebarData();

        //        //从第三行开始读取，前三行为标题
        //        for (int i = sheet.FirstRowNum + 3; i <= sheet.LastRowNum; i++)
        //        {
        //            IRow row = sheet.GetRow(i);
        //            if (row == null) continue;

        //            // 读取单元格数据
        //            //DataGridViewRow dgvRow = row as DataGridViewRow;
        //            //dataGridView1.Rows.Add(dgvRow);//添加一行


        //            dgvRow = new DataGridViewRow();
        //            rebarData.init();   //初始化钢筋对象
        //            string tt = "";

        //            for (int j = row.FirstCellNum; j < row.LastCellNum; j++)
        //            {
        //                ICell cell = row.GetCell(j);
        //                if (cell == null) continue;

        //                if (j == 1)//图形编号列，存入图形编号
        //                {
        //                    tt = GeneralClass.readEXCEL.getCellStringValueAllCase(cell);
        //                    rebarData.TypeNum = tt;
        //                    dgvCell = new DataGridViewTextBoxCell();
        //                    dgvCell.Value = tt;
        //                    dgvRow.Cells.Add(dgvCell);
        //                }
        //                else if (j == 3) //钢筋简图列，需加入图片cell
        //                {
        //                    dgvImageCell = new DataGridViewImageCell();
        //                    string sType = rebarData.TypeNum;
        //                    if (sType == "") continue;  //类型为空，跳过加载图片
        //                    int _iiiii = -1;
        //                    if (FindIndexInImagelist(sType, out _iiiii))//按照图形编号查询图片库中的index
        //                    {
        //                        dgvImageCell.Value = imageList1.Images[_iiiii];//按照index，显示图形
        //                    }
        //                    else
        //                    {
        //                        GeneralClass.interactivityData?.printlog(1, "钢筋图片库中找不到编号为:" + sType + "的图片，请手动添加");
        //                    }
        //                    dgvRow.Cells.Add(dgvImageCell);
        //                }
        //                else if (j == 2) //级别直径列，需要分拆为级别和直径两项
        //                {
        //                    tt = GeneralClass.readEXCEL.getCellStringValueAllCase(cell);

        //                    dgvCell = new DataGridViewTextBoxCell();
        //                    dgvCell.Value = (tt != "") ? tt.Substring(0, 1) : "";//级别,注意有空行的情况，此处做个判断
        //                    dgvRow.Cells.Add(dgvCell);

        //                    dgvCell = new DataGridViewTextBoxCell();
        //                    dgvCell.Value = (tt != "") ? tt.Substring(1, tt.Length - 1) : "";//直径,注意有空行的情况，此处做个判断
        //                    dgvRow.Cells.Add(dgvCell);
        //                }
        //                else if (j == 6)   //下料长度，分拆为长度和是否多段两项
        //                {
        //                    tt = GeneralClass.readEXCEL.getCellStringValueAllCase(cell);
        //                    string[] sss = tt.Split('\n');

        //                    if (sss.Length > 1)
        //                    {
        //                        dgvCell = new DataGridViewTextBoxCell();
        //                        foreach (string item in sss)
        //                        {
        //                            dgvCell.Value += item + "_";
        //                        }
        //                        dgvRow.Cells.Add(dgvCell);

        //                        dgvCell = new DataGridViewTextBoxCell();
        //                        dgvCell.Value = 1;
        //                        dgvRow.Cells.Add(dgvCell);
        //                    }
        //                    else
        //                    {
        //                        dgvCell = new DataGridViewTextBoxCell();
        //                        dgvCell.Value = tt;
        //                        dgvRow.Cells.Add(dgvCell);

        //                        dgvCell = new DataGridViewTextBoxCell();
        //                        dgvCell.Value = 0;
        //                        dgvRow.Cells.Add(dgvCell);
        //                    }
        //                }
        //                else
        //                {
        //                    dgvCell = new DataGridViewTextBoxCell();
        //                    dgvCell.Value = GeneralClass.readEXCEL.getCellStringValueAllCase(cell);//根据cell数据的不同类型分别解析
        //                    dgvRow.Cells.Add(dgvCell);
        //                }
        //            }

        //            dataGridView1.Rows.Add(dgvRow);

        //        }

        //    }
        //    catch (Exception ex) { MessageBox.Show("GetSheetToDGV error:" + ex.Message); }
        //}
        /// <summary>
        /// 选中treeview的某个节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                if (e.Node.Parent != null)
                {
                    //GetSheetToDGV(e.Node.Index);
                    //GetSheetToDGV();
                    //tabControl1.SelectedIndex = 1;
                    GeneralClass.interactivityData?.showAssembly();
                }
            }
        }
        /// <summary>
        /// 选中treeview各个节点的复选勾选后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {

            // 获取当前勾选节点
            TreeNode currentNode = e.Node;

            // 设置以当前节点为根节点的所有子节点的勾选状态等于当前节点的勾选状态
            Treeview_SetChildNodesCheckedState(currentNode, currentNode.Checked);

            // 设置所有父节点的勾选状态
            //Treeview_SetParentNodesCheckedState(currentNode);
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





        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox2.Text = "全选";
                foreach (TreeNode _node1 in treeView1.Nodes)
                {
                    _node1.Checked = true;
                    Treeview_SetChildNodesCheckedState(_node1, true);
                }

                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, true);
                }

            }
            else
            {
                checkBox2.Text = "全不选";
                foreach (TreeNode _node1 in treeView1.Nodes)
                {
                    _node1.Checked = false;
                    Treeview_SetChildNodesCheckedState(_node1, false);
                }
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, false);
                }
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                checkBox3.Text = "展开";
                treeView1.ExpandAll();
            }
            else
            {
                checkBox3.Text = "折叠";
                treeView1.CollapseAll();
            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //MessageBox.Show(checkedListBox1.Items[e.Index].ToString());
            string itemname = checkedListBox1.Items[e.Index].ToString();

            CheckState _checkstate = e.NewValue;
            bool _state = (_checkstate == CheckState.Checked) ? true : false;

            foreach (TreeNode _node in treeView1.Nodes)
            {
                if (_node.Text.IndexOf(itemname) > -1)
                {
                    _node.Checked = _state;
                    Treeview_SetChildNodesCheckedState(_node, _state);
                }
                foreach (TreeNode _childnode in _node.Nodes)
                {
                    if (_childnode.Text.IndexOf(itemname) > -1)
                    {
                        _childnode.Checked = _state;
                        Treeview_SetChildNodesCheckedState(_childnode, _state);
                    }
                }
            }


        }
        private bool IfRebarSelected(RebarData _data)
        {
            string _projectname = _data.ProjectName;
            string _assemblyname = _data.MainAssemblyName;

            foreach (TreeNode _node in treeView1.Nodes)
            {
                if (_node.Text == _projectname)
                {
                    foreach (TreeNode _childnode in _node.Nodes)
                    {
                        if (_childnode.Text == _assemblyname)
                        {
                            return (_childnode.IsSelected) ? true : false;
                        }
                    }
                }

            }
            return false;
        }
        /// <summary>
        /// 在treeview中查询钢筋是否被选中
        /// </summary>
        private bool IfRebarChecked(RebarData _data)
        {
            string _projectname = _data.ProjectName;
            string _assemblyname = _data.MainAssemblyName;

            foreach (TreeNode _node in treeView1.Nodes)
            {
                if (_node.Text == _projectname)
                {
                    foreach (TreeNode _childnode in _node.Nodes)
                    {
                        if (_childnode.Text == _assemblyname)
                        {
                            return (_childnode.Checked) ? true : false;
                        }
                    }
                }

            }
            return false;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                GeneralClass.interactivityData?.printlog(1, "开始创建筛选过的钢筋总表,并存入数据库");

                //先获取备份的钢筋总表
                List<RebarData> _allList_bk = GeneralClass.SQLiteOpt.GetAllRebarList(GeneralClass.AllRebarBKTableName);

                //查询所有被选中的钢筋
                List<RebarData> _newlist = new List<RebarData>();
                foreach (RebarData _data in _allList_bk)
                {
                    if (IfRebarChecked(_data))//查询钢筋是否被选中
                    {
                        _newlist.Add(_data);
                    }
                }

                List<RebarData> _newlist_copy = new List<RebarData>();
                List<RebarData> _insetlist = new List<RebarData>();

                //对newlist做处理，拆分
                for (int i = 0; i < _newlist.Count; i++)
                {
                    if (_newlist[i].IsMulti)//处理多段
                    {
                        _insetlist = new List<RebarData>();
                        _insetlist = GeneralClass.SQLiteOpt.SplitMultiRebar(_newlist[i]);

                        if (_insetlist != null)
                        {
                            _newlist[i].PieceNumUnitNum = "";
                            _newlist[i].TotalPieceNum = 0;
                            _newlist[i].TotalWeight = 0;
                            _newlist_copy.Add(_newlist[i]);//源数据将数量清零，继续存入新的list

                            for (int j = 0; j < _insetlist.Count; j++)
                            {
                                _newlist_copy.Add(_insetlist[j]);
                            }
                        }
                    }
                    else
                    {
                        _newlist_copy.Add(_newlist[i]);
                    }

                }




                //创建筛选后的钢筋总表
                GeneralClass.SQLiteOpt.InitDB(GeneralClass.AllRebarTableName);

                List<string> sqls = new List<string>();
                string _newname = GeneralClass.AllRebarTableName;

                if (_newlist_copy.Count != 0)   //按照新的list来
                {
                    foreach (RebarData _dd in _newlist_copy)
                    {
                        sqls.Add(GeneralClass.SQLiteOpt.InsertRowData(_newname, _dd));
                    }
                }
                //if (_newlist.Count != 0)
                //{
                //    foreach (RebarData _dd in _newlist)
                //    {
                //        sqls.Add(GeneralClass.SQLiteOpt.InsertRowData(_newname, _dd));
                //    }
                //}
                GeneralClass.SQLiteOpt.ExecuteNonQuery(sqls);//批量存入

                GeneralClass.interactivityData?.printlog(1, "创建筛选过的钢筋总表完成！");

                //tabControl1.Enabled = true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }





        private void button12_Click(object sender, EventArgs e)
        {

            List<SingleRebarData> _rebarlist = new List<SingleRebarData>();

            SingleRebarData _singleRebar = new SingleRebarData();
            _singleRebar.projectName = "光谷国际社区";
            _singleRebar.assemblyName = "梁";
            _singleRebar.elementName = "KL57";
            _singleRebar.picNo = "30202";
            _singleRebar.level = "C";
            _singleRebar.diameter = 22;
            _singleRebar.length = 6000;
            _singleRebar.cornerMsg = "350,90;5300,90;350,0";
            _singleRebar.ID = 1;
            _rebarlist.Add(_singleRebar);

            _singleRebar = new SingleRebarData();
            _singleRebar.projectName = "光谷国际社区";
            _singleRebar.assemblyName = "梁";
            _singleRebar.elementName = "KZ1";
            _singleRebar.picNo = "10000";
            _singleRebar.level = "C";
            _singleRebar.diameter = 22;
            _singleRebar.length = 2500;
            _singleRebar.cornerMsg = "0,套;2500,0";
            _singleRebar.ID = 2;
            _rebarlist.Add(_singleRebar);

            GeneralWorkBill _workbill = new GeneralWorkBill();
            _workbill.BillNo = "GJSQ_A_06D_01F_202306280001";
            _workbill.ProjectName = "光谷国际社区";
            _workbill.Block = "A";
            _workbill.Building = "06D";
            _workbill.Floor = "01F";
            _workbill.Level = "C";
            _workbill.Diameter = 22;
            _workbill.OriginalLength = 9;
            _workbill.RebarList = _rebarlist;

            string sss = GeneralClass.JsonOpt.Serializer(_workbill);

            //textBox3.Text = sss;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.TopLevel = false;
            form2.FormBorderStyle = FormBorderStyle.None;
            form2.Parent = this.panel3;
            form2.Dock = DockStyle.Fill;
            form2.Show();

        }
    }
}
