using System.Diagnostics;
using System.Web;
using Framework.Session;

namespace Contracts.Session
{
    public static class Session
    {
        public static PlayerPrincipal Current
        {
            [DebuggerStepThrough]
            get { return HttpContext.Current.User as PlayerPrincipal; }
            [DebuggerStepThrough]
            internal set { HttpContext.Current.User = value; }
        }
    }
}
