using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuGadget.Helpers
{
    /// <summary>
    /// 非负整数
    /// 如果有则检查是否为非负整数,没有则通过
    /// </summary>
    public class NotMinusAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            int num = 0;
            if (int.TryParse(Convert.ToString(value), out num))
            {
                if (num >= 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
    /// <summary>
    /// 正整数验证
    /// </summary>
    public class PositiveIntegerAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            int num = 0;
            if (int.TryParse(Convert.ToString(value), out num))
            {
                if (num > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
    /// <summary>
    /// 分页单页数量
    /// </summary>
    public class PageCountAttribute : ValidationAttribute
    {
        int _minLength;
        int _maxLength;
        public PageCountAttribute(int minLength = 5, int maxLength = 50)
        {
            _minLength = minLength;
            _maxLength = maxLength;
        }
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }
            int num = 0;
            if (int.TryParse(Convert.ToString(value), out num))
            {
                if (num >= _minLength&& num<= _maxLength)
                {
                    return true;
                }
            }
            return false;
        }
    }
    /// <summary>
    /// 月份验证
    /// </summary>
    public class MonthVerifyAttribute : ValidationAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(name);
        }
        public override bool IsValid(object value)
        {
            if (value == null)
                return true;
            int _month = 0;
            if (int.TryParse(Convert.ToString(value), out _month))
                if (_month > 0 && _month < 32)
                    return true;
            return false;
        }
    }
    /// <summary>
    /// 数字范围
    /// </summary>
    public class IntLengthAttribute : ValidationAttribute
    {
        int _min = 0;
        int _max = int.MaxValue;
        public IntLengthAttribute(int min, int max)
        {
            if (min <= max)
            {
                _min = min;
                _max = max;
            }
        }
        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(name);
        }
        public override bool IsValid(object value)
        {
            if (value == null)
                return true;
            int _num = 0;
            if (int.TryParse(Convert.ToString(value), out _num))
                if (_num >= _min && _num <= _max)
                    return true;
            return false;
        }
    }
}
