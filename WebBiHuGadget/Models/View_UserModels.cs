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
    public class View_Login
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "姓名不能为空")]
        [RegularExpression("^\u4E00-\u9FA5+$", ErrorMessage = "姓名格式只能为中文")]
        [StringLength(5, MinimumLength = 2, ErrorMessage = "姓名长度范围为2~5个字符长度")]
        public string UserName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "密码不能为空")]
        [RegularExpression(@"^[A-Za-z\.0-9]{6,20}", ErrorMessage = "密码只能是数字或字母和.组成的长度范围为6~20")]
        public string Pwd { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "‘记住我’不能为空")]
        [ReadMe(ErrorMessage = "请不要手动修改'记住我'")]
        public int ReadMe { get; set; }
    }
    public class ReadMeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                return false;
            }
            int readMe = Convert.ToInt32(value);
            if (readMe == 0 || readMe == 1)
            {
                return true;
            }
            return false;
        }
    }
}