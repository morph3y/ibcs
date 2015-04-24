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

        // TODO: move if grows 
        // Settings
        public virtual int PointsForWin { get; set; }
        public virtual int PointsForTie { get; set; }

        public Tournament()
        {
            Stages = new List<TournamentStage>();
            Contestants = new List<Subject>();
        }

        public override int GetHashCode()
        {
            return ("" + Id.GetHashCode() + (Name ?? string.Empty).GetHashCode() + Status.GetHashCode() + TournamentType.GetHashCode() + "").GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            var item = obj as Tournament;
            if (item == null)
            {
                return false;
            }

            return Id.Equals(item.Id) && (Name != null ? Name.Equals(item.Name) : item.Name == null) && Status.Equals(item.Status) && TournamentType.Equals(item.TournamentType);
        }
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
