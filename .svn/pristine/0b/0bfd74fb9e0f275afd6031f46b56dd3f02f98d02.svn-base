using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CIS726_Assignment2.Tests.Fakes
{
    class FakeIdentity : IIdentity
    {
        string name;

        public FakeIdentity(string aName)
        {
            name = aName;
        }

        public string AuthenticationType
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string Name
        {
            get { return name; }
        }
    }
}
