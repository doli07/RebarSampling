using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RebarSampling
{


    #region ElementDataBZ

    /// <summary>
    /// 构件包中标准化加工的钢筋，主要为9米或12米原材，以及3米、4米、5米、6米、7米等整数长度的非原材
    /// </summary>
    public class ElementDataBZ
    {
        public ElementDataBZ()
        {
            this.projectName = "";
            this.assemblyName = "";
            this.elementIndex = 0;
            this.elementName = "";
            this.diameterType = 0;
            this.diameterList = new List<EnumRebarBang>();
            this.diameterGroup = new List<GroupbyDiameterListWithLength>();
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
        /// <summary>
        /// 所包含的直径规格，Φ16，Φ18，Φ20，Φ22，Φ25，Φ28，Φ32，Φ36，Φ40，
        /// </summary>
        public List<EnumRebarBang> diameterList { get; set; }
        /// <summary>
        /// 根据直径分类的钢筋list
        /// </summary>
        public List<GroupbyDiameterListWithLength> diameterGroup { get; set; }

    }
    #endregion

    #region ElementDataFB
    /// <summary>
    /// 构件包中非标准化加工的钢筋
    /// </summary>
    public class ElementDataFB
    {
        public ElementDataFB()
        {
            this.projectName = "";
            this.assemblyName = "";
            this.elementIndex = 0;
            this.elementName = "";

            this.diameterType = 0;
            this.diameterList = new List<EnumRebarBang>();
            this.diameterGroup = new List<GroupbyDiameterListWithLength>();
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
        public List<EnumRebarBang> diameterList { get; set; }
        /// <summary>
        /// 由diameterlist拼接出来的string，用于排序
        /// </summary>
        public string diameterStr 
        { 
            get 
            {
                string sss = "";
                foreach(var item in this.diameterList)
                {
                    sss+=item.ToString().Substring(6,2)+",";
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
                bool ifpitch1=false,ifpitch2=false,ifpitch3=false;
                foreach(var item in this.diameterList)
                {
                    if(item==EnumRebarBang.BANG_C16||item==EnumRebarBang.BANG_C18||item==EnumRebarBang.BANG_C20||item==EnumRebarBang.BANG_C22)
                    {
                        ifpitch1 = true; 
                    }
                    else if(item == EnumRebarBang.BANG_C25 || item == EnumRebarBang.BANG_C28 || item == EnumRebarBang.BANG_C32 )
                    {
                        ifpitch2 = true;
                    }
                    else if (item == EnumRebarBang.BANG_C36 || item == EnumRebarBang.BANG_C40 )
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
        public List<GroupbyDiameterListWithLength> diameterGroup { get; set; }
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
        ///// <summary>
        ///// 根据仓位分组获得仓位数
        ///// </summary>
        //public int wareNo
        //{
        //    get
        //    {
        //        switch (this.numGroup)
        //        {
        //            case EnumRebarNumGroup.EIGHT: return 8;
        //            case EnumRebarNumGroup.FOUR: return 4;
        //            case EnumRebarNumGroup.TWO: return 2;
        //            case EnumRebarNumGroup.ONE: return 1;
        //            default: return 0;
        //        }
        //    }
        //}

        /// <summary>
        /// 判断本构件的直径种类是否【至少有_includeNum个元素】被包含于_elementFB中，默认为100（全包含）
        /// </summary>
        /// <param name="_elementFB"></param>
        /// <param name="_includeNum"></param>
        /// <returns></returns>
        public bool IfIncludeby(ElementDataFB _elementFB,int _includeNum=100)
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
                if(_includeNum==100)    //全部包含
                {
                    return (num==this.diameterList.Count)?true:false;
                }
                else if(_includeNum>=0 &&_includeNum<10)//部分包含，至少包含_includeNum种直径
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
        ///// <summary>
        ///// 判断本构件的直径种类是否全部被包含于_elementFB中
        ///// </summary>
        ///// <param name="_dlist"></param>
        ///// <returns></returns>
        //public bool IfIncludeby(ElementDataFB _elementFB)
        //{
        //    if (this.diameterList.Count != 0 && _elementFB.diameterList.Count != 0)//均不为空
        //    {
        //        foreach (var item in this.diameterList)
        //        {
        //            if (!_elementFB.diameterList.Exists(t => t == item))//判断本构件的直径种类是否全部被包含于_elementFB中
        //            {
        //                return false;
        //            }
        //        }
        //        return true;
        //    }
        //    else { return false; }
        //}



        /// <summary>
        /// 判断_elementFB的直径种类是否全部被包含于本构件中
        /// </summary>
        /// <param name="_elementFB"></param>
        /// <returns></returns>
        public bool IfInclude(ElementDataFB _elementFB)
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
            this.rebarlist_bc = new List<RebarData>();
            this.rebarlist_xc = new List<RebarData>();
            this.rebarlist_bc_bz = new List<RebarData>();
            this.rebarlist_bc_fb = new List<RebarData>();

            this.elementDataBZ = new ElementDataBZ();
            this.elementDataFB = new ElementDataFB();
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
        public List<RebarData> rebarlist_xc { get; private set; }
        /// <summary>
        /// 棒材list
        /// </summary>
        public List<RebarData> rebarlist_bc { get; private set; }

        /// <summary>
        /// 标化棒材list
        /// </summary>
        public List<RebarData> rebarlist_bc_bz { get; private set; }
        /// <summary>
        /// 非标化棒材list
        /// </summary>
        public List<RebarData> rebarlist_bc_fb { get; private set; }

        /// <summary>
        /// 标准化加工的钢筋
        /// </summary>
        public ElementDataBZ elementDataBZ { get; private set; }
        /// <summary>
        /// 非标加工的钢筋
        /// </summary>
        public ElementDataFB elementDataFB { get; private set; }

        public void Group()
        {
            GroupbyXB();
            GroupbyBiao_bc();
            GroupbyDiameter_bc();
        }
        /// <summary>
        /// 区分棒材、线材
        /// </summary>
        private void GroupbyXB()
        {
            if (this.rebarlist.Count != 0)//不为空
            {
                this.rebarlist_bc.Clear();
                this.rebarlist_xc.Clear();

                foreach (var item in this.rebarlist)
                {
                    if (item.Diameter < 16)
                    {
                        this.rebarlist_xc.Add(item);
                    }
                    else { this.rebarlist_bc.Add(item); }
                }
            }
        }

        /// <summary>
        /// 将棒材按照标化、非标化区分开，标化钢筋包括：9米/12米原材、3米、4米、5米、6米、7米的非原材
        /// </summary>
        private void GroupbyBiao_bc()
        {
            if (this.rebarlist_bc.Count != 0)//棒材list不为空
            {
                this.rebarlist_bc_bz.Clear();
                this.rebarlist_bc_fb.Clear();

                foreach (var item in this.rebarlist_bc)
                {
                    if (item.Length == "9000" || item.Length == "12000" || item.Length == "3000" || item.Length == "4000" || item.Length == "5000" || item.Length == "7000")
                    {
                        this.rebarlist_bc_bz.Add(item);
                    }
                    else
                    {
                        this.rebarlist_bc_fb.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// 将棒材根据直径进行分组
        /// </summary>
        /// <param name="_list"></param>
        private void GroupbyDiameter_bc()
        {
            //标化加工的棒材
            if (this.rebarlist_bc_bz.Count != 0)//不为空
            {
                //找一个构件包中直径种类
                //var _group = GeneralClass.SQLiteOpt.QueryAllListByDiameter(this.rebarlist_bc_bz);
                var _group = GeneralClass.SQLiteOpt.QueryAllListByDiameterWithLength(this.rebarlist_bc_bz);

                elementDataBZ.projectName = this.projectName;
                elementDataBZ.assemblyName = this.assemblyName;
                elementDataBZ.elementIndex = this.elementIndex;
                elementDataBZ.elementName = this.elementName;

                elementDataBZ.diameterType = _group.Count;//直径种类数量

                var _newgroup = _group.OrderBy(t => t._diameter).ToList();//按照直径升序排列

                elementDataBZ.diameterList.Clear();
                elementDataBZ.diameterGroup.Clear();
                foreach (var item in _newgroup)
                {
                    if (item._diameter >= 16)
                    {
                        elementDataBZ.diameterList.Add((EnumRebarBang)System.Enum.Parse(typeof(EnumRebarBang), "BANG_C" + item._diameter.ToString()));//将直径转为enum
                        elementDataBZ.diameterGroup.Add(item);//注意：此处为浅拷贝，改变原list，会同步改变当前list
                    }
                }
            }

            //非标加工的棒材
            if (this.rebarlist_bc_fb.Count != 0)//不为空
            {
                //找一个构件包中直径种类
                //var _group = GeneralClass.SQLiteOpt.QueryAllListByDiameter(this.rebarlist_bc_fb);
                var _group = GeneralClass.SQLiteOpt.QueryAllListByDiameterWithLength(this.rebarlist_bc_fb);

                elementDataFB.projectName = this.projectName;
                elementDataFB.assemblyName = this.assemblyName;
                elementDataFB.elementIndex = this.elementIndex;
                elementDataFB.elementName = this.elementName;

                elementDataFB.diameterType = _group.Count;//直径种类数量

                var _newgroup = _group.OrderBy(t => t._diameter).ToList();//按照直径升序排列

                elementDataFB.diameterList.Clear();
                elementDataFB.diameterGroup.Clear();
                foreach (var item in _newgroup)
                {
                    if (item._diameter >= 16)
                    {
                        elementDataFB.diameterList.Add((EnumRebarBang)System.Enum.Parse(typeof(EnumRebarBang), "BANG_C" + item._diameter.ToString()));//将直径转为enum
                        elementDataFB.diameterGroup.Add(item);//注意：此处为浅拷贝，改变原list，会同步改变当前list
                    }
                }
            }



        }
    }

    #endregion
}
