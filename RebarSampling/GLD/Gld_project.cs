using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebarSampling.GLD
{
    /// <summary>
    /// 广联达料单的project名字的json
    /// </summary>
    public class Gld_project
    {
        public List<BuildingItem> BuildingItems { get; set; }
        public string CalculationRule { get; set; }
        public string ConstructionOrganization { get; set; }
        public string CreatedDate { get; set; }
        public string DesignOrganization { get; set; }
        public string DevelopmentOrganization { get; set; }
        public string EarthquakeResistance { get; set; }
        public List<FloorItem> FloorItems { get; set; }
        public string OvergroundFloorCount { get; set; }
        public string ProjectAddress { get; set; }
        public string ProjectArea { get; set; }
        public string ProjectCity { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectDate { get; set; }
        public string ProjectHeight { get; set; }
        public string ProjectName { get; set; }
        public string ProjectProvince { get; set; }
        public string ProjectType { get; set; }
        public string StructureType { get; set; }
        public string SupervisoryOrganization { get; set; }
        public string UID { get; set; }
        public string UndergroundFloorCount { get; set; }
        public string Version { get; set; }
    }

    public class BuildingItem
    {
        public string BuildingID { get; set; }
        public string BuildingName { get; set; }
    }

    public class FloorItem
    {
        public string BuildingID { get; set; }
        public double FloorArea { get; set; }
        public double FloorElevation { get; set; }
        public double FloorHeight { get; set; }
        public string FloorID { get; set; }
        public string FloorName { get; set; }
        public string Remark { get; set; }
        public int StandardFloorCount { get; set; }


    }
}
