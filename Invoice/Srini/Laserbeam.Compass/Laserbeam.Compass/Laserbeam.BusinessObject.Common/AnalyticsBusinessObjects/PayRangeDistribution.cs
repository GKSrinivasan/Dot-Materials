using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common.AnalyticsBusinessObjects
{
   public class PayRangeDistribution
    {
        public string TypeBefore { get; set; }
        public decimal BelowminBefore { get; set; }
        public decimal LowerBefore { get; set; }
        // public float MidBefore { get; set; }
        public decimal UpperBefore { get; set; }
        public decimal OvermaxBefore { get; set; }

        public string TypeAfter { get; set; }
        public decimal BelowminAfter { get; set; }
        public decimal LowerAfter { get; set; }
        public decimal MidAfter { get; set; }
        public decimal UpperAfter { get; set; }
        public decimal OvermaxAfter { get; set; }

        public decimal BelowminBeforePct { get; set; }
        public decimal LowerBeforePct { get; set; }
        //  public float MidpercBefore { get; set; }
        public decimal UpperBeforePct { get; set; }
        public decimal OvermaxBeforePct { get; set; }

        public decimal BelowminAfterPct { get; set; }
        public decimal LowerAfterPct { get; set; }
        // public float MidpercAfter { get; set; }
        public decimal UpperAfterPct { get; set; }
        public decimal OvermaxAfterPct { get; set; }


        public string Typestr { get; set; }
        public string Belowminstr { get; set; }
        public string Lowerstr { get; set; }
        public string Midstr { get; set; }
        public string Upperstr { get; set; }
        public string Overmaxstr { get; set; }
        public string Title { get; set; }
        public int Before { get; set; }
        public int After { get; set; }
    }
}
