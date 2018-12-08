using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class RuleConfigModel
    {
        public List<string> RegionalSetting { get; set; }
        public List<string> Rounding { get; set; }
        public List<string> DecimalVal { get; set; }
        public List<string> PercentageDecimalVal { get; set; }
        public List<string> HourlyDecimalVal { get; set; }
        public List<DropDownListModel> DateFormat { get; set; }
        public List<DropDownListModel> CurrencyFormat { get; set; }
        public List<string> ProrationDecimalVal { get; set; }
        public bool isWizard { get; set; }
        public List<string> PasswordLength { get; set; }
        public string SSOUrl { get; set; }
    }
}
