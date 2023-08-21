using Etable;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace RebarSampling
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

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

            InitRadioButtion();
            InitCheckBox();

            InitStatisticsDGV();
            InitLabel();
            //tabControl1.Enabled = false;
            GeneralClass.interactivityData.initStatisticsDGV += InitStatisticsDGV;

            GeneralClass.interactivityData.showAssembly += GetSheetToDGV;

            GeneralClass.interactivityData.ifFindImage += FindImageInImagelist;
        }
        private void InitStatisticsDGV()
        {
            InitDGV_BX();
            InitDGV_Xian();
            InitDGV_BangOri();
            InitDGV_Bang();
        }

        private void InitLabel()
        {
            //label1.Text = "*说明：此表百分比数据的统计范围为所有需【锯切】棒材钢筋";
            //label2.Text = "*说明：此表百分比数据的统计范围为所有需【锯切】需【弯曲】棒材钢筋";
        }
        /// <summary>
        /// 其他界面也可以使用此方法
        /// </summary>
        /// <param name="_dgv"></param>
        public static void InitDGV(DataGridView _dgv)
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
                    //|| i == (int)EnumAllRebarTableColName.PIECE_NUM_UNIT_NUM
                    || i == (int)EnumAllRebarTableColName.SERIALNUM

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

                //for (int j = 0; j < (int)EnumDetailTableColName.ONLY_CUT; j++)
                //{
                //    row = new DataGridViewRow();
                //    row.HeaderCell.Value = GeneralClass.sDetailTableColName[j];
                //    dataGridView6.Rows.Add(row);
                //}



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
        private void InitDGV_Bang()
        {
            DataTable dt = new DataTable();
            dt.Clear();

            dataGridView19.DataSource = dt;
            dataGridView20.DataSource = dt;
            dataGridView21.DataSource = dt;
            dataGridView22.DataSource = dt;
            dataGridView8.DataSource = dt;

            dataGridView25.DataSource = dt;
            dataGridView2.DataSource = dt;
            dataGridView3.DataSource = dt;

            this.panel2.Size = new System.Drawing.Size(395, 250);
            this.panel3.Size = new System.Drawing.Size(395, 250);
            this.panel4.Size = new System.Drawing.Size(395, 250);
            this.panel5.Size = new System.Drawing.Size(395, 250);
            //this.panel6.Size = new System.Drawing.Size(395, 250);

            this.panel2.Visible = false;
            this.panel3.Visible = false;
            this.panel4.Visible = false;
            this.panel5.Visible = false;
            //this.panel6.Visible = false;

            //分子，套，弯
            this.checkBox2.Checked = true;
            this.checkBox3.Checked = true;
            this.checkBox16.Checked = true;
            this.checkBox17.Checked = true;

            //分子，直径
            this.checkBox8.Checked = true;
            this.checkBox9.Checked = true;
            this.checkBox10.Checked = true;
            this.checkBox11.Checked = true;
            this.checkBox12.Checked = true;
            this.checkBox13.Checked = true;
            this.checkBox14.Checked = true;
            this.checkBox15.Checked = true;
            this.checkBox18.Checked = true;
            this.checkBox39.Checked = true;

            //分子，材料
            this.checkBox19.Checked = true;
            this.checkBox20.Checked = true;
            this.checkBox37.Checked = true;

            //分母，材料
            this.checkBox21.Checked = true;
            this.checkBox22.Checked = true;
            this.checkBox38.Checked = true;

            //分母，套，弯
            this.checkBox23.Checked = true;
            this.checkBox24.Checked = true;
            this.checkBox25.Checked = true;
            this.checkBox26.Checked = true;

            //分母，直径
            this.checkBox27.Checked = true;
            this.checkBox28.Checked = true;
            this.checkBox29.Checked = true;
            this.checkBox30.Checked = true;
            this.checkBox31.Checked = true;
            this.checkBox32.Checked = true;
            this.checkBox33.Checked = true;
            this.checkBox34.Checked = true;
            this.checkBox35.Checked = true;
            this.checkBox40.Checked = true;

        }
        private void InitDataGridView8()
        {
            //InitDGV(dataGridView8);
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

            checkBox36.Checked = true;
            checkBox36.Text = "Φ14-棒材";

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
        private bool FindImageInImagelist(string picName, out object _image)
        {
            for (int j = 0; j < imageList1.Images.Count; j++)
            {
                if (imageList1.Images.Keys[j].Equals(picName + ".png"))
                {
                    //index = j;
                    _image = imageList1.Images[j];
                    return true;
                }
            }
            _image = null;
            return false;
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
        private void FillDGV6(EnumDetailItem _item)
        {
            try
            {
                //bool _havedata_d = false;//原材的部分有数据
                bool _havedata_o = false;//非原材的部分有数据
                ////原材数据处理
                //if (GeneralClass.AllDetailData != null)
                //{

                //    for (int i = 0; i < (int)EnumDetailTableRowName.maxRowNum; i++)
                //    {
                //        for (int j = 0; j < (int)EnumDetailTableColName.ONLY_CUT; j++)
                //        {
                //            if (GeneralClass.AllDetailData[j, i, (int)_item] != null)
                //            {
                //                _havedata_d = true;
                //                if (_item == EnumDetailItem.TOTAL_WEIGHT)
                //                {
                //                    double _weight = (double)GeneralClass.AllDetailData[j, i, (int)_item];
                //                    dataGridView6.Rows[j].Cells[i + 1].Value = Math.Round(_weight, 1);

                //                }
                //                else if (_item == EnumDetailItem.TOTAL_LENGTH)
                //                {
                //                    int _length = (int)GeneralClass.AllDetailData[j, i, (int)_item];
                //                    dataGridView6.Rows[j].Cells[i + 1].Value = _length / 1000;
                //                }
                //                else
                //                {
                //                    dataGridView6.Rows[j].Cells[i + 1].Value = GeneralClass.AllDetailData[j, i, (int)_item];
                //                }

                //            }
                //        }
                //    }
                //}

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
                                                    dataGridView6.Rows[h /*+ 3*/].Cells[k + 1].Value = Math.Round(_weight, 1);
                                                    break;
                                                }
                                            case EnumDetailItem.TOTAL_LENGTH:
                                                {
                                                    int _length = item.Value.TotalLength;
                                                    dataGridView6.Rows[h /*+ 3*/].Cells[k + 1].Value = _length / 1000;
                                                    break;
                                                }
                                            case EnumDetailItem.TOTAL_PIECE:
                                                {
                                                    dataGridView6.Rows[h /*+ 3*/].Cells[k + 1].Value = item.Value.TotalPieceNum;
                                                    break;
                                                }
                                            case EnumDetailItem.TAO_SI_NUM:
                                                {
                                                    dataGridView6.Rows[h /*+ 3*/].Cells[k + 1].Value = item.Value.TaosiNum;
                                                    break;
                                                }
                                            case EnumDetailItem.TAO_TONG_NUM:
                                                {
                                                    dataGridView6.Rows[h /*+ 3*/].Cells[k + 1].Value = item.Value.TaotongNum;
                                                    break;
                                                }
                                            case EnumDetailItem.ZHENG_SI_TAO_TONG:
                                                {
                                                    dataGridView6.Rows[h /*+ 3*/].Cells[k + 1].Value = item.Value.TaotongNum_P;
                                                    break;
                                                }
                                            case EnumDetailItem.FAN_SI_TAO_TONG:
                                                {
                                                    dataGridView6.Rows[h /*+ 3*/].Cells[k + 1].Value = item.Value.TaotongNum_N;
                                                    break;
                                                }
                                            case EnumDetailItem.BIAN_JING_TAO_TONG:
                                                {
                                                    dataGridView6.Rows[h /*+ 3*/].Cells[k + 1].Value = item.Value.TaotongNum_V;
                                                    break;
                                                }
                                            case EnumDetailItem.CUT_NUM:
                                                {
                                                    dataGridView6.Rows[h /*+ 3*/].Cells[k + 1].Value = item.Value.CutNum;
                                                    break;
                                                }
                                            case EnumDetailItem.BEND_NUM:
                                                {
                                                    dataGridView6.Rows[h /*+ 3*/].Cells[k + 1].Value = item.Value.BendNum;
                                                    break;
                                                }
                                            case EnumDetailItem.ZHI_NUM:
                                                {
                                                    dataGridView6.Rows[h /*+ 3*/].Cells[k + 1].Value = item.Value.StraightenedNum;
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
                if (/*_havedata_d &&*/ _havedata_o)
                {
                    int _total;
                    int ydataTotal = 0;

                    List<string> xData = new List<string>();
                    List<int> yData = new List<int>();
                    for (int u = 0; u < (int)EnumDetailTableRowName.maxRowNum; u++)
                    {
                        _total = 0;
                        for (int y = 0; y < GeneralClass.ExistRebarPicTypeList.Count /*+ 3*/; y++)
                        {
                            _total += Convert.ToInt32(dataGridView6.Rows[y].Cells[u + 1].Value);
                        }
                        xData.Add(GeneralClass.sDetailTableRowName[u]);
                        yData.Add(_total);
                        ydataTotal += _total;
                        dataGridView6.Rows[GeneralClass.ExistRebarPicTypeList.Count /*+ 3*/].Cells[u + 1].Value = _total;
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

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                GeneralClass.interactivityData?.printlog(1, "开始按钢筋规格直径分类统计");

                GeneralClass.AllRebarList = GeneralClass.SQLiteOpt.GetAllRebarList(GeneralClass.AllRebarTableName);//取得所有的钢筋数据list

                List<GroupbyDiameterDatalist> _grouplist = GeneralClass.SQLiteOpt.QueryAllListByDiameter(GeneralClass.AllRebarList);

                int[] _num = new int[(int)EnumDiameter.maxDiameterNum];
                double[] _weight = new double[(int)EnumDiameter.maxDiameterNum];

                int total_num_x = 0;
                double total_weight_x = 0;

                foreach (var item in _grouplist)
                {
                    if (item._diameter == 6)
                    {
                        _num[(int)EnumDiameter.DIAMETER_6] = item._totalnum;
                        _weight[(int)EnumDiameter.DIAMETER_6] = item._totalweight;
                    }
                    else if (item._diameter == 8)
                    {
                        _num[(int)EnumDiameter.DIAMETER_8] = item._totalnum;
                        _weight[(int)EnumDiameter.DIAMETER_8] = item._totalweight;
                    }
                    else if (item._diameter == 10)
                    {
                        _num[(int)EnumDiameter.DIAMETER_10] = item._totalnum;
                        _weight[(int)EnumDiameter.DIAMETER_10] = item._totalweight;
                    }
                    else if (item._diameter == 12)
                    {
                        _num[(int)EnumDiameter.DIAMETER_12] = item._totalnum;
                        _weight[(int)EnumDiameter.DIAMETER_12] = item._totalweight;
                    }
                    else if (item._diameter == 14)
                    {
                        _num[(int)EnumDiameter.DIAMETER_14] = item._totalnum;
                        _weight[(int)EnumDiameter.DIAMETER_14] = item._totalweight;
                    }
                    else if (item._diameter == 16)
                    {
                        _num[(int)EnumDiameter.DIAMETER_16] = item._totalnum;
                        _weight[(int)EnumDiameter.DIAMETER_16] = item._totalweight;
                    }
                    else if (item._diameter == 18)
                    {
                        _num[(int)EnumDiameter.DIAMETER_18] = item._totalnum;
                        _weight[(int)EnumDiameter.DIAMETER_18] = item._totalweight;
                    }
                    else if (item._diameter == 20)
                    {
                        _num[(int)EnumDiameter.DIAMETER_20] = item._totalnum;
                        _weight[(int)EnumDiameter.DIAMETER_20] = item._totalweight;
                    }
                    else if (item._diameter == 22)
                    {
                        _num[(int)EnumDiameter.DIAMETER_22] = item._totalnum;
                        _weight[(int)EnumDiameter.DIAMETER_22] = item._totalweight;
                    }
                    else if (item._diameter == 25)
                    {
                        _num[(int)EnumDiameter.DIAMETER_25] = item._totalnum;
                        _weight[(int)EnumDiameter.DIAMETER_25] = item._totalweight;
                    }
                    else if (item._diameter == 28)
                    {
                        _num[(int)EnumDiameter.DIAMETER_28] = item._totalnum;
                        _weight[(int)EnumDiameter.DIAMETER_28] = item._totalweight;
                    }
                    else if (item._diameter == 32)
                    {
                        _num[(int)EnumDiameter.DIAMETER_32] = item._totalnum;
                        _weight[(int)EnumDiameter.DIAMETER_32] = item._totalweight;
                    }
                    else if (item._diameter == 36)
                    {
                        _num[(int)EnumDiameter.DIAMETER_36] = item._totalnum;
                        _weight[(int)EnumDiameter.DIAMETER_36] = item._totalweight;
                    }
                    else if (item._diameter == 40)
                    {
                        _num[(int)EnumDiameter.DIAMETER_40] = item._totalnum;
                        _weight[(int)EnumDiameter.DIAMETER_40] = item._totalweight;
                    }

                }

                //先统计线材
                DataTable dt_x = new DataTable();
                dt_x.Columns.Add("直径(mm)", typeof(string));
                dt_x.Columns.Add("数量(根)", typeof(int));
                dt_x.Columns.Add("数量(%)", typeof(double));
                dt_x.Columns.Add("重量(kg)", typeof(double));
                dt_x.Columns.Add("重量(%)", typeof(double));

                if (this.diameterChecked_14) //true为Φ14棒材，false为Φ14线材
                {
                    for (int i = (int)EnumDiameter.DIAMETER_6; i < (int)EnumDiameter.DIAMETER_14; i++)
                    {
                        total_num_x += _num[i];
                        total_weight_x += _weight[i];
                    }
                    dt_x.Rows.Add("Φ6", _num[0], (double)_num[0] / (double)total_num_x, _weight[0], (_weight[0] / total_weight_x));
                    dt_x.Rows.Add("Φ8", _num[1], (double)_num[1] / (double)total_num_x, _weight[1], (_weight[1] / total_weight_x));
                    dt_x.Rows.Add("Φ10", _num[2], (double)_num[2] / (double)total_num_x, _weight[2], (_weight[2] / total_weight_x));
                    dt_x.Rows.Add("Φ12", _num[3], (double)_num[3] / (double)total_num_x, _weight[3], (_weight[3] / total_weight_x));
                }
                else
                {
                    for (int i = (int)EnumDiameter.DIAMETER_6; i < (int)EnumDiameter.DIAMETER_16; i++)
                    {
                        total_num_x += _num[i];
                        total_weight_x += _weight[i];
                    }
                    dt_x.Rows.Add("Φ6", _num[0], (double)_num[0] / (double)total_num_x, _weight[0], (_weight[0] / total_weight_x));
                    dt_x.Rows.Add("Φ8", _num[1], (double)_num[1] / (double)total_num_x, _weight[1], (_weight[1] / total_weight_x));
                    dt_x.Rows.Add("Φ10", _num[2], (double)_num[2] / (double)total_num_x, _weight[2], (_weight[2] / total_weight_x));
                    dt_x.Rows.Add("Φ12", _num[3], (double)_num[3] / (double)total_num_x, _weight[3], (_weight[3] / total_weight_x));
                    dt_x.Rows.Add("Φ14", _num[4], (double)_num[4] / (double)total_num_x, _weight[4], (_weight[4] / total_weight_x));
                }
                dataGridView14.DataSource = dt_x;
                dataGridView14.Columns[2].DefaultCellStyle.Format = "P1";
                dataGridView14.Columns[3].DefaultCellStyle.Format = "0.0";
                dataGridView14.Columns[4].DefaultCellStyle.Format = "P1";


                //统计棒材
                DataTable dt_b = new DataTable();//
                dt_b.Columns.Add("直径(mm)", typeof(string));
                dt_b.Columns.Add("数量(根)", typeof(int));
                dt_b.Columns.Add("数量(%)", typeof(double));
                dt_b.Columns.Add("重量(kg)", typeof(double));
                dt_b.Columns.Add("重量(%)", typeof(double));

                int total_num_b = 0;
                double total_weight_b = 0;

                if (this.diameterChecked_14)//true为Φ14棒材，false为Φ14线材
                {
                    for (int i = (int)EnumDiameter.DIAMETER_14; i < (int)EnumDiameter.maxDiameterNum; i++)
                    {
                        total_num_b += _num[i];
                        total_weight_b += _weight[i];
                    }
                    for (int i = (int)EnumDiameter.DIAMETER_14; i < (int)EnumDiameter.maxDiameterNum; i++)
                    {
                        dt_b.Rows.Add("Φ" + ((EnumDiameter)i).ToString().Substring(9, 2), _num[i], (double)_num[i] / (double)total_num_b, _weight[i], _weight[i] / total_weight_b);
                    }

                }
                else
                {
                    for (int i = (int)EnumDiameter.DIAMETER_16; i < (int)EnumDiameter.maxDiameterNum; i++)
                    {
                        total_num_b += _num[i];
                        total_weight_b += _weight[i];
                    }
                    for (int i = (int)EnumDiameter.DIAMETER_16; i < (int)EnumDiameter.maxDiameterNum; i++)
                    {
                        dt_b.Rows.Add("Φ" + ((EnumDiameter)i).ToString().Substring(9, 2), _num[i], (double)_num[i] / (double)total_num_b, _weight[i], _weight[i] / total_weight_b);
                    }
                }


                dataGridView13.DataSource = dt_b;
                dataGridView13.Columns[2].DefaultCellStyle.Format = "P1";
                dataGridView13.Columns[3].DefaultCellStyle.Format = "0.00";
                dataGridView13.Columns[4].DefaultCellStyle.Format = "P1";

                //统计合计
                DataTable dt_z = new DataTable();

                dt_z.Columns.Add("钢筋类型", typeof(string));
                dt_z.Columns.Add("数量(根)", typeof(int));
                dt_z.Columns.Add("数量(%)", typeof(double));
                dt_z.Columns.Add("重量(kg)", typeof(double));
                dt_z.Columns.Add("重量(%)", typeof(double));

                dt_z.Rows.Add("线材", total_num_x, (double)total_num_x / (double)(total_num_x + total_num_b), total_weight_x, total_weight_x / (total_weight_x + total_weight_b));
                dt_z.Rows.Add("棒材", total_num_b, (double)total_num_b / (double)(total_num_x + total_num_b), total_weight_b, total_weight_b / (total_weight_x + total_weight_b));

                dataGridView11.DataSource = dt_z;
                dataGridView11.Columns[2].DefaultCellStyle.Format = "P1";
                dataGridView11.Columns[3].DefaultCellStyle.Format = "0.00";
                dataGridView11.Columns[4].DefaultCellStyle.Format = "P1";



                #region 备份

                //GeneralClass.interactivityData?.printlog(1, "开始按钢筋规格直径分类统计");

                //GeneralClass.AllRebarList = GeneralClass.SQLiteOpt.GetAllRebarList(GeneralClass.AllRebarTableName);//取得所有的钢筋数据list

                //List<RebarData> _bangdata = new List<RebarData>();
                //List<RebarData> _xiandata = new List<RebarData>();
                //foreach (RebarData _dd in GeneralClass.AllRebarList)
                //{
                //    if (_dd.Diameter <= 12)
                //    {
                //        _xiandata.Add(_dd);
                //    }
                //    else
                //    {
                //        _bangdata.Add(_dd);
                //    }
                //}

                //int[] _num = new int[4];                //按照6，8，10，12四种直径来统计
                //double[] _weight = new double[4];
                //int total_num_x;
                //double total_weight_x;
                ////先统计线材
                //DataTable dt_x = new DataTable();
                //dt_x.Columns.Add("直径", typeof(string));
                //dt_x.Columns.Add("数量(根)", typeof(int));
                //dt_x.Columns.Add("数量百分比", typeof(double));
                //dt_x.Columns.Add("重量(kg)", typeof(double));
                //dt_x.Columns.Add("重量百分比", typeof(double));

                //foreach (RebarData _ddd in _xiandata)
                //{
                //    if (_ddd.Diameter == 6)
                //    {
                //        _num[0] += _ddd.TotalPieceNum;
                //        _weight[0] += _ddd.TotalWeight;
                //    }
                //    else if (_ddd.Diameter == 8)
                //    {
                //        _num[1] += _ddd.TotalPieceNum;
                //        _weight[1] += _ddd.TotalWeight;
                //    }
                //    else if (_ddd.Diameter == 10)
                //    {
                //        _num[2] += _ddd.TotalPieceNum;
                //        _weight[2] += _ddd.TotalWeight;
                //    }
                //    else
                //    {
                //        _num[3] += _ddd.TotalPieceNum;
                //        _weight[3] += _ddd.TotalWeight;
                //    }
                //}
                //total_num_x = _num[0] + _num[1] + _num[2] + _num[3];
                //total_weight_x = _weight[0] + _weight[1] + _weight[2] + _weight[3];
                //dt_x.Rows.Add("Φ6", _num[0], (double)_num[0] / (double)total_num_x, _weight[0], (_weight[0] / total_weight_x));
                //dt_x.Rows.Add("Φ8", _num[1], (double)_num[1] / (double)total_num_x, _weight[1], (_weight[1] / total_weight_x));
                //dt_x.Rows.Add("Φ10", _num[2], (double)_num[2] / (double)total_num_x, _weight[2], (_weight[2] / total_weight_x));
                //dt_x.Rows.Add("Φ12", _num[3], (double)_num[3] / (double)total_num_x, _weight[3], (_weight[3] / total_weight_x));
                //dataGridView14.DataSource = dt_x;
                //dataGridView14.Columns[2].DefaultCellStyle.Format = "P1";
                //dataGridView14.Columns[3].DefaultCellStyle.Format = "0.0";
                //dataGridView14.Columns[4].DefaultCellStyle.Format = "P1";


                ////统计棒材
                //DataTable dt_b = new DataTable();//
                //_num = new int[(int)EnumRebarBang.maxRebarBangNum];      //按照14、16、18、20、22、25、28、32、36、40总计十种直径
                //_weight = new double[(int)EnumRebarBang.maxRebarBangNum];

                //dt_b.Columns.Add("直径", typeof(string));
                //dt_b.Columns.Add("数量(根)", typeof(int));
                //dt_b.Columns.Add("数量百分比", typeof(double));
                //dt_b.Columns.Add("重量(kg)", typeof(double));
                //dt_b.Columns.Add("重量百分比", typeof(double));
                //foreach (RebarData _ddd in _bangdata)
                //{
                //    if (_ddd.Diameter == 14)
                //    {
                //        _num[0] += _ddd.TotalPieceNum;
                //        _weight[0] += _ddd.TotalWeight;
                //    }
                //    else if (_ddd.Diameter == 16)
                //    {
                //        _num[1] += _ddd.TotalPieceNum;
                //        _weight[1] += _ddd.TotalWeight;
                //    }
                //    else if (_ddd.Diameter == 18)
                //    {
                //        _num[2] += _ddd.TotalPieceNum;
                //        _weight[2] += _ddd.TotalWeight;
                //    }
                //    else if (_ddd.Diameter == 20)
                //    {
                //        _num[3] += _ddd.TotalPieceNum;
                //        _weight[3] += _ddd.TotalWeight;
                //    }
                //    else if (_ddd.Diameter == 22)
                //    {
                //        _num[4] += _ddd.TotalPieceNum;
                //        _weight[4] += _ddd.TotalWeight;
                //    }
                //    else if (_ddd.Diameter == 25)
                //    {
                //        _num[5] += _ddd.TotalPieceNum;
                //        _weight[5] += _ddd.TotalWeight;
                //    }
                //    else if (_ddd.Diameter == 28)
                //    {
                //        _num[6] += _ddd.TotalPieceNum;
                //        _weight[6] += _ddd.TotalWeight;
                //    }
                //    else if (_ddd.Diameter == 32)
                //    {
                //        _num[7] += _ddd.TotalPieceNum;
                //        _weight[7] += _ddd.TotalWeight;
                //    }
                //    else if (_ddd.Diameter == 36)
                //    {
                //        _num[8] += _ddd.TotalPieceNum;
                //        _weight[8] += _ddd.TotalWeight;
                //    }
                //    else if (_ddd.Diameter == 40)
                //    {
                //        _num[9] += _ddd.TotalPieceNum;
                //        _weight[9] += _ddd.TotalWeight;
                //    }

                //}
                //int total_num_b = 0;
                //double total_weight_b = 0;
                //for (int i = 0; i < 10; i++)
                //{
                //    total_num_b += _num[i];
                //    total_weight_b += _weight[i];
                //}
                //for (int i = (int)EnumRebarBang.BANG_C14; i < (int)EnumRebarBang.maxRebarBangNum; i++)
                //{
                //    dt_b.Rows.Add("Φ" + ((EnumRebarBang)i).ToString().Substring(6, 2), _num[i], (double)_num[i] / (double)total_num_b, _weight[i], _weight[i] / total_weight_b);
                //}

                //dataGridView13.DataSource = dt_b;
                //dataGridView13.Columns[2].DefaultCellStyle.Format = "P1";
                //dataGridView13.Columns[3].DefaultCellStyle.Format = "0.00";
                //dataGridView13.Columns[4].DefaultCellStyle.Format = "P1";

                ////FillDGVWithRebarList(_xiandata, dataGridView9);

                ////统计总的
                //DataTable dt_z = new DataTable();
                //_num = new int[2];      //按照
                //_weight = new double[2];
                //dt_z.Columns.Add("钢筋类型", typeof(string));
                //dt_z.Columns.Add("数量(根)", typeof(int));
                //dt_z.Columns.Add("数量百分比", typeof(double));
                //dt_z.Columns.Add("重量(kg)", typeof(double));
                //dt_z.Columns.Add("重量百分比", typeof(double));

                ////dt_x.Rows.Add("Φ6", _num[0], (double)_num[0] / (double)total_num_x, _weight[0], (_weight[0] / total_weight_x));
                //dt_z.Rows.Add("线材", total_num_x, (double)total_num_x / (double)(total_num_x + total_num_b), total_weight_x, total_weight_x / (total_weight_x + total_weight_b));
                //dt_z.Rows.Add("棒材", total_num_b, (double)total_num_b / (double)(total_num_x + total_num_b), total_weight_b, total_weight_b / (total_weight_x + total_weight_b));

                //dataGridView11.DataSource = dt_z;
                //dataGridView11.Columns[2].DefaultCellStyle.Format = "P1";
                //dataGridView11.Columns[3].DefaultCellStyle.Format = "0.00";
                //dataGridView11.Columns[4].DefaultCellStyle.Format = "P1";

                #endregion

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        /// <summary>
        /// 根据线材的直条、弯曲区分统计，
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="_type"></param>
        /// <param name="_dgv"></param>
        private void FillDGV_Xian_type(List<RebarData> _list, ref DataGridView _dgv, out int _totalnum, out double _totalweight)
        {
            //
            List<GroupbyDiameterDatalist> _grouplist = GeneralClass.SQLiteOpt.QueryAllListByDiameter(_list);

            int[] _num = new int[(int)EnumDiameter.maxDiameterNum];
            double[] _weight = new double[(int)EnumDiameter.maxDiameterNum];

            _totalnum = 0;
            _totalweight = 0;

            foreach (var item in _grouplist)
            {
                if (item._diameter == 6)
                {
                    _num[(int)EnumDiameter.DIAMETER_6] = item._totalnum;
                    _weight[(int)EnumDiameter.DIAMETER_6] = item._totalweight;
                }
                else if (item._diameter == 8)
                {
                    _num[(int)EnumDiameter.DIAMETER_8] = item._totalnum;
                    _weight[(int)EnumDiameter.DIAMETER_8] = item._totalweight;
                }
                else if (item._diameter == 10)
                {
                    _num[(int)EnumDiameter.DIAMETER_10] = item._totalnum;
                    _weight[(int)EnumDiameter.DIAMETER_10] = item._totalweight;
                }
                else if (item._diameter == 12)
                {
                    _num[(int)EnumDiameter.DIAMETER_12] = item._totalnum;
                    _weight[(int)EnumDiameter.DIAMETER_12] = item._totalweight;
                }
                else if (item._diameter == 14)
                {
                    _num[(int)EnumDiameter.DIAMETER_14] = item._totalnum;
                    _weight[(int)EnumDiameter.DIAMETER_14] = item._totalweight;
                }
                else if (item._diameter == 16)
                {
                    _num[(int)EnumDiameter.DIAMETER_16] = item._totalnum;
                    _weight[(int)EnumDiameter.DIAMETER_16] = item._totalweight;
                }
                else if (item._diameter == 18)
                {
                    _num[(int)EnumDiameter.DIAMETER_18] = item._totalnum;
                    _weight[(int)EnumDiameter.DIAMETER_18] = item._totalweight;
                }
                else if (item._diameter == 20)
                {
                    _num[(int)EnumDiameter.DIAMETER_20] = item._totalnum;
                    _weight[(int)EnumDiameter.DIAMETER_20] = item._totalweight;
                }
                else if (item._diameter == 22)
                {
                    _num[(int)EnumDiameter.DIAMETER_22] = item._totalnum;
                    _weight[(int)EnumDiameter.DIAMETER_22] = item._totalweight;
                }
                else if (item._diameter == 25)
                {
                    _num[(int)EnumDiameter.DIAMETER_25] = item._totalnum;
                    _weight[(int)EnumDiameter.DIAMETER_25] = item._totalweight;
                }
                else if (item._diameter == 28)
                {
                    _num[(int)EnumDiameter.DIAMETER_28] = item._totalnum;
                    _weight[(int)EnumDiameter.DIAMETER_28] = item._totalweight;
                }
                else if (item._diameter == 32)
                {
                    _num[(int)EnumDiameter.DIAMETER_32] = item._totalnum;
                    _weight[(int)EnumDiameter.DIAMETER_32] = item._totalweight;
                }
                else if (item._diameter == 36)
                {
                    _num[(int)EnumDiameter.DIAMETER_36] = item._totalnum;
                    _weight[(int)EnumDiameter.DIAMETER_36] = item._totalweight;
                }
                else if (item._diameter == 40)
                {
                    _num[(int)EnumDiameter.DIAMETER_40] = item._totalnum;
                    _weight[(int)EnumDiameter.DIAMETER_40] = item._totalweight;
                }

            }

            DataTable dt = new DataTable();
            dt.Columns.Add("直径", typeof(string));
            dt.Columns.Add("数量(根)", typeof(int));
            dt.Columns.Add("数量(%)", typeof(double));
            dt.Columns.Add("重量(kg)", typeof(double));
            dt.Columns.Add("重量(%)", typeof(double));

            if (this.diameterChecked_14)
            {
                _totalnum = _num[0] + _num[1] + _num[2] + _num[3];
                _totalweight = _weight[0] + _weight[1] + _weight[2] + _weight[3];
                dt.Rows.Add("Φ6", _num[0], (double)_num[0] / (double)_totalnum, _weight[0], (_weight[0] / _totalweight));
                dt.Rows.Add("Φ8", _num[1], (double)_num[1] / (double)_totalnum, _weight[1], (_weight[1] / _totalweight));
                dt.Rows.Add("Φ10", _num[2], (double)_num[2] / (double)_totalnum, _weight[2], (_weight[2] / _totalweight));
                dt.Rows.Add("Φ12", _num[3], (double)_num[3] / (double)_totalnum, _weight[3], (_weight[3] / _totalweight));
            }
            else
            {
                _totalnum = _num[0] + _num[1] + _num[2] + _num[3] + _num[4];
                _totalweight = _weight[0] + _weight[1] + _weight[2] + _weight[3] + _weight[4];
                dt.Rows.Add("Φ6", _num[0], (double)_num[0] / (double)_totalnum, _weight[0], (_weight[0] / _totalweight));
                dt.Rows.Add("Φ8", _num[1], (double)_num[1] / (double)_totalnum, _weight[1], (_weight[1] / _totalweight));
                dt.Rows.Add("Φ10", _num[2], (double)_num[2] / (double)_totalnum, _weight[2], (_weight[2] / _totalweight));
                dt.Rows.Add("Φ12", _num[3], (double)_num[3] / (double)_totalnum, _weight[3], (_weight[3] / _totalweight));
                dt.Rows.Add("Φ14", _num[4], (double)_num[4] / (double)_totalnum, _weight[4], (_weight[4] / _totalweight));
            }

            _dgv.DataSource = dt;
            _dgv.Columns[2].DefaultCellStyle.Format = "P1";
            _dgv.Columns[3].DefaultCellStyle.Format = "0.0";
            _dgv.Columns[4].DefaultCellStyle.Format = "P1";
        }
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                GeneralClass.interactivityData?.printlog(1, "开始统计所有线材");

                GeneralClass.AllRebarList = GeneralClass.SQLiteOpt.GetAllRebarList(GeneralClass.AllRebarTableName);//取得所有的钢筋数据list

                List<RebarData> _xiandata_z = new List<RebarData>();//直条
                List<RebarData> _xiandata_w = new List<RebarData>();//弯曲

                foreach (RebarData _dd in GeneralClass.AllRebarList)
                {
                    if (this.diameterChecked_14)//
                    {
                        if (_dd.Diameter <= 12)
                        {
                            if (_dd.TypeNum == "10000")
                                _xiandata_z.Add(_dd);
                            else
                                _xiandata_w.Add(_dd);
                        }
                    }
                    else
                    {
                        if (_dd.Diameter <= 14)
                        {
                            if (_dd.TypeNum == "10000")
                                _xiandata_z.Add(_dd);
                            else
                                _xiandata_w.Add(_dd);
                        }
                    }
                }
                int total_num_z, total_num_w = 0;
                double total_weight_z, total_weight_w = 0;
                FillDGV_Xian_type(_xiandata_z, ref dataGridView9, out total_num_z, out total_weight_z);
                FillDGV_Xian_type(_xiandata_w, ref dataGridView12, out total_num_w, out total_weight_w);

                //汇总统计
                DataTable dt_hz = new DataTable();
                dt_hz.Columns.Add("类型", typeof(string));
                dt_hz.Columns.Add("数量(根)", typeof(int));
                dt_hz.Columns.Add("数量(%)", typeof(double));
                dt_hz.Columns.Add("重量(kg)", typeof(double));
                dt_hz.Columns.Add("重量(%)", typeof(double));

                //dt_x.Rows.Add("Φ6", _num[0], (double)_num[0] / (double)total_num_x, _weight[0], (_weight[0] / total_weight_x));
                dt_hz.Rows.Add("直条", total_num_z, (double)total_num_z / (double)(total_num_z + total_num_w), total_weight_z, total_weight_z / (total_weight_z + total_weight_w));
                dt_hz.Rows.Add("弯曲", total_num_w, (double)total_num_w / (double)(total_num_z + total_num_w), total_weight_w, total_weight_w / (total_weight_z + total_weight_w));

                dataGridView23.DataSource = dt_hz;
                dataGridView23.Columns[2].DefaultCellStyle.Format = "P1";
                dataGridView23.Columns[3].DefaultCellStyle.Format = "0.00";
                dataGridView23.Columns[4].DefaultCellStyle.Format = "P1";

                #region 备份

                //GeneralClass.interactivityData?.printlog(1, "开始统计所有线材");

                //GeneralClass.AllRebarList = GeneralClass.SQLiteOpt.GetAllRebarList(GeneralClass.AllRebarTableName);//取得所有的钢筋数据list

                //List<RebarData> _xiandata = new List<RebarData>();
                //foreach (RebarData _dd in GeneralClass.AllRebarList)
                //{
                //    if (_dd.Diameter <= 12)
                //    {
                //        _xiandata.Add(_dd);
                //    }
                //}

                //int[] _num = new int[4];                //按照6，8，10，12四种直径来统计
                //double[] _weight = new double[4];
                ////int total_num;
                ////double total_weight;
                ////先统计直条的线材
                //DataTable dt_z = new DataTable();
                //dt_z.Columns.Add("直径", typeof(string));
                //dt_z.Columns.Add("数量", typeof(int));
                //dt_z.Columns.Add("数量百分比", typeof(double));
                //dt_z.Columns.Add("重量(kg)", typeof(double));
                //dt_z.Columns.Add("重量百分比", typeof(double));

                //foreach (RebarData _ddd in _xiandata)
                //{
                //    if (_ddd.TypeNum == "10000")
                //    {
                //        if (_ddd.Diameter == 6)
                //        {
                //            _num[0] += _ddd.TotalPieceNum;
                //            _weight[0] += _ddd.TotalWeight;
                //        }
                //        else if (_ddd.Diameter == 8)
                //        {
                //            _num[1] += _ddd.TotalPieceNum;
                //            _weight[1] += _ddd.TotalWeight;
                //        }
                //        else if (_ddd.Diameter == 10)
                //        {
                //            _num[2] += _ddd.TotalPieceNum;
                //            _weight[2] += _ddd.TotalWeight;
                //        }
                //        else
                //        {
                //            _num[3] += _ddd.TotalPieceNum;
                //            _weight[3] += _ddd.TotalWeight;
                //        }
                //    }
                //}
                //int total_num_z = _num[0] + _num[1] + _num[2] + _num[3];
                //double total_weight_z = _weight[0] + _weight[1] + _weight[2] + _weight[3];
                //dt_z.Rows.Add("Φ6", _num[0], (double)_num[0] / (double)total_num_z, _weight[0], (_weight[0] / total_weight_z));
                //dt_z.Rows.Add("Φ8", _num[1], (double)_num[1] / (double)total_num_z, _weight[1], (_weight[1] / total_weight_z));
                //dt_z.Rows.Add("Φ10", _num[2], (double)_num[2] / (double)total_num_z, _weight[2], (_weight[2] / total_weight_z));
                //dt_z.Rows.Add("Φ12", _num[3], (double)_num[3] / (double)total_num_z, _weight[3], (_weight[3] / total_weight_z));

                //dataGridView9.DataSource = dt_z;
                //dataGridView9.Columns[2].DefaultCellStyle.Format = "P1";
                //dataGridView9.Columns[3].DefaultCellStyle.Format = "0.0";
                //dataGridView9.Columns[4].DefaultCellStyle.Format = "P1";


                ////统计弯曲的线材
                //DataTable dt_w = new DataTable();//弯曲线材
                //_num = new int[4];      //清空
                //_weight = new double[4];

                //dt_w.Columns.Add("直径", typeof(string));
                //dt_w.Columns.Add("数量", typeof(int));
                //dt_w.Columns.Add("数量百分比", typeof(double));
                //dt_w.Columns.Add("重量(kg)", typeof(double));
                //dt_w.Columns.Add("重量百分比", typeof(double));

                //foreach (RebarData _ddd in _xiandata)
                //{
                //    if (_ddd.TypeNum != "10000")
                //    {
                //        if (_ddd.Diameter == 6)
                //        {
                //            _num[0] += _ddd.TotalPieceNum;
                //            _weight[0] += _ddd.TotalWeight;
                //        }
                //        else if (_ddd.Diameter == 8)
                //        {
                //            _num[1] += _ddd.TotalPieceNum;
                //            _weight[1] += _ddd.TotalWeight;
                //        }
                //        else if (_ddd.Diameter == 10)
                //        {
                //            _num[2] += _ddd.TotalPieceNum;
                //            _weight[2] += _ddd.TotalWeight;
                //        }
                //        else
                //        {
                //            _num[3] += _ddd.TotalPieceNum;
                //            _weight[3] += _ddd.TotalWeight;
                //        }
                //    }
                //}
                //int total_num_w = _num[0] + _num[1] + _num[2] + _num[3];
                //double total_weight_w = _weight[0] + _weight[1] + _weight[2] + _weight[3];
                //dt_w.Rows.Add("Φ6", _num[0], (double)_num[0] / (double)total_num_w, _weight[0], (_weight[0] / total_weight_w));
                //dt_w.Rows.Add("Φ8", _num[1], (double)_num[1] / (double)total_num_w, _weight[1], (_weight[1] / total_weight_w));
                //dt_w.Rows.Add("Φ10", _num[2], (double)_num[2] / (double)total_num_w, _weight[2], (_weight[2] / total_weight_w));
                //dt_w.Rows.Add("Φ12", _num[3], (double)_num[3] / (double)total_num_w, _weight[3], (_weight[3] / total_weight_w));
                ////dt_w.Columns[2].ExtendedProperties["Format"] = "P1";//指定列属性为百分比，保留一位小数
                ////dt_w.Columns[4].ExtendedProperties["Format"] = "P1";//指定列属性为百分比，保留一位小数

                //dataGridView12.DataSource = dt_w;
                //dataGridView12.Columns[2].DefaultCellStyle.Format = "P1";
                //dataGridView12.Columns[3].DefaultCellStyle.Format = "0.00";
                //dataGridView12.Columns[4].DefaultCellStyle.Format = "P1";

                ////FillDGVWithRebarList(_xiandata, dataGridView9);

                ////汇总统计
                //DataTable dt_hz = new DataTable();
                //_num = new int[2];      //按照
                //_weight = new double[2];
                //dt_hz.Columns.Add("类型", typeof(string));
                //dt_hz.Columns.Add("数量(根)", typeof(int));
                //dt_hz.Columns.Add("数量百分比", typeof(double));
                //dt_hz.Columns.Add("重量(kg)", typeof(double));
                //dt_hz.Columns.Add("重量百分比", typeof(double));

                ////dt_x.Rows.Add("Φ6", _num[0], (double)_num[0] / (double)total_num_x, _weight[0], (_weight[0] / total_weight_x));
                //dt_hz.Rows.Add("直条", total_num_z, (double)total_num_z / (double)(total_num_z + total_num_w), total_weight_z, total_weight_z / (total_weight_z + total_weight_w));
                //dt_hz.Rows.Add("弯曲", total_num_w, (double)total_num_w / (double)(total_num_z + total_num_w), total_weight_w, total_weight_w / (total_weight_z + total_weight_w));

                //dataGridView23.DataSource = dt_hz;
                //dataGridView23.Columns[2].DefaultCellStyle.Format = "P1";
                //dataGridView23.Columns[3].DefaultCellStyle.Format = "0.00";
                //dataGridView23.Columns[4].DefaultCellStyle.Format = "P1";

                #endregion

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
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
                dataGridView24.Columns[2].DefaultCellStyle.Format = "P1";
                dataGridView24.Columns[3].DefaultCellStyle.Format = "0.00";
                dataGridView24.Columns[4].DefaultCellStyle.Format = "P1";

                GeneralClass.interactivityData?.printlog(1, "所有棒材原材数据统计完成");
                //FillDGVWithRebarList(_bangdata, dataGridView8);
                //GeneralClass.interactivityData?.printlog(1, "dgv填充完成");



            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
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
            _dgv.Columns[2].DefaultCellStyle.Format = "P1";         //百分比
            _dgv.Columns[3].DefaultCellStyle.Format = "0.0";        //double保留两位小数
            _dgv.Columns[4].DefaultCellStyle.Format = "P1";         //百分比

        }

        private void FillDGV_Bang_type(List<RebarData> _sonlist)
        {
            DataTable dt_z = new DataTable();
            dt_z.Columns.Add("直径", typeof(string));
            dt_z.Columns.Add("数量(根)", typeof(int));
            dt_z.Columns.Add("数量(%)", typeof(double));
            dt_z.Columns.Add("重量(kg)", typeof(double));
            dt_z.Columns.Add("重量(%)", typeof(double));

            List<GroupbyTaoBendDatalist> _typelist = GeneralClass.SQLiteOpt.QueryAllListByTaoBend(_sonlist);

            List<GroupbyDiameterDatalist> _diameterlist = null;
            int totalnum = 0;
            double totalweight = 0;

            foreach (var item in _typelist)
            {
                if (!item._ifbend && !item._iftao)//不弯不套
                {
                    _diameterlist = GeneralClass.SQLiteOpt.QueryAllListByDiameter(item._datalist);

                    dt_z.Rows.Clear();
                    totalnum = 0;
                    totalweight = 0;
                    foreach (var it in _diameterlist)
                    {
                        totalnum += it._totalnum;
                        totalweight += it._totalweight;
                    }
                    foreach (var it in _diameterlist)
                    {
                        dt_z.Rows.Add("Φ" + it._diameter.ToString(), it._totalnum, (double)it._totalnum / (double)totalnum, it._totalweight, it._totalweight / totalweight);
                    }

                    dataGridView19.DataSource = dt_z;
                    dataGridView19.Columns[2].DefaultCellStyle.Format = "P1";         //百分比
                    dataGridView19.Columns[3].DefaultCellStyle.Format = "0.0";        //double保留两位小数
                    dataGridView19.Columns[4].DefaultCellStyle.Format = "P1";         //百分比
                }
                else if (!item._ifbend && item._iftao)//不弯套
                {
                    _diameterlist = GeneralClass.SQLiteOpt.QueryAllListByDiameter(item._datalist);

                    dt_z.Rows.Clear();
                    totalnum = 0;
                    totalweight = 0;
                    foreach (var it in _diameterlist)
                    {
                        totalnum += it._totalnum;
                        totalweight += it._totalweight;
                    }
                    foreach (var it in _diameterlist)
                    {
                        dt_z.Rows.Add("Φ" + it._diameter.ToString(), it._totalnum, (double)it._totalnum / (double)totalnum, it._totalweight, it._totalweight / totalweight);
                    }

                    dataGridView20.DataSource = dt_z;
                    dataGridView20.Columns[2].DefaultCellStyle.Format = "P1";         //百分比
                    dataGridView20.Columns[3].DefaultCellStyle.Format = "0.0";        //double保留两位小数
                    dataGridView20.Columns[4].DefaultCellStyle.Format = "P1";         //百分比

                }
                else if (item._ifbend && !item._iftao)//弯不套
                {
                    _diameterlist = GeneralClass.SQLiteOpt.QueryAllListByDiameter(item._datalist);

                    dt_z.Rows.Clear();
                    totalnum = 0;
                    totalweight = 0;
                    foreach (var it in _diameterlist)
                    {
                        totalnum += it._totalnum;
                        totalweight += it._totalweight;
                    }
                    foreach (var it in _diameterlist)
                    {
                        dt_z.Rows.Add("Φ" + it._diameter.ToString(), it._totalnum, (double)it._totalnum / (double)totalnum, it._totalweight, it._totalweight / totalweight);
                    }

                    dataGridView21.DataSource = dt_z;
                    dataGridView21.Columns[2].DefaultCellStyle.Format = "P1";         //百分比
                    dataGridView21.Columns[3].DefaultCellStyle.Format = "0.0";        //double保留两位小数
                    dataGridView21.Columns[4].DefaultCellStyle.Format = "P1";         //百分比

                }
                else if (item._ifbend && item._iftao)//弯套
                {
                    _diameterlist = GeneralClass.SQLiteOpt.QueryAllListByDiameter(item._datalist);

                    dt_z.Rows.Clear();
                    totalnum = 0;
                    totalweight = 0;
                    foreach (var it in _diameterlist)
                    {
                        totalnum += it._totalnum;
                        totalweight += it._totalweight;
                    }
                    foreach (var it in _diameterlist)
                    {
                        dt_z.Rows.Add("Φ" + it._diameter.ToString(), it._totalnum, (double)it._totalnum / (double)totalnum, it._totalweight, it._totalweight / totalweight);
                    }

                    dataGridView22.DataSource = dt_z;
                    dataGridView22.Columns[2].DefaultCellStyle.Format = "P1";         //百分比
                    dataGridView22.Columns[3].DefaultCellStyle.Format = "0.0";        //double保留两位小数
                    dataGridView22.Columns[4].DefaultCellStyle.Format = "P1";         //百分比

                }

            }

        }
        private void FillDGV_Bang_type(List<RebarData> _list, EnumWorkType _worktype, ref DataGridView _dgv/*, out int total_num, out double total_weight*/)
        {

            int[] _num = new int[(int)EnumRebarBang.maxRebarBangNum];                //按照14,16,18,20,22,25,28,32,36,40十种直径来统计
            double[] _weight = new double[(int)EnumRebarBang.maxRebarBangNum];
            int total_num = 0;
            double total_weight = 0;

            DataTable dt_z = new DataTable();
            dt_z.Columns.Add("直径", typeof(string));
            dt_z.Columns.Add("数量", typeof(int));
            dt_z.Columns.Add("数量百分比", typeof(double));
            dt_z.Columns.Add("重量(kg)", typeof(double));
            dt_z.Columns.Add("重量百分比", typeof(double));

            //_list已经过滤过，棒材、非原材、非多段
            foreach (RebarData _ddd in _list)
            {
                switch (_worktype)
                {
                    case EnumWorkType.NO_BEND_NO_TAO:
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
                    case EnumWorkType.NO_BEND_YES_TAO:
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
                    case EnumWorkType.YES_BEND_NO_TAO:
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
                    case EnumWorkType.YES_BEND_YES_TAO:
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
            _dgv.Columns[2].DefaultCellStyle.Format = "P1";         //百分比
            _dgv.Columns[3].DefaultCellStyle.Format = "0.0";        //double保留两位小数
            _dgv.Columns[4].DefaultCellStyle.Format = "P1";         //百分比
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                GeneralClass.interactivityData?.printlog(1, "开始统计所有棒材");

                GeneralClass.AllRebarList = GeneralClass.SQLiteOpt.GetAllRebarList(GeneralClass.AllRebarTableName);//取得所有的钢筋数据list

                List<RebarData> _sonData = new List<RebarData>();//分子
                List<RebarData> _mumData = new List<RebarData>();   //分母

                foreach (RebarData _dd in GeneralClass.AllRebarList)
                {
                    int _threshold = (this.diameterChecked_14) ? 14 : 16;//如果14为线材，则从16开始统计
                    if (_dd.Diameter >= _threshold && _dd.TotalPieceNum != 0
                            &&
                            (
                                (checkBox19.Checked ? (_dd.IsOriginal) : false) ||
                                (checkBox20.Checked ? (!_dd.IsOriginal
                                                    &&(_dd.IfBend
                                                        ||(!_dd.IfBend
                                                            &&_dd.Length!="3000"
                                                            &&_dd.Length!="4000"
                                                            &&_dd.Length!="5000"
                                                            &&_dd.Length!="6000"
                                                            &&_dd.Length!="7000"))
                                                    ) : false)||
                                (checkBox37.Checked?(!_dd.IsOriginal   
                                                    &&(!_dd.IfBend)
                                                    &&(       _dd.Length=="3000"   
                                                            ||_dd.Length=="4000"
                                                            ||_dd.Length=="5000"
                                                            ||_dd.Length=="6000"
                                                            ||_dd.Length=="7000")
                                                    ):false)
                            )
                            &&
                            (
                                (checkBox2.Checked ? (_dd.IfTao) : false) ||
                                (checkBox3.Checked ? (!_dd.IfTao) : false)
                            )
                            &&
                            (
                                (checkBox16.Checked ? (_dd.IfBend) : false) ||
                                (checkBox17.Checked ? (!_dd.IfBend) : false)
                            )
                            &&
                            (
                                ((checkBox8.Checked && this.diameterChecked_14) ? (_dd.Diameter == 14) : false) ||
                                (checkBox9.Checked ? (_dd.Diameter == 16) : false) ||
                                (checkBox10.Checked ? (_dd.Diameter == 18) : false) ||
                                (checkBox11.Checked ? (_dd.Diameter == 20) : false) ||
                                (checkBox12.Checked ? (_dd.Diameter == 22) : false) ||
                                (checkBox13.Checked ? (_dd.Diameter == 25) : false) ||
                                (checkBox14.Checked ? (_dd.Diameter == 28) : false) ||
                                (checkBox15.Checked ? (_dd.Diameter == 32) : false) ||
                                (checkBox18.Checked ? (_dd.Diameter == 36) : false) ||
                                (checkBox39.Checked ? (_dd.Diameter == 40) : false)
                            )
                        )
                    {
                        _sonData.Add(_dd);
                    }
                }

                foreach (RebarData _dd in GeneralClass.AllRebarList)
                {
                    int _threshold = (this.diameterChecked_14) ? 14 : 16;//如果14为线材，则从16开始统计
                    if (_dd.Diameter >= _threshold && _dd.TotalPieceNum != 0
                            &&
                            (
                                (checkBox22.Checked ? (_dd.IsOriginal) : false) ||
                                //(checkBox21.Checked ? (!_dd.IsOriginal) : false)
                                (checkBox21.Checked ? (!_dd.IsOriginal
                                                    && (_dd.IfBend
                                                        || (!_dd.IfBend
                                                            && _dd.Length != "3000"
                                                            && _dd.Length != "4000"
                                                            && _dd.Length != "5000"
                                                            && _dd.Length != "6000"
                                                            && _dd.Length != "7000"))
                                                    ) : false) ||
                                (checkBox38.Checked ? (!_dd.IsOriginal
                                                    && (!_dd.IfBend)
                                                    && (_dd.Length == "3000"
                                                            || _dd.Length == "4000"
                                                            || _dd.Length == "5000"
                                                            || _dd.Length == "6000"
                                                            || _dd.Length == "7000")
                                                    ) : false)

                            )
                            &&
                            (
                                (checkBox25.Checked ? (_dd.IfTao) : false) ||
                                (checkBox26.Checked ? (!_dd.IfTao) : false)
                            )
                            &&
                            (
                                (checkBox24.Checked ? (_dd.IfBend) : false) ||
                                (checkBox23.Checked ? (!_dd.IfBend) : false)
                            )
                            &&
                            (
                                ((checkBox35.Checked && this.diameterChecked_14) ? (_dd.Diameter == 14) : false) ||
                                (checkBox34.Checked ? (_dd.Diameter == 16) : false) ||
                                (checkBox33.Checked ? (_dd.Diameter == 18) : false) ||
                                (checkBox32.Checked ? (_dd.Diameter == 20) : false) ||
                                (checkBox31.Checked ? (_dd.Diameter == 22) : false) ||
                                (checkBox30.Checked ? (_dd.Diameter == 25) : false) ||
                                (checkBox29.Checked ? (_dd.Diameter == 28) : false) ||
                                (checkBox28.Checked ? (_dd.Diameter == 32) : false) ||
                                (checkBox27.Checked ? (_dd.Diameter == 36) : false) ||
                                (checkBox40.Checked ? (_dd.Diameter == 40) : false)
                            )
                        )
                    {
                        _mumData.Add(_dd);
                    }
                }
                //FillDGV_Bang_type(_sonData);
                FillDGV_Bang_Worktype(_sonData, _mumData);
                FillDGV_PiliangCut(_sonData, _mumData);
                FillDGV_LengthArea(_sonData, _mumData);
                FillDGV_Bang_Diameter(_sonData, _mumData);

                //int[] total_num = new int[4];
                //double[] total_weight = new double[4];
                //FillDGV_Bang_type(_sonData, ref dataGridView19, out total_num[0], out total_weight[0]);
                //FillDGV_Bang_type(_sonData, ref dataGridView20, out total_num[1], out total_weight[1]);
                //FillDGV_Bang_type(_sonData, ref dataGridView21, out total_num[2], out total_weight[2]);
                //FillDGV_Bang_type(_sonData, ref dataGridView22, out total_num[3], out total_weight[3]);
                FillDGV_Bang_type(_sonData, EnumWorkType.NO_BEND_NO_TAO, ref dataGridView19);
                FillDGV_Bang_type(_sonData, EnumWorkType.NO_BEND_YES_TAO, ref dataGridView20);
                FillDGV_Bang_type(_sonData, EnumWorkType.YES_BEND_NO_TAO, ref dataGridView21);
                FillDGV_Bang_type(_sonData, EnumWorkType.YES_BEND_YES_TAO, ref dataGridView22);


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void FillDGV_Bang_Diameter(List<RebarData> _sonlist, List<RebarData> _mumlist)
        {
            List<GroupbyDiameterDatalist> _songrouplist = GeneralClass.SQLiteOpt.QueryAllListByDiameter(_sonlist);
            List<GroupbyDiameterDatalist> _mumgrouplist = GeneralClass.SQLiteOpt.QueryAllListByDiameter(_mumlist);

            DataTable dt_z = new DataTable();
            dt_z.Columns.Add("直径", typeof(string));
            dt_z.Columns.Add("数量", typeof(int));
            dt_z.Columns.Add("数量(%)", typeof(double));
            dt_z.Columns.Add("重量(kg)", typeof(double));
            dt_z.Columns.Add("重量(%)", typeof(double));

            //_list已经过滤过，棒材、非原材、非多段
            EnumRebarBang _start;
            if (this.diameterChecked_14)
            {
                _start = EnumRebarBang.BANG_C14;
            }
            else
            {
                _start = EnumRebarBang.BANG_C16;
            }
            int[] _num = new int[(int)EnumRebarBang.maxRebarBangNum];
            double[] _weight = new double[(int)EnumRebarBang.maxRebarBangNum];
            int total_num = 0;
            double total_weight = 0;

            for (int i = (int)_start; i < (int)EnumRebarBang.maxRebarBangNum; i++)
            {
                foreach (var item in _songrouplist)
                {
                    if (item._diameter == Convert.ToInt32(((EnumRebarBang)i).ToString().Substring(6, 2)))
                    {
                        _num[i] = item._totalnum;
                        _weight[i] = item._totalweight;
                        break;
                    }
                }
                foreach (var item in _mumgrouplist)
                {
                    if (item._diameter == Convert.ToInt32(((EnumRebarBang)i).ToString().Substring(6, 2)))
                    {
                        total_num += item._totalnum;
                        total_weight += item._totalweight;
                    }
                }
            }
            for (int i = (int)_start; i < (int)EnumRebarBang.maxRebarBangNum; i++)
            {
                dt_z.Rows.Add("Φ" + ((EnumRebarBang)i).ToString().Substring(6, 2), _num[i], (double)_num[i] / (double)total_num, _weight[i], _weight[i] / total_weight);
            }

            dataGridView8.DataSource = dt_z;
            dataGridView8.Columns[2].DefaultCellStyle.Format = "P1";         //百分比
            dataGridView8.Columns[3].DefaultCellStyle.Format = "0.0";        //double保留两位小数
            dataGridView8.Columns[4].DefaultCellStyle.Format = "P1";         //百分比
        }
        /// <summary>
        /// 棒材的工艺类型汇总统计
        /// </summary>
        /// <param name="_sonlist"></param>
        private void FillDGV_Bang_Worktype(List<RebarData> _sonlist, List<RebarData> _mumlist)
        {
            List<GroupbyTaoBendDatalist> _songrouplist = GeneralClass.SQLiteOpt.QueryAllListByTaoBend(_sonlist);
            List<GroupbyTaoBendDatalist> _mumgrouplist = GeneralClass.SQLiteOpt.QueryAllListByTaoBend(_mumlist);

            DataTable dt = new DataTable();

            dt.Columns.Add("类型", typeof(string));
            dt.Columns.Add("数量(根)", typeof(int));
            dt.Columns.Add("数量(%)", typeof(double));
            dt.Columns.Add("重量(kg)", typeof(double));
            dt.Columns.Add("重量(%)", typeof(double));

            int[] total_num = new int[4];
            double[] total_weight = new double[4];

            foreach (GroupbyTaoBendDatalist item in _songrouplist)
            {
                if (!item._iftao && !item._ifbend)
                {
                    total_num[0] = item._totalnum;
                    total_weight[0] = item._totalweight;
                }
                else if (item._iftao && !item._ifbend)
                {
                    total_num[1] = item._totalnum;
                    total_weight[1] = item._totalweight;
                }
                else if (!item._iftao && item._ifbend)
                {
                    total_num[2] = item._totalnum;
                    total_weight[2] = item._totalweight;
                }
                else if (item._iftao && item._ifbend)
                {
                    total_num[3] = item._totalnum;
                    total_weight[3] = item._totalweight;
                }
            }

            int all_num = 0;
            double all_weight = 0;
            //for (int i = 0; i < 4; i++)
            //{
            //    all_num += total_num[i];
            //    all_weight += total_weight[i];
            //}
            foreach (GroupbyTaoBendDatalist item in _mumgrouplist)//使用分母list的统计数据作为分母
            {
                all_num += item._totalnum;
                all_weight += item._totalweight;
            }
            dt.Rows.Add("不弯不套", total_num[0], (double)total_num[0] / (double)all_num, total_weight[0], total_weight[0] / all_weight);
            dt.Rows.Add("不弯套", total_num[1], (double)total_num[1] / (double)all_num, total_weight[1], total_weight[1] / all_weight);
            dt.Rows.Add("弯不套", total_num[2], (double)total_num[2] / (double)all_num, total_weight[2], total_weight[2] / all_weight);
            dt.Rows.Add("弯套", total_num[3], (double)total_num[3] / (double)all_num, total_weight[3], total_weight[3] / all_weight);
            //dt.Rows.Add("总计", all_num, (double)all_num / (double)all_num, all_weight, all_weight / all_weight);
            dt.Rows.Add("总计",
                (total_num[0] + total_num[1] + total_num[2] + total_num[3]),
                (double)(total_num[0] + total_num[1] + total_num[2] + total_num[3]) / (double)all_num,
                (total_weight[0] + total_weight[1] + total_weight[2] + total_weight[3]),
                (total_weight[0] + total_weight[1] + total_weight[2] + total_weight[3]) / all_weight);

            dataGridView25.DataSource = dt;
            dataGridView25.Columns[2].DefaultCellStyle.Format = "P1";
            dataGridView25.Columns[3].DefaultCellStyle.Format = "0.00";
            dataGridView25.Columns[4].DefaultCellStyle.Format = "P1";
        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                GeneralClass.interactivityData?.printlog(1, "开始进行料单的详细分析");

                GeneralClass.AllRebarList = GeneralClass.SQLiteOpt.GetAllRebarList(GeneralClass.AllRebarTableName);

                //GeneralClass.AllDetailData = GeneralClass.SQLiteOpt.DetailAnalysis(GeneralClass.AllRebarList);

                GeneralClass.AllDetailOtherData = GeneralClass.SQLiteOpt.DetailAnalysis2(GeneralClass.AllRebarList, checkBox7.Checked, checkBox4.Checked, checkBox5.Checked, checkBox6.Checked);

                GeneralClass.interactivityData?.printlog(1, "料单详细分析完成，请选择统计项");

                ////汇总所有图形的list
                //List<RebarData> _newdatalist = new List<RebarData>();
                //foreach (RebarData _dd in GeneralClass.AllRebarList)//先把rebardata列表里面筛出原材和多段的
                //{
                //    if (!_dd.IsMulti && !_dd.IsOriginal)
                //    {
                //        _newdatalist.Add(_dd);
                //    }
                //}
                //GeneralClass.ExistRebarPicTypeList = GeneralClass.SQLiteOpt.GetExistedRebarTypeList(_newdatalist);//得到列表中包含的钢筋图形编号列表
                GeneralClass.ExistRebarPicTypeList = GeneralClass.SQLiteOpt.GetExistedRebarTypeList(GeneralClass.AllRebarList);//得到列表中包含的钢筋图形编号列表


                dataGridView6.Rows.Clear();
                //重新添加行
                string sType = "";
                int _index = -1;

                DataGridViewRow row = new DataGridViewRow();
                //for (int j = 0; j < (int)EnumDetailTableColName.ONLY_CUT; j++)//添加原材的row
                //{
                //    row = new DataGridViewRow();
                //    row.HeaderCell.Value = GeneralClass.sDetailTableColName[j];//
                //    dataGridView6.Rows.Add(row);
                //}
                //if (FindIndexInImagelist("10000", out _index))//按照图形编号查询图片库中的index
                //{
                //    dataGridView6.Rows[0].Cells[0].Value = imageList1.Images[_index];//按照index，显示图形
                //}
                //if (FindIndexInImagelist("10000", out _index))//按照图形编号查询图片库中的index
                //{
                //    dataGridView6.Rows[1].Cells[0].Value = imageList1.Images[_index];//按照index，显示图形
                //}
                //if (FindIndexInImagelist("20100", out _index))//按照图形编号查询图片库中的index
                //{
                //    dataGridView6.Rows[2].Cells[0].Value = imageList1.Images[_index];//按照index，显示图形
                //}

                for (int i = 0; i < GeneralClass.ExistRebarPicTypeList.Count; i++)//添加非原材的row
                {
                    row = new DataGridViewRow();
                    row.HeaderCell.Value = GeneralClass.ExistRebarPicTypeList[i].ToString().Substring(2, 5);//
                    dataGridView6.Rows.Add(row);

                    sType = GeneralClass.ExistRebarPicTypeList[i].ToString().Substring(2, 5);
                    if (FindIndexInImagelist(sType, out _index))//按照图形编号查询图片库中的index
                    {
                        //dataGridView6.Rows[i + 3].Cells[0].Value = imageList1.Images[_index];//按照index，显示图形
                        dataGridView6.Rows[i].Cells[0].Value = imageList1.Images[_index];//按照index，显示图形

                    }
                }

                //添加汇总行
                row = new DataGridViewRow();
                row.HeaderCell.Value = "总计";
                dataGridView6.Rows.Add(row);


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }


        private void GetSheetToDGV(string _projectname, string _assemblyname)
        {
            //先获取备份的钢筋总表
            //List<RebarData> _allList_bk = GeneralClass.SQLiteOpt.GetAllRebarList(GeneralClass.AllRebarBKTableName);
            List<RebarData> _allList_bk = GeneralClass.SQLiteOpt.GetAllRebarList(GeneralClass.AllRebarTableName);

            //查询所有被选中的钢筋
            List<RebarData> _newlist = new List<RebarData>();
            foreach (RebarData _data in _allList_bk)
            {
                //if ((bool)GeneralClass.interactivityData?.ifRebarSelected(_data))//查询钢筋是否被选中
                //{
                //    _newlist.Add(_data);
                //}
                if (_data.ProjectName == _projectname && _data.MainAssemblyName == _assemblyname)
                {
                    _newlist.Add(_data);
                }
            }

            if (_newlist.Count != 0 && dataGridView1 != null)
            {
                FillDGVWithRebarList(_newlist, dataGridView1);
            }



        }

        /// <summary>
        /// 其他界面也可以使用此方法
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="_dgv"></param>
        public static void FillDGVWithRebarList(List<RebarData> _list, DataGridView _dgv)
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
                object _image = null;
                if (GeneralClass.interactivityData.ifFindImage(sType, out _image))
                {
                    dgvImageCell.Value = _image;
                }
                //int _iiiii = -1;
                //if (FindIndexInImagelist(sType, out _iiiii))//按照图形编号查询图片库中的index
                //{
                //    dgvImageCell.Value = imageList1.Images[_iiiii];//按照index，显示图形
                //}
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
                dgvCell.Value = _dd.TotalWeight.ToString("0.00");
                dgvRow.Cells.Add(dgvCell);



                //13备注
                dgvCell = new DataGridViewTextBoxCell();
                dgvCell.Value = _dd.Description;
                dgvRow.Cells.Add(dgvCell);

                ////14标注序号
                //dgvCell = new DataGridViewTextBoxCell();
                //dgvCell.Value = _dd.SerialNum;
                //dgvRow.Cells.Add(dgvCell);

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

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            #region 重绘标签头=======================
            SolidBrush back;
            SolidBrush white;
            if (e.Index == tabControl1.SelectedIndex)//当前Tab page页的样式
            {
                //背景颜色
                back = new SolidBrush(Color.Wheat);
                //back = new SolidBrush(Color.Blue);
                //字体颜色
                white = new SolidBrush(Color.Black);
            }
            else//其余Tab page页的样式
            {
                //背景颜色
                //back = new SolidBrush(Color.SeaShell);
                back = new SolidBrush(SystemColors.GradientInactiveCaption);
                //字体颜色
                white = new SolidBrush(Color.Black);
            }
            StringFormat sf = new StringFormat()
            {
                //文本水平/垂直居中对齐
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
            };

            //绑定选项卡
            Rectangle rec = tabControl1.GetTabRect(e.Index);
            //设置选项卡背景
            e.Graphics.FillRectangle(back, rec);

            //设置选项卡字体及颜色
            e.Graphics.DrawString(tabControl1.TabPages[e.Index].Text, new Font("微软雅黑", 10.8f, FontStyle.Bold), white, rec, sf);
            #endregion

        }



        private void button1_Click(object sender, EventArgs e)
        {
            panel2.Visible = !panel2.Visible;
            button1.BackColor = panel2.Visible ? Color.Wheat : SystemColors.GradientInactiveCaption;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel3.Visible = !panel3.Visible;
            button2.BackColor = panel3.Visible ? Color.Wheat : SystemColors.GradientInactiveCaption;

        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel4.Visible = !panel4.Visible;
            button6.BackColor = panel4.Visible ? Color.Wheat : SystemColors.GradientInactiveCaption;

        }

        private void button11_Click(object sender, EventArgs e)
        {
            panel5.Visible = !panel5.Visible;
            button11.BackColor = panel5.Visible ? Color.Wheat : SystemColors.GradientInactiveCaption;

        }
        //private void button13_Click(object sender, EventArgs e)
        //{
        //    //panel6.Visible = !panel6.Visible;
        //    //button13.BackColor = panel6.Visible ? Color.Wheat :SystemColors.GradientInactiveCaption;
        //}

        /// <summary>
        /// 棒材的批量锯切统计，主要是按照长度统计
        /// </summary>
        /// <param name="_sonlist"></param>
        private void FillDGV_PiliangCut(List<RebarData> _sonlist, List<RebarData> _mumlist)
        {

            DataTable dt = new DataTable();

            //dt.Columns.Add("长度(mm)", typeof(string));
            dt.Columns.Add("长度(mm)", typeof(int));
            dt.Columns.Add("数量(根)", typeof(int));
            dt.Columns.Add("数量(%)", typeof(double));
            dt.Columns.Add("重量(kg)", typeof(double));
            dt.Columns.Add("重量(%)", typeof(double));

            List<GroupbyLengthDatalist> _sonalllist = GeneralClass.SQLiteOpt.QueryAllListByLength(_sonlist);
            List<GroupbyLengthDatalist> _mumalllist = GeneralClass.SQLiteOpt.QueryAllListByLength(_mumlist);

            double total_num = 0;
            double total_weight = 0.0;

            //foreach (var item in _sonalllist)
            //{
            //    total_num += item._totalnum;
            //    total_weight += item._totalweight;
            //}
            foreach (var item in _mumalllist)//使用分母list的统计数据，作为分母
            {
                total_num += item._totalnum;
                total_weight += item._totalweight;
            }
            int ilength = 0;
            foreach (var item in _sonalllist)
            {
                if (!int.TryParse(item._length, out ilength))
                {
                    string[] tt = item._length.Split('~');
                    ilength = (Convert.ToInt32(tt[0]) + Convert.ToInt32(tt[1])) / 2;
                }

                dt.Rows.Add(ilength, item._totalnum, (double)item._totalnum / total_num, item._totalweight, item._totalweight / total_weight);
            }

            dataGridView2.DataSource = dt;
            dataGridView2.Columns[2].DefaultCellStyle.Format = "P2";
            dataGridView2.Columns[3].DefaultCellStyle.Format = "0.00";
            dataGridView2.Columns[4].DefaultCellStyle.Format = "P2";

        }
        /// <summary>
        /// 棒材的长度区间分类，按照1米~12米共计12个长度区间
        /// </summary>
        /// <param name="_sonlist"></param>
        private void FillDGV_LengthArea(List<RebarData> _sonlist, List<RebarData> _mumlist)
        {
            //按照长度12个区间进行统计
            DataTable dt = new DataTable();

            dt.Columns.Add("长度区间(mm)", typeof(string));
            dt.Columns.Add("数量(根)", typeof(int));
            dt.Columns.Add("数量(%)", typeof(double));
            dt.Columns.Add("重量(kg)", typeof(double));
            dt.Columns.Add("重量(%)", typeof(double));

            List<GroupbyLengthDatalist> _sonalllist = GeneralClass.SQLiteOpt.QueryAllListByLength(_sonlist);
            List<GroupbyLengthDatalist> _mumalllist = GeneralClass.SQLiteOpt.QueryAllListByLength(_mumlist);

            int total_num_bend = 0;
            double total_weight_bend = 0;
            int[] area_totalnum = new int[12];
            double[] area_totalweight = new double[12];

            foreach (var item in _mumalllist)
            {
                total_num_bend += item._totalnum;
                total_weight_bend += item._totalweight;
            }
            foreach (var item in _sonalllist)
            {
                //total_num_bend += item._totalnum;
                //total_weight_bend += item._totalweight;

                int temp = 0;
                int.TryParse(item._length, out temp);

                if (temp != 0 && temp / 1000 < 12)
                {
                    area_totalnum[temp / 1000] += item._totalnum;
                    area_totalweight[temp / 1000] += item._totalweight;
                }
                else
                {
                    continue;
                }
            }
            for (int i = 0; i < 12; i++)
            {
                dt.Rows.Add((i * 1000).ToString() + "~" + ((i + 1) * 1000).ToString(), area_totalnum[i], (double)area_totalnum[i] / (double)total_num_bend, area_totalweight[i], area_totalweight[i] / total_weight_bend);
            }
            dataGridView3.DataSource = dt;
            dataGridView3.Columns[2].DefaultCellStyle.Format = "P1";
            dataGridView3.Columns[3].DefaultCellStyle.Format = "0.00";
            dataGridView3.Columns[4].DefaultCellStyle.Format = "P1";

        }
        private void button12_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    GeneralClass.interactivityData?.printlog(1, "开始统计所有棒材非原材中批量锯切数据");

            //    GeneralClass.AllRebarList = GeneralClass.SQLiteOpt.GetAllRebarList(GeneralClass.AllRebarTableName);//取得所有的钢筋数据list


            //    #region 统计需锯切的总数量和总重量                
            //    List<RebarData> _bangCountdata = new List<RebarData>();
            //    foreach (RebarData _dd in GeneralClass.AllRebarList)
            //    {
            //        if (_dd.Diameter >= 14 && !_dd.IsOriginal && _dd.TotalPieceNum != 0)
            //        {
            //            _bangCountdata.Add(_dd);
            //        }
            //    }
            //    List<GroupbyLengthDatalist> _allCountlist = GeneralClass.SQLiteOpt.QueryAllListByLength(_bangCountdata);

            //    double total_num = 0;
            //    double total_weight = 0.0;

            //    foreach (var item in _allCountlist)
            //    {
            //        total_num += item._totalnum;
            //        total_weight += item._totalweight;
            //    }
            //    #endregion


            //    List<RebarData> _bangdata = new List<RebarData>();
            //    foreach (RebarData _dd in GeneralClass.AllRebarList)
            //    {
            //        if (_dd.Diameter >= 14 && !_dd.IsOriginal && _dd.TotalPieceNum != 0
            //            &&
            //            (
            //            (checkBox2.Checked ? (_dd.IfTao) : false) ||
            //            (checkBox3.Checked ? (!_dd.IfTao) : false)
            //            )
            //            && (
            //            (checkBox16.Checked ? (_dd.IfBend) : false) ||
            //            (checkBox17.Checked ? (!_dd.IfBend) : false)
            //            )
            //            &&
            //            (
            //            (checkBox8.Checked ? (_dd.Diameter == 14) : false) ||
            //            (checkBox9.Checked ? (_dd.Diameter == 16) : false) ||
            //            (checkBox10.Checked ? (_dd.Diameter == 18) : false) ||
            //            (checkBox11.Checked ? (_dd.Diameter == 20) : false) ||
            //            (checkBox12.Checked ? (_dd.Diameter == 22) : false) ||
            //            (checkBox13.Checked ? (_dd.Diameter == 25) : false) ||
            //            (checkBox14.Checked ? (_dd.Diameter == 28) : false) ||
            //            (checkBox15.Checked ? (_dd.Diameter == 32) : false) ||
            //            (checkBox18.Checked ? (_dd.Diameter == 36) : false)
            //            )
            //            )
            //        {
            //            _bangdata.Add(_dd);
            //        }
            //    }


            //    //汇总统计
            //    DataTable dt_hz = new DataTable();

            //    dt_hz.Columns.Add("长度(mm)", typeof(string));
            //    dt_hz.Columns.Add("数量(根)", typeof(int));
            //    dt_hz.Columns.Add("数量(%)", typeof(double));
            //    dt_hz.Columns.Add("重量(kg)", typeof(double));
            //    dt_hz.Columns.Add("重量(%)", typeof(double));

            //    List<GroupbyLengthDatalist> _alllist = GeneralClass.SQLiteOpt.QueryAllListByLength(_bangdata);

            //    foreach (var item in _alllist)
            //    {
            //        dt_hz.Rows.Add(item._length, item._totalnum, item._totalnum / total_num, item._totalweight, item._totalweight / total_weight);
            //    }

            //    dataGridView2.DataSource = dt_hz;
            //    dataGridView2.Columns[2].DefaultCellStyle.Format = "P1";
            //    dataGridView2.Columns[3].DefaultCellStyle.Format = "0.00";
            //    dataGridView2.Columns[4].DefaultCellStyle.Format = "P1";




            //    #region 统计需锯切需弯曲的总数量和总重量                
            //    List<RebarData> _bangBenddata = new List<RebarData>();
            //    foreach (RebarData _dd in GeneralClass.AllRebarList)
            //    {
            //        if (_dd.Diameter >= 14 && !_dd.IsOriginal && _dd.TotalPieceNum != 0 && _dd.IfBend)
            //        {
            //            _bangBenddata.Add(_dd);
            //        }
            //    }
            //    List<GroupbyLengthDatalist> _allBendlist = GeneralClass.SQLiteOpt.QueryAllListByLength(_bangBenddata);

            //    double total_num_bend = 0;
            //    double total_weight_bend = 0.0;

            //    foreach (var item in _allBendlist)
            //    {
            //        total_num_bend += item._totalnum;
            //        total_weight_bend += item._totalweight;
            //    }
            //    #endregion


            //    //按照长度12个区间进行统计
            //    DataTable dt_wq = new DataTable();

            //    dt_wq.Columns.Add("长度区间(mm)", typeof(string));
            //    dt_wq.Columns.Add("数量(根)", typeof(int));
            //    dt_wq.Columns.Add("数量(%)", typeof(double));
            //    dt_wq.Columns.Add("重量(kg)", typeof(double));
            //    dt_wq.Columns.Add("重量(%)", typeof(double));

            //    double[] wq_totalnum = new double[12];
            //    double[] wq_totalweight = new double[12];

            //    foreach (var item in _alllist)
            //    {
            //        int temp = 0;
            //        int.TryParse(item._length, out temp);

            //        if (temp != 0 && temp / 1000 < 12)
            //        {
            //            wq_totalnum[temp / 1000] += item._totalnum;
            //            wq_totalweight[temp / 1000] += item._totalweight;
            //        }
            //        else
            //        {
            //            continue;
            //        }
            //    }
            //    for (int i = 0; i < 12; i++)
            //    {
            //        dt_wq.Rows.Add((i * 1000).ToString() + "~" + ((i + 1) * 1000).ToString(), wq_totalnum[i], wq_totalnum[i] / total_num_bend, wq_totalweight[i], wq_totalweight[i] / total_weight_bend);
            //    }
            //    dataGridView3.DataSource = dt_wq;
            //    dataGridView3.Columns[2].DefaultCellStyle.Format = "P1";
            //    dataGridView3.Columns[3].DefaultCellStyle.Format = "0.00";
            //    dataGridView3.Columns[4].DefaultCellStyle.Format = "P1";

            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        /// <summary>
        /// true为Φ14棒材，false为Φ14线材
        /// </summary>
        private bool diameterChecked_14 = true;
        private void checkBox36_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox36.Checked)
            {
                checkBox36.Checked = false;
                checkBox36.Text = "Φ14-线材";
                this.diameterChecked_14 = false;

                checkBox8.Checked = false;
                checkBox8.Enabled = false;
                checkBox35.Checked = false;
                checkBox35.Enabled = false;
            }
            else
            {
                checkBox36.Checked = true;
                checkBox36.Text = "Φ14-棒材";
                this.diameterChecked_14 = true;

                checkBox8.Checked = true;
                checkBox8.Enabled = true;
                checkBox35.Checked = true;
                checkBox35.Enabled = true;
            }

        }


    }
}
