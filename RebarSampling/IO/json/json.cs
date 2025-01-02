using Newtonsoft.Json;
using NPOI.OpenXml4Net.OPC;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;


namespace RebarSampling
{
    public class json
    {
        private JavaScriptSerializer serializer = new JavaScriptSerializer();

        public string Serializer(dynamic _data)
        {
            try
            {
                return serializer.Serialize(_data);
            }
            catch (Exception ex) { MessageBox.Show("Serializer error:" + ex.Message); return ""; }

        }

        public dynamic Deserializer(string _json)
        {
            try
            {
                return serializer.Deserialize<dynamic>(_json);
            }
            catch (Exception ex) { MessageBox.Show("Serializer error:" + ex.Message); return null; }

        }
    }

    public class NewtonJson
    {
        /// <summary>
        /// 以标准json格式(包含缩进换行)，序列化生成json数据string
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        public static string Serializer(object _data)
        {
            try
            {
                return JsonConvert.SerializeObject(_data, Formatting.Indented);
            }
            catch (Exception ex) { MessageBox.Show("Serializer error:" + ex.Message); return ""; }

        }
        /// <summary>
        /// 反序列化json
        /// </summary>
        /// <param name="_json"></param>
        /// <returns></returns>
        public static T Deserializer<T>(string _json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(_json);
            }
            catch (Exception ex) { MessageBox.Show("Serializer error:" + ex.Message); return default(T); }

        }




    }
}
