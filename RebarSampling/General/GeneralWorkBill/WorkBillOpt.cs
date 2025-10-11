using BarTender;
using NPOI.HSSF.Record;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RebarSampling.GeneralWorkBill
{
    public class WorkBillOpt
    {
        /// <summary>
        /// 根据梁板线_rebarOri，创建以一根原材的工单json数据
        /// </summary>
        /// <param name="_rebarOri">排布在一根原材上的钢筋信息</param>
        /// <returns>json格式数据</returns>
        public string CreateWorkBill_LB_RebarOri(WorkBillMsg _msg, RebarOri _rebarOri)
        {
            string returnstr = "";

            WorkBill_LB _workbill = new WorkBill_LB();

            _workbill.Msgtype = GeneralClass.CfgData.Factory == EnumFactory.GaoXiao ? 2 : 8;//高效工厂返回2，柔性工厂返回8
            string _Date = DateTime.Now.ToString("yyyyMMdd");
            _workbill.BillNo = _Date + "_"
                                + _msg.shift.ToString() + "_"
                                //+ _msg.BatchMsg.totalBatch.ToString() + "_"
                                //+ _msg.BatchMsg.curBatch.ToString();
                                + _rebarOri._list[0]._batchMsg.totalBatch.ToString().PadLeft(3, '0') + "/"
                                + _rebarOri._list[0]._batchMsg.curBatch.ToString().PadLeft(3, '0') + "_"
                                + _rebarOri._list[0]._batchMsg.totalchildBatch.ToString().PadLeft(3, '0') + "/"
                                + (_rebarOri._list[0]._batchMsg.curChildBatch + 1).ToString().PadLeft(3, '0');
            _workbill.SteelbarNo = _Date + "_"
                                + _msg.shift.ToString() + "_"
                                //+ _msg.BatchMsg.totalBatch.ToString() + "_"
                                //+ _msg.BatchMsg.curBatch.ToString() + "_"
                                + _rebarOri._list[0]._batchMsg.totalBatch.ToString().PadLeft(3, '0') + "/"
                                + _rebarOri._list[0]._batchMsg.curBatch.ToString().PadLeft(3, '0') + "_"
                                + _rebarOri._list[0]._batchMsg.totalchildBatch.ToString().PadLeft(3, '0') + "/"
                                + (_rebarOri._list[0]._batchMsg.curChildBatch + 1).ToString().PadLeft(3, '0') + "_"
                                + _msg.totalOriginal.ToString().PadLeft(3, '0') + "/"
                                + (_msg.curOriginal + 1).ToString().PadLeft(3, '0');
            _workbill.TasklistNo = _msg.tasklistNo;//料单编号
            _workbill.TasklistName= _msg.tasklistName;//料单名称
            _workbill.ProjectName = _rebarOri._list[0].ProjectName/* _msg.projectName*/;
            _workbill.Block = _msg.block;
            _workbill.Building = _msg.building;
            _workbill.Floor = _msg.floor;
            _workbill.Level = _rebarOri._list[0].Level/* _msg.level*/;
            _workbill.Diameter = _rebarOri._list[0].Diameter;
            _workbill.Brand = _msg.brand;
            _workbill.Specification = _msg.specification;
            _workbill.OriginalLength = _msg.originLength;
            _workbill.TaosiSetting = _msg.taosiSetting;
            //_workbill.SteelbarList = _rebarlist;
            foreach (var item in _rebarOri._list)
            {
                SingleRebarMsg msg = new SingleRebarMsg();
                msg.shift = _msg.shift;
                //msg.BatchMsg.totalBatch = _msg.BatchMsg.totalBatch;
                //msg.BatchMsg.curBatch = _msg.BatchMsg.curBatch;
                msg.BatchMsg = item._batchMsg;
                msg.totalOriginal = _msg.totalOriginal;
                msg.curOriginal = _msg.curOriginal;
                msg.curSingle = /*item.seriNo;*/ _rebarOri._list.IndexOf(item);
                msg.wareMsg = item._wareMsg;

                var temp = CreateWorkBill_LB_Rebar(msg, item);
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

        public List<string> CreateWorkBill_bend_LB(List<RebarData> _datalist)
        {
            List<string> returnstr = new List<string>();

            WorkBill_Bend_LB _workbill = new WorkBill_Bend_LB();

            int _size = 0;

            foreach (RebarData item in _datalist)
            {
                ConvertCode(item.IndexNo, 3,out _size);//这里返回值没用，主要是要先获取进制数
                int tempsize = Math.Min(item.TotalPieceNum, _size);//取同规格钢筋数量与进制数的最小值作为需要编码的size，

                for (int i = 0; i < tempsize; i++)
                {
                    _workbill = new WorkBill_Bend_LB();
                    _workbill.MsgType = 8;
                    _workbill.SteelbarNo = item.IndexNo.ToString();
                    _workbill.ElementNo = "";//构件编号暂时为空
                    _workbill.Diameter = item.Diameter;
                    _workbill.Length = item.iLength;
                    _workbill.Weight = item.TotalWeight;

                    _workbill.IndexCode = "A" + ConvertCode(item.IndexNo,3, out _size).ToUpper().PadLeft(3, '0') +
                    ConvertCode(i, 1,out _size).ToUpper().PadLeft(1, '0');//rebardata的数据库序列号

                    _workbill.PicNo = item.PicTypeNum;
                    _workbill.CornerMsg = item.CornerMessage;
                    _workbill.Num = item.TotalPieceNum;

                    returnstr.Add(NewtonJson.Serializer(_workbill));

                }
            }

            PrintIndexCodeAll(_datalist);//

            return returnstr;
        }
        /// <summary>
        /// 打印所有的indexcode到文本中
        /// </summary>
        /// <param name="_datalist"></param>
        private void PrintIndexCodeAll(List<RebarData> _datalist)
        {

            string filePath =System.Windows.Forms. Application.StartupPath + @"\logfile\indexcode.txt";

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
            }
            if (File.Exists(filePath))
            {
                int _size = 0;

                FileStream file;
                file = new FileStream(filePath, FileMode.Append);
                StreamWriter writeFile = new StreamWriter(file);

                foreach (RebarData item in _datalist)
                {
                    ConvertCode(item.IndexNo, 3, out _size);//这里返回值没用，主要是要先获取进制数
                    int tempsize = Math.Min(item.TotalPieceNum, _size);//取同规格钢筋数量与进制数的最小值作为需要编码的size，

                    for (int i = 0; i < tempsize; i++)
                    {
                        //writeFile.WriteLine("A" + ConvertCode(item.IndexNo, out _size).ToUpper().PadLeft(4, '0') +
                        //ConvertCode(i, out _size).ToUpper().PadLeft(1, '0'));//rebardata的数据库序列号
                        writeFile.Write("A" + ConvertCode(item.IndexNo, 3, out _size).ToUpper().PadLeft(3, '0') +
                        ConvertCode(i, 1,out _size).ToUpper().PadLeft(1, '0') + " ");//rebardata的数据库序列号
                    }
                }

                writeFile.Flush();
                writeFile.Close();
                file.Close();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("文件路径:" + filePath + "不存在!");
            }
        }
        //public string CreateWorkBill_PiCut(WorkBillMsg _msg, RebarOriPiAllDiameter _alldiaWorkdata)
        //{
        //    string returnstr = "";

        //    WorkBill_QZ _workbill = new WorkBill_QZ();

        //    _workbill.Msgtype = 6;
        //    string _Date = DateTime.Now.ToString("yyyyMMdd");
        //    _workbill.BillNo = "B_" + _Date + "_"
        //                        + _msg.shift.ToString() + "_"
        //                        + "001" + "/"
        //                        + "001";

        //    _workbill.ProjectName = _msg.projectName;
        //    _workbill.Block = _msg.block;
        //    _workbill.Building = _msg.building;
        //    _workbill.Floor = _msg.floor;
        //    _workbill.Level = _msg.level;
        //    _workbill.Brand = _msg.brand;
        //    _workbill.Specification = _msg.specification;
        //    _workbill.OriginalLength = _msg.originLength;
        //    _workbill.CuttingUse = 1;//暂定墙柱线

        //    foreach (var _ori in _alldiaWorkdata._list)
        //    {
        //        int _totalbatchNo = _alldiaWorkdata._list.Count;                  //根据不同直径分的总批次数
        //        int _curbatchNo = _alldiaWorkdata._list.IndexOf(_ori);        //当前批次
        //        WorkBill_QZ_PiCutDiameter temp = CreateWorkBill_QZ_PiCutDiameter(new Tuple<int, int, int>(_msg.shift, _totalbatchNo, _curbatchNo), _ori);

        //        _workbill.CuttingList.Add(temp);
        //    }

        //    returnstr = NewtonJson.Serializer(_workbill);//json序列化

        //    return returnstr;
        //}


        public void ParseWorkBill_QZ(WorkBill_QZ _bill,ref List<RebarOri> _data)
        {
            _data = new List<RebarOri>();
            RebarOri _ori;

            string _level;
            int _diameter;
            int _oriLength;

            foreach (var iiii in _bill.CuttingList)
            {
                _level =iiii.Level;
                _diameter = iiii.Diameter;
                _oriLength=iiii.OriginalLength;
                foreach(var tttt in iiii.SolutionList)
                {
                    _ori = new RebarOri(_oriLength, _level, _diameter);
                    int _num = tttt.OriginalNum;

                    foreach(var eeee in tttt.RebarList)
                    {
                        Rebar _rebar = new Rebar();
                        _rebar.Level = _level;
                        _rebar.Diameter = _diameter;
                        _rebar.CornerMessage = eeee.CornerMsg;
                        _rebar.length = eeee.Length;
                        _rebar.SerialNum=eeee.UniqueCode;//唯一识别码

                        _ori._list.Add(_rebar); 
                    }

                    for(int i=0;i<_num;i++)
                    {
                        _data.Add(_ori);
                    }
                }

            }


        }


        /// <summary>
        /// 墙柱线的工单，20240619
        /// </summary>
        /// <param name="msgtype">4：批量剪切，6：批量锯切</param>
        /// <param name="_msg"></param>
        /// <param name="_rebarlist"></param>
        /// <returns></returns>
        public string CreateWorkBill_QZ(WorkBillMsg _msg, RebarOriPiAllDiameter _alldiaWorkdata,out WorkBill_QZ _workbill)
        {
            string returnstr = "";

             _workbill = new WorkBill_QZ();

            //_workbill.Msgtype = msgtype;
            string _Date = DateTime.Now.ToString("yyyyMMdd");
            _workbill.BillNo = "B_" + _Date + "_"
                                + _msg.shift.ToString() + "_"
                                + "001" + "/"
                                + "001";

            _workbill.TasklistNo = _alldiaWorkdata._list[0]._rebarOriPiList[0]._list[0]._list[0].TableNo;//料表编号
            //_workbill.TasklistName = _alldiaWorkdata._list[0]._rebarOriPiList[0]._list[0]._list[0].TableName;//料表名称
            _workbill.TasklistName = _msg.tasklistName;
            _workbill.ProjectName = _msg.projectName;
            _workbill.Block = _msg.block;
            _workbill.Building = _msg.building;
            _workbill.Floor = _msg.floor;
            _workbill.Level = _msg.level;
            _workbill.Brand = _msg.brand;
            _workbill.Specification = _msg.specification;
            //_workbill.OriginalLength = _msg.originLength;
            _workbill.OriginalLength = _alldiaWorkdata.OriginalLength;

            _workbill.OriginalNum = _alldiaWorkdata.OriginalNum;
            _workbill.OriginalWeight = _alldiaWorkdata.OriginalWeight;
            _workbill.SteelbarNum = _alldiaWorkdata.SteelbarNum;
            _workbill.SteelbarWeight = _alldiaWorkdata.SteelbarWeight;
            _workbill.CuttingUse = 1;//暂定墙柱线

            _alldiaWorkdata.totalBatchNo = 1;//此处暂时给1，20240812
            _alldiaWorkdata.curBatchNo = 1;

            bool _iftao = false;
            foreach (var _ori in _alldiaWorkdata._list)
            {
                int _totalbatchNo = _alldiaWorkdata._list.Count;                  //根据不同直径分的总批次数
                int _curbatchNo = _alldiaWorkdata._list.IndexOf(_ori);        //当前批次
                //_ori.totalBatchNo = _totalbatchNo;//修改总批次参数
                //_ori.curBatchNo = _curbatchNo;//修改当前批次参数

                bool _iftao2 = false;
                WorkBill_QZ_PiCutDiameter temp = CreateWorkBill_QZ_PiCutDiameter(new Tuple<int, int, int>(_msg.shift, _totalbatchNo, _curbatchNo), _ori,out _iftao2);

                if (_iftao2) { _iftao = true; }
                _workbill.CuttingList.Add(temp);
            }
            _workbill.Msgtype = _iftao ? 6 : 4;//如果需要套丝，则为锯切，不套丝则为剪切



            returnstr = NewtonJson.Serializer(_workbill);//json序列化

            return returnstr;
        }
        /// <summary>
        /// 墙柱线工单，第二层级，_msg需要传入批次号等信息
        /// </summary>
        /// <param name="_msg">班次、子加工批总批次、子加工批当前批号</param>
        /// <param name="_diaWorkdata"></param>
        /// <returns></returns>
        public WorkBill_QZ_PiCutDiameter CreateWorkBill_QZ_PiCutDiameter(Tuple<int, int, int> _msg, RebarOriPiWithDiameter _diaWorkdata,out bool _iftao)
        {
            WorkBill_QZ_PiCutDiameter _workbill = new WorkBill_QZ_PiCutDiameter();

            string _Date = DateTime.Now.ToString("yyyyMMdd");
            _workbill.SeriNo = "B_" + _Date + "_"
                                + _msg.Item1.ToString() + "_"
                                + "001" + "/"
                                + "001" + "_"
                                + _msg.Item2.ToString().PadLeft(3, '0') + "/"
                                + _msg.Item3.ToString().PadLeft(3, '0');

            _workbill.Diameter = _diaWorkdata._diameter;
            _workbill.Level = _diaWorkdata._level;
            //_workbill.OriginalLength = GeneralClass.OriginalLength(_diaWorkdata._level, _diaWorkdata._diameter);
            _workbill.OriginalLength = _diaWorkdata.OrignalLength;

            _workbill.Num = _diaWorkdata._rebarOriPiList.Sum(t => t.num);
            _workbill.OriginalNum=_diaWorkdata.OriginalNum;
            _workbill.SteelbarNum = _diaWorkdata.SteelbarNum;
            _workbill.OriginalWeight = _diaWorkdata.OriginalWeight;
            _workbill.SteelbarWeight=_diaWorkdata.SteelbarWeight;   

            int _dia = _diaWorkdata._diameter;
            _workbill.TaosiSetting = _dia.ToString() + "_"
                + _dia.ToString() + "_"
                + _dia.ToString() + "_"
                + _dia.ToString() + "_"
                + _dia.ToString() + "_"
                + _dia.ToString();                  //套丝设置暂时定为所有套丝机一致

            _diaWorkdata.totalBatchNo = _msg.Item2;//设置总批次
            _diaWorkdata.curBatchNo = _msg.Item3;//设置当前批次

            _iftao = false;
            foreach (var item in _diaWorkdata._rebarOriPiList)
            {
                int _totalbatchNo = _diaWorkdata._rebarOriPiList.Count;                  //不同锯切方案的数量统计
                int _curbatchNo = _diaWorkdata._rebarOriPiList.IndexOf(item);        //当前锯切方案的序号

                bool _iftao2=false;
                WorkBill_QZ_PiCutSolution temp = CreateWorkBill_QZ_PiCutSolution(new Tuple<int, int, int, int, int>(_msg.Item1, _msg.Item2, _msg.Item3, _totalbatchNo, _curbatchNo),
                    item, _workbill.OriginalLength,out _iftao2);

                if (_iftao2) { _iftao = true; }

                _workbill.SolutionList.Add(temp);
            }

            return _workbill;
        }
        /// <summary>
        /// 墙柱线工单，第三层级，_msg需要传入批次号等信息
        /// </summary>
        /// <param name="_msg">班次、子加工批总批次、子加工批当前批号、锯切方案总数、当前锯切方案序号</param>
        /// <param name="_oriPi"></param>
        /// <returns></returns>
        public WorkBill_QZ_PiCutSolution CreateWorkBill_QZ_PiCutSolution(Tuple<int, int, int, int, int> _msg, RebarOriPi _oriPi, int _orilength,out bool _iftao)
        {
            WorkBill_QZ_PiCutSolution _workbill = new WorkBill_QZ_PiCutSolution();

            string _Date = DateTime.Now.ToString("yyyyMMdd");
            _workbill.SeriNo = "B_" + _Date + "_"
                                + _msg.Item1.ToString() + "_"
                                + "001" + "/"
                                + "001" + "_"
                                + _msg.Item2.ToString().PadLeft(3, '0') + "/"
                                + _msg.Item3.ToString().PadLeft(3, '0') + "_"
                                + _msg.Item4.ToString().PadLeft(3, '0') + "/"
                                + _msg.Item5.ToString().PadLeft(3, '0');

            _workbill.OriginalLength = _orilength;//原材长度
            _workbill.Num = _oriPi.num;
            _workbill.OriginalNum=_oriPi.OriginalNum;
            _workbill.SteelbarNum= _oriPi.SteelbarNum;
            _workbill.OriginalWeight= _oriPi.OriginalWeight;
            _workbill.SteelbarWeight= _oriPi.SteelbarWeight;
            _workbill.PicString =graphics.BitmapToBase64String( graphics.PaintRebar(_oriPi._list[0]));//将钢筋原材排布图画出来，bitmap再转为base64编码字符串，20250929

            _oriPi.totalBatchNo = _msg.Item4;//总批次
            _oriPi.curBatchNo = _msg.Item5;//当前批次

             _iftao = false;
            foreach (var item in _oriPi._list[0]._list)
            {
                int _totalbNo = _oriPi._list[0]._list.Count;                  //锯切方案的钢筋小段数
                int _curNo = _oriPi._list[0]._list.IndexOf(item);        //当前小段

                bool _iftao2 = false;
                WorkBill_QZ_PiCutRebar temp = CreateWorkBill_QZ_PiCutRebar(new Tuple<int, int, int, int, int, int, int>(_msg.Item1, _msg.Item2, _msg.Item3, _msg.Item4, _msg.Item5, _totalbNo, _curNo), item,out _iftao2);

                if (_iftao2) { _iftao = true; }

                _workbill.RebarList.Add(temp);
            }

            return _workbill;
        }
        /// <summary>
        /// 墙柱线工单，第四层级，_msg需要传入批次号等信息
        /// </summary>
        /// <param name="_msg">班次、子加工批总批次、子加工批当前批号、锯切方案总数、当前锯切方案序号、钢筋小段总数、当前钢筋小段</param>
        /// <param name="_rebar"></param>
        /// <returns></returns>
        public WorkBill_QZ_PiCutRebar CreateWorkBill_QZ_PiCutRebar(Tuple<int, int, int, int, int, int, int> _msg, Rebar _rebar,out bool _iftao)
        {
            WorkBill_QZ_PiCutRebar _workbill = new WorkBill_QZ_PiCutRebar();

            string _Date = DateTime.Now.ToString("yyyyMMdd");
            _workbill.SeriNo = "B_" + _Date + "_"
                                + _msg.Item1.ToString() + "_"
                                + "001" + "/"
                                + "001" + "_"
                                + _msg.Item2.ToString().PadLeft(3, '0') + "/"
                                + _msg.Item3.ToString().PadLeft(3, '0') + "_"
                                + _msg.Item4.ToString().PadLeft(3, '0') + "/"
                                + _msg.Item5.ToString().PadLeft(3, '0') + "_"
                                + _msg.Item6.ToString().PadLeft(3, '0') + "/"
                                + _msg.Item7.ToString().PadLeft(3, '0');

            _workbill.CornerMsg = _rebar.CornerMessage;
            _workbill.Length = _rebar.length;
            _workbill.SteelbarWeight = _rebar.weight;
            _workbill.BendType = _rebar.IfBend;
            _workbill.TaosiType = _rebar.TaosiType;
            _workbill.UniqueCode=_rebar.SerialNum;//料单标注序号

            if(_rebar.TaosiType==0)//是否套丝
            {
                _iftao = false;
            }
            else
            {
                _iftao=true;
            }

            return _workbill;
        }

        //public 
        public WorkBill_LB_SingleRebar CreateWorkBill_LB_Rebar(SingleRebarMsg _msg, Rebar _data)
        {
            WorkBill_LB_SingleRebar _singleRebar = new WorkBill_LB_SingleRebar();

            string _Date = DateTime.Now.ToString("yyyyMMdd");
            _singleRebar.SeriNo = _Date + "_"
                                + _msg.shift.ToString() + "_"
                                + _msg.BatchMsg.totalBatch.ToString().PadLeft(3, '0') + "/"
                                + _msg.BatchMsg.curBatch.ToString().PadLeft(3, '0') + "_"
                                + _msg.BatchMsg.totalchildBatch.ToString().PadLeft(3, '0') + "/"
                                + (_msg.BatchMsg.curChildBatch + 1).ToString().PadLeft(3, '0') + "_"
                                + _msg.totalOriginal.ToString().PadLeft(3, '0') + "/"
                                + (_msg.curOriginal + 1).ToString().PadLeft(3, '0') + "_"
                                + (_msg.curSingle + 1).ToString();//所有序号都从1开始编号
            _singleRebar.ProjectName = _data.ProjectName;
            _singleRebar.AssemblyName = _data.MainAssemblyName;
            _singleRebar.ElementName = _data.ElementName;
            _singleRebar.WareInfo = _msg.wareMsg.warehouseNo.ToString() + "_" + GeneralClass.EnumWareSetToInt(_msg.wareMsg.wareSet)/*GeneralClass.wareNum[(int)_msg.wareMsg.totalware].ToString() */+ "_" + _msg.wareMsg.wareno.ToString();
            _singleRebar.PicNo = _data.PicTypeNum;
            _singleRebar.TaosiType = _data.TaosiType;//套丝类型
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

            int _size = 0;
            //根据经验公式计算重量(kg)，保留3位小数
            _singleRebar.Weight = Math.Round(0.00617 * (double)_singleRebar.Diameter * (double)_singleRebar.Diameter * (double)_singleRebar.Length / 1000, 3);
            _singleRebar.CornerMsg = _data.CornerMessage;
            /*转换规则：项目编号   +  钢筋数据库indexcode   +   同一个钢筋rebardata中的流水号
            //          1位字母    +  4位的30进制数         +   1位的30进制数            */

            string temp1 = ConvertCode(_data.IndexNo, 3, out _size).ToUpper().PadLeft(3, '0');
            string temp2 = ConvertCode(_data.seriNo, 1,out _size).ToUpper().PadLeft(1, '0');
            _singleRebar.IndexCode = "A" + temp1 + temp2;

            _singleRebar.UniqueCode = _data.SerialNum;

            return _singleRebar;
        }
        /// <summary>
        /// 梁板线磁吸上料工单
        /// </summary>
        /// <param name="_msg"></param>
        /// <param name="_batchlist"></param>
        /// <returns></returns>
        public string CreateWorkBill_CX(WorkBillMsg _msg, List<RebarOri> _orilist)
        {
            string returnstr = "";

            try
            {
                WorkBill_CX _workbill = new WorkBill_CX();

                _workbill.Msgtype = 10;
                string _Date = DateTime.Now.ToString("yyyyMMdd");
                _workbill.BillNo = "L_" + _Date + "_" + _msg.shift.ToString() + "_";
                _workbill.ProjectName = _msg.projectName;
                _workbill.Block = _msg.block;
                _workbill.Building = _msg.building;
                _workbill.Floor = _msg.floor;
                _workbill.Level = _msg.level;
                _workbill.Brand = _msg.brand;
                _workbill.Specification = _msg.specification;
                _workbill.OriginalLength = _msg.originLength;

                //按照直径和长度进行分组，分组后得到GeneralMaterial原材数据
                var _group = _orilist.GroupBy(t => new { t._level, t._diameter, t._totalLength }).Select(
                    y => new MaterialOri
                    {
                        _level = y.Key._level,
                        _diameter = GeneralClass.IntToEnumDiameter(y.Key._diameter),
                        _length = y.Key._totalLength,
                        _num = y.Count()
                    }).OrderBy(t => t._length).ToList();

                int _index = 0;
                //12米或9米原材
                foreach (var item in _group.Where(t => t._length == GeneralClass.OriginalLength(t._level, t._diameter)).OrderBy(t => t._diameter))//原材长度每个仓位放一个直径的
                {
                    _index++;
                    _workbill.LiaoCangList.Add(CreateWorkBill_CX_LiaoCang(Tuple.Create(1, _index), item));//上层料仓为1
                }
                //6米非定尺原材
                var temp = _group.Where(t => t._length == 6000).OrderBy(t => t._diameter).ToList();//6米非定尺原材，两个直径的放一个仓位
                for (int i = 0; i < temp.Count / 2; i++)
                {
                    _index++;
                    _workbill.LiaoCangList.Add(CreateWorkBill_CX_LiaoCang(Tuple.Create(1, _index), temp[i * 2], temp[i * 2 + 1]));//上层料仓为1
                }
                if (temp.Count % 2 != 0)
                {
                    _index++;
                    _workbill.LiaoCangList.Add(CreateWorkBill_CX_LiaoCang(Tuple.Create(1, _index), temp.Last()));//上层料仓为1
                }

                //3米
                _index = 0;
                foreach (var item in _group.Where(t => t._length == 3000).OrderBy(t => t._diameter))//原材长度每个仓位放一个直径的
                {
                    _index++;
                    _workbill.LiaoCangList.Add(CreateWorkBill_CX_LiaoCang(Tuple.Create(2, _index), item));//下层料仓为2
                }
                //1.5米
                var tttt = _group.Where(t => t._length == 1500).OrderBy(t => t._diameter).ToList();//1.5米非定尺原材，两个直径的放一个仓位
                for (int i = 0; i < tttt.Count / 2; i++)
                {
                    _index++;
                    _workbill.LiaoCangList.Add(CreateWorkBill_CX_LiaoCang(Tuple.Create(2, _index), tttt[i * 2], tttt[i * 2 + 1]));//下层料仓为2
                }
                if (tttt.Count % 2 != 0)
                {
                    _index++;
                    _workbill.LiaoCangList.Add(CreateWorkBill_CX_LiaoCang(Tuple.Create(2, _index), tttt.Last()));//下层料仓为2
                }



                //3米和1.5米
                //for (var i = EnumDiaBang.BANG_C12; i <= EnumDiaBang.BANG_C40; i++)
                //{
                //    var _one = _group.Where(t => t._diameter == i && t._length == 1500).ToList();
                //    var _three = _group.Where(t => t._diameter == i && t._length == 3000).ToList();

                //    var _onemat = (_one == null || _one.Count == 0) ? new GeneralMaterial() : _one.ToList()[0];
                //    var _threemat = (_three == null || _three.Count == 0) ? new GeneralMaterial() : _three.ToList()[0];
                //    _workbill.LiaoCangList.Add(CreateWorkBill_CX_LiaoCang(Tuple.Create(2, (int)i), _onemat, _threemat));//下层料仓为2
                //}


                returnstr = NewtonJson.Serializer(_workbill);//json序列化

            }
            catch (Exception ex) { MessageBox.Show("CreateWorkBill_CX error:" + ex.Message); }

            return returnstr;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_pos"></param>
        /// <param name="_material"></param>
        /// <returns></returns>
        public WorkBill_CX_LiaoCang CreateWorkBill_CX_LiaoCang(Tuple<int, int> _pos, MaterialOri _material)
        {
            WorkBill_CX_LiaoCang _liaocang = new WorkBill_CX_LiaoCang();
            _liaocang.WarePos = _pos.Item1;
            _liaocang.WareNo = _pos.Item2;
            _liaocang.Part1_diameter = GeneralClass.EnumDiameterToInt(_material._diameter);
            _liaocang.Part1_length = _material._length;
            _liaocang.Part1_num = _material._num;

            return _liaocang;
        }
        /// <summary>
        /// 上层6米非定尺原材，两个直径的放一个仓位；下层1.5米和3米放一个仓位
        /// </summary>
        /// <param name="_pos"></param>
        /// <param name="_material1"></param>
        /// <param name="_material2"></param>
        /// <returns></returns>
        public WorkBill_CX_LiaoCang CreateWorkBill_CX_LiaoCang(Tuple<int, int> _pos, MaterialOri _material1, MaterialOri _material2)
        {
            WorkBill_CX_LiaoCang _liaocang = new WorkBill_CX_LiaoCang();
            _liaocang.WarePos = _pos.Item1;
            _liaocang.WareNo = _pos.Item2;
            _liaocang.Part1_diameter = _material1._diameter == EnumDiaBang.NONE ? 0 : GeneralClass.EnumDiameterToInt(_material1._diameter);
            _liaocang.Part1_length = _material1._length;
            _liaocang.Part1_num = _material1._num;
            _liaocang.Part2_diameter = _material2._diameter == EnumDiaBang.NONE ? 0 : GeneralClass.EnumDiameterToInt(_material2._diameter);
            _liaocang.Part2_length = _material2._length;
            _liaocang.Part2_num = _material2._num;

            return _liaocang;
        }
        /// <summary>
        /// 取余倒排法转换进制，将10进制转换成30进制
        /// 1、首字符为项目代号，由大写字母组成。
        ///      大写字母：A ~Z
        ///      排除可能引起混淆的字母B、D、I、J、O、W
        /// 2、后五位字符代表钢筋编号，由数字和大写字母组成。
        ///      数字：0~9；
        ///      大写字母：A ~Z
        ///      排除可能引起混淆的字母B、D、I、J、O、W
        /// 3、注：        B——易与数字8混淆；D——易与数字0混淆；I——易与数字1混淆；J——易与数字1混淆；O——易与数字0混淆；W——字符太宽
        /// 4、注：    20241031排除
        ///                    3——易与8和9混淆、9——易与3和8混淆、Q——易与0混淆、S——易与5混淆
        /// </summary>
        /// <param name="_value">待转换的十进制数</param>
        /// <param name="_limitsize">限制转换的位数</param>
        /// <param name="_size">返回进制数</param>
        /// <param name="_basestr">任意进制的基础字符，其长度即为新的进制数</param>
        /// <returns></returns>
        private string ConvertCode(int _value, int _limitsize, out int _size, string _basestr = "01278AEGKU" /*"01245678ACEFGHKLMNPRTUVXYZ"*/ /*"0123456789ACEFGHKLMNPQRSTUVXYZ"*/)
        {
            string sss = "";
            int _jinzhi = _basestr.Length;
            _size = _jinzhi;

            while (_value > 0)
            {
                var temp = _value % _jinzhi;//取余倒排法转换进制

                //sss += _basestr[temp].ToString() ;
                sss = _basestr[temp].ToString() + sss;//20240925修改重码的bug，先取的余数放后面，不能用sss+=

                _value = _value / _jinzhi;
            }

            sss=sss.Substring(sss.Length < _limitsize ? 0 : (sss.Length - _limitsize));//截取limitsize限制的位数，不允许超过limitsize位数，考虑
            return sss;
        }
    }
}
