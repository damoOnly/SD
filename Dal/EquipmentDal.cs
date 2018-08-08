using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Entity;
using System.Data;

namespace Dal
{
    public class EquipmentDal
    {
        public static bool AddOne(Equipment data)
        {
            string sql = string.Format("insert into tb_Equipment (Address,AlertType,EName,Place,Range,Unit,Point,Revise,LowAlert,HighAlert,Addtime) values ('{0}',{1},'{2}','{3}',{4},'{5}',{6},{7},{8},{9},'{10}')", data.Address, data.AlertType, data.EName, data.Place, data.Range, data.Unit, data.Point, data.Revise, data.LowAlert, data.HighAlert, data.Addtime.ToString("yyyy/MM/dd HH:mm:ss"));
            if (SqliteHelper.ExecuteNonQuery(sql) == 1)
            {
                return true;
            }
            else
            {
                LogLib.Log.GetLogger("EquipmentDal").Warn("插入设备传感器失败");
                return false;
            }
        }

        public static Equipment AddOneR(Equipment data)
        {
            string sql = string.Format("insert into tb_Equipment (Address,AlertType,EName,Place,Range,Unit,Point,Revise,LowAlert,HighAlert,Addtime) values ('{0}',{1},'{2}','{3}',{4},'{5}',{6},{7},{8},{9},'{10}')", data.Address, data.AlertType, data.EName, data.Place, data.Range, data.Unit, data.Point, data.Revise, data.LowAlert, data.HighAlert, data.Addtime.ToString("yyyy/MM/dd HH:mm:ss"));
            if (SqliteHelper.ExecuteNonQuery(sql) == 1)
            {
                string whe = string.Format("Address = '{0}' AND Addtime = '{1}'", data.Address, data.Addtime.ToString("yyyy/MM/dd HH:mm:ss"));
                Equipment eee = EquipmentDal.GetOneByWh(whe);
                return eee;
            }
            else
            {
                LogLib.Log.GetLogger("EquipmentDal").Warn("插入设备传感器失败");
                return null;
            }
        }

        public static bool UpdateOne(Equipment data)
        {
            string sql = string.Format("UPDATE tb_Equipment SET Address='{0}',AlertType={1},EName='{2}',Place='{3}',Range={4},Unit='{5}',Point={6},Revise={7},LowAlert={8},HighAlert={9},Addtime='{10}' WHERE ID={11}", data.Address, data.AlertType, data.EName, data.Place, data.Range, data.Unit, data.Point, data.Revise, data.LowAlert, data.HighAlert, data.Addtime.ToString("yyyy/MM/dd HH:mm:ss"), data.ID);
            if (SqliteHelper.ExecuteNonQuery(sql) == 1)
            {
                return true;
            }
            else
            {
                LogLib.Log.GetLogger("EquipmentDal").Warn("更新设备传感器失败");
                return false;
            }
        }

        public static void AddList(List<Equipment> list)
        {

        }

        public static Equipment GetOneByID(int id)
        {
            string sql = string.Format("select Address,AlertType,EName,Place,Range,Unit,Point,Revise,LowAlert,HighAlert,Addtime from tb_Equipment where ID={0} limit 0,1", id);
            DataSet ds = new DataSet();
            ds = SqliteHelper.Query(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Equipment eq = new Equipment();
                eq.EName = ds.Tables[0].Rows[0]["EName"].ToString();
                eq.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                eq.AlertType = Convert.ToByte(ds.Tables[0].Rows[0]["AlertType"]);
                eq.Place = ds.Tables[0].Rows[0]["Place"].ToString();
                eq.Range = Convert.ToSingle(ds.Tables[0].Rows[0]["Range"]);
                eq.Unit = ds.Tables[0].Rows[0]["Unit"].ToString();
                eq.Point = Convert.ToByte(ds.Tables[0].Rows[0]["Point"]);
                eq.Revise = Convert.ToSingle(ds.Tables[0].Rows[0]["Revise"]);
                eq.LowAlert = Convert.ToSingle(ds.Tables[0].Rows[0]["LowAlert"]);
                eq.HighAlert = Convert.ToSingle(ds.Tables[0].Rows[0]["HighAlert"]);
                eq.Addtime = Convert.ToDateTime(ds.Tables[0].Rows[0]["Addtime"]);


                return eq;
            }
            LogLib.Log.GetLogger("EquipmentDal").Warn("获取设备传感器失败");
            return null;
        }

