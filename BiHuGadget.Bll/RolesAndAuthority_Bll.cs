using BiHuGadget.Dal;
using BiHuGadget.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuGadget.Bll
{
    public class RolesAndAuthority_Bll
    {
        RolesAndAuthority_Dal raDal = new RolesAndAuthority_Dal();
        public RoleModel GetSingleRole(int roleId = 0)
        {
            return raDal.GetSingleRole(roleId);
        }
        public List<AuthorityModel> GetAuthorityList(int roleId = 0)
        {
            return raDal.GetAuthorityList(roleId);
        }
    }
}
