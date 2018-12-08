using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Laserbeam.BusinessObject.Common
{
   public class ExchangeRateGridData
    {
       public string CurrencyCode { get; set; }
       public decimal ExchangeRate { get; set; }
       public string CultureCode { get; set; }
       public int CurrencyCodeNum { get; set; }
        public string BaseCurrency { get; set; }
        //public decimal ConversionPreview { get; set; }
        //public decimal DisplayPreview { get; set; }
    }
}
