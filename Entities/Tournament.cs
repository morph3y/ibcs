using System.Collections.Generic;

namespace Entities
{
    public class Tournament
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual TournamentStatus Status { get; set; }
        public virtual TournamentType TournamentType { get; set; }

        public virtual IList<Subject> Contestants { get; set; }
        public virtual IList<TournamentStage> Stages { get; set; }
    }

    public enum TournamentType
    {
        League = 1,
        SingleElimination = 2
    }

    public enum TournamentStatus
    {
        Registration = 1,
        Active = 2,
        Closed = 3
    }
}
