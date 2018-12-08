using System.ComponentModel.DataAnnotations;

namespace InVoiceSoftware.Models
{
    public class LoginModel
    {
        [Required]
        public string UserID { get; set; }
        [Required]
        public string Password { get; set; }
    }
}