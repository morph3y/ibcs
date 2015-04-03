using Entities;
using FluentNHibernate.Mapping;

namespace Dal.Mappings
{
    internal sealed class TeamMap : SubclassMap<Team>
    {
        public TeamMap()
        {
            References(x => x.Captain);
            HasMany(x => x.Members).Inverse().Cascade.All();
        }
    }
}
