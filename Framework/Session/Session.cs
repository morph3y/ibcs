using System.Diagnostics;
using Contracts.Framework;

namespace Framework.Session
{
    public static class Session
    {
        private static readonly HttpContextSessionStorage<ISession> SessionStorage;
        static Session()
        {
            SessionStorage = new HttpContextSessionStorage<ISession>("ibcsISession");
            SessionStorage.Save(new NullSession());
        }

        public static ISession Current
        {
            [DebuggerStepThrough]
            get { return SessionStorage.Get(); }
            [DebuggerStepThrough]
            internal set { SessionStorage.Save(value); }
        }
    }
}
