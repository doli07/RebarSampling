using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebarSampling.GLD
{
    /// <summary>
    /// 广联达原始料单是一个松散的数据结构，将原始料单转化为树形数据结构
    /// </summary>
    public class GldStructure
    {
        public GldStructure() {
            this.ProjectName = "";
            this._Buildings=new List<Buildings> ();
        }
        public string ProjectName {  get; set; }

        public List<Buildings> _Buildings { get; set; }

    }
    /// <summary>
    /// 楼栋
    /// </summary>
    public class Buildings
    {
        public Buildings() {
            this.BuildingID = "";
            this.BuildingName = "";
            this._Floors = new List<Floors>();
        }
        public string BuildingID {  get; set; }
        public string BuildingName { get; set; }
        public List<Floors> _Floors { get; set; }
    }
    /// <summary>
    /// 楼层
    /// </summary>
    public class Floors
    {
        public Floors() {
            this.FloorID = "";
            this.FloorName = "";
            this._Instances = new List<Instances>();
        }
        public string FloorID { get;set; }    
        public string FloorName { get;set; }

        public List<Instances> _Instances { get; set; }

    }
    /// <summary>
    /// 构件
    /// </summary>
    public class Instances
    {
        public Instances() 
        {
            this.InstanceID = "";
            this.InstanceName = "";
            this.InstanceTypeID = "";
            this._Bars = new List<BarItem>();
        
        }
        public string InstanceID {  get; set; }
        public string InstanceName { get; set; }
        public string InstanceTypeID { get; set; }

        public List<BarItem> _Bars { get; set; }
    }

}
