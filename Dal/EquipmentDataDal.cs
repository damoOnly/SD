using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;
using System.Data;

namespace Dal
{
    public static class EquipmentDataDal
    {
        public static bool AddOne(EquipmentData ed)
        {
            string sql = string.Format("insert into [tb_Chroma] (EquipmentID,Chroma,AddTime) values ({0},{1},'{2}')", ed.EquipmentID, ed.Chroma, ed.AddTime.ToString("yyyy/MM/dd HH:mm:ss"));
            if (SqliteHelper.ExecuteNonQuery(sql) == 1)
            {
                return true;
            }
            else
            {
                LogLib.Log.GetLogger("EquipmentDataDal").Warn("添加浓度数据失败");
                return false;
            }
        }

        public static List<EquipmentData> GetListByTime(long equipmentID,DateTime dt1,DateTime dt2)
        {
            //dt2 = dt2.AddDays(1);
            string sql = string.Format("select a.EquipmentID,a.Chroma,a.AddTime,b.Unit,b.EName from [tb_Chroma] a left join [tb_Equipment] b on a.EquipmentID=b.ID where EquipmentID={0} and a.AddTime >='{1}' and a.AddTime <='{2}'", equipmentID, dt1.ToString("yyyy/MM/dd HH:mm:ss"), dt2.ToString("yyyy/MM/dd HH:mm:ss"));
            DataSet ds = new DataSet();
            ds = SqliteHelper.Query(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                List<EquipmentData> list = new List<EquipmentData>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    EquipmentData eq = new EquipmentData();
                    eq.EquipmentID = Convert.ToInt64(row["EquipmentID"]);
                    eq.Chroma = Convert.ToSingle(row["Chroma"]);
                    eq.AddTime = Convert.ToDateTime(row["AddTime"]);
                    eq.Unit = row["Unit"].ToString();
                    eq.EName = row["EName"].ToString();
                    list.Add(eq);
                }
                return list;
            }
            LogLib.Log.GetLogger("EquipmentDataDal").Warn("获取浓度数据失败");
            return null;
        }

        public static int DeleteByTime(long equipmentID, DateTime dt1, DateTime dt2)
        {
            string sql = string.Format("delete from [tb_Chroma] where EquipmentID={0} and AddTime >='{1}' and AddTime <='{2}'", equipmentID, dt1.ToString("yyyy/MM/dd HH:mm:ss"), dt2.ToString("yyyy/MM/dd HH:mm:ss"));
            return SqliteHelper.ExecuteNonQuery(sql);
        }


        public static int DeleteAll()
        {
            string sql = "delete from [tb_Chroma]";
            return SqliteHelper.ExecuteNonQuery(sql);
        }

        public static int DeleteByEqID(long eqid)
        {
            string sql = string.Format("delete from [tb_Chroma] where EquipmentID={0}", eqid);
            return SqliteHelper.ExecuteNonQuery(sql);
        }
    }
}
