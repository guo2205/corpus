using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace corpus
{
    [Obsolete("不要用")]
    public class MySql : IDisposable
    {

        string mysqlHost = "zhinengjiaju-db.chinacloudapp.cn";
        string mysqlPort = "3306";
        string mysqlDB = "ZhiNengJiaJu";
        string mysqlUser = "root";
        string mysqlPassword = "mypassword";
        MySqlConnection conn;
        MySqlCommand comm;
        public MySql()
        {

            conn = new MySqlConnection();
            string connString = null;
            try
            {
                connString = "Database = " + mysqlDB + "; Data Source = " + mysqlHost + "; User Id = " + mysqlUser + "; Password = " + mysqlPassword + "; pooling=true;min pool size=1;max pool size=50; Connection Timeout = 30; Command Timeout = 30; CharSet = utf8; port = " + mysqlPort;
                conn.ConnectionString = connString;
                conn.Open();
            }
            catch
            {


                connString = "Database = " + mysqlDB + "; Data Source = " + mysqlHost + "; User Id = " + mysqlUser + "; Password = " + mysqlPassword + "; pooling=true;min pool size=1;max pool size=50; Connect Timeout = 2; CharSet = utf8; port = " + mysqlPort;
                conn.ConnectionString = connString;
                conn.Open();
            }
            comm = conn.CreateCommand();
            comm.CommandTimeout = 30;

        }
        public DataSet GetDataSet(string sql, string table)
        {
            MySqlDataAdapter mda = new MySqlDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            mda.Fill(ds, table);
            return ds;
        }
        /// <summary>
        ///  保存一段字符串
        /// </summary>
        public long AddSqlData(string sql)
        {
            comm.CommandText = sql;
            try
            {
                if (comm.ExecuteNonQuery() > 0)
                {
                    return 1;
                    //return comm.LastInsertedId;
                }
                else
                    return -1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        /// <summary>
        ///  更新
        /// </summary>
        public int UpdateSqlData(string sql)
        {
            comm.CommandText = sql;
            try
            {
                return comm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return 0;
            }
        }



        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int DelSqlData(string sql)
        {
            comm.CommandText = sql;
            return comm.ExecuteNonQuery();
        }

        public bool SelectSqlData(string sql)
        {
            bool flg = false;
            comm.CommandText = sql;
            MySqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    //if (int.Parse(reader[0].ToString()) > 0)
                    {
                        flg = true;
                    }
                }
            }
            reader.Close();
            return flg;
        }

        /// <summary>
        /// 专门用来查count的
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool SelectSqlDataByCount(string sql)
        {
            bool flg = false;
            comm.CommandText = sql;
            MySqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    if (int.Parse(reader[0].ToString()) > 0)
                    {
                        flg = true;
                    }
                }
            }
            reader.Close();
            return flg;
        }
        /// <summary>
        /// 查找数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string GetSqlData(string sql, string _value)
        {
            string str = "0";
            comm.CommandText = sql;
            MySqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if (reader.GetName(i) == _value)
                            str = reader[i].ToString();
                    }
                }
            }
            reader.Close();
            return str;
        }

        public string[][] GetSqlListData(string sql, params string[] _value)
        {
            List<string> list = new List<string>();
            List<string[]> llist = new List<string[]>();
            comm.CommandText = sql;
            MySqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    list.Clear();
                    for (int n = 0; n < _value.Length; n++)
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (reader.GetName(i) == _value[n])
                                list.Add(reader[i].ToString());
                        }
                        string[] ss = list.ToArray();
                    }
                    llist.Add(list.ToArray());
                }

            }
            reader.Close();
            string[][] sss = llist.ToArray();
            return llist.ToArray();
        }

        /// <summary>
        /// 获得数据库所有数据
        /// </summary>
        /// <returns></returns>
        public List<List<string>> GetAllSqlData(string sql)
        {
            List<List<string>> list = new List<List<string>>();
            comm.CommandText = sql;
            MySqlDataReader reader = comm.ExecuteReader();

            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    List<string> list1 = new List<string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        list1.Add(reader[i].ToString());
                    }
                    list.Add(list1);
                }
            }

            reader.Close();
            return list;
        }


        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Dispose()
        {
            comm.Dispose();
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }
    }
}