using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace RebarSampling
{
    public class http
    {

        private JavaScriptSerializer js = new JavaScriptSerializer();

        public string HttpGet(string Url, string postDataStr)
        {
        BeginHttpGet:
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            //SaveRecord("打开链接：" + Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            //request.ContentType = "text/json;charset=UTF-8";
            string retString = null;

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
                DialogResult _rt = MessageBox.Show("后台服务器:" + Url + (postDataStr == "" ? "" : "?") + postDataStr + "连接失败,是否重新连接？", "警告", MessageBoxButtons.RetryCancel);
                if (_rt == DialogResult.Retry)
                {
                    goto BeginHttpGet;
                }
                else
                {
                    response = (HttpWebResponse)ex.Response;
                    return retString;
                }

            }

            if (retString == null)
            {
                DialogResult _rt = MessageBox.Show("后台服务器:" + Url + (postDataStr == "" ? "" : "?") + postDataStr + "连接失败,是否重新连接？", "警告", MessageBoxButtons.RetryCancel);
                if (_rt == DialogResult.Retry)
                {
                    goto BeginHttpGet;
                }
            }

            return retString;

        }
        public string HttpPost(string Url, string postDataStr)
        {
        BeginHttpPost:
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentType = "application/json;charset=UTF-8";

            byte[] byteReq = Encoding.UTF8.GetBytes(postDataStr);

            //request.ContentLength = postDataStr.Length;
            request.ContentLength = byteReq.Length;
            string retString = null;

            try
            {
                ////StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
                //StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.UTF8);
                //writer.Write(postDataStr);
                //writer.Flush();

                Stream stream;
                stream = request.GetRequestStream();
                stream.Write(byteReq, 0, byteReq.Length);
                stream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string encoding = response.ContentEncoding;
                if (encoding == null || encoding.Length < 1)
                {
                    encoding = "UTF-8"; //默认编码                  
                }
                //StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);

                retString = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                DialogResult _rt = MessageBox.Show("后台服务器:" + Url + "连接失败,是否重新连接？", "警告", MessageBoxButtons.RetryCancel);
                if (_rt == DialogResult.Retry)
                {
                    goto BeginHttpPost;
                }
                else
                {
                    return retString;
                }
            }

            if (retString == null)
            {
                DialogResult _rt = MessageBox.Show("后台服务器:" + Url + "连接失败,是否重新连接？", "警告", MessageBoxButtons.RetryCancel);
                if (_rt == DialogResult.Retry)
                {
                    goto BeginHttpPost;
                }
            }

            return retString;

        }

    }
}
