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
        public string ClockTime { get; set; }
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
        /// <summary>
        /// 添加备注日期
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 0:未审核
        /// 1:通过  
        /// 2:未通过
        /// </summary>
        public int? IsPass { get; set; }
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
    public class AttendanceSearchAllModel
    {
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public int ClockYear { get; set; }
        public int LeftNum { get; set; }
        public int PageCount { get; set; }
    }
}
