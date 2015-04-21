using System;
using System.Collections.Generic;

namespace Entities
{
    public abstract class Subject
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime DateCreated { get; protected set; }
        public virtual bool Deleted { get; set; }

        public virtual IList<Tournament> ContestantIn { get; set; }
        public virtual IList<Game> WinnerOf { get; set; }
        public virtual IList<Game> ParticipantIn { get; set; }

        [Obsolete("Do not use this its hacky")]
        public virtual Subject Self { get { return this; } }

        public override int GetHashCode()
        {
            return ("" + Id.GetHashCode() + (Name ?? string.Empty).GetHashCode() + DateCreated.GetHashCode() + "").GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            var item = obj as Subject;
            if (item == null)
            {
                return false;
            }

            return Id.Equals(item.Id) && (Name != null ? Name.Equals(item.Name) : item.Name == null) && DateCreated.Equals(item.DateCreated) && Deleted.Equals(item.Deleted);
        }
    }
}
