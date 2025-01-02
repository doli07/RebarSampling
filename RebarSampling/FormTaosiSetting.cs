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
        public FormTaosiSetting(string _old, List<string> _newTaoSet)//重载构造函数，传入套丝参数
        {
            InitializeComponent();

            try
            {
                if (_old == "")
                {
                    label5.Text = "xx";
                    label6.Text = "xx";
                    label7.Text = "xx";
                    label8.Text = "xx";
                }
                else
                {
                    label5.Text = _old.Split('-')[0];
                    label6.Text = _old.Split('-')[1];
                    label7.Text = _old.Split('-')[2];
                    label8.Text = _old.Split('-')[3];
                }

                if (_newTaoSet.Count != 0)
                {
                    foreach (var item in _newTaoSet)
                    {
                        comboBox1.Items.Add(item);
                        comboBox2.Items.Add(item);
                        comboBox3.Items.Add(item);
                        comboBox4.Items.Add(item);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("FormTaosiSetting error:" + ex.Message); }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string _setting = comboBox1.SelectedItem.ToString().Substring(1) + "-" +
                                             comboBox2.SelectedItem.ToString().Substring(1) + "-" +
                                             comboBox3.SelectedItem.ToString().Substring(1) + "-" +
                                             comboBox4.SelectedItem.ToString().Substring(1);//去掉起始的直径符号，只保留数值部分，以及反丝的”*“

                GeneralClass.interactivityData?.getTaosiSetting(_setting);//传递给form3

                this.DialogResult = DialogResult.OK;
                //this.Close();//关闭窗口
            }
            catch (Exception ex) { MessageBox.Show("button1_Click error:" + ex.Message); }

        }
    }
}
