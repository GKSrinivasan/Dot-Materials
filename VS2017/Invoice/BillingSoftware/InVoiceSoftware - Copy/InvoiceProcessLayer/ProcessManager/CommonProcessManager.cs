using InvoiceDataLayer.EntityFramework;
using InvoiceDataLayer.IDataManager;
using InvoiceProcessLayer.IProcessManager;
using System.Collections.Generic;

namespace InvoiceProcessLayer.ProcessManager
{
    public class CommonProcessManager : ICommonProcessManager
    {
        private readonly ICommonRepository m_commonRepository;

        public CommonProcessManager(ICommonRepository commonRepository)
        {
            m_commonRepository = commonRepository;
        }

        public AppUser GetAppUser(string userID)
        {
            return m_commonRepository.GetAppUser(userID);
        }
    }
}
