using Etable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace RebarSampling
{
    public class EjinReader
    {
        private string jsonstr = "";
        /// <summary>
        /// 根据文件路径，获得json格式的料单文件，通过文件路径获取morder，传入json料单文件
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public string GetJsonStr(string filepath, Morder _morder)
        {
            return CreateJsonString(E_Table.Read(filepath, "加工单"), _morder);
        }
        private string CreateJsonString(E_Table e_Table, Morder _morder)
        {
            jsonstr = "";

            try
            {
                BookHelperArray bookArray = new BookHelperArray();

                bookArray.morder = new Morder()
                {
                    levelName = (string[])_morder.levelName.Clone()
                };

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

                    JavaScriptSerializer js = new JavaScriptSerializer();
                    js.MaxJsonLength = Int32.MaxValue;
                    jsonstr = js.Serialize(bookArray);

                }

                return jsonstr;
            }
            catch (Exception ex)
            {
                MessageBox.Show("CreateJsonString error"+ex.Message);
                return jsonstr;
            }
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
