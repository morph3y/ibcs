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

        public virtual Subject Self { get { return this; } }
    }
}
