using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuGadget.Models
{
    public class UserModel
    {
        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public int? UserId { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 扮演角色(权限)
        /// </summary>
        public int? RoleId { get; set; }
    }
}
