using BiHuGadget.Helpers;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebBiHuGadget.Models
{
    public class View_EditMark
    {
        /// <summary>
        /// 打卡备注Id
        /// </summary>
        [DisplayName("打卡备注Id")]
        [NotMinus(ErrorMessage = "{0}数据无效")]
        public int? AttendanceId { get; set; }
        /// <summary>
        /// 操作类型:增,改,删
        /// </summary>
        [DisplayName("操作类型")]
        [Required(ErrorMessage = "参数{0}无效")]
        public MarkIUD? MarkIUD { get; set; }

        [DisplayName("用户Id")]
        [NotMinus(ErrorMessage = "{0}数据无效")]
        public int? UserId { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [DisplayName("记录日期")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0}不能为空")]
        public DateTime ClockTime { get; set; }
        /// <summary>
        /// 时间段
        /// </summary>
        [DisplayName("时间段")]
        [Required(ErrorMessage = "{0}不能为空")]
        [NotMinus(ErrorMessage = "{0}数据无效")]
        public int TimeSlot { get; set; }
        /// <summary>
        /// 打卡状态
        /// </summary>
        [DisplayName("打卡状态")]
        [Required(ErrorMessage = "{0}不能为空")]
        [NotMinus(ErrorMessage = "{0}数据无效")]
        public int UDayStateId { get; set; }
        /// <summary>
        /// 备注说明
        /// </summary>
        [DisplayName("备注说明")]
        [MaxLength(100, ErrorMessage = "{0}请务必简要描述")]
        public string ClockContent { get; set; }
        [DisplayName("姓名")]
        [Required(ErrorMessage = "{0}是必须的")]
        [StringLength(5, MinimumLength = 2, ErrorMessage = "{0}长度错误")]
        public string UserName { get; set; }

    }
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum MarkIUD
    {
        Delete = 0,
        Insert = 1,
        Update = 2
    }

}