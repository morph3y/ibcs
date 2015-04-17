using System.Security.Principal;

namespace Framework.Session
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
    }
}
