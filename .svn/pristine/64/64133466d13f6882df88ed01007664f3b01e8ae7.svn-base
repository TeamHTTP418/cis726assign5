using CIS726_Assignment2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CIS726_Assignment2.Tests.Fakes
{
    class FakePrincipal : IPrincipal
    {
        private IIdentity identity;
        private CIS726_Assignment2.Repositories.IRoles roles;

        public FakePrincipal(string name, IRoles aRole)
        {
            identity = new FakeIdentity(name);
            roles = aRole;
        }

        public IIdentity Identity
        {
            get { return identity; }
        }

        public bool IsInRole(string role)
        {
            return roles.GetRolesForUser(identity.Name).Contains(role);
        }
    }
}
