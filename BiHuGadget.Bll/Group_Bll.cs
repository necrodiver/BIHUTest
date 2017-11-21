using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuGadget.Models;
using BiHuGadget.Dal;

namespace BiHuGadget.Bll
{
    public class Group_Bll
    {
        private static Group_Dal groupDal = new Group_Dal();
        public List<GroupModel> GetGroupList()
        {
            return groupDal.GetGroupList();
        }
        public bool AddGroup(string groupName)
        {
            return groupDal.AddGroup(groupName);
        }
        public bool DeleteGroup(GroupModel groupModel)
        {
            return groupDal.DeleteGroup(groupModel);
        }
        public bool EditGroup(GroupModel groupModel)
        {
            return groupDal.EditGroupName(groupModel);
        }

        public GroupModel GetSingleGroup(int groupId)
        {
            var groupList= groupDal.GetGroupList();
            var group= groupList.SingleOrDefault(g => g.GroupId == groupId);
            return group;
        }
    }
}
