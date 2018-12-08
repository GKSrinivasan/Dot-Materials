// Confidential and proprietary.

// Component Name  : 	Manager Home Model
// Description     : 	Contains the properties required for getting access rights	
// Author          :	Roopan		
// Creation Date   : 	03-11-2014

using Laserbeam.BusinessObject.Common;
using Laserbeam.Constant.HR;

namespace Laserbeam.UI.HR.Models
{
    public class ManagerHomeModel
    {
        public CompensationTypeConfiguration CompensationTypeConfiguration { get; set; }
        public MenuTypeModel menuTypeModel { get; set; }
        public ViewPageType pageType { get; set; }
        public BudgetModel budgetModel { get; set; }
    }
}