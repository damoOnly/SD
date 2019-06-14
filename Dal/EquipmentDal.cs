﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Entity;
using System.Data;
using System.Data.SQLite;

namespace Dal
{
    public class EquipmentDal
    {
        public static bool AddOne(Equipment data)
        {
            string sql = "insert into tb_Equipment (Address,AlertType,EName,Place,Range,Unit,Point,Revise,LowAlert,HighAlert,Addtime, IsAnemoscope) values (@Address,@AlertType,@EName,@Place,@Range,@Unit,@Point,@Revise,@LowAlert,@HighAlert,@Addtime,@IsAnemoscope)";
            List<SQLiteParameter> param = new List<SQLiteParameter>();
            param.Add(new SQLiteParameter() { ParameterName = "@Address", Value = data.Address });
            param.Add(new SQLiteParameter() { ParameterName = "@AlertType", Value = data.AlertType });
            param.Add(new SQLiteParameter() { ParameterName = "@EName", Value = data.EName });
            param.Add(new SQLiteParameter() { ParameterName = "@Place", Value = data.Place });
            param.Add(new SQLiteParameter() { ParameterName = "@Range", Value = data.Range });
            param.Add(new SQLiteParameter() { ParameterName = "@Unit", Value = data.Unit });
            param.Add(new SQLiteParameter() { ParameterName = "@Point", Value = data.Point });
            param.Add(new SQLiteParameter() { ParameterName = "@Revise", Value = data.Revise });
            param.Add(new SQLiteParameter() { ParameterName = "@LowAlert", Value = data.LowAlert });
            param.Add(new SQLiteParameter() { ParameterName = "@HighAlert", Value = data.HighAlert });
            param.Add(new SQLiteParameter() { ParameterName = "@Addtime", Value = data.Addtime });
            param.Add(new SQLiteParameter() { ParameterName = "@IsAnemoscope", Value = data.IsAnemoscope });
            if (SqliteHelper.ExecuteInsert(sql, param.ToArray()) > 0)
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
            string sql = "insert into tb_Equipment (Address,AlertType,EName,Place,Range,Unit,Point,Revise,LowAlert,HighAlert,Addtime, IsAnemoscope) values (@Address,@AlertType,@EName,@Place,@Range,@Unit,@Point,@Revise,@LowAlert,@HighAlert,@Addtime,@IsAnemoscope);";
            List<SQLiteParameter> param = new List<SQLiteParameter>();
            param.Add(new SQLiteParameter() { ParameterName = "@Address", Value = data.Address });
            param.Add(new SQLiteParameter() { ParameterName = "@AlertType", Value = data.AlertType });
            param.Add(new SQLiteParameter() { ParameterName = "@EName", Value = data.EName });
            param.Add(new SQLiteParameter() { ParameterName = "@Place", Value = data.Place });
            param.Add(new SQLiteParameter() { ParameterName = "@Range", Value = data.Range });
            param.Add(new SQLiteParameter() { ParameterName = "@Unit", Value = data.Unit });
            param.Add(new SQLiteParameter() { ParameterName = "@Point", Value = data.Point });
            param.Add(new SQLiteParameter() { ParameterName = "@Revise", Value = data.Revise });
            param.Add(new SQLiteParameter() { ParameterName = "@LowAlert", Value = data.LowAlert });
            param.Add(new SQLiteParameter() { ParameterName = "@HighAlert", Value = data.HighAlert });
            param.Add(new SQLiteParameter() { ParameterName = "@Addtime", Value = data.Addtime });
            param.Add(new SQLiteParameter() { ParameterName = "@IsAnemoscope", Value = data.IsAnemoscope });

            data.ID = SqliteHelper.ExecuteInsert(sql, param.ToArray());
            return data;
        }

