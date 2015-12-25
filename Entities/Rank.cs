using System;

namespace Entities
{
    public class Rank
    {
        public virtual int Id { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual DateTime DateModified { get; set; }
        public virtual Game LastGame { get; set; }
        public virtual int Elo { get; set; }
    }
}
