using Laserbeam.Resource.HR.UserManagementResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laserbeam.UI.HR.Models
{
    public class AddUserModel
    {

        [Display(Name = "LabelUserID", ResourceType = typeof(UserManagementResources))]
        [Required(ErrorMessageResourceType = typeof(UserManagementResources), ErrorMessageResourceName = "RequiredUserID")]
        [Remote("UserValidation", "UserManagement",AdditionalFields ="UserNum" ,HttpMethod = "Post", ErrorMessage = "Entered email address is Already Exist ")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid email address Format")]
        [StringLength(100)]
        public string UserID { get; set; }

        [Display(Name = "LabelEmployeeID", ResourceType = typeof(UserManagementResources))]
        [Required(ErrorMessageResourceType = typeof(UserManagementResources), ErrorMessageResourceName = "RequiredEmployeeID")]
        [Remote("EmployeeIdValidation", "UserManagement", HttpMethod = "Post", ErrorMessage = "Enter Valid Employee ID ")]
        [StringLength(100)]
        public string EmployeeID { get; set; }

        [Display(Name = "LabelFirstName", ResourceType = typeof(UserManagementResources))]        
        [Required(ErrorMessageResourceType = typeof(UserManagementResources), ErrorMessageResourceName = "RequiredFirstName")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Display(Name = "LabelLastName", ResourceType = typeof(UserManagementResources))]
        [Required(ErrorMessageResourceType = typeof(UserManagementResources), ErrorMessageResourceName = "RequiredLastName")]
        [StringLength(100)]
        public string LastName { get; set; }

        [Display(Name = "LabelPreferredName", ResourceType = typeof(UserManagementResources))]
        [StringLength(100)]
        public string PreferredName { get; set; }

        //[DataType(DataType.EmailAddress)]
        //[Display(Name = "LabelEmailAddress", ResourceType = typeof(UserManagementResources))]
        //[RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid EmailAddress")]
        //[Required(ErrorMessageResourceType = typeof(UserManagementResources), ErrorMessageResourceName = "RequiredEmailAddress")]        
        //[StringLength(200)]
        //public string EmailID { get; set; }
                
        public Nullable<int> UserRoleNum { get; set; }
        
        public string UserRole { get; set; }

        [Required(ErrorMessageResourceType = typeof(UserManagementResources), ErrorMessageResourceName = "RequiredUserRole")]
        public string UserRoleType { get; set; }
        public int AdminUserRoleNum { get; set; }
        public string UserStatus { get; set; }       
        public int UserNum { get; set; }
        public Nullable<int> AppUserStatusID { get; set; }       
        public bool IsAdminAccess { get; set; }
    }
}