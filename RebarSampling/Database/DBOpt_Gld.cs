using RebarSampling.GLD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RebarSampling
{
    public partial class DBOpt
    {
        private string gld_dbname_rebar = "gld_rebar";
        private string gld_dbname_building = "gld_Building";
        private string gld_dbname_floor = "gld_floor";
        private string gld_dbname_project = "gld_project";
        private string gld_dbname_category = "gld_category";
        private string gld_dbname_constructionsection = "gld_constructionsection";
        private string gld_dbname_instance = "gld_instance";
        private string gld_dbname_instancetype = "gld_instancetype";

        public void InitDB_Gld()
        {
            try
            {

                //如果table不存在，则创建新表
                if (!dbHelper.IsTableEXIST(gld_dbname_rebar)) { CreateRebarTable_Gld(gld_dbname_rebar); }
                dbHelper.DeleteTable(gld_dbname_rebar);         //清空表
                dbHelper.DeleteTableSequence(gld_dbname_rebar); //清空表的自增列序号

                if (!dbHelper.IsTableEXIST(gld_dbname_building)) { CreatePBuildingTable_Gld(gld_dbname_building); }
                dbHelper.DeleteTable(gld_dbname_building);         //清空表
                dbHelper.DeleteTableSequence(gld_dbname_building); //清空表的自增列序号

                if (!dbHelper.IsTableEXIST(gld_dbname_floor)) { CreatePFloorTable_Gld(gld_dbname_floor); }
                dbHelper.DeleteTable(gld_dbname_floor);         //清空表
                dbHelper.DeleteTableSequence(gld_dbname_floor); //清空表的自增列序号

                if (!dbHelper.IsTableEXIST(gld_dbname_project)) { CreateProjectTable_Gld(gld_dbname_project); }
                dbHelper.DeleteTable(gld_dbname_project);         //清空表
                dbHelper.DeleteTableSequence(gld_dbname_project); //清空表的自增列序号

                if (!dbHelper.IsTableEXIST(gld_dbname_category)) { CreateMCategoryTable_Gld(gld_dbname_category); }
                dbHelper.DeleteTable(gld_dbname_category);         //清空表
                dbHelper.DeleteTableSequence(gld_dbname_category); //清空表的自增列序号

                if (!dbHelper.IsTableEXIST(gld_dbname_constructionsection)) { CreateMConstructionSectionTable_Gld(gld_dbname_constructionsection); }
                dbHelper.DeleteTable(gld_dbname_constructionsection);         //清空表
                dbHelper.DeleteTableSequence(gld_dbname_constructionsection); //清空表的自增列序号

                if (!dbHelper.IsTableEXIST(gld_dbname_instance)) { CreateMInstanceTable_Gld(gld_dbname_instance); }
                dbHelper.DeleteTable(gld_dbname_instance);         //清空表
                dbHelper.DeleteTableSequence(gld_dbname_instance); //清空表的自增列序号

                if (!dbHelper.IsTableEXIST(gld_dbname_instancetype)) { CreateMInstanceTypeTable_Gld(gld_dbname_instancetype); }
                dbHelper.DeleteTable(gld_dbname_instancetype);         //清空表
                dbHelper.DeleteTableSequence(gld_dbname_instancetype); //清空表的自增列序号


            }
            catch (Exception ex) { MessageBox.Show("InitRebarDB error:" + ex.Message); }

        }

        /// <summary>
        /// 广联达料单存入数据库
        /// </summary>
        /// <param name="_data"></param>
        public void Gld_to_DB(GLD.Gld _data)
        {
            List<string> sqls = new List<string>();

            foreach (var item in _data.gld_Barlist.BarItems)
            {
                sqls.Add(InsertRowRebarData_Gld(gld_dbname_rebar, item));
            }
            sqls.Add(InsertRowProject_Gld(gld_dbname_project, _data.gld_Project));

            foreach (var item in _data.gld_Project.BuildingItems)
            {
                sqls.Add(InsertRowPBuilding_Gld(gld_dbname_building, item));
            }
            foreach (var item in _data.gld_Project.FloorItems)
            {
                sqls.Add(InsertRowPFloor_Gld(gld_dbname_floor, item));
            }
            foreach (var item in _data.gld_Model.CategoryItems)
            {
                sqls.Add(InsertRowMCategory_Gld(gld_dbname_category, item));
            }
            foreach (var item in _data.gld_Model.ConstructionSectionItems)
            {
                sqls.Add(InsertRowMConstructionSection_Gld(gld_dbname_constructionsection, item));
            }
            foreach (var item in _data.gld_Model.InstanceItems)
            {
                sqls.Add(InsertRowMInstance_Gld(gld_dbname_instance, item));
            }
            foreach (var item in _data.gld_Model.InstanceTypeItems)
            {
                sqls.Add(InsertRowMInstanceType_Gld(gld_dbname_instancetype, item));
            }

            dbHelper.ExecuteSqlsTran(sqls);//批量存入

        }

        public List<RebarData> Gld_to_rebardata(GldStructure _data)
        {
            List<RebarData> newlist = new List<RebarData>();
            RebarData rebarData = new RebarData();

            foreach (var item in _data._Buildings)
            {
                string buildingname = item.BuildingName;
                foreach (var iii in item._Floors)
                {
                    string floorname = iii.FloorName;
                    foreach (var ttt in iii._Instances)
                    {
                        string instanceName = ttt.InstanceID + "_" + ttt.InstanceName;
                        foreach (var eee in ttt._Bars)
                        {
                            rebarData = new RebarData();
                            rebarData.ProjectName = buildingname;
                            rebarData.MainAssemblyName = floorname;
                            rebarData.ElementName = instanceName;
                            rebarData.PicTypeNum = eee.BarShapeTypeID;
                            rebarData.PicMessage = eee.Formula;
                            rebarData.RebarPic = eee.BarSketchFile;//图片保存路径，广联达的图片是有专门的保存路径的
                            rebarData.Level = eee.LevelName;
                            rebarData.Diameter = (int)Convert.ToDouble(eee.Diameter);
                            rebarData.CornerMessage = eee.BarDataExpression;//e筋的边角结构，广联达的BarDataExpression
                            rebarData.Length = eee.BreakLength;
                            rebarData.PieceNumUnitNum = eee.Count.ToString();
                            rebarData.TotalPieceNum = eee.Count;
                            rebarData.TotalWeight = eee.Weight * eee.Count;
                            rebarData.Description = eee.Remark;
                            rebarData.SerialNum = Convert.ToInt32(eee.BarIndex);
                            rebarData.BarType = eee.BarType;
                            rebarData.FabricationType = eee.FabricationType;
                            ////rebarData.IsOriginal = Convert.ToBoolean(row[(int)EnumAllRebarTableColName.ISORIGINAL + 1].ToString());
                            ////rebarData.IfTao = Convert.ToBoolean(row[(int)EnumAllRebarTableColName.IFTAO + 1].ToString());
                            ////rebarData.IfBend = Convert.ToBoolean(row[(int)EnumAllRebarTableColName.IFBEND + 1].ToString());
                            ////rebarData.IfCut = Convert.ToBoolean(row[(int)EnumAllRebarTableColName.IFCUT + 1].ToString());
                            ////rebarData.IfBendTwice = Convert.ToBoolean(row[(int)EnumAllRebarTableColName.IFBENDTWICE + 1].ToString());
                            rebarData.TableName = buildingname;
                            rebarData.TableSheetName = floorname;
                            //rebarData.IndexNo = int.Parse(row[0].ToString());//数据库索引

                            newlist.Add(rebarData);
                        }

                    }

                }
            }

            return newlist;
        }
        public void CreateRebarTable_Gld(string tableName/*,EnumMaterialBill _matBill = EnumMaterialBill.EJIN*/)
        {
            try
            {
                //if (_matBill == EnumMaterialBill.GLD)//广联达料单
                {
                    dbHelper.CreateTable(tableName);
                    dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName_Gld[0], dbHelper.GetDataType(DbType.String));
                    dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName_Gld[1], dbHelper.GetDataType(DbType.String));
                    dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName_Gld[2], dbHelper.GetDataType(DbType.String));
                    dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName_Gld[3], dbHelper.GetDataType(DbType.String));
                    dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName_Gld[4], dbHelper.GetDataType(DbType.String));
                    dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName_Gld[5], dbHelper.GetDataType(DbType.String));
                    dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName_Gld[6], dbHelper.GetDataType(DbType.String));
                    dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName_Gld[7], dbHelper.GetDataType(DbType.Int32));
                    dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName_Gld[8], dbHelper.GetDataType(DbType.String));
                    dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName_Gld[9], dbHelper.GetDataType(DbType.String));
                    dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName_Gld[10], dbHelper.GetDataType(DbType.String));
                    dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName_Gld[11], dbHelper.GetDataType(DbType.Int32));
                    dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName_Gld[12], dbHelper.GetDataType(DbType.String));
                    dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName_Gld[13], dbHelper.GetDataType(DbType.String));
                    dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName_Gld[14], dbHelper.GetDataType(DbType.Int32));
                    dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName_Gld[15], dbHelper.GetDataType(DbType.String));
                    dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName_Gld[16], dbHelper.GetDataType(DbType.String));
                    dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName_Gld[17], dbHelper.GetDataType(DbType.Double));
                    dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName_Gld[18], dbHelper.GetDataType(DbType.String));
                    dbHelper.AddColumn(tableName, GeneralClass.sRebarColumnName_Gld[19], dbHelper.GetDataType(DbType.Double));
                }
            }
            catch (Exception ex) { throw ex; }
        }
        public void CreatePBuildingTable_Gld(string tableName)
        {
            try
            {
                dbHelper.CreateTable(tableName);
                dbHelper.AddColumn(tableName, GeneralClass.s_pBuildingColumnName_Gld[0], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.s_pBuildingColumnName_Gld[1], dbHelper.GetDataType(DbType.String));
            }
            catch (Exception ex) { throw ex; }
        }

        public void CreatePFloorTable_Gld(string tableName)
        {
            try
            {
                dbHelper.CreateTable(tableName);
                dbHelper.AddColumn(tableName, GeneralClass.s_pFloorColumnName_Gld[0], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.s_pFloorColumnName_Gld[1], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.s_pFloorColumnName_Gld[2], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.s_pFloorColumnName_Gld[3], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.s_pFloorColumnName_Gld[4], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.s_pFloorColumnName_Gld[5], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.s_pFloorColumnName_Gld[6], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.s_pFloorColumnName_Gld[7], dbHelper.GetDataType(DbType.String));

            }
            catch (Exception ex) { throw ex; }
        }

        public void CreateProjectTable_Gld(string tableName)
        {
            try
            {
                dbHelper.CreateTable(tableName);
                dbHelper.AddColumn(tableName, GeneralClass.sProjectColumnName_Gld[0], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sProjectColumnName_Gld[1], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sProjectColumnName_Gld[2], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sProjectColumnName_Gld[3], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sProjectColumnName_Gld[4], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sProjectColumnName_Gld[5], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sProjectColumnName_Gld[6], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sProjectColumnName_Gld[7], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sProjectColumnName_Gld[8], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sProjectColumnName_Gld[9], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sProjectColumnName_Gld[10], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sProjectColumnName_Gld[11], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sProjectColumnName_Gld[12], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sProjectColumnName_Gld[13], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sProjectColumnName_Gld[14], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sProjectColumnName_Gld[15], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sProjectColumnName_Gld[16], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sProjectColumnName_Gld[17], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sProjectColumnName_Gld[18], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sProjectColumnName_Gld[19], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.sProjectColumnName_Gld[20], dbHelper.GetDataType(DbType.String));

            }
            catch (Exception ex) { throw ex; }
        }

        public void CreateMCategoryTable_Gld(string tableName)
        {
            try
            {
                dbHelper.CreateTable(tableName);
                dbHelper.AddColumn(tableName, GeneralClass.s_mCategoryColumnName_Gld[0], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.s_mCategoryColumnName_Gld[1], dbHelper.GetDataType(DbType.String));
            }
            catch (Exception ex) { throw ex; }
        }
        public void CreateMConstructionSectionTable_Gld(string tableName)
        {
            try
            {
                dbHelper.CreateTable(tableName);
                dbHelper.AddColumn(tableName, GeneralClass.s_mConstructionSectionColumnName_Gld[0], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.s_mConstructionSectionColumnName_Gld[1], dbHelper.GetDataType(DbType.String));
            }
            catch (Exception ex) { throw ex; }
        }
        public void CreateMInstanceTable_Gld(string tableName)
        {
            try
            {
                dbHelper.CreateTable(tableName);
                dbHelper.AddColumn(tableName, GeneralClass.s_mInstanceColumnName_Gld[0], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.s_mInstanceColumnName_Gld[1], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.s_mInstanceColumnName_Gld[2], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.s_mInstanceColumnName_Gld[3], dbHelper.GetDataType(DbType.String));

            }
            catch (Exception ex) { throw ex; }
        }

        public void CreateMInstanceTypeTable_Gld(string tableName)
        {
            try
            {
                dbHelper.CreateTable(tableName);
                dbHelper.AddColumn(tableName, GeneralClass.s_mInstanceTypeColumnName_Gld[0], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.s_mInstanceTypeColumnName_Gld[1], dbHelper.GetDataType(DbType.String));
                dbHelper.AddColumn(tableName, GeneralClass.s_mInstanceTypeColumnName_Gld[2], dbHelper.GetDataType(DbType.String));

            }
            catch (Exception ex) { throw ex; }
        }
        public string InsertRowRebarData_Gld(string _tableName, BarItem _bardata)
        {
            try
            {
                string sqlstr = "insert into " + _tableName + "(";

                //for (int i = 0; i < 11; i++)
                //{
                //    sqlstr += GeneralClass.sRebarColumnName_Gld[i] + ",";
                //}
                //sqlstr +="`"+ GeneralClass.sRebarColumnName_Gld[11] + "`,";

                for (int i = 0; i < (int)GeneralClass.sRebarColumnName_Gld.Length; i++)
                {
                    sqlstr += GeneralClass.sRebarColumnName_Gld[i] + ",";
                }



                sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);//去掉尾部的逗号
                sqlstr += ")" + "values(";

                sqlstr += "'" + _bardata.BarDataExpression + "'" + ",";
                sqlstr += "'" + _bardata.BarIndex + "'" + ",";
                sqlstr += "'" + _bardata.BarShapeTypeID + "'" + ",";
                sqlstr += "'" + _bardata.BarSketchFile + "'" + ",";
                sqlstr += "'" + _bardata.BarType + "'" + ",";
                sqlstr += "'" + _bardata.BreakLength + "'" + ",";
                sqlstr += "'" + _bardata.ConstructionSectionID + "'" + ",";
                sqlstr += _bardata.Count.ToString() + ",";
                sqlstr += "'" + _bardata.Diameter + "'" + ",";
                sqlstr += "'" + _bardata.FabricationType + "'" + ",";
                sqlstr += "'" + _bardata.Formula + "'" + ",";
                sqlstr += _bardata.Index.ToString() + ",";
                sqlstr += "'" + _bardata.InstanceID + "'" + ",";
                sqlstr += "'" + _bardata.InstanceIndex + "'" + ",";
                sqlstr += _bardata.Length.ToString() + ",";
                sqlstr += "'" + _bardata.LevelName + "'" + ",";
                sqlstr += "'" + _bardata.Remark + "'" + ",";
                sqlstr += _bardata.Space.ToString() + ",";
                sqlstr += "'" + _bardata.UsedType + "'" + ",";
                sqlstr += _bardata.Weight.ToString();

                sqlstr += ")";

                return sqlstr;

            }
            catch (Exception) { throw; }
            finally { /*command.Dispose(); connection.Close();*/ }

        }

        public string InsertRowPBuilding_Gld(string _tableName, BuildingItem _data)
        {
            try
            {
                string sqlstr = "insert into " + _tableName + "(";

                for (int i = 0; i < (int)GeneralClass.s_pBuildingColumnName_Gld.Length; i++)
                {
                    sqlstr += GeneralClass.s_pBuildingColumnName_Gld[i] + ",";
                }
                sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);//去掉尾部的逗号
                sqlstr += ")" + "values(";

                sqlstr += "'" + _data.BuildingID + "'" + ",";
                sqlstr += "'" + _data.BuildingName + "'";

                sqlstr += ")";

                return sqlstr;

            }
            catch (Exception) { throw; }
            finally { /*command.Dispose(); connection.Close();*/ }

        }

        public string InsertRowPFloor_Gld(string _tableName, FloorItem _data)
        {
            try
            {
                string sqlstr = "insert into " + _tableName + "(";

                for (int i = 0; i < (int)GeneralClass.s_pFloorColumnName_Gld.Length; i++)
                {
                    sqlstr += GeneralClass.s_pFloorColumnName_Gld[i] + ",";
                }
                sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);//去掉尾部的逗号
                sqlstr += ")" + "values(";

                sqlstr += "'" + _data.BuildingID + "'" + ",";
                sqlstr += _data.FloorArea.ToString() + ",";
                sqlstr += _data.FloorElevation.ToString() + ",";
                sqlstr += _data.FloorHeight.ToString() + ",";
                sqlstr += _data.FloorID.ToString() + ",";
                sqlstr += "'" + _data.FloorName + "'" + ",";
                sqlstr += "'" + _data.Remark + "'" + ",";
                sqlstr += "'" + _data.StandardFloorCount + "'";

                sqlstr += ")";

                return sqlstr;

            }
            catch (Exception) { throw; }
            finally { /*command.Dispose(); connection.Close();*/ }

        }

        public string InsertRowProject_Gld(string _tableName, Gld_project _data)
        {
            try
            {
                string sqlstr = "insert into " + _tableName + "(";

                for (int i = 0; i < (int)GeneralClass.sProjectColumnName_Gld.Length; i++)
                {
                    sqlstr += GeneralClass.sProjectColumnName_Gld[i] + ",";
                }
                sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);//去掉尾部的逗号
                sqlstr += ")" + "values(";

                sqlstr += "'" + _data.CalculationRule + "'" + ",";
                sqlstr += "'" + _data.ConstructionOrganization + "'" + ",";
                sqlstr += "'" + _data.CreatedDate + "'" + ",";
                sqlstr += "'" + _data.DesignOrganization + "'" + ",";
                sqlstr += "'" + _data.DevelopmentOrganization + "'" + ",";
                sqlstr += "'" + _data.EarthquakeResistance + "'" + ",";
                sqlstr += "'" + _data.OvergroundFloorCount + "'" + ",";
                sqlstr += "'" + _data.ProjectAddress + "'" + ",";
                sqlstr += "'" + _data.ProjectArea + "'" + ",";
                sqlstr += "'" + _data.ProjectCity + "'" + ",";
                sqlstr += "'" + _data.ProjectCode + "'" + ",";
                sqlstr += "'" + _data.ProjectDate + "'" + ",";
                sqlstr += "'" + _data.ProjectHeight + "'" + ",";
                sqlstr += "'" + _data.ProjectName + "'" + ",";
                sqlstr += "'" + _data.ProjectProvince + "'" + ",";
                sqlstr += "'" + _data.ProjectType + "'" + ",";
                sqlstr += "'" + _data.StructureType + "'" + ",";
                sqlstr += "'" + _data.SupervisoryOrganization + "'" + ",";
                sqlstr += "'" + _data.UID + "'" + ",";
                sqlstr += "'" + _data.UndergroundFloorCount + "'" + ",";
                sqlstr += "'" + _data.Version + "'";

                sqlstr += ")";

                return sqlstr;
            }
            catch (Exception) { throw; }
            finally { /*command.Dispose(); connection.Close();*/ }
        }

        public string InsertRowMCategory_Gld(string _tableName, CategoryItem _data)
        {
            try
            {
                string sqlstr = "insert into " + _tableName + "(";

                for (int i = 0; i < (int)GeneralClass.s_mCategoryColumnName_Gld.Length; i++)
                {
                    sqlstr += GeneralClass.s_mCategoryColumnName_Gld[i] + ",";
                }
                sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);//去掉尾部的逗号
                sqlstr += ")" + "values(";

                sqlstr += "'" + _data.CategoryID + "'" + ",";
                sqlstr += "'" + _data.CategoryName + "'";

                sqlstr += ")";

                return sqlstr;
            }
            catch (Exception) { throw; }
            finally { /*command.Dispose(); connection.Close();*/ }
        }

        public string InsertRowMConstructionSection_Gld(string _tableName, ConstructionSectionItem _data)
        {
            try
            {
                string sqlstr = "insert into " + _tableName + "(";

                for (int i = 0; i < (int)GeneralClass.s_mConstructionSectionColumnName_Gld.Length; i++)
                {
                    sqlstr += GeneralClass.s_mConstructionSectionColumnName_Gld[i] + ",";
                }
                sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);//去掉尾部的逗号
                sqlstr += ")" + "values(";

                sqlstr += "'" + _data.ConstructionSectionID + "'" + ",";
                sqlstr += "'" + _data.ConstructionSectionName + "'";

                sqlstr += ")";
                return sqlstr;
            }
            catch (Exception) { throw; }
            finally { /*command.Dispose(); connection.Close();*/ }
        }

        public string InsertRowMInstance_Gld(string _tableName, InstanceItem _data)
        {
            try
            {
                string sqlstr = "insert into " + _tableName + "(";

                for (int i = 0; i < (int)GeneralClass.s_mInstanceColumnName_Gld.Length; i++)
                {
                    sqlstr += GeneralClass.s_mInstanceColumnName_Gld[i] + ",";
                }
                sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);//去掉尾部的逗号
                sqlstr += ")" + "values(";

                sqlstr += "'" + _data.FloorID + "'" + ",";
                sqlstr += "'" + _data.InstanceID + "'" + ",";
                sqlstr += "'" + _data.InstanceName + "'" + ",";
                sqlstr += "'" + _data.InstanceTypeID + "'";

                sqlstr += ")";
                return sqlstr;
            }
            catch (Exception) { throw; }
            finally { /*command.Dispose(); connection.Close();*/ }
        }

        public string InsertRowMInstanceType_Gld(string _tableName, InstanceTypeItem _data)
        {
            try
            {
                string sqlstr = "insert into " + _tableName + "(";

                for (int i = 0; i < (int)GeneralClass.s_mInstanceTypeColumnName_Gld.Length; i++)
                {
                    sqlstr += GeneralClass.s_mInstanceTypeColumnName_Gld[i] + ",";
                }
                sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);//去掉尾部的逗号
                sqlstr += ")" + "values(";

                sqlstr += "'" + _data.CategoryID + "'" + ",";
                sqlstr += "'" + _data.InstanceTypeID + "'" + ",";
                sqlstr += "'" + _data.InstanceTypeName + "'";

                sqlstr += ")";
                return sqlstr;
            }
            catch (Exception) { throw; }
            finally { /*command.Dispose(); connection.Close();*/ }
        }

    }
}
