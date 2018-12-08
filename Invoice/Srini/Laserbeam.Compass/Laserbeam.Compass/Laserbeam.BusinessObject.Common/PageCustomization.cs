using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    [Serializable()]
    public class PageCustomization
    {
        public int MetaColumnID { get; set; }
        public string AliasName { get; set; }
        public string FunctionalGroup { get; set; }
        public bool PopupDisplay { get; set; }
        public bool GridDisplay { get; set; }
        public bool FilterDisplay { get; set; }
        public string ColumnName { get; set; }        
        public bool ExportDisplay { get; set; }
        public bool IsChanged { get; set; }
    }
}
