using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebarSampling.Database
{
    /// <summary>
    /// 料单解析类
    /// </summary>
    public interface ILDHelper
    {

        List<RebarData> SplitMultiRebar(RebarData _data);

        List<RebarData> SplitSuoChiRebar(RebarData _data);

        List<GeneralMultiData> GetMultiData(string _cornerMsg, int _diameter = 1);

        //List<GeneralMultiData> GetMultiData(string _cornerMsg, int _diameter = 1);
    }
}
