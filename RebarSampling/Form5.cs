using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RebarSampling
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();

            loadconfig();

        }

        private void loadconfig()
        {
            string _json = Config.LoadConfig();
            GeneralClass.CfgData = NewtonJson.Deserializer<ConfigData>(_json);
            //ConfigData _data = JsonConvert.DeserializeObject<ConfigData>(_json);


            if (GeneralClass.CfgData != null)
            {
                //工厂类型
                //GeneralClass.m_factoryType = _data.Factorytype;
                switch (GeneralClass.CfgData.Factorytype)
                {
                    case EnumFactoryType.LowConfig:
                        radioButton1.Checked = true;
                        break;
                    case EnumFactoryType.HighConfig:
                        radioButton2.Checked = true;
                        break;
                    case EnumFactoryType.Experiment:
                        radioButton3.Checked = true;
                        break;
                    default:
                        break;
                }

                //钢筋材料类型
                //GeneralClass.m_typeC12 = _data.TypeC12;
                //GeneralClass.m_typeC14 = _data.TypeC14;
                checkBox1.Checked = GeneralClass.CfgData.TypeC12;
                checkBox1.Text = GeneralClass.CfgData.TypeC12 ? "Φ12-棒材" : "Φ12-线材";
                checkBox2.Checked = GeneralClass.CfgData.TypeC14;
                checkBox2.Text = GeneralClass.CfgData.TypeC14 ? "Φ14-棒材" : "Φ14-线材";

                //    高效/柔性工厂
                switch (GeneralClass.CfgData.Factory)
                {
                    case EnumFactory.GaoXiao:
                        radioButton4.Checked = true;
                        break;
                    case EnumFactory.RouXing:
                        radioButton5.Checked = true;
                        break;
                    default:
                        break;
                }

                //是否加载短钢筋数据
                checkBox3.Checked = GeneralClass.CfgData.IfShortRebar;
                checkBox3.Text = GeneralClass.CfgData.IfShortRebar ? "加载短钢筋数据" : "不加载短钢筋数据";
                textBox1.Text = GeneralClass.CfgData.MinLength.ToString();

                try
                {

                //原材库，三级钢
                radioButton6.Checked=(GeneralClass.CfgData.MaterialOriPool_3.Find(t=>t._diameter==EnumDiaBang.BANG_C12 )._length==9000)?true:false;
                radioButton7.Checked= (GeneralClass.CfgData.MaterialOriPool_3.Find(t => t._diameter == EnumDiaBang.BANG_C12)._length == 12000) ? true : false;
                radioButton18.Checked = (GeneralClass.CfgData.MaterialOriPool_3.Find(t => t._diameter == EnumDiaBang.BANG_C14)._length == 9000) ? true : false;
                radioButton19.Checked = (GeneralClass.CfgData.MaterialOriPool_3.Find(t => t._diameter == EnumDiaBang.BANG_C14)._length == 12000) ? true : false;
                radioButton20.Checked = (GeneralClass.CfgData.MaterialOriPool_3.Find(t => t._diameter == EnumDiaBang.BANG_C16)._length == 9000) ? true : false;
                radioButton21.Checked = (GeneralClass.CfgData.MaterialOriPool_3.Find(t => t._diameter == EnumDiaBang.BANG_C16)._length == 12000) ? true : false;
                radioButton22.Checked = (GeneralClass.CfgData.MaterialOriPool_3.Find(t => t._diameter == EnumDiaBang.BANG_C18)._length == 9000) ? true : false;
                radioButton23.Checked = (GeneralClass.CfgData.MaterialOriPool_3.Find(t => t._diameter == EnumDiaBang.BANG_C18)._length == 12000) ? true : false;
                radioButton24.Checked = (GeneralClass.CfgData.MaterialOriPool_3.Find(t => t._diameter == EnumDiaBang.BANG_C20)._length == 9000) ? true : false;
                radioButton25.Checked = (GeneralClass.CfgData.MaterialOriPool_3.Find(t => t._diameter == EnumDiaBang.BANG_C20)._length == 12000) ? true : false;
                radioButton26.Checked = (GeneralClass.CfgData.MaterialOriPool_3.Find(t => t._diameter == EnumDiaBang.BANG_C22)._length == 9000) ? true : false;
                radioButton27.Checked = (GeneralClass.CfgData.MaterialOriPool_3.Find(t => t._diameter == EnumDiaBang.BANG_C22)._length == 12000) ? true : false;
                radioButton28.Checked = (GeneralClass.CfgData.MaterialOriPool_3.Find(t => t._diameter == EnumDiaBang.BANG_C25)._length == 9000) ? true : false;
                radioButton29.Checked = (GeneralClass.CfgData.MaterialOriPool_3.Find(t => t._diameter == EnumDiaBang.BANG_C25)._length == 12000) ? true : false;
                radioButton30.Checked = (GeneralClass.CfgData.MaterialOriPool_3.Find(t => t._diameter == EnumDiaBang.BANG_C28)._length == 9000) ? true : false;
                radioButton31.Checked = (GeneralClass.CfgData.MaterialOriPool_3.Find(t => t._diameter == EnumDiaBang.BANG_C28)._length == 12000) ? true : false;
                radioButton32.Checked = (GeneralClass.CfgData.MaterialOriPool_3.Find(t => t._diameter == EnumDiaBang.BANG_C32)._length == 9000) ? true : false;
                radioButton33.Checked = (GeneralClass.CfgData.MaterialOriPool_3.Find(t => t._diameter == EnumDiaBang.BANG_C32)._length == 12000) ? true : false;
                radioButton34.Checked = (GeneralClass.CfgData.MaterialOriPool_3.Find(t => t._diameter == EnumDiaBang.BANG_C36)._length == 9000) ? true : false;
                radioButton35.Checked = (GeneralClass.CfgData.MaterialOriPool_3.Find(t => t._diameter == EnumDiaBang.BANG_C36)._length == 12000) ? true : false;
                radioButton36.Checked = (GeneralClass.CfgData.MaterialOriPool_3.Find(t => t._diameter == EnumDiaBang.BANG_C40)._length == 9000) ? true : false;
                radioButton37.Checked = (GeneralClass.CfgData.MaterialOriPool_3.Find(t => t._diameter == EnumDiaBang.BANG_C40)._length == 12000) ? true : false;
                //原材库，四级钢
                radioButton38.Checked = (GeneralClass.CfgData.MaterialOriPool_4.Find(t => t._diameter == EnumDiaBang.BANG_C12)._length == 9000) ? true : false;
                radioButton39.Checked = (GeneralClass.CfgData.MaterialOriPool_4.Find(t => t._diameter == EnumDiaBang.BANG_C12)._length == 12000) ? true : false;
                radioButton40.Checked = (GeneralClass.CfgData.MaterialOriPool_4.Find(t => t._diameter == EnumDiaBang.BANG_C14)._length == 9000) ? true : false;
                radioButton41.Checked = (GeneralClass.CfgData.MaterialOriPool_4.Find(t => t._diameter == EnumDiaBang.BANG_C14)._length == 12000) ? true : false;
                radioButton42.Checked = (GeneralClass.CfgData.MaterialOriPool_4.Find(t => t._diameter == EnumDiaBang.BANG_C16)._length == 9000) ? true : false;
                radioButton43.Checked = (GeneralClass.CfgData.MaterialOriPool_4.Find(t => t._diameter == EnumDiaBang.BANG_C16)._length == 12000) ? true : false;
                radioButton44.Checked = (GeneralClass.CfgData.MaterialOriPool_4.Find(t => t._diameter == EnumDiaBang.BANG_C18)._length == 9000) ? true : false;
                radioButton45.Checked = (GeneralClass.CfgData.MaterialOriPool_4.Find(t => t._diameter == EnumDiaBang.BANG_C18)._length == 12000) ? true : false;
                radioButton46.Checked = (GeneralClass.CfgData.MaterialOriPool_4.Find(t => t._diameter == EnumDiaBang.BANG_C20)._length == 9000) ? true : false;
                radioButton47.Checked = (GeneralClass.CfgData.MaterialOriPool_4.Find(t => t._diameter == EnumDiaBang.BANG_C20)._length == 12000) ? true : false;
                radioButton48.Checked = (GeneralClass.CfgData.MaterialOriPool_4.Find(t => t._diameter == EnumDiaBang.BANG_C22)._length == 9000) ? true : false;
                radioButton49.Checked = (GeneralClass.CfgData.MaterialOriPool_4.Find(t => t._diameter == EnumDiaBang.BANG_C22)._length == 12000) ? true : false;
                radioButton50.Checked = (GeneralClass.CfgData.MaterialOriPool_4.Find(t => t._diameter == EnumDiaBang.BANG_C25)._length == 9000) ? true : false;
                radioButton51.Checked = (GeneralClass.CfgData.MaterialOriPool_4.Find(t => t._diameter == EnumDiaBang.BANG_C25)._length == 12000) ? true : false;
                radioButton52.Checked = (GeneralClass.CfgData.MaterialOriPool_4.Find(t => t._diameter == EnumDiaBang.BANG_C28)._length == 9000) ? true : false;
                radioButton53.Checked = (GeneralClass.CfgData.MaterialOriPool_4.Find(t => t._diameter == EnumDiaBang.BANG_C28)._length == 12000) ? true : false;
                radioButton54.Checked = (GeneralClass.CfgData.MaterialOriPool_4.Find(t => t._diameter == EnumDiaBang.BANG_C32)._length == 9000) ? true : false;
                radioButton55.Checked = (GeneralClass.CfgData.MaterialOriPool_4.Find(t => t._diameter == EnumDiaBang.BANG_C32)._length == 12000) ? true : false;
                radioButton56.Checked = (GeneralClass.CfgData.MaterialOriPool_4.Find(t => t._diameter == EnumDiaBang.BANG_C36)._length == 9000) ? true : false;
                radioButton57.Checked = (GeneralClass.CfgData.MaterialOriPool_4.Find(t => t._diameter == EnumDiaBang.BANG_C36)._length == 12000) ? true : false;
                radioButton58.Checked = (GeneralClass.CfgData.MaterialOriPool_4.Find(t => t._diameter == EnumDiaBang.BANG_C40)._length == 9000) ? true : false;
                radioButton59.Checked = (GeneralClass.CfgData.MaterialOriPool_4.Find(t => t._diameter == EnumDiaBang.BANG_C40)._length == 12000) ? true : false;
                }
                catch (Exception e) { MessageBox.Show("loadconfig error" + e.Message); }


                //switch (GeneralClass.CfgData.OriginType)
                //{
                //    case EnumOriType.ORI_9:
                //        radioButton6.Checked = true;
                //        break;
                //    case EnumOriType.ORI_12:
                //        radioButton7.Checked = true;
                //        break;
                //    default:
                //        break;
                //}

                //是否切端头
                checkBox4.Checked = GeneralClass.CfgData.IfCutHead;
                checkBox4.Text = GeneralClass.CfgData.IfCutHead ? "切端头" : "不切端头";
                textBox2.Text = GeneralClass.CfgData.CutHeadLength.ToString();

                //直径种类分组方式，混合分组还是顺序分组
                switch (GeneralClass.CfgData.DiaGroupType)
                {
                    case EnumDiaGroupTypeSetting.Sequence:
                        radioButton8.Checked = true;
                        break;
                    case EnumDiaGroupTypeSetting.Mix:
                        radioButton9.Checked = true;
                        break;
                    default:
                        break;
                }

                //余料阈值
                textBox3.Text = GeneralClass.CfgData.LeftThreshold.ToString();
                textBox4.Text = GeneralClass.CfgData.MaxThreshold.ToString();

                //套料算法方式
                switch (GeneralClass.CfgData.TaoType)
                {
                    case EnumTaoType.ORITAO:
                        radioButton10.Checked = true;
                        break;
                    case EnumTaoType.ORIPOOLTAO:
                        radioButton11.Checked = true;
                        break;
                    case EnumTaoType.CUTTAO_2:
                        radioButton12.Checked = true;
                        break;
                    case EnumTaoType.CUTTAO_3:
                        radioButton15.Checked = true;
                        break;
                    default:
                        break;
                }

                //原材库设置，模数选择
                switch (GeneralClass.CfgData.MatPoolSetType)
                {
                    case EnumMatPoolSetType.INTEGER:
                        radioButton13.Checked = true;
                        break;
                    case EnumMatPoolSetType.AVERAGE:
                        radioButton14.Checked = true;
                        break;
                    default:
                        break;
                }
                checkBox5.Checked = GeneralClass.CfgData.MatPoolHave3;
                checkBox6.Checked = GeneralClass.CfgData.MatPoolHave6;

                //纳入原材库的余料参数
                textBox5.Text = GeneralClass.CfgData.MatPoolYuliao1.ToString();
                textBox6.Text = GeneralClass.CfgData.MatPoolYuliao2.ToString();

                //数据库设置
                switch (GeneralClass.CfgData.DatabaseType)
                {
                    case EnumDatabaseType.SQLITE:
                        radioButton16.Checked = true;
                        break;
                    case EnumDatabaseType.MYSQL:
                        radioButton17.Checked = true;
                        break;
                    default:
                        break;
                }

                //仓位数量区间设置
                textBox7.Text = GeneralClass.CfgData.WareAreaSet1.ToString();
                textBox8.Text=GeneralClass.CfgData.WareAreaSet2.ToString();
                textBox9.Text=GeneralClass.CfgData.WareAreaSet3.ToString();
                //料仓通道
                textBox10.Text=GeneralClass.CfgData.WareHouseChannels.ToString();

                //原材是否参与套料
                checkBox7.Checked = GeneralClass.CfgData.IfOrignalTao;
                checkBox7.Text = GeneralClass.CfgData.IfOrignalTao ? "原材参与套料" : "原材不参与套料";

                //套料排序是否优化
                checkBox8.Checked = GeneralClass.CfgData.IfSeriTao;
                checkBox8.Text = GeneralClass.CfgData.IfSeriTao ? "执行套料排序优化" : "不执行套料排序优化";
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            //ConfigData _data = new ConfigData();

            //修改工厂类型
            if (radioButton1.Checked)
            {
                GeneralClass.CfgData.Factorytype = EnumFactoryType.LowConfig;
            }
            if (radioButton2.Checked)
            {
                GeneralClass.CfgData.Factorytype = EnumFactoryType.HighConfig;
            }
            if (radioButton3.Checked)
            {
                GeneralClass.CfgData.Factorytype = EnumFactoryType.Experiment;
            }

            //设置Φ12直径和Φ14直径钢筋归属于线材还是棒材
            GeneralClass.CfgData.TypeC12 = checkBox1.Checked;
            GeneralClass.CfgData.TypeC14 = checkBox2.Checked;

            //切换工厂
            if (radioButton4.Checked)
            {
                GeneralClass.CfgData.Factory = EnumFactory.GaoXiao;
            }
            if (radioButton5.Checked)
            {
                GeneralClass.CfgData.Factory = EnumFactory.RouXing;
            }

            //定尺锯切最短长度
            GeneralClass.CfgData.MinLength = Convert.ToInt32(textBox1.Text);
            //是否加载短钢筋数据
            GeneralClass.CfgData.IfShortRebar = checkBox3.Checked;

            //原材库
            GeneralClass.CfgData.MaterialOriPool_3 = new List<MaterialOri>();
            GeneralClass.CfgData.MaterialOriPool_3.Add(new MaterialOri(EnumDiaBang.BANG_C12, radioButton6.Checked ? 9000 : 12000, 9999, "C"));
            GeneralClass.CfgData.MaterialOriPool_3.Add(new MaterialOri(EnumDiaBang.BANG_C14, radioButton18.Checked ? 9000 : 12000, 9999, "C"));
            GeneralClass.CfgData.MaterialOriPool_3.Add(new MaterialOri(EnumDiaBang.BANG_C16, radioButton20.Checked ? 9000 : 12000, 9999, "C"));
            GeneralClass.CfgData.MaterialOriPool_3.Add(new MaterialOri(EnumDiaBang.BANG_C18, radioButton22.Checked ? 9000 : 12000, 9999, "C"));
            GeneralClass.CfgData.MaterialOriPool_3.Add(new MaterialOri(EnumDiaBang.BANG_C20, radioButton24.Checked ? 9000 : 12000, 9999, "C"));
            GeneralClass.CfgData.MaterialOriPool_3.Add(new MaterialOri(EnumDiaBang.BANG_C22, radioButton26.Checked ? 9000 : 12000, 9999, "C"));
            GeneralClass.CfgData.MaterialOriPool_3.Add(new MaterialOri(EnumDiaBang.BANG_C25, radioButton28.Checked ? 9000 : 12000, 9999, "C"));
            GeneralClass.CfgData.MaterialOriPool_3.Add(new MaterialOri(EnumDiaBang.BANG_C28, radioButton30.Checked ? 9000 : 12000, 9999, "C"));
            GeneralClass.CfgData.MaterialOriPool_3.Add(new MaterialOri(EnumDiaBang.BANG_C32, radioButton32.Checked ? 9000 : 12000, 9999, "C"));
            GeneralClass.CfgData.MaterialOriPool_3.Add(new MaterialOri(EnumDiaBang.BANG_C36, radioButton34.Checked ? 9000 : 12000, 9999, "C"));
            GeneralClass.CfgData.MaterialOriPool_3.Add(new MaterialOri(EnumDiaBang.BANG_C40, radioButton36.Checked ? 9000 : 12000, 9999, "C"));

            GeneralClass.CfgData.MaterialOriPool_4 = new List<MaterialOri>();
            GeneralClass.CfgData.MaterialOriPool_4.Add(new MaterialOri(EnumDiaBang.BANG_C12, radioButton38.Checked ? 9000 : 12000, 9999, "D"));
            GeneralClass.CfgData.MaterialOriPool_4.Add(new MaterialOri(EnumDiaBang.BANG_C14, radioButton40.Checked ? 9000 : 12000, 9999, "D"));
            GeneralClass.CfgData.MaterialOriPool_4.Add(new MaterialOri(EnumDiaBang.BANG_C16, radioButton42.Checked ? 9000 : 12000, 9999, "D"));
            GeneralClass.CfgData.MaterialOriPool_4.Add(new MaterialOri(EnumDiaBang.BANG_C18, radioButton44.Checked ? 9000 : 12000, 9999, "D"));
            GeneralClass.CfgData.MaterialOriPool_4.Add(new MaterialOri(EnumDiaBang.BANG_C20, radioButton46.Checked ? 9000 : 12000, 9999, "D"));
            GeneralClass.CfgData.MaterialOriPool_4.Add(new MaterialOri(EnumDiaBang.BANG_C22, radioButton48.Checked ? 9000 : 12000, 9999, "D"));
            GeneralClass.CfgData.MaterialOriPool_4.Add(new MaterialOri(EnumDiaBang.BANG_C25, radioButton50.Checked ? 9000 : 12000, 9999, "D"));
            GeneralClass.CfgData.MaterialOriPool_4.Add(new MaterialOri(EnumDiaBang.BANG_C28, radioButton52.Checked ? 9000 : 12000, 9999, "D"));
            GeneralClass.CfgData.MaterialOriPool_4.Add(new MaterialOri(EnumDiaBang.BANG_C32, radioButton54.Checked ? 9000 : 12000, 9999, "D"));
            GeneralClass.CfgData.MaterialOriPool_4.Add(new MaterialOri(EnumDiaBang.BANG_C36, radioButton56.Checked ? 9000 : 12000, 9999, "D"));
            GeneralClass.CfgData.MaterialOriPool_4.Add(new MaterialOri(EnumDiaBang.BANG_C40, radioButton58.Checked ? 9000 : 12000, 9999, "D"));

            //if (radioButton6.Checked)
            //{
            //    GeneralClass.CfgData.OriginType = EnumOriType.ORI_9;
            //    GeneralClass.AddDefaultMaterial();//20241010修改【临时改原材长度时，原材库并没有同步修改】的bug
            //}
            //if (radioButton7.Checked)
            //{
            //    GeneralClass.CfgData.OriginType = EnumOriType.ORI_12;
            //    GeneralClass.AddDefaultMaterial();//20241010修改【临时改原材长度时，原材库并没有同步修改】的bug
            //}

            //是否切端头
            GeneralClass.CfgData.IfCutHead = checkBox4.Checked;
            //切端头距离
            GeneralClass.CfgData.CutHeadLength = Convert.ToInt32(textBox2.Text);

            //直径种类分组方式
            if (radioButton8.Checked)
            {
                GeneralClass.CfgData.DiaGroupType = EnumDiaGroupTypeSetting.Sequence;
            }
            if (radioButton9.Checked)
            {
                GeneralClass.CfgData.DiaGroupType = EnumDiaGroupTypeSetting.Mix;
            }

            //套料阈值参数
            GeneralClass.CfgData.LeftThreshold = Convert.ToInt32(textBox3.Text);
            GeneralClass.CfgData.MaxThreshold = Convert.ToInt32(textBox4.Text);

            //套料算法
            if (radioButton10.Checked)
            {
                GeneralClass.CfgData.TaoType = EnumTaoType.ORITAO;
            }
            if (radioButton11.Checked)
            {
                GeneralClass.CfgData.TaoType = EnumTaoType.ORIPOOLTAO;
            }
            if (radioButton12.Checked)
            {
                GeneralClass.CfgData.TaoType = EnumTaoType.CUTTAO_2;
            }
            if (radioButton15.Checked)
            {
                GeneralClass.CfgData.TaoType = EnumTaoType.CUTTAO_3;

            }

            //原材库设置
            if (radioButton13.Checked)
            {
                GeneralClass.CfgData.MatPoolSetType = EnumMatPoolSetType.INTEGER;
            }
            if(radioButton14.Checked)
            {
                GeneralClass.CfgData.MatPoolSetType = EnumMatPoolSetType.AVERAGE;
            }
            GeneralClass.CfgData.MatPoolHave3 = checkBox5.Checked;//3米是否进入原材库
            GeneralClass.CfgData.MatPoolHave6 = checkBox6.Checked;//6米是否进入原材库

            //纳入原材库的余料长度
            GeneralClass.CfgData.MatPoolYuliao1 = Convert.ToInt32(textBox5.Text);
            GeneralClass.CfgData.MatPoolYuliao2 = Convert.ToInt32(textBox6.Text);


            //选用哪种数据库
            if(radioButton16.Checked)
            {
                GeneralClass.CfgData.DatabaseType =EnumDatabaseType.SQLITE;
            }
            if(radioButton17.Checked)
            {
                GeneralClass.CfgData.DatabaseType = EnumDatabaseType.MYSQL;
            }

            //仓位划分数量区间设置
            GeneralClass.CfgData.WareAreaSet1= Convert.ToInt32(textBox7.Text);
            GeneralClass.CfgData.WareAreaSet2 = Convert.ToInt32(textBox8.Text);
            GeneralClass.CfgData.WareAreaSet3 = Convert.ToInt32(textBox9.Text);
            //料仓通道数
            GeneralClass.CfgData.WareHouseChannels = Convert.ToInt32(textBox10.Text);


            //原材是否参与套料
            GeneralClass.CfgData.IfOrignalTao=checkBox7.Checked;

            //是否自动优化套料排序，以节省加工时间
            GeneralClass.CfgData.IfSeriTao=checkBox8.Checked;   
            
            string _json = NewtonJson.Serializer(GeneralClass.CfgData);//存为json
            Config.SaveConfig(_json);

            GeneralClass.interactivityData?.printlog(1, "系统配置已修改");

            MessageBox.Show("系统配置参数已修改成功！", "提示");

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                //checkBox1.Checked = false; 
                checkBox1.Text = "Φ12-棒材";
            }
            else
            {
                //checkBox1.Checked = true;
                checkBox1.Text = "Φ12-线材";
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                //checkBox2.Checked = false; 
                checkBox2.Text = "Φ14-棒材";
            }
            else
            {
                //checkBox2.Checked = true; 
                checkBox2.Text = "Φ14-线材";
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                checkBox3.Text = "加载短钢筋数据";
            }
            else
            {
                checkBox3.Text = "不加载短钢筋数据";
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                checkBox4.Text = "切端头";
            }
            else
            {
                checkBox4.Text = "不切端头";
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked)
            {
                checkBox7.Text = "原材参与套料";
            }
            else
            {
                checkBox7.Text = "原材不参与套料";
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked)
            {
                checkBox8.Text = "执行套料排序优化";
            }
            else
            {
                checkBox8.Text = "不执行套料排序优化";
            }
        }
    }
}
