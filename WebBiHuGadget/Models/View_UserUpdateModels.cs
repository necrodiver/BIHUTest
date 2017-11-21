using BiHuGadget.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBiHuGadget.Models
{
    #region 更新用户集合
    public class UserUpdateModel : IValidatableObject
    {
        [Required]
        public int UserId { get; set; }
        [StringLength(20, MinimumLength = 4)]
        public string Pwd { get; set; }
        [RegularExpression(@"^[0-9a-zA-Z]{2,20}\@91bihu\.com", ErrorMessage = "邮箱格式为注册的壁虎账号(只能使用字母)")]
        public string Email { get; set; }
        [DisplayName("权限角色")]
        [RoleId(ErrorMessage = "{0}不符合规范")]
        public int? RoleId { get; set; }
        [DisplayName("归属小组")]
        [IntLength(0, 10)]
        public int? GroupId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var model = validationContext.ObjectInstance as UserUpdateModel;
            if (!string.IsNullOrWhiteSpace(model.Email) || !string.IsNullOrWhiteSpace(model.Pwd) || model.RoleId.HasValue|| model.GroupId.HasValue)
            {

            }
            else
            {
                yield return new ValidationResult("用户密码/用户邮箱/用户权限角色/归属小组 最少有一个不能为空!");
            }
        }
    }
    public class RoleIdAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return true;
            int roleNum = -1;
            if (int.TryParse(Convert.ToString(value), out roleNum))
            {
                if (roleNum >= 1 && roleNum <= 3)
                    return true;
            }
            return false;
        }
    }
    #endregion
}