using Laserbeam.BusinessObject.Common;
using Laserbeam.DataManager.Interfaces.Common;
using Laserbeam.ProcessManager.Interfaces.Common;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace Laserbeam.ProcessManager.Common
{
    public class ChatBoxProcessManager: IChatBoxProcessManager
    {
        #region Fields
        // Author         :   Raja Ganapathy
        // Creation Date  :   05-Jul-2016  
        /// <summary>
        /// Instance of DashboardRepository
        /// </summary>
        IChatBoxRepository m_chatBoxRepository;
        #endregion

        #region Constructors
        public ChatBoxProcessManager(IChatBoxRepository ChatBoxRepository)
        {
            m_chatBoxRepository = ChatBoxRepository;
        }
        #endregion
        public List<ChatAccountModel> GetChatUserDetails(int loggedInUserNum, AppConfigModel appConfig)
        {
            return m_chatBoxRepository.GetChatUserDetails(loggedInUserNum,appConfig);
        }

        public List<ChatStatus> GetStatusList()
        {
            return m_chatBoxRepository.GetStatusList();
        }

        public async Task UpdateStatus(int ID, int UserNum)
        {
            await m_chatBoxRepository.UpdateStatus(ID, UserNum);
        }
        public int GetAppuserStatus(int UserNum)
        {
          return m_chatBoxRepository.GetAppuserStatus(UserNum);
        }
        
        public List<ChatDetails> GetChatDetails(int loggedInUserNum, int selectedUserNum, AppConfigModel appConfig)
        {
            return m_chatBoxRepository.GetChatDetails(loggedInUserNum, selectedUserNum,appConfig);
        }

        public List<ChatDetails> UpdateChat(int loggedInUserNum, int selectedUserNum, string chat, string PathName,int FileTYpe, string FileName, AppConfigModel appConfig)
        {
           return m_chatBoxRepository.UpdateChat(loggedInUserNum, selectedUserNum, chat,PathName, FileTYpe,FileName, appConfig);
        }

        public List<ChatDetails> GetSearchChatDetails(int loggedInUserNum, int selectedUserNum, string Chat)
        {
            return m_chatBoxRepository.GetSearchChatDetails(loggedInUserNum, selectedUserNum, Chat);
        }
    }
}
