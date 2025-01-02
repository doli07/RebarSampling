using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace RebarSampling
{
    /// <summary>
    /// 图表类，用于绘制统计饼图，柱状图等
    /// </summary>
    public class chartshow
    {
        /// <summary>
        /// 统计饼图
        /// </summary>
        public static void ChartPieShow(string _title, List<string> x, List<int> y, Chart _chart)
        {
            //标题
            _chart.Titles.Clear();
            _chart.Titles.Add(_title);
            _chart.Titles[0].Alignment = ContentAlignment.TopCenter;

            _chart.Series[0].ChartType = SeriesChartType.Pie; // 图类型
            //_chart.Series[0]["PieLabelStyle"] = "Outside"; // 将文字移到外侧
            //_chart.Series[0]["PieLineColor"] = "Black"; // 绘制黑色的连线
            _chart.Series[0].Label = "#PERCENT{P1}";
            _chart.Series[0].IsValueShownAsLabel = true;
            _chart.Series[0].Points.DataBindXY(x, y);

            _chart.Legends[0].Enabled = true;//右侧legend启用
            _chart.Legends[0].Alignment = StringAlignment.Center;
            _chart.Legends[0].Docking = Docking.Right;
            //_chart.Legends[0].Title = _title;

            for (int i = 0; i < _chart.Series[0].Points.Count; i++)
            {
                //_chart.Series[0].Points[p].Label = "#VALX\n#PERCENT{P0}\n";
                _chart.Series[0].Points[i].IsVisibleInLegend = true;
                _chart.Series[0].Points[i].LegendText = x[i]+" "+y[i].ToString() /*+ "   " + (Convert.ToDouble(y[i]) / Convert.ToDouble(y.Sum())).ToString("P1")*/;
            }
        }
        public static void ChartPieShow(string _title, List<string> x, List<double> y, Chart _chart)
        {
            //标题
            _chart.Titles.Clear();
            _chart.Titles.Add(_title);
            _chart.Titles[0].Alignment = ContentAlignment.TopCenter;

            _chart.Series[0].ChartType = SeriesChartType.Pie; // 图类型
            //_chart.Series[0]["PieLabelStyle"] = "Outside"; // 将文字移到外侧
            //_chart.Series[0]["PieLineColor"] = "Black"; // 绘制黑色的连线
            _chart.Series[0].Label = "#PERCENT{P1}";
            _chart.Series[0].IsValueShownAsLabel = true;
            _chart.Series[0].Points.DataBindXY(x, y);

            _chart.Legends[0].Enabled = true;//右侧legend启用
            _chart.Legends[0].Alignment = StringAlignment.Center;
            _chart.Legends[0].Docking = Docking.Right;
            //_chart.Legends[0].Title = _title;

            for (int i = 0; i < _chart.Series[0].Points.Count; i++)
            {
                //_chart.Series[0].Points[p].Label = "#VALX\n#PERCENT{P0}\n";
                _chart.Series[0].Points[i].IsVisibleInLegend = true;
                _chart.Series[0].Points[i].LegendText = x[i] +" "+y[i].ToString()/*+ "   " + (Convert.ToDouble(y[i]) / Convert.ToDouble(y.Sum())).ToString("P1")*/;
            }
        }
        /// <summary>
        /// 统计柱状图
        /// </summary>
        public static void ChartRectShow(string _title, List<string> x, List<int> y, Chart _chart)
        {
            //标题
            _chart.Titles.Clear();
            _chart.Titles.Add(_title);
            _chart.Titles[0].Alignment = ContentAlignment.TopCenter;
            //图形类型
            _chart.Series[0].ChartType = SeriesChartType.Column;
            //xy轴网格设置
            _chart.ChartAreas[0].AxisX.LabelStyle.Angle = 0;//90°旋转显示
            _chart.ChartAreas[0].AxisX.Interval = 1;
            _chart.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.NotSet; //设置网格类型为虚线
            _chart.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash; //设置网格类型为虚线
            //标签显示数据
            _chart.Series[0].IsValueShownAsLabel = true;//设置显示示数
            //_chart.Series[0].Label = "#PERCENT{P1}";//显示百分比

            //右侧legend
            _chart.Legends[0].Enabled = false;//不显示图例右侧legend
            //绑定数据
            _chart.Series["Series1"].Points.DataBindXY(x, y);
        }
        public static void ChartRectShow(string _title, List<string> x, List<double> y, Chart _chart)
        {
            //标题
            _chart.Titles.Clear();
            _chart.Titles.Add(_title);
            _chart.Titles[0].Alignment = ContentAlignment.TopCenter;
            //图形类型
            _chart.Series[0].ChartType = SeriesChartType.Column;
            //xy轴网格设置
            _chart.ChartAreas[0].AxisX.LabelStyle.Angle = 0;//90°旋转显示
            _chart.ChartAreas[0].AxisX.Interval = 1;
            _chart.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.NotSet; //设置网格类型为虚线
            _chart.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash; //设置网格类型为虚线
            //标签显示数据
            _chart.Series[0].IsValueShownAsLabel = true;//设置显示示数
            //_chart.Series[0].Label = "#PERCENT{P1}";//显示百分比
            //右侧legend
            _chart.Legends[0].Enabled = false;//不显示图例右侧legend
            //绑定数据
            _chart.Series["Series1"].Points.DataBindXY(x, y);
        }



        //chart1.Series[0]["PieLabelStyle"] = "Outside";//将文字移到外侧
        ////chart1.Series[0]["PieLabelStyle"] = "Enabled";//将文字移到外侧

        //chart1.Series[0]["PieLineColor"] = "Black";//绘制黑色的连线。
        //chart1.Series[0].Points.DataBindXY(xData, yData);

        //chart1.Legends[0].Enabled = true;//右侧legend启用
        //chart1.Legends[0].Alignment = StringAlignment.Center;
        //chart1.Legends[0].Docking = Docking.Right;
        //chart1.Legends[0].Title = GeneralClass.sDetailTableItemName[(int)_item];

        //for (int p = 0; p < chart1.Series[0].Points.Count; p++)
        //{
        //    //chart1.Series[0].Points[p].Label = "#VALX\n#PERCENT{P0}\n";
        //    chart1.Series[0].Points[p].IsVisibleInLegend = true;
        //    chart1.Series[0].Points[p].LegendText = xData[p] + " " + (Convert.ToDouble(yData[p]) / Convert.ToDouble(ydataTotal)).ToString("P1");
        //}
        ////Chart1.Series["Series1"].IsXValueIndexed = false;
        ////Chart1.Series["Series1"].IsValueShownAsLabel = false;
        ////Chart1.Series["Series1"]["PieLineColor"] = "Black";//连线颜色
        ////Chart1.Series["Series1"]["PieLabelStyle"] = "Outside";//标签位置
        ////Chart1.Series["Series1"].ToolTip = "#VALX";//显示提示用语

    }
}
