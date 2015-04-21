using System.Collections.Generic;

namespace Entities
{
    public class TournamentStage
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int Order { get; set; }

        public virtual Tournament Tournament { get; set; }
        public virtual IList<Game> Games { get; set; }

        public TournamentStage()
        {
            Games = new List<Game>();
        }

        public override int GetHashCode()
        {
            return ("" + Id.GetHashCode() + (Name ?? string.Empty).GetHashCode() + Order.GetHashCode() + (Tournament != null ? Tournament.GetHashCode() : 0) + "").GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            var item = obj as TournamentStage;
            if (item == null)
            {
                return false;
            }

            return Id.Equals(item.Id) && (Name != null ? Name.Equals(item.Name) : item.Name == null) && Order.Equals(item.Order) && 
                (Tournament != null ? Tournament.Equals(item.Tournament) : item.Tournament == null);
        }
    }
}
