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
    public static class EquipmentDataDalAverage
    {
        private static readonly string dbPathTemp = string.Format(@"{0}data\{{0}}\", AppDomain.CurrentDomain.BaseDirectory);
        public const string connectionStringTemp = "Data Source={0};Version=3;";
        private const string fileNameTemp = "eqDataAverage{0}.db3";

        public static string GetConnStr(string roomId, string ds)
        {
            string dbPath = string.Format(dbPathTemp, ds);

            string fileName = dbPath + string.Format(fileNameTemp, roomId);

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
                    // 1个房间12个设备，当地址被12整除的时候，就创建一个平局值数据库
                    if (mainList[j].Address % 12 == 0)
                    {
                        //房间号
                        int roomId = (int)Math.Ceiling((double)(mainList[j].Address / 12.00));
                        string fileName = dbPath + string.Format(fileNameTemp, roomId);
                        if (!File.Exists(fileName))
                        {
                            SQLiteConnection.CreateFile(fileName);
                        }

                        string connStr = string.Format(connectionStringTemp, fileName);
                        using (SQLiteConnection conn = new SQLiteConnection(connStr))
                        {
                            conn.Open();
                            if (!EquipmentDataDalAverage.IsTableExist(conn))
                            {
                                EquipmentDataDalAverage.CreateTable(conn);
                            }
                        }
                        
                    }
                }
            }


        }

        public static void CreateTable(SQLiteConnection conn)
        {
            string sql = string.Format(@"create table tb_DataAverage (ID INTEGER PRIMARY KEY  AUTOINCREMENT,
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
            string sql = string.Format(@"SELECT COUNT(*) FROM sqlite_master where type='table' and name='tb_DataAverage'");
            using (SQLiteCommand command = new SQLiteCommand(sql, conn))
            {
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public static bool Add(EquipmentData ed, SQLiteConnection conn, SQLiteTransaction trans)
        {
            string sql = string.Format("insert into [tb_DataAverage] (Temperature,Humidity,AddTime) values (@Temperature,@Humidity, @addTime)");

            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn, trans))
            {
                cmd.Parameters.AddWithValue("@Temperature", ed.temperature);
                cmd.Parameters.AddWithValue("@Humidity", ed.humidity);
                cmd.Parameters.AddWithValue("@addTime", ed.AddTime);

                Trace.WriteLine(ed.AddTime);

                return cmd.ExecuteNonQuery() == 1;
            }

        }

        public static void AddOne(EquipmentData ed, string roomId)
        {
            if (ed == null)
            {
                return;
            }

            string ds = DateTime.Now.ToString("yyyyMM");
            string connStr = GetConnStr(roomId, ds);

            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                conn.Open();
                SQLiteTransaction tran = conn.BeginTransaction();
                EquipmentDataDalAverage.Add(ed, conn, tran);
                tran.Commit();
            }
        }

        public static void AddList(List<EquipmentData> list, string roomId)
        {
            if (list == null || list.Count <= 0)
            {
                return;
            }

            string ds = DateTime.Now.ToString("yyyyMM");
            string connStr = GetConnStr(roomId, ds);

            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                conn.Open();
                SQLiteTransaction tran = conn.BeginTransaction();
                foreach (var item in list)
                {
                    EquipmentDataDalAverage.Add(item, conn, tran);
                }
                tran.Commit();
            }
        }

        public static List<EquipmentData> GetList(SQLiteConnection conn)
        {
            string sql = string.Format("select a.Temperature,a.Humidity, a.AddTime from tb_DataAverage a");

            List<EquipmentData> list = new List<EquipmentData>();
            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EquipmentData eq = new EquipmentData();
                        eq.temperature = Convert.ToSingle(Math.Round(reader.GetFloat(0), 0));
                        eq.humidity = Convert.ToSingle(Math.Round(reader.GetFloat(1), 1));
                        eq.AddTime = Utility.CutOffMillisecond(reader.GetDateTime(2));
                        list.Add(eq);
                    }
                }

                return list;
            }

        }

        public static List<EquipmentData> GetListByMonth(string year, string month, string roomId)
        {
            string connStr = GetConnStr(roomId, year + month);
            List<EquipmentData> list = new List<EquipmentData>();
            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                conn.Open();
                list = EquipmentDataDalAverage.GetList(conn);
            }

            return list;
        }
    }
}
