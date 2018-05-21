using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Data.Sql;
using System.Linq;
using System.ComponentModel;


namespace Dal
{
    /// <summary>
    /// Copyright (C) 2004-2008 LiTianPing 
    /// ���ݷ��ʻ�����(����OleDb)
    /// �����û������޸������Լ���Ŀ����Ҫ��
    /// </summary>
//    public class DbHelperOleDb
//    {
//#if DEBUG
//        public static string DBPath = "LumiCU.mdb";
//        public static string connectionString;
//        //public static string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + ConfigurationSettings.AppSettings["DBPath"].ToString() + ";";
////        2003:Provider=Microsoft.Jet.OLEDB.4.0;Data Source=XXX.mdb
////        2007:Provider=Microsoft.ACE.OLEDB.12.0;Data Source=XXX.accdb;
//    #else
//        public static string DBPath = System.Environment.CurrentDirectory + @"\LumiCU.mdb";
//        public static string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + DBPath + ";";
//    #endif

//        private static OleDbConnection connection = null;
//        public DbHelperOleDb()
//        {
//        }

//        public static void SetConnectionString(string _connectionString)
//        {
//            try
//            {
//                connectionString = _connectionString;
//                connection = new OleDbConnection(connectionString);
//            }
//            catch (Exception ex)
//            {
//                LogLib.Log.GetLogger("DbHelperOleDb").Warn(ex);
//            }
//        }

//        public static void SetConnectionString(string[] _param)
//        {
//            try
//            {
//                connectionString = String.Format("Provider=SQLOLEDB.1;Persist Security Info=false;Server={0};Database={1};Uid={2};Pwd={3}", _param[0], _param[1], _param[2], _param[3]);
//                connection = new OleDbConnection(connectionString);
//            }
//            catch (Exception ex)
//            {
//                LogLib.Log.GetLogger("DbHelperOleDb").Warn(ex);
//            }
//        }



//        #region ����������ִ��

//        public static int ExecuteInsert(string sql)
//        {
//            return ExecuteInsert(sql, null);
//        }
//        public static int ExecuteNonQuery(string sql)
//        {
//            return ExecuteSql(sql, null);
//        }
//        public static int ExecuteScalar(string sql)
//        {
//            return ExecuteScalar(sql, null);
//        }
//        public static void ExecuteTrans(List<string> sqlList)
//        {
//            ExecuteTrans(sqlList, null);
//        }
//        public static object GetSingle(string SQLString)
//        {
//            if (connection == null)
//            {
//                connection = new OleDbConnection(connectionString);
//            }
//            OleDbCommand cmd = new OleDbCommand(SQLString, connection);
//            try
//            {
//                if (connection.State != ConnectionState.Open)
//                {
//                    connection.Open();
//                }
//                object obj = cmd.ExecuteScalar();
//                //connection.Close();
//                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
//                {
//                    return null;
//                }
//                else
//                {
//                    return obj;
//                }
//            }
//            catch (System.Data.OleDb.OleDbException e)
//            {
//                LogLib.Log.GetLogger("DbHelperOleDb").Warn(e);
//                throw new Exception(e.Message);
                
//            }
//        }
//        public static DataSet Query(string sql)
//        {
//            return Query(sql, null);
//        }
//        public static OleDbDataReader ExecuteReader(string sql)
//        {
//            if (connection == null)
//            {
//                connection = new OleDbConnection(connectionString);
//            }

//            OleDbCommand cmd = new OleDbCommand(sql, connection);
//            try
//            {
//                if (connection.State != ConnectionState.Open)
//                {
//                    connection.Open();
//                }
//                return cmd.ExecuteReader(CommandBehavior.CloseConnection);

//            }
//            catch (Exception e)
//            {
//                LogLib.Log.GetLogger("DbHelperOleDb").Warn(e);
//                return null;
//            }
//        }
//        #endregion


//        #region ��������ִ��
//        //ִ�е���������䣬������id������Ҫ����id����ExceuteNonQueryִ�С�
//        public static int ExecuteInsert(string sql, OleDbParameter[] parameters)
//        {
//            if (connection == null)
//            {
//                connection = new OleDbConnection(connectionString);
//            }

//            OleDbCommand cmd = new OleDbCommand(sql, connection);
//            try
//            {
//                if (connection.State != ConnectionState.Open)
//                {
//                    connection.Open();
//                }
//                if (parameters != null) cmd.Parameters.AddRange(parameters);
//                cmd.ExecuteNonQuery();
//                cmd.Parameters.Clear();
//                cmd.CommandText = @"select @@identity";
//                int value = Int32.Parse(cmd.ExecuteScalar().ToString());
//                //connection.Close();
//                ////connection.Close();
//                return value;
//            }
//            catch (Exception e)
//            {
//                LogLib.Log.GetLogger("DbHelperOleDb").Warn(e);
//                return 0;

//            }
//        }

//        //ִ�д�������sql���,����Ӱ��ļ�¼����insert,update,delete)
//        public static int ExecuteSql(string sql, OleDbParameter[] parameters)
//        {
//            if (connection == null)
//            {
//                connection = new OleDbConnection(connectionString);
//            }

//            OleDbCommand cmd = new OleDbCommand(sql, connection);
//            try
//            {
//                if (connection.State != ConnectionState.Open)
//                {
//                    connection.Open();
//                }
//                if (parameters != null) cmd.Parameters.AddRange(parameters);
//                int rows = cmd.ExecuteNonQuery();
//                //connection.Close();
//                return rows;
//            }
//            catch(Exception e)
//            {
//                LogLib.Log.GetLogger("DbHelperOleDb").Warn(e);
//                return 0;
//            }
//        }

//        //ִ�е�����䷵�ص�һ�е�һ��,������������count(*)
//        public static int ExecuteScalar(string sql, OleDbParameter[] parameters)
//        {
//            if (connection == null)
//            {
//                connection = new OleDbConnection(connectionString);
//            }

//            OleDbCommand cmd = new OleDbCommand(sql, connection);
//            try
//            {
//                if (connection.State != ConnectionState.Open)
//                {
//                    connection.Open();
//                }
//                if (parameters != null) cmd.Parameters.AddRange(parameters);
//                int value = Int32.Parse(cmd.ExecuteScalar().ToString());

//                //connection.Close();
//                return value;
//            }
//            catch (Exception e)
//            {
//                LogLib.Log.GetLogger("DbHelperOleDb").Warn(e);
//                return 0;
//            }

//        }
//        //ִ������
//        public static void ExecuteTrans(List<string> sqlList, List<OleDbParameter[]> paraList)
//        {
//            if (connection == null)
//            {
//                connection = new OleDbConnection(connectionString);
//            }
//            {
//                OleDbCommand cmd = new OleDbCommand();
//                OleDbTransaction transaction = null;
//                cmd.Connection = connection;
//                try
//                {
//                    if (connection.State != ConnectionState.Open)
//                    {
//                        connection.Open();
//                    }
//                    transaction = connection.BeginTransaction();
//                    cmd.Transaction = transaction;

//                    for (int i = 0; i < sqlList.Count; i++)
//                    {
//                        cmd.CommandText = sqlList[i];
//                        if (paraList != null && paraList[i] != null)
//                        {
//                            cmd.Parameters.Clear();
//                            cmd.Parameters.AddRange(paraList[i]);
//                        }
//                        cmd.ExecuteNonQuery();
//                    }
//                    transaction.Commit();
//                    //connection.Close();
//                }
//                catch (Exception e)
//                {
//                    LogLib.Log.GetLogger("DbHelperOleDb").Warn(e);
//                    try
//                    {
//                        transaction.Rollback();
//                    }
//                    catch
//                    {

//                    }                    
//                }

//            }
//        }


//        public static object GetSingle(string SQLString, params OleDbParameter[] parameters)
//        {
//            if (connection == null)
//            {
//                connection = new OleDbConnection(connectionString);
//            }

//            OleDbCommand cmd = new OleDbCommand(SQLString, connection);
//            try
//            {
//                if (connection.State != ConnectionState.Open)
//                {
//                    connection.Open();
//                }
//                if (parameters != null) cmd.Parameters.AddRange(parameters);
//                object obj = cmd.ExecuteScalar();
//                cmd.Parameters.Clear();
//                //connection.Close();
//                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
//                {
//                    return null;
//                }
//                else
//                {
//                    return obj;
//                }
//            }
//            catch (System.Data.OleDb.OleDbException e)
//            {
//                LogLib.Log.GetLogger("DbHelperOleDb").Warn(e);
//                throw new Exception(e.Message);
                
//            }
//        }


//        //ִ�в�ѯ��䣬����dataset
//        public static DataSet Query(string sql, OleDbParameter[] parameters) // ����sql��� �������ݶ����ڴ�DS��
//        {
//            if (connection == null)
//            {
//                connection = new OleDbConnection(connectionString);
//            }
//            {
//                DataSet ds = new DataSet();
//                try
//                {
//                    if (connection.State != ConnectionState.Open)
//                    {
//                        connection.Open();
//                    }

//                    OleDbDataAdapter da = new OleDbDataAdapter(sql, connection);
//                    if (parameters != null) da.SelectCommand.Parameters.AddRange(parameters);
//                    da.Fill(ds, "ds");
//                    //connection.Close();
//                }
//                catch (Exception ex)
//                {
//                    LogLib.Log.GetLogger("DbHelperOleDb").Warn(ex);
//                }
//                return ds;
//            }
//        }
//        /// <summary>
//        /// ��ʼ�����ݿ�,���ܷ�����
//        /// </summary>
//        /// <returns></returns>
//        public static bool InitDB()
//        {
//            bool isok = false;
//            try
//            {
//                if (connection == null)
//                {
//                    connection = new OleDbConnection(connectionString);
//                }
//                if (connection.State != ConnectionState.Open)
//                {
//                    connection.Open();
//                }
//                //connection.Close();
//                isok = true;
//            }
//            catch (Exception ex)
//            {
//                LogLib.Log.GetLogger("DbHelperOleDb").Warn(ex);
//                throw ex;
                
//            }
//            return isok;

//        }
//        #endregion
//    }
}