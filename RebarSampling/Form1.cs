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
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //dataGridView1.Rows.Add(new object[] { "构件名称","编号","级别直径","钢筋简图","边角结构","下料长度(mm)","根数/件数","总根数","重量(kg)","备注"});
            //dataGridView2.Rows.Add(new object[] { "构件名称", "编号", "级别直径", "钢筋简图", "边角结构", "下料长度(mm)", "根数/件数", "总根数", "重量(kg)", "备注" });
            //dataGridView3.Rows.Add(new object[] { "构件名称", "编号", "级别直径", "钢筋简图", "边角结构", "下料长度(mm)", "根数/件数", "总根数", "重量(kg)", "备注" });
            InitDataGridView1();
            InitDataGridView2();
            InitDataGridView3();
            InitDataGridView4();
            InitDataGridView5();
            InitDataGridView6();
            //InitDataGridView7();
            InitDataGridView8();
            InitDataGridView9();
            InitDataGridView10();

            InitChecklist1();
            InitTreeView1();
            InitRadioButtion();
            InitCheckBox();

            tabControl1.Enabled = false;

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
                InitDGV_BX();
                InitDGV_Xian();
                InitDGV_BangOri();
                InitDGV_BangNoOri();

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

                tabControl1.Enabled = false;

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
        private void InitDGV(DataGridView _dgv)
        {
            //"构件名称","编号","级别直径","钢筋简图","图形信息","边角结构","下料长度(mm)","根数/件数","总根数","重量(kg)","备注"
            DataGridViewColumn column;
            DataGridViewCell cell;
            DataGridViewImageColumn imageColumn;

            for (int i = (int)EnumAllRebarTableColName.ELEMENT_NAME; i < (int)EnumAllRebarTableColName.maxRebarColNum; i++) //从1开始，子构件名称
            {
                if (i == (int)EnumAllRebarTableColName.PIC_MESSAGE
                    || i == (int)EnumAllRebarTableColName.ISMULTI
                    || i == (int)EnumAllRebarTableColName.IFTAO
                    || i == (int)EnumAllRebarTableColName.IFBEND
                    || i == (int)EnumAllRebarTableColName.IFBENDTWICE
                    || i == (int)EnumAllRebarTableColName.IFCUT
                    || i == (int)EnumAllRebarTableColName.ISORIGINAL
                    )
                {
                    continue;
                }
                if (i == (int)EnumAllRebarTableColName.REBAR_PIC)
                {
                    imageColumn = new DataGridViewImageColumn();
                    imageColumn.HeaderText = GeneralClass.sRebarColumnName[i];//标题
                    imageColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;//设置图片可以根据单元格大小进行自动调整
                    _dgv.Columns.Add(imageColumn);
                }
                else
                {
                    column = new DataGridViewColumn();
                    cell = new DataGridViewTextBoxCell();
                    column.CellTemplate = cell;//设置单元格模板
                    column.HeaderText = GeneralClass.sRebarColumnName[i];//
                    _dgv.Columns.Add(column);

                }
            }
        }
        //初始化dgv1
        private void InitDataGridView1()
        {
            InitDGV(dataGridView1);
            ////"构件名称","编号","级别直径","钢筋简图","图形信息","边角结构","下料长度(mm)","根数/件数","总根数","重量(kg)","备注"
            //DataGridViewColumn column;
            //DataGridViewCell cell;
            //DataGridViewImageColumn imageColumn;

            //for (int i = (int)EnumAllRebarTableColName.ELEMENT_NAME; i < (int)EnumAllRebarTableColName.maxRebarColNum; i++) //从1开始，子构件名称
            //{
            //    if (i == (int)EnumAllRebarTableColName.REBAR_PIC)
            //    {
            //        imageColumn = new DataGridViewImageColumn();
            //        imageColumn.HeaderText = GeneralClass.sRebarColumnName[i];//标题
            //        imageColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;//设置图片可以根据单元格大小进行自动调整
            //        dataGridView1.Columns.Add(imageColumn);
            //    }
            //    else
            //    {
            //        column = new DataGridViewColumn();
            //        cell = new DataGridViewTextBoxCell();
            //        column.CellTemplate = cell;//设置单元格模板
            //        column.HeaderText = GeneralClass.sRebarColumnName[i];//
            //        dataGridView1.Columns.Add(column);

            //    }
            //}

        }
        //初始化dgv2
        private void InitDataGridView2()
        {
            //InitDGV(dataGridView2);
        }
        //初始化dgv3
        private void InitDataGridView3()
        {
            //InitDGV(dataGridView3);
        }

        private void InitDataGridView4()
        {
            DataGridViewColumn column;
            DataGridViewCell cell;

            DataGridViewImageColumn imageColumn;

            for (int i = 0; i < (int)EnumAllPicTableColName.maxAllPicColNum; i++)
            {
                if (i == (int)EnumAllPicTableColName.PIC)
                {
                    imageColumn = new DataGridViewImageColumn();
                    imageColumn.HeaderText = GeneralClass.sAllPicColumnName[i];//标题
                    imageColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;//设置图片可以根据单元格大小进行自动调整
                    dataGridView4.Columns.Add(imageColumn);
                }
                else
                {
                    column = new DataGridViewColumn();
                    cell = new DataGridViewTextBoxCell();
                    column.CellTemplate = cell;//设置单元格模板
                    column.HeaderText = GeneralClass.sAllPicColumnName[i];//
                    dataGridView4.Columns.Add(column);
                }
            }

            //dataGridView4.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            //dataGridView4.ColumnHeadersHeight = 30;
            //dataGridView4.RowTemplate.Height = 30;

        }

        private void InitDataGridView5()
        {
            DataGridViewColumn column;
            DataGridViewCell cell;
            DataGridViewImageColumn imageColumn;

            for (int i = 0; i < (int)EnumAllPicTableColName.maxAllPicColNum; i++)
            {
                if (i == (int)EnumAllPicTableColName.PIC)
                {
                    imageColumn = new DataGridViewImageColumn();
                    imageColumn.HeaderText = GeneralClass.sAllPicColumnName[i];//标题
                    imageColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;//设置图片可以根据单元格大小进行自动调整
                    dataGridView5.Columns.Add(imageColumn);
                }
                else
                {
                    column = new DataGridViewColumn();
                    cell = new DataGridViewTextBoxCell();
                    column.CellTemplate = cell;//设置单元格模板
                    column.HeaderText = GeneralClass.sAllPicColumnName[i];//
                    dataGridView5.Columns.Add(column);

                }
            }

        }

        private void InitDataGridView6()
        {

            try
            {
                //////////////////////////////////////////////////////
                DataGridViewColumn column;
                DataGridViewCell cell;
                DataGridViewImageColumn imageColumn;
                DataGridViewRow row;

                imageColumn = new DataGridViewImageColumn();
                imageColumn.HeaderText = "图形";//标题
                imageColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;//设置图片可以根据单元格大小进行自动调整
                dataGridView6.Columns.Add(imageColumn);

                for (int i = 0; i < (int)EnumDetailTableRowName.maxRowNum; i++)
                {
                    column = new DataGridViewColumn();
                    cell = new DataGridViewTextBoxCell();
                    column.CellTemplate = cell;//设置单元格模板
                    column.HeaderText = GeneralClass.sDetailTableRowName[i];//
                    dataGridView6.Columns.Add(column);
                }

                for (int j = 0; j < (int)EnumDetailTableColName.ONLY_CUT; j++)
                {
                    row = new DataGridViewRow();
                    row.HeaderCell.Value = GeneralClass.sDetailTableColName[j];
                    dataGridView6.Rows.Add(row);
                }



                //// 将所有列的 SortMode 属性设置为 Programmatic
                //foreach (DataGridViewColumn _column in dataGridView6.Columns)
                //{
                //    _column.SortMode = DataGridViewColumnSortMode.Programmatic;
                //}
                // 将 DataGridView 控件的 SelectionMode 设置为 FullColumnSelect
                dataGridView6.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            }
            catch (Exception ex) { MessageBox.Show("InitDataGridView6 error:" + ex.Message); }

        }
        /// <summary>
        /// 棒材线材分类统计界面的dgv
        /// </summary>
        private void InitDGV_BX()
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dataGridView13.DataSource = dt;
            dataGridView14.DataSource = dt;
            dataGridView11.DataSource = dt;
        }
        private void InitDGV_Xian()
        {
            DataTable dt = new DataTable();
            dt.Clear();

            dataGridView9.DataSource = dt;
            dataGridView12.DataSource = dt;
            dataGridView23.DataSource = dt;
        }

        private void InitDGV_BangOri()
        {
            DataTable dt = new DataTable();
            dt.Clear();

            dataGridView15.DataSource = dt;
            dataGridView16.DataSource = dt;
            dataGridView17.DataSource = dt;
            dataGridView18.DataSource = dt;
            dataGridView24.DataSource = dt;

        }
        private void InitDGV_BangNoOri()
        {
            DataTable dt = new DataTable();
            dt.Clear();

            dataGridView19.DataSource = dt;
            dataGridView20.DataSource = dt;
            dataGridView21.DataSource = dt;
            dataGridView22.DataSource = dt;
            dataGridView25.DataSource = dt;


        }
        private void InitDataGridView8()
        {
            InitDGV(dataGridView8);
        }
        private void InitDataGridView9()
        {
            //InitDGV(dataGridView9);

            //DataGridViewColumn column;
            //DataGridViewCell cell;

            //DataGridViewImageColumn imageColumn;

            //for (int i = 0; i < (int)EnumAllPicTableColName.maxAllPicColNum; i++)
            //{
            //    if (i == (int)EnumAllPicTableColName.PIC)
            //    {
            //        imageColumn = new DataGridViewImageColumn();
            //        imageColumn.HeaderText = GeneralClass.sAllPicColumnName[i];//标题
            //        imageColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;//设置图片可以根据单元格大小进行自动调整
            //        dataGridView4.Columns.Add(imageColumn);
            //    }
            //    else
            //    {
            //        column = new DataGridViewColumn();
            //        cell = new DataGridViewTextBoxCell();
            //        column.CellTemplate = cell;//设置单元格模板
            //        column.HeaderText = GeneralClass.sAllPicColumnName[i];//
            //        dataGridView4.Columns.Add(column);
            //    }
            //}


        }
        private void InitDataGridView10()
        {
            InitDGV(dataGridView10);
        }

        private void InitCheckBox()
        {
            checkBox7.Checked = false;
            groupBox7.Enabled = false;

            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
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
        private void InitRadioButtion()
        {
            radioButton1.Text = GeneralClass.sDetailTableItemName[0];
            radioButton2.Text = GeneralClass.sDetailTableItemName[1];
            radioButton3.Text = GeneralClass.sDetailTableItemName[2];
            radioButton4.Text = GeneralClass.sDetailTableItemName[3];
            radioButton5.Text = GeneralClass.sDetailTableItemName[4];
            radioButton6.Text = GeneralClass.sDetailTableItemName[5];
            radioButton7.Text = GeneralClass.sDetailTableItemName[6];
            radioButton8.Text = GeneralClass.sDetailTableItemName[7];
            radioButton9.Text = GeneralClass.sDetailTableItemName[8];
            radioButton10.Text = GeneralClass.sDetailTableItemName[9];
            radioButton11.Text = GeneralClass.sDetailTableItemName[10];

            radioButton1.Checked = true;
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

        /// <summary>
        /// 根据picName在imagelist中检索其index序号
        /// </summary>
        /// <param name="picName"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool FindIndexInImagelist(string picName, out int index)
        {
            //string[] imageNames = new string[imageList1.Images.Count];
            //for(int i =0;i<imageList1.Images.Count;i++)//先取得所有图片的名称
            //{
            //    imageNames[i]= imageList1.Images.Keys[i];
            //}

            for (int j = 0; j < imageList1.Images.Count; j++)
            {
                if (imageList1.Images.Keys[j].Equals(picName + ".png"))
                {
                    index = j;
                    return true;
                }
            }
            index = -1;
            return false;
        }

        /// <summary>
        /// 开始图形汇总,并按照棒材线材分开统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                GeneralClass.interactivityData?.printlog(1, "开始统计所有钢筋图形数量，并按照棒材、线材分类");

                GeneralClass.AllRebarList = GeneralClass.SQLiteOpt.GetAllRebarList(GeneralClass.AllRebarTableName);//取得所有的钢筋数据list
                //GeneralClass.ExistRebarPicTypeList = GeneralClass.SQLiteOpt.GetExistedRebarTypeList(GeneralClass.AllRebarList);//得到list中包含的钢筋图形编号列表

                List<RebarData> _bangdata = new List<RebarData>();
                List<RebarData> _xiandata = new List<RebarData>();
                foreach (RebarData _dd in GeneralClass.AllRebarList)
                {
                    if (_dd.Diameter >= 14)
                    {
                        _bangdata.Add(_dd);
                    }
                    else
                    {
                        _xiandata.Add(_dd);
                    }
                }

                List<EnumRebarPicType> _bangPicTypeList = new List<EnumRebarPicType>();
                List<EnumRebarPicType> _xianPicTypeList = new List<EnumRebarPicType>();

                _bangPicTypeList = GeneralClass.SQLiteOpt.GetExistedRebarTypeList(_bangdata);
                _xianPicTypeList = GeneralClass.SQLiteOpt.GetExistedRebarTypeList(_xiandata);

                DataGridViewRow dgvRow = new DataGridViewRow();
                DataGridViewCell dgvCell = new DataGridViewTextBoxCell();
                DataGridViewImageCell dgvImageCell = new DataGridViewImageCell();

                int _num = 0;
                double _weight = 0.0;
                dataGridView4.Rows.Clear();
                dataGridView5.Rows.Clear();


                //fill dgv4
                foreach (EnumRebarPicType _item in _bangPicTypeList)
                {
                    dgvRow = new DataGridViewRow();
                    //图形编号
                    dgvCell = new DataGridViewTextBoxCell();
                    dgvCell.Value = _item.ToString().Substring(2, 5);
                    dgvRow.Cells.Add(dgvCell);



                    //钢筋简图
                    dgvImageCell = new DataGridViewImageCell();
                    string sType = _item.ToString().Substring(2, 5);
                    int _index = -1;
                    if (FindIndexInImagelist(sType, out _index))//按照图形编号查询图片库中的index
                    {
                        dgvImageCell.Value = imageList1.Images[_index];//按照index，显示图形
                    }
                    else
                    {
                        GeneralClass.interactivityData?.printlog(1, "钢筋图片库中找不到编号为:" + sType + "的图片，请手动添加");
                    }
                    dgvRow.Cells.Add(dgvImageCell);

                    _num = 0;
                    _weight = 0;
                    //长度、重量
                    foreach (RebarData _ddd in _bangdata)
                    {
                        if (_ddd.TypeNum == _item.ToString().Substring(2, 5))
                        {
                            _num += _ddd.TotalPieceNum;
                            _weight += _ddd.TotalWeight;
                        }
                    }
                    //数量
                    dgvCell = new DataGridViewTextBoxCell();
                    dgvCell.Value = _num;
                    dgvRow.Cells.Add(dgvCell);

                    //重量
                    dgvCell = new DataGridViewTextBoxCell();
                    dgvCell.Value = _weight;
                    dgvRow.Cells.Add(dgvCell);

                    //备注
                    dgvCell = new DataGridViewTextBoxCell();
                    dgvCell.Value = "";
                    dgvRow.Cells.Add(dgvCell);

                    dataGridView4.Rows.Add(dgvRow);

                }


                //fill dgv5
                foreach (EnumRebarPicType _item in _xianPicTypeList)
                {
                    dgvRow = new DataGridViewRow();

                    //图形编号
                    dgvCell = new DataGridViewTextBoxCell();
                    dgvCell.Value = _item.ToString().Substring(2, 5);
                    dgvRow.Cells.Add(dgvCell);

                    //钢筋简图
                    dgvImageCell = new DataGridViewImageCell();
                    string sType = _item.ToString().Substring(2, 5);
                    int _index = -1;
                    if (FindIndexInImagelist(sType, out _index))//按照图形编号查询图片库中的index
                    {
                        dgvImageCell.Value = imageList1.Images[_index];//按照index，显示图形
                    }
                    else
                    {
                        GeneralClass.interactivityData?.printlog(1, "钢筋图片库中找不到编号为:" + sType + "的图片，请手动添加");
                    }
                    dgvRow.Cells.Add(dgvImageCell);

                    _num = 0;
                    _weight = 0;
                    //长度、重量
                    foreach (RebarData _ddd in _xiandata)
                    {
                        if (_ddd.TypeNum == _item.ToString().Substring(2, 5))
                        {
                            _num += _ddd.TotalPieceNum;
                            _weight += _ddd.TotalWeight;
                        }
                    }
                    //数量
                    dgvCell = new DataGridViewTextBoxCell();
                    dgvCell.Value = _num;
                    dgvRow.Cells.Add(dgvCell);

                    //重量
                    dgvCell = new DataGridViewTextBoxCell();
                    dgvCell.Value = _weight;
                    dgvRow.Cells.Add(dgvCell);

                    //备注
                    dgvCell = new DataGridViewTextBoxCell();
                    dgvCell.Value = "";
                    dgvRow.Cells.Add(dgvCell);

                    dataGridView5.Rows.Add(dgvRow);

                }




                //if (GeneralClass.ExistRebarPicTypeList != null && GeneralClass.ExistRebarPicTypeList.Count != 0)
                //{
                //    List<string> colList = new List<string>();//准备查询的列名
                //    colList.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TYPE_NAME]);
                //    colList.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.REBAR_PIC]);
                //    colList.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TOTAL_PIECE_NUM]);
                //    colList.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.LENGTH]);
                //    colList.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.ISMULTI]);

                //    DataGridViewRow dgvRow = new DataGridViewRow();
                //    DataGridViewCell dgvCell = new DataGridViewTextBoxCell();
                //    DataGridViewImageCell dgvImageCell = new DataGridViewImageCell();

                //    int _total_num = 0;

                //    double maxlength = 0.0, minlength = 10000.0;

                //    dataGridView4.Rows.Clear();
                //    dataGridView5.Rows.Clear();

                //    //fill dgv4
                //    //foreach (var item in GeneralClass.SQLiteOpt.ExistRebarPicTypeList)
                //    foreach (var item in GeneralClass.ExistRebarPicTypeList)

                //    {
                //        dgvRow = new DataGridViewRow();//clear
                //        DataTable dt = GeneralClass.SQLiteOpt.FindAllPic(GeneralClass.AllRebarTableName, colList, EnumRebarType.BANG, item);

                //        if (dt != null && dt.Rows.Count != 0)
                //        {


                //            maxlength = 0.0;
                //            minlength = 10000.0;

                //            //图形编号
                //            dgvCell = new DataGridViewTextBoxCell();
                //            dgvCell.Value = item.ToString().Substring(2, item.ToString().Length - 2);
                //            dgvRow.Cells.Add(dgvCell);


                //            foreach (DataRow row in dt.Rows)
                //            {
                //                _total_num += Convert.ToInt32(row[2]);

                //                //暂时关闭
                //                //double tt = Convert.ToDouble(row[3]);
                //                //if (tt > maxlength) { maxlength = tt; } //取最大值
                //                //if (tt < minlength) { minlength = tt; } //取最小值
                //            }

                //            //钢筋简图
                //            dgvImageCell = new DataGridViewImageCell();
                //            string sType = item.ToString().Substring(2, item.ToString().Length - 2);
                //            int _index = -1;
                //            if (FindIndexInImagelist(sType, out _index))//按照图形编号查询图片库中的index
                //            {
                //                dgvImageCell.Value = imageList1.Images[_index];//按照index，显示图形
                //            }
                //            else
                //            {
                //                GeneralClass.interactivityData?.printlog(1, "钢筋图片库中找不到编号为:" + sType + "的图片，请手动添加");
                //            }
                //            dgvRow.Cells.Add(dgvImageCell);

                //            //数量
                //            dgvCell = new DataGridViewTextBoxCell();
                //            dgvCell.Value = _total_num;
                //            dgvRow.Cells.Add(dgvCell);

                //            //最大值
                //            dgvCell = new DataGridViewTextBoxCell();
                //            dgvCell.Value = maxlength;
                //            dgvRow.Cells.Add(dgvCell);

                //            //最小值
                //            dgvCell = new DataGridViewTextBoxCell();
                //            dgvCell.Value = minlength;
                //            dgvRow.Cells.Add(dgvCell);

                //            //备注
                //            dgvCell = new DataGridViewTextBoxCell();
                //            dgvCell.Value = "";
                //            dgvRow.Cells.Add(dgvCell);

                //            dataGridView4.Rows.Add(dgvRow);

                //        }

                //    }

                //    //fill dgv5
                //    //foreach (var item in GeneralClass.SQLiteOpt.ExistRebarPicTypeList)
                //    foreach (var item in GeneralClass.ExistRebarPicTypeList)
                //    {
                //        dgvRow = new DataGridViewRow();//clear
                //        DataTable dt = GeneralClass.SQLiteOpt.FindAllPic(GeneralClass.AllRebarTableName, colList, EnumRebarType.XIAN, item);

                //        if (dt != null && dt.Rows.Count != 0)
                //        {
                //            maxlength = 0.0;
                //            minlength = 10000.0;

                //            //图形编号
                //            dgvCell = new DataGridViewTextBoxCell();
                //            dgvCell.Value = item.ToString().Substring(2, item.ToString().Length - 2);
                //            dgvRow.Cells.Add(dgvCell);

                //            //if (dt != null)
                //            //{
                //            foreach (DataRow row in dt.Rows)
                //            {
                //                _total_num += Convert.ToInt32(row[2]);

                //                double tt = Convert.ToDouble(row[3]);
                //                if (tt > maxlength) { maxlength = tt; } //取最大值
                //                if (tt < minlength) { minlength = tt; } //取最小值
                //            }
                //            //}

                //            //钢筋简图
                //            //dgvCell = new DataGridViewTextBoxCell();
                //            //dgvCell.Value = "";
                //            //dgvRow.Cells.Add(dgvCell);
                //            dgvImageCell = new DataGridViewImageCell();
                //            string sType = item.ToString().Substring(2, item.ToString().Length - 2);
                //            int _index = -1;
                //            if (FindIndexInImagelist(sType, out _index))//按照图形编号查询图片库中的index
                //            {
                //                dgvImageCell.Value = imageList1.Images[_index];//按照index，显示图形
                //            }
                //            else
                //            {
                //                GeneralClass.interactivityData?.printlog(1, "钢筋图片库中找不到编号为:" + sType + "的图片，请手动添加");
                //            }
                //            dgvRow.Cells.Add(dgvImageCell);

                //            //数量
                //            dgvCell = new DataGridViewTextBoxCell();
                //            dgvCell.Value = _total_num;
                //            dgvRow.Cells.Add(dgvCell);

                //            //最大值
                //            dgvCell = new DataGridViewTextBoxCell();
                //            dgvCell.Value = maxlength;
                //            dgvRow.Cells.Add(dgvCell);

                //            //最小值
                //            dgvCell = new DataGridViewTextBoxCell();
                //            dgvCell.Value = minlength;
                //            dgvRow.Cells.Add(dgvCell);

                //            //备注
                //            dgvCell = new DataGridViewTextBoxCell();
                //            dgvCell.Value = "";
                //            dgvRow.Cells.Add(dgvCell);

                //            dataGridView5.Rows.Add(dgvRow);

                //        }

                //    }

                //    GeneralClass.interactivityData?.printlog(1, "钢筋图形数量统计完成！");

                //}
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void GetSheetToDGV()
        {
            //先获取备份的钢筋总表
            List<RebarData> _allList_bk = GeneralClass.SQLiteOpt.GetAllRebarList(GeneralClass.AllRebarBKTableName);

            //查询所有被选中的钢筋
            List<RebarData> _newlist = new List<RebarData>();
            foreach (RebarData _data in _allList_bk)
            {
                if (IfRebarSelected(_data))//查询钢筋是否被选中
                {
                    _newlist.Add(_data);
                }
            }

            if (_newlist.Count != 0)
            {
                FillDGVWithRebarList(_newlist, dataGridView1);
            }



        }


        private void FillDGVWithRebarList(List<RebarData> _list, DataGridView _dgv)
        {
            _dgv.Rows.Clear();//清空
            DataGridViewRow dgvRow;
            DataGridViewCell dgvCell;
            DataGridViewImageCell dgvImageCell;

            foreach (RebarData _dd in _list)
            {
                dgvRow = new DataGridViewRow();

                //1子构件名
                dgvCell = new DataGridViewTextBoxCell();
                dgvCell.Value = _dd.ElementName;
                dgvRow.Cells.Add(dgvCell);

                //2图形编号
                dgvCell = new DataGridViewTextBoxCell();
                dgvCell.Value = _dd.TypeNum;
                dgvRow.Cells.Add(dgvCell);

                //3级别
                dgvCell = new DataGridViewTextBoxCell();
                dgvCell.Value = _dd.Level;
                dgvRow.Cells.Add(dgvCell);

                //4直径
                dgvCell = new DataGridViewTextBoxCell();
                dgvCell.Value = _dd.Diameter;
                dgvRow.Cells.Add(dgvCell);

                //5钢筋简图
                dgvImageCell = new DataGridViewImageCell();
                string sType = _dd.TypeNum;
                if (sType == "") continue;  //类型为空，跳过加载图片
                int _iiiii = -1;
                if (FindIndexInImagelist(sType, out _iiiii))//按照图形编号查询图片库中的index
                {
                    dgvImageCell.Value = imageList1.Images[_iiiii];//按照index，显示图形
                }
                else
                {
                    GeneralClass.interactivityData?.printlog(1, "钢筋图片库中找不到编号为:" + sType + "的图片，请手动添加");
                }
                dgvRow.Cells.Add(dgvImageCell);

                ////6图片信息
                //dgvCell = new DataGridViewTextBoxCell();
                //dgvCell.Value = _dd.PicMessage;
                //dgvRow.Cells.Add(dgvCell);

                //7边角结构
                dgvCell = new DataGridViewTextBoxCell();
                dgvCell.Value = _dd.CornerMessage;
                dgvRow.Cells.Add(dgvCell);

                //8下料长度
                dgvCell = new DataGridViewTextBoxCell();
                dgvCell.Value = _dd.Length;
                dgvRow.Cells.Add(dgvCell);

                ////9是否多段
                //dgvCell = new DataGridViewTextBoxCell();
                //dgvCell.Value = _dd.IsMulti;
                //dgvRow.Cells.Add(dgvCell);

                //10根数件数
                dgvCell = new DataGridViewTextBoxCell();
                dgvCell.Value = _dd.PieceNumUnitNum;
                dgvRow.Cells.Add(dgvCell);

                //11总根数
                dgvCell = new DataGridViewTextBoxCell();
                dgvCell.Value = _dd.TotalPieceNum;
                dgvRow.Cells.Add(dgvCell);

                //12总重量
                dgvCell = new DataGridViewTextBoxCell();
                dgvCell.Value = _dd.TotalWeight;
                dgvRow.Cells.Add(dgvCell);

                //13备注
                dgvCell = new DataGridViewTextBoxCell();
                dgvCell.Value = _dd.Description;
                dgvRow.Cells.Add(dgvCell);

                //14标注序号
                dgvCell = new DataGridViewTextBoxCell();
                dgvCell.Value = _dd.SerialNum;
                dgvRow.Cells.Add(dgvCell);

                ////15是否原材
                //dgvCell = new DataGridViewTextBoxCell();
                //dgvCell.Value = _dd.IsOriginal;
                //dgvRow.Cells.Add(dgvCell);

                ////16是否套丝
                //dgvCell = new DataGridViewTextBoxCell();
                //dgvCell.Value = _dd.IfTao;
                //dgvRow.Cells.Add(dgvCell);

                ////17是否弯曲
                //dgvCell = new DataGridViewTextBoxCell();
                //dgvCell.Value = _dd.IfBend;
                //dgvRow.Cells.Add(dgvCell);

                ////18是否切断
                //dgvCell = new DataGridViewTextBoxCell();
                //dgvCell.Value = _dd.IfCut;
                //dgvRow.Cells.Add(dgvCell);

                ////19是否弯曲两次以上
                //dgvCell = new DataGridViewTextBoxCell();
                //dgvCell.Value = _dd.IfBendTwice;
                //dgvRow.Cells.Add(dgvCell);

                _dgv.Rows.Add(dgvRow);

            }

        }
        /// <summary>
        /// 从excel表的第_index个sheet中取数据，存入dgv1
        /// </summary>
        /// <param name="_index"></param>
        private void GetSheetToDGV(int _index)
        {
            try
            {
                ISheet sheet = GeneralClass.readEXCEL.wb.GetSheetAt(_index);

                dataGridView1.Rows.Clear();//清空
                DataGridViewRow dgvRow = new DataGridViewRow();
                DataGridViewCell dgvCell = new DataGridViewTextBoxCell();
                DataGridViewImageCell dgvImageCell = new DataGridViewImageCell();

                RebarData rebarData = new RebarData();

                //从第三行开始读取，前三行为标题
                for (int i = sheet.FirstRowNum + 3; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;

                    // 读取单元格数据
                    //DataGridViewRow dgvRow = row as DataGridViewRow;
                    //dataGridView1.Rows.Add(dgvRow);//添加一行


                    dgvRow = new DataGridViewRow();
                    rebarData.init();   //初始化钢筋对象
                    string tt = "";

                    for (int j = row.FirstCellNum; j < row.LastCellNum; j++)
                    {
                        ICell cell = row.GetCell(j);
                        if (cell == null) continue;

                        if (j == 1)//图形编号列，存入图形编号
                        {
                            tt = GeneralClass.readEXCEL.getCellStringValueAllCase(cell);
                            rebarData.TypeNum = tt;
                            dgvCell = new DataGridViewTextBoxCell();
                            dgvCell.Value = tt;
                            dgvRow.Cells.Add(dgvCell);
                        }
                        else if (j == 3) //钢筋简图列，需加入图片cell
                        {
                            dgvImageCell = new DataGridViewImageCell();
                            string sType = rebarData.TypeNum;
                            if (sType == "") continue;  //类型为空，跳过加载图片
                            int _iiiii = -1;
                            if (FindIndexInImagelist(sType, out _iiiii))//按照图形编号查询图片库中的index
                            {
                                dgvImageCell.Value = imageList1.Images[_iiiii];//按照index，显示图形
                            }
                            else
                            {
                                GeneralClass.interactivityData?.printlog(1, "钢筋图片库中找不到编号为:" + sType + "的图片，请手动添加");
                            }
                            dgvRow.Cells.Add(dgvImageCell);
                        }
                        else if (j == 2) //级别直径列，需要分拆为级别和直径两项
                        {
                            tt = GeneralClass.readEXCEL.getCellStringValueAllCase(cell);

                            dgvCell = new DataGridViewTextBoxCell();
                            dgvCell.Value = (tt != "") ? tt.Substring(0, 1) : "";//级别,注意有空行的情况，此处做个判断
                            dgvRow.Cells.Add(dgvCell);

                            dgvCell = new DataGridViewTextBoxCell();
                            dgvCell.Value = (tt != "") ? tt.Substring(1, tt.Length - 1) : "";//直径,注意有空行的情况，此处做个判断
                            dgvRow.Cells.Add(dgvCell);
                        }
                        else if (j == 6)   //下料长度，分拆为长度和是否多段两项
                        {
                            tt = GeneralClass.readEXCEL.getCellStringValueAllCase(cell);
                            string[] sss = tt.Split('\n');

                            if (sss.Length > 1)
                            {
                                dgvCell = new DataGridViewTextBoxCell();
                                foreach (string item in sss)
                                {
                                    dgvCell.Value += item + "_";
                                }
                                dgvRow.Cells.Add(dgvCell);

                                dgvCell = new DataGridViewTextBoxCell();
                                dgvCell.Value = 1;
                                dgvRow.Cells.Add(dgvCell);
                            }
                            else
                            {
                                dgvCell = new DataGridViewTextBoxCell();
                                dgvCell.Value = tt;
                                dgvRow.Cells.Add(dgvCell);

                                dgvCell = new DataGridViewTextBoxCell();
                                dgvCell.Value = 0;
                                dgvRow.Cells.Add(dgvCell);
                            }
                        }
                        else
                        {
                            dgvCell = new DataGridViewTextBoxCell();
                            dgvCell.Value = GeneralClass.readEXCEL.getCellStringValueAllCase(cell);//根据cell数据的不同类型分别解析
                            dgvRow.Cells.Add(dgvCell);
                        }
                    }

                    dataGridView1.Rows.Add(dgvRow);

                }

            }
            catch (Exception ex) { MessageBox.Show("GetSheetToDGV error:" + ex.Message); }
        }
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
                    GetSheetToDGV();
                    tabControl1.SelectedIndex = 1;
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

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                GeneralClass.interactivityData?.printlog(1, "开始进行料单的详细分析");

                GeneralClass.AllRebarList = GeneralClass.SQLiteOpt.GetAllRebarList(GeneralClass.AllRebarTableName);

                ////定义三维数组，尺寸*工艺*分析项
                //object[,,] _alldata = new object[(int)EnumDetailTableRowName.maxRowNum, (int)EnumDetailTableColName.ONLY_CUT, (int)EnumDetailItem.maxItemNum];//先处理三个原材的

                GeneralClass.AllDetailData = GeneralClass.SQLiteOpt.DetailAnalysis(GeneralClass.AllRebarList);


                GeneralClass.AllDetailOtherData = GeneralClass.SQLiteOpt.DetailAnalysis2(GeneralClass.AllRebarList, checkBox7.Checked, checkBox4.Checked, checkBox5.Checked, checkBox6.Checked);

                GeneralClass.interactivityData?.printlog(1, "料单详细分析完成，请选择统计项");

                //汇总所有图形的list
                List<RebarData> _newdatalist = new List<RebarData>();
                foreach (RebarData _dd in GeneralClass.AllRebarList)//先把rebardata列表里面筛出原材和多段的
                {
                    if (!_dd.IsMulti && !_dd.IsOriginal)
                    {
                        _newdatalist.Add(_dd);
                    }
                }
                GeneralClass.ExistRebarPicTypeList = GeneralClass.SQLiteOpt.GetExistedRebarTypeList(_newdatalist);//得到列表中包含的钢筋图形编号列表



                dataGridView6.Rows.Clear();
                //重新添加行
                string sType = "";
                int _index = -1;

                DataGridViewRow row = new DataGridViewRow();
                for (int j = 0; j < (int)EnumDetailTableColName.ONLY_CUT; j++)//添加原材的row
                {
                    row = new DataGridViewRow();
                    row.HeaderCell.Value = GeneralClass.sDetailTableColName[j];//
                    dataGridView6.Rows.Add(row);
                }
                if (FindIndexInImagelist("10000", out _index))//按照图形编号查询图片库中的index
                {
                    dataGridView6.Rows[0].Cells[0].Value = imageList1.Images[_index];//按照index，显示图形
                }
                if (FindIndexInImagelist("10000", out _index))//按照图形编号查询图片库中的index
                {
                    dataGridView6.Rows[1].Cells[0].Value = imageList1.Images[_index];//按照index，显示图形
                }
                if (FindIndexInImagelist("20100", out _index))//按照图形编号查询图片库中的index
                {
                    dataGridView6.Rows[2].Cells[0].Value = imageList1.Images[_index];//按照index，显示图形
                }

                for (int i = 0; i < GeneralClass.ExistRebarPicTypeList.Count; i++)//添加非原材的row
                {
                    row = new DataGridViewRow();
                    row.HeaderCell.Value = GeneralClass.ExistRebarPicTypeList[i].ToString().Substring(2, 5);//
                    dataGridView6.Rows.Add(row);

                    sType = GeneralClass.ExistRebarPicTypeList[i].ToString().Substring(2, 5);
                    if (FindIndexInImagelist(sType, out _index))//按照图形编号查询图片库中的index
                    {
                        dataGridView6.Rows[i + 3].Cells[0].Value = imageList1.Images[_index];//按照index，显示图形
                    }
                }

                //添加汇总行
                row = new DataGridViewRow();
                row.HeaderCell.Value = "总计";
                dataGridView6.Rows.Add(row);


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                string sqlstr = "";

                sqlstr = "select * from " + GeneralClass.AllRebarTableName + " where ";

                if (textBox1.Text != "")
                {
                    sqlstr += "(" + GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.DIAMETER] + textBox1.Text + " )";  //棒材钢筋
                    sqlstr += " AND ";
                }

                sqlstr += "(" + GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.ISMULTI] + "=" + (checkBox1.Checked ? "1" : "0") + " )";  //棒材钢筋

                if (textBox2.Text != "")
                {
                    sqlstr += " AND ";

                    sqlstr += "(" + GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.LENGTH] + textBox2.Text + " )";  //棒材钢筋

                }

                DataTable dt = GeneralClass.SQLiteOpt.ExecuteQuery(sqlstr, null);



                dataGridView7.DataSource = dt;


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void FillDGV6(EnumDetailItem _item)
        {
            try
            {
                bool _havedata_d = false;//原材的部分有数据
                bool _havedata_o = false;//非原材的部分有数据
                //原材数据处理
                if (GeneralClass.AllDetailData != null)
                {

                    for (int i = 0; i < (int)EnumDetailTableRowName.maxRowNum; i++)
                    {
                        for (int j = 0; j < (int)EnumDetailTableColName.ONLY_CUT; j++)
                        {
                            if (GeneralClass.AllDetailData[j, i, (int)_item] != null)
                            {
                                _havedata_d = true;
                                if (_item == EnumDetailItem.TOTAL_WEIGHT)
                                {
                                    double _weight = (double)GeneralClass.AllDetailData[j, i, (int)_item];
                                    dataGridView6.Rows[j].Cells[i + 1].Value = Math.Round(_weight, 1);

                                }
                                else if (_item == EnumDetailItem.TOTAL_LENGTH)
                                {
                                    int _length = (int)GeneralClass.AllDetailData[j, i, (int)_item];
                                    dataGridView6.Rows[j].Cells[i + 1].Value = _length / 1000;
                                }
                                else
                                {
                                    dataGridView6.Rows[j].Cells[i + 1].Value = GeneralClass.AllDetailData[j, i, (int)_item];
                                }

                            }
                        }
                    }
                }

                //非原材数据处理
                if (GeneralClass.AllDetailOtherData != null)
                {
                    //KeyValuePair<EnumRebarPicType, GeneralDetailData> _pair = new KeyValuePair<EnumRebarPicType, GeneralDetailData>();

                    for (int k = 0; k < (int)EnumDetailTableRowName.maxRowNum; k++)
                    {
                        if (GeneralClass.AllDetailOtherData[k] != null)
                        {
                            _havedata_o = true;
                            for (int h = 0; h < GeneralClass.ExistRebarPicTypeList.Count; h++)
                            {
                                foreach (var item in GeneralClass.AllDetailOtherData[k])
                                {
                                    if (item.Key == GeneralClass.ExistRebarPicTypeList[h])//识别出不同直径的list在existtypelist总表里面存在哪些type
                                    {
                                        switch (_item)
                                        {
                                            case EnumDetailItem.TOTAL_WEIGHT:
                                                {
                                                    double _weight = item.Value.TotalWeight;
                                                    dataGridView6.Rows[h + 3].Cells[k + 1].Value = Math.Round(_weight, 1);
                                                    break;
                                                }
                                            case EnumDetailItem.TOTAL_LENGTH:
                                                {
                                                    int _length = item.Value.TotalLength;
                                                    dataGridView6.Rows[h + 3].Cells[k + 1].Value = _length / 1000;
                                                    break;
                                                }
                                            case EnumDetailItem.TOTAL_PIECE:
                                                {
                                                    dataGridView6.Rows[h + 3].Cells[k + 1].Value = item.Value.TotalPieceNum;
                                                    break;
                                                }
                                            case EnumDetailItem.TAO_SI_NUM:
                                                {
                                                    dataGridView6.Rows[h + 3].Cells[k + 1].Value = item.Value.TaosiNum;
                                                    break;
                                                }
                                            case EnumDetailItem.TAO_TONG_NUM:
                                                {
                                                    dataGridView6.Rows[h + 3].Cells[k + 1].Value = item.Value.TaotongNum;
                                                    break;
                                                }
                                            case EnumDetailItem.ZHENG_SI_TAO_TONG:
                                                {
                                                    dataGridView6.Rows[h + 3].Cells[k + 1].Value = item.Value.TaotongNum_P;
                                                    break;
                                                }
                                            case EnumDetailItem.FAN_SI_TAO_TONG:
                                                {
                                                    dataGridView6.Rows[h + 3].Cells[k + 1].Value = item.Value.TaotongNum_N;
                                                    break;
                                                }
                                            case EnumDetailItem.BIAN_JING_TAO_TONG:
                                                {
                                                    dataGridView6.Rows[h + 3].Cells[k + 1].Value = item.Value.TaotongNum_V;
                                                    break;
                                                }
                                            case EnumDetailItem.CUT_NUM:
                                                {
                                                    dataGridView6.Rows[h + 3].Cells[k + 1].Value = item.Value.CutNum;
                                                    break;
                                                }
                                            case EnumDetailItem.BEND_NUM:
                                                {
                                                    dataGridView6.Rows[h + 3].Cells[k + 1].Value = item.Value.BendNum;
                                                    break;
                                                }
                                            case EnumDetailItem.ZHI_NUM:
                                                {
                                                    dataGridView6.Rows[h + 3].Cells[k + 1].Value = item.Value.StraightenedNum;
                                                    break;
                                                }

                                        }

                                    }
                                }
                            }

                        }

                    }
                }
                //汇总，数据求和
                if (_havedata_d && _havedata_o)
                {
                    int _total;
                    int ydataTotal = 0;

                    List<string> xData = new List<string>();
                    List<int> yData = new List<int>();
                    for (int u = 0; u < (int)EnumDetailTableRowName.maxRowNum; u++)
                    {
                        _total = 0;
                        for (int y = 0; y < GeneralClass.ExistRebarPicTypeList.Count + 3; y++)
                        {
                            _total += Convert.ToInt32(dataGridView6.Rows[y].Cells[u + 1].Value);
                        }
                        xData.Add(GeneralClass.sDetailTableRowName[u]);
                        yData.Add(_total);
                        ydataTotal += _total;
                        dataGridView6.Rows[GeneralClass.ExistRebarPicTypeList.Count + 3].Cells[u + 1].Value = _total;
                    }


                    //绘制chart图表
                    chart1.Series[0]["PieLabelStyle"] = "Outside";//将文字移到外侧
                    //chart1.Series[0]["PieLabelStyle"] = "Enabled";//将文字移到外侧

                    chart1.Series[0]["PieLineColor"] = "Black";//绘制黑色的连线。
                    chart1.Series[0].Points.DataBindXY(xData, yData);

                    chart1.Legends[0].Enabled = true;//右侧legend启用
                    chart1.Legends[0].Alignment = StringAlignment.Center;
                    chart1.Legends[0].Docking = Docking.Right;
                    chart1.Legends[0].Title = GeneralClass.sDetailTableItemName[(int)_item];

                    for (int p = 0; p < chart1.Series[0].Points.Count; p++)
                    {
                        //chart1.Series[0].Points[p].Label = "#VALX\n#PERCENT{P0}\n";
                        chart1.Series[0].Points[p].IsVisibleInLegend = true;
                        chart1.Series[0].Points[p].LegendText = xData[p] + " " + (Convert.ToDouble(yData[p]) / Convert.ToDouble(ydataTotal)).ToString("P1");
                    }
                    //Chart1.Series["Series1"].IsXValueIndexed = false;
                    //Chart1.Series["Series1"].IsValueShownAsLabel = false;
                    //Chart1.Series["Series1"]["PieLineColor"] = "Black";//连线颜色
                    //Chart1.Series["Series1"]["PieLabelStyle"] = "Outside";//标签位置
                    //Chart1.Series["Series1"].ToolTip = "#VALX";//显示提示用语



                }

            }
            catch (Exception ex) { MessageBox.Show("FillDGV6 error:" + ex.Message); }

        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                FillDGV6(EnumDetailItem.TOTAL_PIECE);
            }

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                FillDGV6(EnumDetailItem.TOTAL_WEIGHT);

            }

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                FillDGV6(EnumDetailItem.TOTAL_LENGTH);

            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                FillDGV6(EnumDetailItem.TAO_SI_NUM);

            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                FillDGV6(EnumDetailItem.TAO_TONG_NUM);

            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                FillDGV6(EnumDetailItem.ZHENG_SI_TAO_TONG);

            }
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                FillDGV6(EnumDetailItem.FAN_SI_TAO_TONG);

            }
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                FillDGV6(EnumDetailItem.BIAN_JING_TAO_TONG);

            }
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                FillDGV6(EnumDetailItem.CUT_NUM);

            }
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                FillDGV6(EnumDetailItem.BEND_NUM);


            }
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                FillDGV6(EnumDetailItem.ZHI_NUM);

            }
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

                tabControl1.Enabled = true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked)
            {
                groupBox7.Enabled = true;
            }
            else
            {
                groupBox7.Enabled = false;
            }
        }

        private void FillDGV_original(List<RebarData> _list, ref DataGridView _dgv, out int total_num, out double total_weight)
        {
            int dgv_no = 0;
            if (_dgv == dataGridView15)
            {
                dgv_no = 1;
            }
            else if (_dgv == dataGridView16)
            {
                dgv_no = 2;
            }
            else if (_dgv == dataGridView17)
            {
                dgv_no = 3;
            }
            else if (_dgv == dataGridView18)
            {
                dgv_no = 4;
            }
            else
            {
                dgv_no = 0;
            }

            int[] _num = new int[(int)EnumRebarBang.maxRebarBangNum];                //按照14,16,18,20,22,25,28,32,36,40十种直径来统计
            double[] _weight = new double[(int)EnumRebarBang.maxRebarBangNum];
            total_num = 0;
            total_weight = 0;

            DataTable dt_z = new DataTable();
            dt_z.Columns.Add("直径", typeof(string));
            dt_z.Columns.Add("数量", typeof(int));
            dt_z.Columns.Add("数量百分比", typeof(double));
            dt_z.Columns.Add("重量(kg)", typeof(double));
            dt_z.Columns.Add("重量百分比", typeof(double));

            //_list已经过滤过，棒材、原材、多段
            foreach (RebarData _ddd in _list)
            {
                switch (dgv_no)
                {
                    case 1:
                        {
                            if (!_ddd.IfBend && !_ddd.IfTao)   //不弯不套
                            {
                                for (int i = (int)EnumRebarBang.BANG_C14; i < (int)EnumRebarBang.maxRebarBangNum; i++)
                                {
                                    if (_ddd.Diameter == Convert.ToInt32(((EnumRebarBang)i).ToString().Substring(6, 2)))//BANG_C14
                                    {
                                        _num[i] += _ddd.TotalPieceNum;
                                        _weight[i] += _ddd.TotalWeight;
                                    }
                                }
                            }
                        }
                        break;
                    case 2:
                        {
                            if (!_ddd.IfBend && _ddd.IfTao)   //不弯仅套
                            {
                                for (int i = (int)EnumRebarBang.BANG_C14; i < (int)EnumRebarBang.maxRebarBangNum; i++)
                                {
                                    if (_ddd.Diameter == Convert.ToInt32(((EnumRebarBang)i).ToString().Substring(6, 2)))//BANG_C14
                                    {
                                        _num[i] += _ddd.TotalPieceNum;
                                        _weight[i] += _ddd.TotalWeight;
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        {
                            if (_ddd.IfBend && !_ddd.IfTao)   //不套仅弯
                            {
                                for (int i = (int)EnumRebarBang.BANG_C14; i < (int)EnumRebarBang.maxRebarBangNum; i++)
                                {
                                    if (_ddd.Diameter == Convert.ToInt32(((EnumRebarBang)i).ToString().Substring(6, 2)))//BANG_C14
                                    {
                                        _num[i] += _ddd.TotalPieceNum;
                                        _weight[i] += _ddd.TotalWeight;
                                    }
                                }
                            }
                        }
                        break;
                    case 4:
                        {
                            if (_ddd.IfBend && _ddd.IfTao)   //又套又弯
                            {
                                for (int i = (int)EnumRebarBang.BANG_C14; i < (int)EnumRebarBang.maxRebarBangNum; i++)
                                {
                                    if (_ddd.Diameter == Convert.ToInt32(((EnumRebarBang)i).ToString().Substring(6, 2)))//BANG_C14
                                    {
                                        _num[i] += _ddd.TotalPieceNum;
                                        _weight[i] += _ddd.TotalWeight;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        break;

                }
            }
            for (int i = (int)EnumRebarBang.BANG_C14; i < (int)EnumRebarBang.maxRebarBangNum; i++)
            {
                total_num += _num[i];
                total_weight += _weight[i];
            }
            for (int i = (int)EnumRebarBang.BANG_C14; i < (int)EnumRebarBang.maxRebarBangNum; i++)
            {
                dt_z.Rows.Add("Φ" + ((EnumRebarBang)i).ToString().Substring(6, 2), _num[i], (double)_num[i] / (double)total_num, _weight[i], _weight[i] / total_weight);
            }

            _dgv.DataSource = dt_z;
            _dgv.Columns[2].DefaultCellStyle.Format = "P0";         //百分比
            _dgv.Columns[3].DefaultCellStyle.Format = "0.0";        //double保留两位小数
            _dgv.Columns[4].DefaultCellStyle.Format = "P0";         //百分比

        }
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                GeneralClass.interactivityData?.printlog(1, "开始统计所有棒材原材");

                GeneralClass.AllRebarList = GeneralClass.SQLiteOpt.GetAllRebarList(GeneralClass.AllRebarTableName);//取得所有的钢筋数据list
                //GeneralClass.ExistRebarPicTypeList = GeneralClass.SQLiteOpt.GetExistedRebarTypeList(GeneralClass.AllRebarList);//得到list中包含的钢筋图形编号列表

                List<RebarData> _bangdata = new List<RebarData>();
                foreach (RebarData _dd in GeneralClass.AllRebarList)
                {
                    if ((_dd.Diameter >= 14) && (_dd.IsMulti || _dd.IsOriginal))
                    {
                        _bangdata.Add(_dd);
                    }
                }
                int[] total_num = new int[4];
                double[] total_weight = new double[4];
                FillDGV_original(_bangdata, ref dataGridView15, out total_num[0], out total_weight[0]);
                FillDGV_original(_bangdata, ref dataGridView16, out total_num[1], out total_weight[1]);
                FillDGV_original(_bangdata, ref dataGridView17, out total_num[2], out total_weight[2]);
                FillDGV_original(_bangdata, ref dataGridView18, out total_num[3], out total_weight[3]);

                //汇总统计
                DataTable dt_hz = new DataTable();

                dt_hz.Columns.Add("类型", typeof(string));
                dt_hz.Columns.Add("数量(根)", typeof(int));
                dt_hz.Columns.Add("数量百分比", typeof(double));
                dt_hz.Columns.Add("重量(kg)", typeof(double));
                dt_hz.Columns.Add("重量百分比", typeof(double));

                int all_num = 0;
                double all_weight = 0;
                for (int i = 0; i < 4; i++)
                {
                    all_num += total_num[i];
                    all_weight += total_weight[i];
                }
                dt_hz.Rows.Add("原材不处理", total_num[0], (double)total_num[0] / (double)all_num, total_weight[0], total_weight[0] / all_weight);
                dt_hz.Rows.Add("原材仅套丝", total_num[1], (double)total_num[1] / (double)all_num, total_weight[1], total_weight[1] / all_weight);
                dt_hz.Rows.Add("原材仅弯曲", total_num[2], (double)total_num[2] / (double)all_num, total_weight[2], total_weight[2] / all_weight);
                dt_hz.Rows.Add("原材套丝弯曲", total_num[3], (double)total_num[3] / (double)all_num, total_weight[3], total_weight[3] / all_weight);
                dt_hz.Rows.Add("总计", all_num, (double)all_num / (double)all_num, all_weight, all_weight / all_weight);


                dataGridView24.DataSource = dt_hz;
                dataGridView24.Columns[2].DefaultCellStyle.Format = "P0";
                dataGridView24.Columns[3].DefaultCellStyle.Format = "0.00";
                dataGridView24.Columns[4].DefaultCellStyle.Format = "P0";

                GeneralClass.interactivityData?.printlog(1, "所有棒材原材数据统计完成");
                //FillDGVWithRebarList(_bangdata, dataGridView8);
                //GeneralClass.interactivityData?.printlog(1, "dgv填充完成");



            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                GeneralClass.interactivityData?.printlog(1, "开始统计所有线材");

                GeneralClass.AllRebarList = GeneralClass.SQLiteOpt.GetAllRebarList(GeneralClass.AllRebarTableName);//取得所有的钢筋数据list

                List<RebarData> _xiandata = new List<RebarData>();
                foreach (RebarData _dd in GeneralClass.AllRebarList)
                {
                    if (_dd.Diameter <= 12)
                    {
                        _xiandata.Add(_dd);
                    }
                }

                int[] _num = new int[4];                //按照6，8，10，12四种直径来统计
                double[] _weight = new double[4];
                //int total_num;
                //double total_weight;
                //先统计直条的线材
                DataTable dt_z = new DataTable();
                dt_z.Columns.Add("直径", typeof(string));
                dt_z.Columns.Add("数量", typeof(int));
                dt_z.Columns.Add("数量百分比", typeof(double));
                dt_z.Columns.Add("重量(kg)", typeof(double));
                dt_z.Columns.Add("重量百分比", typeof(double));

                foreach (RebarData _ddd in _xiandata)
                {
                    if (_ddd.TypeNum == "10000")
                    {
                        if (_ddd.Diameter == 6)
                        {
                            _num[0] += _ddd.TotalPieceNum;
                            _weight[0] += _ddd.TotalWeight;
                        }
                        else if (_ddd.Diameter == 8)
                        {
                            _num[1] += _ddd.TotalPieceNum;
                            _weight[1] += _ddd.TotalWeight;
                        }
                        else if (_ddd.Diameter == 10)
                        {
                            _num[2] += _ddd.TotalPieceNum;
                            _weight[2] += _ddd.TotalWeight;
                        }
                        else
                        {
                            _num[3] += _ddd.TotalPieceNum;
                            _weight[3] += _ddd.TotalWeight;
                        }
                    }
                }
                int total_num_z = _num[0] + _num[1] + _num[2] + _num[3];
                double total_weight_z = _weight[0] + _weight[1] + _weight[2] + _weight[3];
                dt_z.Rows.Add("Φ6", _num[0], (double)_num[0] / (double)total_num_z, _weight[0], (_weight[0] / total_weight_z));
                dt_z.Rows.Add("Φ8", _num[1], (double)_num[1] / (double)total_num_z, _weight[1], (_weight[1] / total_weight_z));
                dt_z.Rows.Add("Φ10", _num[2], (double)_num[2] / (double)total_num_z, _weight[2], (_weight[2] / total_weight_z));
                dt_z.Rows.Add("Φ12", _num[3], (double)_num[3] / (double)total_num_z, _weight[3], (_weight[3] / total_weight_z));
                //dt_z.Columns[2].ExtendedProperties["Format"] = "P1";//指定列属性为百分比，保留一位小数
                //dt_z.Columns[4].ExtendedProperties["Format"] = "P1";//指定列属性为百分比，保留一位小数
                dataGridView9.DataSource = dt_z;
                dataGridView9.Columns[2].DefaultCellStyle.Format = "P0";
                dataGridView9.Columns[3].DefaultCellStyle.Format = "0.0";
                dataGridView9.Columns[4].DefaultCellStyle.Format = "P0";


                //统计弯曲的线材
                DataTable dt_w = new DataTable();//弯曲线材
                _num = new int[4];      //清空
                _weight = new double[4];

                dt_w.Columns.Add("直径", typeof(string));
                dt_w.Columns.Add("数量", typeof(int));
                dt_w.Columns.Add("数量百分比", typeof(double));
                dt_w.Columns.Add("重量(kg)", typeof(double));
                dt_w.Columns.Add("重量百分比", typeof(double));

                foreach (RebarData _ddd in _xiandata)
                {
                    if (_ddd.TypeNum != "10000")
                    {
                        if (_ddd.Diameter == 6)
                        {
                            _num[0] += _ddd.TotalPieceNum;
                            _weight[0] += _ddd.TotalWeight;
                        }
                        else if (_ddd.Diameter == 8)
                        {
                            _num[1] += _ddd.TotalPieceNum;
                            _weight[1] += _ddd.TotalWeight;
                        }
                        else if (_ddd.Diameter == 10)
                        {
                            _num[2] += _ddd.TotalPieceNum;
                            _weight[2] += _ddd.TotalWeight;
                        }
                        else
                        {
                            _num[3] += _ddd.TotalPieceNum;
                            _weight[3] += _ddd.TotalWeight;
                        }
                    }
                }
                int total_num_w = _num[0] + _num[1] + _num[2] + _num[3];
                double total_weight_w = _weight[0] + _weight[1] + _weight[2] + _weight[3];
                dt_w.Rows.Add("Φ6", _num[0], (double)_num[0] / (double)total_num_w, _weight[0], (_weight[0] / total_weight_w));
                dt_w.Rows.Add("Φ8", _num[1], (double)_num[1] / (double)total_num_w, _weight[1], (_weight[1] / total_weight_w));
                dt_w.Rows.Add("Φ10", _num[2], (double)_num[2] / (double)total_num_w, _weight[2], (_weight[2] / total_weight_w));
                dt_w.Rows.Add("Φ12", _num[3], (double)_num[3] / (double)total_num_w, _weight[3], (_weight[3] / total_weight_w));
                //dt_w.Columns[2].ExtendedProperties["Format"] = "P1";//指定列属性为百分比，保留一位小数
                //dt_w.Columns[4].ExtendedProperties["Format"] = "P1";//指定列属性为百分比，保留一位小数

                dataGridView12.DataSource = dt_w;
                dataGridView12.Columns[2].DefaultCellStyle.Format = "P0";
                dataGridView12.Columns[3].DefaultCellStyle.Format = "0.00";
                dataGridView12.Columns[4].DefaultCellStyle.Format = "P0";

                //FillDGVWithRebarList(_xiandata, dataGridView9);

                //汇总统计
                DataTable dt_hz = new DataTable();
                _num = new int[2];      //按照
                _weight = new double[2];
                dt_hz.Columns.Add("类型", typeof(string));
                dt_hz.Columns.Add("数量(根)", typeof(int));
                dt_hz.Columns.Add("数量百分比", typeof(double));
                dt_hz.Columns.Add("重量(kg)", typeof(double));
                dt_hz.Columns.Add("重量百分比", typeof(double));

                //dt_x.Rows.Add("Φ6", _num[0], (double)_num[0] / (double)total_num_x, _weight[0], (_weight[0] / total_weight_x));
                dt_hz.Rows.Add("直条", total_num_z, (double)total_num_z / (double)(total_num_z + total_num_w), total_weight_z, total_weight_z / (total_weight_z + total_weight_w));
                dt_hz.Rows.Add("弯曲", total_num_w, (double)total_num_w / (double)(total_num_z + total_num_w), total_weight_w, total_weight_w / (total_weight_z + total_weight_w));

                dataGridView23.DataSource = dt_hz;
                dataGridView23.Columns[2].DefaultCellStyle.Format = "P0";
                dataGridView23.Columns[3].DefaultCellStyle.Format = "0.00";
                dataGridView23.Columns[4].DefaultCellStyle.Format = "P0";



            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void FillDGV_NO_original(List<RebarData> _list, ref DataGridView _dgv, out int total_num, out double total_weight)
        {
            int dgv_no = 0;
            if (_dgv == dataGridView19)
            {
                dgv_no = 1;
            }
            else if (_dgv == dataGridView20)
            {
                dgv_no = 2;
            }
            else if (_dgv == dataGridView21)
            {
                dgv_no = 3;
            }
            else if (_dgv == dataGridView22)
            {
                dgv_no = 4;
            }
            else
            {
                dgv_no = 0;
            }

            int[] _num = new int[(int)EnumRebarBang.maxRebarBangNum];                //按照14,16,18,20,22,25,28,32,36,40十种直径来统计
            double[] _weight = new double[(int)EnumRebarBang.maxRebarBangNum];
            total_num = 0;
            total_weight = 0;

            DataTable dt_z = new DataTable();
            dt_z.Columns.Add("直径", typeof(string));
            dt_z.Columns.Add("数量", typeof(int));
            dt_z.Columns.Add("数量百分比", typeof(double));
            dt_z.Columns.Add("重量(kg)", typeof(double));
            dt_z.Columns.Add("重量百分比", typeof(double));

            //_list已经过滤过，棒材、非原材、非多段
            foreach (RebarData _ddd in _list)
            {
                switch (dgv_no)
                {
                    case 1:
                        {
                            if (!_ddd.IfBend && !_ddd.IfTao)   //不弯不套
                            {
                                for (int i = (int)EnumRebarBang.BANG_C14; i < (int)EnumRebarBang.maxRebarBangNum; i++)
                                {
                                    if (_ddd.Diameter == Convert.ToInt32(((EnumRebarBang)i).ToString().Substring(6, 2)))//BANG_C14
                                    {
                                        _num[i] += _ddd.TotalPieceNum;
                                        _weight[i] += _ddd.TotalWeight;
                                    }
                                }
                            }
                        }
                        break;
                    case 2:
                        {
                            if (!_ddd.IfBend && _ddd.IfTao)   //不弯仅套
                            {
                                for (int i = (int)EnumRebarBang.BANG_C14; i < (int)EnumRebarBang.maxRebarBangNum; i++)
                                {
                                    if (_ddd.Diameter == Convert.ToInt32(((EnumRebarBang)i).ToString().Substring(6, 2)))//BANG_C14
                                    {
                                        _num[i] += _ddd.TotalPieceNum;
                                        _weight[i] += _ddd.TotalWeight;
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        {
                            if (_ddd.IfBend && !_ddd.IfTao)   //不套仅弯
                            {
                                for (int i = (int)EnumRebarBang.BANG_C14; i < (int)EnumRebarBang.maxRebarBangNum; i++)
                                {
                                    if (_ddd.Diameter == Convert.ToInt32(((EnumRebarBang)i).ToString().Substring(6, 2)))//BANG_C14
                                    {
                                        _num[i] += _ddd.TotalPieceNum;
                                        _weight[i] += _ddd.TotalWeight;
                                    }
                                }
                            }
                        }
                        break;
                    case 4:
                        {
                            if (_ddd.IfBend && _ddd.IfTao)   //又套又弯
                            {
                                for (int i = (int)EnumRebarBang.BANG_C14; i < (int)EnumRebarBang.maxRebarBangNum; i++)
                                {
                                    if (_ddd.Diameter == Convert.ToInt32(((EnumRebarBang)i).ToString().Substring(6, 2)))//BANG_C14
                                    {
                                        _num[i] += _ddd.TotalPieceNum;
                                        _weight[i] += _ddd.TotalWeight;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        break;

                }
            }
            for (int i = (int)EnumRebarBang.BANG_C14; i < (int)EnumRebarBang.maxRebarBangNum; i++)
            {
                total_num += _num[i];
                total_weight += _weight[i];
            }
            for (int i = (int)EnumRebarBang.BANG_C14; i < (int)EnumRebarBang.maxRebarBangNum; i++)
            {
                dt_z.Rows.Add("Φ" + ((EnumRebarBang)i).ToString().Substring(6, 2), _num[i], (double)_num[i] / (double)total_num, _weight[i], _weight[i] / total_weight);
            }

            _dgv.DataSource = dt_z;
            _dgv.Columns[2].DefaultCellStyle.Format = "P0";         //百分比
            _dgv.Columns[3].DefaultCellStyle.Format = "0.0";        //double保留两位小数
            _dgv.Columns[4].DefaultCellStyle.Format = "P0";         //百分比
        }
        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                GeneralClass.interactivityData?.printlog(1, "开始统计所有棒材非原材");

                GeneralClass.AllRebarList = GeneralClass.SQLiteOpt.GetAllRebarList(GeneralClass.AllRebarTableName);//取得所有的钢筋数据list

                List<RebarData> _bangdata = new List<RebarData>();
                foreach (RebarData _dd in GeneralClass.AllRebarList)
                {
                    if (_dd.Diameter >= 14 && !_dd.IsMulti && !_dd.IsOriginal)
                    {
                        _bangdata.Add(_dd);
                    }
                }

                int[] total_num = new int[4];
                double[] total_weight = new double[4];
                FillDGV_NO_original(_bangdata, ref dataGridView19, out total_num[0], out total_weight[0]);
                FillDGV_NO_original(_bangdata, ref dataGridView20, out total_num[1], out total_weight[1]);
                FillDGV_NO_original(_bangdata, ref dataGridView21, out total_num[2], out total_weight[2]);
                FillDGV_NO_original(_bangdata, ref dataGridView22, out total_num[3], out total_weight[3]);


                //汇总统计
                DataTable dt_hz = new DataTable();

                dt_hz.Columns.Add("类型", typeof(string));
                dt_hz.Columns.Add("数量(根)", typeof(int));
                dt_hz.Columns.Add("数量百分比", typeof(double));
                dt_hz.Columns.Add("重量(kg)", typeof(double));
                dt_hz.Columns.Add("重量百分比", typeof(double));

                int all_num = 0;
                double all_weight = 0;
                for (int i = 0; i < 4; i++)
                {
                    all_num += total_num[i];
                    all_weight += total_weight[i];
                }
                dt_hz.Rows.Add("切不弯不套", total_num[0], (double)total_num[0] / (double)all_num, total_weight[0], total_weight[0] / all_weight);
                dt_hz.Rows.Add("切不弯套", total_num[1], (double)total_num[1] / (double)all_num, total_weight[1], total_weight[1] / all_weight);
                dt_hz.Rows.Add("切弯不套", total_num[2], (double)total_num[2] / (double)all_num, total_weight[2], total_weight[2] / all_weight);
                dt_hz.Rows.Add("切弯套", total_num[3], (double)total_num[3] / (double)all_num, total_weight[3], total_weight[3] / all_weight);
                dt_hz.Rows.Add("总计", all_num, (double)all_num / (double)all_num, all_weight, all_weight / all_weight);

                dataGridView25.DataSource = dt_hz;
                dataGridView25.Columns[2].DefaultCellStyle.Format = "P0";
                dataGridView25.Columns[3].DefaultCellStyle.Format = "0.00";
                dataGridView25.Columns[4].DefaultCellStyle.Format = "P0";
                //FillDGVWithRebarList(_bangdata, dataGridView10);

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                GeneralClass.interactivityData?.printlog(1, "开始按钢筋规格直径分类统计");

                GeneralClass.AllRebarList = GeneralClass.SQLiteOpt.GetAllRebarList(GeneralClass.AllRebarTableName);//取得所有的钢筋数据list

                List<RebarData> _bangdata = new List<RebarData>();
                List<RebarData> _xiandata = new List<RebarData>();
                foreach (RebarData _dd in GeneralClass.AllRebarList)
                {
                    if (_dd.Diameter <= 12)
                    {
                        _xiandata.Add(_dd);
                    }
                    else
                    {
                        _bangdata.Add(_dd);
                    }
                }

                int[] _num = new int[4];                //按照6，8，10，12四种直径来统计
                double[] _weight = new double[4];
                int total_num_x;
                double total_weight_x;
                //先统计线材
                DataTable dt_x = new DataTable();
                dt_x.Columns.Add("直径", typeof(string));
                dt_x.Columns.Add("数量(根)", typeof(int));
                dt_x.Columns.Add("数量百分比", typeof(double));
                dt_x.Columns.Add("重量(kg)", typeof(double));
                dt_x.Columns.Add("重量百分比", typeof(double));

                foreach (RebarData _ddd in _xiandata)
                {
                    if (_ddd.Diameter == 6)
                    {
                        _num[0] += _ddd.TotalPieceNum;
                        _weight[0] += _ddd.TotalWeight;
                    }
                    else if (_ddd.Diameter == 8)
                    {
                        _num[1] += _ddd.TotalPieceNum;
                        _weight[1] += _ddd.TotalWeight;
                    }
                    else if (_ddd.Diameter == 10)
                    {
                        _num[2] += _ddd.TotalPieceNum;
                        _weight[2] += _ddd.TotalWeight;
                    }
                    else
                    {
                        _num[3] += _ddd.TotalPieceNum;
                        _weight[3] += _ddd.TotalWeight;
                    }
                }
                total_num_x = _num[0] + _num[1] + _num[2] + _num[3];
                total_weight_x = _weight[0] + _weight[1] + _weight[2] + _weight[3];
                dt_x.Rows.Add("Φ6", _num[0], (double)_num[0] / (double)total_num_x, _weight[0], (_weight[0] / total_weight_x));
                dt_x.Rows.Add("Φ8", _num[1], (double)_num[1] / (double)total_num_x, _weight[1], (_weight[1] / total_weight_x));
                dt_x.Rows.Add("Φ10", _num[2], (double)_num[2] / (double)total_num_x, _weight[2], (_weight[2] / total_weight_x));
                dt_x.Rows.Add("Φ12", _num[3], (double)_num[3] / (double)total_num_x, _weight[3], (_weight[3] / total_weight_x));
                dataGridView14.DataSource = dt_x;
                dataGridView14.Columns[2].DefaultCellStyle.Format = "P0";
                dataGridView14.Columns[3].DefaultCellStyle.Format = "0.0";
                dataGridView14.Columns[4].DefaultCellStyle.Format = "P0";


                //统计棒材
                DataTable dt_b = new DataTable();//
                _num = new int[(int)EnumRebarBang.maxRebarBangNum];      //按照14、16、18、20、22、25、28、32、36、40总计十种直径
                _weight = new double[(int)EnumRebarBang.maxRebarBangNum];

                dt_b.Columns.Add("直径", typeof(string));
                dt_b.Columns.Add("数量(根)", typeof(int));
                dt_b.Columns.Add("数量百分比", typeof(double));
                dt_b.Columns.Add("重量(kg)", typeof(double));
                dt_b.Columns.Add("重量百分比", typeof(double));
                foreach (RebarData _ddd in _bangdata)
                {
                    if (_ddd.Diameter == 14)
                    {
                        _num[0] += _ddd.TotalPieceNum;
                        _weight[0] += _ddd.TotalWeight;
                    }
                    else if (_ddd.Diameter == 16)
                    {
                        _num[1] += _ddd.TotalPieceNum;
                        _weight[1] += _ddd.TotalWeight;
                    }
                    else if (_ddd.Diameter == 18)
                    {
                        _num[2] += _ddd.TotalPieceNum;
                        _weight[2] += _ddd.TotalWeight;
                    }
                    else if (_ddd.Diameter == 20)
                    {
                        _num[3] += _ddd.TotalPieceNum;
                        _weight[3] += _ddd.TotalWeight;
                    }
                    else if (_ddd.Diameter == 22)
                    {
                        _num[4] += _ddd.TotalPieceNum;
                        _weight[4] += _ddd.TotalWeight;
                    }
                    else if (_ddd.Diameter == 25)
                    {
                        _num[5] += _ddd.TotalPieceNum;
                        _weight[5] += _ddd.TotalWeight;
                    }
                    else if (_ddd.Diameter == 28)
                    {
                        _num[6] += _ddd.TotalPieceNum;
                        _weight[6] += _ddd.TotalWeight;
                    }
                    else if (_ddd.Diameter == 32)
                    {
                        _num[7] += _ddd.TotalPieceNum;
                        _weight[7] += _ddd.TotalWeight;
                    }
                    else if (_ddd.Diameter == 36)
                    {
                        _num[8] += _ddd.TotalPieceNum;
                        _weight[8] += _ddd.TotalWeight;
                    }
                    else if (_ddd.Diameter == 40)
                    {
                        _num[9] += _ddd.TotalPieceNum;
                        _weight[9] += _ddd.TotalWeight;
                    }

                }
                int total_num_b = 0;
                double total_weight_b = 0;
                for (int i = 0; i < 10; i++)
                {
                    total_num_b += _num[i];
                    total_weight_b += _weight[i];
                }
                for (int i = (int)EnumRebarBang.BANG_C14; i < (int)EnumRebarBang.maxRebarBangNum; i++)
                {
                    dt_b.Rows.Add("Φ" + ((EnumRebarBang)i).ToString().Substring(6, 2), _num[i], (double)_num[i] / (double)total_num_b, _weight[i], _weight[i] / total_weight_b);
                }

                dataGridView13.DataSource = dt_b;
                dataGridView13.Columns[2].DefaultCellStyle.Format = "P0";
                dataGridView13.Columns[3].DefaultCellStyle.Format = "0.00";
                dataGridView13.Columns[4].DefaultCellStyle.Format = "P0";

                //FillDGVWithRebarList(_xiandata, dataGridView9);

                //统计总的
                DataTable dt_z = new DataTable();
                _num = new int[2];      //按照
                _weight = new double[2];
                dt_z.Columns.Add("钢筋类型", typeof(string));
                dt_z.Columns.Add("数量(根)", typeof(int));
                dt_z.Columns.Add("数量百分比", typeof(double));
                dt_z.Columns.Add("重量(kg)", typeof(double));
                dt_z.Columns.Add("重量百分比", typeof(double));

                //dt_x.Rows.Add("Φ6", _num[0], (double)_num[0] / (double)total_num_x, _weight[0], (_weight[0] / total_weight_x));
                dt_z.Rows.Add("线材", total_num_x, (double)total_num_x / (double)(total_num_x + total_num_b), total_weight_x, total_weight_x / (total_weight_x + total_weight_b));
                dt_z.Rows.Add("棒材", total_num_b, (double)total_num_b / (double)(total_num_x + total_num_b), total_weight_b, total_weight_b / (total_weight_x + total_weight_b));

                dataGridView11.DataSource = dt_z;
                dataGridView11.Columns[2].DefaultCellStyle.Format = "P0";
                dataGridView11.Columns[3].DefaultCellStyle.Format = "0.00";
                dataGridView11.Columns[4].DefaultCellStyle.Format = "P0";



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

            textBox3.Text = sss;
        }
    }
}
