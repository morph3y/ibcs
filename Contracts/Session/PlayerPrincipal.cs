using System.Security.Principal;

namespace Contracts.Session
{
    public class PlayerPrincipal : IPrincipal
    {
        public PlayerPrincipal(string userName)
        {
            Identity = new GenericIdentity(userName);
        }

        public bool IsInRole(string role)
        {
            return false;
        }

        public IIdentity Identity { get; private set; }

        public int Id { get; set; }
        public string UserName { get; set; }
        public bool IsAdmin { get; set; }
    }
}
