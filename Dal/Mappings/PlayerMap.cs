using Entities;
using FluentNHibernate.Mapping;

namespace Dal.Mappings
{
    internal sealed class PlayerMap : SubclassMap<Player>
    {
        public PlayerMap()
        {
            Map(x => x.UserName);
            Map(x => x.Passsword).Length(4001);
            Map(x => x.FirstName);
            Map(x => x.LastName);
        }
    }
}
