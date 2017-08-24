using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuGadget.Models
{
    public class AuthorityModel
    {
        /// <summary>
        /// 权限Id
        /// </summary>
        public int AuthorityId { get; set; }
        /// <summary>
        /// 操作Action
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// 操作Controller
        /// </summary>
        public string ControllerName { get; set; }
        /// <summary>
        /// 权限说明
        /// </summary>
        public string Explain { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public string RoleId { get; set; }
    }
}
