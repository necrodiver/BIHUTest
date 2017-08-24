using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuGadget.Models
{
    /// <summary>
    /// 考勤表，只能导入，不可手动修改
    /// </summary>
    public class AttendanceModel
    {
        public int AttendanceId { get; set; }
        public int UserId { get; set; }
        /// <summary>
        /// 打卡时间
        /// </summary>
        public DateTime? ClockTime { get; set; }
        /// <summary>
        /// 打卡状态
        /// </summary>
        public int ClockStatus { get; set; }
    }
}
