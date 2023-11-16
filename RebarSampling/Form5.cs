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
                //GeneralClass.m_factoryType = _data.Factorytype;
                switch (GeneralClass.CfgData.Factorytype)
                {
                    case EnumFactoryType.Standard:
                        radioButton1.Checked = true;
                        break;
                    case EnumFactoryType.Reduction:
                        radioButton2.Checked = true;
                        break;
                    case EnumFactoryType.Experiment:
                        radioButton3.Checked = true;
                        break;
                    default:
                        break;
                }

                //GeneralClass.m_typeC12 = _data.TypeC12;
                //GeneralClass.m_typeC14 = _data.TypeC14;
                checkBox1.Checked = GeneralClass.CfgData.TypeC12;
                checkBox1.Text = GeneralClass.CfgData.TypeC12 ? "Φ12-棒材" : "Φ12-线材";
                checkBox2.Checked = GeneralClass.CfgData.TypeC14;
                checkBox2.Text = GeneralClass.CfgData.TypeC14 ? "Φ14-棒材" : "Φ14-线材";
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            //ConfigData _data = new ConfigData();

            //修改工厂类型
            if (radioButton1.Checked)
            {
                GeneralClass.CfgData.Factorytype = EnumFactoryType.Standard;
            }
            if (radioButton2.Checked)
            {
                GeneralClass.CfgData.Factorytype = EnumFactoryType.Reduction;
            }
            if (radioButton3.Checked)
            {
                GeneralClass.CfgData.Factorytype = EnumFactoryType.Experiment;
            }

            //设置Φ12直径和Φ14直径钢筋归属于线材还是棒材
            GeneralClass.CfgData.TypeC12 = checkBox1.Checked;
            GeneralClass.CfgData.TypeC14 = checkBox2.Checked;

            //存为json
            string _json = NewtonJson.Serializer(GeneralClass.CfgData);
            Config.SaveConfig(_json);

            GeneralClass.interactivityData?.printlog(1, "系统配置已修改");


        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked) 
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
    }
}
