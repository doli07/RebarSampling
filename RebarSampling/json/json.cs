using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;


namespace RebarSampling
{
    public class json
    {
        private JavaScriptSerializer serializer = new JavaScriptSerializer();

        public string Serializer(dynamic _data)
        {
            return serializer.Serialize(_data);
        }

        public dynamic Deserializer(string _json)
        {
            return serializer.Deserialize<dynamic>(_json);
        }
    }
}