        public static Equipment GetOneByWh(string where)
        {
            string sql = string.Format("select ID,Address,AlertType,EName,Place,Range,Unit,Point,Revise,LowAlert,HighAlert,Addtime from tb_Equipment where {0}  limit 0,1", where);
            DataSet ds = new DataSet();
            ds = SqliteHelper.Query(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Equipment eq = new Equipment();
                eq.ID = Convert.ToInt64(ds.Tables[0].Rows[0]["ID"]);
                eq.EName = ds.Tables[0].Rows[0]["EName"].ToString();
                eq.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                eq.AlertType = Convert.ToByte(ds.Tables[0].Rows[0]["AlertType"]);
                eq.Place = ds.Tables[0].Rows[0]["Place"].ToString();
                eq.Range = Convert.ToSingle(ds.Tables[0].Rows[0]["Range"]); 
                eq.Unit = ds.Tables[0].Rows[0]["Unit"].ToString();
                eq.Point = Convert.ToByte(ds.Tables[0].Rows[0]["Point"]);
                eq.Revise = Convert.ToSingle(ds.Tables[0].Rows[0]["Revise"]);
                eq.LowAlert = Convert.ToSingle(ds.Tables[0].Rows[0]["LowAlert"]);
                eq.HighAlert = Convert.ToSingle(ds.Tables[0].Rows[0]["HighAlert"]);
                eq.Addtime = Convert.ToDateTime(ds.Tables[0].Rows[0]["Addtime"]);
                return eq;
            }
            LogLib.Log.GetLogger("EquipmentDal").Warn("获取设备传感器失败");
            return null;
        }

        public static List<Equipment> GetAllList()
        {
            string sql = string.Format("select ID, Address,AlertType,EName,Place,Range,Unit,Point,Revise,LowAlert,HighAlert,Addtime from tb_Equipment");
            DataSet ds = new DataSet();
            ds = SqliteHelper.Query(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                List<Equipment> list = new List<Equipment>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Equipment eq = new Equipment();
                    eq.ID = Convert.ToInt64(row["ID"]);
                    eq.EName = row["EName"].ToString();
                    eq.Address = row["Address"].ToString();
                    eq.AlertType = Convert.ToByte(row["AlertType"]);
                    eq.Place = row["Place"].ToString();
                    eq.Range = Convert.ToSingle(row["Range"]);
                    eq.Unit = row["Unit"].ToString();
                    eq.Point = Convert.ToByte(row["Point"]);
                    eq.Revise = Convert.ToSingle(row["Revise"]);
                    eq.LowAlert = Convert.ToSingle(row["LowAlert"]);
                    eq.HighAlert = Convert.ToSingle(row["HighAlert"]);
                    eq.Addtime = Convert.ToDateTime(row["Addtime"]);
                    list.Add(eq);
                }
                return list;
            }
            LogLib.Log.GetLogger("EquipmentDal").Warn("获取设备传感器列表失败");
            return new List<Equipment>();
        }

