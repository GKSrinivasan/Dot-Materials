using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laserbeam.EntityManager.Common
{
    public partial class LaserbeamEntitiesContainer1 : DbContext
    {
        public LaserbeamEntitiesContainer1(EntityConnection schemaConnection):base(schemaConnection,true)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
    }
}
