using Laserbeam.Resource.HR.LoginResources;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Laserbeam.UI.HR.Models
{
    public class ForgotPasswordModel
    {
        [Display(Name = "LabelUserID", ResourceType = typeof(LoginResource))]
        [Required(ErrorMessageResourceType = typeof(LoginResource), ErrorMessageResourceName = "RequiredUserID")]
        [Remote("UserValidation", "Account", HttpMethod = "Post", ErrorMessage = "Enter valid Email Address ")]
        public string UserID { get; set; }
    }
}