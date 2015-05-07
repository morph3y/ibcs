using System;

using Entities;

namespace Contracts.Business.Dal
{
    public interface ITournamentDataAdapter : IDataAdapter<Tournament>
    {
        bool IsInTournament(int memberId, int tournamentId);
    }
}
