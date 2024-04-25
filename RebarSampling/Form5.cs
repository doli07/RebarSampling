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

                //原材类型
                switch (GeneralClass.CfgData.OriginType)
                {
                    case EnumOriType.ORI_9:
                        radioButton6.Checked = true;
                        break;
                    case EnumOriType.ORI_12:
                        radioButton7.Checked = true;
                        break;
                    default:
                        break;
                }

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

                //纳入原材库的余料参数
                textBox5.Text = GeneralClass.CfgData.MatPoolYuliao1.ToString();
                textBox6.Text = GeneralClass.CfgData.MatPoolYuliao2.ToString();


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

            //原材长度
            if (radioButton6.Checked)
            {
                GeneralClass.CfgData.OriginType = EnumOriType.ORI_9;
            }
            if (radioButton7.Checked)
            {
                GeneralClass.CfgData.OriginType = EnumOriType.ORI_12;
            }

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

            //纳入原材库的余料长度
            GeneralClass.CfgData.MatPoolYuliao1 = Convert.ToInt32(textBox5.Text);
            GeneralClass.CfgData.MatPoolYuliao2 = Convert.ToInt32(textBox6.Text);




            //存为json
            string _json = NewtonJson.Serializer(GeneralClass.CfgData);
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
    }
}
