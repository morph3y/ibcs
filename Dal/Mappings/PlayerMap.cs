using Entities;
using FluentNHibernate.Mapping;

namespace Dal.Mappings
{
    internal sealed class PlayerMap : SubclassMap<Player>
    {
        public PlayerMap()
        {
            Map(x => x.UserName).Not.Nullable();
            Map(x => x.Passsword).Length(4001).Not.Nullable();
            Map(x => x.IsAdmin).ReadOnly().Default("0").Not.Nullable();
            Map(x => x.FirstName);
            Map(x => x.LastName);

            HasManyToMany(x => x.Teams).Inverse().Cascade.SaveUpdate();
        }
    }
}
