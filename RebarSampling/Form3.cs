//using Etable;
//using Grand;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Asn1.X509;
using RebarSampling.General;
using RebarSampling.GLD;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace RebarSampling
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();

            InitDataGridView();
            InitDataGridView9();
            InitDataGridView11();
            InitTreeView();
            //InitCheckBox();
            //InitDgvTaoliao();
            InitCombobox();
            InitCheckbox();
            InitTabcontrol();
            InitRadioButton();
            InitSplitControl();

            //GeneralClass.interactivityData.getTaosiSetting += GetTaosiSetting;
            GeneralClass.interactivityData.getManualBatchList += GetManualBatchList;

            GeneralClass.interactivityData.askForPickStatus += AskForPickStatus;

            GeneralClass.interactivityData.showAssembly += GetSheetToDGV;

        }

        private bool _ifAutoGroup = false;
        private void InitRadioButton()
        {
            radioButton1.Checked = true;
            this._ifAutoGroup = true;
            radioButton2.Checked = false;
        }
        private void InitCheckbox()
        {
            checkBox3.Checked = true;
            checkBox4.Checked = true;
            checkBox5.Checked = true;
            checkBox6.Checked = true;

            checkBox7.Checked = true;
            checkBox8.Checked = true;
            checkBox9.Checked = false;//箍筋，默认不选
            checkBox10.Checked = false;//拉勾，默认不选
            checkBox11.Checked = false;//马凳，默认不选
            checkBox12.Checked = true;
            checkBox13.Checked = true;

        }
        private void InitTabcontrol()
        {
            FormBillSplit _newformBillSplit = new FormBillSplit();
            _newformBillSplit.FormBorderStyle = FormBorderStyle.None;//去除边框
            _newformBillSplit.Dock = DockStyle.Fill;//填充满
            _newformBillSplit.TopLevel = false;
            tabControl5.TabPages[0].Controls.Add(_newformBillSplit);
            _newformBillSplit.Show();


            FormManualGroupLB _newformLB = new FormManualGroupLB();
            _newformLB.FormBorderStyle = FormBorderStyle.None;//去除边框
            _newformLB.Dock = DockStyle.Fill;//填充满
            _newformLB.TopLevel = false;
            tabControl1.TabPages[0].Controls.Add(_newformLB);
            _newformLB.Show();

            FormManualTaoQZ _newformQZ = new FormManualTaoQZ();
            _newformQZ.FormBorderStyle = FormBorderStyle.None;//去除边框
            _newformQZ.Dock = DockStyle.Fill;//填充满
            _newformQZ.TopLevel = false;
            tabControl5.TabPages[2].Controls.Add(_newformQZ);
            _newformQZ.Show();




        }
        private void InitCombobox()
        {
            comboBox1.SelectedIndex = 0;


        }
        private void InitSplitControl()
        {
            ////隐藏导航栏
            //this.splitContainer1.Panel1Collapsed = true;
            ////根据隐藏属性切换项目资源文件中的图片显示
            //button1.Image = this.splitContainer1.Panel1Collapsed ? Properties.Resources.icons8_double_down_26 : Properties.Resources.icons8_double_up_26;

        }
        private void InitTreeView()
        {
            treeView5.Nodes.Clear();
            treeView5.LabelEdit = true;
            treeView5.ExpandAll();
            treeView5.CheckBoxes = false;//节点的勾选框

            treeView2.Nodes.Clear();
            treeView2.LabelEdit = true;
            treeView2.ExpandAll();
            treeView2.CheckBoxes = false;//节点的勾选框

            treeView3.Nodes.Clear();
            treeView3.LabelEdit = true;
            treeView3.ExpandAll();
            treeView3.CheckBoxes = false;//节点的勾选框

            treeView4.Nodes.Clear();
            treeView4.LabelEdit = true;
            treeView4.ExpandAll();
            treeView4.CheckBoxes = false;//节点的勾选框

            treeView1.Nodes.Clear();
            treeView1.LabelEdit = true;
            treeView1.ExpandAll();
            treeView1.CheckBoxes = false;//节点的勾选框

            treeView6.Nodes.Clear();
            treeView6.LabelEdit = true;
            treeView6.ExpandAll();
            treeView6.CheckBoxes = false;//节点的勾选框



        }


        private void InitDataGridView9()
        {
            Form2.InitDGV(dataGridView9);

        }
        private void InitDataGridView()
        {
            DataTable dt = new DataTable();
            dataGridView10.DataSource = dt;
            dataGridView8.DataSource = dt;
        }

        private void InitDataGridView11()
        {
            //解决datagirdview11在加载大量数据时卡顿的bug，20231123
            //在设置ＤataGridView 的AutoSizeColumnsMode以及AotuSizeRowsMode这两项时尽量选择None，不要AllCells,实在需要选择DisplayedCells
            dataGridView11.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridView11.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView11.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dataGridView11.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;

            DataTable dt = new DataTable();
            dataGridView11.DataSource = dt;
        }

        private void GetSheetToDGV(string _projectname, string _assemblyname)
        {
            //先获取钢筋总表
            //List<RebarData> _allList_bk = GeneralClass.SQLiteOpt.GetAllRebarList(GeneralClass.AllRebarBKTableName);
            List<RebarData> _allList = GeneralClass.DBOpt.GetAllRebarList(GeneralClass.TableName_AllRebar);

            //查询所有被选中的钢筋
            List<RebarData> _newlist = new List<RebarData>();
            foreach (RebarData _data in _allList)
            {

                if (_data.TableName == _projectname && _data.TableSheetName == _assemblyname)//20240923修改，原项目名和主构件名修改为料表名和料表sheet名
                {
                    _newlist.Add(_data);
                }
            }

            if (_newlist.Count != 0 && dataGridView1 != null)
            {
                Form2.FillDGVWithRebarList(_newlist, dataGridView13);
            }



        }

        /// <summary>
        /// form3独有的，相比form2的同名方法，增加了勾选、进度管控的列
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="_dgv"></param>
        private void FillDGVWithRebarList(List<RebarData> _list, DataGridView _dgv)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.CHILD_ASSEMBLY_NAME], typeof(string));
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.ELEMENT_NAME], typeof(string));
            //dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PIC_NO], typeof(string));
            //dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.LEVEL], typeof(string));
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.DIAMETER], typeof(string));
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.REBAR_PIC], typeof(Image));
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.CORNER_MESSAGE], typeof(string));
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.LENGTH], typeof(string));
            //dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.PIECE_NUM_UNIT_NUM], typeof(string));
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TOTAL_PIECE_NUM], typeof(string));
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TOTAL_WEIGHT], typeof(string));
            //dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.DESCRIPTION], typeof(string));
            dt.Columns.Add(GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.SERIALNUM], typeof(string));

            dt.Columns.Add("产线生产", typeof(bool));
            dt.Columns.Add("选择", typeof(bool));
            dt.Columns.Add("加工单下发", typeof(bool));
            dt.Columns.Add("加工完成", typeof(bool));

            foreach (var item in _list)
            {
                string sType = item.PicTypeNum;
                if (sType == "") continue;  //类型为空，跳过加载图片
                object _image = null;

                if (GeneralClass.CfgData.MaterialBill == EnumMaterialBill.EJIN)//e筋料单
                {
                    int _realheight = 0;
                    _image = graphics.PaintRebarPic(item, out _realheight);//直接画钢筋简图，20250403修改，关闭下面找钢筋图片库的代码，                    
                }
                else//广联达料单
                {
                    string newpath = GeneralClass.CfgData.GLDpath + @"\" + item.RebarPic;//广联达料单路径+图片路径即为完整路径
                    _image = Gld.LoadGldImage(newpath);
                }

                dt.Rows.Add(
                    item.ChildAssemblyName,
                    item.ElementName,
                   //item.PicTypeNum,
                   //item.Level,
                   item.Level + item.Diameter,
                    _image,
                    item.CornerMessage,
                    item.Length,
                    //item.PieceNumUnitNum,
                    item.TotalPieceNum,
                    item.TotalWeight,
                    //item.Description,
                    item.SerialNum,

                    item.IfMakeInLine,
                    item.IfPickInBill,
                    item.IfSendToPCS,
                    item.IfMakeDone
                    );
            }

            _dgv.DataSource = dt;
            //_dgv.Columns[2].DefaultCellStyle.Format = "P1";
            _dgv.Columns[7].DefaultCellStyle.Format = "0.00";
            //_dgv.Columns[4].DefaultCellStyle.Format = "P1";

        }


        private void ShowElementAddData(List<ElementRebarFB> _fblist, DataGridView _dgv)
        {
            DataTable dt_z = new DataTable();
            dt_z.Columns.Add("直径", typeof(string));
            dt_z.Columns.Add("总长度(m)", typeof(double));
            dt_z.Columns.Add("总数量(根)", typeof(int));
            dt_z.Columns.Add("总重量(kg)", typeof(double));

            List<GroupbyDiaWithLength> _group = new List<GroupbyDiaWithLength>();

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

        private List<ElementBatch> ManualSortBatchAndGroup()
        {
            return m_ManualBatchList;

            //List<ElementBatch> _allBatch = new List<ElementBatch>();

            //return _allBatch;
        }
        /// <summary>
        /// 将所有的构件批（batch）进行分组排产
        /// 排产规则：
        /// 1、不同料仓规格的（8/4/2/1/）可以混批排产；
        /// 2、优先根据直径种类来排产，相同直径组合的不管8仓还是4仓可以混仓加工；
        /// 3、前后批的直径种类最多只替换一种，以满足套丝机切换直径的频次尽可能少的原则
        /// 4、考虑上料便利性，前后批同一种直径的衔接上料
        /// </summary>
        private List<ElementBatch> AutoSortBatchAndGroup()
        {
            List<ElementBatch> _allBatch = new List<ElementBatch>();
            List<ElementDadBatch> _dadBatchList = new List<ElementDadBatch>();//大批的定义

            if (GeneralClass.CfgData.DiaGroupType == EnumDiaGroupTypeSetting.Sequence)//顺序分组
            {
                foreach (var item in _oneWorklist)
                {
                    _allBatch.AddRange(item);
                }
                foreach (var item in _twoWorklist)
                {
                    _allBatch.AddRange(item);
                }
                foreach (var item in _threeWorklist)
                {
                    _allBatch.AddRange(item);
                }
                foreach (var item in _fourWorklist)
                {
                    _allBatch.AddRange(item);
                }

            }
            else//混合分组
            {
                //1~4种直径
                foreach (var item in _fewWorklist)
                {
                    _allBatch.AddRange(item);
                }


                //先按照直径种类的包含关系分组
                bool bUse = false;
                foreach (var item in _allBatch)
                {
                    if (_dadBatchList.Count == 0)//如果大批是空的，则新建一个
                    {
                        ElementDadBatch _dadBatch = new ElementDadBatch();
                        _dadBatch.batchlist.Add(item);
                        _dadBatchList.Add(_dadBatch);
                    }
                    else
                    {
                        bUse = false;
                        foreach (var ttt in _dadBatchList.ToArray())
                        {
                            if (item.IfIncludeUnderFour(ttt.diameterList))//如果当前批的直径种类能够包含进大批，则塞入大批
                            {
                                ttt.batchlist.Add(item);
                                bUse = true;
                                break;
                            }
                        }
                        if (!bUse)//没有匹配的，则新建一个大批
                        {
                            ElementDadBatch _dadBatch = new ElementDadBatch();
                            _dadBatch.batchlist.Add(item);
                            _dadBatchList.Add(_dadBatch);

                        }
                    }
                }

                //再对大构件批进行前后批的排列，保证相邻批次间最多只切换一个直径（格雷码排序）
                for (int i = 0; i < _dadBatchList.Count - 1; i++)
                {
                    for (int j = i + 1; j < _dadBatchList.Count; j++)
                    {
                        if (_dadBatchList[i].IfHaveSameDia(_dadBatchList[j], 3))
                        {
                            //交换j与i+1的位置
                            ElementDadBatch temp = _dadBatchList[i + 1];
                            _dadBatchList[i + 1] = _dadBatchList[j];
                            _dadBatchList[j] = temp;
                        }
                    }
                }

                //重新给_allBatch数据
                _allBatch.Clear();
                foreach (var item in _dadBatchList)
                {
                    _allBatch.AddRange(item.batchlist);
                }

            }
            //把多直径的补上
            foreach (var item in _multiWorklist)
            {
                _allBatch.AddRange(item);
            }

            AddBatchMsg(ref _allBatch);//因为按照大构件批的概念重新排构件批，批次信息需在此修改，20231206

            return _allBatch;

        }
        /// <summary>
        /// 套料之后的所有原材list
        /// </summary>
        private List<RebarTaoLiao> m_rebarTaoliaoList = new List<RebarTaoLiao>();

        /// <summary>
        /// 梁板线创建工单，并且更新dgv绑定的datatable
        /// </summary>
        /// <param name="_dt"></param>
        /// <param name="_elementlist"></param>
        private void CreatWorkBillLB_FromElementList(ref DataTable _dt, WorkBillMsg wbMsg, List<ElementBatch> _elementlist)
        {
            try
            {
                //WorkBillMsg wbMsg = new WorkBillMsg();      //工单信息

                RebarTaoLiao _rebarTao = new RebarTaoLiao();

                foreach (var item in _elementlist)/*批次*/
                {
                    int _index_1 = _elementlist.IndexOf(item);
                    item.totalBatch = _elementlist.Count;
                    item.curBatch = _index_1 + 1;//注意此处要先保存index，否则修改了totalbatch后再找index就会出错,20240920

                    //根据每个批次的直径包含种类，配置套丝机直径规格设置
                    string _taosiSet = "";
                    foreach (var ttt in item.diameterList)//取直径
                    {
                        _taosiSet += ttt.ToString().Substring(6, 2) + "_";
                    }
                    _taosiSet.TrimEnd('_');//去除尾部的"_"

                    foreach (var eee in item.childBatchList)       /*子批次，不同直径*/
                    {
                        int _index_2 = item.childBatchList.IndexOf(eee);
                        eee.totalChildBatch = item.childBatchList.Count;
                        eee.curChildBatch = _index_2 + 1;//注意此处要先保存index，否则修改了totalbatch后再找index就会出错,20240920

                        _rebarTao = new RebarTaoLiao();//套料后的

                        int totalProductlength = 0;
                        /*长度套料后生成的钢筋原材list*/
                        List<RebarOri> _orilist = Algorithm.Taoliao(eee._list, out totalProductlength);//套料时顺便算一下成品总长度

                        if (_orilist.Count != 0) { GeneralClass.interactivityData?.printlog(1, "套料后，原材使用量为0，结果异常！ "); }

                        int rrr = _orilist.Sum(t => t._totalLength);
                        if (rrr < totalProductlength) { GeneralClass.interactivityData?.printlog(1, "套料后，原材总长小于成品总长，翻样长度异常 "); }//原材总长小于成品总长，这是异常情况

                        double _yuliao, _feiliao = 0;


                        foreach (var aaa in _orilist)
                        {

                            wbMsg.originLength = aaa._totalLength/*GeneralClass.OriginalLength*/;
                            wbMsg.taosiSetting = _taosiSet;
                            wbMsg.totalOriginal = _orilist.Count;//总的原材根数
                            wbMsg.curOriginal = _orilist.IndexOf(aaa);//当前的原材流水号

                            //foreach (var bbb in aaa._list)//修改aaa的所有批次信息.20240919
                            //{
                            //    BatchMsg _msg = new BatchMsg();
                            //    _msg.totalBatch = item.totalBatch;
                            //    _msg.curBatch = item.curBatch;
                            //    _msg.totalchildBatch = eee.totalChildBatch;
                            //    _msg.curChildBatch = eee.curChildBatch;
                            //    bbb._batchMsg = _msg;
                            //}

                            string jsonstr = GeneralClass.WorkBillOpt.CreateWorkBill_LB_RebarOri(wbMsg, aaa);
                            GeneralClass.jsonList_LB.Add(jsonstr);

                            //_yuliao = aaa._lengthFirstLeft;//计算余料
                            _yuliao = aaa._lengthSecondLeft;//计算余料,把1500可以用的余料长度也排除
                            _feiliao += (_yuliao < 300) ? _yuliao : 0;//短于300的余料当作废料

                            _rebarTao._rebarOriList.Add(aaa);
                        }
                        //更新dgv11
                        string diameterStr = "";
                        if (item./*elementData[0].*/diameterList.Count > 4)
                        {
                            diameterStr = GeneralClass.m_DiaType[(int)EnumDiaGroupType.MULTI];
                            _rebarTao.DiameterType = EnumDiaGroupType.MULTI;
                        }
                        else
                        {
                            if (GeneralClass.CfgData.DiaGroupType == EnumDiaGroupTypeSetting.Sequence)
                            {
                                switch (item.diameterList.Count)
                                {
                                    case 1:
                                        diameterStr = GeneralClass.m_DiaType[(int)EnumDiaGroupType.ONE];
                                        _rebarTao.DiameterType = EnumDiaGroupType.ONE;
                                        break;
                                    case 2:
                                        diameterStr = GeneralClass.m_DiaType[(int)EnumDiaGroupType.TWO];
                                        _rebarTao.DiameterType = EnumDiaGroupType.TWO;
                                        break;
                                    case 3:
                                        diameterStr = GeneralClass.m_DiaType[(int)EnumDiaGroupType.THREE];
                                        _rebarTao.DiameterType = EnumDiaGroupType.THREE;
                                        break;
                                    case 4:
                                        diameterStr = GeneralClass.m_DiaType[(int)EnumDiaGroupType.FOUR];
                                        _rebarTao.DiameterType = EnumDiaGroupType.FOUR;
                                        break;
                                }
                            }
                            else
                            {
                                diameterStr = GeneralClass.m_DiaType[(int)EnumDiaGroupType.FEW];
                                _rebarTao.DiameterType = EnumDiaGroupType.FEW;

                            }
                        }

                        totalProductlength += _orilist.Sum(t => t._lengthSecondUsed);//把二次利用的部分也加入成品总长，20240321

                        //直径种类，仓位，批次，直径，总数量，总长度，废料，利用率
                        _dt.Rows.Add(
                            diameterStr,
                            //GeneralClass.wareNum[(int)item.numGroup],
                            //_elementlist[i].IndexOf(item),
                            item.curBatch,//批次
                            "Φ" + eee._diameter.ToString().Substring(6, 2),//直径
                            _orilist.Count,//原材数量
                            eee._list.Sum(t => t.TotalPieceNum),
                            eee._list.Sum(t => t.TotalWeight),
                            (double)totalProductlength / (double)(_orilist.Sum(t => t._totalLength)) /*(double)(_newlist.Count * GeneralClass.OriginalLength)*/,
                            _feiliao / (double)(_orilist.Sum(t => t._totalLength)),
                            (double)(_orilist.Sum(t => t._totalLength)) / 1000,
                            (double)totalProductlength / 1000,
                            _feiliao / 1000
                            );

                        //更新套料后的rebarlist
                        _rebarTao.WareNumType = item.numGroup;
                        _rebarTao.BatchNo = /*_elementlist[i].IndexOf(item)*/item.curBatch;
                        _rebarTao.Diameter = Convert.ToInt32(eee._diameter.ToString().Substring(6, 2));

                        m_rebarTaoliaoList.Add(_rebarTao);

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("CreatWorkBillLB_FromElementList error :" + ex.Message);
            }


        }

        /// <summary>
        /// 工单datatable
        /// </summary>
        //private static DataTable dt_wb = new DataTable();
        private void FillDGV_WorkBill()
        {
            DataTable dt_wb = new DataTable();
            dt_wb.Columns.Add("类型", typeof(string));
            dt_wb.Columns.Add("仓位", typeof(int));
            dt_wb.Columns.Add("批次", typeof(int));
            dt_wb.Columns.Add("直径", typeof(string));
            dt_wb.Columns.Add("原材(根)", typeof(int));

            dt_wb.Columns.Add("废料率(%)", typeof(double));

            dt_wb.Columns.Add("一次利用率(%)", typeof(double));
            dt_wb.Columns.Add("原材长(m)", typeof(double));
            dt_wb.Columns.Add("成品长(m)", typeof(double));
            dt_wb.Columns.Add("废料(m)", typeof(double));



            dataGridView11.DataSource = dt_wb;
            //dataGridView11.Columns[1].DefaultCellStyle.Format = "0.000";        //
            //dataGridView11.Columns[2].DefaultCellStyle.Format = "0.0";        //
            //dataGridView11.Columns[3].DefaultCellStyle.Format = "0.0";          //
            dataGridView11.Columns[5].DefaultCellStyle.Format = "P1";          //废料率
            dataGridView11.Columns[6].DefaultCellStyle.Format = "P1";          //利用率
        }
        /// <summary>
        /// 梁板线手动分组的加工批list
        /// </summary>
        private List<ElementBatch> m_ManualBatchList = new List<ElementBatch>();
        /// <summary>
        /// 从手动分组UI获取到batchlist
        /// </summary>
        /// <param name="batchList"></param>
        private void GetManualBatchList(List<ElementBatch> batchList)
        {
            this.m_ManualBatchList.Clear();
            this.m_ManualBatchList.AddRange(batchList);
        }
        /// <summary>
        /// 从筛选项中返回pick的状态，依次是：棒材、线材、箍筋、拉钩、马凳、端头、主筋
        /// </summary>
        /// <param name="_pickstatus"></param>
        private void AskForPickStatus(out Tuple<bool, bool, bool, bool, bool, bool, bool> _pickstatus)
        {
            _pickstatus = new Tuple<bool, bool, bool, bool, bool, bool, bool>(
                checkBox7.Checked, checkBox8.Checked, checkBox9.Checked, checkBox10.Checked, checkBox11.Checked, checkBox13.Checked, checkBox12.Checked);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //if (_multiWorklist[0].Count == 0 && _oneWorklist[0].Count == 0 && _twoWorklist[0].Count == 0 && _threeWorklist[0].Count == 0 && _fourWorklist[0].Count == 0)

            //if (_multiWorklist[0].Count == 0
            //    && (GeneralClass.CfgData.DiaGroupType == EnumDiaGroupType.Sequence ?
            //    (_oneWorklist[0].Count == 0 && _twoWorklist[0].Count == 0 && _threeWorklist[0].Count == 0 && _fourWorklist[0].Count == 0)
            //    : (_fewWorklist[0].Count == 0))
            //    )
            //{
            //    MessageBox.Show("工单数据未准备好，请先点击【组合匹配】");
            //    return;
            //}

            try
            {
                InitDataGridView11();//先清空工单表

                GeneralClass.interactivityData?.printlog(1, "开始长度套料排程");
                GeneralClass.AddDefaultMaterial();//先添加默认的原材库

                DataTable dt_wb = new DataTable();
                dt_wb.Columns.Add(GeneralClass.CfgData.LanguageType == EnumLanguageType.Chinese ? "类型" : "Type", typeof(string));
                //dt_wb.Columns.Add("仓位", typeof(int));
                dt_wb.Columns.Add(GeneralClass.CfgData.LanguageType == EnumLanguageType.Chinese ? "批次" : "Batch", typeof(int));
                dt_wb.Columns.Add(GeneralClass.CfgData.LanguageType == EnumLanguageType.Chinese ? "直径" : "Diameter", typeof(string));
                dt_wb.Columns.Add(GeneralClass.CfgData.LanguageType == EnumLanguageType.Chinese ? "原材(根)" : "Raw num(piece)", typeof(int));

                dt_wb.Columns.Add(GeneralClass.CfgData.LanguageType == EnumLanguageType.Chinese ? "成品(根)" : "Product num(piece)", typeof(int));
                dt_wb.Columns.Add(GeneralClass.CfgData.LanguageType == EnumLanguageType.Chinese ? "总重量(kg)" : "Weight(Kg)", typeof(int));

                dt_wb.Columns.Add(GeneralClass.CfgData.LanguageType == EnumLanguageType.Chinese ? "一次利用率(%)" : "Used rate(%)", typeof(double));
                dt_wb.Columns.Add(GeneralClass.CfgData.LanguageType == EnumLanguageType.Chinese ? "废料率(%)" : "Scrape rate(%)", typeof(double));

                dt_wb.Columns.Add(GeneralClass.CfgData.LanguageType == EnumLanguageType.Chinese ? "原材长(m)" : "Raw length(m)", typeof(double));
                dt_wb.Columns.Add(GeneralClass.CfgData.LanguageType == EnumLanguageType.Chinese ? "成品长(m)" : "Product length(m)", typeof(double));
                dt_wb.Columns.Add(GeneralClass.CfgData.LanguageType == EnumLanguageType.Chinese ? "废料(m)" : "Scrape(m)", typeof(double));

                GeneralClass.jsonList_LB.Clear();
                GeneralClass.json_CX = "";
                m_rebarTaoliaoList.Clear();

                //构件批分组排程
                List<ElementBatch> _allbatch = new List<ElementBatch>();
                if (radioButton1.Checked)
                {
                    _allbatch = AutoSortBatchAndGroup();
                    DBOpt.SaveAllElementBatchToDB(_allbatch);
                }
                else
                {
                    _allbatch = ManualSortBatchAndGroup();
                    DBOpt.SaveAllElementBatchToDB(_allbatch);

                }


                WorkBillMsg wbMsg = new WorkBillMsg();
                wbMsg.shift = 1;
                wbMsg.projectName = "";//项目，光谷国际社区
                wbMsg.block = "A";
                wbMsg.building = "06D";
                wbMsg.floor = "01F";
                wbMsg.level = "";//钢筋级别,C
                wbMsg.brand = "";//厂商,鄂钢
                wbMsg.specification = "";//规格型号,HRB400
                wbMsg.tasklistNo = _allbatch[0].childBatchList[0]._list[0].TableNo;//料单编号
                wbMsg.tasklistName = _allbatch[0].childBatchList[0]._list[0].TableName;//料单名称
                CreatWorkBillLB_FromElementList(ref dt_wb, wbMsg, _allbatch);

                GeneralClass.interactivityData?.printlog(1, "创建工单完成");

                dataGridView11.DataSource = dt_wb;
                //dataGridView11.Columns[1].DefaultCellStyle.Format = "0.000";        //
                //dataGridView11.Columns[4].DefaultCellStyle.Format = "0.0";        //
                dataGridView11.Columns[5].DefaultCellStyle.Format = "0.00";          //
                dataGridView11.Columns[6].DefaultCellStyle.Format = "P1";          //废料率
                dataGridView11.Columns[7].DefaultCellStyle.Format = "P1";          //利用率

                string s_rawnum = GeneralClass.CfgData.LanguageType == EnumLanguageType.Chinese ? "原材(根)" : "Raw num(piece)";
                string s_rawlength = GeneralClass.CfgData.LanguageType == EnumLanguageType.Chinese ? "原材长(m)" : "Raw length(m)";
                string s_productlength = GeneralClass.CfgData.LanguageType == EnumLanguageType.Chinese ? "成品长(m)" : "Product length(m)";
                string s_scrape = GeneralClass.CfgData.LanguageType == EnumLanguageType.Chinese ? "废料(m)" : "Scrape(m)";

                //int totalpiece = Convert.ToInt32(dt_wb.Compute("sum([原材(根)])", ""));
                //double totalOriLength = Convert.ToDouble(dt_wb.Compute("sum([原材长(m)])", ""));
                //double totallength = Convert.ToDouble(dt_wb.Compute("sum([成品长(m)])", ""));
                //double _feiliao = Convert.ToDouble(dt_wb.Compute("sum([废料(m)])", ""));
                int totalpiece = dt_wb.AsEnumerable().Sum(row => row.Field<int>(s_rawnum));
                double totalOriLength = dt_wb.AsEnumerable().Sum(row => row.Field<double>(s_rawlength));
                double totallength = dt_wb.AsEnumerable().Sum(row => row.Field<double>(s_productlength));
                double _feiliao = dt_wb.AsEnumerable().Sum(row => row.Field<double>(s_scrape));

                int _chengpin = 0;
                foreach (var item in m_rebarTaoliaoList)
                {
                    foreach (var ttt in item._rebarOriList)
                    {
                        _chengpin += ttt._list.Count;
                    }
                }
                textBox2.Text = totalpiece.ToString();
                //textBox1.Text = totallength.ToString("F2");
                textBox1.Text = _chengpin.ToString();
                textBox3.Text = (totallength / totalOriLength /*((double)GeneralClass.OriginalLength / 1000 * (double)totalpiece)*/      ).ToString("P2");
                textBox4.Text = (_feiliao / totalOriLength /*((double)GeneralClass.OriginalLength / 1000 * (double)totalpiece)*/    ).ToString("P2");

                //GeneralClass.interactivityData?.sendworkbill(GeneralClass.jsonList, 100);

                //根据套料结果，统计原材库所需原材种类
                List<RebarOri> _allOriList = new List<RebarOri>();
                foreach (var item in m_rebarTaoliaoList)
                {
                    _allOriList.AddRange(item._rebarOriList);
                }
                GeneralClass.json_CX = GeneralClass.WorkBillOpt.CreateWorkBill_CX(wbMsg, _allOriList);//生成磁吸上料的json
                MessageBox.Show("梁板线工单、磁吸上料工单已生成！");
                GeneralClass.interactivityData?.printlog(1, "梁板线工单、磁吸上料工单已生成！");

                FillDGV_AllMaterialPool(_allOriList);
                checkBox1.Checked = true;
                checkBox2.Checked = true;

                //根据套料结果，统计二次利用的情况
                FillDGV_SecondUsed(_allOriList);

            }
            catch (Exception ex) { MessageBox.Show("长度套料 error:" + ex.Message); }

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
        /// 单直径种类构件包分组后的工单
        /// </summary>
        //private static List<List<ElementRebarFB>>[] _oneWorklist = new List<List<ElementRebarFB>>[(int)EnumWareNumGroup.maxNum] {
        //    new List<List<ElementRebarFB>>(),
        //    new List<List<ElementRebarFB>>(),
        //    new List<List<ElementRebarFB>>(),
        //    new List<List<ElementRebarFB>>()};//分组后的构件包，按照数量仓位分组
        private static List<ElementBatch>[] _oneWorklist = new List<ElementBatch>[(int)EnumWareNumSet.maxNum] {
            new List<ElementBatch>(),
            new List<ElementBatch>(),
            new List<ElementBatch>(),
            new List<ElementBatch>(),
            new List<ElementBatch>()};//分组后的构件包，按照数量仓位分组

        /// <summary>
        /// 两直径种类构件包分组后的工单
        /// </summary>
        //private static List<List<ElementRebarFB>>[] _twoWorklist = new List<List<ElementRebarFB>>[(int)EnumWareNumGroup.maxNum] {
        //    new List<List<ElementRebarFB>>(),
        //    new List<List<ElementRebarFB>>(),
        //    new List<List<ElementRebarFB>>(),
        //    new List<List<ElementRebarFB>>()};//分组后的构件包，按照数量仓位分组
        private static List<ElementBatch>[] _twoWorklist = new List<ElementBatch>[(int)EnumWareNumSet.maxNum] {
            new List<ElementBatch>(),
            new List<ElementBatch>(),
            new List<ElementBatch>(),
            new List<ElementBatch>(),
        new List<ElementBatch>()};//分组后的构件包，按照数量仓位分组

        /// <summary>
        /// 三直径种类构件包分组后的工单，分三个维度：[]维度代表仓位区间，list(外)维度代表直径种类，List(内)维度代表所包含的构件包list
        /// </summary>
        //private static List<List<ElementRebarFB>>[] _threeWorklist = new List<List<ElementRebarFB>>[(int)EnumWareNumGroup.maxNum] {
        //    new List<List<ElementRebarFB>>(),
        //    new List<List<ElementRebarFB>>(),
        //    new List<List<ElementRebarFB>>(),
        //    new List<List<ElementRebarFB>>()};//分组后的构件包，按照数量仓位分组
        private static List<ElementBatch>[] _threeWorklist = new List<ElementBatch>[(int)EnumWareNumSet.maxNum] {
            new List<ElementBatch>(),
            new List<ElementBatch>(),
            new List<ElementBatch>(),
            new List<ElementBatch>(), new List < ElementBatch >()};//分组后的构件包，按照数量仓位分组

        /// <summary>
        /// 四直径种类构件包分组后的工单，分三个维度：[]维度代表仓位区间，list(外)维度代表直径种类，List(内)维度代表所包含的构件包list
        /// </summary>
        //private static List<List<ElementRebarFB>>[] _fourWorklist = new List<List<ElementRebarFB>>[(int)EnumWareNumGroup.maxNum] {
        //    new List<List<ElementRebarFB>>(),
        //    new List<List<ElementRebarFB>>(),
        //    new List<List<ElementRebarFB>>(),
        //    new List<List<ElementRebarFB>>()};//分组后的构件包，按照数量仓位分组
        private static List<ElementBatch>[] _fourWorklist = new List<ElementBatch>[(int)EnumWareNumSet.maxNum] {
            new List<ElementBatch>(),
            new List<ElementBatch>(),
            new List<ElementBatch>(),
            new List<ElementBatch>(), new List < ElementBatch >()};//分组后的构件包，按照数量仓位分组

        /// <summary>
        /// 5种直径及以上的构件包分组后的工单，分两个维度：[]维度代表仓位区间，list维度代表构件批的list
        /// </summary>
        private static List<ElementBatch>[] _multiWorklist = new List<ElementBatch>[(int)EnumWareNumSet.maxNum] {
            new List<ElementBatch>(),
            new List<ElementBatch>(),
            new List<ElementBatch>(),
            new List<ElementBatch>(), new List < ElementBatch >()};//分组后的构件包，按照数量仓位分组
        //private static List<List<ElementDataFB>>[] _multiWorklist = new List<List<ElementDataFB>>[(int)EnumWareNumGroup.maxNum] {
        //    new List<List<ElementDataFB>>(),
        //    new List<List<ElementDataFB>>(),
        //    new List<List<ElementDataFB>>(),
        //    new List<List<ElementDataFB>>()};//分组后的构件包，按照数量仓位分组

        /// <summary>
        /// 1~4种直径的构件包分组后的工单，分两个维度：[]维度代表仓位区间，list维度代表构件批的list
        /// </summary>
        private static List<ElementBatch>[] _fewWorklist = new List<ElementBatch>[(int)EnumWareNumSet.maxNum] {
            new List<ElementBatch>(),
            new List<ElementBatch>(),
            new List<ElementBatch>(),
            new List<ElementBatch>(), new List < ElementBatch >()};//分组后的构件包，按照数量仓位分组
        //private static List<List<ElementDataFB>>[] _fewWorklist = new List<List<ElementDataFB>>[(int)EnumWareNumGroup.maxNum] {
        //    new List<List<ElementDataFB>>(),
        //    new List<List<ElementDataFB>>(),
        //    new List<List<ElementDataFB>>(),
        //    new List<List<ElementDataFB>>()};//分组后的构件包，按照数量仓位分组

        private static List<ElementRebarFB> _oneShowlist = new List<ElementRebarFB>();
        private static List<ElementRebarFB> _twoShowlist = new List<ElementRebarFB>();
        private static List<ElementRebarFB> _threeShowlist = new List<ElementRebarFB>();
        private static List<ElementRebarFB> _fourShowlist = new List<ElementRebarFB>();
        private static List<ElementRebarFB> _multiShowlist = new List<ElementRebarFB>();
        private static List<ElementRebarFB> _fewShowlist = new List<ElementRebarFB>();


        private void ComboboxPickDia()
        {

        }
        /// <summary>
        /// 从界面获取料单选择状态，依次为：棒材、线材、箍筋、拉钩、马凳、端头、主筋
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        private List<RebarData> Pick(List<RebarData> _data)
        {
            List<RebarData> _ret = new List<RebarData>();

            //从form3界面获取料单选择状态，依次为：棒材、线材、箍筋、拉钩、马凳、端头、主筋
            Tuple<bool, bool, bool, bool, bool, bool, bool> _sts = new Tuple<bool, bool, bool, bool, bool, bool, bool>(false, false, false, false, false, false, false);//init
            //GeneralClass.interactivityData?.askForPickStatus(out _sts);
            AskForPickStatus(out _sts);

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
                       &&
                       (comboBox1.SelectedIndex == 0 ? true : t.Diameter == GeneralClass.EnumDiameterToInt((EnumDiaBang)(comboBox1.SelectedIndex - 1)))
            &&
                       (GeneralClass.CfgData.IfOrignalTao ? true : t.iLength != GeneralClass.OriginalLength(t.Level, t.Diameter))//20241010增加原材是否参与套料的功能
                ).ToList();

            return _ret;
        }
        /// <summary>
        /// 从界面获取料单选择状态，依次为：棒材、线材、箍筋、拉钩、马凳、端头、主筋
        /// </summary>
        /// <param name="_elementlist"></param>
        /// <returns></returns>
        private List<ElementRebarFB> PickElementList(List<ElementData> _elementlist)
        {
            List<ElementRebarFB> _ret = new List<ElementRebarFB>();

            ElementData elementAfterPick;//经过条件筛选后的构件

            List<RebarData> temp = new List<RebarData>();
            foreach (var item in _elementlist)
            {
                elementAfterPick = new ElementData();
                elementAfterPick.Copy(item);

                temp = Pick(item.rebarlist);
                elementAfterPick.rebarlist.Clear();
                foreach (var ttt in temp)//复制筛选后的rebarlist
                {
                    elementAfterPick.rebarlist.Add(ttt);
                }

                _ret.Add(elementAfterPick.elementDataFB);
            }
            return _ret;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {

                InitDataGridView();
                InitTreeView();

                GeneralClass.interactivityData?.printlog(1, "开始进行构件包匹配");

                GeneralClass.AllElementList = GeneralClass.DBOpt.GetAllElementList(GeneralClass.TableName_AllRebar);

                List<ElementRebarFB> _fblist = new List<ElementRebarFB>();

                _fblist = PickElementList(GeneralClass.AllElementList);//根据筛选项，获取待加工数据
                //foreach (var item in GeneralClass.AllElementList)
                //{
                //    _fblist.Add(item.elementDataFB);//单独存入构件包非标list
                //}




                //行：直径种类，1种，2种，3种，4种，5种，。。。10种
                //列：数量仓位,EIGHT:1~10(8仓)，FOUR:11~50(4仓)，TWO:51~100(2仓)，ONE:100~(1仓)
                List<ElementRebarFB>[,] fb_diameterGroup = new List<ElementRebarFB>[(int)EnumDiaBang.maxRebarBangNum, (int)EnumWareNumSet.maxNum];//待分组的构件包

                //分组
                for (int i = 0; i < (int)EnumDiaBang.maxRebarBangNum; i++)
                {
                    for (int j = 0; j < (int)EnumWareNumSet.maxNum; j++)
                    {
                        fb_diameterGroup[i, j] = _fblist.Where(t => t.diameterType == i + 1 && t.wareNumSet == (EnumWareNumSet)j).ToList();
                        //按照直径种类字符串进行排序，这步很重要
                        fb_diameterGroup[i, j] = fb_diameterGroup[i, j].OrderBy(t => t.diameterStr).ToList();
                    }
                }

                //清空数据先
                foreach (var item in _multiWorklist)
                { item.Clear(); }
                if (GeneralClass.CfgData.DiaGroupType == EnumDiaGroupTypeSetting.Sequence)//顺序分组
                {
                    foreach (var item in _oneWorklist)
                    { item.Clear(); }
                    foreach (var item in _twoWorklist)
                    { item.Clear(); }
                    foreach (var item in _threeWorklist)
                    { item.Clear(); }
                    foreach (var item in _fourWorklist)
                    { item.Clear(); }
                }
                else
                {
                    foreach (var item in _fewWorklist)//混合分组
                    { item.Clear(); }
                }



                #region 先处理多直径种类的     ,20241024关闭，区分直径种类超过4种的已经没有意义           
                ////先处理多直径种类的
                //for (int i = (int)EnumDiaBang.maxRebarBangNum - 1; i > 3; i--)//倒序，从直径种类多的开始分组,
                //{
                //    for (int j = 0; j < (int)EnumWareNumSet.maxNum; j++)
                //    {
                //        if (fb_diameterGroup[i, j].Count == 0) continue;//直径种类太多的一般没有，例如5种以上的

                //        for (int k = 0; k < fb_diameterGroup[i, j].Count; k++)
                //        {
                //            if (_multiWorklist[j].Count == 0)//刚开始没有元素
                //            {
                //                ElementBatch _batch = new ElementBatch();
                //                _batch.elementData.Add(fb_diameterGroup[i, j][0]);//将第一个构件包存入，建立第一个小批次
                //                _multiWorklist[j].Add(_batch);

                //            }
                //            else
                //            {
                //                bool bUse = false;
                //                for (int m = 0; m < _multiWorklist[j].Count; m++)//是否能够匹配现有的小批次
                //                {
                //                    //条件一：至少包含一种直径，条件二：螺距类型一致，条件三：仓位未满
                //                    if (fb_diameterGroup[i, j][k].IfIncludeby(_multiWorklist[j][m].elementData[0], GeneralClass.m_inclueNum) &&
                //                        _multiWorklist[j][m].elementData.Count < GeneralClass.wareNum[j] &&
                //                      (GeneralClass.m_checkPitchType ? (fb_diameterGroup[i, j][k].diameterPitchType == _multiWorklist[j][m].elementData[0].diameterPitchType) : true))//根据直径种类的包含关系分组，同时考虑仓位是否满仓
                //                    {
                //                        _multiWorklist[j][m].elementData.Add(fb_diameterGroup[i, j][k]);
                //                        bUse = true;
                //                        break;
                //                    }
                //                }
                //                if (!bUse)//如果匹配不了现有的批次，则建立新的小批次
                //                {
                //                    //List<ElementDataFB> temp = new List<ElementDataFB> { fb_diameterGroup[i, j][k] };
                //                    //_multiWorklist[j].Add(temp);

                //                    ElementBatch _batch = new ElementBatch();
                //                    _batch.elementData.Add(fb_diameterGroup[i, j][k]);//
                //                    _multiWorklist[j].Add(_batch);

                //                }
                //            }
                //        }
                //    }
                //}
                #endregion

                if (GeneralClass.CfgData.DiaGroupType == EnumDiaGroupTypeSetting.Sequence)//顺序分组
                {
                    #region 再处理单直径的
                    for (int j = 0; j < (int)EnumWareNumSet.maxNum; j++)
                    {
                        for (int k = 0; k < fb_diameterGroup[0, j].Count; k++)//0为1种直径的
                        {
                            if (_oneWorklist[j].Count == 0)//刚开始没有元素
                            {
                                ElementBatch temp = new ElementBatch();
                                temp.elementData.Add(fb_diameterGroup[0, j][0]);
                                _oneWorklist[j].Add(temp);//将第一个构件包存入，建立第一个小批次
                            }
                            else
                            {
                                bool bUse = false;
                                for (int m = 0; m < _oneWorklist[j].Count; m++)//是否能够匹配现有的小批次
                                {
                                    //条件一：直径一致，条件二：螺距类型一致，条件三：仓位未满
                                    if (fb_diameterGroup[0, j][k].IfIncludeby(_oneWorklist[j][m].diameterList)
                                        && _oneWorklist[j][m].elementData.Count < GeneralClass.wareNum[j]
                                       /*&&(GeneralClass.m_checkPitchType ? (fb_diameterGroup[0, j][k].diameterPitchType == _oneWorklist[j][m][0].diameterPitchType) : true)*/)//根据直径种类的包含关系分组，同时考虑仓位是否满仓
                                    {
                                        _oneWorklist[j][m].elementData.Add(fb_diameterGroup[0, j][k]);
                                        bUse = true;
                                    }
                                }
                                if (!bUse)//如果匹配不了现有的批次，则建立新的小批次
                                {
                                    ElementBatch temp = new ElementBatch();
                                    temp.elementData.Add(fb_diameterGroup[0, j][k]);
                                    _oneWorklist[j].Add(temp);
                                }
                            }
                        }
                    }
                    #endregion

                    #region 再处理两种直径的
                    for (int j = 0; j < (int)EnumWareNumSet.maxNum; j++)
                    {


                        for (int k = 0; k < fb_diameterGroup[1, j].Count; k++)//1为2种直径的
                        {
                            if (_twoWorklist[j].Count == 0)//刚开始没有元素
                            {
                                ElementBatch temp = new ElementBatch();
                                temp.elementData.Add(fb_diameterGroup[1, j][0]);
                                _twoWorklist[j].Add(temp);//将第一个构件包存入，建立第一个小批次
                            }
                            else
                            {
                                bool bUse = false;
                                for (int m = 0; m < _twoWorklist[j].Count; m++)//是否能够匹配现有的小批次
                                {
                                    //条件一：至少包含一种直径，条件二：螺距类型一致，条件三：仓位未满
                                    if (fb_diameterGroup[1, j][k].IfIncludeby(_twoWorklist[j][m].diameterList)
                                        && _twoWorklist[j][m].elementData.Count < GeneralClass.wareNum[j]
                                       /*&&(GeneralClass.m_checkPitchType ? (fb_diameterGroup[1, j][k].diameterPitchType == _twoWorklist[j][m][0].diameterPitchType) : true)*/)//根据直径种类的包含关系分组，同时考虑仓位是否满仓
                                    {
                                        _twoWorklist[j][m].elementData.Add(fb_diameterGroup[1, j][k]);
                                        bUse = true;
                                    }
                                }
                                if (!bUse)//如果匹配不了现有的批次，则建立新的小批次
                                {
                                    //List<ElementRebarFB> temp = new List<ElementRebarFB> { fb_diameterGroup[1, j][k] };
                                    //_twoWorklist[j].Add(temp);

                                    ElementBatch temp = new ElementBatch();
                                    temp.elementData.Add(fb_diameterGroup[1, j][k]);
                                    _twoWorklist[j].Add(temp);//将第一个构件包存入，建立第一个小批次

                                }
                            }
                        }
                    }
                    #endregion

                    #region 再处理三种直径的
                    for (int j = 0; j < (int)EnumWareNumSet.maxNum; j++)
                    {

                        for (int k = 0; k < fb_diameterGroup[2, j].Count; k++)//2为3种直径的
                        {
                            if (_threeWorklist[j].Count == 0)//刚开始没有元素
                            {
                                //List<ElementRebarFB> temp = new List<ElementRebarFB> { fb_diameterGroup[2, j][0] };
                                //_threeWorklist[j].Add(temp);//将第一个构件包存入，建立第一个小批次

                                ElementBatch temp = new ElementBatch();
                                temp.elementData.Add(fb_diameterGroup[2, j][0]);
                                _threeWorklist[j].Add(temp);//将第一个构件包存入，建立第一个小批次

                            }
                            else
                            {
                                bool bUse = false;
                                for (int m = 0; m < _threeWorklist[j].Count; m++)//是否能够匹配现有的小批次
                                {
                                    //条件一：至少包含一种直径，条件二：螺距类型一致，条件三：仓位未满
                                    if (fb_diameterGroup[2, j][k].IfIncludeby(_threeWorklist[j][m].diameterList)
                                        && _threeWorklist[j][m].elementData.Count < GeneralClass.wareNum[j]
                                       /*&&(GeneralClass.m_checkPitchType ? (fb_diameterGroup[2, j][k].diameterPitchType == _threeWorklist[j][m][0].diameterPitchType) : true)*/)//根据直径种类的包含关系分组，同时考虑仓位是否满仓
                                    {
                                        _threeWorklist[j][m].elementData.Add(fb_diameterGroup[2, j][k]);
                                        bUse = true;
                                    }
                                }
                                if (!bUse)//如果匹配不了现有的批次，则建立新的小批次
                                {
                                    //List<ElementRebarFB> temp = new List<ElementRebarFB> { fb_diameterGroup[2, j][k] };
                                    //_threeWorklist[j].Add(temp);

                                    ElementBatch temp = new ElementBatch();
                                    temp.elementData.Add(fb_diameterGroup[2, j][k]);
                                    _threeWorklist[j].Add(temp);//将第一个构件包存入，建立第一个小批次

                                }
                            }
                        }
                    }
                    #endregion

                    #region 再处理四种直径的
                    for (int j = 0; j < (int)EnumWareNumSet.maxNum; j++)
                    {

                        for (int k = 0; k < fb_diameterGroup[3, j].Count; k++)//3为4种直径的
                        {
                            if (_fourWorklist[j].Count == 0)//刚开始没有元素
                            {
                                //List<ElementRebarFB> temp = new List<ElementRebarFB> { fb_diameterGroup[3, j][0] };
                                //_fourWorklist[j].Add(temp);//将第一个构件包存入，建立第一个小批次

                                ElementBatch temp = new ElementBatch();
                                temp.elementData.Add(fb_diameterGroup[3, j][0]);
                                _fourWorklist[j].Add(temp);//将第一个构件包存入，建立第一个小批次

                            }
                            else
                            {
                                bool bUse = false;
                                for (int m = 0; m < _fourWorklist[j].Count; m++)//是否能够匹配现有的小批次
                                {
                                    //条件一：至少包含一种直径，条件二：螺距类型一致，条件三：仓位未满
                                    if (fb_diameterGroup[3, j][k].IfIncludeby(_fourWorklist[j][m].diameterList)
                                        && _fourWorklist[j][m].elementData.Count < GeneralClass.wareNum[j]
                                       /*&&(GeneralClass.m_checkPitchType ? (fb_diameterGroup[3, j][k].diameterPitchType == _fourWorklist[j][m][0].diameterPitchType) : true)*/)//根据直径种类的包含关系分组，同时考虑仓位是否满仓
                                    {
                                        _fourWorklist[j][m].elementData.Add(fb_diameterGroup[3, j][k]);
                                        bUse = true;
                                    }
                                }
                                if (!bUse)//如果匹配不了现有的批次，则建立新的小批次
                                {
                                    //List<ElementRebarFB> temp = new List<ElementRebarFB> { fb_diameterGroup[3, j][k] };
                                    //_fourWorklist[j].Add(temp);
                                    ElementBatch temp = new ElementBatch();
                                    temp.elementData.Add(fb_diameterGroup[3, j][k]);
                                    _fourWorklist[j].Add(temp);//将第一个构件包存入，建立第一个小批次

                                }
                            }
                        }
                    }
                    #endregion

                }
                else
                {
                    #region 再处理1~4种直径种类的
                    //再处理1~4种直径的
                    //for (int i = 3; i >= 0; i--)//1~4种直径的，采用倒序，先排4种直径的
                    for (int i = (int)EnumDiaBang.maxRebarBangNum - 1; i >= 0; i--)//20241024修改，将多直径和1~4种直径的合并，之前区分多直径和1~4种直径无意义
                    {
                        for (int j = 0; j < (int)EnumWareNumSet.maxNum; j++)
                        {
                            if (fb_diameterGroup[i, j].Count == 0) continue;//直径种类太多的一般没有，例如5种以上的

                            for (int k = 0; k < fb_diameterGroup[i, j].Count; k++)
                            {
                                if (_fewWorklist[j].Count == 0)//刚开始没有元素
                                {
                                    ElementBatch _batch = new ElementBatch();
                                    _batch.elementData.Add(fb_diameterGroup[i, j][0]);//将第一个构件包存入，建立第一个小批次
                                    _fewWorklist[j].Add(_batch);
                                }
                                else
                                {
                                    bool bUse = false;
                                    for (int m = 0; m < _fewWorklist[j].Count; m++)//是否能够匹配现有的小批次，m为批次
                                    {
                                        //条件一：直径包含关系小于7种，条件二：直径包含关系从属于batch的直径种类，条件三：仓位未满
                                        if (/*fb_diameterGroup[i, j][k].IfIncludeUnderFour(_fewWorklist[j][m].elementData[0])*/
                                            fb_diameterGroup[i, j][k].IfIncludeUnderX(_fewWorklist[j][m].diameterList, 7) &&
                                           ((_fewWorklist[j][m].diameterList.Count == 7) ? fb_diameterGroup[i, j][k].IfIncludeby(_fewWorklist[j][m].diameterList) : true) &&
                                            _fewWorklist[j][m].elementData.Count < GeneralClass.wareNum[j])//根据直径种类的包含关系分组，同时考虑仓位是否满仓
                                        {
                                            _fewWorklist[j][m].elementData.Add(fb_diameterGroup[i, j][k]);
                                            bUse = true;
                                            break;//匹配上了，则break
                                        }
                                    }
                                    if (!bUse)//如果匹配不了现有的批次，则建立新的小批次
                                    {
                                        ElementBatch _batch = new ElementBatch();
                                        _batch.elementData.Add(fb_diameterGroup[i, j][k]);//
                                        _fewWorklist[j].Add(_batch);
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                }



                FillTreeView(_multiWorklist, ref treeView2);
                if (GeneralClass.CfgData.DiaGroupType == EnumDiaGroupTypeSetting.Sequence)
                {
                    FillTreeView(_oneWorklist, ref treeView3);
                    FillTreeView(_twoWorklist, ref treeView4);
                    FillTreeView(_threeWorklist, ref treeView1);
                    FillTreeView(_fourWorklist, ref treeView6);
                }
                else
                {
                    FillTreeView(_fewWorklist, ref treeView5);
                }

                if (GeneralClass.CfgData.DiaGroupType == EnumDiaGroupTypeSetting.Sequence)
                {
                    AddWareMsg(ref _oneWorklist);
                    AddWareMsg(ref _twoWorklist);
                    AddWareMsg(ref _threeWorklist);
                    AddWareMsg(ref _fourWorklist);
                }
                else
                {
                    AddWareMsg(ref _fewWorklist);
                }
                AddWareMsg(ref _multiWorklist);

                //AddBatchMsg(ref _oneWorklist, ref _twoWorklist, ref _threeWorklist, ref _fourWorklist, ref _multiWorklist);
                //AddBatchMsg(ref _fewWorklist, ref _multiWorklist);

                GeneralClass.interactivityData?.printlog(1, "构件包匹配完成");

            }

            catch (Exception ex) { MessageBox.Show("构件包组合 error:" + ex.Message); }

        }
        /// <summary>
        /// 有了大构件批的概念后，弱化数量分组，并以此进行批次的设定
        /// </summary>
        /// <param name="_batchlist"></param>
        private void AddBatchMsg(ref List<ElementBatch> _batchlist)
        {
            BatchMsg _msg = new BatchMsg();
            _msg.totalBatch = 0;
            _msg.curBatch = 0;
            _msg.totalchildBatch = 0;//子批次,按照直径来分
            _msg.curChildBatch = 0;

            int _totalbatch = _batchlist.Count;
            int _batchNo = 0;

            //按子批的概念，更新rebardata层级的信息
            foreach (var item in _batchlist)
            {
                _batchNo++;
                foreach (var iii in item.childBatchList)
                {
                    foreach (var ttt in iii._list)
                    {
                        _msg = new BatchMsg();
                        _msg.totalchildBatch = iii.totalChildBatch;
                        _msg.curChildBatch = iii.curChildBatch;
                        _msg.totalBatch = _totalbatch;
                        _msg.curBatch = _batchNo;
                        ttt._batchMsg = _msg;
                    }
                }

                //20240815新增，批次序列号及仓位信息
                item.BatchSeri = "A-" + DateTime.Now.ToString("yyyyMMdd") + "-" + _totalbatch.ToString("D3") + "-" + _batchNo.ToString("D3");//批次序列号

                foreach (ElementRebarFB _element in item.elementData)//
                {
                    int _warenumSet = GeneralClass.EnumWareSetToInt(_element.wareNumSet);//仓位设置
                    int _index = item.elementData.IndexOf(_element);//构件在批里面的序号

                    _element.warehouseNo = _index / _warenumSet + 1;//料仓号
                    _element.wareNo = _index % _warenumSet + 1;//仓位号
                    //_element.wareNumSet

                }
            }




        }


        private void AddBatchMsg(ref List<ElementBatch>[] _fewlist, ref List<ElementBatch>[] _multilist)
        {
            BatchMsg _msg = new BatchMsg();
            _msg.totalBatch = 0;
            _msg.curBatch = 0;
            _msg.totalchildBatch = 0;//子批次,按照直径来分
            _msg.curChildBatch = 0;

            int _totalbatch = 0;
            int _batchNo = 0;

            List<GroupbyDiaWithLength> _dlist = new List<GroupbyDiaWithLength>();

            //先统计总共多少个批次
            for (int i = 0; i < (int)EnumWareNumSet.maxNum; i++)/*仓*/
            {
                foreach (var item in _fewlist[i])/*批次*/
                {
                    _totalbatch++;
                }
            }
            for (int i = 0; i < (int)EnumWareNumSet.maxNum; i++)/*仓*/
            {
                foreach (var item in _multilist[i])/*批次*/
                {
                    _totalbatch++;
                }
            }

            //更新批次信息
            for (int i = 0; i < (int)EnumWareNumSet.maxNum; i++)/*仓*/
            {
                foreach (var item in _fewlist[i])/*批次*/
                {
                    _batchNo++;
                    //_msg.totalchildBatch = item.elementData.Max(t => t.diameterType);//直径种类的最大值为子批次总数
                    //_msg.totalchildBatch = item.diameterList.Count;//批次的直径种类为子批次总数


                    foreach (var iii in item.childBatchList)
                    {
                        //_msg.curChildBatch = item.childBatchList.IndexOf(iii);

                        foreach (var ttt in iii._list)
                        {
                            _msg = new BatchMsg();
                            _msg.totalchildBatch = iii.totalChildBatch;
                            _msg.curChildBatch = iii.curChildBatch;
                            _msg.totalBatch = _totalbatch;
                            _msg.curBatch = _batchNo;
                            ttt._batchMsg = _msg;
                        }
                    }
                    //foreach (var iii in item.elementData)/*构件包*/
                    //{
                    //    foreach (var ttt in iii.diameterGroup)/*直径分组*/
                    //    {
                    //        foreach (var eee in ttt._datalist)/*钢筋*/
                    //        {
                    //            _msg.totalBatch = _totalbatch;
                    //            _msg.curBatch = _batchNo;
                    //            eee.BatchMsg = _msg;
                    //        }
                    //    }
                    //}
                }
            }
            for (int i = 0; i < (int)EnumWareNumSet.maxNum; i++)/*仓*/
            {
                foreach (var item in _multilist[i])/*批次*/
                {
                    _batchNo++;
                    //_msg.totalchildBatch = item.elementData.Max(t => t.diameterType);//直径种类的最大值为子批次总数
                    //_msg.totalchildBatch = item.diameterList.Count;//批次的直径种类为子批次总数

                    foreach (var iii in item.childBatchList)
                    {
                        //_msg.curChildBatch = item.childBatchList.IndexOf(iii);

                        foreach (var ttt in iii._list)
                        {
                            _msg = new BatchMsg();
                            _msg.totalchildBatch = iii.totalChildBatch;
                            _msg.curChildBatch = iii.curChildBatch;
                            _msg.totalBatch = _totalbatch;
                            _msg.curBatch = _batchNo;
                            ttt._batchMsg = _msg;
                        }
                    }
                    //foreach (var iii in item.elementData)/*构件包*/
                    //{
                    //    foreach (var ttt in iii.diameterGroup)/*直径分组*/
                    //    {
                    //        foreach (var eee in ttt._datalist)/*钢筋*/
                    //        {
                    //            _msg.totalBatch = _totalbatch;
                    //            _msg.curBatch = _batchNo;
                    //            eee.BatchMsg = _msg;
                    //        }
                    //    }
                    //}
                }
            }

        }
        /// <summary>
        /// 添加批次号，需要将所有的worklist汇总进行批次排序
        /// </summary>
        private void AddBatchMsg(ref List<List<ElementRebarFB>>[] _onelist,
            ref List<List<ElementRebarFB>>[] _twolist,
            ref List<List<ElementRebarFB>>[] _threelist,
            ref List<List<ElementRebarFB>>[] _fourlist,
            ref List<List<ElementRebarFB>>[] _multilist)
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

            List<GroupbyDiaWithLength> _dlist = new List<GroupbyDiaWithLength>();

            //先统计总共多少个批次
            for (int i = 0; i < (int)EnumWareNumSet.maxNum; i++)/*仓*/
            {
                foreach (var item in _onelist[i])/*批次*/
                {
                    _totalbatch++;
                }
            }
            for (int i = 0; i < (int)EnumWareNumSet.maxNum; i++)/*仓*/
            {
                foreach (var item in _twolist[i])/*批次*/
                {
                    _totalbatch++;
                }
            }
            for (int i = 0; i < (int)EnumWareNumSet.maxNum; i++)/*仓*/
            {
                foreach (var item in _threelist[i])/*批次*/
                {
                    _totalbatch++;
                }
            }
            for (int i = 0; i < (int)EnumWareNumSet.maxNum; i++)/*仓*/
            {
                foreach (var item in _fourlist[i])/*批次*/
                {
                    _totalbatch++;
                }
            }
            for (int i = 0; i < (int)EnumWareNumSet.maxNum; i++)/*仓*/
            {
                foreach (var item in _multilist[i])/*批次*/
                {
                    _totalbatch++;
                }
            }

            //更新批次信息
            for (int i = 0; i < (int)EnumWareNumSet.maxNum; i++)/*仓*/
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
                                eee._batchMsg = _msg;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < (int)EnumWareNumSet.maxNum; i++)/*仓*/
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
                                eee._batchMsg = _msg;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < (int)EnumWareNumSet.maxNum; i++)/*仓*/
            {
                foreach (var item in _threelist[i])/*批次*/
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
                                eee._batchMsg = _msg;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < (int)EnumWareNumSet.maxNum; i++)/*仓*/
            {
                foreach (var item in _fourlist[i])/*批次*/
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
                                eee._batchMsg = _msg;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < (int)EnumWareNumSet.maxNum; i++)/*仓*/
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
                                eee._batchMsg = _msg;
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 为所有的worklist添加waremsg(仓储信息)到rebardata里面
        /// 仓储信息规则如下：
        /// 1、
        /// 2、每条通道按照8421仓来进行切换
        /// 3、
        /// </summary>
        /// <param name="_worklist"></param>
        private void AddWareMsg(ref List<ElementBatch>[] _worklist)
        {
            WareMsg _msg = new WareMsg();
            _msg.warehouseNo = 1;
            _msg.wareSet = EnumWareNumSet.NONE;
            _msg.wareno = 1;

            for (int i = 0; i < (int)EnumWareNumSet.maxNum; i++)/*仓*/
            {
                _msg.wareSet = (EnumWareNumSet)i;

                foreach (var item in _worklist[i])/*批次*/
                {
                    foreach (var iii in item.elementData)/*构件包*/
                    {
                        int _index = item.elementData.IndexOf(iii);//第几个构件包就是第几个仓位
                        //_msg.wareno = _index % GeneralClass.wareNum[i] + 1;
                        _msg.wareno = _index % GeneralClass.EnumWareSetToInt(_msg.wareSet) + 1;

                        foreach (var ttt in iii.diameterGroup)/*直径分组*/
                        {
                            foreach (var eee in ttt._datalist)/*钢筋*/
                            {
                                //if (eee.IfBend)
                                //{
                                //    _msg.warehouseNo = _index / GeneralClass.wareNum[i] + 1;
                                //}
                                //else
                                //{
                                //    _msg.warehouseNo = _index / GeneralClass.wareNum[i] + 2;
                                //}
                                _msg.warehouseNo = _index / GeneralClass.EnumWareSetToInt(_msg.wareSet) + 1;
                                eee._wareMsg = _msg;//重新赋值仓储信息
                            }
                        }
                    }
                }

            }



        }

        private void FillTreeView(List<ElementBatch>[] _list, ref TreeView _tv)
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

                    for (int k = 0; k < _list[i][j].elementData.Count; k++)
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
                    int _index = Convert.ToInt32(dataGridView8.Rows[e.RowIndex].Cells[0].Value.ToString());
                    string _project = dataGridView8.Rows[e.RowIndex].Cells[1].Value.ToString();
                    string _assembly = dataGridView8.Rows[e.RowIndex].Cells[2].Value.ToString();
                    string _element = dataGridView8.Rows[e.RowIndex].Cells[3].Value.ToString();

                    if (tabControl2.SelectedIndex == (int)EnumDiaGroupType.MULTI)
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
                                //dataGridView9.Columns[5].Visible = checkBox3.Checked;

                            }
                        }
                    }
                    else if (tabControl2.SelectedIndex == (int)EnumDiaGroupType.FEW)
                    {
                        foreach (var item in _fewShowlist)
                        {
                            if (item.projectName == _project && item.assemblyName == _assembly && item.elementName == _element && item.elementIndex == _index)
                            {
                                List<RebarData> newlist = new List<RebarData>();

                                foreach (var ttt in item.diameterGroup)
                                {
                                    newlist.AddRange(ttt._datalist);
                                }

                                Form2.FillDGVWithRebarList(newlist, dataGridView9);
                                //dataGridView9.Columns[5].Visible = checkBox3.Checked;
                            }
                        }
                    }
                    else if (tabControl2.SelectedIndex == (int)EnumDiaGroupType.ONE)
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
                                //dataGridView9.Columns[5].Visible = checkBox3.Checked;

                            }
                        }
                    }
                    else if (tabControl2.SelectedIndex == (int)EnumDiaGroupType.TWO)
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
                                //dataGridView9.Columns[5].Visible = checkBox3.Checked;

                            }
                        }
                    }
                    else if (tabControl2.SelectedIndex == (int)EnumDiaGroupType.THREE)
                    {
                        foreach (var item in _threeShowlist)
                        {
                            if (item.projectName == _project && item.assemblyName == _assembly && item.elementName == _element && item.elementIndex == _index)
                            {
                                List<RebarData> newlist = new List<RebarData>();

                                foreach (var ttt in item.diameterGroup)
                                {
                                    newlist.AddRange(ttt._datalist);
                                }

                                Form2.FillDGVWithRebarList(newlist, dataGridView9);
                                //dataGridView9.Columns[5].Visible = checkBox3.Checked;

                            }
                        }
                    }
                    else if (tabControl2.SelectedIndex == (int)EnumDiaGroupType.FOUR)
                    {
                        foreach (var item in _fourShowlist)
                        {
                            if (item.projectName == _project && item.assemblyName == _assembly && item.elementName == _element && item.elementIndex == _index)
                            {
                                List<RebarData> newlist = new List<RebarData>();

                                foreach (var ttt in item.diameterGroup)
                                {
                                    newlist.AddRange(ttt._datalist);
                                }

                                Form2.FillDGVWithRebarList(newlist, dataGridView9);
                                //dataGridView9.Columns[5].Visible = checkBox3.Checked;

                            }
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("dataGridView8_CellClick error:" + ex.Message); }

        }

        private void treeview_afterSelect(TreeViewEventArgs e, ref List<ElementRebarFB> _showlist, List<ElementBatch>[] _worklist)
        {
            if (e.Action == TreeViewAction.ByMouse || e.Action == TreeViewAction.ByKeyboard)
            {

                _showlist = new List<ElementRebarFB>();

                if (e.Node.Level == 0)
                {
                    for (int i = 0; i < _worklist[e.Node.Index].Count; i++)
                    {
                        for (int j = 0; j < _worklist[e.Node.Index][i].elementData.Count; j++)
                        {
                            _showlist.Add(_worklist[e.Node.Index][i].elementData[j]);
                        }
                    }
                    ShowElementAddData(_showlist, dataGridView10);
                }
                else if (e.Node.Level == 1)
                {
                    for (int i = 0; i < _worklist[e.Node.Parent.Index][e.Node.Index].elementData.Count; i++)
                    {
                        _showlist.Add(_worklist[e.Node.Parent.Index][e.Node.Index].elementData[i]);

                    }
                    ShowElementAddData(_showlist, dataGridView10);
                }
                else
                {
                    _showlist.Add(_worklist[e.Node.Parent.Parent.Index][e.Node.Parent.Index].elementData[e.Node.Index]);
                    ShowElementAddData(_showlist, dataGridView10);
                }

                FillDGV_Elements_show(_showlist, ref dataGridView8);

            }
        }

        private void FillDGV_Elements_show(List<ElementRebarFB> _elements, ref DataGridView _dgv)
        {

            DataTable dt_z = new DataTable();
            //dt_z.Columns.Add("索引", typeof(int));
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

            //int _index = 0;
            foreach (var item in _elements)
            {
                dt_z.Rows.Add(/*_index,*/ item.elementIndex, item.projectName, item.assemblyName, item.elementName, item.totalNum, item.totalWeight, item.diameterType, item.diameterStr);
                //_index++;
            }

            _dgv.DataSource = dt_z;
            _dgv.Columns[5].DefaultCellStyle.Format = "0.00";          //
        }
        /// <summary>
        /// 手动排程sourcelist
        /// </summary>
        private List<ElementData> _ElementSourceList = new List<ElementData>();
        /// <summary>
        /// 手动排程targetlist
        /// </summary>
        private List<ElementData> _ElementTargetList = new List<ElementData>();

        private List<ElementData> _ElementTargetList_1 = new List<ElementData>();//1#料仓
        private List<ElementData> _ElementTargetList_2 = new List<ElementData>();//2#料仓
        private List<ElementData> _ElementTargetList_3 = new List<ElementData>();//3#料仓
        private List<ElementData> _ElementTargetList_4 = new List<ElementData>();//4#料仓

        /// <summary>
        /// 拖拽中的list
        /// </summary>
        private List<ElementData> _flyingElement = new List<ElementData>();

        private void FillDGV_Elements_target_show(List<ElementData> _elements, ref DataGridView _dgv)
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
                int _wareSet = Convert.ToInt32(item.numSetting_bc.ToString().Substring(8, 1));

                dt_z.Rows.Add("A-" + item.elementIndex.ToString(),
                    item.totalNum_bc,
                   _wareSet,
                   0,

                    item.projectName, item.mainAssemblyName, item.elementName);
            }

            _dgv.DataSource = dt_z;
            //_dgv.Columns[7].DefaultCellStyle.Format = "0.00";          //

        }
        /// <summary>
        /// 根据套丝选择，筛选list
        /// </summary>
        /// <param name="_element"></param>
        /// <returns></returns>
        private List<RebarData> SettingCheck(ElementData _element)
        {
            List<RebarData> _list = new List<RebarData>();

            if (checkBox4.Checked)//选了不套丝，则显示所有棒材
            {
                _list = _element.rebarlist_bc;
            }
            else//只套丝的话，区分显示正丝反丝
            {
                if (checkBox5.Checked && checkBox6.Checked)//正反丝
                {
                    _list = _element.rebarlist_bc_tao;
                }
                else if (checkBox5.Checked && !checkBox6.Checked)//正丝
                {
                    _list = _element.rebarlist_bc_tao_zheng;
                }
                else if (!checkBox5.Checked && checkBox6.Checked)//反丝
                {
                    _list = _element.rebarlist_bc_tao_fan;
                }
            }
            return _list;
        }
        private void FillDGV_Elements_source_show(List<ElementData> _elements, ref DataGridView _dgv)
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
                    item.projectName, item.mainAssemblyName, item.elementName);
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
        private void treeView5_AfterSelect(object sender, TreeViewEventArgs e)
        {
            treeview_afterSelect(e, ref _fewShowlist, _fewWorklist);
        }
        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            treeview_afterSelect(e, ref _multiShowlist, _multiWorklist);
        }

        private void treeView3_AfterSelect(object sender, TreeViewEventArgs e)
        {
            treeview_afterSelect(e, ref _oneShowlist, _oneWorklist);
        }

        private void treeView4_AfterSelect(object sender, TreeViewEventArgs e)
        {
            treeview_afterSelect(e, ref _twoShowlist, _twoWorklist);
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            treeview_afterSelect(e, ref _threeShowlist, _threeWorklist);
        }
        private void treeView6_AfterSelect(object sender, TreeViewEventArgs e)
        {
            treeview_afterSelect(e, ref _fourShowlist, _fourWorklist);

        }

        /// <summary>
        /// 套料显示时，当前选中的子加工批的rebarlist
        /// </summary>
        private List<Rebar> _SelectedRebarlist = new List<Rebar>();
        /// <summary>
        /// 套料显示时，当前选中的子加工批的rebarOriList
        /// </summary>
        private List<RebarOri> _SelectedRebarOriList = new List<RebarOri>();
        private void dataGridView11_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //点击dgv11每个构件包时，在dgv12中显示其详细套料信息
                if (e.RowIndex > -1)
                {
                    //直径种类
                    EnumDiaGroupType _type;
                    string sss = dataGridView11.Rows[e.RowIndex].Cells[0].Value.ToString();
                    if (sss == GeneralClass.m_DiaType[(int)EnumDiaGroupType.MULTI])
                    {
                        _type = EnumDiaGroupType.MULTI;
                    }
                    else if (sss == GeneralClass.m_DiaType[(int)EnumDiaGroupType.FEW])
                    {
                        _type = EnumDiaGroupType.FEW;
                    }
                    else if (sss == GeneralClass.m_DiaType[(int)EnumDiaGroupType.ONE])
                    {
                        _type = EnumDiaGroupType.ONE;
                    }
                    else if (sss == GeneralClass.m_DiaType[(int)EnumDiaGroupType.TWO])
                    {
                        _type = EnumDiaGroupType.TWO;
                    }
                    else if (sss == GeneralClass.m_DiaType[(int)EnumDiaGroupType.THREE])
                    {
                        _type = EnumDiaGroupType.THREE;
                    }
                    else if (sss == GeneralClass.m_DiaType[(int)EnumDiaGroupType.FOUR])
                    {
                        _type = EnumDiaGroupType.FOUR;
                    }
                    else
                    {
                        _type = EnumDiaGroupType.NONE;

                    }

                    ////仓位数
                    //EnumWareNumSet _ware;
                    //int _wareno = (int)dataGridView11.Rows[e.RowIndex].Cells[1].Value;
                    //if (_wareno == GeneralClass.wareNum[0])
                    //{
                    //    _ware = EnumWareNumSet.WARESET_8;
                    //}
                    //else if (_wareno == GeneralClass.wareNum[1])
                    //{
                    //    _ware = EnumWareNumSet.WARESET_4;
                    //}
                    //else if (_wareno == GeneralClass.wareNum[2])
                    //{
                    //    _ware = EnumWareNumSet.WARESET_2;
                    //}
                    //else if (_wareno == GeneralClass.wareNum[3])
                    //{
                    //    _ware = EnumWareNumSet.WARESET_1;
                    //}
                    //else
                    //{
                    //    _ware = EnumWareNumSet.NONE;
                    //}

                    int _batchNo = (int)dataGridView11.Rows[e.RowIndex].Cells[1].Value;
                    int _diameter = Convert.ToInt32(dataGridView11.Rows[e.RowIndex].Cells[2].Value.ToString().Substring(1, 2));

                    //当前选中的子加工批
                    _SelectedRebarlist = new List<Rebar>();
                    _SelectedRebarOriList = new List<RebarOri>();

                    var item = m_rebarTaoliaoList.Where(t => t.DiameterType == _type /*&& t.WareNumType == _ware*/ && t.BatchNo == _batchNo && t.Diameter == _diameter);

                    foreach (var ttt in item.ToList())
                    {
                        List<RebarOri> _temp = ttt._rebarOriList;

                        foreach (var eee in _temp)
                        {
                            _SelectedRebarlist.AddRange(eee._list);
                            _SelectedRebarOriList.Add(eee);
                        }
                    }

                    _SelectedRebarOriList = _SelectedRebarOriList.OrderByDescending(t => t._totalLength).ToList();//套料显示时，按照原材长度降序排序
                    FillDGV_Tao_show(_SelectedRebarOriList);
                    FillDGV_Length(_SelectedRebarlist);//显示rebarlist的长度统计结果
                    FillDGV_MaterialPool(_SelectedRebarOriList);
                }
            }
            catch (Exception ex) { MessageBox.Show("dataGridView11_CellClick error:" + ex.Message); }

        }
        /// <summary>
        /// 显示套料效果图
        /// </summary>
        private void FillDGV_Tao_show(List<RebarOri> _list)
        {
            DataTable dt = new DataTable();
            //dt.Columns.Add("序号", typeof(int));
            dt.Columns.Add(GeneralClass.CfgData.LanguageType == EnumLanguageType.Chinese ? "钢筋原材分段" : "Raw rebar nesting detail ", typeof(Image));


            foreach (var item in _list)
            {
                //int _index = _list.IndexOf(item);
                dt.Rows.Add(/*_index, */graphics.PaintRebar(item));

            }

            dataGridView12.DataSource = dt;


        }
        /// <summary>
        /// 每个加工批，显示长度统计
        /// </summary>
        /// <param name="_list"></param>
        private void FillDGV_Length(List<Rebar> _list)
        {
            DataTable dt = new DataTable();

            //dt.Columns.Add("", System.Type.GetType("System.Boolean"));
            dt.Columns.Add(" ", typeof(bool));
            dt.Columns.Add("长度(mm)", typeof(int));
            dt.Columns.Add("数量(根)", typeof(int));
            dt.Columns.Add("数量(%)", typeof(double));
            dt.Columns.Add("重量(kg)", typeof(double));
            dt.Columns.Add("重量(%)", typeof(double));

            //List<GroupbyLengthDatalist> _alllist = GeneralClass.SQLiteOpt.QueryAllListByLength(_list);

            double total_num = _list.Count;
            double total_weight = _list.Sum(t => t.weight);

            var _alllist = _list.GroupBy(t => t.length).ToList();

            foreach (var item in _alllist)
            {
                var _length = item.Key;
                var tt = item.ToList();

                dt.Rows.Add(false, _length, tt.Count, (double)tt.Count / total_num, tt.Sum(t => t.weight), tt.Sum(t => t.weight) / total_weight);
            }

            //int ilength = 0;
            //foreach (var item in _alllist)
            //{
            //    if (!int.TryParse(item._length, out ilength))
            //    {
            //        string[] tt = item._length.Split('~');
            //        ilength = (Convert.ToInt32(tt[0]) + Convert.ToInt32(tt[1])) / 2;
            //    }
            //    dt.Rows.Add(false, ilength, item._totalnum, (double)item._totalnum / total_num, item._totalweight, item._totalweight / total_weight);
            //}

            dataGridView1.DataSource = dt;
            //dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].DefaultCellStyle.Format = "P2";
            dataGridView1.Columns[4].DefaultCellStyle.Format = "0.00";
            dataGridView1.Columns[5].DefaultCellStyle.Format = "P2";

        }
        /// <summary>
        /// 每个加工批，显示套料后的原材库
        /// </summary>
        /// <param name="_list"></param>
        private void FillDGV_MaterialPool(List<RebarOri> _list)
        {
            if (_list == null || _list.Count == 0)
            {
                return;
            }

            DataTable dt = new DataTable();
            //dt.Columns.Add(" ", typeof(bool));
            dt.Columns.Add("直径(Φ)", typeof(string));
            dt.Columns.Add("长度(mm)", typeof(int));
            dt.Columns.Add("数量(根)", typeof(int));
            //dt.Columns.Add("重量(kg)", typeof(double));

            //List<GeneralMaterial> _material = new List<GeneralMaterial>();  

            string s_dia = "Φ" + _list[0]._list[0].Diameter;
            //EnumDiameterBang _bang = GeneralClass.IntToEnumDiameter(_list[0]._list[0].Diameter);

            var _newlist = _list.GroupBy(t => t._totalLength).ToList();

            foreach (var item in _newlist)
            {
                var _length = item.Key;
                int _num = item.ToList().Count;
                dt.Rows.Add(s_dia, _length, _num);
            }

            dataGridView2.DataSource = dt;
            //dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dataGridView2.Columns[3].DefaultCellStyle.Format = "P2";
            //dataGridView2.Columns[4].DefaultCellStyle.Format = "0.00";
            //dataGridView2.Columns[5].DefaultCellStyle.Format = "P2";
        }
        /// <summary>
        /// 套料后，所有的原材库汇总
        /// </summary>
        /// <param name="_list"></param>
        private void FillDGV_AllMaterialPool(List<RebarOri> _list)
        {
            if (_list == null || _list.Count == 0)
            {
                return;
            }

            DataTable dt = new DataTable();
            dt.Columns.Add(" ", typeof(bool));
            dt.Columns.Add("直径(Φ)", typeof(string));
            dt.Columns.Add("长度(mm)", typeof(int));
            dt.Columns.Add("数量(根)", typeof(int));
            //dt.Columns.Add("重量(kg)", typeof(double));

            var _diaList = _list.GroupBy(t => t._diameter).ToList();

            foreach (var item in _diaList)
            {
                string s_dia = "Φ" + item.Key.ToString();

                var _lengthlist = item.ToList().GroupBy(t => t._totalLength).ToList();

                foreach (var ttt in _lengthlist)
                {
                    var _length = ttt.Key;
                    int _num = ttt.ToList().Count;
                    dt.Rows.Add(true, s_dia, _length, _num);
                }
            }


            dataGridView3.DataSource = dt;
            //dataGridView3.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dataGridView3.Columns[3].DefaultCellStyle.Format = "P2";
            //dataGridView3.Columns[4].DefaultCellStyle.Format = "0.00";
            //dataGridView3.Columns[5].DefaultCellStyle.Format = "P2";
        }

        private void FillDGV_SecondUsed(List<RebarOri> _list)
        {
            if (_list == null || _list.Count == 0)
            {
                return;
            }

            DataTable dt = new DataTable();
            dt.Columns.Add(" ", typeof(bool));
            dt.Columns.Add("直径(Φ)", typeof(string));
            dt.Columns.Add("长度(mm)", typeof(int));
            dt.Columns.Add("数量(根)", typeof(int));
            //dt.Columns.Add("重量(kg)", typeof(double));



            var _diaList = _list.GroupBy(t => t._diameter).ToList();

            foreach (var item in _diaList)
            {
                string s_dia = "Φ" + item.Key.ToString();

                //var _lengthlist = item.ToList().GroupBy(t => t.s).ToList();

                List<int> _lengthlist = new List<int>();
                foreach (var ttt in item.ToList())
                {
                    _lengthlist.AddRange(ttt._secondUsedList);
                }

                var temp = _lengthlist.GroupBy(t => t).ToList();

                foreach (var ttt in temp)
                {
                    var _length = ttt.Key;
                    int _num = ttt.ToList().Count;
                    if (_length != 0)
                    {
                        dt.Rows.Add(true, s_dia, _length, _num);

                    }
                }
            }


            dataGridView4.DataSource = dt;
            //dataGridView4.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dataGridView4.Columns[3].DefaultCellStyle.Format = "P2";
            //dataGridView4.Columns[4].DefaultCellStyle.Format = "0.00";
            //dataGridView4.Columns[5].DefaultCellStyle.Format = "P2";
        }



        //private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        //{
        //    if (dataGridView1.Rows[e.RowIndex].IsNewRow && e.ColumnIndex == 4)//消除默认的红叉叉
        //    {
        //        e.Value = pictureBox1.Image;
        //    }
        //}

        //private void dataGridView4_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        //{
        //    if (dataGridView4.Rows[e.RowIndex].IsNewRow && e.ColumnIndex == 4)//消除默认的红叉叉
        //    {
        //        e.Value = pictureBox1.Image;
        //    }
        //}

        private void dataGridView9_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView9.Rows[e.RowIndex].IsNewRow && e.ColumnIndex == 4)//消除默认的红叉叉
            {
                e.Value = pictureBox1.Image;
            }
        }

        private void dataGridView12_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView12.Rows[e.RowIndex].IsNewRow && e.ColumnIndex == 1)//消除默认的红叉叉
            {
                e.Value = pictureBox1.Image;
            }
        }
        /// <summary>
        /// 首列自动编号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView11_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //自动编号，与数据无关
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
               e.RowBounds.Location.Y,
               dataGridView11.RowHeadersWidth - 4,
               e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics,
                  (e.RowIndex + 1).ToString(),
                   dataGridView11.RowHeadersDefaultCellStyle.Font,
                   rectangle,
                   dataGridView11.RowHeadersDefaultCellStyle.ForeColor,
                   TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CurrencyManager cm = (CurrencyManager)BindingContext[dataGridView3.DataSource];

            if (dataGridView3.Rows != null && dataGridView3.Rows.Count != 0)
            {
                for (int i = 0; i < dataGridView3.Rows.Count - 1; i++)
                {
                    if ((int)dataGridView3.Rows[i].Cells[2].Value <= 4500)
                    {
                        cm.SuspendBinding();//挂起数据的绑定，是必要有的，不加这句代码。下面一句第一次执行时就报错
                        dataGridView3.Rows[i].Visible = checkBox1.Checked;
                        cm.ResumeBinding(); //恢复数据绑定
                    }
                }
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            CurrencyManager cm = (CurrencyManager)BindingContext[dataGridView3.DataSource];

            if (dataGridView3.Rows != null && dataGridView3.Rows.Count != 0)
            {
                for (int i = 0; i < dataGridView3.Rows.Count - 1; i++)
                {
                    if ((int)dataGridView3.Rows[i].Cells[2].Value >= 6000)
                    {
                        cm.SuspendBinding();//挂起数据的绑定，是必要有的，不加这句代码。下面一句第一次执行时就报错
                        dataGridView3.Rows[i].Visible = checkBox2.Checked;
                        cm.ResumeBinding(); //恢复数据绑定

                    }
                }
            }
        }
        /// <summary>
        /// 列头自动编号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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


        private void dataGridView12_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //自动编号，与数据无关
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
               e.RowBounds.Location.Y,
               dataGridView12.RowHeadersWidth - 4,
               e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics,
                  (e.RowIndex + 1).ToString(),
                   dataGridView12.RowHeadersDefaultCellStyle.Font,
                   rectangle,
                   dataGridView12.RowHeadersDefaultCellStyle.ForeColor,
                   TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this._ifAutoGroup = radioButton1.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ////隐藏导航栏
            //this.splitContainer1.Panel1Collapsed = !this.splitContainer1.Panel1Collapsed;
            ////根据隐藏属性切换项目资源文件中的图片显示
            //button1.Image = this.splitContainer1.Panel1Collapsed ? Properties.Resources.icons8_double_down_26 : Properties.Resources.icons8_double_up_26;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView13.DataSource = null;
            List<RebarData> _pickedlist = Pick(GeneralClass.AllRebarList);
            FillDGVWithRebarList(_pickedlist, dataGridView13);
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView13.DataSource = null;
            List<RebarData> _pickedlist = Pick(GeneralClass.AllRebarList);
            FillDGVWithRebarList(_pickedlist, dataGridView13);
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView13.DataSource = null;
            List<RebarData> _pickedlist = Pick(GeneralClass.AllRebarList);
            FillDGVWithRebarList(_pickedlist, dataGridView13);
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView13.DataSource = null;
            List<RebarData> _pickedlist = Pick(GeneralClass.AllRebarList);
            FillDGVWithRebarList(_pickedlist, dataGridView13);
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView13.DataSource = null;
            List<RebarData> _pickedlist = Pick(GeneralClass.AllRebarList);
            FillDGVWithRebarList(_pickedlist, dataGridView13);
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView13.DataSource = null;
            List<RebarData> _pickedlist = Pick(GeneralClass.AllRebarList);
            FillDGVWithRebarList(_pickedlist, dataGridView13);
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView13.DataSource = null;
            List<RebarData> _pickedlist = Pick(GeneralClass.AllRebarList);
            FillDGVWithRebarList(_pickedlist, dataGridView13);
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView13.DataSource = null;
            List<RebarData> _pickedlist = Pick(GeneralClass.AllRebarList);
            FillDGVWithRebarList(_pickedlist, dataGridView13);
        }
        //保存至数据库
        private void button9_Click(object sender, EventArgs e)
        {

        }
        //料单总表根据进度栏状态改变当前行的颜色
        private void dataGridView13_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView13.Rows[e.RowIndex].IsNewRow && e.ColumnIndex == 3)//消除默认的红叉叉
            {
                //e.Value = pictureBox1.Image;
            }

            // 对数量为0 的列，显示灰色
            if (e.RowIndex >= 0 &&
                e.RowIndex < (dataGridView13.Rows.Count - 1) &&
                dataGridView13.Rows.Count > e.RowIndex &&
                dataGridView13.Columns.Count >= 10)
            {
                DataGridViewRow row = dataGridView13.Rows[e.RowIndex];

                // 获取【总根数】列的值
                int _num = Convert.ToInt32(row.Cells[GeneralClass.sRebarColumnName[(int)EnumAllRebarTableColName.TOTAL_PIECE_NUM]].Value);

                if (_num == 0)
                {
                    e.CellStyle.BackColor = System.Drawing.Color.Gray;//数量为0，显示灰色
                }
                else
                {
                    e.CellStyle.BackColor = System.Drawing.Color.White;//其他白色
                }

            }
        }
        /// <summary>
        /// 翻转边角结构
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            textBox5.Text = GeneralClass.LDOpt.ldhelper.ExchangeRebarMsg(m_cornerMsg);
        }

        private string m_cornerMsg = "";
        private void dataGridView13_MouseDown(object sender, MouseEventArgs e)
        {
            // 获取鼠标在DataGridView中的位置（行和列索引）  
            int rowIndex = dataGridView13.HitTest(e.X, e.Y).RowIndex;
            int colIndex = dataGridView13.HitTest(e.X, e.Y).ColumnIndex;

            if (rowIndex >= 0 && colIndex >= 0)
            {
                 m_cornerMsg = (string)(dataGridView13.Rows[rowIndex].Cells[5].Value);

            }

            //int _length = (int)(dt.Rows[rowIndex][2]);


        }
    }
}
