using Laserbeam.BusinessObject.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Laserbeam.ProcessManager.Interfaces.Common
{
    public interface IChatBoxProcessManager
    {
        List<ChatAccountModel> GetChatUserDetails(int loggedInUserNum, AppConfigModel appConfig);
        List<ChatStatus> GetStatusList();
        Task UpdateStatus(int ID, int UserNum);
        int GetAppuserStatus(int UserNum);
        List<ChatDetails> GetChatDetails(int loggedInUserNum, int selectedUserNum, AppConfigModel appConfig);
        List<ChatDetails> UpdateChat(int loggedInUserNum, int selectedUserNum, string chat, string PathName, int FileTYpe, string FileName,AppConfigModel appConfig);
        List<ChatDetails> GetSearchChatDetails(int loggedInUserNum, int selectedUserNum, string Chat);
    }
}
