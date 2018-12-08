using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class CountryCodeModel
    {
       
        public short SortOrder { get; set; }
        public bool Active { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
    
        public int EmployeeCount { get; set; }
        public int CurrencyNum { get; set; }
        public Nullable<decimal> BaseExchangeRate { get; set; }
    }
}
