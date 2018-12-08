using Laserbeam.EntityManager.Common;
using System.Collections.Generic;
using System.Data;

namespace Laserbeam.Libraries.Interfaces.Common
{
    public interface IExport
    {        
        void ExportWord<T>(List<T> source, string fileName);
        void ExporttoExcel(DataTable data, string fileName);      
        
    }
}
