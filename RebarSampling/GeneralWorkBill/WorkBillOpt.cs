using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebarSampling.GeneralWorkBill
{
    public class WorkBillOpt
    {
        /// <summary>
        /// 根据rebarlist，创建以一根原材的工单json数据
        /// </summary>
        /// <param name="_rebarlist">排布在一根原材上的钢筋信息</param>
        /// <returns>json格式数据</returns>
        public string CreateWorkBill(WorkBillMsg _msg, List<Rebar> _rebarlist)
        {
            //int shift = 1;//班次
            //int totalBatch = 100;//总批次
            //int curBatch = 15;//当前批次
            //int totalOriginal = 100;//总的原材根数
            //int curOriginal = 40;//当前的原材流水号
            //string projectName = "光谷国际社区";//项目名称
            //string block = "A";//区域
            //string building = "06D";//楼栋
            //string floor = "01F";//楼层
            //string level = "C";//钢筋级别
            //string brand = "鄂钢";//厂商
            //string specification = "HRB400";//规格型号
            //int originLength = 12000;

            string returnstr = "";

            WorkBill _workbill = new WorkBill();

            _workbill.Msgtype = 2;
            string _Date = DateTime.Now.ToString("yyyyMMdd");
            _workbill.BillNo = _Date + "_"
                                + _msg.shift.ToString() + "_"
                                //+ _msg.BatchMsg.totalBatch.ToString() + "_"
                                //+ _msg.BatchMsg.curBatch.ToString();
                                + _rebarlist[0].BatchMsg.totalBatch.ToString().PadLeft(3,'0') + "_"
                                + _rebarlist[0].BatchMsg.curBatch.ToString().PadLeft(3,'0')+"-"
                                + _rebarlist[0].BatchMsg.totalchildBatch.ToString().PadLeft(3, '0') + "_"
                                + (_rebarlist[0].BatchMsg.curChildBatch+1).ToString().PadLeft(3, '0');
            _workbill.SteelbarNo = _Date + "_"
                                + _msg.shift.ToString() + "_"
                                //+ _msg.BatchMsg.totalBatch.ToString() + "_"
                                //+ _msg.BatchMsg.curBatch.ToString() + "_"
                                + _rebarlist[0].BatchMsg.totalBatch.ToString().PadLeft(3,'0') + "_"
                                + _rebarlist[0].BatchMsg.curBatch.ToString().PadLeft(3,'0') + "-"
                                + _rebarlist[0].BatchMsg.totalchildBatch.ToString().PadLeft(3, '0') + "_"
                                + (_rebarlist[0].BatchMsg.curChildBatch+1).ToString().PadLeft(3, '0') + "-"
                                + _msg.totalOriginal.ToString().PadLeft(3,'0') + "_"
                                +( _msg.curOriginal+1).ToString().PadLeft(3,'0');
            _workbill.ProjectName = _msg.projectName;
            _workbill.Block = _msg.block;
            _workbill.Building = _msg.building;
            _workbill.Floor = _msg.floor;
            _workbill.Level = _msg.level;
            _workbill.Diameter = _rebarlist[0].Diameter;
            _workbill.Brand = _msg.brand;
            _workbill.Specification = _msg.specification;
            _workbill.OriginalLength = _msg.originLength;
            //_workbill.SteelbarList = _rebarlist;
            foreach (var item in _rebarlist)
            {
                SingleRebarMsg msg = new SingleRebarMsg();
                msg.shift = _msg.shift;
                //msg.BatchMsg.totalBatch = _msg.BatchMsg.totalBatch;
                //msg.BatchMsg.curBatch = _msg.BatchMsg.curBatch;
                msg.BatchMsg = item.BatchMsg;
                msg.totalOriginal = _msg.totalOriginal;
                msg.curOriginal = _msg.curOriginal;
                msg.curSingle = /*item.seriNo;*/ _rebarlist.IndexOf(item);
                msg.wareMsg = item.WareMsg;

                _workbill.SteelbarList.Add(CreateSingleRebarData(msg, item));
            }

            returnstr = GeneralClass.JsonOpt.Serializer(_workbill);//json序列化

            return returnstr;
        }


        public SingleRebarData CreateSingleRebarData(SingleRebarMsg _msg, Rebar _data)
        {
            //int shift = 1;//班次
            //int totalBatch = 100;//总批次
            //int curBatch = 15;//当前批次
            //int totalOriginal = 100;//总的原材根数
            //int curOriginal = 40;//当前的原材流水号
            //int curSingle = 1;//当前的小段的编号
            //int channel = 2;//成品仓通道编号
            //int totalware = 8;//仓位总数
            //int wareno = 3;//仓位编号

            SingleRebarData _singleRebar = new SingleRebarData();

            string _Date = DateTime.Now.ToString("yyyyMMdd");
            _singleRebar.SeriNo = _Date + "_"
                                + _msg.shift.ToString() + "_"
                                + _msg.BatchMsg.totalBatch.ToString().PadLeft(3, '0') + "_"
                                + _msg.BatchMsg.curBatch.ToString().PadLeft(3, '0') + "-"
                                + _msg.BatchMsg.totalchildBatch.ToString().PadLeft(3, '0') + "_"
                                + (_msg.BatchMsg.curChildBatch+1).ToString().PadLeft(3, '0') + "-"
                                + _msg.totalOriginal.ToString().PadLeft(3, '0') + "_"
                                + (_msg.curOriginal+1).ToString().PadLeft(3, '0') + "-"
                                + _msg.curSingle.ToString();
            _singleRebar.ProjectName = _data.ProjectName;
            _singleRebar.AssemblyName = _data.MainAssemblyName;
            _singleRebar.ElementName = _data.ElementName;
            _singleRebar.WareInfo = _msg.wareMsg.channel.ToString() + "_" +GeneralClass.wareNum[(int)_msg.wareMsg.totalware] .ToString() + "_" + _msg.wareMsg.wareno.ToString();
            _singleRebar.PicNo = _data.TypeNum;
            _singleRebar.Level = _data.Level;
            _singleRebar.Diameter = _data.Diameter;
            if (_data.Length.Contains('~'))
            {
                string[] str = _data.Length.Split('~');
                _singleRebar.Length = (int.Parse(str[0]) + int.Parse(str[1])) / 2;//缩尺暂时取平均值
            }
            else
            {
                _singleRebar.Length = Convert.ToInt32(_data.Length);
            }

            //根据经验公式计算重量(kg)，保留3位小数
            _singleRebar.Weight = Math.Round(0.00617 * (double)_singleRebar.Diameter * (double)_singleRebar.Diameter * (double)_singleRebar.Length / 1000, 3);
            _singleRebar.CornerMsg = _data.CornerMessage;
            _singleRebar.IndexCode = Convert.ToString(_data.IndexNo, 16).ToUpper().PadLeft(5, '0');//转换成5位的16进制数

            return _singleRebar;
        }
    }
}
