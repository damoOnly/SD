using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;
using System.Data;

namespace Dal
{
    public static class AlertDal
    {
        public static bool AddOne(Alert info)
        {
            string sql = string.Format("insert into tb_Alert (EquipmentID,StartTime,EndTime,AlertName,AlertValue) values ({0},'{1}','{2}','{3}',{4})", info.EquipmentID, info.StartTime.ToString("yyyy/MM/dd HH:mm:ss"), info.EndTime.ToString("yyyy/MM/dd HH:mm:ss"), info.AlertName, info.AlertValue);
            if (SqliteHelper.ExecuteNonQuery(sql) == 1)
            {
                return true;
            }
            else
            {
                LogLib.Log.GetLogger("AlertDal").Warn("插入报警记录失败！");
                return false;
            }
        }

        public static Alert AddOneR(Alert info)
        {
            string sql = string.Format("insert into tb_Alert (EquipmentID,StartTime,EndTime,AlertName,AlertValue) values ({0},'{1}','{2}','{3}',{4})", info.EquipmentID, info.StartTime.ToString("yyyy/MM/dd HH:mm:ss"), info.EndTime.ToString("yyyy/MM/dd HH:mm:ss"), info.AlertName, info.AlertValue);
            if (SqliteHelper.ExecuteNonQuery(sql) == 1)
            {
                string sql2 = string.Format("select ID from tb_Alert where EquipmentID = {0} and AlertName = '{1}' and StartTime = '{2}' limit 0,1", info.EquipmentID, info.AlertName, info.StartTime.ToString("yyyy/MM/dd HH:mm:ss"));
                DataSet ds = SqliteHelper.Query(sql2);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    info.ID = Convert.ToInt64(ds.Tables[0].Rows[0]["ID"]);
                    return info;
                }
                else
                {
                    LogLib.Log.GetLogger("AlertDal").Warn("插入报警记录失败！");
                    return null;
                }
            }
            else
            {
                LogLib.Log.GetLogger("AlertDal").Warn("插入报警记录失败！");
                return null;
            }
        }

        public static bool UpdateOne(Alert info)
        {
            string sql = string.Format("update tb_Alert set EndTime='{0}' where ID={1}", info.EndTime.ToString("yyyy/MM/dd HH:mm:ss"), info.ID);
            if (SqliteHelper.ExecuteNonQuery(sql) == 1)
            {
                return true;
            }
            else
            {
                LogLib.Log.GetLogger("AlertDal").Warn("更新报警记录失败！");
                return false;
            }
        }

        public static bool UpdateOneByStr(int equipmentID, string str, DateTime date)
        {
            string sql2 = string.Format("select ID from tb_Alert where EquipmentID = {0} and AlertName = '{1}' limit 0,1", equipmentID, str);
            DataSet ds = SqliteHelper.Query(sql2);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                long id = Convert.ToInt64(ds.Tables[0].Rows[0]["ID"]);
                string sql = string.Format("update tb_Alert set EndTime='{0}' where ID={1}", date.ToString("yyyy/MM/dd HH:mm:ss"), id);
                if (SqliteHelper.ExecuteNonQuery(sql) == 1)
                {
                    return true;
                }
                else
                {
                    LogLib.Log.GetLogger("AlertDal").Warn("更新报警记录失败！");
                    return false;
                }
            }
            else
            {
                LogLib.Log.GetLogger("AlertDal").Warn("更新报警记录失败！");
                return false;
            }
        }

        public static List<Alert> GetListByTime(DateTime t1,DateTime t2)
        {
            string sql = string.Format("select a.ID, a.EquipmentID,a.StartTime,a.EndTime,a.AlertName,a.AlertValue,b.Address,b.EName,b.Place from tb_Alert a left join tb_Equipment b on a.EquipmentID=b.ID where a.StartTime >= '{0}' and a.EndTime <= '{1}'", t1.ToString("yyyy/MM/dd HH:mm:ss"), t2.ToString("yyyy/MM/dd HH:mm:ss"));
            DataSet ds = new DataSet();
            ds = SqliteHelper.Query(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                List<Alert> list = new List<Alert>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Alert at = new Alert();
                    at.ID = Convert.ToInt64(row["ID"]);
                    at.EquipmentID = Convert.ToInt64(row["EquipmentID"]);
                    at.StartTime = Convert.ToDateTime(row["StartTime"]);
                    at.EndTime = Convert.ToDateTime(row["EndTime"]);
                    at.AlertName = row["AlertName"].ToString();
                    at.AlertValue = Convert.ToSingle(row["AlertValue"]);
                    at.Address = Convert.ToByte(row["Address"]);
                    at.EName = row["EName"].ToString();
                    at.Place = row["Place"].ToString();
                    list.Add(at);
                }
                return list;
            }
            LogLib.Log.GetLogger("AlertDal").Warn("获取报警记录列表失败！");
            return null;
        }

        public static List<Alert> GetListByWH(string wherestr)
        {
            string sql = string.Format("select ID, EquipmentID,StartTime,EndTime,AlertName,AlertValue from tb_Alert where {0}", wherestr);
            DataSet ds = new DataSet();
            ds = SqliteHelper.Query(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                List<Alert> list = new List<Alert>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Alert at = new Alert();
                    at.ID = Convert.ToInt64(row["Address"]);
                    at.EquipmentID = Convert.ToInt64(row["Address"]);
                    at.StartTime = Convert.ToDateTime(row["StartTime"]);
                    at.EndTime = Convert.ToDateTime(row["EndTime"]);
                    at.AlertName = row["AlertName"].ToString();
                    at.AlertValue = Convert.ToSingle(row["AlertValue"]);
                    list.Add(at);
                }
                return list;
            }
            LogLib.Log.GetLogger("AlertDal").Warn("获取报警记录列表失败！");
            return null;
        }

        public static int DeleteByEqID(long eqid)
        {
            string sql = string.Format("delete from tb_Alert where EquipmentID={0}", eqid);
            return SqliteHelper.ExecuteNonQuery(sql);
        }
    }
}
