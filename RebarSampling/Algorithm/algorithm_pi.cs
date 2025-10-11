using NPOI.OpenXmlFormats.Dml;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebarSampling
{
    /******
     * 本类为algorithm的分支，处理批量锯切套料功能
     *  
     * 
     * ******/
    public partial class Algorithm
    {
        /// <summary>
        /// 批量锯切二维套料
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static List<RebarOri> Taoliao_pi(List<RebarData> _list)
        {
            List<RebarOri> returnlist = new List<RebarOri>();

            List<Rebar> _rebarlist = ListExpand(_list);

            //先排标准原材的整数模数的
            for (int i = 1; i < 16; i++)
            {
                returnlist.AddRange(Tao_CutStdOri(ref _rebarlist, i, 100));//设定阈值100，遍历所有均匀切的段数
            }

            //再排批量自由组合套料的
            int _count = 0;
            while (_rebarlist != null && _rebarlist.Count != 0 && _count <= 3)
            {
                returnlist.AddRange(Tao_pi(ref _rebarlist, 3000));//先定3000的阈值
                _count++;
            }

            //最后排自我套料的
            if (_rebarlist.Count != 0)
            {
                returnlist.AddRange(Tao_single(ref _rebarlist));
            }
            return returnlist;
        }
        /// <summary>
        /// 自由组合套料剩余的，按照每种规格，自己跟自己组合套料
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        private static List<RebarOri> Tao_single(ref List<Rebar> _list)
        {
            List<RebarOri> returnlist = new List<RebarOri>();
            RebarOri temp = new RebarOri(_list.First().Level, _list.First().Diameter);

            //先按照长度分组，并累加数量
            List<RebarPi> _grouplist = _list.GroupBy(x => x.length).Select(
                 y => new RebarPi
                 {
                     //length = y.Key,
                     _rebarList = y.ToList()
                 }).ToList().OrderBy(t => t.length).ToList();//升序

            foreach (var item in _grouplist)
            {
                temp = new RebarOri(_list.First().Level, _list.First().Diameter);
                for (int i = 0; i < item.num; i++)
                {
                    if (temp._lengthFirstLeft < item.length)//塞不进去了，新建一个rebarOri
                    {
                        returnlist.Add(temp);
                        temp = new RebarOri(_list.First().Level, _list.First().Diameter);
                        temp._list.Add(item._rebarList[i]);
                    }
                    else
                    {
                        temp._list.Add(item._rebarList[i]);
                        //if (i == item.num - 1)//最后一根，存入returnlist
                        //{
                        //    returnlist.Add(temp);
                        //}
                    }
                }
                    returnlist.Add(temp);//最后一根，存入returnlist

            }

            return returnlist;

        }
        /// <summary>
        /// 按照标准原材均匀切多少段的模数来套料，比如12000切成6段，每段不超过2000，需要卡阈值
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="_cutNum">将标准原材均匀切的段数，进行自我套料</param>
        /// <param name="_threshold">卡阈值</param>
        /// <returns></returns>
        private static List<RebarOri> Tao_CutStdOri(ref List<Rebar> _list, int _cutNum, int _threshold)
        {
            List<RebarOri> returnlist = new List<RebarOri>();
            RebarOri temp = new RebarOri(_list.First().Level, _list.First().Diameter);

            //先按照长度分组，并累加数量
            List<RebarPi> _grouplist = _list.GroupBy(x => x.length).Select(
                 y => new RebarPi
                 {
                     //length = y.Key,
                     _rebarList = y.ToList()
                 }).ToList().OrderBy(t => t.length).ToList();//升序

            double _pieceLength = GeneralClass.OriginalLength(_list.First().Level, _list.First().Diameter) / _cutNum;//每一段的长度

            foreach (var item in _grouplist)
            {
                if ((_pieceLength - item.length) > 0 && (_pieceLength - item.length) < _threshold)//找到与小段长度接近（小于阈值）的
                {
                    while (item._rebarList.Count > 0)
                    {
                        temp = new RebarOri(_list.First().Level, _list.First().Diameter);
                        temp._list.AddRange((item._rebarList.Count >= _cutNum) ? item._rebarList.Take(_cutNum) : item._rebarList);//存入新建的rebarOri

                        if (item._rebarList.Count >= _cutNum)//移除
                            item._rebarList.RemoveRange(0, _cutNum);
                        else
                            item._rebarList.Clear();
                    }
                    _list.RemoveAll(t => t.length == item.length);//从原始数据中删除用掉的元素
                }
            }


            return returnlist;
        }
        /// <summary>
        /// 批量锯切二维套料
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        private static List<RebarOri> Tao_pi(ref List<Rebar> _list, int _threshold)
        {
            List<RebarOri> returnlist = new List<RebarOri>();

            //先按照长度分组，并累加数量
            List<RebarPi> _grouplist = _list.GroupBy(x => x.length).Select(
                 y => new RebarPi
                 {
                     //length = y.Key,
                     _rebarList = y.ToList()
                 }).ToList().OrderBy(t => t.length).ToList();//升序

            //第一步，只考虑长度维度，按照长度套料
            List<RebarPiOri> _templist = new List<RebarPiOri>();//虚拟的rebarOri链表
            RebarPiOri _temp = new RebarPiOri(_list.First().Level,_list.First().Diameter);//20241121修改，根据级别直径创建rebarpiori
            for (int i = _grouplist.Count - 1; i >= 0; i--)
            {
                if (_templist.Count == 0)//原材list为空，新增一根原材
                {
                    _temp = new RebarPiOri(_list.First().Level,_list.First().Diameter);

                    _temp._list.Add(_grouplist[i]);//新建一个指定长度的rebar
                    _templist.Add(_temp);
                    //_grouplist.Remove(_grouplist[i]);
                }
                else
                {
                    foreach (var ttt in _templist)//遍历所有原材
                    {
                        if ((ttt._lengthListUsed + _grouplist[i].length) <= ttt._totalLength)//找到长度塞的下的原材
                        {
                            ttt._list.Add(_grouplist[i]);//塞进去就break
                            //_grouplist.Remove(_grouplist[i]);
                            break;
                        }
                        else
                        {
                            if (ttt == _templist.Last())//如果是最后一根原材了，还是塞不进去，就新建一根原材
                            {
                                _temp = new RebarPiOri(_list.First().Level, _list.First().Diameter);
                                _temp._list.Add(_grouplist[i]);//新建一个指定长度的rebar
                                _templist.Add(_temp);
                                //_grouplist.Remove(_grouplist[i]);
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

            //第二步，按照数量维度数量最少的rebarPi进行分离，其他rebarPi多余的部分，存回list，继续下次批量套料
            _list.Clear();
            foreach (var item in _templist)
            {
                returnlist.AddRange(ApartRebarPi(item, ref _list));//将多余的部分重新赋值给list
            }

            //第三步，判定returnlist中的余料长度是否超过阈值，如果超过，则回笼
            for (int i = returnlist.Count - 1; i > 0; i--)
            {
                if (returnlist[i]._lengthFirstLeft > _threshold)
                {
                    _list.AddRange(returnlist[i]._list);//回笼
                    returnlist.Remove(returnlist[i]);
                }
            }

            return returnlist;
        }
        /// <summary>
        /// 将RebarPiOri分离成rebarOri和剩余的rebar
        /// </summary>
        /// <param name="_rebarPiOri"></param>
        /// <param name="_leftRebar"></param>
        /// <returns></returns>
        private static List<RebarOri> ApartRebarPi(RebarPiOri _rebarPiOri, ref List<Rebar> _leftRebar)
        {
            string _level=_rebarPiOri._list.First()._rebarList.First().Level;
            int _diameter= _rebarPiOri._list.First()._rebarList.First().Diameter;//获取钢筋级别直径

            List<RebarOri> _returnlist = new List<RebarOri>();

            int cutNum = _rebarPiOri._list.Min(t => t.num);
            for (int i = 0; i < cutNum; i++)
            {
                RebarOri _newOri = new RebarOri(_level, _diameter);
                foreach (var ttt in _rebarPiOri._list)
                {
                    _newOri._list.Add(ttt._rebarList.Last());//纳入新的rebarOri
                    ttt._rebarList.Remove(ttt._rebarList.Last());//删除
                }
                _returnlist.Add(_newOri);
            }

            //回收利用
            foreach (var item in _rebarPiOri._list)
            {
                if (item._rebarList != null && item._rebarList.Count != 0)//有可能删完了
                {
                    _leftRebar.AddRange(item._rebarList);
                }
            }

            return _returnlist;

        }
    }
}
