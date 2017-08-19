using BiHuGadget.Helpers;
using BiHuGadget.Models;
using Dapper;
using System.Data;
using System.Data.SQLite;
using System.Text;

namespace BiHuGadget.Dal
{
    public class User_Dal : Base_Dal
    {
        public UserModel GetSingleUser(UserModel userModel)
        {
            string _where = string.Empty;
            if (_where == null)
                return null;
            var _args = DisposeUserModel(userModel, out _where);
            string _sql = string.Format(User_QueryString.Select_UserSingle, _where);
            return GetSingle<UserModel>(_sql, _args);
        }

        /// <summary>
        /// 处理UserModel，拼接where条件
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="where">返回条件sql语句</param>
        /// <returns></returns>
        private DynamicParameters DisposeUserModel(UserModel userModel, out string where)
        {
            var args = new DynamicParameters();
            if (userModel == null)
            {
                where = null;
                return null;
            }
            StringBuilder sb = new StringBuilder();
            if (userModel.UserId != null)
            {
                sb.Append(" UserId=@UserId AND ");
                args.Add("@UserId", userModel.UserId);
            }
            if (!string.IsNullOrWhiteSpace(userModel.UserName))
            {
                sb.Append(" UserName=@UserName AND ");
                args.Add("@UserName", userModel.UserName);
            }
            if (!string.IsNullOrWhiteSpace(userModel.Pwd))
            {
                sb.Append(" Pwd=@Pwd AND ");
                args.Add("@Pwd", userModel.Pwd);
            }
            if (userModel.CreateTime != null)
            {
                sb.Append(" CreateTime=@CreateTime AND ");
                args.Add("@CreateTime", userModel.CreateTime);
            }
            if (userModel.RoleId != null)
            {
                sb.Append(" RoleId=@RoleId AND ");
                args.Add("@RoleId", userModel.RoleId);
            }
            sb.Append(" 1=1");
            where = sb.ToString();
            return args;
        }
    }
    public static class User_QueryString
    {
        /// <summary>
        /// 查询Users数据库
        /// </summary>
        public static string Select_UserSingle = "SELECT * FROM Users WHERE {0}";
    }
}
