using System;

namespace Entities
{
    public abstract class Subject
    {
        protected Subject()
        {
            DateCreated = DateTime.Now;
        }

        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime DateCreated { get; protected set; }
        public virtual bool Deleted { get; set; }
    }
}
