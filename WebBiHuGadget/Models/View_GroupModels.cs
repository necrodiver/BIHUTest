using BiHuGadget.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBiHuGadget.Models
{
    public class View_GroupModel : IValidatableObject
    {
        [Required]
        public MarkIUD MarkIUD { get; set; }
        [NotMinus]
        public int? GroupId { get; set; }
        [RegularExpression(@"^[\u4E00-\u9FA5]+$", ErrorMessage = "{0}格式错误！")]
        [StringLength(20, MinimumLength = 3)]
        public string GroupName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var model = validationContext.ObjectInstance as View_GroupModel;
            if (model.GroupId != null || !string.IsNullOrWhiteSpace(model.GroupName))
            {

            }
            else
            {
                yield return new ValidationResult("分组Id/分组名称最少有一个不能为空!");
            }
        }
    }
}