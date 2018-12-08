
namespace Laserbeam.BusinessObject.Common
    {
    public class JobList
        {
        public int JobNum { get; set; }
        public int MarketReferenceNum { get; set; }
        public int? JobMarketNum { get; set; }
        public int? GradeOrder { get; set; }
        public string JobDescription { get; set; }
         public decimal? MktMax { get; set; }
        public decimal? MktMid { get; set; }
        public decimal? MktMin { get; set; }
        }
    }
