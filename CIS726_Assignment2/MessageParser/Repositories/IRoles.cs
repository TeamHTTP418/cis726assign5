using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageParser.Repositories
{
    public interface IRoles
    {
        string[] GetAllRoles();
        string[] GetRolesForUser(string username);
        void RemoveUserFromRole(string username, string roleName);
        void AddUserToRole(string username, string roleName);
    }
}