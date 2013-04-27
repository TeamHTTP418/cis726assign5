using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MessageParser.Repositories
{
    public class RolesImpl : IRoles
    {
        public string[] GetAllRoles()
        {
            return Roles.GetAllRoles();
        }

        public string[] GetRolesForUser(string username)
        {
            return Roles.GetRolesForUser(username);
        }

        public void RemoveUserFromRole(string username, string roleName)
        {
            Roles.RemoveUserFromRole(username, roleName);
        }

        public void AddUserToRole(string username, string roleName)
        {
            Roles.AddUserToRole(username, roleName);
        }
    }
}