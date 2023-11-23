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
            List<Rebar> _alllist = ListDescend(_list);//降序展开
            //List<Rebar> _alllist = ListExpand(_list);//展开
            //List<Rebar> _alllist = ListAscend(_list);//升序展开


            _totallength = _alllist.Sum(t => t.length);//统计总长度

            //List<RebarOri> _returnlist = Algorithm_FFD(_alllist);//FFD首次适应算法
            List<RebarOri> _returnlist = Algorithm_FFD_1(_alllist);//FFD首次适应算法，改进版

            //List<RebarOri> _returnlist = Algorithm_BFD(_alllist);//BFD最佳适应算法

            return _returnlist;
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
        private static List<Rebar> ListExpand(List<RebarData> _list)
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

            CutTail(ref _returnlist);
            ////最后一根的尾料做处理
            //bool _flag = false;

            //if (_returnlist.Last()._lengthleft > 3000)//如果最后一根的尾料超过3000的处理，3000以下的先不管
            //{
            //    foreach (var item in _returnlist.Last()._list.ToArray())//_returnlist.Last()._list可能会修改，所以用toArray
            //    {
            //        _flag = FindTwoRebarLeftMatch(item, ref _returnlist);
            //        if (_flag)
            //        {
            //            GeneralClass.interactivityData?.printlog(1, "find match success!");
            //            _returnlist.Last()._list.Remove(item);//塞成功了，则从总的list中去掉当前这个小段rebar
            //        }

            //    }
            //    if(_returnlist.Last()._list.Count==0)
            //    {
            //        _returnlist.Remove(_returnlist.Last());//如果最后一个钢筋原材已经空了，则去掉

            //    }
            //}

            return _returnlist;
        }

        private static void CutTail(ref List<RebarOri> _list)
        {
            //最后一根的尾料做处理
            bool _flag = false;

            if (_list.Last()._lengthleft > 2000)//如果最后一根的尾料超过2000的处理，2000以下的先不管
            {
                foreach (var item in _list.Last()._list.ToArray())//_returnlist.Last()._list可能会修改，所以用toArray
                {
                    _flag = FindTwoRebarLeftMatch(item, ref _list);
                    if (_flag)
                    {
                        GeneralClass.interactivityData?.printlog(1, "find match success!");
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
                    if ((_temp1._lengthleft + _temp2._lengthleft) > _rebar.length)
                    {
                        bool _flag = ExchangeJoin(_rebar, ref _temp1, ref _temp2);
                        if (_flag)
                        {
                            GeneralClass.interactivityData?.printlog(1, "exchange success!");
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
            if (_list1.Sum(t => t.length) <= GeneralClass.OriginalLength2 && _list2.Sum(t => t.length) <= GeneralClass.OriginalLength2)
            {
                _ori1._list = _list1;//
                _ori2._list = _list2;
                return true;
            }
            else { return false; }

        }
    }
}
