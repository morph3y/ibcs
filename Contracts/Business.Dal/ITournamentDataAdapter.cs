namespace Contracts.Business.Dal
{
    public interface ITournamentDataAdapter : IDataAdapter<Entities.Tournament>
    {
        bool IsInTournament(int memberId, int tournamentId);
    }
}
