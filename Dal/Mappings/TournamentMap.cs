using Entities;
using FluentNHibernate.Mapping;

namespace Dal.Mappings
{
    internal sealed class TournamentMap : ClassMap<Tournament>
    {
        public TournamentMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.TournamentType);
            Map(x => x.Status);

            HasMany(x => x.Stages).Cascade.AllDeleteOrphan();
            HasManyToMany(x => x.Contestants).Inverse().Cascade.AllDeleteOrphan();
        }
    }
}
