﻿namespace Entities
{
    public class User
    {
        public virtual int Id { get; set; }

        public virtual string UserName { get; set; }
        public virtual string Passsword { get; set; }

        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
    }
}
