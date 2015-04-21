namespace Entities
{
    public class Player : Subject
    {
        public virtual string UserName { get; set; }
        public virtual string Passsword { get; set; }
        public virtual bool IsAdmin { get; set; }

        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }

        public override int GetHashCode()
        {
            return ("" + base.GetHashCode() + (UserName ?? string.Empty).GetHashCode() + IsAdmin.GetHashCode() + (FirstName ?? string.Empty).GetHashCode() + (LastName ?? string.Empty).GetHashCode() + "").GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            var item = obj as Player;
            if (item == null)
            {
                return false;
            }

            return base.Equals(obj) && (FirstName != null ? FirstName.Equals(item.FirstName) : item.FirstName == null)
                && (LastName != null ? LastName.Equals(item.LastName) : item.LastName == null) && IsAdmin.Equals(item.IsAdmin)
                && (UserName != null ? UserName.Equals(item.UserName) : item.UserName == null);
        }
    }
}
