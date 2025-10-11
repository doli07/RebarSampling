using NPOI.HSSF.Record.CF;
using NPOI.OpenXmlFormats.Dml.ChartDrawing;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RebarSampling
{
    /// <summary>
    /// 长度套料算法
    /// </summary>
    public partial class Algorithm
    {
        /// <summary>
        /// 长度套料算法
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="_totallength">返回成品总长</param>
        /// <returns>返回套料后的原材钢筋</returns>
        public static List<RebarOri> Taoliao(List<RebarData> _list, out int _totallength)
        {


            List<Rebar> _alllist = new List<Rebar>();
            _alllist = ListDescend(_list);//降序展开

            //List<Rebar> _alllist = ListExpand(_list);//展开
            //List<Rebar> _alllist = ListAscend(_list);//升序展开

            List<Rebar> _part1 = new List<Rebar>();//长度1500以上
            List<Rebar> _part2 = new List<Rebar>();//长度1500以下

            _totallength = _alllist.Sum(t => t.length);//统计总长度

            foreach (var item in _alllist)//将_alllist按照长度1500以上和以下的分开套料
            {
                if (item.length >= GeneralClass.CfgData.MinLength)
                {
                    _part1.Add(item);
                }
                else
                {
                    _part2.Add(item);
                }
            }


            List<RebarOri> _returnlist = new List<RebarOri>();
            // _returnlist = Algorithm_FFD(_alllist);//FFD首次适应算法
            // _returnlist = Algorithm_BFD(_alllist);//BFD最佳适应算法
            //_returnlist = Algorithm_FFD_1(_alllist);//FFD首次适应算法，改进版
            //_returnlist.AddRange(Algorithm_FFD_1(_part1));//FFD首次适应算法，改进版
            if (_part1.Count != 0)//要先判断part1是否为空
            {
                switch (GeneralClass.CfgData.TaoType)
                {
                    case EnumTaoType.ORITAO:
                        _returnlist.AddRange(Algorithm_FFD_1(_part1));//FFD首次适应算法，改进版V1.0
                        break;
                    case EnumTaoType.ORIPOOLTAO:
                        _returnlist.AddRange(Algorithm_FFD_2(_part1, GeneralClass.m_MaterialPool));//FFD首次适应算法，改进版V2.0
                                                                                                   //_returnlist.AddRange(Algorithm_FFD_3(_part1, GeneralClass.m_MaterialPool));//FFD首次适应算法，改进版V3.0
                        break;
                    case EnumTaoType.CUTTAO_2:
                        //_returnlist.AddRange(Algorithm_FFD_4(_part1, GeneralClass.m_MaterialPool));//FFD首次适应算法，改进版V4.0
                        _returnlist.AddRange(Algorithm_FFD_5(_part1, GeneralClass.m_MaterialPool));//FFD首次适应算法，改进版V5.0
                        break;
                    case EnumTaoType.CUTTAO_3:
                        _returnlist.AddRange(Algorithm_FFD_6(_part1, GeneralClass.m_MaterialPool));//二叉树算法，改进版V6.0
                        break;
                    default:
                        break;
                }
            }

            if (GeneralClass.CfgData.IfShortRebar)
            {

                //短钢筋只用1500、1200的二次利用余料来套料，20240402
                List<MaterialOri> _materialPool = GeneralClass.m_MaterialPool.Where(t => t._length == GeneralClass.CfgData.MatPoolYuliao1
                || t._length == GeneralClass.CfgData.MatPoolYuliao2).ToList();

                //_returnlist.AddRange(Algorithm_FFD_1(_part2));//FFD首次适应算法，改进版
                if (_part2.Count != 0)
                {
                    switch (GeneralClass.CfgData.TaoType)
                    {
                        case EnumTaoType.ORITAO:
                            _returnlist.AddRange(Algorithm_FFD_1(_part2));//FFD首次适应算法，改进版V1.0
                            break;
                        case EnumTaoType.ORIPOOLTAO:
                            _returnlist.AddRange(Algorithm_FFD_2(_part2, GeneralClass.m_MaterialPool));//FFD首次适应算法，改进版V2.0
                                                                                                       //_returnlist.AddRange(Algorithm_FFD_3(_part2, GeneralClass.m_MaterialPool));//FFD首次适应算法，改进版V3.0
                            break;
                        case EnumTaoType.CUTTAO_2:
                            //_returnlist.AddRange(Algorithm_FFD_4(_part2, GeneralClass.m_MaterialPool));//FFD首次适应算法，改进版V4.0
                            _returnlist.AddRange(Algorithm_FFD_5(_part2, GeneralClass.m_MaterialPool));//FFD首次适应算法，改进版V5.0
                            break;
                        case EnumTaoType.CUTTAO_3:
                            _returnlist.AddRange(Algorithm_FFD_6(_part2, _materialPool/*GeneralClass.m_MaterialPool*/));//二叉树算法，改进版V6.0
                            break;

                        default:
                            break;
                    }
                }
            }

            _returnlist = _returnlist.OrderBy(t => t._lengthFirstLeft).ToList();

            //按照工序优化的原则，对原材list重新排序，20241104添加此功能
            if (GeneralClass.CfgData.IfSeriTao)
            {
                SerialTao(ref _returnlist);
            }

            return _returnlist;
        }

        /// <summary>
        /// 将套料结果进行排序，目的是优化工序节拍
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        private static void SerialTao(ref List<RebarOri> _list)
        {
            //第一步，将一根原材内部按照加工工序数量来排序，工序最多的在前面，最少的在后面
            foreach (RebarOri o in _list)
            {
                o._list = o._list.OrderByDescending(t => t.caseCount).ToList();//按照加工工序数量多少，降序排列
            }

            //第二步，将前后两个原材按照加工工序多少间隔开
            _list = _list.OrderByDescending(t => t._caseCount).ToList();//先降序排列
            int _zeroIndex = 0;
            RebarOri temp = new RebarOri(_list.First()._level, _list.First()._diameter);
            for (int i = 0; i < _list.Count; i++)
            {
                if (_list[i]._caseCount != 0) continue;//先找到caseCount==0的元素

                _zeroIndex++;
                if (_zeroIndex * 2 < _list.Count && _list[_zeroIndex * 2 - 1]._caseCount != 0)//把当前caseCount==0跟前面不为0且为偶数行的元素进行互换
                {
                    temp = _list[i];
                    _list[i] = _list[_zeroIndex * 2 - 1];
                    _list[_zeroIndex * 2 - 1] = temp;//交换
                }

            }

        }

        /// <summary>
        /// 展开rebardata，并降序排列
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        private static List<Rebar> ListDescend(List<RebarData> _list)
        {
            List<Rebar> _alllist = new List<Rebar>();
            Rebar rebar = new Rebar();

            //将rebardata按照piecenum拆分成每一根钢筋
            foreach (RebarData data in _list)
            {
                for (int i = 0; i < data.TotalPieceNum; i++)
                {
                    rebar = new Rebar();
                    rebar.Copy(data);//先复制
                    rebar.seriNo = i;    //序号
                    //rebar.TaoUsed = false;//是否纳入套料
                    //rebar.PickUsed = false;//未选中
                    rebar.TotalPieceNum = 1;//拆分开后，数量置为1
                    rebar.TotalWeight = data.TotalWeight / data.TotalPieceNum;//重量需要除一下
                    _alllist.Add(rebar);
                }
            }
            _alllist = _alllist.OrderByDescending(t => t.length).ToList();//按照长度降序排序

            return _alllist;
        }
        /// <summary>
        /// 展开rebardata，不做排序
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static List<Rebar> ListExpand(List<RebarData> _list)
        {
            List<Rebar> _alllist = new List<Rebar>();
            Rebar rebar = new Rebar();

            //将rebardata按照piecenum拆分成每一根钢筋
            foreach (RebarData data in _list)
            {
                for (int i = 0; i < data.TotalPieceNum; i++)
                {
                    rebar = new Rebar();
                    rebar.Copy(data);//先复制
                    rebar.seriNo = i;    //序号
                    //rebar.TaoUsed = false;//是否纳入套料
                    //rebar.PickUsed = false;//未选中
                    rebar.TotalPieceNum = 1;//拆分开后，数量置为1
                    rebar.TotalWeight = data.TotalWeight / data.TotalPieceNum;//重量需要除一下
                    _alllist.Add(rebar);
                }
            }
            return _alllist;
        }
        /// <summary>
        /// 展开rebardata，并升序排列
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        private static List<Rebar> ListAscend(List<RebarData> _list)
        {
            List<Rebar> _alllist = new List<Rebar>();
            Rebar rebar = new Rebar();

            //将rebardata按照piecenum拆分成每一根钢筋
            foreach (RebarData data in _list)
            {
                for (int i = 0; i < data.TotalPieceNum; i++)
                {
                    rebar = new Rebar();
                    rebar.Copy(data);//先复制
                    rebar.seriNo = i;    //序号
                    //rebar.TaoUsed = false;//是否纳入套料
                    //rebar.PickUsed = false;//未选中
                    rebar.TotalPieceNum = 1;//拆分开后，数量置为1
                    rebar.TotalWeight = data.TotalWeight / data.TotalPieceNum;//重量需要除一下
                    _alllist.Add(rebar);
                }
            }
            _alllist = _alllist.OrderBy(t => t.length).ToList();//按照长度降序排序

            return _alllist;
        }

        /// <summary>
        /// 将rebar按照长度和边角信息两个变量汇总成rebarPi
        /// </summary>
        /// <param name="_rebarlist"></param>
        /// <returns></returns>
        public static List<RebarPi> PackageRebar(List<Rebar> _rebarlist)
        {
            List<RebarPi> _rebarPi = _rebarlist.GroupBy(t => new { t.length, t.CornerMessage }).Select(
                y => new RebarPi
                {
                    //length = y.Key.length,
                    _rebarList = y.ToList(),
                }
                ).ToList();

            return _rebarPi;
        }
        /// <summary>
        /// 批量锯切场景下，将rebarOri的list转换为rebarPiOri
        /// </summary>
        /// <param name="_oriList"></param>
        /// <returns></returns>
        public static List<RebarOriPi> ExchangeRebarOri(List<RebarOri> _oriList)
        {
            //注意此处的泛型委托func写法，func<输入类型，输出类型>
            Func<List<Rebar>, string> msglist = x =>
            {
                string sss = "";
                foreach (var ttt in x)
                {
                    sss += ttt.CornerMessage + ttt.SerialNum;//拼接cornerMessage，20250812增加serialNum的筛选
                    //sss += ttt.CornerMessage;//拼接cornerMessage，20250812增加serialNum的筛选

                }
                return sss;
            };

            //此处使用func委托类型创建一个复杂的键选择器，目的是根据rebarOri中所有rebar边角信息的拼接来进行筛选
            var temp = _oriList.GroupBy(p => new { cornerMsgkey = msglist(p._list) }).Select(
                            y => new RebarOriPi
                            {
                                //cornerMsgList = y.Key,
                                //num = y.Count(),
                                _list = y.ToList()
                            }
                            ).ToList();

            return temp;
        }


        /// <summary>
        /// 首次适应算法FFD（first fit） 
        /// 算法思路：
        /// 1、配合降序排序，先把长的塞进背包
        /// 2、后面每根钢筋塞入时，遍历所有已生成的背包，找到首次塞得下的背包塞入
        /// </summary>
        /// <param name="_alllist"></param>
        /// <returns></returns>
        private static List<RebarOri> Algorithm_FFD(List<Rebar> _alllist)
        {
            List<RebarOri> _returnlist = new List<RebarOri>();
            RebarOri _temp = new RebarOri(_alllist.First().Level, _alllist.First().Diameter);

            foreach (var item in _alllist)//取一根钢筋过来
            {
                if (_returnlist.Count == 0)//原材list为空，新增一根原材
                {
                    _temp._list.Add(item);
                    _returnlist.Add(_temp);
                }
                else
                {
                    foreach (var ttt in _returnlist)//遍历所有原材
                    {
                        if ((ttt._lengthListUsed + item.length) <= GeneralClass.OriginalLength(item.Level, item.Diameter))//找到长度塞的下的原材
                        {
                            ttt._list.Add(item);//塞进去就break
                            break;
                        }
                        else
                        {
                            if (ttt == _returnlist.Last())//如果是最后一根原材了，还是塞不进去，就新建一根原材
                            {
                                _temp = new RebarOri(_alllist.First().Level, _alllist.First().Diameter);
                                _temp._list.Add(item);
                                _returnlist.Add(_temp);
                                break;
                            }
                            else
                            {
                                continue;//塞不进去，找下一根原材
                            }

                        }
                    }
                }
            }

            return _returnlist;
        }


        /// <summary>
        /// 最佳适应算法BFD（best fit），匹配升序
        /// 算法思路：
        /// 1、遍历所有已生成的背包，如果塞得下就塞
        /// 2、塞不下就拿当前钢筋跟背包中最长的做对比，如果当前钢筋更长，就替换掉
        /// </summary>
        /// <param name="_alllist"></param>
        /// <returns></returns>
        private static List<RebarOri> Algorithm_BFD(List<Rebar> _alllist)
        {
            List<RebarOri> _returnlist = new List<RebarOri>();
            RebarOri _temp = new RebarOri(_alllist.First().Level, _alllist.First().Diameter);

            while (_alllist.Count > 0)
            {
                Thread.Sleep(1);

                foreach (var item in _alllist.ToArray())//取一根钢筋过来,注意此处为 _alllist.ToArray()，为一个拷贝版本，因为后面会修改_alllist
                {
                    if (_returnlist.Count == 0)//原材list为空，新增一根原材
                    {
                        _temp = new RebarOri(_alllist.First().Level, _alllist.First().Diameter);
                        _temp._list.Add(item);
                        _returnlist.Add(_temp);

                        _alllist.Remove(item);//已经分配的从_alllist移除
                    }
                    else
                    {
                        foreach (var ttt in _returnlist)//遍历所有原材
                        {
                            if ((ttt._lengthListUsed + item.length) <= GeneralClass.OriginalLength(item.Level, item.Diameter))//找到长度塞的下的原材
                            {
                                ttt._list.Add(item);//塞进去就break
                                _alllist.Remove(item);//已经分配的从_alllist移除
                                break;
                            }
                            else if (item.length > ttt._list.Max(t => t.length)//原材空缺中塞不下，但比当前原材中已排好的钢筋更长，并且替换后不会超出原材长度，就可以替换
                                && (item.length - ttt._list.Max(t => t.length)) < ttt._lengthFirstLeft)
                            {
                                Rebar exchange = ttt._list.OrderBy(t => t.length).Last();//取最大值的元素
                                ttt._list[ttt._list.IndexOf(exchange)] = item;
                                _alllist[_alllist.IndexOf(item)] = exchange;//注意此处交换的执行，是更改_allList中的元素，但不做移除

                                break;
                            }
                            else
                            {
                                if (ttt == _returnlist.Last())//如果是最后一根原材了，还是塞不进去，就新建一根原材
                                {
                                    _temp = new RebarOri(_alllist.First().Level, _alllist.First().Diameter);
                                    _temp._list.Add(item);
                                    _returnlist.Add(_temp);

                                    _alllist.Remove(item);//已经分配的从_alllist移除

                                    break;
                                }
                                else
                                {
                                    continue;//塞不进去，找下一根原材
                                }

                            }
                        }
                    }
                }
            }

            return _returnlist;
        }


        /// <summary>
        /// 找一下所需的长度在原材库中是否找得到，或比较接近,目前差值阈值设为500
        /// </summary>
        /// <param name="_rebar">所需的长度</param>
        /// <param name="_material">原材库</param>
        /// <param name="_lengthInMaterial">返回所用的原材库长度</param>
        /// <param name="_threshold">阈值,默认为0</param>
        /// <returns></returns>
        private static bool IfContain(Rebar _rebar, List<MaterialOri> _material, out int _lengthInMaterial, int _threshold = 0)
        {

            foreach (var item in _material.FindAll(t => t._level == _rebar.Level && t._diameter == GeneralClass.IntToEnumDiameter(_rebar.Diameter)))//20250104修改bug，增加级别和直径判断，以免出现12米、9米原材混淆的问题
            {
                //if (_rebar.Diameter != GeneralClass.EnumDiameterToInt(item._diameter))//直径不对，跳过
                //{
                //    continue;
                //}
                if ((item._length - _rebar.length) >= 0 && (item._length - _rebar.length) <= _threshold)//所需长度与原材库的长度相差:0≤x＜500
                {
                    _lengthInMaterial = item._length;
                    return true;
                }
            }
            _lengthInMaterial = 0;
            return false;
        }
        /// <summary>
        /// 找一下所需的rebar序列总长度在原材库中是否找得到，或比较接近,差值低于阈值
        /// </summary>
        /// <param name="_rebarlist"></param>
        /// <param name="_material">原材库</param>
        /// <param name="_lengthInMaterial">返回所使用的原材</param>
        /// <param name="_threshold">阈值</param>
        /// <returns></returns>
        private static bool IfContain(List<Rebar> _rebarlist, List<MaterialOri> _material, out int _lengthInMaterial, int _threshold = 0)
        {
            foreach (var item in _material)
            {
                if (_rebarlist[0].Diameter != GeneralClass.EnumDiameterToInt(item._diameter))//直径不对，跳过
                {
                    continue;
                }
                if ((item._length - _rebarlist.Sum(t => t.length)) >= 0 && (item._length - _rebarlist.Sum(t => t.length)) <= _threshold)//所需长度与原材库的长度相差:0≤x＜500
                {
                    _lengthInMaterial = item._length;
                    return true;
                }
            }
            _lengthInMaterial = 0;
            return false;
        }
        /// <summary>
        /// 判断rebar序列的总长度是否与原材长度接近，或差值低于阈值
        /// </summary>
        /// <param name="_rebarlist"></param>
        /// <param name="_material">原材</param>
        /// <param name="_threshold">阈值</param>
        /// <returns></returns>
        private static bool IfContain(List<Rebar> _rebarlist, MaterialOri _material, int _threshold = 0)
        {
            if ((_material._length - _rebarlist.Sum(t => t.length)) >= 0 && (_material._length - _rebarlist.Sum(t => t.length)) <= _threshold)//所需长度与原材库的长度相差:0≤x＜500
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 首次适应算法FFD（first fit） 的改进版V1.0版,
        /// 原材均为使用9m或12米定尺原材，针对最后一根余料较长的钢筋做优化
        /// </summary>
        /// <param name="_alllist"></param>
        /// <returns></returns>
        private static List<RebarOri> Algorithm_FFD_1(List<Rebar> _alllist)
        {
            List<RebarOri> _returnlist = new List<RebarOri>();
            RebarOri _temp = new RebarOri(_alllist.First().Level, _alllist.First().Diameter);

            _returnlist.AddRange(Tao(ref _alllist, GeneralClass.OriginalLength(_alllist[0].Level, _alllist[0].Diameter), 99999, true));//允许超过原材长度

            ////首次套料
            //foreach (var item in _alllist)//取一根钢筋过来
            //{
            //    if (_returnlist.Count == 0)//原材list为空，新增一根原材
            //    {
            //        _temp = new RebarOri();
            //        _temp._list.Add(item);
            //        _returnlist.Add(_temp);
            //    }
            //    else
            //    {
            //        foreach (var ttt in _returnlist)//遍历所有原材
            //        {
            //            if ((ttt._lengthListUsed + item.length) <= ttt._totalLength/*GeneralClass.OriginalLength*/)//找到长度塞的下的原材
            //            {
            //                ttt._list.Add(item);//塞进去就break
            //                break;
            //            }
            //            else
            //            {
            //                if (ttt == _returnlist.Last())//如果是最后一根原材了，还是塞不进去，就新建一根原材
            //                {
            //                    _temp = new RebarOri();
            //                    _temp._list.Add(item);
            //                    _returnlist.Add(_temp);
            //                    break;
            //                }
            //                else
            //                {
            //                    continue;//塞不进去，找下一根原材
            //                }

            //            }
            //        }
            //    }
            //}

            //最后一根的尾料做处理
            CutTail(ref _returnlist);

            return _returnlist;
        }
        /// <summary>
        /// 首次适应算法FFD（first fit） 的改进版V2.0版,
        /// 引入原材库的概念，其中包含有9m或12m定尺原材，也包含有3m、4m、5m、6m、7m等非定尺材，20240223
        /// </summary>
        /// <param name="_alllist">待加工的rebarlist</param>
        /// <param name="_material">原材库</param>
        /// <returns></returns>
        private static List<RebarOri> Algorithm_FFD_2(List<Rebar> _alllist, List<MaterialOri> _material)
        {
            string _level = _alllist.First().Level;
            int _diameter = _alllist.First().Diameter;

            List<RebarOri> _returnlist = new List<RebarOri>();
            RebarOri _temp = new RebarOri(_level, _diameter);

            //先排完全不用切的，跟原材库长度完全一致的，主要考虑到柱构件中的3米
            _material = _material.OrderBy(t => t._length).ToList();//先升序，排阈值0的
            if (_alllist != null && _alllist.Count != 0)
            {
                _returnlist.AddRange(Tao_1(ref _alllist, _material, 0, 0));//0
            }
            //foreach (var item in _material)
            //{
            //    _returnlist.AddRange(Tao(ref _alllist, item._length, 0));
            //    if (_alllist.Count == 0)
            //    {
            //        break;
            //    }
            //}

            //再排需要套料的
            if (_alllist != null && _alllist.Count != 0)
            {
                _returnlist.AddRange(Tao(ref _alllist, GeneralClass.OriginalLength(_level, _diameter), 99999, true));//允许超过原材长度
            }

            ////如果最后还没排完，按区间去丢
            //if (_alllist.Count != 0)
            //{
            //    GeneralClass.interactivityData?.printlog(1, "原材库没排完！！用原材库区间丢");

            //    foreach (var item in _alllist)
            //    {
            //        _temp = new RebarOri(lengthBetween(_material, item.length)._length);//查询所在的原材区间，并新建一个对应长度的原材
            //        _temp._list.Add(item);
            //        _returnlist.Add(_temp);
            //    }
            //}


            ////对已经排好的，做筛选，余料长度太长的，都要重新用合适长度的原材库替换
            //for(int i =0;i<_returnlist.Count;i++)
            //{
            //    _temp = new RebarOri(lengthBetween(_material, _returnlist[i]._lengthListUsed)._length, _level, _diameter);
            //    _temp._list.AddRange(_returnlist[i]._list);
            //    _returnlist[i] = _temp;//重新替换
            //}
            //20241124关闭

            ////最后一根的尾料做处理
            //CutTail(ref _returnlist);

            return _returnlist;
        }

        /// <summary>
        /// 首次适应算法FFD（first fit） 的改进版V3.0版,
        /// 1、非定尺原材也纳入综合套料，对于余料有所控制
        /// 2、引入原材库的概念，其中包含有9m或12m定尺原材，也包含有3m、4m、5m、6m、7m等非定尺材，20240223
        /// 3、新增综合套料Tao函数
        /// </summary>
        /// <param name="_alllist">待加工的rebarlist</param>
        /// <param name="_material">原材库</param>
        /// <returns></returns>
        private static List<RebarOri> Algorithm_FFD_3(List<Rebar> _alllist, List<MaterialOri> _material)
        {
            if (_material.Count == 0)
            {
                GeneralClass.interactivityData?.printlog(1, "原材库为空");
                return new List<RebarOri>();
            }
            List<RebarOri> _returnlist = new List<RebarOri>();
            RebarOri _temp = new RebarOri(_alllist.First().Level, _alllist.First().Diameter);

            //首次套料，用非定尺原材
            //foreach (var item in _alllist)//取一根钢筋过来
            for (int i = _alllist.Count - 1; i >= 0; i--)
            {
                int _lengthUsed = 0;
                if (IfContain(_alllist[i], _material, out _lengthUsed, GeneralClass.CfgData.LeftThreshold))//先找原材库中是否有合适长度的
                {
                    _temp = new RebarOri(_lengthUsed, _alllist.First().Level, _alllist.First().Diameter);//新建一个指定长度的原材
                    _temp._list.Add(_alllist[i]);
                    _returnlist.Add(_temp);

                    _alllist.RemoveAt(i);//去掉已经用的
                    continue;
                }
            }



            //二次套料，处理定尺原材尾料大于1000的，用非定尺原材来套料

            for (int _threshold = GeneralClass.CfgData.LeftThreshold; _threshold <= GeneralClass.CfgData.MaxThreshold; _threshold += 100)
            {
                //如果所有原材套一遍，还有剩余的，则放开阈值,保证
                if (_alllist.Count != 0)
                {
                    //提取直径一致的原材按照长度降序排列
                    _material = _material.Where(t => t._diameter == GeneralClass.IntToEnumDiameter(_alllist[0].Diameter)).ToList().OrderByDescending(k => k._length).ToList();

                    foreach (var item in _material)
                    {
                        _returnlist.AddRange(Tao(ref _alllist, item._length, _threshold));//0
                        if (_alllist.Count == 0)
                        {
                            break;
                        }
                    }
                }
            }

            if (_alllist.Count != 0)//如果最后allList还有剩余的，一般是长度大于9m或12m的，此为料单问题，暂时用一根原材来排
            {
                GeneralClass.interactivityData?.printlog(1, "料单异常，钢筋长度大于原材长度");
                foreach (var item in _alllist)
                {
                    _temp = new RebarOri(_alllist.First().Level, _alllist.First().Diameter);//新建一个原材
                    _temp._list.Add(item);
                    _returnlist.Add(_temp);
                }
            }

            _returnlist = _returnlist.OrderByDescending(t => t._totalLength).ToList();//最后排序一下，按照余料从少到多排序


            //最后一根的尾料做处理
            //CutTail(ref _returnlist);//20240226暂时关闭

            return _returnlist;
        }
        /// <summary>
        /// 首次适应算法FFD（first fit） 的改进版V4.0版,
        /// 1、弱化长度套料
        /// 2、突出批量锯切，
        /// 3、尽可能减少单根锯切的频次
        /// 4、余料尽可能短，最好都短于1000
        /// </summary>
        /// <param name="_alllist"></param>
        /// <param name="_material"></param>
        /// <returns></returns>
        private static List<RebarOri> Algorithm_FFD_4(List<Rebar> _alllist, List<MaterialOri> _material)
        {
            if (_material.Count == 0)
            {
                GeneralClass.interactivityData?.printlog(1, "原材库为空");
                return new List<RebarOri>();
            }
            List<RebarOri> _returnlist = new List<RebarOri>();
            RebarOri _temp = new RebarOri(_alllist.First().Level, _alllist.First().Diameter);

            //_alllist = _alllist.OrderByDescending(t => t.length).ToList();//先排序 ，从长到短
            _alllist = _alllist.OrderBy(t => t.length).ToList();//先排序 ，从短到长

            //首次套料，用非定尺原材
            for (int i = _alllist.Count - 1; i >= 0; i--)//先把长度与非定尺原材完全一致的找出来
            {
                int _lengthUsed = 0;
                if (IfContain(_alllist[i], _material, out _lengthUsed))//先找原材库中是否有合适长度的
                {
                    _temp = new RebarOri(_lengthUsed, _alllist.First().Level, _alllist.First().Diameter);//新建一个指定长度的原材
                    _temp._list.Add(_alllist[i]);
                    _returnlist.Add(_temp);

                    _alllist.RemoveAt(i);//去掉已经用的
                    continue;
                }
            }

            //临时list，用于排序
            List<RebarOri> _returnTemp = new List<RebarOri>();

            //二次套料，处理定尺原材尾料大于1000的，用非定尺原材来套料
            for (int _threshold = 100; _threshold <= GeneralClass.CfgData.MaxThreshold; _threshold += 100)
            {
                for (int i = _alllist.Count - 1; i >= 0; i--)//先把长度与非定尺原材完全一致的找出来
                {
                    int _lengthUsed = 0;
                    if (IfContain(_alllist[i], _material, out _lengthUsed, _threshold))//先找原材库中是否有合适长度的
                    {
                        _temp = new RebarOri(_lengthUsed, _alllist.First().Level, _alllist.First().Diameter);//新建一个指定长度的原材
                        _temp._list.Add(_alllist[i]);
                        _returnTemp.Add(_temp);

                        _alllist.RemoveAt(i);//去掉已经用的
                        continue;
                    }
                }
            }
            if (_alllist.Count != 0)//如果最后allList还有剩余的，一般是长度大于9m或12m的，此为料单问题，暂时用一根原材来排
            {
                GeneralClass.interactivityData?.printlog(1, "料单异常，钢筋长度大于原材长度");
                foreach (var item in _alllist)
                {
                    _temp = new RebarOri(_alllist.First().Level, _alllist.First().Diameter);//新建一个原材
                    _temp._list.Add(item);
                    _returnlist.Add(_temp);
                }
            }

            _returnTemp = _returnTemp.OrderByDescending(t => t._totalLength).ToList();//按照总长度排序
            //_returnTemp = _returnTemp.OrderByDescending(t => t._lengthUsed).ToList();//按照已使用长度排序
            //_returnTemp = _returnTemp.OrderByDescending(t => t._lengthLeft).ToList();//按照剩余长度排序

            _returnlist.AddRange(_returnTemp);



            return _returnlist;
        }

        /// <summary>
        /// 首次适应算法FFD（first fit） 的改进版V5.0版,  递归算法   20240306
        /// 1、先用各个非定尺原材，排一根不切或切的余料很少
        /// 2、再排两根组合的
        /// 3、再排三根组合的
        /// 4、。。。
        /// 5、设置一个余料阈值，逐渐放开
        /// </summary>
        /// <param name="_alllist"></param>
        /// <param name="_material"></param>
        /// <returns></returns>
        private static List<RebarOri> Algorithm_FFD_5(List<Rebar> _alllist, List<MaterialOri> _material)
        {
            if (_material.Count == 0)
            {
                GeneralClass.interactivityData?.printlog(1, "原材库为空");
                return new List<RebarOri>();
            }
            List<RebarOri> _returnlist = new List<RebarOri>();
            RebarOri _temp = new RebarOri(_alllist.First().Level, _alllist.First().Diameter);

            _alllist = _alllist.OrderBy(t => t.length).ToList();//先排序 ，从短到长
            //_alllist = _alllist.OrderByDescending(t => t.length).ToList();//先排序 ，从长到短

            //提取直径一致的原材库按照长度降序排列
            if (_alllist.Count != 0)
            {
                _material = _material.Where(t => t._diameter == GeneralClass.IntToEnumDiameter(_alllist[0].Diameter)).Distinct().ToList().OrderByDescending(k => k._length).ToList();
            }

            GeneralClass.interactivityData?.printlog(1, "1");

            //先提取与原材库完全一致的
            if (_alllist.Count != 0)
            {
                //_returnlist.AddRange(Tao(ref _alllist, _material, _piece, _threshold));//0
                _returnlist.AddRange(Tao_1(ref _alllist, _material, 0, 0));//0
                _returnlist.AddRange(Tao_2(ref _alllist, _material, 0, 0));//0
                _returnlist.AddRange(Tao_3(ref _alllist, _material, 0, 0));//0
            }

            GeneralClass.interactivityData?.printlog(1, "2");

            //卡小阈值，排一遍
            if (_alllist.Count != 0)
            {
                //_returnlist.AddRange(Tao(ref _alllist, _material, _piece, _threshold));//0
                _returnlist.AddRange(Tao_1(ref _alllist, _material, 0, GeneralClass.CfgData.LeftThreshold /*_threshold*/));//0
                _returnlist.AddRange(Tao_2(ref _alllist, _material, 0, GeneralClass.CfgData.LeftThreshold /*_threshold*/));//0
                _returnlist.AddRange(Tao_3(ref _alllist, _material, 0, GeneralClass.CfgData.LeftThreshold /*_threshold*/));//0
            }

            GeneralClass.interactivityData?.printlog(1, "3");

            //放大阈值，再排一次
            if (_alllist.Count != 0)
            {
                //_returnlist.AddRange(Tao(ref _alllist, _material, _piece, _threshold));//0
                _returnlist.AddRange(Tao_1(ref _alllist, _material, GeneralClass.CfgData.LeftThreshold, GeneralClass.CfgData.MaxThreshold/*_threshold*/));//0
                _returnlist.AddRange(Tao_2(ref _alllist, _material, GeneralClass.CfgData.LeftThreshold, GeneralClass.CfgData.MaxThreshold/*_threshold*/));//0
                _returnlist.AddRange(Tao_3(ref _alllist, _material, GeneralClass.CfgData.LeftThreshold, GeneralClass.CfgData.MaxThreshold/*_threshold*/));//0

            }

            GeneralClass.interactivityData?.printlog(1, "4");

            //如果最后allList还有剩余的，用背包算法排，保证余料尽可能长一点
            if (_alllist.Count != 0)
            {
                GeneralClass.interactivityData?.printlog(1, "卡阈值结束，没排完！！开始用原材套");

                foreach (var item in _material)
                {
                    _returnlist.AddRange(Tao(ref _alllist, item._length, 3000));
                    if (_alllist.Count == 0)
                    {
                        break;
                    }
                }
            }

            GeneralClass.interactivityData?.printlog(1, "5");

            //如果最后还没排完，按区间去丢
            if (_alllist.Count != 0)
            {
                GeneralClass.interactivityData?.printlog(1, "原材没排完！！用区间丢");

                foreach (var item in _alllist)
                {
                    ////提取直径一致的原材按照长度降序排列
                    //_material = _material.Where(t => t._diameter == GeneralClass.IntToEnumDiameter(_alllist[0].Diameter)).ToList().OrderByDescending(k => k._length).ToList();

                    _temp = new RebarOri(lengthBetween(_material, item.length)._length, _alllist.First().Level, _alllist.First().Diameter);//查询所在的原材区间，并新建一个对应长度的原材
                    _temp._list.Add(item);
                    _returnlist.Add(_temp);
                }
            }
            //GeneralClass.interactivityData?.printlog(1, "6");


            return _returnlist;
        }
        /// <summary>
        /// 二叉树算法，
        /// </summary>
        /// <param name="_alllist"></param>
        /// <param name="_material"></param>
        /// <returns></returns>
        private static List<RebarOri> Algorithm_FFD_6(List<Rebar> _alllist, List<MaterialOri> _material)
        {
            if (_material.Count == 0)
            {
                GeneralClass.interactivityData?.printlog(1, "原材库为空");
                return new List<RebarOri>();
            }
            List<RebarOri> _returnlist = new List<RebarOri>();
            RebarOri _temp = new RebarOri(_alllist.First().Level, _alllist.First().Diameter);

            _alllist = _alllist.OrderBy(t => t.length).ToList();//先排序 ，从短到长
            //_alllist = _alllist.OrderByDescending(t => t.length).ToList();//先排序 ，从长到短

            //提取直径一致的原材库按照长度降序排列
            if (_alllist.Count != 0)
            {
                _material = _material.Where(t => t._diameter == GeneralClass.IntToEnumDiameter(_alllist[0].Diameter)).Distinct().ToList().OrderByDescending(k => k._length).ToList();
            }

            GeneralClass.interactivityData?.printlog(1, "1");

            //先提取与原材库完全一致的
            if (_alllist.Count != 0)
            {
                _returnlist.AddRange(Tao_1_tree(ref _alllist, _material, 0, 0));
                _returnlist.AddRange(Tao_2_tree(ref _alllist, _material, 0, 0));//卡阈值0，用升序
                //_returnlist.AddRange(Tao_3_tree(ref _alllist, _material, 0, 0));
            }

            GeneralClass.interactivityData?.printlog(1, "2");

            //卡小阈值，排一遍
            if (_alllist.Count != 0)
            {
                _returnlist.AddRange(Tao_1_tree(ref _alllist, _material, 0, GeneralClass.CfgData.LeftThreshold));
                _returnlist.AddRange(Tao_2_tree(ref _alllist, _material, 0, GeneralClass.CfgData.LeftThreshold, false));//降序
                //_returnlist.AddRange(Tao_3_tree(ref _alllist, _material, 0, GeneralClass.CfgData.LeftThreshold));
            }

            GeneralClass.interactivityData?.printlog(1, "3");

            //放大阈值，再排一次
            if (_alllist.Count != 0)
            {
                _returnlist.AddRange(Tao_1_tree(ref _alllist, _material, GeneralClass.CfgData.LeftThreshold, GeneralClass.CfgData.MaxThreshold));
                _returnlist.AddRange(Tao_2_tree(ref _alllist, _material, GeneralClass.CfgData.LeftThreshold, GeneralClass.CfgData.MaxThreshold, false));
                //_returnlist.AddRange(Tao_3_tree(ref _alllist, _material, GeneralClass.CfgData.LeftThreshold, GeneralClass.CfgData.MaxThreshold));
            }

            GeneralClass.interactivityData?.printlog(1, "4");

            //如果最后allList还有剩余的，用背包算法排，保证余料尽可能长一点
            if (_alllist.Count != 0)
            {
                GeneralClass.interactivityData?.printlog(1, "卡阈值结束，没排完！！开始用原材库套");

                foreach (var item in _material)
                {
                    _returnlist.AddRange(Tao(ref _alllist, item._length, GeneralClass.CfgData.LeftThreshold));
                    _returnlist.AddRange(Tao(ref _alllist, item._length, GeneralClass.CfgData.MaxThreshold));
                    _returnlist.AddRange(Tao(ref _alllist, item._length, 400));
                    _returnlist.AddRange(Tao(ref _alllist, item._length, 500));
                    _returnlist.AddRange(Tao(ref _alllist, item._length, 700));
                    _returnlist.AddRange(Tao(ref _alllist, item._length, 1000));
                    _returnlist.AddRange(Tao(ref _alllist, item._length, 3000));


                    if (_alllist.Count == 0)
                    {
                        break;
                    }
                }
            }

            GeneralClass.interactivityData?.printlog(1, "5");

            //如果最后还没排完，按区间去丢
            if (_alllist.Count != 0)
            {
                GeneralClass.interactivityData?.printlog(1, "原材库没排完！！用原材库区间丢");

                foreach (var item in _alllist)
                {
                    ////提取直径一致的原材按照长度降序排列
                    //_material = _material.Where(t => t._diameter == GeneralClass.IntToEnumDiameter(_alllist[0].Diameter)).ToList().OrderByDescending(k => k._length).ToList();

                    _temp = new RebarOri(lengthBetween(_material, item.length)._length, item.Level, item.Diameter);//查询所在的原材区间，并新建一个对应长度的原材
                    _temp._list.Add(item);
                    _returnlist.Add(_temp);
                }
            }
            GeneralClass.interactivityData?.printlog(1, "6");
            if (_alllist.Count != 0)
            {
                GeneralClass.interactivityData?.printlog(1, "原材库区间没排完！！用定尺原材套");

                _returnlist.AddRange(Tao(ref _alllist, GeneralClass.OriginalLength(_alllist[0].Level, _alllist[0].Diameter)));

            }

            return _returnlist;
        }

        /// <summary>
        /// 确定当前钢筋长度在原材库的哪个区间，并返回应使用的原材
        /// </summary>
        /// <param name="_ma"></param>
        /// <param name="_length"></param>
        private static MaterialOri lengthBetween(List<MaterialOri> _ma, int _length)
        {

            //提取按照长度降序排列
            _ma = _ma.OrderByDescending(k => k._length).ToList();

            _ma.Add(new MaterialOri(_ma.First()._diameter, 0, 999));//加入一个长度为0的在末尾，防止越界


            if (_length > _ma.First()._length)
            {
                GeneralClass.interactivityData?.printlog(1, "长度=" + _length.ToString() + ",数据异常!");
                return new MaterialOri(_ma.First()._diameter, GeneralClass.OriginalLength(_ma.First()._level, _ma.First()._diameter), 999, _ma.First()._level);//给个原材吧
            }
            for (int i = 0; i < _ma.Count; i++)
            {
                if (_length > _ma[i + 1]._length && _length <= _ma[i]._length)
                {
                    return _ma[i];
                }
            }

            return new MaterialOri(_ma.First()._diameter, GeneralClass.OriginalLength(_ma.First()._level, _ma.First()._diameter), 999, _ma.First()._level);
        }

        /// <summary>
        /// 按照1段钢筋卡阈值，递归算法
        /// </summary>
        /// <param name="_rebarlist"></param>
        /// <param name="_materialPool"></param>
        /// <param name="_minThreshold"></param>
        /// <param name="_maxThreshold"></param>
        /// <returns></returns>
        private static List<RebarOri> Tao_1(ref List<Rebar> _rebarlist, List<MaterialOri> _materialPool, int _minThreshold, int _maxThreshold/*, int _threshold = 99999*/)
        {
            List<RebarOri> _returnlist = new List<RebarOri>();
            RebarOri _temp = new RebarOri(_rebarlist.First().Level, _rebarlist.First().Diameter);
            int _lengthUSE = 0;

            for (int i = _rebarlist.Count - 1; i >= 0; i--)//注意逆序 检索，因rebarlist会删除部分元素
            {
                for (int _th = _minThreshold; _th <= _maxThreshold; _th += 100)//余料阈值逐渐放开
                {
                    if (IfContain(_rebarlist[i], _materialPool, out _lengthUSE, _th))
                    {
                        _temp = new RebarOri(_lengthUSE, _rebarlist.First().Level, _rebarlist.First().Diameter);
                        _temp._list.Add(_rebarlist[i]);
                        _returnlist.Add(_temp);

                        _rebarlist.Remove(_rebarlist[i]);//删除已排的
                        break;
                    }
                }
            }
            return _returnlist;
        }
        /// <summary>
        /// 按照2段钢筋卡阈值，递归算法
        /// </summary>
        /// <param name="_rebarlist"></param>
        /// <param name="_materialPool"></param>
        /// <param name="_minThreshold"></param>
        /// <param name="_maxThreshold"></param>
        /// <returns></returns>
        private static List<RebarOri> Tao_2(ref List<Rebar> _rebarlist, List<MaterialOri> _materialPool, int _minThreshold, int _maxThreshold/*, int _threshold = 99999*/)
        {
            List<RebarOri> _returnlist = new List<RebarOri>();
            RebarOri _temp = new RebarOri(_rebarlist.First().Level, _rebarlist.First().Diameter);
            int _lengthUSE = 0;

            List<Rebar> _tttlist = new List<Rebar>();

            for (int i = _rebarlist.Count - 1; i >= 0; i--)//注意逆序 检索，因rebarlist会删除部分元素
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (_rebarlist.Count <= j || _rebarlist.Count <= i)
                    {
                        //return new List<RebarOri>();
                        break;
                    }
                    _tttlist = new List<Rebar>();
                    _tttlist.Add(_rebarlist[i]);
                    _tttlist.Add(_rebarlist[j]);

                    for (int _th = _minThreshold; _th <= _maxThreshold; _th += 100)//余料阈值逐渐放开
                    {
                        if (IfContain(_tttlist, _materialPool, out _lengthUSE, _th))
                        {
                            _temp = new RebarOri(_lengthUSE, _rebarlist.First().Level, _rebarlist.First().Diameter);
                            _temp._list.Add(_rebarlist[i]);
                            _temp._list.Add(_rebarlist[j]);

                            _returnlist.Add(_temp);

                            _rebarlist.Remove(_rebarlist[i]);//删除已排的
                            _rebarlist.Remove(_rebarlist[j]);//删除已排的

                            //Tao_2(ref _rebarlist, _materialPool, _threshold);//递归
                            _returnlist.AddRange(Tao_2(ref _rebarlist, _materialPool, _minThreshold, _maxThreshold/*, _threshold*/));//递归
                            break;
                        }
                    }

                }
            }
            return _returnlist;
        }
        /// <summary>
        /// 按照3段钢筋卡阈值，递归算法
        /// </summary>
        /// <param name="_rebarlist"></param>
        /// <param name="_materialPool"></param>
        /// <param name="_minThreshold"></param>
        /// <param name="_maxThreshold"></param>
        /// <returns></returns>
        private static List<RebarOri> Tao_3(ref List<Rebar> _rebarlist, List<MaterialOri> _materialPool, int _minThreshold, int _maxThreshold/*, int _threshold = 99999*/)
        {
            List<RebarOri> _returnlist = new List<RebarOri>();
            RebarOri _temp = new RebarOri(_rebarlist.First().Level, _rebarlist.First().Diameter);
            int _lengthUSE = 0;

            List<Rebar> _tttlist = new List<Rebar>();

            for (int i = _rebarlist.Count - 1; i >= 0; i--)//注意逆序 检索，因rebarlist会删除部分元素
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    for (int k = j - 1; k >= 0; k--)
                    {
                        if (_rebarlist.Count <= k || _rebarlist.Count <= j || _rebarlist.Count <= i)
                        {
                            //return new List<RebarOri>();
                            break;
                        }
                        _tttlist = new List<Rebar>();
                        _tttlist.Add(_rebarlist[i]);
                        _tttlist.Add(_rebarlist[j]);
                        _tttlist.Add(_rebarlist[k]);

                        for (int _th = _minThreshold; _th <= _maxThreshold; _th += 100)//余料阈值逐渐放开
                        {
                            if (IfContain(_tttlist, _materialPool, out _lengthUSE, _th))
                            {
                                _temp = new RebarOri(_lengthUSE, _rebarlist.First().Level, _rebarlist.First().Diameter);
                                _temp._list.Add(_rebarlist[i]);
                                _temp._list.Add(_rebarlist[j]);
                                _temp._list.Add(_rebarlist[k]);
                                _returnlist.Add(_temp);

                                _rebarlist.Remove(_rebarlist[i]);//删除已排的
                                _rebarlist.Remove(_rebarlist[j]);//删除已排的
                                _rebarlist.Remove(_rebarlist[k]);//删除已排的

                                //Tao_3(ref _rebarlist, _materialPool, _threshold);//递归
                                _returnlist.AddRange(Tao_3(ref _rebarlist, _materialPool, _minThreshold, _maxThreshold/*, _threshold*/));//递归
                                break;
                            }
                        }
                    }
                }
            }
            return _returnlist;
        }

        /// <summary>
        /// 原材库二叉树算法，最多切一根
        /// </summary>
        /// <param name="_rebarlist"></param>
        /// <param name="_materialPool"></param>
        /// <param name="_minThreshold"></param>
        /// <param name="_maxThreshold"></param>
        /// <returns></returns>
        private static List<RebarOri> Tao_1_tree(ref List<Rebar> _rebarlist, List<MaterialOri> _materialPool, int _minThreshold, int _maxThreshold/*, int _threshold = 99999*/)
        {
            List<RebarOri> _returnlist = new List<RebarOri>();
            RebarOri _temp = new RebarOri(_rebarlist.First().Level, _rebarlist.First().Diameter);
            int _lengthUSE = 0;

            for (int i = _rebarlist.Count - 1; i >= 0; i--)//注意逆序 检索，因rebarlist会删除部分元素
            {
                for (int _th = _minThreshold; _th <= _maxThreshold; _th += 100)//余料阈值逐渐放开
                {
                    if (IfContain(_rebarlist[i], _materialPool, out _lengthUSE, _th))
                    {
                        _temp = new RebarOri(_lengthUSE, _rebarlist.First().Level, _rebarlist.First().Diameter);
                        _temp._list.Add(_rebarlist[i]);
                        _returnlist.Add(_temp);

                        _rebarlist.Remove(_rebarlist[i]);//删除已排的
                        break;
                    }
                }
            }
            return _returnlist;
        }
        /// <summary>
        /// 原材库二叉树算法，最多切两根
        /// </summary>
        /// <param name="_rebarlist"></param>
        /// <param name="_materialPool"></param>
        /// <param name="_minThreshold"></param>
        /// <param name="_maxThreshold"></param>
        /// <param name="_ascend">原材库升序OR降序,TRUE升序，FALSE降序</param>
        /// <returns></returns>
        private static List<RebarOri> Tao_2_tree(ref List<Rebar> _rebarlist, List<MaterialOri> _materialPool, int _minThreshold, int _maxThreshold, bool _ascend = true/*, int _threshold = 99999*/)
        {
            List<RebarOri> _returnlist = new List<RebarOri>();
            RebarOri _temp = new RebarOri(_rebarlist.First().Level, _rebarlist.First().Diameter);
            //int _lengthUSE = 0;
            bool _find = false;

            //根据_ascend，确定原材库为升序还是降序
            _materialPool = _ascend ? _materialPool.OrderBy(t => t._length).ToList() : _materialPool.OrderByDescending(t => t._length).ToList();

            for (int i = _rebarlist.Count - 1; i >= 0; i--)//注意逆序 检索，因rebarlist会删除部分元素
            {
                BiTreeNode root = Solution.CreateTree(_rebarlist, 0, i - 1);//创建二叉树

                for (int _th = _minThreshold; _th <= _maxThreshold; _th += 100)//余料阈值逐渐放开
                {
                    foreach (var item in _materialPool)
                    {
                        if (item._length < _rebarlist[i].length) continue;//长度比原材还长，跳去下个原材

                        var ttt = Solution.FindNode(root, _rebarlist[i], item, _th);
                        if (ttt != null)
                        {
                            _temp = new RebarOri(item._length, _rebarlist.First().Level, _rebarlist.First().Diameter);     //建立新的rebarOri
                            _temp._list.Add(_rebarlist[i]);
                            _temp._list.Add(ttt.val);

                            _returnlist.Add(_temp);

                            ttt.val.TaoUsed = true;         //套料使用标记置true
                            _rebarlist[i].TaoUsed = true;

                            _find = true;
                        }
                        if (_find) break;
                    }
                    if (_find) break;
                }
                if (_find) break;
            }
            if (_find)//如果找到匹配的，则删除已排的，继续递归匹配
            {
                for (int i = _rebarlist.Count - 1; i >= 0; i--)
                {
                    if (_rebarlist[i].TaoUsed)
                    {
                        _rebarlist.Remove(_rebarlist[i]);
                    }
                }
                _returnlist.AddRange(Tao_2_tree(ref _rebarlist, _materialPool, _minThreshold, _maxThreshold, _ascend));//递归

            }

            return _returnlist;
        }

        /// <summary>
        /// 原材库二叉树算法，最多切三根
        /// </summary>
        /// <param name="_rebarlist"></param>
        /// <param name="_materialPool"></param>
        /// <param name="_minThreshold"></param>
        /// <param name="_maxThreshold"></param>
        /// <returns></returns>
        private static List<RebarOri> Tao_3_tree(ref List<Rebar> _rebarlist, List<MaterialOri> _materialPool, int _minThreshold, int _maxThreshold, bool _ascend = true)
        {
            List<RebarOri> _returnlist = new List<RebarOri>();
            RebarOri _temp = new RebarOri(_rebarlist.First().Level, _rebarlist.First().Diameter);
            //int _lengthUSE = 0;
            bool _find = false;

            //根据_ascend，确定原材库为升序还是降序
            _materialPool = _ascend ? _materialPool.OrderBy(t => t._length).ToList() : _materialPool.OrderByDescending(t => t._length).ToList();

            for (int i = _rebarlist.Count - 1; i >= 0; i--)//注意逆序 检索，因rebarlist会删除部分元素
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    List<Rebar> templist = new List<Rebar>();
                    templist.Add(_rebarlist[i]);
                    templist.Add(_rebarlist[j]);

                    BiTreeNode root = Solution.CreateTree(_rebarlist, 0, j - 1);//创建二叉树

                    for (int _th = _minThreshold; _th <= _maxThreshold; _th += 100)//余料阈值逐渐放开
                    {
                        foreach (var item in _materialPool)
                        {
                            if (item._length < templist.Sum(t => t.length) /*_rebarlist[i].length*/) continue;//长度比原材还长，跳去下个原材

                            var ttt = Solution.FindNode(root, templist /*_rebarlist[i]*/, item, _th);
                            if (ttt != null)
                            {
                                _temp = new RebarOri(item._length, _rebarlist.First().Level, _rebarlist.First().Diameter);     //建立新的rebarOri
                                //_temp._list.Add(_rebarlist[i]);
                                _temp._list.AddRange(templist);
                                _temp._list.Add(ttt.val);

                                _returnlist.Add(_temp);

                                ttt.val.TaoUsed = true;         //套料使用标记置true
                                //_rebarlist[i].TaoUsed = true;
                                foreach (var eee in templist)
                                {
                                    eee.TaoUsed = true;
                                }

                                _find = true;
                            }
                            if (_find) break;
                        }
                        if (_find) break;
                    }
                    if (_find) break;
                }
                if (_find) break;
            }
            if (_find)//如果找到匹配的，则删除已排的，继续递归匹配
            {
                for (int i = _rebarlist.Count - 1; i >= 0; i--)
                {
                    if (_rebarlist[i].TaoUsed)
                    {
                        _rebarlist.Remove(_rebarlist[i]);
                    }
                }
                _returnlist.AddRange(Tao_3_tree(ref _rebarlist, _materialPool, _minThreshold, _maxThreshold, _ascend));//递归

            }

            return _returnlist;
        }




        /// <summary>
        /// 组合，返回所有3个元素的组合可能
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        private static List<List<KeyValuePair<int, Rebar>>> Pick_3(List<Rebar> _list)
        {
            List<List<KeyValuePair<int, Rebar>>> _returnlist = new List<List<KeyValuePair<int, Rebar>>>();
            List<KeyValuePair<int, Rebar>> _return = new List<KeyValuePair<int, Rebar>>();

            for (int i = 0; i < _list.Count; i++)
            {
                for (int j = i + 1; j < _list.Count; j++)
                {
                    for (int k = j + 1; k < _list.Count; k++)
                    {
                        _return = new List<KeyValuePair<int, Rebar>>();
                        _return.Add(new KeyValuePair<int, Rebar>(i, _list[i]));
                        _return.Add(new KeyValuePair<int, Rebar>(j, _list[j]));
                        _return.Add(new KeyValuePair<int, Rebar>(k, _list[k]));
                        _returnlist.Add(_return);
                    }
                }
            }
            return _returnlist;
        }
        /// <summary>
        /// 组合，返回所有2个元素的组合可能
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        private static List<List<KeyValuePair<int, Rebar>>> Pick_2(List<Rebar> _list)
        {
            List<List<KeyValuePair<int, Rebar>>> _returnlist = new List<List<KeyValuePair<int, Rebar>>>();
            List<KeyValuePair<int, Rebar>> _return = new List<KeyValuePair<int, Rebar>>();

            for (int i = 0; i < _list.Count; i++)
            {
                for (int j = i + 1; j < _list.Count; j++)
                {
                    _return = new List<KeyValuePair<int, Rebar>>();
                    _return.Add(new KeyValuePair<int, Rebar>(i, _list[i]));
                    _return.Add(new KeyValuePair<int, Rebar>(j, _list[j]));
                    _returnlist.Add(_return);
                }
            }
            return _returnlist;
        }
        /// <summary>
        /// 组合，返回所有1个元素的组合可能
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        private static List<List<KeyValuePair<int, Rebar>>> Pick_1(List<Rebar> _list)
        {
            List<List<KeyValuePair<int, Rebar>>> _returnlist = new List<List<KeyValuePair<int, Rebar>>>();
            List<KeyValuePair<int, Rebar>> _return = new List<KeyValuePair<int, Rebar>>();

            for (int i = 0; i < _list.Count; i++)
            {
                _return = new List<KeyValuePair<int, Rebar>>();
                _return.Add(new KeyValuePair<int, Rebar>(i, _list[i]));
                _returnlist.Add(_return);
            }
            return _returnlist;
        }

        //private static List<RebarOri> Pick_3(List<Rebar> _list)
        //{
        //    List<RebarOri> _returnlist = new List<RebarOri>();
        //    RebarOri _return = new RebarOri();

        //    for (int i = 0; i < _list.Count; i++)
        //    {
        //        for (int j = i + 1; j < _list.Count; j++)
        //        {
        //            for (int k = j + 1; k < _list.Count; k++)
        //            {
        //                _return = new RebarOri();
        //                _return._list.Add(_list[i]);
        //                _return._list.Add(_list[j]);
        //                _return._list.Add(_list[k]);
        //                _returnlist.Add(_return);
        //            }
        //        }
        //    }
        //    return _returnlist;
        //}
        //private static List<RebarOri> Pick_2(List<Rebar> _list)
        //{
        //    List<RebarOri> _returnlist = new List<RebarOri>();
        //    RebarOri _return = new RebarOri();

        //    for (int i = 0; i < _list.Count; i++)
        //    {
        //        for (int j = i + 1; j < _list.Count; j++)
        //        {
        //            _return = new RebarOri();
        //            _return._list.Add(_list[i]);
        //            _return._list.Add(_list[j]);
        //            _returnlist.Add(_return);
        //        }
        //    }
        //    return _returnlist;
        //}
        //private static List<RebarOri> Pick_1(List<Rebar> _list)
        //{
        //    List<RebarOri> _returnlist = new List<RebarOri>();
        //    RebarOri _return = new RebarOri();

        //    for (int i = 0; i < _list.Count; i++)
        //    {
        //        _return = new RebarOri();
        //        _return._list.Add(_list[i]);
        //        _returnlist.Add(_return);
        //    }
        //    return _returnlist;
        //}



        /// <summary>
        /// 从rebar库中选取所需数目的所有rebar段组合，并返回组合list，即为C[n,k]组合算法
        /// </summary>
        /// <param name="_rebarlist">待选rebar库</param>
        /// <param name="_piece">组合所需的rebar段数量</param>
        /// <returns></returns>
        /// 
        //private static List<List<KeyValuePair<int,Rebar>>> Pick(List<Rebar> _rebarlist, int _piece)
        //{
        //    List<List<KeyValuePair<int, Rebar>>> _returnlist = new List<List<KeyValuePair<int, Rebar>>>();


        //    for (int i = 0; i < (1 << _rebarlist.Count); i++)   //使用二进制法进行C[n,k]的组合算法
        //    {
        //        List<KeyValuePair<int, Rebar>> _temp = new List<KeyValuePair<int, Rebar>>();

        //        for (int j = 0; j < _rebarlist.Count; j++)
        //        {
        //            if (((i >> j) & 1) == 1)
        //            {
        //                KeyValuePair<int, Rebar> _pair = new KeyValuePair<int, Rebar>(j, _rebarlist[j]);
        //                _temp.Add(_pair);
        //            }
        //        }
        //        if (_temp.Count == _piece) //数量够数，为有效
        //        {
        //            _returnlist.Add(_temp);
        //        }
        //    }
        //    return _returnlist;
        //}



        /// <summary>
        /// 用指定长度的原材进行套料，如果套完后，还有余料大于阈值的则返回
        /// </summary>
        /// <param name="_rebarlist">待套料的rebar序列，如果余料大于阈值的剩余部分rebar，也通过此返回</param>
        /// <param name="_materialLength">指定长度的原材，可以是定尺原材，也可以是非定尺原材</param>
        /// <param name="_threshold">余料阈值</param>
        /// <param name="_useOver">是否允许超过原材长度</param>
        /// <returns></returns>
        private static List<RebarOri> Tao(ref List<Rebar> _rebarlist, int _materialLength, int _threshold = 99999, bool _useOver = false)
        {
            List<RebarOri> _returnlist = new List<RebarOri>();
            RebarOri _temp = new RebarOri(_rebarlist.First().Level, _rebarlist.First().Diameter);

            _rebarlist = _rebarlist.OrderBy(t => t.length).ToList();//先升序排序，因为后面是从最后一根开始

            //foreach (var item in _rebarlist)//取一根钢筋过来
            for (int i = _rebarlist.Count - 1; i >= 0; i--)
            {

                if (_returnlist.Count == 0)//原材list为空，新增一根原材
                {
                    _temp = new RebarOri(_materialLength, _rebarlist.First().Level, _rebarlist.First().Diameter);

                    if (_useOver ? true : (_rebarlist[i].length <= _temp._totalLength)) //新增一根原材，也要判定准备塞的长度是否大于原材长度
                    {
                        _temp._list.Add(_rebarlist[i]);
                        _returnlist.Add(_temp);
                        _rebarlist.Remove(_rebarlist[i]);
                    }

                }
                else
                {
                    foreach (var ttt in _returnlist)//遍历所有原材
                    {
                        if ((ttt._lengthListUsed + _rebarlist[i].length) <= ttt._totalLength/*GeneralClass.OriginalLength*/)//找到长度塞的下的原材
                        {
                            ttt._list.Add(_rebarlist[i]);//塞进去就break
                            _rebarlist.Remove(_rebarlist[i]);
                            break;
                        }
                        else
                        {
                            if (ttt == _returnlist.Last())//如果是最后一根原材了，还是塞不进去，就新建一根原材
                            {
                                _temp = new RebarOri(_materialLength, _rebarlist.First().Level, _rebarlist.First().Diameter);
                                if (_useOver ? true : (_rebarlist[i].length <= _temp._totalLength)) //新增一根原材，也要判定准备塞的长度是否大于原材长度
                                {
                                    _temp._list.Add(_rebarlist[i]);
                                    _returnlist.Add(_temp);
                                    _rebarlist.Remove(_rebarlist[i]);//塞进去就break
                                    break;
                                }

                            }
                            else
                            {
                                continue;//塞不进去，找下一根原材
                            }
                        }
                    }
                }
            }

            //对于余料长度大于阈值的，则将rebar返回到rebarlist中
            for (int i = _returnlist.Count - 1; i >= 0; i--)   //注意需要遍历时删除list的元素，使用for循环逆序遍历，以免越界,千万不要使用foreach
            {
                if (_returnlist[i]._lengthFirstLeft > _threshold)
                {
                    foreach (var tt in _returnlist[i]._list)
                    {
                        _rebarlist.Add(tt);//装回去
                    }
                    _returnlist.RemoveAt(i);//删除
                }
            }

            return _returnlist;
        }

        /// <summary>
        /// 最后一根的尾料做处理
        /// </summary>
        /// <param name="_list"></param>
        private static void CutTail(ref List<RebarOri> _list)
        {
            bool _flag = false;

            if (_list.Count != 0 && _list.Last()._lengthFirstLeft > 3000)//如果最后一根的尾料超过2000的处理，2000以下的先不管
            {
                foreach (var item in _list.Last()._list.ToArray())//_returnlist.Last()._list可能会修改，所以用toArray
                {
                    _flag = FindTwoRebarLeftMatch(item, ref _list);
                    if (_flag)
                    {
                        //GeneralClass.interactivityData?.printlog(1, "find match success!");
                        _list.Last()._list.Remove(item);//塞成功了，则从总的list中去掉当前这个小段rebar
                    }

                }
                if (_list.Last()._list.Count == 0)
                {
                    _list.Remove(_list.Last());//如果最后一个钢筋原材已经空了，则去掉

                    //CutTail(ref _list);//递归，20231123暂不加，太卡了
                }
            }
        }
        /// <summary>
        /// 在总的钢筋原材list中，从第一个到倒数第二个钢筋原材，遍历查找是否有可以塞得下当前小段rebar的两条原材
        /// </summary>
        /// <param name="_rebar"></param>
        /// <param name="_returnlist"></param>
        /// <returns></returns>
        private static bool FindTwoRebarLeftMatch(Rebar _rebar, ref List<RebarOri> _returnlist)
        {
            RebarOri _temp1;
            RebarOri _temp2;

            for (int i = 0; i < _returnlist.Count - 1; i++)
            {
                _temp1 = _returnlist[i];
                for (int j = i; j < _returnlist.Count - 1; j++)
                {
                    _temp2 = _returnlist[j];
                    if ((_temp1._lengthFirstLeft + _temp2._lengthFirstLeft) > _rebar.length)
                    {
                        bool _flag = ExchangeJoin(_rebar, ref _temp1, ref _temp2);
                        if (_flag)
                        {
                            //GeneralClass.interactivityData?.printlog(1, "exchange success!");
                            _returnlist[i] = _temp1;//修改的值再传回去
                            _returnlist[j] = _temp2;
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 交换两根原材中的小段位置，插入一个小段
        /// 算法思路：
        /// 1、将插入的小段与原本两个list合并为一个list
        /// 2、问题简化为：如何把一堆数据分成和接近的两组
        /// </summary>
        /// <returns></returns>
        private static bool ExchangeJoin(Rebar _rebar, ref RebarOri _ori1, ref RebarOri _ori2)
        {
            //先合并所有rebar成一个list
            List<Rebar> _allrebar = new List<Rebar>();
            _allrebar.AddRange(_ori1._list);
            _allrebar.AddRange(_ori2._list);
            _allrebar.Add(_rebar);

            _allrebar = _allrebar.OrderByDescending(t => t.length).ToList();//降序排列

            //分成和接近的两组
            List<Rebar> _list1 = new List<Rebar>();
            List<Rebar> _list2 = new List<Rebar>();

            for (int i = 0; i < _allrebar.Count; i++)
            {
                if (_list1.Sum(t => t.length) <= _list2.Sum(t => t.length))
                {
                    _list1.Add(_allrebar[i]);
                }
                else
                {
                    _list2.Add(_allrebar[i]);
                }
            }

            //如果分成两组的长度都没有超过原材长度，则分配成功
            if (_list1.Sum(t => t.length) <= GeneralClass.OriginalLength(_list1.First().Level, _list1.First().Diameter) &&
                _list2.Sum(t => t.length) <= GeneralClass.OriginalLength(_list1.First().Level, _list1.First().Diameter))
            {
                _ori1._list = _list1;//
                _ori2._list = _list2;
                return true;
            }
            else { return false; }

        }
    }
}
