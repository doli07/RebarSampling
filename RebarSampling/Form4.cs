using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RebarSampling
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();

            GeneralClass.interactivityData.servermsg += ShowServerMsg;
            GeneralClass.interactivityData.clientmsg += ShowClientMsg;

            GeneralClass.interactivityData.mqttpublishmsg += ShowPublisherMsg;
            GeneralClass.interactivityData.mqttsubscribmsg += ShowSubscriberMsg;
        }
        private void ShowPublisherMsg(string msg)
        {
            if (this != null)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    textBox13.Clear();
                    textBox13.Text = DateTime.Now.ToString() + ":" + msg;
                }
                    ));
            }
        }
        private void ShowSubscriberMsg(string msg)
        {
            if (this != null)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    textBox15.Clear();
                    textBox15.Text = DateTime.Now.ToString() + ":" + msg;
                }
                    ));
            }
        }
        private void ShowServerMsg(string msg)
        {
            if (this != null)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    textBox4.Clear();
                    textBox4.Text = DateTime.Now.ToString() + ":" + msg;
                }
                    ));
            }
        }

        private void ShowClientMsg(string msg)
        {
            if (this != null)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    textBox7.Clear();
                    textBox7.Text = DateTime.Now.ToString() + ":" + msg;
                }
                ));
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            List<SingleRebarData> _rebarlist = new List<SingleRebarData>();

            SingleRebarData _singleRebar = new SingleRebarData();
            _singleRebar.SeriNo = 0;
            _singleRebar.ProjectName = "光谷国际社区";
            _singleRebar.AssemblyName = "梁";
            _singleRebar.ElementName = "KL57";
            _singleRebar.WareNo = 1;
            _singleRebar.PicNo = "30202";
            _singleRebar.Level = "C";
            _singleRebar.Diameter = 22;
            _singleRebar.Length = 6000;
            _singleRebar.CornerMsg = "350,90;5300,90;350,0";
            _singleRebar.IndexCode = "1A5C3";
            _rebarlist.Add(_singleRebar);

            _singleRebar = new SingleRebarData();
            _singleRebar.SeriNo = 1;
            _singleRebar.ProjectName = "光谷国际社区";
            _singleRebar.AssemblyName = "梁";
            _singleRebar.ElementName = "KZ1";
            _singleRebar.WareNo = 2;
            _singleRebar.PicNo = "10000";
            _singleRebar.Level = "C";
            _singleRebar.Diameter = 22;
            _singleRebar.Length = 2500;
            _singleRebar.CornerMsg = "0,套;2500,0";
            _singleRebar.IndexCode = "1A5C4";
            _rebarlist.Add(_singleRebar);

            WorkBill _workbill = new WorkBill();
            _workbill.Msgtype = 2;
            _workbill.BillNo = "GJSQ_A_06D_01F_20230628_0001";
            _workbill.TotalNum = 100;
            _workbill.SteelbarNo = 65;
            _workbill.ProjectName = "光谷国际社区";
            _workbill.Block = "A";
            _workbill.Building = "06D";
            _workbill.Floor = "01F";
            _workbill.Level = "C";
            _workbill.Diameter = 22;
            _workbill.OriginalLength = 9;
            _workbill.SteelbarList = _rebarlist;

            string sss = GeneralClass.JsonOpt.Serializer(_workbill);

            textBox1.Text = sss;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            GeneralClass.webServer.Start(textBox2.Text, textBox3.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GeneralClass.webServer.Stop();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            GeneralClass.webClient.Connect(textBox5.Text);
            button4.Enabled = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            GeneralClass.webClient.Disconnect();
            button4.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            GeneralClass.webServer.SendMsg(textBox6.Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            GeneralClass.webClient.SendMsg(textBox8.Text);
        }

        private async void buttonServerStart_Click(object sender, EventArgs e)
        {
            buttonServerStart.BackColor = Color.LightGreen;
            buttonServerStart.Enabled = false;
            await GeneralClass.mqttServer.StartMqttServer(textBox9.Text);
            GeneralClass.interactivityData?.printlog(1, "mqttserver start");
        }

        private async void buttonServerStop_Click(object sender, EventArgs e)
        {
            buttonServerStart.BackColor = Color.Transparent;
            buttonServerStart.Enabled=true;
            await GeneralClass.mqttServer.StopMqttServer();
            GeneralClass.interactivityData?.printlog(1, "mqttserver stop");

        }

        private async void buttonPublisherStart_Click(object sender, EventArgs e)
        {
            buttonPublisherStart.BackColor = Color.LightGreen;
            buttonPublisherStart.Enabled=false;
            await GeneralClass.mqttClient.PublisherStart(textBox10.Text, textBox9.Text);
            GeneralClass.interactivityData?.printlog(1, $"mqttclient publisher start,server:{textBox10.Text}|port:{textBox9.Text}");

        }

        private async void buttonPublisherStop_Click(object sender, EventArgs e)
        {
            buttonPublisherStart.BackColor = Color.Transparent;
            buttonPublisherStart.Enabled=true;
            await GeneralClass.mqttClient.PublisherStop();
            GeneralClass.interactivityData?.printlog(1, "mqttclient publisher stop");

        }

        private async void buttonPublish_Click(object sender, EventArgs e)
        {
            await GeneralClass.mqttClient.Publish(textBox11.Text, textBox12.Text);
            GeneralClass.interactivityData?.printlog(1, $"mqttclient publish,topic:{textBox11.Text}|payload:{textBox12.Text}");

        }

        private async void buttonSubscriberStart_Click(object sender, EventArgs e)
        {
            buttonSubscriberStart.BackColor = Color.LightGreen;
            buttonSubscriberStart.Enabled=false;
            await GeneralClass.mqttClient.SubscriberStart(textBox10.Text, textBox9.Text);
            GeneralClass.interactivityData?.printlog(1, $"mqttclient subscriber start,server:{textBox10.Text}|port:{textBox9.Text}");

        }

        private async void buttonSubscrib_Click(object sender, EventArgs e)
        {
            await GeneralClass.mqttClient.Subscrib(textBox14.Text);
            GeneralClass.interactivityData?.printlog(1, $"mqttclient subscrib,topic:{textBox14.Text}");

        }

        private async void buttonSubscriberStop_Click(object sender, EventArgs e)
        {
            buttonSubscriberStart.BackColor = Color.Transparent;
            buttonSubscriberStart.Enabled = true;
            await GeneralClass.mqttClient.SubscriberStop();
            GeneralClass.interactivityData?.printlog(1, "mqttclient subscriber stop");

        }
    }
}
