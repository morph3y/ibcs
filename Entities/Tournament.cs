using System.Collections.Generic;
using System.ComponentModel;

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
        [Description("League")]
        League = 1,
        [Description("Single Elimination")]
        SingleElimination = 2
    }

    public enum TournamentStatus
    {
        [Description("Registration")]
        Registration = 1,
        [Description("Active")]
        Active = 2,
        [Description("Closed")]
        Closed = 3
    }
}
