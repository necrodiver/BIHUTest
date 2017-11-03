using BiHuGadget.Helpers;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuGadget.Dal
{
    public class Base_Dal
    {
        /// <summary>
        /// 查询单条数据，如果有
        /// </summary>
        /// <param name="sql">操作语句</param>
        /// <param name="param">附加操作的内容</param>
        /// <returns></returns>
        protected T GetSingle<T>(string sql, DynamicParameters param) where T : class, new()
        {
            try
            {
                using (IDbConnection conn = GetOpenConnection())
                {
                    var tModel = conn.QueryFirstOrDefault<T>(sql, param);
                    return tModel;
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("查询GetSingle：" + ex.ToString());
            }
            return null;

        }
        /// <summary>
        /// 查询语句，适用于自定义操作
        /// </summary>
        /// <typeparam name="T">模型类</typeparam>
        /// <param name="sql">查询语句</param>
        /// <param name="param">附带条件内容</param>
        protected List<T> GetList<T>(string sql, DynamicParameters param) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();
                using (IDbConnection conn = GetOpenConnection())
                {
                    IEnumerable<T> models = conn.Query<T>(sql, param);
                    list = models as List<T>;
                }
                return list;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("查询GetList：" + ex.ToString());
            }
            return null;

        }

        /// <summary>
        /// 增删改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        protected bool Operate<T>(string sql, T ob)
        {
            try
            {
                using (IDbConnection conn = GetOpenConnection())
                {
                    return conn.Execute(sql, ob) >= 0;
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("查询Operate：" + ex.ToString());
            }
            return false;

        }
        /// <summary>
        /// 增删改,只适用于sql
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns></returns>
        protected bool Operate(string sql)
        {
            try
            {
                using (IDbConnection conn = GetOpenConnection())
                {
                    return conn.Execute(sql) >= 0;
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("查询Operate：" + ex.ToString());
            }
            return false;
        }

        /// <summary>
        /// 查找数据的第一行第一列的内容
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public string GetScaler(string sql, DynamicParameters param)
        {
            using (IDbConnection conn = GetOpenConnection())
            {
                return conn.ExecuteScalar(sql, param).ToString();
            }
        }

        /// <summary>
        /// 调用存储过程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="spName">存储过程名字</param>
        /// <param name="dp">参数</param>
        /// <param name="buffered">是否缓冲（默认是）</param>
        /// <returns></returns>
        public List<T> GetListSP<T>(string spName, object dp, bool buffered = true) where T : class, new()
        {
            List<T> list = new List<T>();
            using (IDbConnection conn = GetOpenConnection())
            {
                IEnumerable<T> models = conn.Query<T>(spName, dp, null, buffered, null, CommandType.StoredProcedure);
                list = models as List<T>;
            }
            return list;
        }
        /// <summary>
        /// 处理链接
        /// </summary>
        /// <returns></returns>
        protected static IDbConnection GetOpenConnection()
        {
            if (Settings.IsSQLite)
            {
                SQLiteConnection conn = new SQLiteConnection(Settings.SqliteConnection);
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                return conn;
            }
            else
            {
                MySqlConnection conn = new MySqlConnection(Settings.MysqlConnection);
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                return conn;
            }
        }
    }
}
