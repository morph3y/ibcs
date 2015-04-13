using System;
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
        }

        public static ISession Current
        {
            [DebuggerStepThrough]
            get { return SessionStorage.Get(); }
            [DebuggerStepThrough]
            set { SessionStorage.Save(value); }
        }
    }
}
