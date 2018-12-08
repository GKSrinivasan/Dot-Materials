using InvoiceDataLayer.EntityFramework;
using InvoiceDataLayer.IDataManager;
using System.Collections.Generic;
using System.Linq;

namespace InvoiceDataLayer.DataManager
{
    public class CommonRepository:ICommonRepository
    {
        private readonly IBaseRepository m_baseRepository;

        public CommonRepository(IBaseRepository baseRepository)
        {
            m_baseRepository = baseRepository;
        }

        public AppUser GetAppUser(string userID)
        {
            return m_baseRepository.dbcontext().AppUsers.Where(x => x.UserID == userID).FirstOrDefault();
        }
    }
}
