using System;

using Contracts.Framework;

namespace Framework.Session
{
    internal sealed class NullSession : ISession
    {
        private bool _isNullSession = true;
        public bool IsNullSession
        {
            get { return _isNullSession; }
            set { _isNullSession = true; }
        }

        public int PlayerId
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public bool IsAdmin()
        {
            return false;
        }
    }
}
