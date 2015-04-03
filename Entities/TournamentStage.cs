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
    }
}
