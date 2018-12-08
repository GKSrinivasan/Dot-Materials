using System.ComponentModel.DataAnnotations;

namespace Laserbeam.UI.HR.Common
{
    public class LockedUserAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
       
    {
            if ((string)value == "laserbeam")
                return false;
            else
                return true;
        }
    }
}