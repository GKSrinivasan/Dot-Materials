using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class TemplateErrorListModel
    {
        public string Error { get; set; }
        public string AffectedData { get; set; }
        public string HowToFix { get; set; }
    }
}
