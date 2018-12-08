using Laserbeam.Resource.HR.LoginResources;
using Laserbeam.UI.HR.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Laserbeam.UI.HR.Models
{
    public class LoginModel
    {
        [Display(Name = "LabelUserID", ResourceType = typeof(LoginResource))]
        [Required(ErrorMessageResourceType = typeof(LoginResource), ErrorMessageResourceName = "RequiredUserID")]
        [Remote("UserValidation", "Account", HttpMethod = "Post", ErrorMessage = "Enter valid email address ")]
        public string UserID { get; set; }

        [Display(Name = "LabelPassword", ResourceType = typeof(LoginResource))]
        [Required(ErrorMessageResourceType = typeof(LoginResource), ErrorMessageResourceName = "RequiredPassword")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]        
        public string UserPassword { get; set; }
        public DateTime? LockedDate { get; set; }
        public string ReCaptchaSiteKey { get; set; }
        public bool IsRecaptcha { get; set; }
        public string ReCaptcha { get; set; }
    }
}