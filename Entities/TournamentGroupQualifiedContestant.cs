using System;

namespace Entities
{
    public class TournamentGroupQualifiedContestant
    {
        public virtual int Id { get; set; }
        public virtual Subject Contestant { get; set; }
        public virtual TournamentGroup Group { get; set; }
        public virtual int Order { get; set; }

        public override int GetHashCode()
        {
            return (Id + Contestant.Id.GetHashCode() + Order.GetHashCode()).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            var item = obj as TournamentGroupQualifiedContestant;
            if (item == null)
            {
                return false;
            }

            return Contestant.Id.Equals(item.Contestant.Id) && Id.Equals(item.Id) && Order.Equals(item.Order) ;
        }
    }
}
