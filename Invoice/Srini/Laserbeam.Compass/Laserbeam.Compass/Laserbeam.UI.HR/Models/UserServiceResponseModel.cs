using Laserbeam.Constant.HR;

namespace Laserbeam.UI.HR.Models
{
    public class UserServiceResponseModel
    {
        public UserCredentialStatus UserStatus { get; set; }
        public string TenantUrl { get; set; }
    }
}