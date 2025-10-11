using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RebarSampling
{
    public class ConfigData
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName {  get; set; }
        /// <summary>
        /// 工厂类型
        /// </summary>
        public EnumFactoryType Factorytype { get; set; }
        /// <summary>
        /// 语言设置
        /// </summary>
        public EnumLanguageType LanguageType { get; set; }
        /// <summary>
        /// Φ12直径钢筋归类于线材还是棒材，false为线材，true为棒材
        /// </summary>
        public bool TypeC12 { get; set; }
        /// <summary>
        /// Φ14直径钢筋归类于线材还是棒材，false为线材，true为棒材
        /// </summary>
        public bool TypeC14 { get; set;}
        /// <summary>
        /// 工厂，高效/柔性
        /// </summary>
        public EnumFactory Factory { get; set; }
        /// <summary>
        /// 料单类型，e筋还是广联达
        /// </summary>
        public EnumMaterialBill MaterialBill { get; set; }
        /// <summary>
        /// 广联达料单的默认路径
        /// </summary>
        public string GLDpath { get; set; }
        /// <summary>
        /// E筋料单的默认路径
        /// </summary>
        public string EJINpath { get; set; }
        /// <summary>
        /// 是否加载短钢筋数据（1.5米以下）
        /// </summary>
        public bool IfShortRebar { get; set; }
        /// <summary>
        /// 可锯切的最短长度
        /// </summary>
        public int MinLength { get; set; }
        /// <summary>
        /// 三级钢的原材库，考虑不同直径可能会用不同长度的钢筋，9米/12米
        /// </summary>      
        
        public List<MaterialOri> MaterialOriPool_3 { get; set; }
        /// <summary>
        /// 四级钢的原材库，考虑不同直径可能会用不同长度的钢筋，9米/12米
        /// </summary>
        public List<MaterialOri> MaterialOriPool_4 {  get; set; }

        //public EnumOriType OriginType { get; set; }

        /// <summary>
        /// 是否切端头
        /// </summary>
        public bool IfCutHead { get; set; }
        /// <summary>
        /// 切端头距离
        /// </summary>
        public int CutHeadLength { get; set; }
        /// <summary>
        /// 直径种类分组方式，顺序分组、混合分组
        /// </summary>
        public EnumDiaGroupTypeSetting DiaGroupType { get; set; }
        /// <summary>
        /// webserver的ip地址，默认为本机127.0.0.1
        /// </summary>
        public string webserverIP { get; set; }
        /// <summary>
        /// webserver的监听端口
        /// </summary>
        public string webserverPort { get; set;}

        /// <summary>
        /// 余料阈值
        /// </summary>
        public int LeftThreshold { get; set; }
        /// <summary>
        /// 余料最大阈值
        /// </summary>
        public int MaxThreshold { get; set; }
        /// <summary>
        /// 套料算法方式
        /// </summary>
        public EnumTaoType TaoType { get; set; }
        /// <summary>
        /// 原材库设置，切换整数倍模数和等间距模数
        /// </summary>
        public EnumMatPoolSetType MatPoolSetType { get; set; }
        /// <summary>
        /// 是否选择3米的原材库
        /// </summary>
        public bool MatPoolHave3 { get; set; }
        /// <summary>
        /// 是否选择6米的原材库
        /// </summary>
        public bool MatPoolHave6 { get; set; }
        /// <summary>
        /// 纳入原材库的余料1
        /// </summary>
        public int MatPoolYuliao1 { get; set; }
        /// <summary>
        /// 纳入原材库的余料2
        /// </summary>
        public int MatPoolYuliao2 { get; set; }
        /// <summary>
        /// 使用的数据库类型
        /// </summary>
        public EnumDatabaseType DatabaseType { get; set; }
        /// <summary>
        /// 料仓区间设置，仓位划分的数量阈值区间，五种仓位，四个阈值节点:10,25,50,100
        /// </summary>
        public int WareAreaSet1 {  get; set; }
        /// <summary>
        /// 料仓区间设置，仓位划分的数量阈值区间，五种仓位，四个阈值节点:10,25,50,100
        /// </summary>
        public int WareAreaSet2 { get;set; }
        /// <summary>
        /// 料仓区间设置，仓位划分的数量阈值区间，五种仓位，四个阈值节点:10,25,50,100
        /// </summary>
        public int WareAreaSet3 { get;set; }
        /// <summary>
        /// 料仓区间设置，仓位划分的数量阈值区间，五种仓位，四个阈值节点:10,25,50,100
        /// </summary>
        public int WareAreaSet4 { get; set; }
        /// <summary>
        /// 智能料仓通道
        /// </summary>
        public int WareHouseChannels {  get; set; }

        /// <summary>
        /// 套料时，原材是否参与套料排程
        /// </summary>
        public bool IfOrignalTao { get; set; }
        /// <summary>
        /// 套料时，是否优化套料排序，包括连续原材的排序、一根原材内的小段的排序
        /// </summary>
        public bool IfSeriTao {  get; set; }

        /// <summary>
        /// 弯曲参数，正角度最长弯拐长度
        /// </summary>
        public int P_AngleMaxLength { get; set; }
        /// <summary>
        /// 弯曲参数，负角度最长弯拐长度
        /// </summary>
        public int N_AngleMaxLength { get; set; }
        /// <summary>
        /// 弯曲参数，最短中段长度
        /// </summary>
        public int MinMiddleLength { get; set; }
        /// <summary>
        /// 弯拐长度超过某个长度，自动设负角度
        /// </summary>
        public int OverLengthAutoN_Angle {  get; set; }
        /// <summary>
        /// 角度小于某个值，自动不弯
        /// </summary>
        public int BelowAngleAutoNoBend {  get; set; }
        /// <summary>
        /// 缩尺设置，几根一缩，五个区间，1~5mm，5~10mm，10~20mm，20~30mm，30~mm
        /// </summary>
        public int SuoChiNum_1 {  get; set; }
        /// <summary>
        ///  缩尺设置，几根一缩，五个区间，1~5mm，5~10mm，10~20mm，20~30mm，30~mm
        /// </summary>
        public int SuoChiNum_2 { get; set; }
        /// <summary>
        ///  缩尺设置，几根一缩，五个区间，1~5mm，5~10mm，10~20mm，20~30mm，30~mm
        /// </summary>
        public int SuoChiNum_3 { get; set; }
        /// <summary>
        ///  缩尺设置，几根一缩，五个区间，1~5mm，5~10mm，10~20mm，20~30mm，30~mm
        /// </summary>
        public int SuoChiNum_4 { get; set; }
        /// <summary>
        ///  缩尺设置，几根一缩，五个区间，1~5mm，5~10mm，10~20mm，20~30mm，30~mm
        /// </summary>
        public int SuoChiNum_5 { get; set; }
        /// <summary>
        /// 每吊钢筋分解策略：按重量阈值(kg)分解
        /// </summary>
        public int SplitWeightThreshold { get; set; }
        /// <summary>
        /// 带弯拐的分解策略：按每吊的数量阈值（根）分解，注意对应不同直径的数量不一样
        /// </summary>
        public int SplitNumThresholdWithBend_16 { get; set; }
        public int SplitNumThresholdWithBend_18 { get; set; }
        public int SplitNumThresholdWithBend_20 { get; set; }
        public int SplitNumThresholdWithBend_22 { get; set; }
        public int SplitNumThresholdWithBend_25 { get; set; }
        public int SplitNumThresholdWithBend_28 { get; set; }
        public int SplitNumThresholdWithBend_32 { get; set; }
        /// <summary>
        /// 分解是否重量优先
        /// </summary>
        public bool SplitIfWeightFirst { get; set; }
        /// <summary>
        /// 单头正丝翻转边角结构的倾向选择，-1:翻转至左侧，1:翻转至右侧，0:不翻转
        /// </summary>
        public int InverseCornerMsgForTao { get; set; }
        /// <summary>
        /// 单头反丝翻转边角结构的倾向设置，-1:翻转至左侧，1：翻转至右侧，0：不翻转
        /// </summary>
        public int InverseCornerMsgForFan {  get; set; }

        

    }
    public class Config
    {
        public static string filepath = Directory.GetCurrentDirectory() + @"\configfile\config.json";

        static ReaderWriterLockSlim LogWriteLock = new ReaderWriterLockSlim();

        public static void SaveConfig(string _json)
        {
            try
            {
                LogWriteLock.EnterWriteLock();

                if (!File.Exists(filepath))
                {
                    File.Create(filepath);
                }
                if(File.Exists(filepath))
                {
                    File.WriteAllText(filepath, _json);
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
            finally { LogWriteLock.ExitWriteLock(); }
        }

        public static string LoadConfig()
        {
            try
            {
                LogWriteLock.EnterWriteLock();

                string rt = "";

                if (File.Exists(filepath))
                {
                    rt = File.ReadAllText(filepath);
                }
                return rt;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                return "";
            }
            finally { LogWriteLock.ExitWriteLock();  }


        }
    }
}
