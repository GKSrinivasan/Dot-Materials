using System.ComponentModel.DataAnnotations;

namespace InvoiceDataLayer.CustomModel
{
    public class LoginModel
    {
        [Required]
        public string UserID { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
