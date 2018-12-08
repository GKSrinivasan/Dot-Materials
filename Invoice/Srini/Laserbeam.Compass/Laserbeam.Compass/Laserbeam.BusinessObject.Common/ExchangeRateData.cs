using System.ComponentModel.DataAnnotations;

namespace Laserbeam.BusinessObject.Common
{
    public class ExchangeRateData
    {
        [RegularExpression("^[a-zA-Z]", ErrorMessage = "Invalid Currency Code")]
        public string CurrencyCode { get; set; }
        public int CurrencyCodeNum { get; set; }
        public decimal? ExchangeRate { get; set; }
        public string CultureCode { get; set; }

    }
}
