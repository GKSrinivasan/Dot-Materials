
namespace Laserbeam.UI.HR.Common
{
    public static class ApplicationState
    {
        
        public static T GetApplicationState<T>(string objectName)
        {
            var appState = System.Web.HttpContext.Current.Application;
            T retVal = default(T);
            appState.Lock();
            if (appState[objectName] != null)
                retVal = (T)appState[objectName];
            appState.UnLock();
            return retVal;
        }
        public static void SetApplicationState<T>(string objectName, T objectValue)
        {
            var appState = System.Web.HttpContext.Current.Application;
            appState.Lock();
            appState[objectName] = objectValue;
            appState.UnLock();
        }
        public static bool isApplicationState(string objectName)
        {
            var appState = System.Web.HttpContext.Current.Application;
            return appState[objectName] != null;
        }
    }
}