using System.Web;
using Contracts.Dal;

namespace Web
{
    public class CurrentSessionHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.EndRequest += (sesnder, e) => ((IDbSessionManager)(NinjectWebCommon.GetKernel().GetService(typeof(IDbSessionManager)))).Close();
        }

        public void Dispose()
        { }
    }
}