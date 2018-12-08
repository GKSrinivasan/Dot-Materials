using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.BusinessObject.Common
{
    public class TemplateMetaColumns
    {
        public int TemplateMetaColumnID { get; set; }
        public string FieldName { get; set; }
        public string AliasName { get; set; }
        public string FunctionalGroup { get; set; }        
        public string SampleData { get; set; }
        public string DataLength { get; set; }
        public string DataFormat { get; set; }
        public string ControlType { get; set; }
        public string ControlFormat { get; set; }
        public string PlaceHolder { get; set; }
        public string FieldInformation { get; set; }
        public string FieldValue { get; set; }
        public string FieldDescription { get; set; }
        public bool IsTemplateColumn { get; set; }
        public bool IsTemplateDefaultColumn { get; set; }
        public bool IsEnabled { get; set; }        
        public Nullable<bool> IsMandate { get; set; }
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
    }
}
