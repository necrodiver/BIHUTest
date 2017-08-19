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
    }
}
