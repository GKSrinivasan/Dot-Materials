using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class RoundingTypeModel
    {
        public int RoundingTypeId { get; set; }
        public string RoundTypeKey { get; set; }
        public string RoundTypeValue { get; set; }
        public bool Active { get; set; }
    }
}
