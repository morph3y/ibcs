namespace Entities
{
    public class TeamMemberRequest
    {
        public virtual int Id { get; set; }
        public virtual Team Team { get; set; }
        public virtual Player Member { get; set; }

        public override int GetHashCode()
        {
            return ("" + Team.GetHashCode() + Member.GetHashCode() + "").GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            var item = obj as TeamMemberRequest;
            if (item == null)
            {
                return false;
            }

            return Team.Equals(item.Team) && Member.Equals(item.Member);
        }
    }
}
