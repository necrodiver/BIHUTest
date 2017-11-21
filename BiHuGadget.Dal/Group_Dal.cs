using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuGadget.Models;
using Dapper;

namespace BiHuGadget.Dal
{
    public class Group_Dal : Base_Dal
    {
        /// <summary>
        /// 获取分组列表
        /// </summary>
        /// <returns></returns>
        public List<GroupModel> GetGroupList()
        {
            string _sql = string.Format(Group_QueryString.Select_Group, "1=1");
            return GetList<GroupModel>(_sql, null);
        }
        /// <summary>
        /// 添加单个组
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public bool AddGroup(string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName))
                return false;
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@GroupName", groupName);
            string _sql = string.Format(Group_QueryString.Insert_Group);
            return Operate(_sql, dp);
        }
        public bool DeleteGroup(GroupModel groupModel)
        {
            if (groupModel == null || (groupModel.GroupId == null && string.IsNullOrWhiteSpace(groupModel.GroupName)))
                return false;
            StringBuilder _sbwhere = new StringBuilder();
            DynamicParameters dp = new DynamicParameters();
            if (groupModel.GroupId != null)
            {
                dp.Add("@GroupId", groupModel.GroupId);
                _sbwhere.Append(" GroupId=@GroupId AND");
            }
            else if (!string.IsNullOrWhiteSpace(groupModel.GroupName))
            {
                dp.Add("@GroupName", groupModel.GroupName);
                _sbwhere.Append(" GroupName=@GroupName AND");
            }
            _sbwhere.Append(" 1=1 ");
            string _sql = string.Format(Group_QueryString.Delete_Group, _sbwhere.ToString());
            return Operate(_sql);
        }
        public bool EditGroupName(GroupModel groupModel)
        {
            if (groupModel == null || groupModel.GroupId == null || string.IsNullOrWhiteSpace(groupModel.GroupName))
                return false;
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@GroupId", groupModel.GroupId);
            dp.Add("@GroupName", groupModel.GroupName);
            return Operate(Group_QueryString.Update_Group, dp);
        }
    }

    public static class Group_QueryString
    {
        public static string Select_Group = "SELECT * FROM companygroup WHERE {0}";
        public static string Insert_Group = "INSERT INTO companygroup (GroupName) values (@GroupName)";
        public static string Delete_Group = "DELETE FROM companygroup WHERE {0}";
        public static string Update_Group = "UPDATE users SET GroupName=@GroupName WHERE GroupId=@GroupId";
    }
}
