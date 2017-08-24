using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuGadget.Models
{
    /// <summary>
    /// 打卡标记(可改动)
    /// </summary>
    public class ClockMark
    {
        public int ClockMarkId { get; set; }
        /// <summary>
        /// 标记记录说明
        /// </summary>
        public string Record { get; set; }
        /// <summary>
        /// 标记状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 是否审核通过，审核通过：1，未通过：0
        /// </summary>
        public int Pass { get; set; }
    }
}
