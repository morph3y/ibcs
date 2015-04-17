using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using Framework.Session;

namespace Web.Infrastructure
{
    internal static class CookieManager
    {
        public static HttpCookie CreateCookie(PlayerPrincipal principal)
        {
            var serializableModel = new PrincipalModel
            {
                Id = principal.Id,
                UserName = principal.Identity.Name
            };

            var authTicket = new FormsAuthenticationTicket(1, serializableModel.UserName, DateTime.Now, DateTime.Now.AddMinutes(15), false, new JavaScriptSerializer().Serialize(serializableModel));

            var encTicket = FormsAuthentication.Encrypt(authTicket);
            return new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
        }
    }
}