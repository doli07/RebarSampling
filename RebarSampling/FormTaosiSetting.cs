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
    public partial class FormTaosiSetting : Form
    {
        public FormTaosiSetting(List<string> _allTaoType)//重载构造函数，传入套丝参数
        {
            InitializeComponent();

            if (_allTaoType.Count != 0)
            {
                foreach (var item in _allTaoType)
                {
                    comboBox1.Items.Add(item);
                    comboBox2.Items.Add(item);
                    comboBox3.Items.Add(item);
                    comboBox4.Items.Add(item);
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            string _setting = comboBox1.SelectedItem.ToString().Substring(1) + "-" +
                                         comboBox2.SelectedItem.ToString().Substring(1) + "-" +
                                         comboBox3.SelectedItem.ToString().Substring(1) + "-" +
                                         comboBox4.SelectedItem.ToString().Substring(1);//去掉起始的直径符号，只保留数值部分，以及反丝的”*“

            GeneralClass.interactivityData?.getTaosiSetting(_setting);//传递给form3

            this.DialogResult = DialogResult.OK;
            //this.Close();//关闭窗口
        }
    }
}
