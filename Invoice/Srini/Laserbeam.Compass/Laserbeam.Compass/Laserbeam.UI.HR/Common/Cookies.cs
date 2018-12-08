using System;
using System.Linq;
using System.Web;

namespace Laserbeam.UI.HR.Controllers
{
    public static class Cookies
    {
        public static void CreateCookie<T>(string cookieName,T cookieValue)
        {
             HttpCookie cookie = new HttpCookie(cookieName);
             cookie.Value =Convert.ToString(cookieValue);
             cookie.Expires = DateTime.Now.AddDays(365);
             HttpContext.Current.Response.Cookies.Add(cookie); 
        }
        public static void CreateCookie<T>(string cookieName, T cookieValue, int expirationDays)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Value = Convert.ToString(cookieValue);
            cookie.Expires = DateTime.Now.AddDays(expirationDays);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        public static T GetCookie<T>(string cookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            Type type = typeof(T);
            if (cookie != null)
            {
                
                return (T)Convert.ChangeType(cookie.Value, type);
            }
            return (T)type.GetConstructors().First().Invoke(new object[]{});
        }
        public static void DeleteCookie(string cookieName)
        {
            HttpContext.Current.Request.Cookies.Remove(cookieName);
        }
    }
}