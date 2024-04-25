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
        public List<EnumDiameterBang> diameterList
        {
            get
            {
                List<EnumDiameterBang> _list = new List<EnumDiameterBang>();

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
        public EnumDiameterBang _diameter { get; set; }

        public List<RebarData> _list { get; set; }

        public int totalChildBatch { get; set; }
        public int curChildBatch { get; set; }
    }
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
            this.elementData = new List<ElementRebarFB>();
        }
        public List<EnumDiameterBang> diameterList
        {
            get
            {
                List<EnumDiameterBang> _list = new List<EnumDiameterBang>();

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
        /// 批次信息，包括子批次（按直径分）
        /// </summary>
        //public BatchMsg batchMsg { get; set; }
        /// <summary>
        /// 总批次
        /// </summary>
        public int totalBatch { get; set; }
        /// <summary>
        /// 当前批次
        /// </summary>
        public int curBatch { get; set; }
        /// <summary>
        /// 当前批次的仓位分类，一般来说跟批次内构件一致
        /// </summary>
        public EnumWareNumGroup numGroup
        {
            get
            {
                if (this.elementData.Count != 0)
                {
                    if (this.elementData.First().numGroup != this.elementData.Last().numGroup)
                    {
                        GeneralClass.interactivityData?.printlog(1, "本批次不同构件的仓位分类不一致，请检查");
                        return EnumWareNumGroup.NONE;
                    }
                    else
                    {
                        return this.elementData.First().numGroup;
                    }
                }
                else { return EnumWareNumGroup.NONE; }
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
                    _child._diameter = (EnumDiameterBang)System.Enum.Parse(typeof(EnumDiameterBang), "BANG_C" + eee.Key.ToString());//将直径转为enum
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
        public bool IfIncludeUnderFour(List<EnumDiameterBang> _diaList)
        {
            if (this.diameterList.Count > 4 || _diaList.Count > 4)//如果本构件批与_diaList所包含的直径种类大于4，则返回false
            {
                return false;
            }
            if (this.diameterList.Count != 0 && _diaList.Count != 0)//均不为空
            {
                List<EnumDiameterBang> _list = new List<EnumDiameterBang>();

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
        public bool IfIncludeby(List<EnumDiameterBang> _list)
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
            this.elementIndex = 0;
            this.elementName = "";

            this.diameterType = 0;
            this.diameterList = new List<EnumDiameterBang>();
            this.diameterGroup = new List<GroupbyDiaWithLength>();
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
        public List<EnumDiameterBang> diameterList { get; set; }
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
        /// 钢筋螺距区间，Φ16~Φ22用2.5螺距，Φ25~Φ32用3.0螺距，Φ36~Φ40用3.5螺距
        /// </summary>
        public EnumDiameterPitchType diameterPitchType
        {
            get
            {
                bool ifpitch1 = false, ifpitch2 = false, ifpitch3 = false;
                foreach (var item in this.diameterList)
                {
                    if (item == EnumDiameterBang.BANG_C16 || item == EnumDiameterBang.BANG_C18 || item == EnumDiameterBang.BANG_C20 || item == EnumDiameterBang.BANG_C22)
                    {
                        ifpitch1 = true;
                    }
                    else if (item == EnumDiameterBang.BANG_C25 || item == EnumDiameterBang.BANG_C28 || item == EnumDiameterBang.BANG_C32)
                    {
                        ifpitch2 = true;
                    }
                    else if (item == EnumDiameterBang.BANG_C36 || item == EnumDiameterBang.BANG_C40)
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
        public double totalweight
        {
            get { return (this.diameterGroup.Count != 0) ? this.diameterGroup.Sum(t => t._totalweight) : 0; }
        }
        /// <summary>
        /// 根据总数量进行的分组,原则：
        /// EIGHT:1~10(8仓)，
        /// FOUR: ~50(4仓)，
        /// TWO:51~100(2仓)，
        /// ONE:100~(1仓)
        /// </summary>
        public EnumWareNumGroup numGroup
        {
            get
            {
                if (this.totalNum != 0)
                {
                    if (this.totalNum > 0 && this.totalNum <= GeneralClass.wareArea[0])
                    {
                        return EnumWareNumGroup.EIGHT;
                    }
                    else if (this.totalNum > GeneralClass.wareArea[0] && this.totalNum <= GeneralClass.wareArea[1])
                    {
                        return EnumWareNumGroup.FOUR;
                    }
                    else if (this.totalNum > GeneralClass.wareArea[1] && this.totalNum <= GeneralClass.wareArea[2])
                    {
                        return EnumWareNumGroup.TWO;
                    }
                    else if (this.totalNum > GeneralClass.wareArea[2])
                    {
                        return EnumWareNumGroup.ONE;
                    }
                    else
                    {
                        return EnumWareNumGroup.NONE;
                    }
                }
                else
                {
                    return EnumWareNumGroup.NONE;
                }
            }
        }


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
                List<EnumDiameterBang> _list = new List<EnumDiameterBang>();

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
        /// 判断本构件与_diaList汇总的直径种类不超过4个
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public bool IfIncludeUnderFour(List<EnumDiameterBang> _diaList)
        {
            if (this.diameterType > 4 || _diaList.Count > 4)//如果本构件与_elementFB所包含的直径种类大于4，则返回false
            {
                return false;
            }
            if (this.diameterList.Count != 0 && _diaList.Count != 0)//均不为空
            {
                List<EnumDiameterBang> _list = new List<EnumDiameterBang>();

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
        public bool IfIncludeby(List<EnumDiameterBang> _list)
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
            this.assemblyName = "";
            this.elementIndex = 0;
            this.elementName = "";

            this.rebarlist = new List<RebarData>();
            //this.rebarlist_bc = new List<RebarData>();
            //this.rebarlist_xc = new List<RebarData>();
            //this.rebarlist_bc_bz = new List<RebarData>();
            //this.rebarlist_bc_fb = new List<RebarData>();

            //this.elementDataBZ = new ElementDataBZ();
            //this.elementDataFB = new ElementRebarFB();
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
        /// 因为elementname并非唯一，所以需要elementindex来索引，指示在主构件中的索引位置
        /// </summary>
        public int elementIndex { get; set; }
        /// <summary>
        /// 构件名称
        /// </summary>
        public string elementName { get; set; }

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
                List<RebarData> _xc = new List<RebarData>();
                if (this.rebarlist.Count != 0)//不为空
                {
                    _xc.Clear();

                    int _bangThreshold = GeneralClass.m_typeC12 ? 12 : (GeneralClass.m_typeC14 ? 14 : 16);//区分线材棒材的阈值

                    foreach (var item in this.rebarlist)
                    {
                        if (item.Diameter < _bangThreshold)
                        {
                            _xc.Add(item);
                        }
                    }
                }
                return _xc;
            }
        }
        /// <summary>
        /// 棒材list
        /// </summary>
        public List<RebarData> rebarlist_bc
        {
            get
            {
                List<RebarData> _bc = new List<RebarData>();
                if (this.rebarlist.Count != 0)//不为空
                {
                    _bc.Clear();

                    int _bangThreshold =GeneralClass.m_typeC12 ? 12 : (GeneralClass.m_typeC14 ? 14 : 16) ;//区分线材棒材的阈值

                    foreach (var item in this.rebarlist)
                    {
                        if (item.Diameter >= _bangThreshold)
                        {
                            _bc.Add(item);
                        }
                    }
                }
                return _bc;
            }
        }

        ///// <summary>
        ///// 标化棒材list
        ///// </summary>
        //public List<RebarData> rebarlist_bc_bz { get; private set; }
        ///// <summary>
        ///// 非标化棒材list
        ///// </summary>
        //public List<RebarData> rebarlist_bc_fb { get; private set; }

        /// <summary>
        /// 标准化加工的钢筋
        /// </summary>
        //public ElementRebarBZ elementDataBZ { get; private set; }

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
                    var _group = GeneralClass.SQLiteOpt.QueryAllListByDiameterWithLength(this.rebarlist_bc);

                    _fb.projectName = this.projectName;
                    _fb.assemblyName = this.assemblyName;
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
                        _fb.diameterList.Add((EnumDiameterBang)System.Enum.Parse(typeof(EnumDiameterBang), "BANG_C" + item._diameter.ToString()));//将直径转为enum
                        _fb.diameterGroup.Add(item);//注意：此处为浅拷贝，改变原list，会同步改变当前list
                        //}
                    }
                }
                return _fb;
            }
        }

        //public void Group()
        //{
        //    //GroupbyXB();
        //    //GroupbyBiao_bc();//20231116，暂取消区分标准化加工和非标化加工
        //    //GroupbyDiameter_bc();
        //}

        /// <summary>
        /// 区分棒材、线材
        /// </summary>
        //private void GroupbyXB()
        //{
        //    if (this.rebarlist.Count != 0)//不为空
        //    {
        //        this.rebarlist_bc.Clear();
        //        this.rebarlist_xc.Clear();

        //        int _bangThreshold = GeneralClass.m_typeC14 ? 14 : 16;//区分线材棒材的阈值

        //        foreach (var item in this.rebarlist)
        //        {
        //            if (item.Diameter < _bangThreshold)
        //            {
        //                this.rebarlist_xc.Add(item);
        //            }
        //            else { this.rebarlist_bc.Add(item); }
        //        }
        //    }
        //}

        /// <summary>
        /// 将棒材按照标化、非标化区分开，标化钢筋包括：9米/12米原材、3米、4米、5米、6米、7米的非原材
        /// </summary>
        //private void GroupbyBiao_bc()
        //{
        //    if (this.rebarlist_bc.Count != 0)//棒材list不为空
        //    {
        //        this.rebarlist_bc_bz.Clear();
        //        this.rebarlist_bc_fb.Clear();

        //        foreach (var item in this.rebarlist_bc)
        //        {
        //            if (item.Length == "9000" || item.Length == "12000" || item.Length == "3000" || item.Length == "4000" || item.Length == "5000" || item.Length == "7000")
        //            {
        //                this.rebarlist_bc_bz.Add(item);
        //            }
        //            else
        //            {
        //                this.rebarlist_bc_fb.Add(item);
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// 将棒材根据直径进行分组
        /// </summary>
        /// <param name="_list"></param>
        //private void GroupbyDiameter_bc()
        //{
        //    //非标加工的棒材
        //    //if (this.rebarlist_bc_fb.Count != 0)//不为空
        //    if (this.rebarlist_bc.Count != 0)//不为空
        //    {
        //        //找一个构件包中直径种类
        //        //var _group = GeneralClass.SQLiteOpt.QueryAllListByDiameterWithLength(this.rebarlist_bc_fb);
        //        var _group = GeneralClass.SQLiteOpt.QueryAllListByDiameterWithLength(this.rebarlist_bc);

        //        elementDataFB.projectName = this.projectName;
        //        elementDataFB.assemblyName = this.assemblyName;
        //        elementDataFB.elementIndex = this.elementIndex;
        //        elementDataFB.elementName = this.elementName;

        //        elementDataFB.diameterType = _group.Count;//直径种类数量

        //        var _newgroup = _group.OrderBy(t => t._diameter).ToList();//按照直径升序排列

        //        elementDataFB.diameterList.Clear();
        //        elementDataFB.diameterGroup.Clear();

        //        int _bangThreshold = GeneralClass.m_typeC14 ? 14 : 16;//区分棒材线材的阈值

        //        foreach (var item in _newgroup)
        //        {
        //            if (item._diameter >= _bangThreshold)
        //            {
        //                elementDataFB.diameterList.Add((EnumRebarBang)System.Enum.Parse(typeof(EnumRebarBang), "BANG_C" + item._diameter.ToString()));//将直径转为enum
        //                elementDataFB.diameterGroup.Add(item);//注意：此处为浅拷贝，改变原list，会同步改变当前list
        //            }
        //        }
        //    }



        //}
    }

    #endregion
}
