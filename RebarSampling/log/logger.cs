using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    internal class logger
    {
        private string _path;

        private FileStream _stream;

        private StreamWriter _writer;

        private StreamReader _reader;

        public logger(string filepath) 
        { 
            _path = filepath;
            if(!File.Exists(filepath))
            {
                File.Create(filepath).Close();
            }
        }

        public bool Open()
        {
            if(File.Exists(this._path))
            {
                _stream = new FileStream(this._path,FileMode.Append);
                _writer = new StreamWriter(_stream);
                return true;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("文件路径:" + this._path + "不存在!");
                return false;
            }
        }

        public void Write(string _msg)
        {
            string recordTime = DateTime.Now.ToString("HH:mm:ss.fff");

            if(_writer!=null)
            {
                _writer.WriteLine(recordTime + "-->" + _msg);
            }
        }

        public void Close()
        {
            _writer.Flush();
            _writer.Close();
            _stream.Close();
        }
    }
}
