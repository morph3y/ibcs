using System.Diagnostics;
using System.Web;

namespace Framework.Session
{
    public static class Session
    {
        public static PlayerPrincipal Current
        {
            [DebuggerStepThrough]
            get { return (PlayerPrincipal) HttpContext.Current.User; }
            [DebuggerStepThrough]
            internal set { HttpContext.Current.User = value; }
        }
    }
}
