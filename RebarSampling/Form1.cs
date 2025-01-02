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
using RebarSampling.log;
using NPOI.POIFS.Crypt.Dsig;

namespace RebarSampling
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //Font font = new Font(textBox1.Font.FontFamily, float.Parse(textBox1.Text), textBox1.Font.Style);
            //Font font = new Font("⿊体", 15);

            

            this.Text = "钢筋云工厂翻样料单分析套料软件V1.0版";
            queueLogger.Instance().Register();
            GeneralClass.interactivityData.printlog += LogAdd;
            GeneralClass.interactivityData.ifRebarSelected += IfRebarSelected;
            GeneralClass.interactivityData.ifRebarChecked += IfRebarChecked;
        }

        private Form2 form2;
        private Form3 form3;
        private Form4 form4;
        private Form5 form5;
        private FormLeftUsed FormLeftUsed;

        private void Form1_Load(object sender, EventArgs e)
        {
            form5 = new Form5();//form的构造函数中会load系统配置文件，需先加载
            form5.TopLevel = false;
            form5.FormBorderStyle = FormBorderStyle.None;
            form5.Dock = DockStyle.Fill;

            form2 = new Form2();
            form2.TopLevel = false;
            form2.FormBorderStyle = FormBorderStyle.None;
            form2.Dock = DockStyle.Fill;

            form3 = new Form3();
            form3.TopLevel = false;
            form3.FormBorderStyle = FormBorderStyle.None;
            form3.Dock = DockStyle.Fill;

            form4 = new Form4();
            form4.TopLevel = false;
            form4.FormBorderStyle = FormBorderStyle.None;
            form4.Dock = DockStyle.Fill;



            FormLeftUsed = new FormLeftUsed();
            FormLeftUsed.TopLevel = false;
            FormLeftUsed.FormBorderStyle = FormBorderStyle.None;
            FormLeftUsed.Dock = DockStyle.Fill;


            InitCheckbox();
            InitTreeView1();
            
            if(GeneralClass.CfgData.DatabaseType==EnumDatabaseType.SQLITE)
            {
                GeneralClass.DBOpt = new DBOpt(EnumDatabaseType.SQLITE);
            }
            else
            {
                GeneralClass.DBOpt = new DBOpt(EnumDatabaseType.MYSQL);
            }

            //20240728关闭，因sqlite数据库为本地数据库，mysql与此不同
            string filepath = Application.StartupPath + @"\dbfile\" + GeneralClass.AllRebarDBfileName + ".db";
            GeneralClass.DBOpt.dbHelper.ConnectDB(filepath); //无密码连接

            LoadTreeview1();//从【筛选】数据库中获取工单筛选状态，更新treeview1


        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //GeneralClass.readEXCEL.CloseFile();//关闭文件流

        }

        /// <summary>
        /// 解析e筋格式文件时，用于树形数据设置节点
        /// </summary>
        /// <param name="_path"></param>
        /// <param name="_node"></param>
        /// <param name="_level"></param>
        /// <param name="_morder"></param>
        /// <returns></returns>
        private bool SetTreeViewNodes(string _path, TreeNode _node, int _level, Morder _morder)
        {
            bool _validfile = false;
            TreeNode newnode = new TreeNode();

            DirectoryInfo folder = new DirectoryInfo(_path);

            foreach (FileInfo newfile in folder.GetFiles())
            {
                if (newfile.Extension == ".ejb")
                {
                    _validfile = true;
                    newnode = new TreeNode();
                    newnode.Text = Path.GetFileNameWithoutExtension(newfile.Name);
                    _node.Nodes.Add(newnode);

                    _morder.levelName[_level] = newnode.Text;//将当前文件名作为morder的层级名称
                    //string jsonstr = GeneralClass.readEjin.GetJsonStr(_path + "\\" + newfile.Name, _morder);
                    //GeneralClass.interactivityData?.printlog(3, jsonstr);//存入历史数据
                }
            }

            foreach (DirectoryInfo newfolder in folder.GetDirectories())
            {
                string newpath = _path + "\\" + newfolder.Name;

                _morder.levelName[_level] = newfolder.Name;//将当前文件夹名作为morder的层级名称
                newnode = new TreeNode();

                if (SetTreeViewNodes(newpath, newnode, _level + 1, _morder))//如果下层文件夹有.ejb文件，才需要创建node节点
                {
                    _validfile = true;

                    newnode.Text = newfolder.Name;
                    _node.Nodes.Add(newnode);
                }
            }

            return _validfile;
        }

        private string dialogPath = "";
        private void button3_Click(object sender, EventArgs e)
        {
            InitTreeView1();
            InitCheckbox();
            if (GeneralClass.interactivityData?.initStatisticsDGV != null)
            {
                GeneralClass.interactivityData?.initStatisticsDGV();//清空统计界面的dgv
            }

            GeneralClass.interactivityData?.printlog(1, "开始导入E筋料单至数据库文件，请等待。。。");

            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "打开料单文件";
            dialog.SelectedPath = dialogPath;//保留历史记录，方便下次打开

            string folderpath = "";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                folderpath = dialog.SelectedPath;
                this.dialogPath = folderpath;

                DirectoryInfo folder = new DirectoryInfo(folderpath);

                TreeNode _rootNode = new TreeNode();
                _rootNode.Text = folder.Name;
                this.treeView1.Nodes.Add(_rootNode);

                int _level = 0;
                Morder _morder = new Morder();
                _morder.levelName[_level] = _rootNode.Text;//

                SetTreeViewNodes(folderpath, _rootNode, (_level + 1), _morder);

            }


            GeneralClass.interactivityData?.printlog(1, "E筋料单[" + Path.GetFileNameWithoutExtension(folderpath) + "]导入数据库文件成功！");





            #region MyRegion

            //try
            //{
            //    //string filepath = "";

            //    //打开excel文件
            //    OpenFileDialog openFileDialog = new OpenFileDialog();//打开文件夹对话框
            //    openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            //    openFileDialog.Filter = "E筋文件(*.ejb)|*.ejb|所有文件(*.*)|*.*";
            //    openFileDialog.Title = "打开料单";
            //    openFileDialog.FilterIndex = 0;
            //    openFileDialog.Multiselect = true;
            //    openFileDialog.CheckFileExists = true;
            //    openFileDialog.CheckPathExists = true;


            //    //treeView1.Nodes.Clear();
            //    InitTreeView1();
            //    InitCheckbox();
            //    if (GeneralClass.interactivityData?.initStatisticsDGV != null)
            //    {
            //        GeneralClass.interactivityData?.initStatisticsDGV();//清空统计界面的dgv
            //    }

            //    //GeneralClass.SQLiteOpt.InitDB(GeneralClass.AllRebarTableName);//先创建并清空db数据库表单
            //    GeneralClass.SQLiteOpt.InitDB(GeneralClass.AllRebarBKTableName);//先创建并清空db数据库表单

            //    if (openFileDialog.ShowDialog() == DialogResult.OK)
            //    {
            //        foreach (string filepath in openFileDialog.FileNames)
            //        {
            //            string filename = System.IO.Path.GetFileNameWithoutExtension(filepath); //获取excel文件名作为根节点名称,不带后缀名

            //            this.toolStripStatusLabel1.Text = filepath;//显示文件名称

            //            string tableName = GeneralClass.AllRebarBKTableName;

            //            GeneralClass.interactivityData?.printlog(1, "开始导入E筋料单至数据库文件，请等待。。。");
            //            //GeneralClass.SQLiteOpt.ExcelToDB(filepath, tableName);//excel文件数据存入数据库

            //            string jsonstr = GeneralClass.readEjin.GetJsonStr(filepath);



            //            GeneralClass.interactivityData?.printlog(3, jsonstr);//存入历史数据

            //            GeneralClass.interactivityData?.printlog(1, "E筋料单[" + filename + "]导入数据库文件成功！");

            //            //列举所有的sheet名称
            //            TreeNode tn = new TreeNode();
            //            TreeNode tn1 = new TreeNode();
            //            TreeNode tn2 = new TreeNode();
            //            tn.Text = filename; //获取excel文件名作为根节点名称
            //            for (int stnum = 0; stnum < GeneralClass.readEXCEL.wb?.NumberOfSheets - 1; stnum++)//因为料单一般最后一页是汇总表，格式不一致，不解析
            //            {
            //                ISheet sheet = GeneralClass.readEXCEL.wb?.GetSheetAt(stnum);
            //                tn1 = new TreeNode();
            //                tn1.Text = sheet.SheetName;

            //                tn.Nodes.Add(tn1);
            //            }
            //            //tn.Checked = true;//设置节点为选中
            //            treeView1.Nodes.Add(tn);

            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            #endregion
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //string filepath = "";

                //打开excel文件
                OpenFileDialog openFileDialog = new OpenFileDialog();//打开文件夹对话框
                openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                openFileDialog.Filter = "excel文件(*.xls)|*.xls|excel文件(*.xlsx)|*.xlsx|所有文件(*.*)|*.*";
                openFileDialog.Title = "打开料单";
                openFileDialog.FilterIndex = 0;
                openFileDialog.Multiselect = true;
                openFileDialog.CheckFileExists = true;
                openFileDialog.CheckPathExists = true;


                //treeView1.Nodes.Clear();
                InitCheckbox();
                InitTreeView1();

                if (GeneralClass.interactivityData?.initStatisticsDGV != null)
                {
                    GeneralClass.interactivityData?.initStatisticsDGV();//清空统计界面的dgv
                }

                //GeneralClass.SQLiteOpt.InitDB(GeneralClass.AllRebarTableName);//先创建并清空db数据库表单
                GeneralClass.DBOpt.InitRebarDB(GeneralClass.TableName_AllRebarBK);//先创建并清空db数据库表单

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string filepath in openFileDialog.FileNames)
                    {

                        string filename = System.IO.Path.GetFileNameWithoutExtension(filepath); //获取excel文件名作为根节点名称,不带后缀名

                        //GeneralClass.readEXCEL.OpenFile(filepath);


                        this.toolStripStatusLabel1.Text = filepath;//显示文件名称

                        string tableName = GeneralClass.TableName_AllRebarBK;

                        GeneralClass.interactivityData?.printlog(1, "开始导入料单至数据库文件，请等待。。。");
                        //GeneralClass.SQLiteOpt.ExcelToDB(filename, tableName);//excel文件数据存入数据库
                        GeneralClass.DBOpt.ExcelToDB(filepath, tableName);//excel文件数据存入数据库
                        GeneralClass.interactivityData?.printlog(1, "料单[" + filename + "]导入数据库文件成功！");

                        //treeView1.Nodes.Clear();
                        //列举所有的sheet名称
                        TreeNode tn = new TreeNode();
                        TreeNode tn1 = new TreeNode();
                        TreeNode tn2 = new TreeNode();
                        tn.Text = filename; //获取excel文件名作为根节点名称
                        for (int stnum = 0; stnum < GeneralClass.ExcelOpt.wb?.NumberOfSheets - 1; stnum++)//因为料单一般最后一页是汇总表，格式不一致，不解析
                        {
                            ISheet sheet = GeneralClass.ExcelOpt.wb?.GetSheetAt(stnum);
                            tn1 = new TreeNode();
                            tn1.Text = sheet.SheetName;

                            //List<ElementData> _list = GeneralClass.SQLiteOpt.GetAllElementList(tableName, filename, sheet.SheetName);
                            //foreach (var item in _list)
                            //{
                            //    tn2 = new TreeNode();
                            //    tn2.Text = item.elementName;
                            //    tn1.Nodes.Add(tn2);
                            //}


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
        /// 根据数据库存储状态，加载treeview1，20240801
        /// </summary>
        private void LoadTreeview1()
        {
            DataTable dt = GeneralClass.DBOpt.dbHelper.GetDataTable(GeneralClass.TableName_Pick);
            if (dt != null)
            {
                List<Tuple<string, string, bool>> _list = new List<Tuple<string, string, bool>>();//项目名称，主构件名称，是否选择

                foreach (DataRow row in dt.Rows)
                {
                    //var tt = row[3].ToString();
                    Tuple<string, string, bool> temp;
                    if (GeneralClass.CfgData.DatabaseType==EnumDatabaseType.SQLITE)
                    {
                        temp = new Tuple<string, string, bool>(row[1].ToString(), row[2].ToString(), Convert.ToBoolean(row[3].ToString()));//注意sqlite数据库有boolean类型
                    }
                    else
                    {
                        temp = new Tuple<string, string, bool>(row[1].ToString(), row[2].ToString(), Convert.ToInt16(row[3].ToString()) == 1 ? true : false);//mysql数据库没有boolean类型
                    }

                    _list.Add(temp);
                }

                TreeNode tn = new TreeNode();
                TreeNode tn1 = new TreeNode();
                //TreeNode tn2 = new TreeNode();
                var _group = _list.GroupBy(t => t.Item1).ToList();//根据项目名称分类

                foreach (var ttt in _group)
                {
                    tn = new TreeNode();
                    tn.Text = ttt.Key;
                    foreach (var eee in ttt.ToList())
                    {
                        tn1 = new TreeNode();
                        tn1.Text = eee.Item2;
                        tn1.Checked = eee.Item3 ? true : false;

                        tn.Nodes.Add(tn1);
                    }
                    treeView1.Nodes.Add(tn);
                }

                ////列举所有的sheet名称
                //TreeNode tn = new TreeNode();
                //TreeNode tn1 = new TreeNode();
                //TreeNode tn2 = new TreeNode();
                //tn.Text = filename; //获取excel文件名作为根节点名称
                //for (int stnum = 0; stnum < GeneralClass.readEXCEL.wb?.NumberOfSheets - 1; stnum++)//因为料单一般最后一页是汇总表，格式不一致，不解析
                //{
                //    ISheet sheet = GeneralClass.readEXCEL.wb?.GetSheetAt(stnum);
                //    tn1 = new TreeNode();
                //    tn1.Text = sheet.SheetName;

                //    tn.Nodes.Add(tn1);
                //}
                ////tn.Checked = true;//设置节点为选中
                //treeView1.Nodes.Add(tn);









            }

        }


        /// <summary>
        /// Log添加,保存信息的类别 
        /// 1：操作记录 
        /// 2：料单记录
        /// 
        /// </summary>
        /// <param name="type">记录类型</param>
        ///                                   
        /// <param name="MES">记录信息内容</param>   
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

                        //Logfile.SaveLog(message, type);
                        queueLogger.Info(message);




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

        private void InitCheckbox()
        {
            checkBox2.Checked = false;
            checkBox2.Text = "全不选";
            checkBox3.Checked = false;
            checkBox3.Text = "折叠";

            checkedListBox1.Items.Clear();
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
                    if (this.panel3.Controls.Contains(form2))//如果当前显示的是form2（统计界面），则执行显示主构件信息
                    {
                        GeneralClass.interactivityData?.showAssembly(e.Node.Parent.Text, e.Node.Text);
                    }
                    if (this.panel3.Controls.Contains(form3))//如果当前显示 的是form3（套料界面），则显示子构件详细信息
                    {
                        GeneralClass.interactivityData?.showAssembly(e.Node.Parent.Text, e.Node.Text);
                    }

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
                            return _childnode.IsSelected;
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
            string _tableName = _data.TableName;
            string _tablesheetName = _data.TableSheetName;

            foreach (TreeNode _node in treeView1.Nodes)
            {
                if (_node.Text == _tableName)
                {
                    foreach (TreeNode _childnode in _node.Nodes)
                    {
                        if (_childnode.Text == _tablesheetName)
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
                List<RebarData> _allList_bk = GeneralClass.DBOpt.GetAllRebarList(GeneralClass.TableName_AllRebarBK);

                //查询所有被选中的钢筋
                List<RebarData> _newlist = new List<RebarData>();
                foreach (RebarData _data in _allList_bk)
                {
                    if (IfRebarChecked(_data))//查询钢筋是否被选中
                    {
                        _newlist.Add(_data);
                    }
                }

                List<RebarData> _newlist_1 = new List<RebarData>();
                List<RebarData> _newlist_2=new List<RebarData>();
                List<RebarData> _insetlist = new List<RebarData>();

                //对newlist做处理，拆分多段
                for (int i = 0; i < _newlist.Count; i++)
                {
                    if (_newlist[i].IsMulti)//处理多段
                    {
                        _insetlist = new List<RebarData>();
                        _insetlist = GeneralClass.DBOpt.SplitMultiRebar(_newlist[i]);

                        if (_insetlist != null)
                        {
                            _newlist[i].PieceNumUnitNum = "";
                            _newlist[i].TotalPieceNum = 0;
                            _newlist[i].TotalWeight = 0;
                            _newlist_1.Add(_newlist[i]);//源数据将数量清零，继续存入新的list

                            for (int j = 0; j < _insetlist.Count; j++)
                            {
                                _newlist_1.Add(_insetlist[j]);
                            }
                        }
                    }
                    else
                    {
                        _newlist_1.Add(_newlist[i]);
                    }
                }

                //对newlist_1做处理，拆分缩尺
                for (int i = 0; i < _newlist_1.Count; i++)
                {
                    if (_newlist_1[i].IsSuoChi && _newlist_1[i].TotalPieceNum!=0)//处理缩尺，且在上轮处理多段后数量不为零
                    {
                        _insetlist = new List<RebarData>();
                        _insetlist = GeneralClass.DBOpt.SplitSuoChiRebar(_newlist_1[i]);

                        if (_insetlist != null)
                        {
                            _newlist_1[i].PieceNumUnitNum = "";
                            _newlist_1[i].TotalPieceNum = 0;
                            _newlist_1[i].TotalWeight = 0;
                            _newlist_2.Add(_newlist_1[i]);//源数据将数量清零，继续存入新的list

                            for (int j = 0; j < _insetlist.Count; j++)
                            {
                                _newlist_2.Add(_insetlist[j]);
                            }
                        }
                    }
                    else
                    {
                        _newlist_2.Add(_newlist_1[i]);
                    }
                }


                //创建筛选后的钢筋总表
                GeneralClass.DBOpt.InitRebarDB(GeneralClass.TableName_AllRebar);

                List<string> sqls = new List<string>();
                string _newname = GeneralClass.TableName_AllRebar;

                if (_newlist_2.Count != 0)   //按照新的list来
                {
                    foreach (RebarData _dd in _newlist_2)
                    {
                        sqls.Add(GeneralClass.DBOpt.InsertRowRebarData(_newname, _dd));
                    }
                }
                GeneralClass.DBOpt.dbHelper.ExecuteSqlsTran(sqls);//批量存入


                //可以提前生成弯曲所需的工单list
                GeneralClass.AllRebarList = GeneralClass.DBOpt.GetAllRebarList(GeneralClass.TableName_AllRebar);//取得所有的钢筋数据list
                GeneralClass.jsonList_bend=GeneralClass.WorkBillOpt.CreateWorkBill_bend_LB(GeneralClass.AllRebarList);//生成所有弯曲的加工信息json

                //创建钢筋总表的筛选表
                GeneralClass.DBOpt.InitPickDB(GeneralClass.TableName_Pick);
                string _projectname;
                string _assemblyname;

                sqls = new List<string>();//重置
                foreach (TreeNode _node in treeView1.Nodes)
                {
                    _projectname = _node.Text;
                    foreach (TreeNode _childnode in _node.Nodes)
                    {
                        _assemblyname = _childnode.Text;
                        sqls.Add(GeneralClass.DBOpt.InsertRowPickData(GeneralClass.TableName_Pick, _projectname, _assemblyname, _childnode.Checked));
                    }
                }
                GeneralClass.DBOpt.dbHelper.ExecuteSqlsTran(sqls);//批量存入


                //创建钢筋总表的构件表
                GeneralClass.DBOpt.InitElementDB(GeneralClass.TableName_Element);

                GeneralClass.AllElementList = GeneralClass.DBOpt.GetAllElementList(GeneralClass.TableName_AllRebar);

                sqls = new List<string>();//重置
                foreach (var item in GeneralClass.AllElementList)
                {
                    sqls.Add(GeneralClass.DBOpt.InsertRowElementData(GeneralClass.TableName_Element, item.projectName, item.assemblyName, item.elementName));
                }
                GeneralClass.DBOpt.dbHelper.ExecuteSqlsTran(sqls);//批量存入




                GeneralClass.interactivityData?.printlog(1, "创建筛选过的钢筋总表完成！");

                //tabControl1.Enabled = true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //隐藏导航栏
            this.splitContainer1.Panel1Collapsed = !this.splitContainer1.Panel1Collapsed;
            //根据隐藏属性切换项目资源文件中的图片显示
            button4.Image = this.splitContainer1.Panel1Collapsed ? Properties.Resources.icons8_double_right_26 : Properties.Resources.icons8_double_left_26;
        }



        private void button12_Click(object sender, EventArgs e)
        {
            button11.BackColor = SystemColors.GradientInactiveCaption;
            button12.BackColor = Color.Wheat;
            button2.BackColor = SystemColors.GradientInactiveCaption;
            button5.BackColor = SystemColors.GradientInactiveCaption;
            button7.BackColor = SystemColors.GradientInactiveCaption;

            form3.Show();
            this.panel3.Controls.Clear();
            this.panel3.Controls.Add(form3);


        }

        private void button11_Click(object sender, EventArgs e)
        {
            button11.BackColor = Color.Wheat;
            button12.BackColor = SystemColors.GradientInactiveCaption;
            button2.BackColor = SystemColors.GradientInactiveCaption;
            button5.BackColor = SystemColors.GradientInactiveCaption;
            button7.BackColor = SystemColors.GradientInactiveCaption;

            form2.Show();
            this.panel3.Controls.Clear();
            this.panel3.Controls.Add(form2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button11.BackColor = SystemColors.GradientInactiveCaption;
            button12.BackColor = SystemColors.GradientInactiveCaption;
            button2.BackColor = Color.Wheat;
            button5.BackColor = SystemColors.GradientInactiveCaption;
            button7.BackColor = SystemColors.GradientInactiveCaption;

            form4.Show();
            this.panel3.Controls.Clear();
            this.panel3.Controls.Add(form4);

        }



        private void button5_Click(object sender, EventArgs e)
        {
            button11.BackColor = SystemColors.GradientInactiveCaption;
            button12.BackColor = SystemColors.GradientInactiveCaption;
            button2.BackColor = SystemColors.GradientInactiveCaption;
            button5.BackColor = Color.Wheat;
            button7.BackColor = SystemColors.GradientInactiveCaption;

            form5.Show();
            this.panel3.Controls.Clear();
            this.panel3.Controls.Add(form5);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button11.BackColor = SystemColors.GradientInactiveCaption;
            button12.BackColor = SystemColors.GradientInactiveCaption;
            button2.BackColor = SystemColors.GradientInactiveCaption;
            button5.BackColor = SystemColors.GradientInactiveCaption;
            button7.BackColor = Color.Wheat;

            FormLeftUsed.Show();
            this.panel3.Controls.Clear();
            this.panel3.Controls.Add(FormLeftUsed);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                //先做初始化动作
                InitCheckbox();
                InitTreeView1();
                if (GeneralClass.interactivityData?.initStatisticsDGV != null)
                {
                    GeneralClass.interactivityData?.initStatisticsDGV();//清空统计界面的dgv
                }
                //GeneralClass.SQLiteOpt.InitDB(GeneralClass.AllRebarTableName);//先创建并清空db数据库表单
                GeneralClass.DBOpt.InitRebarDB(GeneralClass.TableName_AllRebarBK);//先创建并清空db数据库表单

                //开始解析文件夹
                string folderPath = "";
                FolderBrowserDialog folderdlg = new FolderBrowserDialog();
                folderdlg.Description = "请选择解压缩后的料单完整文件夹";
                folderdlg.RootFolder = Environment.SpecialFolder.Recent;//上次打开的目录
                folderdlg.ShowNewFolderButton = false;
                if(folderdlg.ShowDialog()==DialogResult.OK)
                {
                    folderPath= folderdlg.SelectedPath;
                }
                

                //if (openFileDialog.ShowDialog() == DialogResult.OK)
                //{

                    //foreach (string filepath in openFileDialog.FileNames)
                    //{

                    //    string filename = System.IO.Path.GetFileNameWithoutExtension(filepath); //获取excel文件名作为根节点名称,不带后缀名

                    //    //GeneralClass.readEXCEL.OpenFile(filepath);


                    //    this.toolStripStatusLabel1.Text = filepath;//显示文件名称

                    //    string tableName = GeneralClass.TableName_AllRebarBK;

                    //    GeneralClass.interactivityData?.printlog(1, "开始导入料单至数据库文件，请等待。。。");
                    //    GeneralClass.DBOpt.ExcelToDB(filepath, tableName);//excel文件数据存入数据库
                    //    GeneralClass.interactivityData?.printlog(1, "料单[" + filename + "]导入数据库文件成功！");

                    //    //列举所有的sheet名称
                    //    TreeNode tn = new TreeNode();
                    //    TreeNode tn1 = new TreeNode();
                    //    TreeNode tn2 = new TreeNode();
                    //    tn.Text = filename; //获取excel文件名作为根节点名称
                    //    for (int stnum = 0; stnum < GeneralClass.ExcelOpt.wb?.NumberOfSheets - 1; stnum++)//因为料单一般最后一页是汇总表，格式不一致，不解析
                    //    {
                    //        ISheet sheet = GeneralClass.ExcelOpt.wb?.GetSheetAt(stnum);
                    //        tn1 = new TreeNode();
                    //        tn1.Text = sheet.SheetName;

                    //        tn.Nodes.Add(tn1);
                    //    }
                    //    //tn.Checked = true;//设置节点为选中
                    //    treeView1.Nodes.Add(tn);

                    //    //GeneralClass.readEXCEL.CloseFile();//关闭文件流

                    //}
                //}

                //tabControl1.Enabled = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //GeneralClass.readEXCEL.CloseFile();//关闭文件流
            }

        }
    }
}
