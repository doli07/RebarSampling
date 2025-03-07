using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebarSampling.GLD
{
    public class Gld_model
    {
        public List<CategoryItem> CategoryItems { get; set; }
        public List<ConstructionSectionItem> ConstructionSectionItems { get; set; }

        public List<InstanceItem> InstanceItems { get; set; }
        public List<InstanceTypeItem> InstanceTypeItems { get; set; }

    }

    public class CategoryItem
    {
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
    }

    public class ConstructionSectionItem
    {
        public string ConstructionSectionID        { get; set; }
        public string ConstructionSectionName { get;  set; }

    }

    public class InstanceItem
    {
        public string FloorID { get; set; }
        public string InstanceID { get; set; }
        public string InstanceName { get; set; }
        public string InstanceTypeID { get; set; }
    }

    public class InstanceTypeItem
    {
        public string CategoryID { get; set; }
        public string InstanceTypeID { get; set; }
        public string InstanceTypeName { get; set; }

    }

}
