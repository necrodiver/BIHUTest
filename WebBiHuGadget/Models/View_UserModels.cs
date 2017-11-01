using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBiHuGadget.Models
{
    public class View_UserModels
    {
    }
    #region 登录传输内容
    public class View_Login
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "邮箱不能为空")]
        [RegularExpression(@"^[A-Za-z]{2,20}\@91bihu\.com", ErrorMessage = "邮箱格式为注册的壁虎账号")]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "密码不能为空")]
        [RegularExpression(@"^[A-Za-z\.0-9]{6,20}", ErrorMessage = "密码只能是数字或字母和.组成的长度范围为6~20")]
        public string Pwd { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "‘记住我’不能为空")]
        [ReadMe(ErrorMessage = "请不要手动修改'记住我'")]
        public int ReadMe { get; set; }
    }
    /// <summary>
    /// 是否记住我(单独用于登录,所以不用抽出来)
    /// </summary>
    public class ReadMeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            int readMe = 0;
            if (int.TryParse(Convert.ToString(value), out readMe))
            {
                if (readMe == 0 || readMe == 1)
                    return true;
            }
            return false;
        }
    }
    #endregion

}