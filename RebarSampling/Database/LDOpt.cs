using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebarSampling.Database
{
    public class LDOpt
    {
        public  ILDHelper ldhelper;

        public LDOpt()
        {
        }

        public LDOpt(EnumMaterialBill _ldtype)
        {
            if (_ldtype == EnumMaterialBill.EJIN)
            {
                ldhelper = new EJINHelper();
            }
            if (_ldtype == EnumMaterialBill.GLD)
            {
                ldhelper = new GLDHelper();
            }
        }
    }
}
