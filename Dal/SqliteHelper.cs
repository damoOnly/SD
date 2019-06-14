using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Configuration;
using System.Data.Sql;
using System.Linq;
using System.ComponentModel;
using System.Data.SQLite;
using System.Globalization;


namespace Dal
{
    /// <summary>
    /// Copyright (C) 2004-2008 LiTianPing 
    /// 数据访问基础类(基于OleDb)
    /// 可以用户可以修改满足自己项目的需要。
    /// </summary>
    public class SqliteHelper
    {
        public static string connectionString = string.Empty;
        private static SQLiteConnection connection = null;
        public SqliteHelper()
        {
        }

        public static void SetConnectionString(string _connectionString)
        {
            try
            {
                connectionString = _connectionString;
                connection = new SQLiteConnection(_connectionString);
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger("SqliteHelper").Warn(ex);
            }
        }

        //public static void SetConnectionString(string[] _param)
        //{
        //    try
        //    {
        //        connectionString = String.Format("Provider=SQLOLEDB.1;Persist Security Info=false;Server={0};Database={1};Uid={2};Pwd={3}", _param[0], _param[1], _param[2], _param[3]);
        //        connection = new SQLiteConnection(connectionString);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogLib.Log.GetLogger("SqliteHelper").Warn(ex);
        //    }
        //}



        #region 不带参数的执行

        public static int ExecuteInsert(string sql)
        {
            return ExecuteInsert(sql, null);
        }
        public static int ExecuteNonQuery(string sql)
        {
            return ExecuteSql(sql, null);
        }
        public static int ExecuteScalar(string sql)
        {
            return ExecuteScalar(sql, null);
        }
        public static void ExecuteTrans(List<string> sqlList)
        {
            ExecuteTrans(sqlList, null);
        }
        public static object GetSingle(string SQLString)
        {
            if (connection == null)
            {
                connection = new SQLiteConnection(connectionString);
            }
            SQLiteCommand cmd = new SQLiteCommand(SQLString, connection);
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                object obj = cmd.ExecuteScalar();
                //connection.Close();
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                {
                    return null;
                }
                else
                {
                    return obj;
                }
            }
            catch (System.Data.OleDb.OleDbException e)
            {
                LogLib.Log.GetLogger("SqliteHelper").Warn(e);
                throw new Exception(e.Message);
                
            }
        }
        public static DataSet Query(string sql)
        {
            return Query(sql, null);
        }
        public static SQLiteDataReader ExecuteReader(string sql)
        {
            if (connection == null)
            {
                connection = new SQLiteConnection(connectionString);
            }

            SQLiteCommand cmd = new SQLiteCommand(sql, connection);
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);

            }
            catch (Exception e)
            {
                LogLib.Log.GetLogger("SqliteHelper").Warn(e);
                return null;
            }
        }
        #endregion


        #region 带参数的执行
        //执行单条插入语句，并返回id，不需要返回id的用ExceuteNonQuery执行。
        public static int ExecuteInsert(string sql, SQLiteParameter[] parameters)
        {
            if (connection == null)
            {
                connection = new SQLiteConnection(connectionString);
            }

            SQLiteCommand cmd = new SQLiteCommand(sql, connection);
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                cmd.CommandText = @"select last_insert_rowid()";
                int value = Int32.Parse(cmd.ExecuteScalar().ToString());
                //connection.Close();
                ////connection.Close();
                return value;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //执行带参数的sql语句,返回影响的记录数（insert,update,delete)
        public static int ExecuteSql(string sql, SQLiteParameter[] parameters)
        {
            if (connection == null)
            {
                connection = new SQLiteConnection(connectionString);
            }

            SQLiteCommand cmd = new SQLiteCommand(sql, connection);
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                int rows = cmd.ExecuteNonQuery();
                //connection.Close();
                return rows;
            }
            catch(Exception e)
            {
                LogLib.Log.GetLogger("SqliteHelper").Warn(e);
                return 0;
            }
        }

        //执行单条语句返回第一行第一列,可以用来返回count(*)
        public static int ExecuteScalar(string sql, SQLiteParameter[] parameters)
        {
            if (connection == null)
            {
                connection = new SQLiteConnection(connectionString);
            }

            SQLiteCommand cmd = new SQLiteCommand(sql, connection);
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                int value = Int32.Parse(cmd.ExecuteScalar().ToString());

                //connection.Close();
                return value;
            }
            catch (Exception e)
            {
                LogLib.Log.GetLogger("SqliteHelper").Warn(e);
                return 0;
            }

        }
        //执行事务
        public static void ExecuteTrans(List<string> sqlList, List<SQLiteParameter[]> paraList)
        {
            if (connection == null)
            {
                connection = new SQLiteConnection(connectionString);
            }
            {
                SQLiteCommand cmd = new SQLiteCommand();
                SQLiteTransaction transaction = null;
                cmd.Connection = connection;
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    transaction = connection.BeginTransaction() as SQLiteTransaction;
                    cmd.Transaction = transaction;

                    for (int i = 0; i < sqlList.Count; i++)
                    {
                        cmd.CommandText = sqlList[i];
                        if (paraList != null && paraList[i] != null)
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddRange(paraList[i]);
                        }
                        cmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                    //connection.Close();
                }
                catch (Exception e)
                {
                    LogLib.Log.GetLogger("SqliteHelper").Warn(e);
                    try
                    {
                        transaction.Rollback();
                    }
                    catch
                    {

                    }                    
                }

            }
        }


        public static object GetSingle(string SQLString, params SQLiteParameter[] parameters)
        {
            if (connection == null)
            {
                connection = new SQLiteConnection(connectionString);
            }

            SQLiteCommand cmd = new SQLiteCommand(SQLString, connection);
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                object obj = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                //connection.Close();
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                {
                    return null;
                }
                else
                {
                    return obj;
                }
            }
            catch (System.Data.OleDb.OleDbException e)
            {
                LogLib.Log.GetLogger("SqliteHelper").Warn(e);
                throw new Exception(e.Message);
                
            }
        }


        //执行查询语句，返回dataset
        public static DataSet Query(string sql, SQLiteParameter[] parameters) // 连接sql语句 并将数据读入内存DS中
        {
            if (connection == null)
            {
                connection = new SQLiteConnection(connectionString);
            }
            {
                DataSet ds = new DataSet("myset");
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    SQLiteDataAdapter da = new SQLiteDataAdapter(sql, connection);
                    if (parameters != null) da.SelectCommand.Parameters.AddRange(parameters);
                    ds.Locale = CultureInfo.InvariantCulture;
                    da.Fill(ds);
                    //connection.Close();
                }
                catch (Exception ex)
                {
                    LogLib.Log.GetLogger("SqliteHelper").Warn(ex);
                }
                return ds;
            }
        }
        /// <summary>
        /// 初始化数据库,看能否连上
        /// </summary>
        /// <returns></returns>
        public static bool InitDB()
        {
            bool isok = false;
            try
            {
                if (connection == null)
                {
                    connection = new SQLiteConnection(connectionString);
                }
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                //connection.Close();
                isok = true;
            }
            catch (Exception ex)
            {
                LogLib.Log.GetLogger("SqliteHelper").Warn(ex);
                throw ex;
                
            }
            return isok;

        }
        #endregion
    }
}
