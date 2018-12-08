using System;

namespace InvoiceDataLayer.BussinessModel
{
    public class EmployeeModel
    {
        public string EmployeeID { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string EmailID { get; set; }
        public string City { get; set; }
        public int Pincode { get; set; }
        public string Location { get; set; }
        public DateTime HireDate { get; set; }
        public string ContactPerson { get; set; }
        public string UserRoll { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public string Address1 { get; set; }
        public string State { get; set; }
        public string Department { get; set; }
        public string IDType { get; set; }
        public string ManagerID { get; set; }
        public string ContactPersonAddress { get; set; }
        public string UserStatus { get; set; }
        public string LastName { get; set; }
        public int PhoneNo { get; set; }
        public string Address2 { get; set; }
        public string Country { get; set; }
        public string Designation { get; set; }
        public string IDNo { get; set; }
        public string ManagerName { get; set; }
        public int ContactPersonNo { get; set; }
        public string UserID { get; set; }
        public bool IsUser { get; set; }
    }
}
