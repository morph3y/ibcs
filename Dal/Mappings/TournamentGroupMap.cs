using Entities;
using FluentNHibernate.Mapping;

namespace Dal.Mappings
{
    internal sealed class TournamentGroupMap : ClassMap<TournamentGroup>
    {
        public TournamentGroupMap()
        {
            Id(x => x.Id);
            Map(x => x.Name).Not.Nullable();

            References(x => x.Stage).Not.Nullable();

            HasMany(x => x.Games).KeyColumn("Group_Id").Cascade.AllDeleteOrphan();
            HasManyToMany(x => x.Contestants).Cascade.SaveUpdate();
            HasMany(x => x.QualifiedContestants).KeyColumn("Group_Id").Inverse().Cascade.AllDeleteOrphan();
        }
    }
}
