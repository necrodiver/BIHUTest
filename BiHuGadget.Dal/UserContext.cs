using BiHuGadget.Helpers;
using BiHuGadget.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuGadget.Dal
{
    public class UserContext : DbContext
    {
        public UserContext(string connectionName)
            : base(connectionName)
        {

        }
        public DbSet<UserModel> UserModels { get; set; }
    }
    public class UserCtrl
    {
        public bool SetUsers(UserModel userModel)
        {
            try
            {
                using (var ctx = new UserContext("BiHuDB"))
                {
                    userModel.CreateTime = DateTime.Now;
                    ctx.UserModels.Add(userModel);
                    ctx.SaveChanges();
                    //var query = from user in ctx.UserModels
                    //            select user;
                    //foreach (var q in query)
                    //{
                    //}

                }
                return true;
            }
            catch (Exception ex)
            {
                LogCollectHelper.ErrorLog("保存用户信息(UserContext)：" + ex.ToString());
            }
            return false;

        }

        public string GetUsersName()
        {
            try
            {
                string userNames = "";
                using (var ctx = new UserContext("BiHuDB"))
                {
                    UserModel userModel = new UserModel();
                    userModel.UserName = "张三" + DateTime.Now.ToString("HH:ss");
                    userModel.CreateTime = DateTime.Now;
                    ctx.UserModels.Add(userModel);
                    ctx.SaveChanges();
                    var query = from user in ctx.UserModels
                                select user;
                    foreach (var q in query)
                    {
                        userNames += q.UserName+"||";
                    }
                }
                return userNames;
            }
            catch (Exception ex)
            {
                LogCollectHelper.ErrorLog("读取用户信息(GetUsersName)：" + ex.ToString());
            }
            return null;
        }
    }
}
