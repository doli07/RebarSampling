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
        /// 根据梁板线rebarlist，创建以一根原材的工单json数据
        /// </summary>
        /// <param name="_rebarlist">排布在一根原材上的钢筋信息</param>
        /// <returns>json格式数据</returns>
        public string CreateWorkBill_LB(WorkBillMsg _msg, RebarOri _rebarlist)
        {
            string returnstr = "";

            WorkBill_LB _workbill = new WorkBill_LB();

            _workbill.Msgtype = 2;
            string _Date = DateTime.Now.ToString("yyyyMMdd");
            _workbill.BillNo = _Date + "_"
                                + _msg.shift.ToString() + "_"
                                //+ _msg.BatchMsg.totalBatch.ToString() + "_"
                                //+ _msg.BatchMsg.curBatch.ToString();
                                + _rebarlist._list[0].BatchMsg.totalBatch.ToString().PadLeft(3, '0') + "_"
                                + _rebarlist._list[0].BatchMsg.curBatch.ToString().PadLeft(3, '0') + "-"
                                + _rebarlist._list[0].BatchMsg.totalchildBatch.ToString().PadLeft(3, '0') + "_"
                                + (_rebarlist._list[0].BatchMsg.curChildBatch + 1).ToString().PadLeft(3, '0');
            _workbill.SteelbarNo = _Date + "_"
                                + _msg.shift.ToString() + "_"
                                //+ _msg.BatchMsg.totalBatch.ToString() + "_"
                                //+ _msg.BatchMsg.curBatch.ToString() + "_"
                                + _rebarlist._list[0].BatchMsg.totalBatch.ToString().PadLeft(3, '0') + "_"
                                + _rebarlist._list[0].BatchMsg.curBatch.ToString().PadLeft(3, '0') + "-"
                                + _rebarlist._list[0].BatchMsg.totalchildBatch.ToString().PadLeft(3, '0') + "_"
                                + (_rebarlist._list[0].BatchMsg.curChildBatch + 1).ToString().PadLeft(3, '0') + "-"
                                + _msg.totalOriginal.ToString().PadLeft(3, '0') + "_"
                                + (_msg.curOriginal + 1).ToString().PadLeft(3, '0');
            _workbill.ProjectName = _msg.projectName;
            _workbill.Block = _msg.block;
            _workbill.Building = _msg.building;
            _workbill.Floor = _msg.floor;
            _workbill.Level = _msg.level;
            _workbill.Diameter = _rebarlist._list[0].Diameter;
            _workbill.Brand = _msg.brand;
            _workbill.Specification = _msg.specification;
            _workbill.OriginalLength = _msg.originLength;
            _workbill.TaosiSettiing = _msg.taosiSetting;
            //_workbill.SteelbarList = _rebarlist;
            foreach (var item in _rebarlist._list)
            {
                SingleRebarMsg msg = new SingleRebarMsg();
                msg.shift = _msg.shift;
                //msg.BatchMsg.totalBatch = _msg.BatchMsg.totalBatch;
                //msg.BatchMsg.curBatch = _msg.BatchMsg.curBatch;
                msg.BatchMsg = item.BatchMsg;
                msg.totalOriginal = _msg.totalOriginal;
                msg.curOriginal = _msg.curOriginal;
                msg.curSingle = /*item.seriNo;*/ _rebarlist._list.IndexOf(item);
                msg.wareMsg = item.WareMsg;

                var temp = CreateSingleRebarData(msg, item);
                if (GeneralClass.CfgData.Factory == EnumFactory.RouXing && !item.IfBend)//如果是柔性工厂里面的不弯的，则不用生成json工单
                {
                    continue;
                }
                else
                {
                    _workbill.SteelbarList.Add(temp);
                }
            }

            returnstr = NewtonJson.Serializer(_workbill);//json序列化

            return returnstr;
        }

        public string CreateWorkBill_QZ(WorkBillMsg _msg, RebarOri _rebarlist)
        {
            string returnstr = "";

            WorkBill_LB _workbill = new WorkBill_LB();

            _workbill.Msgtype = 2;
            string _Date = DateTime.Now.ToString("yyyyMMdd");
            _workbill.BillNo = _Date + "_"
                                + _msg.shift.ToString() + "_"
                                //+ _msg.BatchMsg.totalBatch.ToString() + "_"
                                //+ _msg.BatchMsg.curBatch.ToString();
                                + _rebarlist._list[0].BatchMsg.totalBatch.ToString().PadLeft(3, '0') + "_"
                                + _rebarlist._list[0].BatchMsg.curBatch.ToString().PadLeft(3, '0') + "-"
                                + _rebarlist._list[0].BatchMsg.totalchildBatch.ToString().PadLeft(3, '0') + "_"
                                + (_rebarlist._list[0].BatchMsg.curChildBatch + 1).ToString().PadLeft(3, '0');
            _workbill.SteelbarNo = _Date + "_"
                                + _msg.shift.ToString() + "_"
                                //+ _msg.BatchMsg.totalBatch.ToString() + "_"
                                //+ _msg.BatchMsg.curBatch.ToString() + "_"
                                + _rebarlist._list[0].BatchMsg.totalBatch.ToString().PadLeft(3, '0') + "_"
                                + _rebarlist._list[0].BatchMsg.curBatch.ToString().PadLeft(3, '0') + "-"
                                + _rebarlist._list[0].BatchMsg.totalchildBatch.ToString().PadLeft(3, '0') + "_"
                                + (_rebarlist._list[0].BatchMsg.curChildBatch + 1).ToString().PadLeft(3, '0') + "-"
                                + _msg.totalOriginal.ToString().PadLeft(3, '0') + "_"
                                + (_msg.curOriginal + 1).ToString().PadLeft(3, '0');
            _workbill.ProjectName = _msg.projectName;
            _workbill.Block = _msg.block;
            _workbill.Building = _msg.building;
            _workbill.Floor = _msg.floor;
            _workbill.Level = _msg.level;
            _workbill.Diameter = _rebarlist._list[0].Diameter;
            _workbill.Brand = _msg.brand;
            _workbill.Specification = _msg.specification;
            _workbill.OriginalLength = _msg.originLength;
            _workbill.TaosiSettiing = _msg.taosiSetting;
            //_workbill.SteelbarList = _rebarlist;
            foreach (var item in _rebarlist._list)
            {
                SingleRebarMsg msg = new SingleRebarMsg();
                msg.shift = _msg.shift;
                //msg.BatchMsg.totalBatch = _msg.BatchMsg.totalBatch;
                //msg.BatchMsg.curBatch = _msg.BatchMsg.curBatch;
                msg.BatchMsg = item.BatchMsg;
                msg.totalOriginal = _msg.totalOriginal;
                msg.curOriginal = _msg.curOriginal;
                msg.curSingle = /*item.seriNo;*/ _rebarlist._list.IndexOf(item);
                msg.wareMsg = item.WareMsg;

                var temp = CreateSingleRebarData(msg, item);
                if (GeneralClass.CfgData.Factory == EnumFactory.RouXing && !item.IfBend)//如果是柔性工厂里面的不弯的，则不用生成json工单
                {
                    continue;
                }
                else
                {
                    _workbill.SteelbarList.Add(temp);
                }
            }

            returnstr = NewtonJson.Serializer(_workbill);//json序列化

            return returnstr;
        }

        public WorkBill_SingleRebar CreateSingleRebarData(SingleRebarMsg _msg, Rebar _data)
        {
            WorkBill_SingleRebar _singleRebar = new WorkBill_SingleRebar();

            string _Date = DateTime.Now.ToString("yyyyMMdd");
            _singleRebar.SeriNo = _Date + "_"
                                + _msg.shift.ToString() + "_"
                                + _msg.BatchMsg.totalBatch.ToString().PadLeft(3, '0') + "_"
                                + _msg.BatchMsg.curBatch.ToString().PadLeft(3, '0') + "-"
                                + _msg.BatchMsg.totalchildBatch.ToString().PadLeft(3, '0') + "_"
                                + (_msg.BatchMsg.curChildBatch + 1).ToString().PadLeft(3, '0') + "-"
                                + _msg.totalOriginal.ToString().PadLeft(3, '0') + "_"
                                + (_msg.curOriginal + 1).ToString().PadLeft(3, '0') + "-"
                                + (_msg.curSingle+1).ToString();//所有序号都从1开始编号
            _singleRebar.ProjectName = _data.ProjectName;
            _singleRebar.AssemblyName = _data.MainAssemblyName;
            _singleRebar.ElementName = _data.ElementName;
            _singleRebar.WareInfo = _msg.wareMsg.channel.ToString() + "_" + GeneralClass.wareNum[(int)_msg.wareMsg.totalware].ToString() + "_" + _msg.wareMsg.wareno.ToString();
            _singleRebar.PicNo = _data.PicTypeNum;
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
            /*转换规则：项目编号   +  钢筋数据库indexcode   +   同一个钢筋rebardata中的流水号
            //          1位字母    +  4位的31进制数         +   1位的31进制数            */
            _singleRebar.IndexCode ="A"+ ConvertCode(_data.IndexNo).ToUpper().PadLeft(4, '0') +
                ConvertCode(_data.seriNo).ToUpper().PadLeft(1, '0');

            return _singleRebar;
        }

        /// <summary>
        /// 将10进制转换成30进制
        /// 1、首字符为项目代号，由大写字母组成。
        ///      大写字母：A ~Z
        ///      排除可能引起混淆的字母B、D、I、J、O、W
        /// 2、后五位字符代表钢筋编号，由数字和大写字母组成。
        ///      数字：0~9；
        ///      大写字母：A ~Z
        ///      排除可能引起混淆的字母B、D、I、J、O、W
        /// 3、注：        B——易与数字8混淆；D——易与数字0混淆；I——易与数字1混淆；J——易与数字1混淆；O——易与数字0混淆；W——字符太宽
        /// 
        /// </summary>
        /// <param name="_value">待转换的十进制数</param>
        /// <param name="_basestr">任意进制的基础字符，其长度即为新的进制数</param>
        /// <returns></returns>
        private string ConvertCode(int _value, string _basestr = "0123456789ACEFGHKLMNPQRSTUVXYZ")
        {
            string sss = "";
            int _jinzhi = _basestr.Length;

            while (_value > 0)
            {
                var temp = _value % _jinzhi;

                sss += _basestr[temp].ToString();

                _value = _value / _jinzhi;
            }

            return sss;
        }
    }
}
