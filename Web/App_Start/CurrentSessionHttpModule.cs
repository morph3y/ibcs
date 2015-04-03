using System.Web;
using Contracts.Dal;

namespace Web
{
    public class CurrentSessionHttpModule : IHttpModule
    {
        private readonly IDbSessionManager _sessionManager;

        public CurrentSessionHttpModule(IDbSessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public void Init(HttpApplication context)
        {
            context.EndRequest += (sesnder, e) => _sessionManager.Close();
        }

        public void Dispose()
        { }
    }
}