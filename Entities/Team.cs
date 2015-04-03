using System.Collections.Generic;

namespace Entities
{
    public class Team : Subject
    {
        public virtual Player Captain { get; set; }
        public virtual IList<Player> Members { get; set; }
    }
}
