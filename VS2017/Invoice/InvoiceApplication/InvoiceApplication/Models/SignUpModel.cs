using System;

namespace InvoiceApplication.Models
{
    public class SignUpModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Int64 PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string UserID { get; set; }
        public string CompanyName { get; set; }
        public string TenantName { get; set; }
    }
}