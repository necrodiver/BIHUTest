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
    public class Users_Bll
    {
        UserCtrl userCtrl = new UserCtrl();
        public bool SetUser(UserModel userModel)
        {
            userModel.Pwd = userModel.Pwd.AESEncrypt();
            return  userCtrl.SetUsers(userModel);
        }

        public string GetUsers()
        {
            return  userCtrl.GetUsersName();
        }
    }
}
