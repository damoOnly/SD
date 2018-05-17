using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Entity;
using System.Data;
namespace Dal
{
    public static class UserInfoDal
    {
        public static bool AddOne(UserInfo info)
        {
            string sql = string.Format("insert into [tb_User] ([Account],[PassWord],[UserName],[Level]) values ('{0}','{1}','{2}',{3})", info.Account,info.PassWord,info.UserName,info.Level);
            if (SqliteHelper.ExecuteNonQuery(sql) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool UpdateOne(UserInfo info)
        {
            string sql = string.Format("update [tb_User] set [Account]='{0}',[PassWord]='{1}',[UserName]='{2}',[Level]={3} where [ID]={4}", info.Account,info.PassWord,info.UserName,(byte)info.Level,info.ID);
            if (SqliteHelper.ExecuteNonQuery(sql) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static UserInfo GetOneByID(int id)
        {
            string sql = string.Format("select ID,Account,PassWord,UserName,[Level] from [tb_User] where ID={0} limit 0,1", id);
            DataSet ds = new DataSet();
            ds = SqliteHelper.Query(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                UserInfo eq = new UserInfo();
                eq.ID = Convert.ToInt64(ds.Tables[0].Rows[0]["ID"]);
                eq.Account = ds.Tables[0].Rows[0]["Account"].ToString();
                eq.PassWord = ds.Tables[0].Rows[0]["PassWord"].ToString();
                eq.UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                eq.Level = (EM_UserType)ds.Tables[0].Rows[0]["Level"];
                return eq;
            }
            return null;
        }
        public static UserInfo GetOneByUser(string account,string password)
        {
            string sql = string.Format("select ID,Account,PassWord,UserName,[Level] from [tb_User] where [Account]='{0}' and [PassWord]='{1}' limit 0,1", account, password);
            DataSet ds = new DataSet();
            ds = SqliteHelper.Query(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                UserInfo eq = new UserInfo();
                eq.ID = Convert.ToInt64(ds.Tables[0].Rows[0]["ID"]);
                eq.Account = ds.Tables[0].Rows[0]["Account"].ToString();
                eq.PassWord = ds.Tables[0].Rows[0]["PassWord"].ToString();
                eq.UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                eq.Level = (EM_UserType)ds.Tables[0].Rows[0]["Level"];
                return eq;
            }
            return null;
        }
    }
}
