using Entities;
using FluentNHibernate.Mapping;

namespace Dal.Mappings
{
    internal sealed class RankMap : ClassMap<Rank>
    {
        public RankMap()
        {
            Id(x => x.Id);
            Map(x => x.DateModified);
            Map(x => x.Elo);

            References(x => x.LastGame).Column("Game_Id");
            References(x => x.Subject).Column("Subject_Id").Not.Nullable();
        }
    }
}
