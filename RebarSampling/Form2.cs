//using Etable;
using NPOI.OpenXmlFormats.Dml.Chart;
using NPOI.SS.Formula.Functions;
using RebarSampling.GLD;
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
using System.Xml.Linq;

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
            //InitDataGridView4();
            //InitDataGridView5();
            InitDataGridView6();
            //InitDataGridView7();
            InitDataGridView8();
            InitDataGridView9();
            InitDataGridView10();
            InitDGV_element();

            InitRadioButtion();
            InitCheckBox();
            InitCheckBox_element();

            InitStatisticsDGV();
            InitLabel();

            InitTreeView1();

            //InitManualTao();

            //tabControl1.Enabled = false;
            GeneralClass.interactivityData.initStatisticsDGV += InitStatisticsDGV;

            GeneralClass.interactivityData.showAssembly += GetSheetToDGV;

            GeneralClass.interactivityData.ifFindImage += FindImageInImagelist;

            GeneralClass.interactivityData.showAssembly += ShowAllElement;

            GeneralClass.interactivityData.getImageUsePicNum += GetImageFromList;

        }
        private void InitStatisticsDGV()
        {
            InitDGV_BX();
            InitDGV_Xian();
            InitDGV_BangOri();
            InitDGV_Bang();
        }

        //private void InitManualTao()
        //{
        //    for (int i = 1; i <= 20; i++)
        //    {
        //        this.comboBox1.Items.Add(i.ToString());
        //    }
        //    this.comboBox1.SelectedIndex = 0;

        //    label27.Text = ((int)((double)GeneralClass.OriginalLength / Convert.ToInt32(label20.Text))).ToString();
        //    label21.Text = ((int)((double)GeneralClass.OriginalLength / Convert.ToInt32(label22.Text))).ToString();
        //    label26.Text = ((int)((double)GeneralClass.OriginalLength / Convert.ToInt32(label23.Text))).ToString();
        //    label35.Text = ((int)((double)GeneralClass.OriginalLength / Convert.ToInt32(label25.Text))).ToString();
        //    label37.Text = ((int)((double)GeneralClass.OriginalLength / Convert.ToInt32(label28.Text))).ToString();

        //    label39.Text = ((int)((double)GeneralClass.OriginalLength / Convert.ToInt32(label29.Text))).ToString();
        //    label41.Text = ((int)((double)GeneralClass.OriginalLength / Convert.ToInt32(label30.Text))).ToString();
        //    label43.Text = ((int)((double)GeneralClass.OriginalLength / Convert.ToInt32(label31.Text))).ToString();
        //    label45.Text = ((int)((double)GeneralClass.OriginalLength / Convert.ToInt32(label32.Text))).ToString();
        //    label47.Text = ((int)((double)GeneralClass.OriginalLength / Convert.ToInt32(label33.Text))).ToString();

        //    label72.Text = ((int)((double)GeneralClass.OriginalLength / Convert.ToInt32(label77.Text))).ToString();
        //    label65.Text = ((int)((double)GeneralClass.OriginalLength / Convert.ToInt32(label76.Text))).ToString();
        //    label63.Text = ((int)((double)GeneralClass.OriginalLength / Convert.ToInt32(label75.Text))).ToString();
        //    label61.Text = ((int)((double)GeneralClass.OriginalLength / Convert.ToInt32(label74.Text))).ToString();
        //    label59.Text = ((int)((double)GeneralClass.OriginalLength / Convert.ToInt32(label73.Text))).ToString();

        //    label57.Text = ((int)((double)GeneralClass.OriginalLength / Convert.ToInt32(label71.Text))).ToString();
        //    label55.Text = ((int)((double)GeneralClass.OriginalLength / Convert.ToInt32(label70.Text))).ToString();
        //    label53.Text = ((int)((double)GeneralClass.OriginalLength / Convert.ToInt32(label69.Text))).ToString();
        //    label51.Text = ((int)((double)GeneralClass.OriginalLength / Convert.ToInt32(label68.Text))).ToString();
        //    label49.Text = ((int)((double)GeneralClass.OriginalLength / Convert.ToInt32(label67.Text))).ToString();

        //}
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
            //"构件名称","图形编号","级别直径","钢筋简图","图形信息","边角结构","下料长度(mm)","根数/件数","总根数","重量(kg)","备注"
            DataTable dt = new DataTable();
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.ELEMENT_NAME], typeof(string));
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PIC_NO], typeof(string));
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.LEVEL], typeof(string));
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.DIAMETER], typeof(int));
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.REBAR_PIC], typeof(Image));
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.CORNER_MESSAGE], typeof(string));
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.LENGTH], typeof(string));
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PIECE_NUM_UNIT_NUM], typeof(string));
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TOTAL_PIECE_NUM], typeof(int));
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TOTAL_WEIGHT], typeof(double));
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.DESCRIPTION], typeof(string));

            _dgv.DataSource = dt;


            ////"构件名称","编号","级别直径","钢筋简图","图形信息","边角结构","下料长度(mm)","根数/件数","总根数","重量(kg)","备注"
            //DataGridViewColumn column;
            //DataGridViewCell cell;
            //DataGridViewImageColumn imageColumn;

            //for (int i = (int)EnumAllRebarTableColName.PROJECT_NAME; i <= (int)EnumAllRebarTableColName.IFBENDTWICE; i++) //从1开始，子构件名称
            //{
            //    if (i == (int)EnumAllRebarTableColName.PIC_MESSAGE
            //        || i == (int)EnumAllRebarTableColName.ISMULTI
            //        || i == (int)EnumAllRebarTableColName.IFTAO
            //        || i == (int)EnumAllRebarTableColName.IFBEND
            //        || i == (int)EnumAllRebarTableColName.IFBENDTWICE
            //        || i == (int)EnumAllRebarTableColName.IFCUT
            //        || i == (int)EnumAllRebarTableColName.ISORIGINAL
            //        //|| i == (int)EnumAllRebarTableColName.PIECE_NUM_UNIT_NUM
            //        || i == (int)EnumAllRebarTableColName.SERIALNUM

            //        )
            //    {
            //        continue;
            //    }
            //    if (i == (int)EnumAllRebarTableColName.REBAR_PIC)
            //    {
            //        imageColumn = new DataGridViewImageColumn();
            //        imageColumn.HeaderText = GeneralClass.sRebarColumnName[i];//标题
            //        imageColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;//设置图片可以根据单元格大小进行自动调整
            //        _dgv.Columns.Add(imageColumn);
            //    }
            //    else
            //    {
            //        column = new DataGridViewColumn();
            //        cell = new DataGridViewTextBoxCell();
            //        column.CellTemplate = cell;//设置单元格模板
            //        column.HeaderText = GeneralClass.sRebarColumnName[i];//
            //        _dgv.Columns.Add(column);

            //    }
            //}
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
                    //imageColumn.Image = pictureBox1.Image;
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

                for (int i = (int)EnumBangOrXian.BANG_C16; i < (int)EnumBangOrXian.maxRowNum; i++)//只显示棒材的
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

        private void InitDGV_element()
        {
            InitDGV(dataGridView28);
            InitDGV(dataGridView27);
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

            //this.panel2.Size = new System.Drawing.Size(395, 250);
            //this.panel3.Size = new System.Drawing.Size(395, 250);
            //this.panel4.Size = new System.Drawing.Size(395, 250);
            //this.panel5.Size = new System.Drawing.Size(395, 250);
            ////this.panel6.Size = new System.Drawing.Size(395, 250);

            //this.panel2.Visible = false;
            //this.panel3.Visible = false;
            //this.panel4.Visible = false;
            //this.panel5.Visible = false;
            ////this.panel6.Visible = false;

            //分子，套，弯
            this.checkBox2.Checked = true;
            this.checkBox3.Checked = true;
            this.checkBox16.Checked = true;
            this.checkBox17.Checked = true;

            //分子，直径
            this.checkBox68.Checked = true;
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
            this.checkBox69.Checked = true;
            this.checkBox35.Checked = true;
            this.checkBox34.Checked = true;
            this.checkBox33.Checked = true;
            this.checkBox32.Checked = true;
            this.checkBox31.Checked = true;
            this.checkBox30.Checked = true;
            this.checkBox29.Checked = true;
            this.checkBox28.Checked = true;
            this.checkBox27.Checked = true;
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

            checkBox36.Checked = false;
            checkBox36.Text = "Φ14-线材";

            checkBox65.Checked = false;
            checkBox65.Text = "全不选";
            checkBox66.Checked = false;
            checkBox66.Text = "全不选";
            checkBox67.Checked = true;
            checkBox67.Text = "全选";

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

                sqlstr = "select * from " + GeneralClass.TableName_AllRebar + " where ";

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

                DataTable dt = GeneralClass.DBOpt.dbHelper.ExecuteQuery(sqlstr);



                dataGridView7.DataSource = dt;


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }


        private Image GetImageFromList(string _pictypeNum)
        {
            Image image = null;
            //string sType = item.ToString().Substring(2, 5);
            int _index = -1;
            if (FindIndexInImagelist(_pictypeNum, out _index))//按照图形编号查询图片库中的index
            {
                image = imageList1.Images[_index];//按照index，显示图形
            }
            else
            {
                GeneralClass.interactivityData?.printlog(1, "钢筋图片库中找不到编号为:" + _pictypeNum + "的图片，请手动添加");

                if (GeneralClass.m_showNoFindPic)
                {
                    FindIndexInImagelist("00000", out _index);//临时用00000.png代替
                    image = imageList1.Images[_index];
                }
            }
            return image;
        }
        /// <summary>
        /// 统计棒材线材图形
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="_dgv"></param>
        private void FillDGV_Pic(List<RebarData> _list, ref DataGridView _dgv)
        {
            try
            {

                //统计棒材图形
                DataTable dt_z = new DataTable();
                dt_z.Columns.Add("图形编号", typeof(string));
                dt_z.Columns.Add("钢筋简图", typeof(Image));
                dt_z.Columns.Add("边角信息", typeof(string));
                dt_z.Columns.Add("长度", typeof(int));

                dt_z.Columns.Add("数量(根)", typeof(int));
                dt_z.Columns.Add("数量百分比", typeof(double));
                dt_z.Columns.Add("重量(吨)", typeof(double));
                dt_z.Columns.Add("重量百分比", typeof(double));

                var _group = _list.GroupBy(t => new { t.PicTypeNum, t.CornerMessage, t.iLength }).Select(
                    y => new
                    {
                        _picNum = y.Key.PicTypeNum,
                        _cornermsg = y.Key.CornerMessage,
                        _length = y.Key.iLength,

                        _num = y.Sum(x => x.TotalPieceNum),
                        _weight = y.Sum(x => x.TotalWeight),
                        _datalist = y.ToList(),
                    }).ToList();

                int _totalnum = _group.Sum(t => t._num);
                double _totalweight = _group.Sum(t => t._weight);

                foreach (var item in _group)
                {
                    if (GeneralClass.CfgData.MaterialBill == EnumMaterialBill.EJIN)//e筋料单
                    {
                        dt_z.Rows.Add(item._picNum, graphics.PaintRebarPic(item._datalist?.First()), item._cornermsg, item._length, item._num, (double)(item._num) / (double)_totalnum, item._weight / 1000, item._weight / _totalweight);
                    }
                    else//广联达料单
                    {
                        string newpath = GeneralClass.CfgData.GLDpath + @"\" + item._datalist?.First().RebarPic;//广联达料单路径+图片路径即为完整路径
                        //_image = Gld.LoadGldImage(newpath);
                        dt_z.Rows.Add(item._picNum, Gld.LoadGldImage(newpath), item._cornermsg, item._length, item._num, (double)(item._num) / (double)_totalnum, item._weight / 1000, item._weight / _totalweight);
                    }

                }
                _dgv.DataSource = dt_z;
                _dgv.Columns[5].DefaultCellStyle.Format = "P1";         //百分比
                _dgv.Columns[6].DefaultCellStyle.Format = "F2";        //double保留两位小数
                _dgv.Columns[7].DefaultCellStyle.Format = "P1";         //百分比

            }
            catch (Exception ex) { MessageBox.Show("FillDGV_Pic error:" + ex.Message); }

        }
        /// <summary>
        /// 弯曲角度汇总统计
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="_dgv"></param>
        private void FillDGV_Bend(List<RebarData> _list, ref DataGridView _dgv)
        {
            //统计棒材弯曲角度
            DataTable dt_z = new DataTable();
            dt_z.Columns.Add("弯曲角度(°)", typeof(int));
            dt_z.Columns.Add("弯曲次数", typeof(int));

            List<string> xx = new List<string>();
            List<int> yy = new List<int>();

            List<GeneralMultiData> _MultiData = new List<GeneralMultiData>();

            foreach (var item in _list)
            {
                _MultiData.AddRange(GeneralClass.LDOpt.ldhelper.GetMultiData(item.CornerMessage));//拆解corner信息,并汇总
            }

            var _group = _MultiData.GroupBy(t => t.angle).OrderBy(t => t.Key).ToList();//根据弯曲角度分类，同时从小到大排序一下

            foreach (var item in _group)
            {
                if (item.Key != 0)//==0表示不是bend属性的multidata
                {
                    dt_z.Rows.Add(item.Key, item.ToList().Count);

                    xx.Add(item.Key.ToString());
                    yy.Add(item.ToList().Count);
                }
            }

            _dgv.DataSource = dt_z;

            chartshow.ChartRectShow("弯曲角度(x)——弯曲次数(y)", xx, yy, chart6);
            //_dgv.Columns[3].DefaultCellStyle.Format = "P1";         //百分比
            //_dgv.Columns[4].DefaultCellStyle.Format = "F2";        //double保留两位小数
            //_dgv.Columns[5].DefaultCellStyle.Format = "P1";         //百分比
        }
        private void FillDGV_Taosi(List<RebarData> _list, ref DataGridView _dgv)
        {
            try
            {
                DataTable dt_z = new DataTable();
                //dt_z.Columns.Add("图形编号", typeof(string));
                //dt_z.Columns.Add("钢筋简图", typeof(Image));
                dt_z.Columns.Add("端头类型", typeof(string));
                dt_z.Columns.Add("数量", typeof(int));

                List<string> xx = new List<string>();
                List<int> yy = new List<int>();

                List<GeneralMultiData> _MultiData = new List<GeneralMultiData>();

                List<Rebar> _rebarlist = Algorithm.ListExpand(_list);//将rebardata展开成rebar，以免数量计算有误
                foreach (var item in _rebarlist)
                {
                    _MultiData.AddRange(GeneralClass.LDOpt.ldhelper.GetMultiData(item.CornerMessage));//拆解corner信息,并汇总
                }

                var _group = _MultiData.GroupBy(t => t.headType).ToList();//根据弯曲角度分类，同时从小到大排序一下

                foreach (var item in _group)
                {
                    switch (item.Key)
                    {
                        case EnumMultiHeadType.ORG:
                            break;
                        case EnumMultiHeadType.BEND:
                            break;
                        case EnumMultiHeadType.TAO_P:
                            dt_z.Rows.Add("正丝套筒", item.ToList().Count);
                            xx.Add("正丝套筒");
                            yy.Add(item.ToList().Count);
                            break;
                        case EnumMultiHeadType.TAO_V:
                            dt_z.Rows.Add("变径套筒", item.ToList().Count);
                            xx.Add("变径套筒");
                            yy.Add(item.ToList().Count);
                            break;
                        case EnumMultiHeadType.TAO_N:
                            dt_z.Rows.Add("反丝套筒", item.ToList().Count);
                            xx.Add("反丝套筒");
                            yy.Add(item.ToList().Count);
                            break;
                        case EnumMultiHeadType.SI_P:
                            dt_z.Rows.Add("正丝", item.ToList().Count);
                            xx.Add("正丝");
                            yy.Add(item.ToList().Count);
                            break;
                        case EnumMultiHeadType.SI_N:
                            dt_z.Rows.Add("反丝", item.ToList().Count);
                            xx.Add("反丝");
                            yy.Add(item.ToList().Count);
                            break;
                        case EnumMultiHeadType.DA:
                            dt_z.Rows.Add("搭接", item.ToList().Count);
                            xx.Add("搭接");
                            yy.Add(item.ToList().Count);
                            break;
                        default:
                            break;
                    }
                }
                chartshow.ChartRectShow("端头类型(x)——次数(y)", xx, yy, chart7);


                _dgv.DataSource = dt_z;

            }
            catch (Exception ex) { MessageBox.Show("FillDGV_Taosi error:" + ex.Message); return; }

        }
        private void FillDGV_Taosi_Pic(List<RebarData> _list, ref DataGridView _dgv)
        {
            try
            {
                //统计棒材图形
                DataTable dt_z = new DataTable();
                dt_z.Columns.Add("图形编号", typeof(string));
                dt_z.Columns.Add("钢筋简图", typeof(Image));
                dt_z.Columns.Add("仅左端套丝数量(根)", typeof(int));
                dt_z.Columns.Add("仅右端套丝数量(根)", typeof(int));
                dt_z.Columns.Add("两端套丝数量(根)", typeof(int));


                var _group = _list.GroupBy(t => t.PicTypeNum).ToList();


                foreach (var item in _group)
                {
                    var _picNum = item.Key;
                    var _rebarList = item.ToList();

                    int _left = 0, _right = 0, _both = 0;
                    //List<GeneralMultiData> _MultiData;

                    foreach (var ttt in _rebarList)
                    {
                        List<GeneralMultiData> _MultiData = GeneralClass.LDOpt.ldhelper.GetMultiData(ttt.CornerMessage);//拆解corner信息,并汇总

                        if (_MultiData != null && _MultiData.Count != 0)//存在返回为null的情况，需要跳过
                        {
                            if (_MultiData.First().type == 2 && _MultiData.Last().type != 2)
                            {
                                _left += _MultiData.First().num * ttt.TotalPieceNum;//multidata的数量*当前rebardata的总根数
                            }
                            else if (_MultiData.First().type != 2 && _MultiData.Last().type == 2)
                            {
                                _right += _MultiData.Last().num * ttt.TotalPieceNum;//multidata的数量*当前rebardata的总根数
                            }
                            else if (_MultiData.First().type == 2 && _MultiData.Last().type == 2)
                            {
                                _both += (_MultiData.First().num * ttt.TotalPieceNum + _MultiData.Last().num * ttt.TotalPieceNum);//multidata的数量*当前rebardata的总根数
                            }
                        }
                    }

                    dt_z.Rows.Add(_picNum, GetImageFromList(_picNum), _left, _right, _both);
                }
                _dgv.DataSource = dt_z;
                //_dgv.Columns[3].DefaultCellStyle.Format = "P1";         //百分比
                //_dgv.Columns[4].DefaultCellStyle.Format = "F2";        //double保留两位小数
                //_dgv.Columns[5].DefaultCellStyle.Format = "P1";         //百分比

            }
            catch (Exception ex) { MessageBox.Show("FillDGV_Taosi_Pic error:" + ex.Message); return; }


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

                //dataGridView4.Rows.Clear();
                //dataGridView5.Rows.Clear();
                DataTable dt = new DataTable();
                dataGridView4.DataSource = dt;
                dataGridView5.DataSource = dt;

                GeneralClass.AllRebarList = GeneralClass.DBOpt.GetAllRebarList(GeneralClass.TableName_AllRebar);//取得所有的钢筋数据list
                //GeneralClass.ExistRebarPicTypeList = GeneralClass.SQLiteOpt.GetExistedRebarTypeList(GeneralClass.AllRebarList);//得到list中包含的钢筋图形编号列表

                List<RebarData> _bangdata = new List<RebarData>();
                List<RebarData> _xiandata = new List<RebarData>();

                //int _threshold = (GeneralClass.m_typeC14) ? 14 : 16;//如果14为线材，则从16开始统计
                int _threshold = (GeneralClass.m_typeC12) ? 12 : ((GeneralClass.m_typeC14) ? 14 : 16);//先看12是否为棒材，再看14是否为棒材

                foreach (RebarData _dd in GeneralClass.AllRebarList)
                {
                    if (_dd.Diameter >= _threshold)
                    {
                        _bangdata.Add(_dd);
                    }
                    else
                    {
                        _xiandata.Add(_dd);
                    }
                }

                FillDGV_Pic(_bangdata, ref dataGridView4);
                FillDGV_Pic(_xiandata, ref dataGridView5);

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

                    for (int k = (int)EnumBangOrXian.BANG_C16; k < (int)EnumBangOrXian.maxRowNum; k++)//只考虑棒材，20231027
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
                                                    dataGridView6.Rows[h /*+ 3*/].Cells[k - 6].Value = Math.Round(_weight, 1);
                                                    break;
                                                }
                                            case EnumDetailItem.TOTAL_LENGTH:
                                                {
                                                    int _length = item.Value.TotalLength;
                                                    dataGridView6.Rows[h /*+ 3*/].Cells[k - 6].Value = _length / 1000;
                                                    break;
                                                }
                                            case EnumDetailItem.TOTAL_PIECE:
                                                {
                                                    dataGridView6.Rows[h /*+ 3*/].Cells[k - 6].Value = item.Value.TotalPieceNum;
                                                    break;
                                                }
                                            case EnumDetailItem.TAO_SI_NUM:
                                                {
                                                    dataGridView6.Rows[h /*+ 3*/].Cells[k - 6].Value = item.Value.TaosiNum;
                                                    break;
                                                }
                                            case EnumDetailItem.TAO_TONG_NUM:
                                                {
                                                    dataGridView6.Rows[h /*+ 3*/].Cells[k - 6].Value = item.Value.TaotongNum;
                                                    break;
                                                }
                                            case EnumDetailItem.ZHENG_SI_TAO_TONG:
                                                {
                                                    dataGridView6.Rows[h /*+ 3*/].Cells[k - 6].Value = item.Value.TaotongNum_P;
                                                    break;
                                                }
                                            case EnumDetailItem.FAN_SI_TAO_TONG:
                                                {
                                                    dataGridView6.Rows[h /*+ 3*/].Cells[k - 6].Value = item.Value.TaotongNum_N;
                                                    break;
                                                }
                                            case EnumDetailItem.BIAN_JING_TAO_TONG:
                                                {
                                                    dataGridView6.Rows[h /*+ 3*/].Cells[k - 6].Value = item.Value.TaotongNum_V;
                                                    break;
                                                }
                                            case EnumDetailItem.CUT_NUM:
                                                {
                                                    dataGridView6.Rows[h /*+ 3*/].Cells[k - 6].Value = item.Value.CutNum;
                                                    break;
                                                }
                                            case EnumDetailItem.BEND_NUM:
                                                {
                                                    dataGridView6.Rows[h /*+ 3*/].Cells[k - 6].Value = item.Value.BendNum;
                                                    break;
                                                }
                                            case EnumDetailItem.ZHI_NUM:
                                                {
                                                    dataGridView6.Rows[h /*+ 3*/].Cells[k - 6].Value = item.Value.StraightenedNum;
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
                    for (int u = (int)EnumBangOrXian.BANG_C16; u < (int)EnumBangOrXian.maxRowNum; u++)//仅棒材，20231027
                    {
                        _total = 0;
                        for (int y = 0; y < GeneralClass.ExistRebarPicTypeList.Count /*+ 3*/; y++)
                        {
                            _total += Convert.ToInt32(dataGridView6.Rows[y].Cells[u - 6].Value);
                        }
                        xData.Add(GeneralClass.sDetailTableRowName[u]);
                        yData.Add(_total);
                        ydataTotal += _total;
                        dataGridView6.Rows[GeneralClass.ExistRebarPicTypeList.Count /*+ 3*/].Cells[u - 6].Value = _total;
                    }


                    //绘制chart图表
                    chartshow.ChartRectShow("", xData, yData, chart1);

                }

            }
            catch (Exception ex) { MessageBox.Show("FillDGV6 error:" + ex.Message); }

        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                GeneralClass.interactivityData?.printlog(1, "开始按钢筋规格直径分类统计");

                GeneralClass.AllRebarList = GeneralClass.DBOpt.GetAllRebarList(GeneralClass.TableName_AllRebar);//取得所有的钢筋数据list

                List<RebarData> _banglist = GeneralClass.AllRebarList.Where(t => t.Diameter >= GeneralClass.m_threshold_BX).ToList();
                List<RebarData> _xianlist = GeneralClass.AllRebarList.Where(t => t.Diameter < GeneralClass.m_threshold_BX).ToList();

                _banglist = PickNewList(_banglist);//再经过一次钢筋形状的筛选，20240805
                _xianlist = PickNewList(_xianlist);


                FillDGV_Total(_banglist, ref dataGridView13);
                FillDGV_Total(_xianlist, ref dataGridView14);

                //汇总一起显示,20231027
                List<string> _x = new List<string>();
                List<int> _y1 = new List<int>();
                List<double> _y2 = new List<double>();

                Count_total(GeneralClass.AllRebarList, ref _x, ref _y1, ref _y2);

                for (int i = 0; i < _y2.Count; i++)
                {
                    _y2[i] = double.Parse((_y2[i] / 1000).ToString("F2"));//转成吨，两位小数
                }
                chartshow.ChartRectShow("数量(根)", _x, _y1, chart4);
                chartshow.ChartRectShow("重量(吨)", _x, _y2, chart5);



                //统计合计
                DataTable dt_z = new DataTable();

                dt_z.Columns.Add("钢筋类型", typeof(string));
                dt_z.Columns.Add("数量(根)", typeof(int));
                dt_z.Columns.Add("数量(%)", typeof(double));
                dt_z.Columns.Add("重量(吨)", typeof(double));
                dt_z.Columns.Add("重量(%)", typeof(double));

                int total_num_x, total_num_b;
                double total_weight_x, total_weight_b;
                List<string> x_name = new List<string>();
                List<int> x_num = new List<int>();
                List<double> x_weight = new List<double>();
                List<string> b_name = new List<string>();
                List<int> b_num = new List<int>();
                List<double> b_weight = new List<double>();

                Count_total(_xianlist, ref x_name, ref x_num, ref x_weight);
                Count_total(_banglist, ref b_name, ref b_num, ref b_weight);

                total_num_x = x_num.Sum(t => t);
                total_num_b = b_num.Sum(t => t);
                total_weight_x = x_weight.Sum(t => t);
                total_weight_b = b_weight.Sum(t => t);
                dt_z.Rows.Add("线材", total_num_x, (double)total_num_x / (double)(total_num_x + total_num_b), double.Parse((total_weight_x / 1000).ToString("F3")), total_weight_x / (total_weight_x + total_weight_b));
                dt_z.Rows.Add("棒材", total_num_b, (double)total_num_b / (double)(total_num_x + total_num_b), double.Parse((total_weight_b / 1000).ToString("F3")), total_weight_b / (total_weight_x + total_weight_b));

                dataGridView11.DataSource = dt_z;
                dataGridView11.Columns[2].DefaultCellStyle.Format = "P1";
                dataGridView11.Columns[3].DefaultCellStyle.Format = "0.000";
                dataGridView11.Columns[4].DefaultCellStyle.Format = "P1";

                //画饼图
                List<string> x = new List<string>() { "线材", "棒材" };
                List<int> y1 = new List<int>() { total_num_x, total_num_b };
                List<double> y2 = new List<double>() { double.Parse((total_weight_x / 1000).ToString("F3")), double.Parse((total_weight_b / 1000).ToString("F3")) };
                chartshow.ChartPieShow("总览——数量(根)", x, y1, chart2);
                chartshow.ChartPieShow("总览——重量(吨)", x, y2, chart3);

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Count_total(List<RebarData> _list, ref List<string> _dialist, ref List<int> _numlist, ref List<double> _weightlist)
        {
            _dialist.Clear();
            _numlist.Clear();
            _weightlist.Clear();

            var _grouplist = _list.GroupBy(t => t.Diameter).Select(
                y => new
                {
                    _diameter = y.Key,
                    _totalnum = y.Sum(x => x.TotalPieceNum),
                    _totalweight = y.Sum(x => x.TotalWeight),
                    _list = y.ToList()
                }).ToList().OrderBy(t => t._diameter);

            foreach (var item in _grouplist)
            {
                _dialist.Add("Φ" + item._diameter.ToString());
                _numlist.Add(item._totalnum);
                _weightlist.Add(item._totalweight);
            }
        }
        private void FillDGV_Total(List<RebarData> _list, ref DataGridView _dgv)
        {
            List<string> _dialist = new List<string>();
            List<int> _numlist = new List<int>();
            List<double> _weightlist = new List<double>();

            //先统计线材
            DataTable dt_x = new DataTable();
            dt_x.Columns.Add("直径(mm)", typeof(string));
            dt_x.Columns.Add("数量(根)", typeof(int));
            dt_x.Columns.Add("数量(%)", typeof(double));
            dt_x.Columns.Add("重量(吨)", typeof(double));
            dt_x.Columns.Add("重量(%)", typeof(double));

            int totalnum = 0;
            double totalweight = 0;

            Count_total(_list, ref _dialist, ref _numlist, ref _weightlist);

            totalnum = _numlist.Sum(t => t);
            totalweight = _weightlist.Sum(t => t);

            for (int i = 0; i < _dialist.Count; i++)
            {
                dt_x.Rows.Add(_dialist[i], _numlist[i], (double)_numlist[i] / (double)totalnum, double.Parse((_weightlist[i] / 1000).ToString("F3")), (_weightlist[i] / totalweight));
            }

            _dgv.DataSource = dt_x;
            _dgv.Columns[2].DefaultCellStyle.Format = "P1";
            _dgv.Columns[3].DefaultCellStyle.Format = "0.000";
            _dgv.Columns[4].DefaultCellStyle.Format = "P1";
        }


        private void FillDGV_Xian_detail(List<RebarData> _list, ref DataGridView _dgv)
        {
            if (_list.Count == 0)
            {
                return;
            }
            int _diameter = _list[0].Diameter;

            DataTable dt = new DataTable();
            dt.Columns.Add("直径", typeof(string));
            dt.Columns.Add("钢筋简图", typeof(Image));
            dt.Columns.Add("长度(mm)", typeof(string));
            dt.Columns.Add("数量(根)", typeof(int));
            //dt.Columns.Add("展开图", typeof(Image));
            dt.Columns.Add("边角信息", typeof(string));

            var _group = _list.GroupBy(t => new { t.PicTypeNum, t.Length, t.CornerMessage }).Select(
    y => new
    {
        _picNum = y.Key.PicTypeNum,
        _length = y.Key.Length,
        _cornermsg = y.Key.CornerMessage,

        _num = y.Sum(x => x.TotalPieceNum),
        _list = y.ToList()
    }).ToList();

            foreach (var item in _group)
            {
                List<Rebar> temp = Algorithm.ListExpand(item._list);

                if (GeneralClass.CfgData.MaterialBill == EnumMaterialBill.EJIN)//e筋料单
                {
                    dt.Rows.Add("Φ" + _diameter.ToString(), graphics.PaintRebarPic(item._list[0]), item._length, item._num, /*graphics.PaintRebarXian(temp[0]),*/ item._cornermsg);
                }
                else//广联达料单
                {
                    string newpath = GeneralClass.CfgData.GLDpath + @"\" + item._list[0].RebarPic;//广联达料单路径+图片路径即为完整路径
                    dt.Rows.Add("Φ" + _diameter.ToString(), Gld.LoadGldImage(newpath), item._length, item._num, /*graphics.PaintRebarXian(temp[0]),*/ item._cornermsg);
                }

            }

            _dgv.DataSource = dt;
            //_dgv.Columns[2].DefaultCellStyle.Format = "P1";
            //_dgv.Columns[3].DefaultCellStyle.Format = "0.0";
            //_dgv.Columns[4].DefaultCellStyle.Format = "P1";
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
            List<GroupbyDia> _grouplist = GeneralClass.DBOpt.QueryAllListByDiameter(_list);

            int[] _num = new int[(int)EnumDiameter.maxDiameterNum];
            double[] _weight = new double[(int)EnumDiameter.maxDiameterNum];

            _totalnum = 0;
            _totalweight = 0;

            foreach (var item in _grouplist)
            {
                for (EnumDiameter i = EnumDiameter.DIAMETER_6; i < EnumDiameter.maxDiameterNum; i++)
                {
                    if (item._diameter == Convert.ToInt32(i.ToString().Substring(9)))
                    {
                        _num[(int)i] = item._totalnum;
                        _weight[(int)i] = item._totalweight;
                    }
                }
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("直径", typeof(string));
            dt.Columns.Add("数量(根)", typeof(int));
            dt.Columns.Add("数量(%)", typeof(double));
            dt.Columns.Add("重量(kg)", typeof(double));
            dt.Columns.Add("重量(%)", typeof(double));

            if (GeneralClass.m_typeC12 && GeneralClass.m_typeC14)//12和14都是棒材
            {
                _totalnum = _num[0] + _num[1] + _num[2];
                _totalweight = _weight[0] + _weight[1] + _weight[2];
                dt.Rows.Add("Φ6", _num[0], (double)_num[0] / (double)_totalnum, _weight[0], (_weight[0] / _totalweight));
                dt.Rows.Add("Φ8", _num[1], (double)_num[1] / (double)_totalnum, _weight[1], (_weight[1] / _totalweight));
                dt.Rows.Add("Φ10", _num[2], (double)_num[2] / (double)_totalnum, _weight[2], (_weight[2] / _totalweight));

            }
            else if (!GeneralClass.m_typeC12 && GeneralClass.m_typeC14)//12线材，14棒材
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

                GeneralClass.AllRebarList = GeneralClass.DBOpt.GetAllRebarList(GeneralClass.TableName_AllRebar);//取得所有的钢筋数据list

                List<RebarData> _xiandata_z = new List<RebarData>();//直条
                List<RebarData> _xiandata_w = new List<RebarData>();//弯曲


                int _threshold = (GeneralClass.m_typeC12) ? 12 : ((GeneralClass.m_typeC14) ? 14 : 16);//先看12是否为棒材，再看14是否为棒材
                //_xiandata_z = GeneralClass.AllRebarList.Where(t => t.Diameter < _threshold && t.PicTypeNum == "10000").ToList();
                //_xiandata_w = GeneralClass.AllRebarList.Where(t => t.Diameter < _threshold && t.PicTypeNum != "10000").ToList();
                _xiandata_z = GeneralClass.AllRebarList.Where(t => t.Diameter < _threshold&& t.RebarShapeType!=EnumRebarShapeType.SHAPE_GJ
                &&t.RebarShapeType!=EnumRebarShapeType.SHAPE_LG).ToList();
                _xiandata_w = GeneralClass.AllRebarList.Where(t => t.Diameter < _threshold&&( t.RebarShapeType==EnumRebarShapeType.SHAPE_GJ
                ||t.RebarShapeType==EnumRebarShapeType.SHAPE_LG)).ToList();

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
                dt_hz.Rows.Add("非箍筋拉勾", total_num_z, (double)total_num_z / (double)(total_num_z + total_num_w), total_weight_z, total_weight_z / (total_weight_z + total_weight_w));
                dt_hz.Rows.Add("箍筋拉勾", total_num_w, (double)total_num_w / (double)(total_num_z + total_num_w), total_weight_w, total_weight_w / (total_weight_z + total_weight_w));

                dataGridView23.DataSource = dt_hz;
                dataGridView23.Columns[2].DefaultCellStyle.Format = "P1";
                dataGridView23.Columns[3].DefaultCellStyle.Format = "0.00";
                dataGridView23.Columns[4].DefaultCellStyle.Format = "P1";


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                GeneralClass.interactivityData?.printlog(1, "开始统计所有棒材原材");

                GeneralClass.AllRebarList = GeneralClass.DBOpt.GetAllRebarList(GeneralClass.TableName_AllRebar);//取得所有的钢筋数据list
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

            int[] _num = new int[(int)EnumDiaBang.maxRebarBangNum];                //按照14,16,18,20,22,25,28,32,36,40十种直径来统计
            double[] _weight = new double[(int)EnumDiaBang.maxRebarBangNum];
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
                                for (int i = (int)EnumDiaBang.BANG_C14; i < (int)EnumDiaBang.maxRebarBangNum; i++)
                                {
                                    if (_ddd.Diameter == GeneralClass.EnumDiameterToInt((EnumDiaBang)i)/*Convert.ToInt32(((EnumDiaBang)i).ToString().Substring(6, 2))*/)//BANG_C14
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
                                for (int i = (int)EnumDiaBang.BANG_C14; i < (int)EnumDiaBang.maxRebarBangNum; i++)
                                {
                                    if (_ddd.Diameter == GeneralClass.EnumDiameterToInt((EnumDiaBang)i) /*Convert.ToInt32(((EnumDiaBang)i).ToString().Substring(6, 2))*/)//BANG_C14
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
                                for (int i = (int)EnumDiaBang.BANG_C14; i < (int)EnumDiaBang.maxRebarBangNum; i++)
                                {
                                    if (_ddd.Diameter == GeneralClass.EnumDiameterToInt((EnumDiaBang)i)/*Convert.ToInt32(((EnumDiaBang)i).ToString().Substring(6, 2))*/)//BANG_C14
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
                                for (int i = (int)EnumDiaBang.BANG_C14; i < (int)EnumDiaBang.maxRebarBangNum; i++)
                                {
                                    if (_ddd.Diameter == GeneralClass.EnumDiameterToInt((EnumDiaBang)i) /*Convert.ToInt32(((EnumDiaBang)i).ToString().Substring(6, 2))*/)//BANG_C14
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
            for (int i = (int)EnumDiaBang.BANG_C14; i < (int)EnumDiaBang.maxRebarBangNum; i++)
            {
                total_num += _num[i];
                total_weight += _weight[i];
            }
            for (int i = (int)EnumDiaBang.BANG_C14; i < (int)EnumDiaBang.maxRebarBangNum; i++)
            {
                dt_z.Rows.Add("Φ" + GeneralClass.EnumDiameterToInt((EnumDiaBang)i)  /*((EnumDiaBang)i).ToString().Substring(6, 2)*/, _num[i], (double)_num[i] / (double)total_num, _weight[i], _weight[i] / total_weight);
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

            List<GroupbyTaoBendDatalist> _typelist = GeneralClass.DBOpt.QueryAllListByTaoBend(_sonlist);

            List<GroupbyDia> _diameterlist = null;
            int totalnum = 0;
            double totalweight = 0;

            foreach (var item in _typelist)
            {
                if (!item._ifbend && !item._iftao)//不弯不套
                {
                    _diameterlist = GeneralClass.DBOpt.QueryAllListByDiameter(item._datalist);

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
                    _diameterlist = GeneralClass.DBOpt.QueryAllListByDiameter(item._datalist);

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
                    _diameterlist = GeneralClass.DBOpt.QueryAllListByDiameter(item._datalist);

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
                    _diameterlist = GeneralClass.DBOpt.QueryAllListByDiameter(item._datalist);

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
        /// <summary>
        /// 按照棒材的不同加工工艺分类汇总
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="_worktype"></param>
        /// <param name="_dgv"></param>
        private void FillDGV_Bang_type(List<RebarData> _list, EnumWorkType _worktype, ref DataGridView _dgv/*, out int total_num, out double total_weight*/)
        {

            int[] _num = new int[(int)EnumDiaBang.maxRebarBangNum];                //按照14,16,18,20,22,25,28,32,36,40十种直径来统计
            double[] _weight = new double[(int)EnumDiaBang.maxRebarBangNum];
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
                                for (int i = (int)EnumDiaBang.BANG_C14; i < (int)EnumDiaBang.maxRebarBangNum; i++)
                                {
                                    if (_ddd.Diameter == Convert.ToInt32(((EnumDiaBang)i).ToString().Substring(6, 2)))//BANG_C14
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
                                for (int i = (int)EnumDiaBang.BANG_C14; i < (int)EnumDiaBang.maxRebarBangNum; i++)
                                {
                                    if (_ddd.Diameter == Convert.ToInt32(((EnumDiaBang)i).ToString().Substring(6, 2)))//BANG_C14
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
                                for (int i = (int)EnumDiaBang.BANG_C14; i < (int)EnumDiaBang.maxRebarBangNum; i++)
                                {
                                    if (_ddd.Diameter == Convert.ToInt32(((EnumDiaBang)i).ToString().Substring(6, 2)))//BANG_C14
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
                                for (int i = (int)EnumDiaBang.BANG_C14; i < (int)EnumDiaBang.maxRebarBangNum; i++)
                                {
                                    if (_ddd.Diameter == Convert.ToInt32(((EnumDiaBang)i).ToString().Substring(6, 2)))//BANG_C14
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
            for (int i = (int)EnumDiaBang.BANG_C14; i < (int)EnumDiaBang.maxRebarBangNum; i++)
            {
                total_num += _num[i];
                total_weight += _weight[i];
            }
            for (int i = (int)EnumDiaBang.BANG_C14; i < (int)EnumDiaBang.maxRebarBangNum; i++)
            {
                dt_z.Rows.Add("Φ" + ((EnumDiaBang)i).ToString().Substring(6, 2), _num[i], (double)_num[i] / (double)total_num, _weight[i], _weight[i] / total_weight);
            }

            _dgv.DataSource = dt_z;
            _dgv.Columns[2].DefaultCellStyle.Format = "P1";         //百分比
            _dgv.Columns[3].DefaultCellStyle.Format = "0.0";        //double保留两位小数
            _dgv.Columns[4].DefaultCellStyle.Format = "P1";         //百分比
        }
        /// <summary>
        /// 筛选钢筋长度类型，原长、非整长、整长
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_sonORmum"></param>
        /// <returns></returns>
        private bool CheckLengthType(RebarData _data, bool _sonORmum)
        {
            bool _return;
            if (!_sonORmum)//区分分子分母，false分子，true分母
            {
                _return = (checkBox19.Checked ? (_data.IsOriginal) : false) ||
                        (checkBox20.Checked ? (!_data.IsOriginal
                     && (_data.IfBend
                         || (!_data.IfBend
                             && _data.Length != "3000"
                             && _data.Length != "4000"
                             && _data.Length != "5000"
                             && _data.Length != "6000"
                             && _data.Length != "7000"))
                     ) : false) ||
                        (checkBox37.Checked ? (!_data.IsOriginal
                     && (!_data.IfBend)
                     && (_data.Length == "3000"
                             || _data.Length == "4000"
                             || _data.Length == "5000"
                             || _data.Length == "6000"
                             || _data.Length == "7000")
                     ) : false);

            }
            else
            {
                _return = (checkBox22.Checked ? (_data.IsOriginal) : false) ||
                        (checkBox21.Checked ? (!_data.IsOriginal
                    && (_data.IfBend
                        || (!_data.IfBend
                            && _data.Length != "3000"
                            && _data.Length != "4000"
                            && _data.Length != "5000"
                            && _data.Length != "6000"
                            && _data.Length != "7000"))
                    ) : false) ||
                        (checkBox38.Checked ? (!_data.IsOriginal
                    && (!_data.IfBend)
                    && (_data.Length == "3000"
                            || _data.Length == "4000"
                            || _data.Length == "5000"
                            || _data.Length == "6000"
                            || _data.Length == "7000")
                    ) : false);
            }
            return _return;
        }
        /// <summary>
        /// 工艺类型，是否弯曲，是否套丝，_sonORmum区分是分子/分母
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_sonORmum"></param>
        /// <returns></returns>
        private bool CheckWorkType(RebarData _data, bool _sonORmum)
        {
            bool _rt;
            if (!_sonORmum)
            {
                _rt = (
                            (checkBox2.Checked ? (_data.IfTao) : false) ||
                            (checkBox3.Checked ? (!_data.IfTao) : false)
                        )
                        &&
                        (
                            (checkBox16.Checked ? (_data.IfBend) : false) ||
                            (checkBox17.Checked ? (!_data.IfBend) : false)
                        );
            }
            else
            {
                _rt = (
                                (checkBox25.Checked ? (_data.IfTao) : false) ||
                                (checkBox26.Checked ? (!_data.IfTao) : false)
                            )
                            &&
                            (
                                (checkBox24.Checked ? (_data.IfBend) : false) ||
                                (checkBox23.Checked ? (!_data.IfBend) : false)
                            );
            }
            return _rt;

        }
        /// <summary>
        /// 判断直径选择是否处于单选中状态，只考虑分子
        /// </summary>
        /// <returns></returns>
        private bool CheckDiameterSinglePick()
        {
            int _count = 0;
            if (checkBox68.Checked) { _count++; }
            if (checkBox8.Checked) { _count++; }
            if (checkBox9.Checked) { _count++; }
            if (checkBox10.Checked) { _count++; }
            if (checkBox11.Checked) { _count++; }
            if (checkBox12.Checked) { _count++; }
            if (checkBox13.Checked) { _count++; }
            if (checkBox14.Checked) { _count++; }
            if (checkBox15.Checked) { _count++; }
            if (checkBox18.Checked) { _count++; }
            if (checkBox39.Checked) { _count++; }

            if (_count == 1)
                return true;
            else
                return false;

        }
        /// <summary>
        /// 获取直径选择中处于选中的是哪种直径，前提必须只选中一种直径
        /// </summary>
        /// <returns></returns>
        private EnumDiaBang CheckDiameterWhichPick()
        {
            if (!CheckDiameterSinglePick())
            {
                return EnumDiaBang.NONE;
            }
            if (checkBox68.Checked) { return EnumDiaBang.BANG_C12; }
            if (checkBox8.Checked) { return EnumDiaBang.BANG_C14; }
            if (checkBox9.Checked) { return EnumDiaBang.BANG_C16; }
            if (checkBox10.Checked) { return EnumDiaBang.BANG_C18; }
            if (checkBox11.Checked) { return EnumDiaBang.BANG_C20; }
            if (checkBox12.Checked) { return EnumDiaBang.BANG_C22; }
            if (checkBox13.Checked) { return EnumDiaBang.BANG_C25; }
            if (checkBox14.Checked) { return EnumDiaBang.BANG_C28; }
            if (checkBox15.Checked) { return EnumDiaBang.BANG_C32; }
            if (checkBox18.Checked) { return EnumDiaBang.BANG_C36; }
            if (checkBox39.Checked) { return EnumDiaBang.BANG_C40; }

            return EnumDiaBang.NONE;
        }
        /// <summary>
        /// 根据勾选项选择哪些直径，_sonORmum用以区分是分子/分母
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_sonORmum"></param>
        /// <returns></returns>
        private bool CheckDiameter(RebarData _data, bool _sonORmum)
        {
            bool _rt;
            if (!_sonORmum)
            {
                _rt = ((checkBox68.Checked && GeneralClass.m_typeC12) ? (_data.Diameter == 12) : false) ||
                    ((checkBox8.Checked && GeneralClass.m_typeC14) ? (_data.Diameter == 14) : false) ||
                                (checkBox9.Checked ? (_data.Diameter == 16) : false) ||
                                (checkBox10.Checked ? (_data.Diameter == 18) : false) ||
                                (checkBox11.Checked ? (_data.Diameter == 20) : false) ||
                                (checkBox12.Checked ? (_data.Diameter == 22) : false) ||
                                (checkBox13.Checked ? (_data.Diameter == 25) : false) ||
                                (checkBox14.Checked ? (_data.Diameter == 28) : false) ||
                                (checkBox15.Checked ? (_data.Diameter == 32) : false) ||
                                (checkBox18.Checked ? (_data.Diameter == 36) : false) ||
                                (checkBox39.Checked ? (_data.Diameter == 40) : false);
            }
            else
            {
                _rt = ((checkBox69.Checked && GeneralClass.m_typeC12) ? (_data.Diameter == 12) : false) ||
                     ((checkBox35.Checked && GeneralClass.m_typeC14) ? (_data.Diameter == 14) : false) ||
                                (checkBox34.Checked ? (_data.Diameter == 16) : false) ||
                                (checkBox33.Checked ? (_data.Diameter == 18) : false) ||
                                (checkBox32.Checked ? (_data.Diameter == 20) : false) ||
                                (checkBox31.Checked ? (_data.Diameter == 22) : false) ||
                                (checkBox30.Checked ? (_data.Diameter == 25) : false) ||
                                (checkBox29.Checked ? (_data.Diameter == 28) : false) ||
                                (checkBox28.Checked ? (_data.Diameter == 32) : false) ||
                                (checkBox27.Checked ? (_data.Diameter == 36) : false) ||
                                (checkBox40.Checked ? (_data.Diameter == 40) : false);
            }
            return _rt;
        }
        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                GeneralClass.interactivityData?.printlog(1, "开始统计所有棒材");

                GeneralClass.m_MaterialPool.Clear();//清空原材库
                GeneralClass.AddDefaultMaterial();//添加默认的原材库

                GeneralClass.AllRebarList = GeneralClass.DBOpt.GetAllRebarList(GeneralClass.TableName_AllRebar);//取得所有的钢筋数据list

                List<RebarData> _sonData = new List<RebarData>();//分子
                List<RebarData> _mumData = new List<RebarData>();   //分母

                //int _threshold = (GeneralClass.m_typeC14) ? 14 : 16;//如果14为线材，则从16开始统计
                // _threshold = (GeneralClass.m_typeC12) ? 12 : 14;//如果12为线材，则从14开始统计
                int _threshold = (GeneralClass.m_typeC12) ? 12 : ((GeneralClass.m_typeC14) ? 14 : 16);//先看12是否为棒材，再看14是否为棒材

                var _pickedList = PickNewList(GeneralClass.AllRebarList);//先经过棒材/线材、箍筋/拉勾/马凳/主筋的筛选
                foreach (RebarData _dd in _pickedList)
                {
                    //int _threshold = (GeneralClass.m_typeC14) ? 14 : 16;//如果14为线材，则从16开始统计
                    if (_dd.Diameter >= _threshold && _dd.TotalPieceNum != 0
                            && CheckLengthType(_dd, false)
                            && CheckWorkType(_dd, false)
                            && CheckDiameter(_dd, false))
                    {
                        _sonData.Add(_dd);
                    }
                }

                foreach (RebarData _dd in _pickedList)
                {
                    //int _threshold = (GeneralClass.m_typeC14) ? 14 : 16;//如果14为线材，则从16开始统计
                    if (_dd.Diameter >= _threshold && _dd.TotalPieceNum != 0
                            && CheckLengthType(_dd, true)
                            && CheckWorkType(_dd, true)
                            && CheckDiameter(_dd, true))
                    {
                        _mumData.Add(_dd);
                    }
                }
                //棒材图形统计
                FillDGV_Pic(_sonData, ref dataGridView4);

                //工艺类型统计
                FillDGV_Bang_Worktype(_sonData, _mumData);
                //长度
                FillDGV_Length(_sonData, _mumData);
                //长度区间
                FillDGV_LengthArea(_sonData, _mumData);
                //直径规格
                FillDGV_Bang_Diameter(_sonData, _mumData);

                //不同工艺统计
                FillDGV_Bang_type(_sonData, EnumWorkType.NO_BEND_NO_TAO, ref dataGridView19);
                FillDGV_Bang_type(_sonData, EnumWorkType.NO_BEND_YES_TAO, ref dataGridView20);
                FillDGV_Bang_type(_sonData, EnumWorkType.YES_BEND_NO_TAO, ref dataGridView21);
                FillDGV_Bang_type(_sonData, EnumWorkType.YES_BEND_YES_TAO, ref dataGridView22);

                //弯曲角度统计
                FillDGV_Bend(_sonData, ref dataGridView30);

                //套丝图形统计
                FillDGV_Taosi_Pic(_sonData, ref dataGridView31);
                //套丝类型统计
                FillDGV_Taosi(_sonData, ref dataGridView32);


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        /// <summary>
        /// 按照直径规格汇总
        /// </summary>
        /// <param name="_sonlist"></param>
        /// <param name="_mumlist"></param>
        private void FillDGV_Bang_Diameter(List<RebarData> _sonlist, List<RebarData> _mumlist)
        {
            List<GroupbyDia> _songrouplist = GeneralClass.DBOpt.QueryAllListByDiameter(_sonlist);
            List<GroupbyDia> _mumgrouplist = GeneralClass.DBOpt.QueryAllListByDiameter(_mumlist);

            DataTable dt_z = new DataTable();
            dt_z.Columns.Add("直径", typeof(string));
            dt_z.Columns.Add("数量", typeof(int));
            dt_z.Columns.Add("数量(%)", typeof(double));
            dt_z.Columns.Add("重量(kg)", typeof(double));
            dt_z.Columns.Add("重量(%)", typeof(double));

            //_list已经过滤过，棒材、非原材、非多段
            EnumDiaBang _start = (GeneralClass.m_typeC12) ? EnumDiaBang.BANG_C12 :
                ((GeneralClass.m_typeC14) ? EnumDiaBang.BANG_C14 : EnumDiaBang.BANG_C16);//先看12是否为棒材，再看14是否为棒材

            int[] _num = new int[(int)EnumDiaBang.maxRebarBangNum];
            double[] _weight = new double[(int)EnumDiaBang.maxRebarBangNum];
            int total_num = 0;
            double total_weight = 0;

            for (int i = (int)_start; i < (int)EnumDiaBang.maxRebarBangNum; i++)
            {
                foreach (var item in _songrouplist)
                {
                    if (item._diameter == Convert.ToInt32(((EnumDiaBang)i).ToString().Substring(6, 2)))
                    {
                        _num[i] = item._totalnum;
                        _weight[i] = item._totalweight;
                        break;
                    }
                }
                foreach (var item in _mumgrouplist)
                {
                    if (item._diameter == Convert.ToInt32(((EnumDiaBang)i).ToString().Substring(6, 2)))
                    {
                        total_num += item._totalnum;
                        total_weight += item._totalweight;
                    }
                }
            }
            for (int i = (int)_start; i < (int)EnumDiaBang.maxRebarBangNum; i++)
            {
                dt_z.Rows.Add("Φ" + ((EnumDiaBang)i).ToString().Substring(6, 2), _num[i], (double)_num[i] / (double)total_num, _weight[i], _weight[i] / total_weight);
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
            List<GroupbyTaoBendDatalist> _songrouplist = GeneralClass.DBOpt.QueryAllListByTaoBend(_sonlist);
            List<GroupbyTaoBendDatalist> _mumgrouplist = GeneralClass.DBOpt.QueryAllListByTaoBend(_mumlist);

            DataTable dt = new DataTable();

            dt.Columns.Add("类型", typeof(string));
            dt.Columns.Add("数量(根)", typeof(int));
            dt.Columns.Add("数量(%)", typeof(double));
            dt.Columns.Add("重量(kg)", typeof(double));
            dt.Columns.Add("重量(%)", typeof(double));

            List<string> x = new List<string>() { "不弯不套", "不弯套丝", "弯曲套丝", "弯曲不套" };
            int[] total_num = new int[4] { 0, 0, 0, 0 };
            double[] total_weight = new double[4] { 0, 0, 0, 0 };

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
                else if (item._iftao && item._ifbend)
                {
                    total_num[2] = item._totalnum;
                    total_weight[2] = item._totalweight;
                }
                else if (!item._iftao && item._ifbend)
                {
                    total_num[3] = item._totalnum;
                    total_weight[3] = item._totalweight;
                }
            }

            int all_num = 0;
            double all_weight = 0;

            foreach (GroupbyTaoBendDatalist item in _mumgrouplist)//使用分母list的统计数据作为分母
            {
                all_num += item._totalnum;
                all_weight += item._totalweight;
            }

            for (int i = 0; i < 4; i++)
            {
                dt.Rows.Add(x[i], total_num[i], (double)total_num[i] / (double)all_num, total_weight[i], total_weight[i] / all_weight);
            }
            dt.Rows.Add("总计", total_num.Sum(), (double)(total_num.Sum()) / (double)all_num, total_weight.Sum(), total_weight.Sum() / all_weight);

            dataGridView25.DataSource = dt;
            dataGridView25.Columns[2].DefaultCellStyle.Format = "P1";
            dataGridView25.Columns[3].DefaultCellStyle.Format = "0.00";
            dataGridView25.Columns[4].DefaultCellStyle.Format = "P1";

            chartshow.ChartPieShow("工艺统计——数量", x, total_num.ToList(), chart8);
            chartshow.ChartPieShow("工艺统计——重量", x, total_weight.ToList(), chart9);
            //ChartRectShow("工艺统计——数量", x, total_num, chart8);
            //ChartRectShow("工艺统计——重量", x, total_weight, chart9);


        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                GeneralClass.interactivityData?.printlog(1, "开始进行料单的详细分析");

                GeneralClass.AllRebarList = GeneralClass.DBOpt.GetAllRebarList(GeneralClass.TableName_AllRebar);

                //GeneralClass.AllDetailData = GeneralClass.SQLiteOpt.DetailAnalysis(GeneralClass.AllRebarList);

                GeneralClass.AllDetailOtherData = GeneralClass.DBOpt.DetailAnalysis2(GeneralClass.AllRebarList, checkBox7.Checked, checkBox4.Checked, checkBox5.Checked, checkBox6.Checked);

                GeneralClass.interactivityData?.printlog(1, "料单详细分析完成，请选择统计项");

                GeneralClass.ExistRebarPicTypeList = GeneralClass.DBOpt.GetExistedRebarTypeList(GeneralClass.AllRebarList);//得到列表中包含的钢筋图形编号列表


                dataGridView6.Rows.Clear();
                //重新添加行
                string sType = "";
                int _index = -1;

                DataGridViewRow row = new DataGridViewRow();


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
                    else
                    {
                        GeneralClass.interactivityData?.printlog(1, "钢筋图片库中找不到编号为:" + sType + "的图片，请手动添加");

                        if (GeneralClass.m_showNoFindPic)
                        {
                            FindIndexInImagelist("00000", out _index);//临时用00000.png代替
                            dataGridView6.Rows[i].Cells[0].Value = imageList1.Images[_index];
                        }
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
            List<RebarData> _allList_bk = GeneralClass.DBOpt.GetAllRebarList(GeneralClass.TableName_AllRebar);

            //查询所有被选中的钢筋
            List<RebarData> _newlist = new List<RebarData>();
            foreach (RebarData _data in _allList_bk)
            {
                //if (_data.ProjectName == _projectname && _data.MainAssemblyName == _assemblyname)
                //{
                //    _newlist.Add(_data);
                //}
                if (_data.TableName == _projectname && _data.TableSheetName == _assemblyname)//20240923修改，原项目名和主构件名修改为料表名和料表sheet名
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
        /// 其他界面也可以使用此方法，列名：子构件名、图形编号、钢筋级别、直径、简图、边角信息、下料长度、根数件数、总根数、总重量、备注、
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="_dgv"></param>
        public static void FillDGVWithRebarList(List<RebarData> _list, DataGridView _dgv)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.ELEMENT_NAME], typeof(string));
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PIC_NO], typeof(string));
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.LEVEL], typeof(string));
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.DIAMETER], typeof(int));
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.REBAR_PIC], typeof(Image));
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.CORNER_MESSAGE], typeof(string));
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.LENGTH], typeof(string));
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PIECE_NUM_UNIT_NUM], typeof(string));
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TOTAL_PIECE_NUM], typeof(string));
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TOTAL_WEIGHT], typeof(string));
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.DESCRIPTION], typeof(string));

            foreach (var item in _list)
            {
                string sType = item.PicTypeNum;
                if (sType == "") continue;  //类型为空，跳过加载图片
                object _image = null;

                if (GeneralClass.CfgData.MaterialBill == EnumMaterialBill.EJIN)//e筋料单
                {
                    if (GeneralClass.interactivityData.ifFindImage(sType, out _image))
                    {
                        _image = graphics.PaintRebarPic(item);//直接画钢筋简图
                    }
                    else
                    {
                        GeneralClass.interactivityData?.printlog(1, "钢筋图片库中找不到编号为:" + sType + "的图片，请手动添加");
                        if (GeneralClass.m_showNoFindPic)
                        {
                            GeneralClass.interactivityData.ifFindImage("00000", out _image);//临时用00000.png代替
                        }
                    }
                }
                else//广联达料单
                {
                    string newpath = GeneralClass.CfgData.GLDpath + @"\" + item.RebarPic;//广联达料单路径+图片路径即为完整路径
                    _image = Gld.LoadGldImage(newpath);
                }

                dt.Rows.Add(item.ElementName,
                    item.PicTypeNum,
                    item.Level,
                    item.Diameter,
                    _image,
                    item.CornerMessage,
                    item.Length,
                    item.PieceNumUnitNum,
                    item.TotalPieceNum,
                    item.TotalWeight,
                    item.Description
                    );
            }

            _dgv.DataSource = dt;
            //_dgv.Columns[2].DefaultCellStyle.Format = "P1";
            _dgv.Columns[9].DefaultCellStyle.Format = "0.00";
            //_dgv.Columns[4].DefaultCellStyle.Format = "P1";

            #region backup


            //_dgv.Rows.Clear();//清空
            //DataGridViewRow dgvRow;
            //DataGridViewCell dgvCell;
            //DataGridViewImageCell dgvImageCell;

            //foreach (RebarData _dd in _list)
            //{
            //    dgvRow = new DataGridViewRow();

            //    //1子构件名
            //    dgvCell = new DataGridViewTextBoxCell();
            //    dgvCell.Value = _dd.ElementName;
            //    dgvRow.Cells.Add(dgvCell);

            //    //2图形编号
            //    dgvCell = new DataGridViewTextBoxCell();
            //    dgvCell.Value = _dd.PicTypeNum;
            //    dgvRow.Cells.Add(dgvCell);

            //    //3级别
            //    dgvCell = new DataGridViewTextBoxCell();
            //    dgvCell.Value = _dd.Level;
            //    dgvRow.Cells.Add(dgvCell);

            //    //4直径
            //    dgvCell = new DataGridViewTextBoxCell();
            //    dgvCell.Value = _dd.Diameter;
            //    dgvRow.Cells.Add(dgvCell);

            //    //5钢筋简图
            //    dgvImageCell = new DataGridViewImageCell();
            //    string sType = _dd.PicTypeNum;
            //    if (sType == "") continue;  //类型为空，跳过加载图片
            //    object _image = null;
            //    if (GeneralClass.interactivityData.ifFindImage(sType, out _image))
            //    {
            //        dgvImageCell.Value = _image;
            //    }
            //    else
            //    {
            //        GeneralClass.interactivityData?.printlog(1, "钢筋图片库中找不到编号为:" + sType + "的图片，请手动添加");
            //        if (GeneralClass.m_showNoFindPic)
            //        {
            //            GeneralClass.interactivityData.ifFindImage("00000", out _image);//临时用00000.png代替
            //            dgvImageCell.Value = _image;
            //        }
            //    }
            //    dgvRow.Cells.Add(dgvImageCell);


            //    ////6图片信息
            //    //dgvCell = new DataGridViewTextBoxCell();
            //    //dgvCell.Value = _dd.PicMessage;
            //    //dgvRow.Cells.Add(dgvCell);

            //    //7边角结构
            //    dgvCell = new DataGridViewTextBoxCell();
            //    dgvCell.Value = _dd.CornerMessage;
            //    dgvRow.Cells.Add(dgvCell);

            //    //8下料长度
            //    dgvCell = new DataGridViewTextBoxCell();
            //    dgvCell.Value = _dd.Length;
            //    dgvRow.Cells.Add(dgvCell);

            //    ////9是否多段
            //    //dgvCell = new DataGridViewTextBoxCell();
            //    //dgvCell.Value = _dd.IsMulti;
            //    //dgvRow.Cells.Add(dgvCell);

            //    //10根数件数
            //    dgvCell = new DataGridViewTextBoxCell();
            //    dgvCell.Value = _dd.PieceNumUnitNum;
            //    dgvRow.Cells.Add(dgvCell);

            //    //11总根数
            //    dgvCell = new DataGridViewTextBoxCell();
            //    dgvCell.Value = _dd.TotalPieceNum;
            //    dgvRow.Cells.Add(dgvCell);

            //    //12总重量
            //    dgvCell = new DataGridViewTextBoxCell();
            //    dgvCell.Value = _dd.TotalWeight.ToString("0.00");
            //    dgvRow.Cells.Add(dgvCell);



            //    //13备注
            //    dgvCell = new DataGridViewTextBoxCell();
            //    dgvCell.Value = _dd.Description;
            //    dgvRow.Cells.Add(dgvCell);

            //    ////14标注序号
            //    //dgvCell = new DataGridViewTextBoxCell();
            //    //dgvCell.Value = _dd.SerialNum;
            //    //dgvRow.Cells.Add(dgvCell);

            //    ////15是否原材
            //    //dgvCell = new DataGridViewTextBoxCell();
            //    //dgvCell.Value = _dd.IsOriginal;
            //    //dgvRow.Cells.Add(dgvCell);

            //    ////16是否套丝
            //    //dgvCell = new DataGridViewTextBoxCell();
            //    //dgvCell.Value = _dd.IfTao;
            //    //dgvRow.Cells.Add(dgvCell);

            //    ////17是否弯曲
            //    //dgvCell = new DataGridViewTextBoxCell();
            //    //dgvCell.Value = _dd.IfBend;
            //    //dgvRow.Cells.Add(dgvCell);

            //    ////18是否切断
            //    //dgvCell = new DataGridViewTextBoxCell();
            //    //dgvCell.Value = _dd.IfCut;
            //    //dgvRow.Cells.Add(dgvCell);

            //    ////19是否弯曲两次以上
            //    //dgvCell = new DataGridViewTextBoxCell();
            //    //dgvCell.Value = _dd.IfBendTwice;
            //    //dgvRow.Cells.Add(dgvCell);

            //    _dgv.Rows.Add(dgvRow);

            //}
            #endregion
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



        //private void button1_Click(object sender, EventArgs e)
        //{
        //    panel2.Visible = !panel2.Visible;
        //    button1.BackColor = panel2.Visible ? Color.Wheat : SystemColors.GradientInactiveCaption;
        //}

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    panel3.Visible = !panel3.Visible;
        //    button2.BackColor = panel3.Visible ? Color.Wheat : SystemColors.GradientInactiveCaption;

        //}

        //private void button6_Click(object sender, EventArgs e)
        //{
        //    panel4.Visible = !panel4.Visible;
        //    button6.BackColor = panel4.Visible ? Color.Wheat : SystemColors.GradientInactiveCaption;

        //}

        //private void button11_Click(object sender, EventArgs e)
        //{
        //    panel5.Visible = !panel5.Visible;
        //    button11.BackColor = panel5.Visible ? Color.Wheat : SystemColors.GradientInactiveCaption;

        //}
        //private void button13_Click(object sender, EventArgs e)
        //{
        //    //panel6.Visible = !panel6.Visible;
        //    //button13.BackColor = panel6.Visible ? Color.Wheat :SystemColors.GradientInactiveCaption;
        //}

        /// <summary>
        /// 批量锯切，按照单一直径的挑选长度进入原材库
        /// </summary>
        private void FillDGV_MaterialPoolSingleDia(DataTable dt)
        {
            dataGridView33.DataSource = dt;
            dataGridView33.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView33.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }
        /// <summary>
        /// 批量锯切，汇总所有直径的已挑选长度形成原材库
        /// </summary>
        private void FillDGV_MaterialPoolAllDia(DataTable dt)
        {
            dataGridView34.DataSource = dt;
            dataGridView34.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView34.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

        }

        /// <summary>
        /// 棒材的批量锯切统计，主要是按照长度统计
        /// </summary>
        /// <param name="_sonlist"></param>
        private void FillDGV_Length(List<RebarData> _sonlist, List<RebarData> _mumlist)
        {

            var _sonalllist = Algorithm.ListExpand(_sonlist);
            var _mumalllist = Algorithm.ListExpand(_mumlist);

            FillDGV_Length(_sonalllist, _mumalllist);

            //DataTable dt = new DataTable();

            ////dt.Columns.Add("", System.Type.GetType("System.Boolean"));
            //dt.Columns.Add(" ", typeof(bool));
            //dt.Columns.Add("长度(mm)", typeof(int));
            //dt.Columns.Add("数量(根)", typeof(int));
            //dt.Columns.Add("数量(%)", typeof(double));
            //dt.Columns.Add("重量(kg)", typeof(double));
            //dt.Columns.Add("重量(%)", typeof(double));

            //List<GroupbyLengthDatalist> _sonalllist = GeneralClass.SQLiteOpt.QueryAllListByLength(_sonlist);
            //List<GroupbyLengthDatalist> _mumalllist = GeneralClass.SQLiteOpt.QueryAllListByLength(_mumlist);

            //double total_num = 0;
            //double total_weight = 0.0;

            //foreach (var item in _mumalllist)//使用分母list的统计数据，作为分母
            //{
            //    total_num += item._totalnum;
            //    total_weight += item._totalweight;
            //}
            //int ilength = 0;
            //foreach (var item in _sonalllist)
            //{
            //    if (!int.TryParse(item._length, out ilength))//针对缩尺~
            //    {
            //        string[] tt = item._length.Split('~');
            //        ilength = (Convert.ToInt32(tt[0]) + Convert.ToInt32(tt[1])) / 2;
            //    }

            //    dt.Rows.Add(false, ilength, item._totalnum, (double)item._totalnum / total_num, item._totalweight, item._totalweight / total_weight);
            //}

            //dataGridView2.DataSource = dt;
            ////dataGridView2.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dataGridView2.Columns[3].DefaultCellStyle.Format = "P2";
            //dataGridView2.Columns[4].DefaultCellStyle.Format = "0.00";
            //dataGridView2.Columns[5].DefaultCellStyle.Format = "P2";

        }

        /// <summary>
        /// 显示批量锯切套料效果图
        /// </summary>
        private void FillDGV_Pi_Tao_show(List<RebarOri> _list)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("序号", typeof(int));
            dt.Columns.Add("钢筋原材分段", typeof(Image));

            foreach (var item in _list)
            {
                int _index = _list.IndexOf(item);
                dt.Rows.Add(_index, graphics.PaintRebar(item));
            }
            dataGridView37.DataSource = dt;
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

                //注意此处的泛型委托func写法，func<输入类型，输出类型>
                Func<List<Rebar>, string> msglist = x =>
                {
                    string sss = "";
                    foreach (var ttt in x)
                    {
                        sss += ttt.CornerMessage;//拼接cornerMessage
                    }
                    return sss;
                };

                //此处使用func委托类型创建一个复杂的键选择器，目的是根据rebarOri中所有rebar边角信息的拼接来进行筛选
                var temp = _list.GroupBy(p => new { cornerMsgkey = msglist(p._list) }).Select(
                                y => new
                                {
                                    cornerMsgList = y.Key,
                                    num = y.Count(),
                                    datalist = y.ToList()
                                }
                                ).ToList();

                foreach (var item in temp)
                {
                    var str = item.cornerMsgList;
                    //int _index = temp.IndexOf(item);
                    //dt.Rows.Add(_index, item.num, graphics.PaintRebar(item.datalist[0]));
                    dt.Rows.Add(item.num, graphics.PaintRebar(item.datalist[0]));

                }


                //foreach (var item in _list)
                //{
                //    int _index = _list.IndexOf(item);
                //    dt.Rows.Add(_index, graphics.PaintRebar(item));
                //}
                dataGridView38.DataSource = dt;

            }
            else
            {
                DataTable dt = new DataTable();

                //dt.Columns.Add("序号", typeof(int));
                //dt.Columns.Add("数量", typeof(int));
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

                //foreach (var item in temp)
                //{
                //    var str = item.cornerMsgList;
                //    int _index = temp.IndexOf(item);
                //    dt.Rows.Add(_index, item.num, graphics.PaintRebar(item.datalist[0]));
                //}


                foreach (var item in _list)
                {
                    //int _index = _list.IndexOf(item);
                    //dt.Rows.Add(_index, graphics.PaintRebar(item));
                    dt.Rows.Add(graphics.PaintRebar(item));

                }
                dataGridView38.DataSource = dt;

            }

        }
        /// <summary>
        /// 棒材的长度区间分类，0米~12米按照500一个区间共计25个长度区间，
        /// </summary>
        /// <param name="_sonlist"></param>
        private void FillDGV_LengthArea(List<RebarData> _sonlist, List<RebarData> _mumlist)
        {
            //按照长度12个区间进行统计
            DataTable dt = new DataTable();
            dt.Columns.Add(" ", typeof(bool));
            dt.Columns.Add("长度区间(m)", typeof(string));
            dt.Columns.Add("数量(根)", typeof(int));
            dt.Columns.Add("数量(%)", typeof(double));
            dt.Columns.Add("重量(吨)", typeof(double));
            dt.Columns.Add("重量(%)", typeof(double));

            List<GroupbyLengthDatalist> _sonalllist = GeneralClass.DBOpt.QueryAllListByLength(_sonlist);
            List<GroupbyLengthDatalist> _mumalllist = GeneralClass.DBOpt.QueryAllListByLength(_mumlist);


            #region 按照原材库的长度区间来分

            //List<Rebar> _sonRebarlist = Algorithm.ListExpand(_sonlist);//先把所有的rebardata拆开，成为单根rebar

            //List<string> x = new List<string>();
            //List<int> y1 = new List<int>();
            //List<double> y2 = new List<double>();

            //int total_num_bend = _mumlist.Sum(t => t.TotalPieceNum);
            //double total_weight_bend = _mumlist.Sum(t => t.TotalWeight);

            //List<int> area_totalnum = new List<int>();
            //List<double> area_totalweight = new List<double>();

            ////添加默认原材库时，所有直径都已添加，随便选一个直径即可
            //List<GeneralMaterial> _material = GeneralClass.m_MaterialPool.Where(t => t._diameter == EnumDiameterBang.BANG_C16).ToList().OrderBy(k => k._length).ToList();

            //for (int i = 0; i < _material.Count; i++)
            //{
            //    //筛选在原材区间的
            //    var temp = _sonRebarlist.Where(t => t.length > ((i != 0) ? (_material[i - 1]._length) : 0) && t.length <= _material[i]._length).ToList();
            //    area_totalnum .Add(temp.Count);
            //    area_totalweight .Add(temp.Sum(t=>t.weight));

            //    double t0 =(double)( (i != 0) ? (_material[i - 1]._length) : 0)/1000;
            //    double t1 = (double)(_material[i]._length) / 1000;

            //    dt.Rows.Add(false, t0.ToString() + "~" + t1.ToString(), area_totalnum[i], (double)area_totalnum[i] / (double)total_num_bend, Convert.ToDouble((area_totalweight[i] / 1000).ToString("F1")), area_totalweight[i] / total_weight_bend);

            //    x.Add(t0.ToString() + "~" + t1.ToString());
            //    y1.Add(area_totalnum[i]);
            //    y2.Add(Convert.ToDouble((area_totalweight[i] / 1000).ToString("F1")));

            //}

            //var ttt = _sonRebarlist.Where(t => t.length >  _material.Last()._length).ToList();//还有大于最大原材的一个范围区间，
            //area_totalnum.Add(ttt.Count);
            //area_totalweight.Add(ttt.Sum(t => t.weight));

            //double t2 = (double)(_material.Last()._length) / 1000;
            //dt.Rows.Add(false, t2.ToString() + "~", area_totalnum.Last(), (double)area_totalnum.Last() / (double)total_num_bend, Convert.ToDouble((area_totalweight.Last() / 1000).ToString("F1")), area_totalweight.Last() / total_weight_bend);

            //x.Add(t2.ToString() + "~");
            //y1.Add(area_totalnum.Last());
            //y2.Add(Convert.ToDouble((area_totalweight.Last() / 1000).ToString("F1")));





            #endregion

            #region 按照1500一个区间

            int total_num_bend = 0;
            double total_weight_bend = 0;
            int[] area_totalnum = new int[9];              //添加大于12000的长度区间
            double[] area_totalweight = new double[9];
            int _interval = 1500;//1500一个区间

            foreach (var item in _mumalllist)
            {
                total_num_bend += item._totalnum;
                total_weight_bend += item._totalweight;
            }
            foreach (var item in _sonalllist)
            {
                int temp = 0;
                int.TryParse(item._length, out temp);

                if (temp <= 0) continue;

                if (temp / _interval < 8)    //按照500一个区间分隔
                {
                    if (temp % _interval == 0) //如果没有余数，完全整除，则累加到数组前一个元素，比如1.5~2米，钢筋是按照右边长度来切，此处很重要***
                    {
                        area_totalnum[temp / _interval - 1] += item._totalnum;
                        area_totalweight[temp / _interval - 1] += item._totalweight;
                    }
                    else
                    {
                        area_totalnum[temp / _interval] += item._totalnum;
                        area_totalweight[temp / _interval] += item._totalweight;
                    }
                }
                else
                {
                    area_totalnum[8] += item._totalnum;
                    area_totalweight[8] += item._totalweight;
                }
            }
            List<string> x = new List<string>();
            List<int> y1 = new List<int>();
            List<double> y2 = new List<double>();

            for (int i = 0; i < 8; i++)
            {
                double t0 = i * (double)_interval / 1000;
                double t1 = (i + 1) * (double)_interval / 1000;
                dt.Rows.Add(false, t0.ToString() + "~" + t1.ToString(), area_totalnum[i], (double)area_totalnum[i] / (double)total_num_bend, Convert.ToDouble((area_totalweight[i] / 1000).ToString("F1")), area_totalweight[i] / total_weight_bend);

                x.Add(t0.ToString() + "~" + t1.ToString());
                y1.Add(area_totalnum[i]);
                y2.Add(Convert.ToDouble((area_totalweight[i] / 1000).ToString("F1")));
            }

            dt.Rows.Add(false, (12).ToString() + "~", area_totalnum[8], (double)area_totalnum[8] / (double)total_num_bend, Convert.ToDouble((area_totalweight[8] / 1000).ToString("F1")), area_totalweight[8] / total_weight_bend);

            x.Add((12).ToString() + "~");
            y1.Add(area_totalnum[8]);
            y2.Add(Convert.ToDouble((area_totalweight[8] / 1000).ToString("F1")));
            #endregion

            #region 按照500一个区间
            //int total_num_bend = 0;
            //double total_weight_bend = 0;
            //int[] area_totalnum = new int[25];              //添加大于12000的长度区间
            //double[] area_totalweight = new double[25];
            //int _interval = 500;//500一个区间

            //foreach (var item in _mumalllist)
            //{
            //    total_num_bend += item._totalnum;
            //    total_weight_bend += item._totalweight;
            //}
            //foreach (var item in _sonalllist)
            //{
            //    int temp = 0;
            //    int.TryParse(item._length, out temp);

            //    if (temp <= 0) continue;

            //    if (temp / _interval < 24)    //按照500一个区间分隔
            //    {
            //        if (temp % _interval == 0) //如果没有余数，完全整除，则累加到数组前一个元素，比如1.5~2米，钢筋是按照右边长度来切，此处很重要***
            //        {
            //            area_totalnum[temp / _interval - 1] += item._totalnum;
            //            area_totalweight[temp / _interval - 1] += item._totalweight;
            //        }
            //        else
            //        {
            //            area_totalnum[temp / _interval] += item._totalnum;
            //            area_totalweight[temp / _interval] += item._totalweight;
            //        }
            //    }
            //    else
            //    {
            //        area_totalnum[24] += item._totalnum;
            //        area_totalweight[24] += item._totalweight;
            //    }
            //}
            //List<string> x = new List<string>();
            //List<int> y1 = new List<int>();
            //List<double> y2 = new List<double>();

            //for (int i = 0; i < 24; i++)
            //{
            //    double t0 = i * 0.5;
            //    double t1 = (i + 1) * 0.5;
            //    dt.Rows.Add(false, t0.ToString() + "~" + t1.ToString(), area_totalnum[i], (double)area_totalnum[i] / (double)total_num_bend, Convert.ToDouble((area_totalweight[i] / 1000).ToString("F1")), area_totalweight[i] / total_weight_bend);

            //    x.Add(t0.ToString() + "~" + t1.ToString());
            //    y1.Add(area_totalnum[i]);
            //    y2.Add(Convert.ToDouble((area_totalweight[i] / 1000).ToString("F1")));
            //}

            //dt.Rows.Add(false, (12).ToString() + "~", area_totalnum[24], (double)area_totalnum[24] / (double)total_num_bend, Convert.ToDouble((area_totalweight[24] / 1000).ToString("F1")), area_totalweight[24] / total_weight_bend);

            //x.Add((12).ToString() + "~");
            //y1.Add(area_totalnum[24]);
            //y2.Add(Convert.ToDouble((area_totalweight[24] / 1000).ToString("F1")));

            #endregion







            dataGridView3.DataSource = dt;
            dataGridView3.Columns[3].DefaultCellStyle.Format = "P1";
            dataGridView3.Columns[4].DefaultCellStyle.Format = "0.00";
            dataGridView3.Columns[5].DefaultCellStyle.Format = "P1";

            chartshow.ChartRectShow("长度区间(m)——数量(根)", x, y1, chart10);
            chartshow.ChartRectShow("长度区间(m)——重量(吨)", x, y2, chart11);
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
        //private bool diameterChecked_14 = false;
        private void checkBox36_CheckedChanged(object sender, EventArgs e)
        {
            //if (!checkBox36.Checked)
            //{
            //    checkBox36.Checked = false;
            //    checkBox36.Text = "Φ14-线材";
            //    this.diameterChecked_14 = false;

            //    checkBox8.Checked = false;
            //    checkBox8.Enabled = false;
            //    checkBox35.Checked = false;
            //    checkBox35.Enabled = false;
            //}
            //else
            //{
            //    checkBox36.Checked = true;
            //    checkBox36.Text = "Φ14-棒材";
            //    this.diameterChecked_14 = true;

            //    checkBox8.Checked = true;
            //    checkBox8.Enabled = true;
            //    checkBox35.Checked = true;
            //    checkBox35.Enabled = true;
            //}

        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].IsNewRow && e.ColumnIndex == 4)//消除默认的红叉叉
            {
                e.Value = pictureBox1.Image;
            }
        }

        private void dataGridView4_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView4.Rows[e.RowIndex].IsNewRow && e.ColumnIndex == 1)//消除默认的红叉叉
            {
                e.Value = pictureBox1.Image;
            }
        }

        private void dataGridView5_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView5.Rows[e.RowIndex].IsNewRow && e.ColumnIndex == 1)//消除默认的红叉叉
            {
                e.Value = pictureBox1.Image;
            }
        }

        private void dataGridView6_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView6.Rows[e.RowIndex].IsNewRow && e.ColumnIndex == 0)//消除默认的红叉叉
            {
                e.Value = pictureBox1.Image;
            }
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
                    return checkBox55.Checked ? true : false;
                case 2:
                    return checkBox58.Checked ? true : false;
                case 3:
                    return checkBox62.Checked ? true : false;
                case 4:
                    return checkBox61.Checked ? true : false;
                case 5:
                    return checkBox57.Checked ? true : false;
                case 6:
                    return checkBox59.Checked ? true : false;
                case 7:
                    return checkBox63.Checked ? true : false;
                case 8:
                    return checkBox60.Checked ? true : false;
                case 9:
                    return checkBox56.Checked ? true : false;
                case 10:
                    return checkBox64.Checked ? true : false;
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

            //int _bangThreshold = GeneralClass.m_typeC14 ? 14 : 16;
            int _bangThreshold = (GeneralClass.m_typeC12) ? 12 : ((GeneralClass.m_typeC14) ? 14 : 16);//先看12是否为棒材，再看14是否为棒材

            _newlist = _list.Where(t =>
            //棒材、线材
           (checkBox49.Checked ? (t.Diameter > 0) : (t.Diameter >= _bangThreshold)) &&
           //是否弯曲
           ((checkBox46.Checked ? (t.IfBend) : false) || ((checkBox45.Checked) ? (!t.IfBend) : false)) &&
           //是否套丝
           ((checkBox44.Checked ? (t.IfTao) : false) || ((checkBox43.Checked) ? (!t.IfTao) : false)) &&
           //形状类型，箍筋、拉勾、马凳、单多段
           ((checkBox70.Checked ? (t.RebarShapeType == EnumRebarShapeType.SHAPE_GJ) : false) ||
                        (checkBox71.Checked ? (t.RebarShapeType == EnumRebarShapeType.SHAPE_LG) : false) ||
                        (checkBox72.Checked ? (t.RebarShapeType == EnumRebarShapeType.SHAPE_MD) : false) ||
                        (checkBox74.Checked ? (t.RebarShapeType == EnumRebarShapeType.SHAPE_DT) : false) ||
                        (checkBox73.Checked ? (t.RebarShapeType == EnumRebarShapeType.SHAPE_ZJ) : false)) &&

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

            bool _flag = (checkBox54.Checked ? (_num <= GeneralClass.wareArea[0]) : false)
                        || (checkBox53.Checked ? (_num > GeneralClass.wareArea[0] && _num <= GeneralClass.wareArea[1]) : false)
                        || (checkBox52.Checked ? (_num > GeneralClass.wareArea[1] && _num <= GeneralClass.wareArea[2]) : false)
                        || (checkBox51.Checked ? (_num > GeneralClass.wareArea[2]) : false);

            return _flag;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //解析钢筋总表的所有构件

            GeneralClass.interactivityData?.printlog(1, "开始解析所有料单的构件包");

            GeneralClass.AllElementList = GeneralClass.DBOpt.GetAllElementList(GeneralClass.TableName_AllRebar);

            if (GeneralClass.AllElementList.Count == 0)
            {
                GeneralClass.interactivityData?.printlog(1, "构件包列表为空");
                return;
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
            dt_z.Columns.Add("最大长度", typeof(int));
            dt_z.Columns.Add("最小长度", typeof(int));

            int _num = 0;
            double _weight = 0;
            int _maxlength, _minlength = 0;
            int _index = 0;

            List<string> x1 = new List<string>() { "1种", "2种", "3种", "4种", "5种", "6种", "7种", "8种", "9种", "10种" };
            List<int> y1 = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            List<string> x2 = new List<string>() { "0~" + GeneralClass.wareArea[0].ToString(),
                                                 GeneralClass.wareArea[0].ToString()+"~"+ GeneralClass.wareArea[1].ToString(),
                                                 GeneralClass.wareArea[1].ToString()+"~"+ GeneralClass.wareArea[2].ToString(),
                                                 GeneralClass.wareArea[2].ToString()+"~"};
            List<int> y2 = new List<int>() { 0, 0, 0, 0 };

            foreach (ElementData item in GeneralClass.AllElementList)
            {
                var _banglist = PickNewList(item.rebarlist);

                _num = _banglist.Sum(t => t.TotalPieceNum);
                _weight = _banglist.Sum(t => t.TotalWeight);

                if (checkBox47.Checked ? (_num != 0) : true)//去零显示
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
                    var _group = GeneralClass.DBOpt.QueryAllListByDiameter(_banglist);

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

                    //绘制柱状图
                    y1[_group.Count - 1]++;//对应几种直径种类的++
                    if (_num > 0 && _num <= GeneralClass.wareArea[0])
                    {
                        y2[0]++;
                    }
                    else if (_num > GeneralClass.wareArea[0] && _num <= GeneralClass.wareArea[1])
                    {
                        y2[1]++;
                    }
                    else if (_num > GeneralClass.wareArea[1] && _num <= GeneralClass.wareArea[2])
                    {
                        y2[2]++;
                    }
                    else if (_num > GeneralClass.wareArea[2])
                    {
                        y2[3]++;
                    }
                    chartshow.ChartRectShow("直径种类——构件包数量", x1, y1, chart13);
                    chartshow.ChartRectShow("数量区间——构件包数量", x2, y2, chart12);

                }

            }
            dataGridView29.DataSource = dt_z;
            //dataGridView293.Columns[3].DefaultCellStyle.Format = "0.000";        //
            dataGridView29.Columns[6].DefaultCellStyle.Format = "0.00";          //

            GeneralClass.interactivityData?.printlog(1, "所有构件包解析完成");

        }

        private void checkBox48_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView27.Columns[5].Visible = checkBox48.Checked;
            dataGridView28.Columns[5].Visible = checkBox48.Checked;
            //dataGridView.Columns[5].Visible = checkBox48.Checked;

        }

        private void dataGridView29_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                //点击dgv3每个构件包时，在dgv4中显示其详细信息
                if (e.RowIndex > -1)
                {
                    int _index = Convert.ToInt32(dataGridView29.Rows[e.RowIndex].Cells[1].Value.ToString());
                    string _project = dataGridView29.Rows[e.RowIndex].Cells[2].Value.ToString();
                    string _assembly = dataGridView29.Rows[e.RowIndex].Cells[3].Value.ToString();
                    string _element = dataGridView29.Rows[e.RowIndex].Cells[4].Value.ToString();

                    foreach (var item in GeneralClass.AllElementList)
                    {
                        if (item.projectName == _project && item.assemblyName == _assembly && item.elementName == _element && item.elementIndex == _index)
                        {
                            //var newlist = item.rebarlist.Where(t => (checkBox2.Checked ? (t.Diameter > 0) : (t.Diameter >= 16))).ToList();//是否显示线材
                            var newlist = PickNewList(item.rebarlist);

                            Form2.FillDGVWithRebarList(newlist, dataGridView28);

                            dataGridView28.Columns[5].Visible = checkBox48.Checked;

                        }
                    }

                }
            }
            catch (Exception ex) { MessageBox.Show("dataGridView29_CellClick error:" + ex.Message); }
        }

        private List<ElementData> _elements = new List<ElementData>();

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
                            if (item.rebarlist.Count != 0 && dataGridView27 != null)
                            {
                                ////选择是否显示线材
                                //var newlist = item.rebarlist.Where(t => (checkBox2.Checked ? (t.Diameter > 0) : (t.Diameter >= 16))).ToList();

                                var newlist = PickNewList(item.rebarlist);

                                //显示构件详细
                                int _threshold = (GeneralClass.m_typeC12) ? 12 : ((GeneralClass.m_typeC14) ? 14 : 16);//先看12是否为棒材，再看14是否为棒材
                                if (checkBox50.Checked && !checkBox49.Checked)//仅棒材
                                {
                                    newlist = newlist.Where(t => t.Diameter >= _threshold).ToList();
                                }
                                else if (!checkBox50.Checked && checkBox49.Checked)//仅线材
                                {
                                    newlist = newlist.Where(t => t.Diameter < _threshold).ToList();
                                }

                                FillDGVWithRebarList(newlist, dataGridView27);



                                //ShowElementAddData(item.rebarlist);
                                ShowElementAddData(newlist);

                                dataGridView27.Columns[5].Visible = checkBox48.Checked;

                            }
                        }
                    }

                }
            }
        }

        //private string _selectproject = "";
        //private string _selectassembly = "";

        private void InitTreeView1()
        {
            treeView1.Nodes.Clear();
            treeView1.LabelEdit = true;
            treeView1.ExpandAll();
            treeView1.CheckBoxes = true;//节点的勾选框

        }
        private void ShowAllElement(string _project, string _assembly)
        {
            InitTreeView1();

            //if(GeneralClass.CfgData.MaterialBill==EnumMaterialBill.EJIN)//e筋料单
            //{
            _elements = GeneralClass.DBOpt.GetAllElementList(GeneralClass.TableName_AllRebar, _project, _assembly);
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
            //}
            //else//广联达料单
            //{
            //    TreeNode tn = new TreeNode();
            //    TreeNode tn1 = new TreeNode();

            //    foreach(var item in GeneralClass.GldOpt.gld_Structure._Buildings.Find(t => t.BuildingName == _project)._Floors.Find(t => t.FloorName == _assembly)._Instances)
            //    {
            //        tn1=new TreeNode();
            //        tn1.Text=item.InstanceID+"_"+item.InstanceName;
            //        tn.Nodes.Add(tn1);
            //    }
            //    tn.Text= _assembly;
            //    treeView1.Nodes.Add(tn);
            //    treeView1.ExpandAll();

            //}



        }
        /// <summary>
        /// 显示单个构件包按照直径规格的汇总信息
        /// </summary>
        /// <param name="_list"></param>
        private void ShowElementAddData(List<RebarData> _list)
        {
            try
            {

                DataTable dt_z = new DataTable();
                dt_z.Columns.Add("直径", typeof(string));
                dt_z.Columns.Add("总长度(m)", typeof(double));
                dt_z.Columns.Add("总数量(根)", typeof(int));
                dt_z.Columns.Add("总重量(kg)", typeof(double));

                List<GroupbyDiaWithLength> _grouplist = GeneralClass.DBOpt.QueryAllListByDiameterWithLength(_list);

                foreach (var item in _grouplist)
                {
                    int _threshold = (GeneralClass.m_typeC12) ? 12 : ((GeneralClass.m_typeC14) ? 14 : 16);//先看12是否为棒材，再看14是否为棒材

                    if (checkBox50.Checked ? (item._diameter >= _threshold) : false)//棒材
                    {
                        dt_z.Rows.Add("Φ" + item._diameter.ToString(), item._totallength / 1000, item._totalnum, item._totalweight);
                    }
                    if (checkBox49.Checked ? (item._diameter < _threshold) : false)//线材
                    {
                        dt_z.Rows.Add("Φ" + item._diameter.ToString(), item._totallength / 1000, item._totalnum, item._totalweight);
                    }
                }
                dataGridView26.DataSource = dt_z;
                dataGridView26.Columns[1].DefaultCellStyle.Format = "0.000";        //
                                                                                    //dataGridView26.Columns[2].DefaultCellStyle.Format = "0.0";        //
                dataGridView26.Columns[3].DefaultCellStyle.Format = "0.0";          //
            }
            catch (Exception ex) { /*throw ex;*/MessageBox.Show("ShowElementAddData error :" + ex.Message); }

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

        private void InitCheckBox_element()
        {
            checkBox50.Checked = true;//棒材
            checkBox49.Checked = true;//线材

            checkBox48.Checked = false;//详细
            checkBox47.Checked = true;//去零

            checkBox46.Checked = true;//弯曲
            checkBox45.Checked = true;//不弯曲
            checkBox44.Checked = true;//套丝
            checkBox43.Checked = true;//不套丝

            checkBox42.Checked = false;//标化
            checkBox41.Checked = true;//非标

            checkBox70.Checked = true;//箍筋
            checkBox71.Checked = true;//拉勾
            checkBox72.Checked = true;//马凳
            checkBox73.Checked = true;//主筋
            checkBox74.Checked = true;//端头


            //直径种类
            checkBox55.Checked = true;
            checkBox58.Checked = true;
            checkBox62.Checked = true;
            checkBox61.Checked = true;
            checkBox57.Checked = true;
            checkBox59.Checked = true;
            checkBox63.Checked = true;
            checkBox60.Checked = true;
            checkBox56.Checked = true;
            checkBox64.Checked = true;

            //数量区间
            checkBox54.Checked = true;
            checkBox53.Checked = true;
            checkBox52.Checked = true;
            checkBox51.Checked = true;



        }

        private void dataGridView27_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView27.Rows[e.RowIndex].IsNewRow && e.ColumnIndex == 4)//消除默认的红叉叉
            {
                e.Value = pictureBox1.Image;
            }
        }

        private void dataGridView28_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView28.Rows[e.RowIndex].IsNewRow && e.ColumnIndex == 4)//消除默认的红叉叉
            {
                e.Value = pictureBox1.Image;
            }
        }

        private void dataGridView31_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView31.Rows[e.RowIndex].IsNewRow && e.ColumnIndex == 1)//消除默认的红叉叉
            {
                e.Value = pictureBox1.Image;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!CheckDiameterSinglePick())
            {
                MessageBox.Show("操作无效，请先确保只选择一种直径规格的钢筋！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            GeneralClass.m_MaterialPool.Clear();//清空原材库
            GeneralClass.AddDefaultMaterial();//添加默认的原材库

            DataTable dt_ori = new DataTable();
            dt_ori.Columns.Add(" ", typeof(bool));
            dt_ori.Columns.Add("直径", typeof(string));
            dt_ori.Columns.Add("长度", typeof(int));
            dt_ori.Columns.Add("数量", typeof(int));

            EnumDiaBang _dia = CheckDiameterWhichPick();//获取选中哪种直径
            string s_dia = "Φ" + _dia.ToString().Substring(6, 2);

            if (tabControl4.SelectedIndex == 0)
            {
                //从长度列表里面挑选进入单一直径原材库
                DataTable dt = (DataTable)dataGridView2.DataSource;//获取更新的数据
                foreach (DataRow item in dt.Rows)
                {
                    if ((bool)item[0])
                    {
                        dt_ori.Rows.Add(true, s_dia, (int)item[1], (int)item[2]);
                    }
                }
            }
            else
            {
                //从长度区间列表里面挑选进入单一直径原材库
                DataTable dt = (DataTable)dataGridView3.DataSource;//获取更新的数据
                foreach (DataRow item in dt.Rows)
                {
                    if ((bool)item[0])
                    {
                        dt_ori.Rows.Add(true, s_dia, Convert.ToDouble(item[1].ToString().Split('~')[1]) * 1000, (int)item[2]);
                    }
                }


            }

            //添加默认原材库
            List<MaterialOri> _material = GeneralClass.m_MaterialPool.Where(t => t._diameter == _dia).ToList().OrderBy(k => k._length).ToList();
            foreach (var item in _material)
            {
                dt_ori.Rows.Add(true, s_dia, item._length, 999);
            }

            ////获取已经加入的
            //List<int> _exist = new List<int>();
            //foreach (DataRow item in dt_ori.Rows)
            //{
            //    _exist.Add((int)item[2]);
            //}

            ////添加默认的几个长度，
            ////9米原材包括：1.8米、2.25米、3米、4.5米、6米、7.2米、9米
            ////12米原材包括：2米、3米、4米、5米、6米、7米、8米、9米、10米、12米
            //if (GeneralClass.CfgData.OriginType == EnumOriType.ORI_9)
            //{
            //    //if (!_exist.Contains(1800)) dt_ori.Rows.Add(true, s_dia, 1000, 999);
            //    if (!_exist.Contains(1800)) dt_ori.Rows.Add(true, s_dia, 1800, 999);
            //    if (!_exist.Contains(2250)) dt_ori.Rows.Add(true, s_dia, 2250, 999);
            //    if (!_exist.Contains(3000)) dt_ori.Rows.Add(true, s_dia, 3000, 999);
            //    if (!_exist.Contains(4500)) dt_ori.Rows.Add(true, s_dia, 4500, 999);
            //    if (!_exist.Contains(6000)) dt_ori.Rows.Add(true, s_dia, 6000, 999);
            //    if (!_exist.Contains(7200)) dt_ori.Rows.Add(true, s_dia, 7200, 999);
            //    //if (!_exist.Contains(9000)) dt_ori.Rows.Add(true, s_dia, 8000, 999);
            //    if (!_exist.Contains(9000)) dt_ori.Rows.Add(true, s_dia, 9000, 999);
            //}
            //else
            //{
            //    if (!_exist.Contains(2000)) dt_ori.Rows.Add(true, s_dia, 2000, 999);
            //    if (!_exist.Contains(3000)) dt_ori.Rows.Add(true, s_dia, 3000, 999);
            //    if (!_exist.Contains(4000)) dt_ori.Rows.Add(true, s_dia, 4000, 999);
            //    if (!_exist.Contains(5000)) dt_ori.Rows.Add(true, s_dia, 5000, 999);
            //    if (!_exist.Contains(6000)) dt_ori.Rows.Add(true, s_dia, 6000, 999);
            //    if (!_exist.Contains(7000)) dt_ori.Rows.Add(true, s_dia, 7000, 999);
            //    if (!_exist.Contains(8000)) dt_ori.Rows.Add(true, s_dia, 8000, 999);
            //    if (!_exist.Contains(9000)) dt_ori.Rows.Add(true, s_dia, 9000, 999);
            //    if (!_exist.Contains(10000)) dt_ori.Rows.Add(true, s_dia, 10000, 999);
            //    if (!_exist.Contains(12000)) dt_ori.Rows.Add(true, s_dia, 12000, 999);
            //}

            FillDGV_MaterialPoolSingleDia(dt_ori);

            checkBox65.Checked = true;//默认全选
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //清除勾选
            DataTable dt = (DataTable)dataGridView33.DataSource;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if ((bool)dt.Rows[i][0])//删掉勾选行
                {
                    dt.Rows[i].Delete();
                }
            }
            dt.AcceptChanges();//注意用delete后，需要acceptchange，才会生效

            FillDGV_MaterialPoolSingleDia(dt);

        }
        private void button15_Click(object sender, EventArgs e)
        {
            //清除勾选
            DataTable dt = (DataTable)dataGridView34.DataSource;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if ((bool)dt.Rows[i][0])//删掉勾选行
                {
                    dt.Rows[i].Delete();
                }
            }
            dt.AcceptChanges();//注意用delete后，需要acceptchange，才会生效

            FillDGV_MaterialPoolAllDia(dt);
        }
        private void button11_Click(object sender, EventArgs e)
        {
            //从单一直径原材库挑选进入总原材库

            DataTable dt_all = (DataTable)dataGridView34.DataSource;//获取更新的数据
            if (dt_all == null || dt_all.Rows.Count == 0)//空的
            {
                dt_all = new DataTable();
                dt_all.Columns.Add(" ", typeof(bool));
                dt_all.Columns.Add("直径", typeof(string));
                dt_all.Columns.Add("长度", typeof(int));
                dt_all.Columns.Add("数量", typeof(int));
            }

            DataTable dt_single = (DataTable)dataGridView33.DataSource;//获取更新的数据
            foreach (DataRow item in dt_single.Rows)
            {
                if ((bool)item[0])
                {
                    dt_all.Rows.Add(true, (string)item[1], (int)item[2], (int)item[3]);
                }
            }
            FillDGV_MaterialPoolAllDia(dt_all);
            checkBox66.Checked = true;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            //dt.Columns.Add(" ", typeof(bool));
            //dt.Columns.Add("直径", typeof(string));
            //dt.Columns.Add("长度", typeof(int));
            //dt.Columns.Add("数量", typeof(int));
            FillDGV_MaterialPoolAllDia(dt);//清空
        }

        private void checkBox65_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView33.DataSource;
            foreach (DataRow item in dt.Rows)
            {
                item[0] = checkBox65.Checked ? true : false;
            }
            checkBox65.Text = checkBox65.Checked ? "全选" : "全不选";
        }

        private void checkBox66_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView34.DataSource;
            foreach (DataRow item in dt.Rows)
            {
                item[0] = checkBox66.Checked ? true : false;
            }
            checkBox66.Text = checkBox66.Checked ? "全选" : "全不选";

        }

        private void checkBox67_CheckedChanged(object sender, EventArgs e)
        {
            checkBox68.Checked = checkBox67.Checked ? true : false;
            checkBox8.Checked = checkBox67.Checked ? true : false;
            checkBox9.Checked = checkBox67.Checked ? true : false;
            checkBox10.Checked = checkBox67.Checked ? true : false;
            checkBox11.Checked = checkBox67.Checked ? true : false;
            checkBox12.Checked = checkBox67.Checked ? true : false;
            checkBox13.Checked = checkBox67.Checked ? true : false;
            checkBox14.Checked = checkBox67.Checked ? true : false;
            checkBox15.Checked = checkBox67.Checked ? true : false;
            checkBox18.Checked = checkBox67.Checked ? true : false;
            checkBox39.Checked = checkBox67.Checked ? true : false;

            checkBox69.Checked = checkBox67.Checked ? true : false;
            checkBox35.Checked = checkBox67.Checked ? true : false;
            checkBox34.Checked = checkBox67.Checked ? true : false;
            checkBox33.Checked = checkBox67.Checked ? true : false;
            checkBox32.Checked = checkBox67.Checked ? true : false;
            checkBox31.Checked = checkBox67.Checked ? true : false;
            checkBox30.Checked = checkBox67.Checked ? true : false;
            checkBox29.Checked = checkBox67.Checked ? true : false;
            checkBox28.Checked = checkBox67.Checked ? true : false;
            checkBox27.Checked = checkBox67.Checked ? true : false;
            checkBox40.Checked = checkBox67.Checked ? true : false;

            checkBox67.Text = checkBox67.Checked ? "全选" : "全不选";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MaterialOri _material = new MaterialOri();
            GeneralClass.m_MaterialPool.Clear();//清空原材库
            GeneralClass.AddDefaultMaterial();//添加默认的原材库

            List<MaterialOri> _list = new List<MaterialOri>();//临时存原材库

            DataTable dt = (DataTable)dataGridView34.DataSource;
            if (dt != null)
            {
                foreach (DataRow item in dt.Rows)
                {
                    if ((bool)item[0])
                    {
                        _material = new MaterialOri();
                        _material._diameter = GeneralClass.IntToEnumDiameter(Convert.ToInt16(item[1].ToString().Substring(1, 2)));//取直径
                        _material._length = (int)item[2];
                        _material._num = (int)item[3];

                        //_list.Add(_material);
                        GeneralClass.m_MaterialPool.Add(_material);
                    }
                }
            }

            GeneralClass.interactivityData?.printlog(1, "生成原材库成功！");
            MessageBox.Show("生成原材库成功！");
        }

        private void dataGridView12_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //点击线材，显示其不同规格统计信息
                if (e.RowIndex > -1)
                {
                    int _diameter = Convert.ToInt32(dataGridView12.Rows[e.RowIndex].Cells[0].Value.ToString().Substring(1));//取直径

                    List<RebarData> _xiandata_w = new List<RebarData>();//弯曲
                    //_xiandata_w = GeneralClass.AllRebarList.Where(t => t.Diameter == _diameter && t.PicTypeNum != "10000").ToList();
                    _xiandata_w = GeneralClass.AllRebarList.Where(t => t.Diameter == _diameter&&( t.RebarShapeType == EnumRebarShapeType.SHAPE_GJ
                    || t.RebarShapeType == EnumRebarShapeType.SHAPE_LG)).ToList();

                    FillDGV_Xian_detail(_xiandata_w, ref dataGridView36);

                }
            }
            catch (Exception ex) { MessageBox.Show("dataGridView12_CellClick error:" + ex.Message); }
        }

        private void dataGridView36_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView36.Rows[e.RowIndex].IsNewRow && (e.ColumnIndex == 1 /*|| e.ColumnIndex == 4*/))//消除默认的红叉叉
            {
                e.Value = pictureBox1.Image;
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (!CheckDiameterSinglePick())
            {
                MessageBox.Show("操作无效，请先确保只选择一种直径规格的钢筋！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<RebarData> _sonData = new List<RebarData>();//分子
            int _threshold = (GeneralClass.m_typeC12) ? 12 : ((GeneralClass.m_typeC14) ? 14 : 16);//先看12是否为棒材，再看14是否为棒材
            foreach (RebarData _dd in GeneralClass.AllRebarList)
            {
                if (_dd.Diameter >= _threshold && _dd.TotalPieceNum != 0
                        && CheckLengthType(_dd, false)
                        && CheckWorkType(_dd, false)
                        && CheckDiameter(_dd, false))
                {
                    _sonData.Add(_dd);
                }
            }

            _piAutoTaoTargetList = Algorithm.Taoliao_pi(_sonData);

            FillDGV_Pi_Tao_show(_piAutoTaoTargetList);

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //RebarOri temp = new RebarOri();

            //int _count = Convert.ToInt32(comboBox1.SelectedItem.ToString());

            //for (int i = 1; i <= _count; i++)
            //{
            //    Rebar _rebar = new Rebar((int)((double)GeneralClass.OriginalLength(_list1.First().Level, _list1.First().Diameter) / _count));
            //    temp._list.Add(_rebar);
            //}
            //pictureBox2.Image = graphics.PaintRebar(temp);
        }

        private void tabControl2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                isShiftDown = true;
            }
        }

        private void tabControl2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                isShiftDown = false;
            }
        }

        private bool _fold = false;
        private void button18_Click(object sender, EventArgs e)
        {
            FillDGV_Pi_Tao_Manual(_piManulTaoTargetList, _fold);
            _fold = !_fold;
        }
        /// <summary>
        /// 自动编号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView38_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //自动编号，与数据无关
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
               e.RowBounds.Location.Y,
               dataGridView38.RowHeadersWidth - 4,
               e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics,
                  (e.RowIndex + 1).ToString(),
                   dataGridView38.RowHeadersDefaultCellStyle.Font,
                   rectangle,
                   dataGridView38.RowHeadersDefaultCellStyle.ForeColor,
                   TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void dataGridView9_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //点击线材，显示其不同规格统计信息
                if (e.RowIndex > -1)
                {
                    int _diameter = Convert.ToInt32(dataGridView9.Rows[e.RowIndex].Cells[0].Value.ToString().Substring(1));//取直径

                    List<RebarData> _xiandata_z = new List<RebarData>();//弯曲
                    //_xiandata_z = GeneralClass.AllRebarList.Where(t => t.Diameter == _diameter && t.PicTypeNum == "10000").ToList();
                    _xiandata_z = GeneralClass.AllRebarList.Where(t => t.Diameter == _diameter&& t.RebarShapeType != EnumRebarShapeType.SHAPE_GJ
                                            && t.RebarShapeType != EnumRebarShapeType.SHAPE_LG).ToList();
                    //_xiandata_w = GeneralClass.AllRebarList.Where(t => t.RebarShapeType == EnumRebarShapeType.SHAPE_GJ
                    //|| t.RebarShapeType == EnumRebarShapeType.SHAPE_LG).ToList();


                    FillDGV_Xian_detail(_xiandata_z, ref dataGridView36);

                }
            }
            catch (Exception ex) { MessageBox.Show("dataGridView9_CellClick error:" + ex.Message); }
        }
    }
}
