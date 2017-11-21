using BiHuGadget.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static BiHuGadget.Helpers.NotMinusAttribute;

namespace WebBiHuGadget.Models
{
    public class View_MonthInUserMark : IValidatableObject
    {
        [DisplayName("用户Id")]
        [NotMinus(ErrorMessage = "{0}数据无效")]
        public int? UserId { get; set; }

        [DisplayName("年份")]
        [Required]
        [IntLength(2017, 2020, ErrorMessage = "{0}格式不正确")]
        public int Year { get; set; }

        [DisplayName("用户名")]
        public string UserName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var model = validationContext.ObjectInstance as View_MonthInUserMark;
            if ((model.UserId == null || (model.UserId != null && model.UserId < 0)) && string.IsNullOrWhiteSpace(model.UserName))
            {
                yield return new ValidationResult("用户名和用户Id最少有一个不为空");
            }
        }
    }
    public class View_AttendanceUserWhere
    {
        [DisplayName("用户Id")]
        [NotMinus(ErrorMessage = "{0}数据无效")]
        public int? UserId { get; set; }
        [DisplayName("用户名")]
        [RegularExpression(@"^[\u4E00-\u9FA5]{2,5}$", ErrorMessage = "{0}格式错误！")]
        public string UserName { get; set; }
        [DisplayName("用户Id")]
        [Required]
        [IntLength(2017, 2020, ErrorMessage = "{0}格式不正确")]
        public int Year { get; set; }
        [Required]
        [PositiveInteger(ErrorMessage = "{0}数据无效")]
        public int PageIndex { get; set; }
        [DisplayName("单页数量")]
        [PageCount(ErrorMessage = "{0}不符合规范")]
        public int PageSize { get; set; }
        [DisplayName("选择查询月份")]
        [IntLength(1, 12)]
        public int? Month { get; set; }
        [DisplayName("查询分组")]
        [IntLength(1, 10)]
        public int? GroupId { get; set; }
    }
    public class EditAttendanceIsPass
    {
        [DisplayName("打卡备注Id")]
        [NotMinus(ErrorMessage = "{0}不符合规范")]
        public int AttendanceId { get; set; }
        [DisplayName("审核状态")]
        [IntLength(1, 2, ErrorMessage = "{0}不符合规范")]
        public int IsPass { get; set; }
    }
}