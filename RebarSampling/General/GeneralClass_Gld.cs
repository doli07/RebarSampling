using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebarSampling
{
    public static partial class GeneralClass
    {
        /// <summary>
        /// 钢筋表的列名
        /// </summary>
        public static string[] sRebarColumnName_Gld = new string[20]
        {
            "BarDataExpression",
            "BarIndex",
           "BarShapeTypeID",
           "BarSketchFile",
           "BarType",
            "BreakLength",
            "ConstructionSectionID",
            "Count",//int
            "Diameter",
            "FabricationType",
            "Formula",
            "`Index`",//int，注意index属于数据库保留字段，前后各加一个反引号``
            "InstanceID",
            "InstanceIndex",
            "Length",//int
            "LevelName",
            "Remark",
            "Space",//double
            "UsedType",
            "Weight"//double
        };

        public static string[] s_pBuildingColumnName_Gld = new string[]
        {
            "BuildingID",
            "BuildingName"
        };

        public static string[] s_pFloorColumnName_Gld = new string[]
        {
            "BuildingID",
            "FloorArea",
            "FloorElevation",
            "FloorHeight",
            "FloorID",
            "FloorName",
            "Remark",
            "StandardFloorCount"
        };

        public static string[] sProjectColumnName_Gld = new string[]
        {
            "CalculationRule",
            "ConstructionOrganization",
            "CreatedDate",
            "DesignOrganization",
            "DevelopmentOrganization",
            "EarthquakeResistance",
            "OvergroundFloorCount",
            "ProjectAddress",
            "ProjectArea",
            "ProjectCity",
            "ProjectCode",
            "ProjectDate",
            "ProjectHeight",
            "ProjectName",
            "ProjectProvince",
            "ProjectType",
            "StructureType",
            "SupervisoryOrganization",
            "UID",
            "UndergroundFloorCount",
            "Version"
        };

        public static string[] s_mCategoryColumnName_Gld = new string[]
        {
            "CategoryID",
            "CategoryName"
        };

        public static string[] s_mConstructionSectionColumnName_Gld = new string[]
        {
                "ConstructionSectionID",
                "ConstructionSectionName"
        };

        public static string[] s_mInstanceColumnName_Gld = new string[]
            {
                "FloorID",
                "InstanceID",
                "InstanceName",
                "InstanceTypeID"
            };

        public static string[] s_mInstanceTypeColumnName_Gld = new string[]
        {
            "CategoryID",
            "InstanceTypeID",
            "InstanceTypeName"
        };
    }
}
