using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class AccessTree
    {
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public int? ParentNum { get; set; }
        public bool HasChildren { get; set; }
        public int Depth { get; set; }
        public List<AccessTree> Items { get; set; }
        public string MenuLinks { get; set; }
    }
}
