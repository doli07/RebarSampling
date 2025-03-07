using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RebarSampling.Database
{
    /// <summary>
    /// 广联达料单解析类
    /// </summary>
    public class GLDHelper:ILDHelper
    {


        /// <summary>
        /// 分解多段钢筋，广联达料单专用
        /// 主要是拆解长度信息，例如：1680/9000/9000，广联达料单一般以“/”作为多段长度的分隔符
        /// 注意：
        ///         "7500/8950*2/8950/760"
        ///          "8980/Δ=335\r\n780~1450"       缩尺
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        public List<RebarData> SplitMultiRebar(RebarData _data)
        {
            try
            {
                List<RebarData> _returnlist = new List<RebarData>();


                RebarData temp = new RebarData();

                ////先复制源数据，将其数量、重量归零，存回去
                //temp.Copy(_data);
                //temp.PieceNumUnitNum = "0";
                //temp.TotalPieceNum = 0;
                //temp.TotalWeight = 0;
                //_returnlist.Add(temp);

                string[] slength = _data.Length.Split('/');
                int _total = GetMultiTotalLength(_data.Length);//计算总长度

                for (int i = 0; i < slength.Length; i++)
                {
                    temp = new RebarData();
                    temp.Copy(_data);

                    if (slength[i].Split('*').Length>1)//如果长度信息中带了“*”，表示其中有多根的情况，例如："7500/8950*2/8950/760"
                    {
                        //int _length = Convert.ToInt32(slength[i].Split('*')[0]);
                        int _num = Convert.ToInt32(slength[i].Split('*')[1]);

                        int _length = 0;
                        if (slength[i].Split('*')[0].IndexOf('~')>-1)//判定缩尺，"8980/Δ=335\r\n780~1450"
                        {
                            temp.TotalWeight = _data.TotalWeight * SuoChiDeal(slength[i].Split('*')[0],_data.TotalPieceNum).Sum() / _total;
                        }
                        else
                        {
                            _length = Convert.ToInt32(slength[i].Split('*')[0]);
                            temp.TotalWeight = _data.TotalWeight * _num * _length / _total;
                        }
                        temp.TotalPieceNum= _data.TotalPieceNum*_num;
                        temp.PieceNumUnitNum= _data.TotalPieceNum.ToString()+"*"+_num.ToString();
                        temp.Length= slength[i].Split('*')[0];
                        _returnlist.Add(temp);//其他不变

                    }
                    else
                    {
                        int _length = 0;
                        if (slength[i].IndexOf('~') > -1)//判定缩尺，"8980/Δ=335\r\n780~1450"
                        {
                            temp.TotalWeight = _data.TotalWeight * SuoChiDeal(slength[i], _data.TotalPieceNum).Sum() / _total;
                        }
                        else
                        {
                            _length = Convert.ToInt32(slength[i]);
                            temp.TotalWeight = _data.TotalWeight *  _length / _total;//重量要按照长度均分一下
                        }

                        temp.Length = slength[i];//保留分隔后的长度
                        _returnlist.Add(temp);//其他不变

                    }
                }

                return _returnlist;
            }
            catch (Exception ex) { MessageBox.Show("SplitMultiRebar_Gld error:" + ex.Message); return null; }
        }

        public List<RebarData> SplitSuoChiRebar(RebarData _data)
        {
            List<RebarData> _returnlist=new List<RebarData> ();



            return _returnlist; 
        }

        public List<GeneralMultiData> GetMultiData(string _cornerMsg, int _diameter = 1)
        {
            List<GeneralMultiData> _returnlist=new List<GeneralMultiData> ();

            return _returnlist; 
        }

        public int GetMultiTotalLength(string strlength)
        {
            try
            {
                int _totallength = 0;

                string[] slength = strlength.Split('/');

                for(int i=0;i<slength.Length;i++)
                {
                    if (slength[i].Split('*').Length > 1)//如果长度信息中带了“*”，表示其中有多根的情况，例如："7500/8950*2/8950/760"
                    {                        
                        int _length = 0;
                        string temp = slength[i].Split('*')[0];
                        if (temp.IndexOf('~')>-1)//判定缩尺
                        {
                            _length = SuoChiDealAva(temp);
                        }
                        else
                        {
                            _length = Convert.ToInt32(temp);
                        }
                        int _num = Convert.ToInt32(slength[i].Split('*')[1]);
                        _totallength += _length*_num;
                    }
                    else
                    {
                        int _length = 0;
                        string temp =slength[i];    
                        if(temp.IndexOf('~')>-1)//判定缩尺
                        {
                            _length = SuoChiDealAva(temp);
                        }
                        else
                        {
                            _length = Convert.ToInt32(slength[i]);
                        }
                        _totallength += _length;
                    }

                }

                //List<GeneralMultiLength> _multilength = GetMultiLength(strlength);

                //for (int i = 0; i < _multilength.Count; i++)
                //{
                //    if (_multilength[i].length.IndexOf('~') > -1)//如果含有~，为缩尺
                //    {
                //        string[] ttt = _multilength[i].length.Split('~');
                //        _max = Convert.ToInt32(ttt[0]);
                //        _min = Convert.ToInt32(ttt[1]);

                //        _totallength += (int)(_max + _min) / 2 * _multilength[i].num;
                //    }
                //    else
                //    {
                //        _totallength += Convert.ToInt32(_multilength[i].length) * _multilength[i].num;
                //    }

                //}
                return _totallength;
            }
            catch (Exception ex) { MessageBox.Show("GetMultiTotalLength error:" + ex.Message); return 0; }

        }

        /// <summary>
        /// 取缩尺的平均值，例如："8980/Δ=335\r\n780~1450"
        /// </summary>
        /// <param name="_suochi"></param>
        /// <returns></returns>
        private int SuoChiDealAva(string _suochi)
        {
            int _length = 0;

            string[] lengthlist = _suochi.Split('\n')[1].Split('~');//先截取“Δ=335\r\n780~1450”的后半段，再截取长度，
            int _startlength = Convert.ToInt32(lengthlist[0]);//缩尺的起始长度，不一定最大，有可能是最小
            int _endlength = Convert.ToInt32(lengthlist[1]);//缩尺的终止长度，不一定最小，有可能是最大

            _length = (_startlength + _endlength) / 2;//取平均值

            return _length;
        }
        /// <summary>
        /// 处理缩尺信息，例如："8980/Δ=335\r\n780~1450"
        /// </summary>
        /// <param name="_suochi">缩尺长度信息，例如：1420~870</param>
        /// <param name="_num">总共多少根缩尺筋，间距数量-1</param>
        /// <returns>返回处理好的长度数值的list</returns>
        public List<int> SuoChiDeal(string _suochi, int _num)
        {
            try
            {
                List<int> _retlist = new List<int>();
                
                string[] _length = _suochi.Split('\n')[1].Split('~');//先截取“Δ=335\r\n780~1450”的后半段，再截取长度，
                int _startlength = Convert.ToInt32(_length[0]);//缩尺的起始长度，不一定最大，有可能是最小
                int _endlength = Convert.ToInt32(_length[1]);//缩尺的终止长度，不一定最小，有可能是最大

                int _maxlength = Math.Max(_startlength, _endlength);
                int _minlength = Math.Min(_startlength, _endlength);

                for (int i = 0; i < _num; i++)//从最短的开始
                {
                    if (_num == 1)
                    {
                        GeneralClass.interactivityData?.printlog(1, "缩尺信息:" + _suochi + "，数量:" + _num + "，数据不正确，请检查！");
                    }
                    int temp = _minlength + i * (_maxlength - _minlength) / ((_num - 1) != 0 ? (_num - 1) : 1);
                    _retlist.Add(temp);
                }

                return _retlist;
            }
            catch (Exception ex) { MessageBox.Show("SuoChiDeal error:" + ex.Message); return null; }
        }

    }
}
