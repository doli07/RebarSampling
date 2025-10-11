using Newtonsoft.Json;
using NPOI.OpenXml4Net.OPC;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.IO;
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

        /// <summary>
        /// 将 JSON 字符串保存到指定文件
        /// </summary>
        /// <param name="jsonString">JSON 字符串</param>
        /// <param name="filePath">文件路径</param>
        public static void SaveJsonToFile(string jsonString, string filePath)
        {
            try
            {
                // 获取文件目录
                string directory = Path.GetDirectoryName(filePath);

                // 如果目录不存在，则创建目录
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // 写入文件（使用 UTF-8 编码，确保中文等字符能正确保存）
                File.WriteAllText(filePath, jsonString, System.Text.Encoding.UTF8);
            }
            catch (Exception ex) { MessageBox.Show("SaveJsonToFile error:" + ex.Message);  }
        }

        /// <summary>
        /// 从文件读取 JSON 并反序列化为对象
        /// </summary>
        public static string ReadJsonFromFile(string filePath)
        {
            try
            {
                // 检查文件是否存在
                if (!File.Exists(filePath))
                {
                    //throw new FileNotFoundException($"指定的文件不存在: {filePath}");
                    MessageBox.Show("指定路径的文件不存在：" + filePath);
                }

                // 读取文件内容
                string jsonString = File.ReadAllText(filePath, System.Text.Encoding.UTF8);

                return jsonString;
            }
            catch (Exception ex) { MessageBox.Show("ReadJsonFromFile error:" + ex.Message); return string.Empty; }
        }


    }
}
