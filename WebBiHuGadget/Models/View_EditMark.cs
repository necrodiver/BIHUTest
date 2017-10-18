using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBiHuGadget.Models
{
    public class View_EditMark
    {
        /// <summary>
        /// 操作类型:增,改,删
        /// </summary>
        [DisplayName("操作类型")]
        [Required(ErrorMessage = "参数{0}无效")]
        public MarkIUD MarkIUD { get; set; }
        [DisplayName("用户Id")]
        [Required(ErrorMessage = "{0}数据不规范")]
        public string UserId { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        [DisplayName("记录日期")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0}不能为空")]
        public string DayTime { get; set; }
        /// <summary>
        /// 时间段
        /// </summary>
        [Required(ErrorMessage = "时间段不能为空")]
        public int TimeSlot { get; set; }
        /// <summary>
        /// 打卡状态
        /// </summary>
        [DisplayName("打卡状态")]
        [Required(ErrorMessage = "{0}不能为空")]
        public int MarkState { get; set; }
        /// <summary>
        /// 备注说明
        /// </summary>
        [DisplayName("备注说明")]
        [MaxLength(100, ErrorMessage = "{0}请务必简要描述")]
        public string MarkReason { get; set; }

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