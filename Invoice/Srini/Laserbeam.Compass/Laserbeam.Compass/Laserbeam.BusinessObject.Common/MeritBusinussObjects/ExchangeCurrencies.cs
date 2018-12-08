using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class ExchangeCurrencies
    {
        public int CurrencyNum { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
    }
}
