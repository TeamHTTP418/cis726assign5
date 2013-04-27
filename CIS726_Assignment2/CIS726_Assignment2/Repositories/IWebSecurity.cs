using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace CIS726_Assignment2.Repositories
{
    public interface IWebSecurity
    {
        int CurrentUserId { get; }
        string CreateUserAndAccount(string userName, string password, Object propertyValues = null, bool requireConfirmationToken = false);
        IPrincipal CurrentUser { get; }
        bool Login(string userName, string password, bool persistCookie = false);
        void Logout();
    }
}