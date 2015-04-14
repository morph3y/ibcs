using System;
using Contracts.Framework;
using Entities;
using Framework.Session;

namespace Framework
{
    internal sealed class SessionManager : ISessionManager
    {
        public void CreateOrValidate(Player player)
        {
            var session = Session.Session.Current;
            bool isValid;
            if (session.IsNullSession)
            {
                CreateSession(player);
                isValid = true;
            }
            else
            {
                isValid = ValidateSession(session, player);
            }

            if (!isValid)
            {
                throw new Exception("Session is invalid");
            }
        }

        public void Destroy()
        {
            Session.Session.Current = new NullSession();
        }

        private bool ValidateSession(ISession session, Player player)
        {
            return player != null && !session.IsNullSession && session.Player != null && session.Player.Id == player.Id;
        }

        private void CreateSession(Player player)
        {
            Session.Session.Current = new PlayerSession(player);
        }
    }
}
