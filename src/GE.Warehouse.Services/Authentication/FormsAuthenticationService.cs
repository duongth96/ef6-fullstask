using System;
using System.Web;
using System.Web.Security;

namespace GE.Warehouse.Services.Authentication
{
    /// <summary>
    /// Authentication service
    /// </summary>
    public partial class FormsAuthenticationService : IAuthenticationService
    {
        private readonly HttpContextBase _httpContext;
        //private readonly UserSettings _UserSettings;
        private readonly TimeSpan _expirationTimeSpan;
        

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="httpContext">HTTP context</param>     
        public FormsAuthenticationService(HttpContextBase httpContext)
        {
            this._httpContext = httpContext;
            //this._UserSettings = UserSettings;
            this._expirationTimeSpan = FormsAuthentication.Timeout;
        }


        public virtual void SignIn(string username, bool createPersistentCookie)
        {
            var now = DateTime.UtcNow.ToLocalTime();

            var ticket = new FormsAuthenticationTicket(
                1 /*version*/,
                username,
                now,
                now.Add(_expirationTimeSpan),
                createPersistentCookie,
                username,
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Secure = FormsAuthentication.RequireSSL,
                Path = FormsAuthentication.FormsCookiePath
            };
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }
            FormsAuthentication.SetAuthCookie(username, true);
            _httpContext.Response.Cookies.Add(cookie);
        }

        public virtual void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}