using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RebarSampling
{

    #region ElementDadBatch大的构件批（按直径种类包含关系分组，不考虑数量仓位分区）
    /// <summary>
    /// 大构件批，里面为直径种类具有包含关系的构件批，不考虑数量仓位分区
    /// </summary>
    public class ElementDadBatch
    {
        public ElementDadBatch()
        {
            this.batchlist = new List<ElementBatch>();
        }
        public List<EnumDiaBang> diameterList
        {
            get
            {
                List<EnumDiaBang> _list = new List<EnumDiaBang>();

                if (this.batchlist.Count != 0)
                {
                    foreach (var item in this.batchlist)
                    {
                        _list.AddRange(item.diameterList);//汇总
                    }
                    _list = _list.Distinct().OrderBy(t => t).ToList();//去除重复、并按照升序排序
                }
                return _list;
            }
        }

        public List<ElementBatch> batchlist { get; set; }
        /// <summary>
        /// 查找当前大批与其他大批有几种直径是相同的
        /// </summary>
        /// <param name="_batch">目标大批</param>
        /// <param name="_sameNum">相同直径的种类</param>
        /// <returns></returns>
        public bool IfHaveSameDia(ElementDadBatch _dadBatch, int _sameNum)
        {
            return (this.diameterList.Intersect(_dadBatch.diameterList).ToList().Count == _sameNum) ? true : false;//求交集用intersect
        }
    }
    #endregion

    #region ElementChildBatch构件子批（按直径分）
    public class ElementChildBatch
    {
        public ElementChildBatch()
        {
            this._diameter = 0;
            this.totalChildBatch = 0;
            this.curChildBatch = 0;
            this._list = new List<RebarData>();
        }
        /// <summary>
        /// 直径
        /// </summary>
        public EnumDiaBang _diameter { get; set; }

        public List<RebarData> _list { get; set; }

        public int totalChildBatch { get; set; }
        public int curChildBatch { get; set; }
    }
    #endregion

    #region ElementDataBatch构件批
    ///// <summary>
    ///// 构件批，20240531添加，注意与elementBatch的区别，后续再做甄别删减
    ///// </summary>
    //public class ElementDataBatch
    //{
    //    public ElementDataBatch()
    //    {
    //        this.totalBatch = 0;
    //        this.curBatch = 0;
    //        this.elementData = new List<ElementData>();
    //    }

    //    /// <summary>
    //    /// 总批次数量
    //    /// </summary>
    //    public int totalBatch { get; set; }
    //    /// <summary>
    //    /// 当前批次序号
    //    /// </summary>
    //    public int curBatch { get; set; }
    //    /// <summary>
    //    /// 构件list
    //    /// </summary>
    //    public List<ElementData> elementData {  get; set; }
    //    /// <summary>
    //    /// 套丝机设置，格式为xx-xx-xx-xx，其中xx代表每条套丝轨道设置的套丝直径，如有反丝，则xx中含有"*"
    //    /// </summary>
    //    public string TaosiSetting { get; set; }
    //    /// <summary>
    //    /// 棒材的直径种类的汇总
    //    /// </summary>
    //    public List<EnumDiaBang> diameterList_bc
    //    {
    //        get
    //        {
    //            List<EnumDiaBang> _list = new List<EnumDiaBang>();

    //            if (this.elementData.Count != 0)
    //            {
    //                foreach (var item in this.elementData)
    //                {
    //                    _list.AddRange(item.diameterList_bc);//汇总
    //                }
    //                _list = _list.Distinct().OrderBy(t => t).ToList();//去除重复、并按照升序排序
    //            }
    //            return _list;
    //        }
    //    }

    //    /// <summary>
    //    /// 由棒材diameterList_bc拼接出来的string，已经过升序排序
    //    /// </summary>
    //    public string diameterStr_bc
    //    {
    //        get
    //        {
    //            string sss = "";
    //            foreach (var item in this.diameterList_bc)
    //            {
    //                sss += item.ToString().Substring(6, 2) + ",";
    //            }
    //            return sss;
    //        }
    //    }
    //    /// <summary>
    //    /// 需要套丝的棒材直径种类的汇总
    //    /// </summary>
    //    public List<EnumDiaBang> diameterList_bc_tao
    //    {
    //        get
    //        {
    //            List<EnumDiaBang> _list = new List<EnumDiaBang>();

    //            if (this.elementData.Count != 0)
    //            {
    //                foreach (var item in this.elementData)
    //                {
    //                    _list.AddRange(item.diameterList_bc_tao);//汇总
    //                }
    //                _list = _list.Distinct().OrderBy(t => t).ToList();//去除重复、并按照升序排序
    //            }
    //            return _list;
    //        }
    //    }

    //    /// <summary>
    //    /// 由需要套丝的diameterList_bc_tao拼接出来的string，已经过升序排序
    //    /// </summary>
    //    public string diameterStr_bc_tao
    //    {
    //        get
    //        {
    //            string sss = "";
    //            foreach (var item in this.diameterList_bc_tao)
    //            {
    //                sss += item.ToString().Substring(6, 2) + ",";
    //            }
    //            return sss;
    //        }
    //    }

    //    public int totalnum
    //    {
    //        get
    //        {
    //            return this.elementData.Sum(t => t.totalNum_bc);
    //        }
    //    }
    //    public double totalweight
    //    {
    //        get
    //        {
    //            return this.elementData.Sum(t=>t.totalweight_bc);
    //        }
    //    }
    //}
    #endregion

    #region ElementBatch构件批    
    /// <summary>
    /// 构件批，组成worklist
    /// </summary>
    public class ElementBatch
    {
        public ElementBatch()
        {
            //this.batchMsg = new BatchMsg();
            this.totalBatch = 0;
            this.curBatch = 0;
            this.BatchSeri=string.Empty;
            this.elementData = new List<ElementRebarFB>();
        }
        public List<EnumDiaBang> diameterList
        {
            get
            {
                List<EnumDiaBang> _list = new List<EnumDiaBang>();

                if (this.elementData.Count != 0)
                {
                    foreach (var item in this.elementData)
                    {
                        _list.AddRange(item.diameterList);//汇总
                    }
                    _list = _list.Distinct().OrderBy(t => t).ToList();//去除重复、并按照升序排序
                }
                return _list;
            }
        }
        /// <summary>
        /// 套丝机设置，格式为xx-xx-xx-xx，其中xx代表每条套丝轨道设置的套丝直径，如有反丝，则xx中含有"*"
        /// </summary>
        public string TaosiSetting { get; set; }

        /// <summary>
        /// 棒材总数量根数
        /// </summary>
        public int totalNum_bc
        {
            get { return (this.elementData.Count != 0) ? this.elementData.Sum(t => t.totalNum) : 0; }
        }
        /// <summary>
        /// 棒材总重量
        /// </summary>
        public double totalWeight_bc
        {
            get { return (this.elementData.Count != 0) ? this.elementData.Sum(t => t.totalWeight) : 0; }
        }
        /// <summary>
        /// 构件批的序号
        /// </summary>
        public string BatchSeri { get; set; }
        /// <summary>
        /// 棒材的直径种类的汇总
        /// </summary>
        public List<EnumDiaBang> diameterList_bc
        {
            get
            {
                List<EnumDiaBang> _list = new List<EnumDiaBang>();

                if (this.elementData.Count != 0)
                {
                    foreach (var item in this.elementData)
                    {
                        _list.AddRange(item.diameterList);//汇总
                    }
                    _list = _list.Distinct().OrderBy(t => t).ToList();//去除重复、并按照升序排序
                }
                return _list;
            }
        }

        /// <summary>
        /// 由棒材diameterList_bc拼接出来的string，已经过升序排序
        /// </summary>
        public string diameterStr_bc
        {
            get
            {
                string sss = "";
                foreach (var item in this.diameterList_bc)
                {
                    sss += item.ToString().Substring(6, 2) + ",";
                }
                return sss;
            }
        }
        /// <summary>
        /// 需要套丝的棒材直径种类的汇总
        /// </summary>
        public List<EnumDiaBang> diameterList_bc_tao
        {
            get
            {
                List<EnumDiaBang> _list = new List<EnumDiaBang>();

                if (this.elementData.Count != 0)
                {
                    foreach (var item in this.elementData)
                    {
                        _list.AddRange(item.diameterList_bc_tao);//汇总
                    }
                    _list = _list.Distinct().OrderBy(t => t).ToList();//去除重复、并按照升序排序
                }
                return _list;
            }
        }

        /// <summary>
        /// 由需要套丝的diameterList_bc_tao拼接出来的string，已经过升序排序
        /// </summary>
        public string diameterStr_bc_tao
        {
            get
            {
                string sss = "";
                foreach (var item in this.diameterList_bc_tao)
                {
                    sss += item.ToString().Substring(6, 2) + ",";
                }
                return sss;
            }
        }
        /// <summary>
        /// 批次信息，包括子批次（按直径分）
        /// </summary>
        //public BatchMsg batchMsg { get; set; }

        /// <summary>
        /// 总批次数量
        /// </summary>
        public int totalBatch { get; set; }
        /// <summary>
        /// 当前批次序号
        /// </summary>
        public int curBatch { get; set; }

        /// <summary>
        /// 当前批次的仓位分类，一般来说跟批次内构件一致
        /// </summary>
        public EnumWareNumSet numGroup
        {
            get
            {
                if (this.elementData.Count != 0)
                {
                    if (this.elementData.First().wareNumSet != this.elementData.Last().wareNumSet)
                    {
                        GeneralClass.interactivityData?.printlog(1, "本批次不同构件的仓位分类不一致，请检查");
                        return EnumWareNumSet.NONE;
                    }
                    else
                    {
                        return this.elementData.First().wareNumSet;
                    }
                }
                else { return EnumWareNumSet.NONE; }
            }
        }
        /// <summary>
        /// 构件list
        /// </summary>
        public List<ElementRebarFB> elementData { get; set; }
        /// <summary>
        /// 子批list，从解析构件list而来
        /// </summary>
        public List<ElementChildBatch> childBatchList
        {
            get
            {
                List<ElementChildBatch> _list = new List<ElementChildBatch>();
                ElementChildBatch _child = new ElementChildBatch();

                List<GroupbyDiaWithLength> _group = new List<GroupbyDiaWithLength>();

                foreach (var item in this.elementData)
                {
                    foreach (var iii in item.diameterGroup)
                    {
                        _group.Add(iii);//先把所有的diaGroup提取出来
                    }
                }

                var _newgroup = _group.GroupBy(t => t._diameter).ToList();
                foreach (var eee in _newgroup)
                {
                    //elementDataBZ.diameterList.Add((EnumRebarBang)System.Enum.Parse(typeof(EnumRebarBang), "BANG_C" + item._diameter.ToString()));//将直径转为enum
                    _child = new ElementChildBatch();
                    //_child._diameter = (EnumDiaBang)System.Enum.Parse(typeof(EnumDiaBang), "BANG_C" + eee.Key.ToString());//将直径转为enum
                    _child._diameter = GeneralClass.IntToEnumDiameter(eee.Key);//将直径转为enum
                    var _gg = eee.ToList();
                    foreach (var g in _gg)
                    {
                        _child._list.AddRange(g._datalist);
                    }
                    _child.totalChildBatch = _newgroup.Count;//总的子批次
                    _list.Add(_child);
                }
                _list = _list.OrderBy(t => t._diameter).ToList();
                foreach (var item in _list)
                {
                    item.curChildBatch = _list.IndexOf(item);//当前子批号
                }

                return _list;
            }
        }

        /// <summary>
        /// 判断本构件与_diaList汇总的直径种类不超过4个
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public bool IfIncludeUnderFour(List<EnumDiaBang> _diaList)
        {
            if (this.diameterList.Count > 4 || _diaList.Count > 4)//如果本构件批与_diaList所包含的直径种类大于4，则返回false
            {
                return false;
            }
            if (this.diameterList.Count != 0 && _diaList.Count != 0)//均不为空
            {
                List<EnumDiaBang> _list = new List<EnumDiaBang>();

                _list.AddRange(this.diameterList);
                _list.AddRange(_diaList);//汇总

                var _newlist = _list.Distinct().ToList();//去除重复

                return (_newlist.Count > 4) ? false : true;
            }
            else
            {
                return false;
            }
        }
        public bool IfIncludeby(List<EnumDiaBang> _list)
        {
            if (this.diameterList.Count != 0 && _list.Count != 0)//均不为空
            {
                foreach (var item in this.diameterList)
                {
                    if (!_list.Exists(t => t == item))
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    #endregion

    #region ElementRebarBZ

    /// <summary>
    /// 构件包中标准化加工的钢筋，主要为9米或12米原材，以及3米、4米、5米、6米、7米等整数长度的非原材
    /// </summary>
    //public class ElementRebarBZ
    //{
    //    public ElementRebarBZ()
    //    {
    //        this.projectName = "";
    //        this.assemblyName = "";
    //        this.elementIndex = 0;
    //        this.elementName = "";
    //        this.diameterType = 0;
    //        this.diameterList = new List<EnumRebarBang>();
    //        this.diameterGroup = new List<GroupbyDiaWithLength>();
    //    }

    //    /// <summary>
    //    /// 项目名称
    //    /// </summary>
    //    public string projectName { get; set; }
    //    /// <summary>
    //    /// 主构件名称
    //    /// </summary>
    //    public string assemblyName { get; set; }
    //    /// <summary>
    //    /// 因为elementname并非唯一，所以需要elementindex来索引，指示在主构件中的索引位置
    //    /// </summary>
    //    public int elementIndex { get; set; }
    //    /// <summary>
    //    /// 构件名称
    //    /// </summary>
    //    public string elementName { get; set; }
    //    /// <summary>
    //    /// 直径种类数量
    //    /// </summary>
    //    public int diameterType { get; set; }
    //    /// <summary>
    //    /// 所包含的直径规格，Φ16，Φ18，Φ20，Φ22，Φ25，Φ28，Φ32，Φ36，Φ40，
    //    /// </summary>
    //    public List<EnumRebarBang> diameterList { get; set; }
    //    /// <summary>
    //    /// 根据直径分类的钢筋list
    //    /// </summary>
    //    public List<GroupbyDiaWithLength> diameterGroup { get; set; }

    //}
    #endregion

    #region ElementRebarFB
    /// <summary>
    /// 构件包中非标准化加工的钢筋
    /// </summary>
    public class ElementRebarFB
    {
        public ElementRebarFB()
        {
            this.projectName = "";
            this.assemblyName = "";
            this.childAssemblyName = "";
            this.elementIndex = -1;
            this.elementName = "";

            this.diameterType = 0;
            this.diameterList = new List<EnumDiaBang>();
            this.diameterGroup = new List<GroupbyDiaWithLength>();

            this.rebarlist = new List<RebarData>();

            this.warehouseNo = 0;
            this.wareNo = 0;
            this.batchSeri = "";
            //this.totalNum = 0;
            //this.numGroup = EnumRebarNumGroup.NONE;
            //this.wareNo = 0;
        }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string projectName { get; set; }
        /// <summary>
        /// 主构件名称
        /// </summary>
        public string assemblyName { get; set; }
        /// <summary>
        /// 子构件部位名称
        /// </summary>
        public string childAssemblyName { get;set; }
        /// <summary>
        /// 因为elementname并非唯一，所以需要elementindex来索引，指示在主构件中的索引位置
        /// </summary>
        public int elementIndex { get; set; }
        /// <summary>
        /// 构件名称
        /// </summary>
        public string elementName { get; set; }


        /// <summary>
        /// 直径种类数量
        /// </summary>
        public int diameterType { get; set; }
        //private List<EnumRebarBang> diameterlist { get; set; }
        /// <summary>
        /// 所包含的直径规格，Φ16，Φ18，Φ20，Φ22，Φ25，Φ28，Φ32，Φ36，Φ40，
        /// </summary>
        //public List<EnumRebarBang> diameterList { get { return this.diameterlist.OrderBy(t => t).ToList(); } set { this.diameterlist = value; } }
        public List<EnumDiaBang> diameterList { get; set; }
        /// <summary>
        /// 由diameterlist拼接出来的string，已经过升序排序
        /// </summary>
        public string diameterStr
        {
            get
            {
                this.diameterList = this.diameterList.OrderBy(t => t).ToList();//先进行升序的排序

                string sss = "";
                foreach (var item in this.diameterList)
                {
                    sss += item.ToString().Substring(6, 2) + ",";
                }
                return sss;
            }
        }
        /// <summary>
        /// 棒材需要套丝的钢筋所包含的直径规格，Φ12，Φ14，Φ16，Φ18，Φ20，Φ22，Φ25，Φ28，Φ32，Φ36，Φ40，
        /// </summary>
        public List<EnumDiaBang> diameterList_bc_tao
        {
            get
            {
                List<EnumDiaBang> _dialist = new List<EnumDiaBang>();
                var temp = this.rebarlist_bc_tao.GroupBy(t => t.Diameter).OrderBy(y => y.Key).ToList();
                foreach (var item in temp)
                {
                    _dialist.Add(GeneralClass.IntToEnumDiameter(item.Key));
                }
                return _dialist;
            }
        }
        /// <summary>
        /// 料仓编号
        /// </summary>
        public int warehouseNo { get; set; }
        /// <summary>
        /// 仓位编号
        /// </summary>
        public int wareNo { get; set; }

        //public WareMsg wareMsg { get; set; }

        /// <summary>
        /// 生产批的批号
        /// </summary>
        public string batchSeri {  get; set; }

        /// <summary>
        ///钢筋数量分组，
        ///原则：
        ///WARESET_12:  1~10(12包，1仓1包)，
        ///WARESET_6:   10~25(6包，2仓1包)，
        ///WARESET_3:   25~50(3包，4仓1包)，
        ///WARESET_2:   50~100(2包，6仓1包)
        ///WARESET_1:   100~(1包，12仓1包)
        /// </summary>
        public EnumWareNumSet wareNumSet
        {
            get
            {
                if (this.totalNum != 0)
                {
                    if (this.totalNum > 0 && this.totalNum <= GeneralClass.wareThreshold[0])
                    {
                        return EnumWareNumSet.WARESET_12;
                    }
                    else if (this.totalNum > GeneralClass.wareThreshold[0] && this.totalNum <= GeneralClass.wareThreshold[1])
                    {
                        return EnumWareNumSet.WARESET_6;
                    }
                    else if (this.totalNum > GeneralClass.wareThreshold[1] && this.totalNum <= GeneralClass.wareThreshold[2])
                    {
                        return EnumWareNumSet.WARESET_3;
                    }
                    else if (this.totalNum > GeneralClass.wareThreshold[2] && this.totalNum <= GeneralClass.wareThreshold[3])
                    {
                        return EnumWareNumSet.WARESET_2;
                    }
                    else if (this.totalNum > GeneralClass.wareThreshold[3])
                    {
                        return EnumWareNumSet.WARESET_1;
                    }
                    else
                    {
                        return EnumWareNumSet.NONE;
                    }
                }
                else
                {
                    return EnumWareNumSet.NONE;
                }
            }
        }

        /// <summary>
        /// 钢筋螺距区间，Φ16~Φ22用2.5螺距，Φ25~Φ32用3.0螺距，Φ36~Φ40用3.5螺距
        /// </summary>
        public EnumDiameterPitchType diameterPitchType
        {
            get
            {
                bool ifpitch1 = false, ifpitch2 = false, ifpitch3 = false;
                foreach (var item in this.diameterList)
                {
                    if (item == EnumDiaBang.BANG_C16 || item == EnumDiaBang.BANG_C18 || item == EnumDiaBang.BANG_C20 || item == EnumDiaBang.BANG_C22)
                    {
                        ifpitch1 = true;
                    }
                    else if (item == EnumDiaBang.BANG_C25 || item == EnumDiaBang.BANG_C28 || item == EnumDiaBang.BANG_C32)
                    {
                        ifpitch2 = true;
                    }
                    else if (item == EnumDiaBang.BANG_C36 || item == EnumDiaBang.BANG_C40)
                    {
                        ifpitch3 = true;
                    }
                }
                if (ifpitch1 && !ifpitch2 && !ifpitch3) return EnumDiameterPitchType.PITCH_1;
                if (!ifpitch1 && ifpitch2 && !ifpitch3) return EnumDiameterPitchType.PITCH_2;
                if (!ifpitch1 && !ifpitch2 && ifpitch3) return EnumDiameterPitchType.PITCH_3;
                if (ifpitch1 && ifpitch2 && !ifpitch3) return EnumDiameterPitchType.PITCH_12;
                if (ifpitch1 && !ifpitch2 && ifpitch3) return EnumDiameterPitchType.PITCH_13;
                if (!ifpitch1 && ifpitch2 && ifpitch3) return EnumDiameterPitchType.PITCH_23;
                if (ifpitch1 && ifpitch2 && ifpitch3) return EnumDiameterPitchType.PITCH_123;

                return EnumDiameterPitchType.NONE;
            }
        }
        /// <summary>
        /// 根据直径分类的钢筋list
        /// </summary>
        public List<GroupbyDiaWithLength> diameterGroup { get; set; }
        /// <summary>
        /// 总数量根数
        /// </summary>
        public int totalNum
        {
            get { return (this.diameterGroup.Count != 0) ? this.diameterGroup.Sum(t => t._totalnum) : 0; }
        }
        public double totalWeight
        {
            get { return (this.diameterGroup.Count != 0) ? this.diameterGroup.Sum(t => t._totalweight) : 0; }
        }
        /// <summary>
        /// rebardata数据的list
        /// </summary>
        public List<RebarData> rebarlist { get; set; }
        /// <summary>
        /// 棒材list
        /// </summary>
        public List<RebarData> rebarlist_bc
        {
            get
            {
                int _bangThreshold = GeneralClass.m_typeC12 ? 12 : (GeneralClass.m_typeC14 ? 14 : 16);//区分线材棒材的阈值
                return this.rebarlist.Where(t => t.Diameter >= _bangThreshold).ToList();
            }
        }
        /// <summary>
        /// 棒材需要套丝的
        /// </summary>
        public List<RebarData> rebarlist_bc_tao
        {
            get
            {
                return this.rebarlist_bc.Where(t => t.IfTao == true).ToList();
            }
        }

        /// <summary>
        /// 棒材不需要套丝的
        /// </summary>
        public List<RebarData> rebarlist_bc_butao
        {
            get
            {
                return this.rebarlist_bc.Where(t => t.IfTao == false).ToList();
            }
        }
        /// <summary>
        /// 棒材套丝中的正丝
        /// </summary>
        public List<RebarData> rebarlist_bc_tao_zheng
        {
            get
            {
                return this.rebarlist_bc.Where(t => t.IfTaoZheng == true).ToList();
            }
        }
        /// <summary>
        /// 棒材套丝中的反丝
        /// </summary>
        public List<RebarData> rebarlist_bc_tao_fan
        {
            get
            {
                return this.rebarlist_bc.Where(t => t.IfTaoFan == true).ToList();
            }
        }

        ///// <summary>
        ///// 根据总数量进行的分组,原则：
        ///// EIGHT:1~10(8仓)，
        ///// FOUR: ~50(4仓)，
        ///// TWO:51~100(2仓)，
        ///// ONE:100~(1仓)
        ///// </summary>
        //public EnumWareNumSet numGroup
        //{
        //    get
        //    {
        //        if (this.totalNum != 0)
        //        {
        //            if (this.totalNum > 0 && this.totalNum <= GeneralClass.wareArea[0])
        //            {
        //                return EnumWareNumSet.WARESET_8;
        //            }
        //            else if (this.totalNum > GeneralClass.wareArea[0] && this.totalNum <= GeneralClass.wareArea[1])
        //            {
        //                return EnumWareNumSet.WARESET_4;
        //            }
        //            else if (this.totalNum > GeneralClass.wareArea[1] && this.totalNum <= GeneralClass.wareArea[2])
        //            {
        //                return EnumWareNumSet.WARESET_2;
        //            }
        //            else if (this.totalNum > GeneralClass.wareArea[2])
        //            {
        //                return EnumWareNumSet.WARESET_1;
        //            }
        //            else
        //            {
        //                return EnumWareNumSet.NONE;
        //            }
        //        }
        //        else
        //        {
        //            return EnumWareNumSet.NONE;
        //        }
        //    }
        //}


        /// <summary>
        /// 判断本构件与_elementFB构件总共的直径种类不超过4个
        /// </summary>
        /// <param name="_elementFB"></param>
        /// <returns></returns>
        public bool IfIncludeUnderFour(ElementRebarFB _elementFB)
        {

            if (this.diameterType > 4 || _elementFB.diameterType > 4)//如果本构件与_elementFB所包含的直径种类大于4，则返回false
            {
                return false;
            }
            if (this.diameterList.Count != 0 && _elementFB.diameterList.Count != 0)//均不为空
            {
                List<EnumDiaBang> _list = new List<EnumDiaBang>();

                _list.AddRange(this.diameterList);
                _list.AddRange(_elementFB.diameterList);//汇总

                var _newlist = _list.Distinct().ToList();//去除重复

                return (_newlist.Count > 4) ? false : true;
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// 判断本构件与_diaList汇总的直径种类不超过X个，X由_underNum定义
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public bool IfIncludeUnderX(List<EnumDiaBang> _diaList,int _underNum)
        {
            if (this.diameterType > _underNum || _diaList.Count > _underNum)//如果本构件与_elementFB所包含的直径种类大于4，则返回false
            {
                return false;
            }
            if (this.diameterList.Count != 0 && _diaList.Count != 0)//均不为空
            {
                List<EnumDiaBang> _list = new List<EnumDiaBang>();

                _list.AddRange(this.diameterList);
                _list.AddRange(_diaList);//汇总

                var _newlist = _list.Distinct().ToList();//去除重复

                return (_newlist.Count > _underNum) ? false : true;
            }
            else
            {
                return false;
            }
        }
        public bool IfIncludeby(List<EnumDiaBang> _list)
        {
            if (this.diameterList.Count != 0 && _list.Count != 0)//均不为空
            {
                foreach (var item in this.diameterList)
                {
                    if (!_list.Exists(t => t == item))
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 判断本构件的直径种类是否【至少有_includeNum个元素】被包含于_elementFB中，默认为100（全包含）
        /// </summary>
        /// <param name="_elementFB"></param>
        /// <param name="_includeNum"></param>
        /// <returns></returns>
        public bool IfIncludeby(ElementRebarFB _elementFB, int _includeNum = 100)
        {
            int num = 0;

            if (this.diameterList.Count != 0 && _elementFB.diameterList.Count != 0)//均不为空
            {
                foreach (var item in this.diameterList)
                {
                    if (_elementFB.diameterList.Exists(t => t == item))//判断本构件的直径种类是否全部被包含于_elementFB中
                    {
                        num++;
                    }
                }
                if (_includeNum == 100)    //全部包含
                {
                    return (num == this.diameterList.Count) ? true : false;
                }
                else if (_includeNum >= 0 && _includeNum < 10)//部分包含，至少包含_includeNum种直径
                {
                    return (num >= _includeNum) ? true : false;
                }
                else
                {
                    return false;       //无效输入
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断_elementFB的直径种类是否全部被包含于本构件中
        /// </summary>
        /// <param name="_elementFB"></param>
        /// <returns></returns>
        public bool IfInclude(ElementRebarFB _elementFB)
        {
            if (this.diameterList.Count != 0 && _elementFB.diameterList.Count != 0)//均不为空
            {
                foreach (var item in _elementFB.diameterList)
                {
                    if (!this.diameterList.Exists(t => t == item))//判断_elementFB的直径种类是否全部被包含于本构件中
                    {
                        return false;
                    }
                }
                return true;
            }
            else { return false; }
        }
    }
    #endregion

    #region ElementData       
    /// <summary>
    /// 构件包的数据结构，里面包含有多个钢筋
    /// </summary>
    public class ElementData
    {
        public ElementData()
        {
            this.projectName = "";
            this.mainAssemblyName = "";
            this.childAssemblyName = "";
            this.elementIndex = 0;
            this.elementName = "";

            this.rebarlist = new List<RebarData>();
            //this.rebarlist_bc = new List<RebarData>();
            //this.rebarlist_xc = new List<RebarData>();
            //this.rebarlist_bc_bz = new List<RebarData>();
            //this.rebarlist_bc_fb = new List<RebarData>();
            this.batchSeri = "";
            //this.elementDataBZ = new ElementDataBZ();
            //this.elementDataFB = new ElementRebarFB();
        }
        public void Copy(ElementData _data)
        {
            this.projectName = _data.projectName;
            this.mainAssemblyName= _data.mainAssemblyName;
            this.childAssemblyName = _data.childAssemblyName;
            this.elementIndex= _data.elementIndex;
            this.elementName = _data.elementName;
            this.rebarlist.Clear();
            foreach(var item in _data.rebarlist)
            {
                this.rebarlist.Add(item);
            }
        }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string projectName { get; set; }
        /// <summary>
        /// 主构件名称
        /// </summary>
        public string mainAssemblyName { get; set; }
        /// <summary>
        /// 子构件部位名称
        /// </summary>
        public string childAssemblyName { get; set; }
        /// <summary>
        /// 因为elementname并非唯一，所以需要elementindex来索引，指示在主构件中的索引位置
        /// </summary>
        public int elementIndex { get; set; }
        /// <summary>
        /// 记录在总的料表中构件的总数量
        /// </summary>
        public int elementTotalNum { get; set; }
        /// <summary>
        /// 构件名称
        /// </summary>
        public string elementName { get; set; }
        /// <summary>
        /// 料仓通道编号，20240911增加
        /// </summary>
        public int warehouseNo {  get; set; }
        /// <summary>
        /// 料仓中的分割仓编号，20240911增加
        /// </summary>
        public int wareNo {  get; set; }
        /// <summary>
        /// 批次序号，20240911增加
        /// </summary>
        public string batchSeri {  get; set; }

        /// <summary>
        /// 钢筋所属构件类型，分为墙、柱、梁、板、楼梯
        /// </summary>
        public EnumRebarAssemblyType RebarAssemblyType { get; set; }

        /// <summary>
        /// 构件中所有的钢筋列表
        /// </summary>
        public List<RebarData> rebarlist { get; set; }

        /**  以下成员变量均为内部可写，外部只读***/

        /// <summary>
        /// 线材list
        /// </summary>
        public List<RebarData> rebarlist_xc
        {
            get
            {
                return this.rebarlist.Where(t=>t.RebarSizeType==EnumRebarSizeType.XIAN).ToList();
                //int _bangThreshold = GeneralClass.m_typeC12 ? 12 : (GeneralClass.m_typeC14 ? 14 : 16);//区分线材棒材的阈值
                //return this.rebarlist.Where(t => t.Diameter < _bangThreshold).ToList();
            }
        }
        /// <summary>
        /// 棒材list
        /// </summary>
        public List<RebarData> rebarlist_bc
        {
            get
            {
                return this.rebarlist.Where(t=>t.RebarSizeType==EnumRebarSizeType.BANG).ToList();
                //int _bangThreshold = GeneralClass.m_typeC12 ? 12 : (GeneralClass.m_typeC14 ? 14 : 16);//区分线材棒材的阈值
                //return this.rebarlist.Where(t=>t.Diameter>=_bangThreshold).ToList();
            }
        }
        /// <summary>
        /// 棒材需要套丝的
        /// </summary>
        public List<RebarData> rebarlist_bc_tao
        {
            get
            {
                return this.rebarlist_bc.Where(t=>t.IfTao==true).ToList();
            }
        }

        /// <summary>
        /// 棒材不需要套丝的
        /// </summary>
        public List<RebarData> rebarlist_bc_butao
        {
            get
            {
                return this.rebarlist_bc.Where(t => t.IfTao == false).ToList();
            }
        }
        /// <summary>
        /// 棒材套丝中的正丝
        /// </summary>
        public List<RebarData> rebarlist_bc_tao_zheng
        {
            get
            {
                return this.rebarlist_bc.Where(t => t.IfTaoZheng == true).ToList();
            }
        }
        /// <summary>
        /// 棒材套丝中的反丝
        /// </summary>
        public List<RebarData> rebarlist_bc_tao_fan
        {
            get
            {
                return this.rebarlist_bc.Where(t => t.IfTaoFan == true).ToList();
            }
        }

        /// <summary>
        /// 棒材总数量根数
        /// </summary>
        public int totalNum_bc
        {
            get { return (this.rebarlist_bc.Count != 0) ? this.rebarlist_bc.Sum(t => t.TotalPieceNum) : 0; }
        }
        /// <summary>
        /// 棒材总重量
        /// </summary>
        public double totalweight_bc
        {
            get { return (this.rebarlist_bc.Count != 0) ? this.rebarlist_bc.Sum(t => t.TotalWeight) : 0; }
        }

        public double totalweight
        {
            get { return (this.rebarlist.Count != 0) ? this.rebarlist.Sum(t => t.TotalWeight) : 0; }
        }
        /// <summary>
        ///钢筋数量分组，原则：WARESET_8:1~25(8仓)，WARESET_4:26~50(4仓)，WARESET_2:51~100(2仓)，WARESET_1:100~(1仓)
        /// </summary>
        public EnumWareNumSet numSetting_bc
        {
            get
            {
                if (this.totalNum_bc != 0)
                {
                    if (this.totalNum_bc > 0 && this.totalNum_bc <= GeneralClass.wareThreshold[0])
                    {
                        return EnumWareNumSet.WARESET_12;
                    }
                    else if (this.totalNum_bc > GeneralClass.wareThreshold[0] && this.totalNum_bc <= GeneralClass.wareThreshold[1])
                    {
                        return EnumWareNumSet.WARESET_6;
                    }
                    else if (this.totalNum_bc > GeneralClass.wareThreshold[1] && this.totalNum_bc <= GeneralClass.wareThreshold[2])
                    {
                        return EnumWareNumSet.WARESET_3;
                    }
                    else if (this.totalNum_bc > GeneralClass.wareThreshold[2] && this.totalNum_bc <= GeneralClass.wareThreshold[3])
                    {
                        return EnumWareNumSet.WARESET_2;
                    }
                    else if (this.totalNum_bc > GeneralClass.wareThreshold[3])
                    {
                        return EnumWareNumSet.WARESET_1;
                    }
                    else
                    {
                        return EnumWareNumSet.NONE;
                    }
                }
                else
                {
                    return EnumWareNumSet.NONE;
                }
            }
        }

        /// <summary>
        /// 棒材所包含的直径规格，Φ12，Φ14，Φ16，Φ18，Φ20，Φ22，Φ25，Φ28，Φ32，Φ36，Φ40，
        /// </summary>
        public List<EnumDiaBang> diameterList_bc 
        {
            get
            {
                List<EnumDiaBang > _dialist = new List<EnumDiaBang>();
                var temp = this.rebarlist_bc.GroupBy(t=>t.Diameter).OrderBy(y=>y.Key).ToList();
                foreach(var item in temp)
                {
                    _dialist.Add(GeneralClass.IntToEnumDiameter(item.Key));
                }
                return _dialist;
            }             
        }
        /// <summary>
        /// 由棒材diameterList_bc拼接出来的string，已经过升序排序
        /// </summary>
        public string diameterStr_bc
        {
            get
            {
                string sss = "";
                foreach (var item in this.diameterList_bc)
                {
                    sss += item.ToString().Substring(6, 2) + ",";
                }
                return sss;
            }
        }

        /// <summary>
        /// 棒材需要套丝的钢筋所包含的直径规格，Φ12，Φ14，Φ16，Φ18，Φ20，Φ22，Φ25，Φ28，Φ32，Φ36，Φ40，
        /// </summary>
        public List<EnumDiaBang> diameterList_bc_tao
        {
            get
            {
                List<EnumDiaBang> _dialist = new List<EnumDiaBang>();
                var temp = this.rebarlist_bc_tao.GroupBy(t => t.Diameter).OrderBy(y => y.Key).ToList();
                foreach (var item in temp)
                {
                    _dialist.Add(GeneralClass.IntToEnumDiameter(item.Key));
                }
                return _dialist;
            }
        }
        /// <summary>
        /// 由需要套丝的diameterList_bc_tao拼接出来的string，已经过升序排序
        /// </summary>
        public string diameterStr_bc_tao
        {
            get
            {
                string sss = "";
                foreach (var item in this.diameterList_bc_tao)
                {
                    sss += item.ToString().Substring(6, 2) + ",";
                }
                return sss;
            }
        }

        /// <summary>
        /// 非标加工的钢筋
        /// </summary>
        public ElementRebarFB elementDataFB
        {
            get
            {
                ElementRebarFB _fb = new ElementRebarFB();
                //非标加工的棒材
                //if (this.rebarlist_bc_fb.Count != 0)//不为空
                if (this.rebarlist_bc.Count != 0)//不为空
                {
                    //找一个构件包中直径种类
                    //var _group = GeneralClass.SQLiteOpt.QueryAllListByDiameterWithLength(this.rebarlist_bc_fb);
                    var _group = GeneralClass.DBOpt.QueryAllListByDiameterWithLength(this.rebarlist_bc);

                    _fb.projectName = this.projectName;
                    _fb.assemblyName = this.mainAssemblyName;
                    _fb.childAssemblyName= this.childAssemblyName;
                    _fb.elementIndex = this.elementIndex;
                    _fb.elementName = this.elementName;

                    _fb.diameterType = _group.Count;//直径种类数量

                    var _newgroup = _group.OrderBy(t => t._diameter).ToList();//按照直径升序排列

                    _fb.diameterList.Clear();
                    _fb.diameterGroup.Clear();

                    //int _bangThreshold = GeneralClass.m_typeC14 ? 14 : 16;//区分棒材线材的阈值

                    foreach (var item in _newgroup)
                    {
                        //if (item._diameter >= _bangThreshold)
                        //{
                        _fb.diameterList.Add((EnumDiaBang)System.Enum.Parse(typeof(EnumDiaBang), "BANG_C" + item._diameter.ToString()));//将直径转为enum
                        _fb.diameterGroup.Add(item);//注意：此处为浅拷贝，改变原list，会同步改变当前list
                        //}
                    }

                    _fb.rebarlist.Clear();
                    _fb.rebarlist.AddRange(this.rebarlist_bc);
                }
                return _fb;
            }
        }


    }

    #endregion
}
