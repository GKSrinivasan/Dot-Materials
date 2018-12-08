using Laserbeam.Resource.HR.LoginResources;
using Laserbeam.UI.HR.Common;
using System.ComponentModel.DataAnnotations;

namespace Laserbeam.UI.HR.Models
{
    public class ChangePasswordModel
    {
        [Display(Name = "LabelUserID", ResourceType = typeof(LoginResource))]
        [Required(ErrorMessageResourceType = typeof(LoginResource), ErrorMessageResourceName = "RequiredUserID")]
       // [Remote("ValidateUser", "Account", AdditionalFields = "UserPassword")]
        public string UserID { get; set; }

        [Display(Name = "LabelPassword", ResourceType = typeof(LoginResource))]
        //[Required(ErrorMessageResourceType = typeof(LoginResource), ErrorMessageResourceName = "RequiredPassword")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string UserPassword { get; set; }

        [Display(Name = "LabelNewPassword", ResourceType = typeof(LoginResource))]
        [Required(ErrorMessageResourceType = typeof(LoginResource), ErrorMessageResourceName = "RequiredNewPassword")]
        [NotEqual("UserPassword", ErrorMessageResourceType = typeof(LoginResource), ErrorMessageResourceName = "MessageDifferentPasswords")]
       // [RegexPassword(ErrorMessageResourceType = typeof(LoginResource), ErrorMessageResourceName = "MessagePasswordNotStrongEnough")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "LabelConfirmPassword", ResourceType = typeof(LoginResource))]
        [Required(ErrorMessageResourceType = typeof(LoginResource), ErrorMessageResourceName = "RequiredConfirmPassword")]
        [CompareAttribute("NewPassword", ErrorMessageResourceType = typeof(LoginResource), ErrorMessageResourceName = "MessagePasswordMismatch")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string PasswordStrengthMessage { get; set; }
        public int PasswordLength { get; set; }
    }
}