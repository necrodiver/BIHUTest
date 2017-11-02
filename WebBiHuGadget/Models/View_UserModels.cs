﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public class UserNameModel
    {
        [DisplayName("用户姓名集合")]
        [Required(ErrorMessage = "{0}必须填写")]
        [RegularExpression(@"^[\u4E00-\u9FA5\,]+$", ErrorMessage = "{0}格式错误！")]
        [StringLength(1000, MinimumLength = 2, ErrorMessage = "{0}长度不符合规范的长度")]
        [UserList(ErrorMessage ="{0}不符合规范")]
        public string UserNames { get; set; }
    }
    public class UserListAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            string userNames = value.ToString();
            var userNameList = userNames.Split(',').ToList<string>();
            var userNameListLast = userNameList.Where(u =>
            {
                if (string.IsNullOrWhiteSpace(u))
                    return false;
                else if (u.Length < 2)
                    return false;
                return true;
            }).ToList<string>();
            if (userNameListLast != null && userNameListLast.Count == userNameList.Count)
                return true;
            return false;
        }
    }
}