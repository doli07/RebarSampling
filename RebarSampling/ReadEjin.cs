using Etable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace RebarSampling
{
    public class EjinReader
    {
        private string jsonstr = "";
        /// <summary>
        /// 根据文件路径，获得json格式的料单文件
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public string GetJsonStr(string filepath)
        {
            return CreateJsonString(E_Table.Read(filepath, "加工单"));
        }
        private string  CreateJsonString(E_Table e_Table)
        {
            BookHelperArray bookArray = new BookHelperArray();
            //测试数据
            bookArray.morder = new Morder()
            {
                cusbillno = "KH001",
                projectname = "工程名称",
                xiangmuname = "项目名称",
                cuslinker = "客户联系人",
                linktel = "13988888888",
                cusaddress = "发货客户地址",
                jiaohuodate = "2023-05-27: 00: 00: 00",
                weight = 567.9,
                remark = "备注",
                creator = 1,
                customername = "易精软件公司测试"
            };
            //label1.Text = "行数: 0";
            if (e_Table.lis != null && e_Table.lis.Count > 1)
            {
                List<string[]> eTable = e_Table.lis;
                string[] fhjs = eTable[0][12].Split(',');//A = HPB300,B = HRB335,C = HRB400,D = HRB500
                for (int i = 1; i < eTable.Count; i++)
                {
                    string[] arr = eTable[i];
                    //"GJMC"构件名称, "GJJT"钢筋简图, "BH"编号, "XH"序号, "JBZJ"级别直径, "XLCD"下料长度, "GSJS"根数件数, "ZGS"总根数, "ZL"重量, "BZ"备注, "TJSM"统计说明, "HSX"边角结构
                    if (arr[4].Length > 1)
                    {
                        bookArray.mOrderDtls.Add(new MOrderDtls()
                        {
                            //goujianplace = "构件位置",
                            goujianname = arr[0],
                            chartparam = arr[1],
                            cusno = arr[2],
                            orderindexno = (arr[3].Length > 0 ? int.Parse(arr[3]) : 0),
                            diaspec = ChangeGJFH(fhjs, arr[4]),
                            diameter = float.Parse(arr[4].Substring(1)),
                            cuslong = arr[5],
                            //根数件数= arr[6],
                            neednum = int.Parse(arr[7]),
                            cusliweight = double.Parse(arr[8]),
                            remark = arr[9],
                            makeparam = arr[11]
                        });
                    }
                }
                //label1.Text = "行数: " + (eTable.Count - 1);
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    bookArray.mOrderDtls.Add(new MOrderDtls()
                    {
                        diameter = 20,
                        cusliweight = 79.92,
                        cuslong = "1500",
                        cusno = "N1",
                        diaspec = "HPB300",
                        goujianname = "构件名称",
                        goujianplace = "构件位置",
                        neednum = 10,
                        orderindexno = i + 1,
                        remark = "无",
                        makeparam = "110,-135;6500,-135;110,0",
                        chartparam = "11 /P2 50 19 39 30 141 30 130 19/L110 37 15 0 2,6500 90 14 0 1,110 142 15 0 0/D0,-135,-135,0"
                    });
                }
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            js.MaxJsonLength = Int32.MaxValue;
            jsonstr = js.Serialize(bookArray);

            return jsonstr;
        }

        //钢筋符号转换
        private string ChangeGJFH(string[] fhjs, string str)
        {
            for (int i = 0; i < fhjs.Length; i++)
            {
                if (fhjs[i][0] == str[0])
                {
                    str = fhjs[i].Substring(2);
                    break;
                }
            }
            return str;
        }
    }
}
