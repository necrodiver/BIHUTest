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
        [YearVerify(2017, 2020, ErrorMessage = "{0}格式不正确")]
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
    public class View_AttendanceUserWhere: IValidatableObject
    {
        [DisplayName("用户Id")]
        [NotMinus(ErrorMessage = "{0}数据无效")]
        public int? UserId { get; set; }
        [DisplayName("用户名")]
        public string UserName { get; set; }
        [DisplayName("用户Id")]
        [Required]
        [YearVerify(2017, 2020, ErrorMessage = "{0}格式不正确")]
        public int Year { get; set; }
        [Required]
        [PositiveInteger(ErrorMessage = "{0}数据无效")]
        public int PageIndex { get; set; }
        [DisplayName("单页数量")]
        [PageCount(ErrorMessage ="{0}不符合规范")]
        public int PageCount { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var model = validationContext.ObjectInstance as View_MonthInUserMark;
            if ((model.UserId == null || (model.UserId != null && model.UserId < 0)) && string.IsNullOrWhiteSpace(model.UserName))
            {
                yield return new ValidationResult("用户名和用户Id最少有一个不为空");
            }
        }
    }
}