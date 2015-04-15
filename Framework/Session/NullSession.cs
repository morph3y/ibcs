using Contracts.Framework;
using Entities;

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

        private Player _player;
        public Player Player
        {
            get { return _player; }
            set { _player = null; }
        }

        public bool IsAdmin()
        {
            return false;
        }
    }
}
