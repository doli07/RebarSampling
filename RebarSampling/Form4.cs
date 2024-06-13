//using Etable;
using SuperSocket.SocketEngine.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Threading;
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

            GeneralClass.interactivityData.sendworkbill += SendWorkBill;

            InitControl();
            //InitDGV();

            //textBox2.Text = GeneralClass.CfgData.webserverIP;
            //textBox3.Text = GeneralClass.CfgData.webserverPort;

        }

        ~Form4()
        {
            this.dispose();
            workflow.Instance.StopWorkBill();
        }
        private async void dispose()
        {
            GeneralClass.webServer.Stop();
            GeneralClass.webClient.Disconnect();

            await GeneralClass.mqttServer.StopMqttServer();
            await GeneralClass.mqttClient.PublisherStop();
            await GeneralClass.mqttClient.SubscriberStop();
        }

        private void InitControl()
        {
            this.comboBox1.Items.Clear();
            this.comboBox1.Items.Add("Φ16");
            this.comboBox1.Items.Add("Φ18");
            this.comboBox1.Items.Add("Φ20");
            this.comboBox1.Items.Add("Φ22");
            this.comboBox1.Items.Add("Φ25");
            this.comboBox1.Items.Add("Φ28");
            this.comboBox1.Items.Add("Φ32");
            this.comboBox1.Items.Add("Φ36");
            this.comboBox1.SelectedIndex = 0;

            this.comboBox2.Items.Clear();
            this.comboBox2.Items.Add("1");
            this.comboBox2.Items.Add("2");
            this.comboBox2.Items.Add("3");
            this.comboBox2.Items.Add("4");
            this.comboBox2.Items.Add("5");
            this.comboBox2.Items.Add("6");
            this.comboBox2.SelectedIndex = 0;

            this.comboBox3.Items.Clear();
            this.comboBox3.Items.Add("1");
            this.comboBox3.Items.Add("2");
            this.comboBox3.Items.Add("3");
            this.comboBox3.Items.Add("4");
            this.comboBox3.Items.Add("5");
            this.comboBox3.Items.Add("6");
            this.comboBox3.SelectedIndex = 0;

            this.comboBox4.Items.Clear();
            this.comboBox4.Items.Add("1");
            this.comboBox4.Items.Add("2");
            this.comboBox4.Items.Add("3");
            this.comboBox4.Items.Add("4");
            this.comboBox4.Items.Add("5");
            this.comboBox4.Items.Add("6");
            this.comboBox4.SelectedIndex = 0;

            this.textBox16.Text = "12000";
            this.textBox18.Text = "0";
            this.textBox19.Text = "0";
            this.textBox20.Text = "0";

            this.checkBox1.Checked = false;
            this.checkBox2.Checked = false;
            this.checkBox3.Checked = false;
            this.checkBox4.Checked = false;
            this.checkBox5.Checked = false;
            this.checkBox6.Checked = false;
            this.checkBox7.Checked = false;
            this.checkBox8.Checked = false;
            this.checkBox9.Checked = false;


        }

        private DataTable m_table;
        private void InitDGV()
        {
            m_table = new DataTable();

            DataColumn column = new DataColumn();
            column.ColumnName = "序号";
            column.AutoIncrement = true;
            column.AutoIncrementSeed = 0;
            column.AutoIncrementStep = 1;
            m_table.Columns.Add(column);

            column = new DataColumn("直径", typeof(int));
            m_table.Columns.Add(column);

            column = new DataColumn("json", typeof(string));
            m_table.Columns.Add(column);

            dataGridView1.DataSource = m_table;

            //DataGridViewColumn column;
            //DataGridViewCell cell;

            ////init datagridview1
            //column = new DataGridViewColumn();
            //cell = new DataGridViewTextBoxCell();
            //column.CellTemplate = cell;//设置单元格模板
            //column.HeaderText = "序号";//
            //dataGridView1.Columns.Add(column);

            //column = new DataGridViewColumn();
            //cell = new DataGridViewTextBoxCell();
            //column.CellTemplate = cell;//设置单元格模板
            //column.HeaderText = "直径";//
            //dataGridView1.Columns.Add(column);

            //column = new DataGridViewColumn();
            //cell = new DataGridViewTextBoxCell();
            //column.CellTemplate = cell;//设置单元格模板
            //column.HeaderText = "json";//
            //dataGridView1.Columns.Add(column);
        }
        private void ShowPublisherMsg(string msg)
        {
            if (this != null)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    //textBox13.Clear();
                    //textBox13.Text += DateTime.Now.ToString() + ":" + msg+"\r\n";
                    textBox13.Text = textBox13.Text.Insert(0, DateTime.Now.ToString() + ":" + msg + "\r\n");

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
                    //textBox15.Clear();
                    //textBox15.Text += DateTime.Now.ToString() + ":" + msg + "\r\n";
                    textBox15.Text = textBox15.Text.Insert(0, DateTime.Now.ToString() + ":" + msg + "\r\n");

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
                    //textBox4.Clear();
                    //textBox4.Text += DateTime.Now.ToString() + ":" + msg + "\r\n";
                    textBox4.Text = textBox4.Text.Insert(0, DateTime.Now.ToString() + ":" + msg + "\r\n");

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
                    //textBox7.Clear();
                    //textBox7.Text += DateTime.Now.ToString() + ":" + msg + "\r\n";
                    textBox7.Text = textBox7.Text.Insert(0, DateTime.Now.ToString() + ":" + msg + "\r\n");

                }
                ));
            }
        }
        private Random _random = new Random();//随机数生成器

        /// <summary>
        /// 虚拟造一个单根钢筋数据
        /// </summary>
        /// <param name="_index"></param>
        /// <returns></returns>
        private WorkBill_SingleRebar CreateSingleRebarData(int _index)
        {
            string _length = "";
            string _wareno = "";
            int _diameter = 0;
            bool _headtao = false;
            bool _tailtao = false;
            bool _bend = false;


            switch (_index)
            {
                case 0:
                    _length = textBox18.Text;
                    _wareno = (string)comboBox2.SelectedItem;
                    _diameter = Convert.ToInt32(((string)comboBox1.SelectedItem).Substring(1, 2));
                    _headtao = checkBox1.Checked;
                    _tailtao = checkBox2.Checked;
                    _bend = checkBox3.Checked;
                    break;
                case 1:
                    _length = textBox19.Text;
                    _wareno = (string)comboBox3.SelectedItem;
                    _diameter = Convert.ToInt32(((string)comboBox1.SelectedItem).Substring(1, 2));
                    _headtao = checkBox4.Checked;
                    _tailtao = checkBox5.Checked;
                    _bend = checkBox6.Checked;
                    break;
                case 2:
                    _length = textBox20.Text;
                    _wareno = (string)comboBox4.SelectedItem;
                    _diameter = Convert.ToInt32(((string)comboBox1.SelectedItem).Substring(1, 2));
                    _headtao = checkBox7.Checked;
                    _tailtao = checkBox8.Checked;
                    _bend = checkBox9.Checked;
                    break;
            }

            WorkBill_SingleRebar _singleRebar = new WorkBill_SingleRebar();
            GeneralMultiData _mData1 = new GeneralMultiData();
            GeneralMultiData _mData2 = new GeneralMultiData();

            if (_length != "0")
            {
                _singleRebar = new WorkBill_SingleRebar();
                _singleRebar.SeriNo = _index.ToString();
                _singleRebar.ProjectName = "光谷国际社区";
                _singleRebar.AssemblyName = "梁";
                _singleRebar.ElementName = "KL57";
                _singleRebar.WareInfo = _wareno;
                _singleRebar.PicNo = "30202";
                _singleRebar.Level = "C";
                _singleRebar.Diameter = _diameter;
                _singleRebar.Length = Convert.ToInt32(_length);
                //根据经验公式计算重量(kg)，保留3位小数
                _singleRebar.Weight = Math.Round(0.00617 * (double)_diameter * (double)_diameter * (double)_singleRebar.Length / 1000, 3);

                if (_bend)//弯曲
                {
                    _mData1 = new GeneralMultiData();
                    _mData2 = new GeneralMultiData();

                    _mData1.length = (_singleRebar.Length / 4).ToString();
                    _mData2.length = (_singleRebar.Length * 3 / 4).ToString();

                    _mData1.cornerMsg = _mData1.length + ",90;";
                    if (_tailtao)//端尾套丝
                    {
                        _mData2.cornerMsg = _mData2.length + ",丝;";
                    }
                    else//端尾不套丝
                    {
                        _mData2.cornerMsg = _mData2.length + ",0;";
                    }
                    if (_headtao)//端头套丝
                    {
                        _mData1.cornerMsg = "0,套;" + _mData1.cornerMsg;
                    }
                    _singleRebar.CornerMsg = _mData1.cornerMsg + _mData2.cornerMsg;
                }
                else
                {
                    _mData1 = new GeneralMultiData();
                    _mData1.length = _singleRebar.Length.ToString();

                    if (_tailtao)//端尾套丝
                    {
                        _mData1.cornerMsg = _mData1.length + ",丝;";
                    }
                    else//端尾不套丝
                    {
                        _mData1.cornerMsg = _mData1.length + ",0;";
                    }
                    if (_headtao)//端头套丝
                    {
                        _mData1.cornerMsg = "0,套;" + _mData1.cornerMsg;
                    }
                    _singleRebar.CornerMsg = _mData1.cornerMsg;

                }

                //_singleRebar.CornerMsg = "350,90;5300,90;350,0";
                //_singleRebar.IndexCode = "1A5C3";
                _singleRebar.IndexCode = Convert.ToString(_random.Next(100000, 1000000), 16).ToUpper().PadLeft(5, '0');

                //             int a = 123456;
                //             Convert.ToString(a, 16).ToUpper().PadLeft(8, '0') = 0001E240
                ////Convert.ToString(a, 16)十进制转为十六进制;
                ////string.ToUpper()返回大写的格式; 
                ////String.PadLeft(8,'0'); 表示检查字符串长度是否少于8位,若少于8位,则自动在其左侧以'0'补足。 

                return _singleRebar;

            }
            else
            {
                return null;
            }



        }

        private int SeriNo = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            WorkBill_LB _workbill = new WorkBill_LB();

            List<WorkBill_SingleRebar> _rebarlist = new List<WorkBill_SingleRebar>();

            WorkBill_SingleRebar _singleRebar;

            for (int i = 0; i < 3; i++)
            {
                _singleRebar = CreateSingleRebarData(i);
                if (_singleRebar != null)
                {
                    _rebarlist.Add(_singleRebar);
                }
            }


            _workbill.Msgtype = 2;
            string recordDate = DateTime.Now.ToString("yyyy_MM_dd");
            this.SeriNo++;
            _workbill.BillNo = "GJSQ_A_06D_01F_" + recordDate + "_" + this.SeriNo.ToString().PadLeft(4, '0');//四位流水号，用0补全
            //_workbill.TotalNum = 100;
            _workbill.SteelbarNo = this.SeriNo.ToString();
            _workbill.ProjectName = "光谷国际社区";
            _workbill.Block = "A";
            _workbill.Building = "06D";
            _workbill.Floor = "01F";
            _workbill.Level = "C";
            _workbill.Diameter = Convert.ToInt32(((string)comboBox1.SelectedItem).Substring(1, 2)); ;
            _workbill.OriginalLength = Convert.ToInt32(textBox16.Text);
            _workbill.SteelbarList = _rebarlist;

            string sss = NewtonJson.Serializer(_workbill);//json序列化

            textBox1.Text = sss;



            ////dataGridView1.Rows.Clear();//清空
            //DataGridViewRow dgvRow;
            //DataGridViewCell dgvCell;

            //dgvRow = new DataGridViewRow();

            ////直径
            //dgvCell = new DataGridViewTextBoxCell();
            //dgvCell.Value = "Φ"+ _workbill.Diameter;
            //dgvRow.Cells.Add(dgvCell);

            ////直径
            //dgvCell = new DataGridViewTextBoxCell();
            //dgvCell.Value = sss;
            //dgvRow.Cells.Add(dgvCell);

            ////Φ16
            //dataGridView1.Rows.Add(dgvRow);

            DataRow _row = m_table.NewRow();
            _row[1] = _workbill.Diameter;
            _row[2] = sss;
            m_table.Rows.Add(_row);
            dataGridView1.DataSource = m_table;

        }

        private void button15_Click(object sender, EventArgs e)
        {
            GeneralClass.CfgData.webserverIP = textBox2.Text;
            GeneralClass.CfgData.webserverPort = textBox3.Text;

            //存为json
            string _json = NewtonJson.Serializer(GeneralClass.CfgData);
            Config.SaveConfig(_json);

            GeneralClass.interactivityData?.printlog(1, "系统配置已修改");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //先加载ip和port
            textBox2.Text = GeneralClass.CfgData.webserverIP;
            textBox3.Text = GeneralClass.CfgData.webserverPort;

            button2.BackColor = Color.LightGreen;
            button2.Enabled = false;

            GeneralClass.webServer.Start(textBox2.Text, textBox3.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button2.BackColor = Color.Transparent;
            button2.Enabled = true;

            GeneralClass.webServer.Stop();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button4.BackColor = Color.LightGreen;
            button4.Enabled = false;

            GeneralClass.webClient.Connect(textBox5.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button4.BackColor = Color.Transparent;
            button4.Enabled = true;

            GeneralClass.webClient.Disconnect();
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
            buttonServerStart.Enabled = true;
            await GeneralClass.mqttServer.StopMqttServer();
            GeneralClass.interactivityData?.printlog(1, "mqttserver stop");

        }

        private async void buttonPublisherStart_Click(object sender, EventArgs e)
        {
            buttonPublisherStart.BackColor = Color.LightGreen;
            buttonPublisherStart.Enabled = false;
            await GeneralClass.mqttClient.PublisherStart(textBox10.Text, textBox9.Text);
            GeneralClass.interactivityData?.printlog(1, $"mqttclient publisher start,server:{textBox10.Text}|port:{textBox9.Text}");

        }

        private async void buttonPublisherStop_Click(object sender, EventArgs e)
        {
            buttonPublisherStart.BackColor = Color.Transparent;
            buttonPublisherStart.Enabled = true;
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
            buttonSubscriberStart.Enabled = false;
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

        private void button8_Click(object sender, EventArgs e)
        {
            InitControl();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //m_table = new DataTable();
            //dataGridView1.DataSource = m_table;
            InitDGV();

        }

        private void button10_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();

            if (m_table.Rows.Count != 0)
            {
                foreach (DataRow row in m_table.Rows)
                {
                    list.Add(row[2].ToString());
                }
                int _timestep = Convert.ToInt32(textBox17.Text);
                GeneralClass.interactivityData?.sendworkbill(list, _timestep);

            }
        }


        /// <summary>
        /// 发送工单数据
        /// </summary>
        /// <param name="_jsonlist">工单json</param>
        /// <param name="timestep">间隔时间，ms</param>
        private void SendWorkBill(List<string> _jsonlist, int timestep)
        {


            workflow.Instance.SendWorkBill(_jsonlist,timestep);


            //Thread sendthread = new Thread(() =>
            //{
            //    bool _flag = true;
            //    int _step = 0;

            //    while (_flag)
            //    {

            //        switch (_step)
            //        {
            //            case 0://先开启webserver
            //                {
            //                    if(GeneralClass.webServer.Start(GeneralClass.CfgData.webserverIP, GeneralClass.CfgData.webserverPort)==0)
            //                    {
            //                        _step++;
            //                    }                                
            //                }
            //                break;
            //            case 1://等待工单发送信号
            //                {
            //                    if(_sendflag)
            //                    {
            //                        _step++;
            //                    }                                
            //                }
            //                break;
            //            case 2://发送json工单
            //                {
            //                    foreach (string item in _jsonlist)
            //                    {
            //                        GeneralClass.webServer.SendMsg(item);
            //                        Thread.Sleep(timestep);
            //                    }
            //                    _step = 1;//回到step1，继续等待发送信号
            //                    _sendflag = false;//flag复位
            //                }
            //                break;
            //        }
            //        Thread.Sleep(1);
            //    }

            //}
            //    );
            //sendthread.IsBackground = true;
            //sendthread.Start();

        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (GeneralClass.jsonList.Count != 0)
            {
                int _timestep = Convert.ToInt32(textBox17.Text);
                //GeneralClass.interactivityData?.sendworkbill(GeneralClass.jsonList, _timestep);

                SendWorkBill(GeneralClass.jsonList, _timestep);

                //Thread thread = new Thread(() =>
                //{
                //    foreach (string item in GeneralClass.jsonList)
                //    {
                //        GeneralClass.webServer.SendMsg(item);
                //        Thread.Sleep(_timestep);
                //    }
                //});
                //thread.IsBackground = true;
                //thread.Start();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            InitDGV();

            foreach (var item in GeneralClass.jsonList)
            {
                DataRow _row = m_table.NewRow();
                //_row[1] = _workbill.Diameter;
                _row[2] = item;
                m_table.Rows.Add(_row);
            }

            dataGridView1.DataSource = m_table;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                string sss = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();

                //var jsonObj=JsonConvert.DeserializeObject(sss);
                //textBox1.Text = JsonConvert.SerializeObject(jsonObj,Formatting.Indented);

                var jsonobj = NewtonJson.Deserializer<WorkBill_LB>(sss);// 将JSON字符串转换为对象
                textBox1.Text = NewtonJson.Serializer(jsonobj);// 在textBox.Text中显示格式化的JSON内容
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            ElementRebarFB _fb = new ElementRebarFB();

            pictureBox1.Image = graphics.PaintElementLabel(_fb);

        }

        private void button14_Click(object sender, EventArgs e)
        {
            ElementRebarFB _fb = new ElementRebarFB();

            Image img = graphics.PaintElementLabel(_fb);

            img.Save(@"D:\\test.bmp",System.Drawing.Imaging.ImageFormat.Bmp);
            
            LabelPrint.print(img);

        }


    }
}
