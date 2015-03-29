using System;

namespace Entities
{
    public abstract class Subject
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual bool Deleted { get; set; }
    }
}
