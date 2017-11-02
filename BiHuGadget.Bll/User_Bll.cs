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
        public List<UserModel> GetListUser(UserModel userModel)
        {
            if (userModel == null)
                return null;
            if (!string.IsNullOrWhiteSpace(userModel.Pwd))
                userModel.Pwd = AESHelper.AESEncrypt(userModel.Pwd);
            return userDal.GetListUser(userModel);
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

        public bool AddUserList(List<KVModel> userNameList)
        {
            //INSERT INTO MyTable(ID, NAME) VALUES(7, '003'),(8, '004'),(9, '005');
            //UserModel 
            return false;
        }
    }
}
