using System.Diagnostics;
using System.Threading;
using System.Web;

namespace Contracts.Session
{
    public static class Session
    {
        public static PlayerPrincipal Current
        {
            [DebuggerStepThrough] get
            {
                if (HttpContext.Current != null)
                {
                    return HttpContext.Current.User as PlayerPrincipal;
                }
                return Thread.CurrentPrincipal as PlayerPrincipal;
            }
            [DebuggerStepThrough] internal set
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.User = value;
                }
                else
                {
                    Thread.CurrentPrincipal = value;
                }
            }
        }
    }
}