        /// <summary>
        /// 获取所有列表，包括已删除的
        /// </summary>
        /// <returns></returns>
        //public static List<Equipment> GetListIn()
        //{
        //    string sql = string.Format("select ID,Name,Address,GasType,SensorType,Low,High,Max,UnitType,CreateTime,UpDateTime,Point,LowChroma from tb_Equipment");
        //    DataSet ds = new DataSet();
        //    ds = SqliteHelper.Query(sql);
        //    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        List<Equipment> list = new List<Equipment>();
        //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //        {
        //            Equipment eq = new Equipment();
        //            eq.ID = Convert.ToInt32(ds.Tables[0].Rows[i]["ID"]);
        //            eq.Name = ds.Tables[0].Rows[i]["Name"].ToString();
        //            eq.Address = Convert.ToByte(ds.Tables[0].Rows[i]["Address"]);
        //            eq.GasType = Convert.ToByte(ds.Tables[0].Rows[i]["GasType"]);
        //            eq.SensorType = (EM_HighType)ds.Tables[0].Rows[i]["SensorType"];
        //            eq.A1 = Convert.ToSingle(ds.Tables[0].Rows[i]["Low"]);
        //            eq.A2 = Convert.ToSingle(ds.Tables[0].Rows[i]["High"]);
        //            eq.Max = Convert.ToUInt32(ds.Tables[0].Rows[i]["Max"]);
        //            eq.UnitType = Convert.ToByte(ds.Tables[0].Rows[i]["UnitType"]);
        //            eq.CreateTime = (DateTime)ds.Tables[0].Rows[i]["CreateTime"];
        //            eq.UpDateTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["UpDateTime"]);
        //            eq.Point = Convert.ToByte(ds.Tables[0].Rows[0]["Point"]);
        //            eq.LowChroma = Convert.ToSingle(ds.Tables[0].Rows[0]["LowChroma"]);
        //            list.Add(eq);
        //        }
        //        return list;
        //    }
        //    LogLib.Log.GetLogger("EquipmentDal").Warn("获取设备传感器列表失败");
        //    return new List<Equipment>();
        //}

        /// <summary>
        /// 获取有效的设备地址
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAddress()
        {
            string sql = string.Format("select Address from tb_Equipment");
            DataSet ds = new DataSet();
            ds = SqliteHelper.Query(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                List<string> list = new List<string>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(row["Address"].ToString());
                }
                return list.Distinct().ToList();
            }
            LogLib.Log.GetLogger("EquipmentDal").Warn("获取设备传感器地址列表失败");
            return new List<string>();
        }

        /// <summary>
        /// 获取所有设备名称，包括已经删除的
        /// </summary>
        /// <returns></returns>
        //public static List<string> GetNames()
        //{
        //    string sql = string.Format("select Name from tb_Equipment");
        //    DataSet ds = new DataSet();
        //    ds = .Query(sql);
        //    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        List<string> list = new List<string>();
        //        foreach (DataRow row in ds.Tables[0].Rows)
        //        {
        //            list.Add(row["Name"].ToString());
        //        }
        //        return list.Distinct().ToList();
        //    }
        //    LogLib.Log.GetLogger("EquipmentDal").Warn("获取设备传感器名称列表失败");
        //    return new List<string>();
        //}

        public static bool DeleteList(List<Equipment> list)
        {
            string sql;
            foreach (Equipment item in list)
            {
                sql = string.Format("delete from tb_Equipment where ID = {0}", item.ID);
                if (SqliteHelper.ExecuteNonQuery(sql) != 1)
                {
                    LogLib.Log.GetLogger("EquipmentDal").Warn("批量删除失败");
                }
            }
            return true;
        }

        /// <summary>
        /// 按设备地址删除
        /// </summary>
        /// <param name="address">设备地址</param>
        /// <returns></returns>
        public static bool DeleteListByID(int address)
        {
            string sql;

            sql = string.Format("delete from tb_Equipment where Address = {0}", address);

            //sql = string.Format("update [tb_Equipment set IsDel = 1 where Address = {0}", address);
            if (SqliteHelper.ExecuteNonQuery(sql) < 1)
            {
                LogLib.Log.GetLogger("EquipmentDal").Warn(string.Format("删除‘{0}’设备失败", address));
                return false;
            }
            return true;
        }

        public static bool DeleteOne(Equipment one)
        {
            string sql;
            sql = string.Format("delete from tb_Equipment where ID = {0}", one.ID);
            if (SqliteHelper.ExecuteNonQuery(sql) != 1)
            {                
                LogLib.Log.GetLogger("EquipmentDal").Warn("删除失败");
                return false;
            }
            EquipmentDataDal.DeleteByEqID(one.ID);
            AlertDal.DeleteByEqID(one.ID);
            return true;
        }
    }
}
