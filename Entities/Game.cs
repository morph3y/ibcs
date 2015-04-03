namespace Entities
{
    public class Game
    {
        public virtual int Id { get; set; }

        public virtual TournamentStage TournamentStage { get; set; }
        public virtual GameStatus Status { get; set; }

        public virtual Subject Participant1 { get; set; }
        public virtual int Participant1Score { get; set; }
        public virtual Subject Participant2 { get; set; }
        public virtual int Participant2Score { get; set; }
        public virtual Subject Winner { get; set; }
    }

    public enum GameStatus
    {
        NotStarted = 1,
        Pending = 2,
        Finished = 3
    }
}
