using NPOI.OpenXmlFormats.Dml;
using System;
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
        public static List<RebarOri> Taoliao_pi(List<RebarData> _list)
        {
            List<RebarOri> returnlist = new List<RebarOri>();


            return returnlist;
        }


        public static List<RebarOri> Tao_pi(List<Rebar> _list)
        {
            List<RebarOri> returnlist = new List<RebarOri>();

            RebarOri _temp = new RebarOri();

            //先按照长度分组，并累加数量
            var _grouplist = _list.GroupBy(x => x.length).Select(
                 y => new
                 {
                     _length = y.Key,
                     _totalnum = y.Sum(item => item.TotalPieceNum),
                     _totalweight = y.Sum(item => item.TotalWeight),
                     _datalist = y.ToList()
                 }).ToList().OrderBy(t => t._length).ToList();//升序

            //将长度单独列list
            List<int> _lengthlist = new List<int>();
            foreach(var item in _grouplist)
            {
                _lengthlist.Add(item._length);
            }

            //
            for(int i= _lengthlist.Count-1;i>=0;i--)
            {
                //if (returnlist.Count == 0)//原材list为空，新增一根原材
                //{
                //    _temp = new RebarOri();

                //    _temp._list.Add(_rebarlist[i]);
                //    _returnlist.Add(_temp);
                //    _rebarlist.Remove(_rebarlist[i]);

                //}
                //else
                //{
                //    foreach (var ttt in _returnlist)//遍历所有原材
                //    {
                //        if ((ttt._lengthListUsed + _rebarlist[i].length) <= ttt._totalLength/*GeneralClass.OriginalLength*/)//找到长度塞的下的原材
                //        {
                //            ttt._list.Add(_rebarlist[i]);//塞进去就break
                //            _rebarlist.Remove(_rebarlist[i]);
                //            break;
                //        }
                //        else
                //        {
                //            if (ttt == _returnlist.Last())//如果是最后一根原材了，还是塞不进去，就新建一根原材
                //            {
                //                _temp = new RebarOri(_materialLength);
                //                if (_useOver ? true : (_rebarlist[i].length <= _temp._totalLength)) //新增一根原材，也要判定准备塞的长度是否大于原材长度
                //                {
                //                    _temp._list.Add(_rebarlist[i]);
                //                    _returnlist.Add(_temp);
                //                    _rebarlist.Remove(_rebarlist[i]);//塞进去就break
                //                    break;
                //                }

                //            }
                //            else
                //            {
                //                continue;//塞不进去，找下一根原材
                //            }
                //        }
                //    }
                //}

            }






            return returnlist;
        }
    }
}
