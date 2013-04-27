using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using AuthParser.Models;
using MessageParser;

namespace CIS726_Assignment2.Repositories
{
    public class MSMQRoleProvider : RoleProvider
    {

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            foreach (string username in usernames)
            {
                foreach (string rolename in roleNames)
                {
                    if (!IsUserInRole(username, rolename))
                    {
                        if (!RoleExists(rolename))
                        {
                            CreateRole(rolename);
                        }
                        int userID = getIDforUser(username);
                        int roleID = getIDforRole(rolename);
                        UserRoles userroles = new UserRoles()
                        {
                            userID = userID,
                            roleID = roleID
                        };
                        Request<UserRoles>.AddUserRole(userroles, "A", "B");
                    }
                }
            }
        }

        private string applicationName = "CIS726";

        public override string ApplicationName
        {
            get
            {
                return applicationName;
            }
            set
            {
                applicationName = value;
            }
        }

        public override void CreateRole(string roleName)
        {
            if(getIDforRole(roleName) < 0){
                Role role = new Role()
                {
                    rolename = roleName,
                };
                Request<Role>.AddUserRole(role, "A", "B");
            }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            int id = getIDforRole(roleName);
            if (id >= 0)
            {
                return Request<Role>.DeleteUserRole(id, "A", "B");
            }
            return false;
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            return GetUsersInRole(roleName);
        }

        public override string[] GetAllRoles()
        {
            List<Role> roles = Request<Role>.GetAllUserRoles("A", "B");
            List<string> rolelist = new List<string>();
            foreach (Role role in roles)
            {
                rolelist.Add(role.rolename);
            }
            return rolelist.ToArray<string>();
        }

        public override string[] GetRolesForUser(string username)
        {
            int ID = getIDforUser(username);
            if (ID >= 0)
            {
                List<Role> userroles = Request<Role>.GetRolesForUser(ID, "A", "B");
                List<string> rolelist = new List<string>();
                foreach (Role role in userroles)
                {
                    rolelist.Add(role.rolename);
                }
                return rolelist.ToArray<string>();
            }
            return new string[0];
        }

        public override string[] GetUsersInRole(string roleName)
        {
            int ID = getIDforRole(roleName);
            if (ID >= 0)
            {
                List<User> userroles = Request<User>.GetRolesForUser(ID, "A", "B");
                List<string> rolelist = new List<string>();
                foreach (User role in userroles)
                {
                    rolelist.Add(role.username);
                }
                return rolelist.ToArray<string>();
            }
            return new string[0];
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            string[] roles = GetRolesForUser(username);
            foreach (string role in roles)
            {
                if (role.Equals(roleName))
                {
                    return true;
                }
            }
            return false;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            foreach (string username in usernames)
            {
                int userID = getIDforUser(username);
                List<UserRoles> userroles = Request<UserRoles>.GetRolesForUser(userID, "A", "B");
                foreach (UserRoles ur in userroles)
                {
                    if (roleNames.Contains(ur.role.rolename))
                    {
                        Request<UserRoles>.DeleteUserRole(ur.ID, "A", "B");
                    }
                }
            }
        }

        public override bool RoleExists(string roleName)
        {
            if (getIDforRole(roleName) >= 0)
            {
                return true;
            }
            return false;
        }

        private int getIDforUser(string username)
        {
            User user = Request<User>.GetUserRoleByName(username, "A", "B");
            if (user != null)
            {
                return user.ID;
            }
            return -1;
        }

        private int getIDforRole(string rolename)
        {
            Role role = Request<Role>.GetUserRoleByName(rolename, "A", "B");
            if (role != null)
            {
                return role.ID;
            }
            return -1;
        }

    }
}