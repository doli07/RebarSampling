using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static List<List<Rebar>> Taoliao(List<RebarData> _list,out int _totallength)
        {
            List<Rebar> _alllist = new List<Rebar>();
            Rebar rebar = new Rebar();
            List<List<Rebar>> _returnlist = new List<List<Rebar>>();

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

            _totallength = _alllist.Sum(t => t.length);//统计总长度

            _alllist = _alllist.OrderByDescending(t => t.length).ToList();//按照长度降序排序

            List<Rebar> _temp = new List<Rebar>();
            foreach (var item in _alllist)//取一根钢筋过来
            {
                if (_returnlist.Count==0)//原材list为空，新增一根原材
                {
                    _temp = new List<Rebar> { item};
                    _returnlist.Add(_temp); 
                }
                else
                {
                    foreach (var ttt in _returnlist)//遍历所有原材
                    {
                        if ((ttt.Sum(t => t.length) + item.length) <= GeneralClass.OriginalLength2)//找到长度塞的下的原材
                        {
                            ttt.Add(item);//塞进去就break
                            break;
                        }
                        else
                        {
                            if(ttt==_returnlist.Last())//如果是最后一根原材了，还是塞不进去，就新建一根原材
                            {
                                _temp = new List<Rebar> { item };
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
    }
}
