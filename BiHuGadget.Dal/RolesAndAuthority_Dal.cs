using BiHuGadget.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuGadget.Dal
{
    public class RolesAndAuthority_Dal : Base_Dal
    {
        public RoleModel GetSingleRole(int roleId = 0)
        {
            string sql = string.Format(RA_QueryString.Select_Role, " RoleId=@RoleId ");
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@RoleId", roleId);
            return this.GetSingle<RoleModel>(sql, dp);
        }

        public List<AuthorityModel> GetAuthorityList(int roleId = 0)
        {
            string sql = string.Format(RA_QueryString.Select_Authroity, " RoleId=@RoleId ");
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@RoleId", roleId);
            return this.GetList<AuthorityModel>(sql, dp);
        }

        public static class RA_QueryString
        {
            /// <summary>
            /// 查询Users数据库
            /// </summary>
            public static string Select_Role = "SELECT * FROM Roles WHERE {0}";
            public static string Select_Authroity = "SELECT * FROM Authority WHERE {0}";
        }
    }
}
