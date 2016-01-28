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
            Map(x => x.PointsForTie).Default("3");
            Map(x => x.PointsForWin).Default("1");
            Map(x => x.IsRanked).Default("0");
            Map(x => x.IsTeamEvent).Default("0");

            References(x => x.Parent).Nullable();

            HasMany(x => x.Stages).Cascade.AllDeleteOrphan();
            HasManyToMany(x => x.Contestants).Cascade.SaveUpdate();
        }
    }
}
