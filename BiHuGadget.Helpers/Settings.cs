using System;
using static System.Configuration.ConfigurationManager;

namespace BiHuGadget.Helpers
{
    /// <summary>
    /// 基本配置文件集合
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Sqlite数据库链接
        /// </summary>
        public static readonly string SqliteConnection = ConnectionStrings["BiHiDB_Sqlite"].ToString();
        /// <summary>
        /// MySql数据库链接
        /// </summary>
        public static readonly string MysqlConnection = ConnectionStrings["BiHiDB_MySql"].ToString();
        /// <summary>
        /// 是否是Sqlite链接方式
        /// </summary>
        public static bool IsSQLite
        {
            get
            {
                string isSqliteStr = Convert.ToString(AppSettings["IsSQLite"]);
                if (string.IsNullOrWhiteSpace(isSqliteStr))
                    return false;
                if (isSqliteStr.Equals("1"))
                    return true;
                return false;
            }
        }
    }
}
