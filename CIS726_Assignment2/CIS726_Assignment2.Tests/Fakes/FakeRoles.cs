using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS726_Assignment2.Repositories;

namespace CIS726_Assignment2.Tests.Fakes
{
    class FakeRoles : IRoles
    {
        List<string> adminRoles;
        List<string> advisorRoles;

        public FakeRoles()
        {
            adminRoles = new List<string>();
            adminRoles.Add("Administrator");
            advisorRoles = new List<string>();
            advisorRoles.Add("Advisor");
        }

        public string[] GetAllRoles()
        {
            string[] temp = { "Administrator", "Advisor" };
            return temp;
        }

        public string[] GetRolesForUser(string username)
        {
            if(username.Equals("Administrator")){
                return adminRoles.ToArray();
            }
            else if (username.Equals("Advisor"))
            {
                return advisorRoles.ToArray();
            }
            else
            {
                return new string[0];
            }
        }

        public void RemoveUserFromRole(string username, string roleName)
        {
            if (username.Equals("Administrator"))
            {
                adminRoles.Remove(roleName);
            }
            else if (username.Equals("Advisor"))
            {
                advisorRoles.Remove(roleName);
            }
        }

        public void AddUserToRole(string username, string roleName)
        {
            if (username.Equals("Administrator"))
            {
                adminRoles.Add(roleName);
            }
            else if (username.Equals("Advisor"))
            {
                advisorRoles.Add(roleName);
            }
        }
    }
}
