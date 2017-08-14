using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuGadget.Models
{
    public class Authority
    {
        /// <summary>
        /// 权限Id
        /// </summary>
        public int AuthorityId { get; set; }
        /// <summary>
        /// 操作模块
        /// </summary>
        public string OperateModule { get; set; }
        /// <summary>
        /// 权限说明
        /// </summary>
        public string Explain { get; set; }
    }
}
