using System;
using Contracts.Framework;
using Entities;

namespace Framework.Session
{
    internal sealed class PlayerSession : ISession
    {
        private readonly int _playerId;
        public PlayerSession(Player player)
        {
            _playerId = player.Id;
        }

        private bool _isNullSession;
        public bool IsNullSession
        {
            get { return _isNullSession; }
            set { _isNullSession = false; }
        }

        public int PlayerId
        {
            get { return _playerId; }
            set { throw new NotSupportedException(); }
        }
    }
}
