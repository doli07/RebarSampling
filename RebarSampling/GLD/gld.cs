using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace RebarSampling.GLD
{
    /// <summary>
    /// 广联达料单解析类
    /// </summary>
    public class Gld
    {

        public Gld() 
        { 
            this.gld_Model=new Gld_model();
            this.gld_Barlist=new Gld_barlist();
            this.gld_Project=new Gld_project();
        }
        public Gld_barlist gld_Barlist {  get; set; }
        public Gld_project gld_Project { get; set; }
        public Gld_model gld_Model { get; set; }


        /// <summary>
        /// 广联达料单树形数据结构的实例
        /// </summary>
        public GldStructure gld_Structure
        {
            get
            {
                GldStructure gldstruct=new  GldStructure();

                gldstruct.ProjectName=gld_Project.ProjectName;
                foreach(var item in gld_Project.BuildingItems)
                {
                    Buildings temp_B = new Buildings();   
                    temp_B.BuildingID= item.BuildingID;
                    temp_B.BuildingName= item.BuildingName;

                    foreach(var iii in gld_Project.FloorItems.FindAll(t=>t.BuildingID==item.BuildingID))//查找buildingID一致的floor
                    {
                        Floors temp_F = new Floors();
                        temp_F.FloorID=iii.FloorID;
                        temp_F.FloorName=iii.FloorName;
                        
                        foreach(var ttt in gld_Model.InstanceItems.FindAll(t=>t.FloorID==iii.FloorID))//查找floorID一致的instance
                        {
                            Instances temp_I = new Instances();
                            temp_I.InstanceID=ttt.InstanceID;
                            temp_I.InstanceName=ttt.InstanceName;
                            temp_I.InstanceTypeID=ttt.InstanceTypeID;

                            foreach(var eee in gld_Barlist.BarItems.FindAll(t=>t.InstanceID==ttt.InstanceID))
                            {
                                temp_I._Bars.Add(eee);
                            }
                            temp_F._Instances.Add(temp_I);
                        }
                        temp_B._Floors.Add(temp_F);
                    }

                    gldstruct._Buildings.Add(temp_B);
                }

                return gldstruct;
            }
        }


        private static ReaderWriterLockSlim LogWriteLock = new ReaderWriterLockSlim();
        public static string LoadGldJson(string _path)
        {
            try
            {
                LogWriteLock.EnterWriteLock();

                string rt = "";

                if (File.Exists(_path))
                {
                    rt = File.ReadAllText(_path);
                }
                return rt;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                return "";
            }
            finally { LogWriteLock.ExitWriteLock(); }

        }

        public static Image LoadGldImage(string _path)
        {
            try
            {
                LogWriteLock.EnterWriteLock();

                Image rt = null;

                if (File.Exists(_path))
                {
                    //rt = File.ReadAllText(_path);
                    rt=Image.FromFile(_path);
                }
                return rt;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                return null;
            }
            finally { LogWriteLock.ExitWriteLock(); }

        }
    }




}
