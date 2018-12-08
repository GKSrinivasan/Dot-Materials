using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{

    public class YahooExchangeApi
    {
        public Query query { get; set; }
    }
    public class Query
    {
        public int count { get; set; }
        public DateTime created { get; set; }
        public string lang { get; set; }
        public Results results { get; set; }

    }
    public class Results
    {
        public List<ExchangeRatesAPI> rate { get; set; }
    }
    public class ExchangeRatesAPI
    {

        public string id { get; set; }
        public string Name { get; set; }
        public decimal Rate { get; set; }
        public DateTime Date {get;set;}
        public string Time { get; set; }
        public string Ask { get; set; }
        public string Bid { get; set; }
    }

   
    
   
}
