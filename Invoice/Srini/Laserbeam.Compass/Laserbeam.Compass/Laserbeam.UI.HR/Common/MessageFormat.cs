using System.Web.Mvc;
namespace Laserbeam.UI.HR
{
    public static class MessageFormat
    {
        public static MvcHtmlString SetMessage(string message, bool isSuccess)
        {
            string color = (isSuccess) ? "green" : "red";
            return (new MvcHtmlString("<span style=\"color:" + color + "\">" + message + "</span>"));
        }
    }
}