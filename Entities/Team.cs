using System.Collections.Generic;

namespace Entities
{
    public class Team : Subject
    {
        public virtual Player Captain { get; set; }
        public virtual IList<Player> Members { get; set; }

        public override int GetHashCode()
        {
            return ("" + base.GetHashCode() + (Captain != null ? Captain.GetHashCode() : 0) + "").GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            var item = obj as Team;
            if (item == null)
            {
                return false;
            }

            return base.Equals(obj) && (Captain != null ? Captain.Equals(item.Captain) : item.Captain == null);
        }
    }
}
