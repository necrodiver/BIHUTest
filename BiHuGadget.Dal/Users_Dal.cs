using BiHuGadget.Helpers;
using BiHuGadget.Models;
using Dapper;
using System.Data;
using System.Data.SQLite;
using System.Text;
using System;
using System.Collections.Generic;

namespace BiHuGadget.Dal
{
    public class User_Dal : Base_Dal
    {
        public UserModel GetSingleUser(UserModel userModel)
        {
            string _where = string.Empty;
            var _args = DisposeUserModel(userModel, true, out _where);
            string _sql = string.Format(User_QueryString.Select_User, _where);
            return GetSingle<UserModel>(_sql, _args);
        }
        public List<UserModel> GetListUser(UserModel userModel = null)
        {
            if (userModel == null)
                userModel = new UserModel();
            string _where = string.Empty;
            var _args = DisposeUserModel(userModel, true, out _where);
            string _sql = string.Format(User_QueryString.Select_User, _where);
            return GetList<UserModel>(_sql, _args);
        }

        /// <summary>
        /// 批量添加用户账号
        /// </summary>
        /// <param name="userList"></param>
        /// <returns></returns>
        public bool AddUserList(List<UserModel> userList)
        {
            if (userList == null)
                return false;
            StringBuilder addStr = new StringBuilder();
            foreach (var item in userList)
            {
                addStr.Append($"('{item.Email}','{item.UserName}','{item.Pwd}','{item.CreateTime}',{item.RoleId},{item.GroupId}),");
            }
            string _values = addStr.ToString();
            _values = _values.Substring(0, _values.Length - 1);
            string _sql = string.Format(User_QueryString.Insert_UserList, _values);
            return Operate(_sql);
        }
        /// <summary>
        /// 批量删除用户账号
        /// </summary>
        /// <param name="userIdList"></param>
        /// <returns></returns>
        public bool DeleteUserList(List<string> userIdList)
        {
            if (userIdList == null)
                return false;
            StringBuilder sb = new StringBuilder();
            foreach (var item in userIdList)
            {
                sb.Append($"{item},");
            }
            string _where = sb.ToString();
            _where = _where.Substring(0, _where.Length - 1);
            string _sql = string.Format(User_QueryString.Delete_User, _where);
            return Operate(_sql);
        }

        public bool EditUser(UserModel userModel)
        {
            if (userModel == null)
                return false;
            string _value = string.Empty;
            var _args = DisposeUserModel(userModel, false, out _value);
            string _sql = string.Format(User_QueryString.Update_User, _value, " UserId=@UserId");
            return Operate(_sql, _args);
        }

        /// <summary>
        /// 处理UserModel，拼接where条件
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="where">返回条件sql语句</param>
        /// <returns></returns>
        private DynamicParameters DisposeUserModel(UserModel userModel, bool isSelect, out string where)
        {
            var args = new DynamicParameters();
            if (userModel == null)
            {
                where = null;
                return null;
            }
            StringBuilder sb = new StringBuilder();
            StringBuilder sbUp = new StringBuilder();
            if (userModel.UserId != null)
            {
                sb.Append(" UserId=@UserId AND ");
                args.Add("@UserId", userModel.UserId);
            }
            if (!string.IsNullOrWhiteSpace(userModel.UserName))
            {
                sb.Append(" UserName=@UserName AND ");
                sbUp.Append(" UserName=@UserName,");
                args.Add("@UserName", userModel.UserName);
            }
            if (!string.IsNullOrWhiteSpace(userModel.Pwd))
            {
                sb.Append(" Pwd=@Pwd AND ");
                sbUp.Append(" Pwd=@Pwd,");
                args.Add("@Pwd", userModel.Pwd);
            }
            if (!string.IsNullOrWhiteSpace(userModel.Email))
            {
                sb.Append(" Email=@Email AND ");
                sbUp.Append(" Email=@Email,");
                args.Add("@Email", userModel.Email);
            }
            if (userModel.CreateTime != null)
            {
                sb.Append(" CreateTime=@CreateTime AND ");
                args.Add("@CreateTime", userModel.CreateTime);
            }
            if (userModel.RoleId != null)
            {
                sb.Append(" RoleId=@RoleId AND ");
                sbUp.Append(" RoleId=@RoleId,");
                args.Add("@RoleId", userModel.RoleId);
            }
            if (userModel.GroupId != null)
            {
                sb.Append(" GroupId=@GroupId AND ");
                sbUp.Append(" GroupId=@GroupId,");
                args.Add("@GroupId", userModel.GroupId);
            }
            sb.Append(" 1=1");
            if (isSelect)
            {
                where = sb.ToString();
            }
            else
            {
                where = sbUp.ToString().Substring(0, sbUp.ToString().Length - 1);
            }
            return args;
        }
    }
    public static class User_QueryString
    {
        /// <summary>
        /// 查询Users数据库
        /// </summary>
        public static string Select_User = "SELECT * FROM Users WHERE {0}";
        public static string Insert_User = "INSERT INTO Users (Email,UserName,Pwd,CreateTime,RoleId,GroupId) VALUES (@Email,@UserName,@Pwd,@CreateTime,@RoleId,@GroupId)";
        public static string Insert_UserList = "INSERT INTO Users (Email,UserName,Pwd,CreateTime,RoleId,GroupId) VALUES {0}";
        public static string Delete_User = "DELETE FROM Users WHERE UserId IN ({0})";
        public static string Update_User = "UPDATE users SET {0} WHERE {1}";
    }
}
