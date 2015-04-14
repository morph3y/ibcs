namespace Entities
{
    public class Player : Subject
    {
        public virtual string UserName { get; set; }
        public virtual string Passsword { get; set; }
        public virtual bool IsAdmin { get; set; }

        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
    }
}
