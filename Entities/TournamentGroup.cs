using System.Collections.Generic;

namespace Entities
{
    public class TournamentGroup
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual TournamentStage Stage { get; set; }
        public virtual IList<Game> Games { get; set; }
        public virtual IList<Subject> Contestants { get; set; } 

        public TournamentGroup()
        {
            Games = new List<Game>();
        }

        public override int GetHashCode()
        {
            return ("" + Id + Stage.Id.GetHashCode() + "" + (Name ?? string.Empty).GetHashCode()).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            var item = obj as TournamentGroup;
            if (item == null)
            {
                return false;
            }

            return Stage.Id.Equals(item.Stage.Id) && (Name != null ? Name.Equals(item.Name) : item.Name == null);
        }
    }
}
