using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuGadget.Models
{
    /// <summary>
    /// 考勤表，打卡记录
    /// </summary>
    public class AttendanceModel
    {
        public int? AttendanceId { get; set; }
        public int? UserId { get; set; }
        /// <summary>
        /// 打卡年份
        /// </summary>
        public int ClockYear { get; set; }
        /// <summary>
        /// 打卡时间
        /// </summary>
        public DateTime ClockTime { get; set; }
        /// <summary>
        /// 时间段
        /// 0：整天,1：上午,2：下午
        /// </summary>
        public int TimeSlot { get; set; }
        /// <summary>
        /// 打卡状态
        ///1:迟到
        ///2:早退 
        ///3:早回家
        ///4:正常
        ///5:加班
        ///6:请假
        /// </summary>
        public int UDayStateId { get; set; }
        /// <summary>
        /// 备注说明
        /// </summary>
        public string ClockContent { get; set; }
    }
    /// <summary>
    /// 查询考勤打卡备注
    /// </summary>
    public class AttendanceSearchModel
    {
        public int? UserId { get; set; }
        public string  UserName { get; set; }
        public int ClockYear { get; set; }
    }
}
