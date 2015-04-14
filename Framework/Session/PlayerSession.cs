using System;
using Contracts.Framework;
using Entities;

namespace Framework.Session
{
    internal sealed class PlayerSession : ISession
    {
        private readonly Player _player;
        public PlayerSession(Player player)
        {
            _player = player;
        }

        private bool _isNullSession;
        public bool IsNullSession
        {
            get { return _isNullSession; }
            set { _isNullSession = false; }
        }

        public Player Player
        {
            get { return _player; }
            set { throw new NotSupportedException(); }
        }

        public bool IsAdmin()
        {
            return _player.IsAdmin;
        }
    }
}
