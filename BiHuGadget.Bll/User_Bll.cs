using BiHuGadget.Dal;
using BiHuGadget.Helpers;
using BiHuGadget.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuGadget.Bll
{
    public class User_Bll
    {
        User_Dal userDal = new User_Dal();
        public UserModel GetSingleUser(UserModel userModel)
        {
            if (userModel == null)
                return null;
            if (!string.IsNullOrWhiteSpace(userModel.Pwd))
                userModel.Pwd = AESHelper.AESEncrypt(userModel.Pwd);
            return userDal.GetSingleUser(userModel);
        }
        public List<UserModel> GetListUser(UserModel userModel=null)
        {
            if (userModel!=null&&!string.IsNullOrWhiteSpace(userModel.Pwd))
                userModel.Pwd = AESHelper.AESEncrypt(userModel.Pwd);
            return userDal.GetListUser(userModel);
        }
        public List<UserModel> GetPageUserList(UserSearchPageWhereModel whereModel)
        {
            if(whereModel==null)
                return null;
            return userDal.GetPageUserList(whereModel);
        }

        public bool ExistUser(UserModel userModel)
        {
            if (userModel == null)
                return false;
            var backUser = userDal.GetSingleUser(userModel);
            if (backUser == null || string.IsNullOrWhiteSpace(backUser.Email))
                return false;
            return true;
        }

        public bool AddUserList(List<UserModel> userList)
        {
            if (userList == null)
                return false;
            try
            {
                return userDal.AddUserList(userList);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("批量添加用户账号时出错:" + ex.ToString());
            }
            return false;
        }

        public int GetPageUserListCount(UserSearchPageWhereModel whereModel)
        {
            return userDal.GetPageUserListCount(whereModel);
        }

        public bool DeleteUserList(List<string> userIdList)
        {
            return userDal.DeleteUserList(userIdList);
        }

        public bool EditUser(UserModel userModel)
        {
            if (userModel == null)
                return false;
            if (!string.IsNullOrWhiteSpace(userModel.Pwd))
                userModel.Pwd = AESHelper.AESEncrypt(userModel.Pwd);
            return userDal.EditUser(userModel);
        }
    }
}