        public static bool UpdateOne(Equipment data)
        {
            string sql = "UPDATE tb_Equipment SET Address=@Address,AlertType=@AlertType,EName=@EName,Place=@Place,Range=@Range,Unit=@Unit,Point=@Point,Revise=@Revise,LowAlert=@LowAlert,HighAlert=@HighAlert,Addtime=@Addtime,IsAnemoscope=@IsAnemoscope WHERE ID=@id";
            List<SQLiteParameter> param = new List<SQLiteParameter>();
            param.Add(new SQLiteParameter() { ParameterName = "@Address", Value = data.Address });
            param.Add(new SQLiteParameter() { ParameterName = "@AlertType", Value = data.AlertType });
            param.Add(new SQLiteParameter() { ParameterName = "@EName", Value = data.EName });
            param.Add(new SQLiteParameter() { ParameterName = "@Place", Value = data.Place });
            param.Add(new SQLiteParameter() { ParameterName = "@Range", Value = data.Range });
            param.Add(new SQLiteParameter() { ParameterName = "@Unit", Value = data.Unit });
            param.Add(new SQLiteParameter() { ParameterName = "@Point", Value = data.Point });
            param.Add(new SQLiteParameter() { ParameterName = "@Revise", Value = data.Revise });
            param.Add(new SQLiteParameter() { ParameterName = "@LowAlert", Value = data.LowAlert });
            param.Add(new SQLiteParameter() { ParameterName = "@HighAlert", Value = data.HighAlert });
            param.Add(new SQLiteParameter() { ParameterName = "@Addtime", Value = data.Addtime });
            param.Add(new SQLiteParameter() { ParameterName = "@IsAnemoscope", Value = data.IsAnemoscope });
            param.Add(new SQLiteParameter() { ParameterName = "@id", Value = data.ID });

            if (SqliteHelper.ExecuteSql(sql,param.ToArray()) == 1)
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
            string sql = string.Format("select Address,AlertType,EName,Place,Range,Unit,Point,Revise,LowAlert,HighAlert,Addtime,IsAnemoscope from tb_Equipment where ID={0} limit 0,1", id);
            DataSet ds = new DataSet();
            ds = SqliteHelper.Query(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Equipment eq = new Equipment();
                eq.EName = ds.Tables[0].Rows[0]["EName"].ToString();
                eq.Address = Convert.ToByte(ds.Tables[0].Rows[0]["Address"]);
                eq.AlertType = Convert.ToByte(ds.Tables[0].Rows[0]["AlertType"]);
                eq.Place = ds.Tables[0].Rows[0]["Place"].ToString();
                eq.Range = Convert.ToSingle(ds.Tables[0].Rows[0]["Range"]);
                eq.Unit = ds.Tables[0].Rows[0]["Unit"].ToString();
                eq.Point = Convert.ToByte(ds.Tables[0].Rows[0]["Point"]);
                eq.Revise = Convert.ToSingle(ds.Tables[0].Rows[0]["Revise"]);
                eq.LowAlert = Convert.ToSingle(ds.Tables[0].Rows[0]["LowAlert"]);
                eq.HighAlert = Convert.ToSingle(ds.Tables[0].Rows[0]["HighAlert"]);
                eq.Addtime = Convert.ToDateTime(ds.Tables[0].Rows[0]["Addtime"]);
                eq.IsAnemoscope = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAnemoscope"]);

                return eq;
            }
            LogLib.Log.GetLogger("EquipmentDal").Warn("获取设备传感器失败");
            return null;
        }

        public static Equipment GetOneByWh(string where)
        {
            string sql = string.Format("select ID,Address,AlertType,EName,Place,Range,Unit,Point,Revise,LowAlert,HighAlert,Addtime,IsAnemoscope from tb_Equipment where {0}  limit 0,1", where);
            DataSet ds = new DataSet();
            ds = SqliteHelper.Query(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Equipment eq = new Equipment();
                eq.ID = Convert.ToInt64(ds.Tables[0].Rows[0]["ID"]);
                eq.EName = ds.Tables[0].Rows[0]["EName"].ToString();
                eq.Address = Convert.ToByte(ds.Tables[0].Rows[0]["Address"]);
                eq.AlertType = Convert.ToByte(ds.Tables[0].Rows[0]["AlertType"]);
                eq.Place = ds.Tables[0].Rows[0]["Place"].ToString();
                eq.Range = Convert.ToSingle(ds.Tables[0].Rows[0]["Range"]);
                eq.Unit = ds.Tables[0].Rows[0]["Unit"].ToString();
                eq.Point = Convert.ToByte(ds.Tables[0].Rows[0]["Point"]);
                eq.Revise = Convert.ToSingle(ds.Tables[0].Rows[0]["Revise"]);
                eq.LowAlert = Convert.ToSingle(ds.Tables[0].Rows[0]["LowAlert"]);
                eq.HighAlert = Convert.ToSingle(ds.Tables[0].Rows[0]["HighAlert"]);
                eq.Addtime = Convert.ToDateTime(ds.Tables[0].Rows[0]["Addtime"]);
                eq.IsAnemoscope = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAnemoscope"]);
                return eq;
            }
            LogLib.Log.GetLogger("EquipmentDal").Warn("获取设备传感器失败");
            return null;
        }

        public static List<Equipment> GetAllList()
        {
            string sql = string.Format("select ID, Address,AlertType,EName,Place,Range,Unit,Point,Revise,LowAlert,HighAlert,Addtime,IsAnemoscope from tb_Equipment");
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
                    eq.Address = Convert.ToByte(row["Address"]);
                    eq.AlertType = Convert.ToByte(row["AlertType"]);
                    eq.Place = row["Place"].ToString();
                    eq.Range = Convert.ToSingle(row["Range"]);
                    eq.Unit = row["Unit"].ToString();
                    eq.Point = Convert.ToByte(row["Point"]);
                    eq.Revise = Convert.ToSingle(row["Revise"]);
                    eq.LowAlert = Convert.ToSingle(row["LowAlert"]);
                    eq.HighAlert = Convert.ToSingle(row["HighAlert"]);
                    eq.Addtime = Convert.ToDateTime(row["Addtime"]);
                    eq.IsAnemoscope = Convert.ToBoolean(row["IsAnemoscope"]);
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
        public static List<byte> GetAddress()
        {
            string sql = string.Format("select Address from tb_Equipment");
            DataSet ds = new DataSet();
            ds = SqliteHelper.Query(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                List<byte> list = new List<byte>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(Convert.ToByte(row["Address"]));
                }
                return list.Distinct().ToList();
            }
            LogLib.Log.GetLogger("EquipmentDal").Warn("获取设备传感器地址列表失败");
            return new List<byte>();
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
