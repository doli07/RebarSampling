using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RebarSampling
{
    /// <summary>
    /// 长度套料算法
    /// </summary>
    public class Algorithm
    {
        /// <summary>
        /// 长度套料算法
        /// </summary>
        /// <param name="_list"></param>
        /// <returns>返回套料后的原材钢筋</returns>
        public static List<RebarOri> Taoliao(List<RebarData> _list, out int _totallength)
        {
            List<Rebar> _alllist = DescendList(_list);//降序展开
                                                      //List<Rebar> _alllist = ExpandList(_list);//展开
                                                      //List<Rebar> _alllist = AscendList(_list);//升序展开


            _totallength = _alllist.Sum(t => t.length);//统计总长度

            List<RebarOri> _returnlist = Algorithm_FFD(_alllist);//FFD首次适应算法
            //List<RebarOri> _returnlist = Algorithm_BFD(_alllist);//BFD最佳适应算法

            return _returnlist;
        }
        //public static List<List<Rebar>> Taoliao(List<RebarData> _list, out int _totallength)
        //{
        //    List<Rebar> _alllist = DescendList(_list);//降序展开
        //    //List<Rebar> _alllist = ExpandList(_list);//展开
        //    //List<Rebar> _alllist = AscendList(_list);//升序展开

        //    _totallength = _alllist.Sum(t => t.length);//统计总长度

        //    List<List<Rebar>> _returnlist = Algorithm_FFD(_alllist);//FFD首次适应算法
        //    //List<List<Rebar>> _returnlist = Algorithm_BFD(_alllist);//BFD最佳适应算法

        //    return _returnlist;
        //}

        /// <summary>
        /// 展开rebardata，并降序排列
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        private static List<Rebar> DescendList(List<RebarData> _list)
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
                    //rebar.TotalPieceNum = 1;
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
        private static List<Rebar> ExpandList(List<RebarData> _list)
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
                    //rebar.TotalPieceNum = 1;
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
        private static List<Rebar> AscendList(List<RebarData> _list)
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
                    //rebar.TotalPieceNum = 1;
                    _alllist.Add(rebar);
                }
            }
            _alllist = _alllist.OrderBy(t => t.length).ToList();//按照长度降序排序

            return _alllist;
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
            RebarOri _temp =new RebarOri();

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
                        if ((ttt._totalLength + item.length) <= GeneralClass.OriginalLength2)//找到长度塞的下的原材
                        {
                            ttt._list.Add(item);//塞进去就break
                            break;
                        }
                        else
                        {
                            if (ttt == _returnlist.Last())//如果是最后一根原材了，还是塞不进去，就新建一根原材
                            {
                                _temp = new RebarOri();
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
            RebarOri _temp = new RebarOri();

            while (_alllist.Count > 0)
            {
                Thread.Sleep(1);

                foreach (var item in _alllist.ToArray())//取一根钢筋过来,注意此处为 _alllist.ToArray()，为一个拷贝版本，因为后面会修改_alllist
                {
                    if (_returnlist.Count == 0)//原材list为空，新增一根原材
                    {
                        _temp = new RebarOri();
                        _temp._list.Add(item);
                        _returnlist.Add(_temp);

                        _alllist.Remove(item);//已经分配的从_alllist移除
                    }
                    else
                    {
                        foreach (var ttt in _returnlist)//遍历所有原材
                        {
                            if ((ttt._totalLength + item.length) <= GeneralClass.OriginalLength2)//找到长度塞的下的原材
                            {
                                ttt._list.Add(item);//塞进去就break
                                _alllist.Remove(item);//已经分配的从_alllist移除
                                break;
                            }
                            else if (item.length > ttt._list.Max(t => t.length)//原材空缺中塞不下，但比当前原材中已排好的钢筋更长，并且替换后不会超出原材长度，就可以替换
                                && (item.length - ttt._list.Max(t => t.length)) < ttt._lengthleft)
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
                                    _temp = new RebarOri();
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
        /// 首次适应算法FFD（first fit） 的改进版
        /// </summary>
        /// <param name="_alllist"></param>
        /// <returns></returns>
        private static List<RebarOri> Algorithm_FFD_1(List<Rebar> _alllist)
        {
            List<RebarOri> _returnlist = new List<RebarOri>();
            RebarOri _temp = new RebarOri();

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
                        if ((ttt._totalLength + item.length) <= GeneralClass.OriginalLength2)//找到长度塞的下的原材
                        {
                            ttt._list.Add(item);//塞进去就break
                            break;
                        }
                        else
                        {
                            if (ttt == _returnlist.Last())//如果是最后一根原材了，还是塞不进去，就新建一根原材
                            {
                                _temp = new RebarOri();
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


            ////最后一根的尾料做处理
            //if (_returnlist.Last()._lengthleft > 3000)//如果最后一根的尾料超过3000的处理，3000以下的先不管
            //{
            //    foreach (var item in _returnlist.Last()._list)
            //    {
            //        for
            //    }
            //}

            return _returnlist;
        }


        //private static bool FindTwoRebarLeft()
        //{

        //}
        ///// <summary>
        ///// 交换两根原材中的小段位置，插入一个小段
        ///// </summary>
        ///// <returns></returns>
        //private static bool ExchangeJoin(Rebar _rebar, ref List<Rebar> _list1, ref List<Rebar> _list2)
        //{

        //}
    }
}
