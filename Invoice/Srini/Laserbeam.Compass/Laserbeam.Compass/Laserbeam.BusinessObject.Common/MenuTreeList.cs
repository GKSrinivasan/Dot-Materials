using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class MenuTreeList
    {
        public int? id { get; set; }
        public string Name { get; set; }
        public bool hasChildren { get; set; }
        public Nullable<int> ParentNum { get; set; }
        public string sortOrder { get; set; }
        public bool expanded { get; set; }
        public List<MenuTreeList> items { get; set; }
        public string urlLink { get; set; }
    }
}
