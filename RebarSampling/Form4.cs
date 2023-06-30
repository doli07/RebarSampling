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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<SingleRebarData> _rebarlist = new List<SingleRebarData>();

            SingleRebarData _singleRebar = new SingleRebarData();
            _singleRebar.projectName = "光谷国际社区";
            _singleRebar.assemblyName = "梁";
            _singleRebar.elementName = "KL57";
            _singleRebar.picNo = "30202";
            _singleRebar.level = "C";
            _singleRebar.diameter = 22;
            _singleRebar.length = 6000;
            _singleRebar.cornerMsg = "350,90;5300,90;350,0";
            _singleRebar.ID = 1;
            _rebarlist.Add(_singleRebar);

            _singleRebar = new SingleRebarData();
            _singleRebar.projectName = "光谷国际社区";
            _singleRebar.assemblyName = "梁";
            _singleRebar.elementName = "KZ1";
            _singleRebar.picNo = "10000";
            _singleRebar.level = "C";
            _singleRebar.diameter = 22;
            _singleRebar.length = 2500;
            _singleRebar.cornerMsg = "0,套;2500,0";
            _singleRebar.ID = 2;
            _rebarlist.Add(_singleRebar);

            GeneralWorkBill _workbill = new GeneralWorkBill();
            _workbill.BillNo = "GJSQ_A_06D_01F_202306280001";
            _workbill.ProjectName = "光谷国际社区";
            _workbill.Block = "A";
            _workbill.Building = "06D";
            _workbill.Floor = "01F";
            _workbill.Level = "C";
            _workbill.Diameter = 22;
            _workbill.OriginalLength = 9;
            _workbill.RebarList = _rebarlist;

            string sss = GeneralClass.JsonOpt.Serializer(_workbill);

            textBox1.Text = sss;
        }
    }
}
