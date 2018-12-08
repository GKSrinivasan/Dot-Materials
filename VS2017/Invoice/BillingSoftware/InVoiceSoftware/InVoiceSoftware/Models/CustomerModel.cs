using System;

namespace InVoiceSoftware.Models
{
    public class CustomerModel
    {
        public int CustomerNo { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public int PhoneNo { get; set; }
        public string IDType { get; set; }
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public bool StoreCard { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string EmailID { get; set; }
        public string IDNo { get; set; }
        public string AddressLine2 { get; set; }
        public string State { get; set; }
        public int PinCode { get; set; }
        public bool CreateAccount { get; set; }
    }
}