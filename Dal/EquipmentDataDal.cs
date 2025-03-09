using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;
using System.Data;
using System.IO;
using System.Data.SQLite;
using System.Diagnostics;

namespace Dal
{
    public static class EquipmentDataDal
    {
        private static readonly string dbPathTemp = string.Format(@"{0}data\{{0}}\", AppDomain.CurrentDomain.BaseDirectory);
        public const string connectionStringTemp = "Data Source={0};Version=3;";
        private const string fileNameTemp = "eqData{0}.db3";

        public static string GetConnStr(string fileId, string ds)
        {
            string dbPath = string.Format(dbPathTemp, ds);

            string fileName = dbPath + string.Format(fileNameTemp, fileId);

            string connStr = string.Format(connectionStringTemp, fileName);
            return connStr;
        }

        public static void InitDbFile()
        {
            List<Equipment> mainList = EquipmentDal.GetAllList();

            // 初始化月份文件夹202502
            List<string> list = new List<string> { };
            for (int i = 0; i < 24; i++)
            {
                DateTime dt = new DateTime().AddMonths(i);
                string dicName = Utility.CutOffMillisecond(DateTime.Now).AddMonths(i).ToString("yyyyMM");
                list.Add(dicName);
                string dbPath = string.Format(dbPathTemp, dicName);
                if (!Directory.Exists(dbPath))
                {
                    Directory.CreateDirectory(dbPath);
                }

                for (int j = 0; j < mainList.Count; j++)
                {
                    // 奇数才创建数据库文件，温度和湿度用一个数据库文件
                    if (mainList[j].Address % 2 != 0)
                    {
                        string fileName = dbPath + string.Format(fileNameTemp, mainList[j].Address);
                        if (!File.Exists(fileName))
                        {
                            SQLiteConnection.CreateFile(fileName);
                        }

                        string connStr = string.Format(connectionStringTemp, fileName);
                        using (SQLiteConnection conn = new SQLiteConnection(connStr))
                        {
                            conn.Open();
                            if (!EquipmentDataDal.IsTableExist(conn))
                            {
                                EquipmentDataDal.CreateTable(conn);
                            }
                        }
                        
                    }
                }
            }


        }

        public static void CreateTable(SQLiteConnection conn)
        {
            string sql = string.Format(@"create table tb_EquipmentData (ID INTEGER PRIMARY KEY  AUTOINCREMENT,
                                                           Temperature REAL NOT NULL,
                                                           Humidity REAL NOT NULL,
                                                           AddTime INTEGER NOT NULL
                )");
            using (SQLiteCommand command = new SQLiteCommand(sql, conn))
            {
                command.ExecuteNonQuery();
            }
        }

        public static bool IsTableExist(SQLiteConnection conn)
        {
            string sql = string.Format(@"SELECT COUNT(*) FROM sqlite_master where type='table' and name='tb_EquipmentData'");
            using (SQLiteCommand command = new SQLiteCommand(sql, conn))
            {
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public static bool IsFileExist(string date, string fileId)
        {
            string dbPath = string.Format(dbPathTemp, date);
            string fileName = dbPath + string.Format(fileNameTemp, fileId);

            return File.Exists(fileName);
        }

        public static bool Add(EquipmentData ed, SQLiteConnection conn, SQLiteTransaction trans)
        {
            string sql = string.Format("insert into [tb_EquipmentData] (Temperature,Humidity,AddTime) values (@Temperature,@Humidity, @addTime)");

            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn, trans))
            {
                cmd.Parameters.AddWithValue("@Temperature", ed.temperature);
                cmd.Parameters.AddWithValue("@Humidity", ed.humidity);
                cmd.Parameters.AddWithValue("@addTime", ed.AddTime);

                Trace.WriteLine(ed.AddTime);

                return cmd.ExecuteNonQuery() == 1;
            }

        }

        public static void AddOne(EquipmentData ed, string fileId)
        {
            if (ed == null)
            {
                return;
            }

            string ds = DateTime.Now.ToString("yyyyMM");
            string connStr = GetConnStr(fileId, ds);

            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                conn.Open();
                SQLiteTransaction tran = conn.BeginTransaction();
                EquipmentDataDal.Add(ed, conn, tran);
                tran.Commit();
            }
        }

        public static void AddList(List<EquipmentData> list,string fileId)
        {
            if (list == null || list.Count <= 0)
            {
                return;
            }

            string ds = DateTime.Now.ToString("yyyyMM");
            string connStr = GetConnStr(fileId, ds);

            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                conn.Open();
                SQLiteTransaction tran = conn.BeginTransaction();
                foreach (var item in list)
                {
                    EquipmentDataDal.Add(item, conn, tran);
                }
                tran.Commit();
            }
        }

        public static List<EquipmentData> GetList(SQLiteConnection conn, DateTime start, DateTime end)
        {
            string sql = string.Format("select a.Temperature,a.Humidity, a.AddTime from tb_EquipmentData a where a.AddTime >= @start and a.AddTime <= @end");

            List<EquipmentData> list = new List<EquipmentData>();
            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@start", start);
                cmd.Parameters.AddWithValue("@end", end);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EquipmentData eq = new EquipmentData();
                        eq.temperature = Convert.ToSingle(Math.Round(reader.GetFloat(0), 1));
                        eq.humidity = Convert.ToSingle(Math.Round(reader.GetFloat(1), 0));
                        eq.AddTime = Utility.CutOffMillisecond(reader.GetDateTime(2));
                        list.Add(eq);
                    }
                }

                return list;
            }

        }

        public static List<EquipmentData> GetListByMonth(string year, string month, string fileId)
        {
            string connStr = GetConnStr(fileId, year + month);
            List<EquipmentData> list = new List<EquipmentData>();
            DateTime end = new DateTime(int.Parse(year), int.Parse(month), 1);
            DateTime start = end.AddMonths(-1);
            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                conn.Open();
                list = EquipmentDataDal.GetList(conn, start, end);
            }

            return list;
        }

        public static List<EquipmentData> GetListByTime(string start, string end, string fileId)
        {
            DateTime startTime = DateTime.Parse(start);
            DateTime endTime = DateTime.Parse(end);

            List<DateTime> dateList = new List<DateTime>();
            dateList.Add(endTime.AddMonths(-2));
            dateList.Add(endTime.AddMonths(-1));
            dateList.Add(endTime);

            List<EquipmentData> list = new List<EquipmentData>();

            foreach (var item in dateList)
            {
                string ym = item.ToString("yyyyMM");
                if (!IsFileExist(ym, fileId))
                {
                    continue;
                }
                string connStr = GetConnStr(fileId, ym);
                using (SQLiteConnection conn = new SQLiteConnection(connStr))
                {
                    conn.Open();
                    var ll = EquipmentDataDal.GetList(conn, startTime, endTime);
                    list.AddRange(ll);
                }
            }

            

            return list;
        }
    }
}
