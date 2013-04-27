using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace CIS726_Assignment2.Repositories
{
    public class WebSecurityImpl : IWebSecurity
    {
        public int CurrentUserId
        {
            get { return WebSecurity.CurrentUserId; }
        }

        public string CreateUserAndAccount(string userName, string password, object propertyValues = null, bool requireConfirmationToken = false)
        {
            return WebSecurity.CreateUserAndAccount(userName, password, propertyValues, requireConfirmationToken);
        }

        public System.Security.Principal.IPrincipal CurrentUser
        {
            get { return HttpContext.Current.User; }
        }


        public bool Login(string userName, string password, bool persistCookie = false)
        {
            return WebSecurity.Login(userName, password, persistCookie);
        }

        public void Logout()
        {
            WebSecurity.Logout();
        }
    }
}