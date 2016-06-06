using System;

namespace Entities
{
    public class Game
    {
        public virtual int Id { get; set; }

        public virtual TournamentStage TournamentStage { get; set; }
        public virtual TournamentGroup Group { get; set; }
        public virtual int Order { get; set; }
        public virtual GameStatus Status { get; set; }

        public virtual Subject Participant1 { get; set; }
        public virtual int Participant1Score { get; set; }
        public virtual Subject Participant2 { get; set; }
        public virtual int Participant2Score { get; set; }
        public virtual Subject Winner { get; set; }

        public override int GetHashCode()
        {
            return ("" + Participant1.GetHashCode() + Participant2.GetHashCode() + (Winner != null ? Winner.GetHashCode() : 0) + "").GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            var item = obj as Game;
            if (item == null)
            {
                return false;
            }

            return ParticipantsEqual(item)
                    && (TournamentStage != null ? TournamentStage.Equals(item.TournamentStage) : item.TournamentStage == null) && Status.Equals(item.Status) 
                    && (Winner != null ? Winner.Equals(item.Winner) : item.Winner == null) 
                   && Participant1Score.Equals(item.Participant1Score) && item.Participant2Score.Equals(item.Participant2Score);
        }

        private bool ParticipantsEqual(Game item)
        {
            if (Participant1 == null)
            {
                return (Participant2.Equals(item.Participant2) && item.Participant1 == null) || (Participant2.Equals(item.Participant1) && item.Participant2 == null) ;
            }
            else if (Participant2 == null)
            {
                return (Participant1.Equals(item.Participant1) && item.Participant2 == null) ||
                       (Participant1.Equals(item.Participant2) && item.Participant1 == null);
            }

            return (Participant1.Equals(item.Participant1) || Participant2.Equals(item.Participant1)) &&
                   (Participant2.Equals(item.Participant2) || Participant1.Equals(item.Participant2));
        }
    }

    public enum GameStatus
    {
        NotStarted = 1,
        Pending = 2,
        Finished = 3
    }
}
