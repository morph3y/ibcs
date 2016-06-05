using Entities;
using FluentNHibernate.Mapping;

namespace Dal.Mappings
{
    internal sealed class GameMap : ClassMap<Game>
    {
        public GameMap()
        {
            Id(x => x.Id);
            Map(x => x.Status);
            Map(x => x.Participant1Score);
            Map(x => x.Participant2Score);
            Map(x => x.Order).Column("[Order]").Default("0");

            References(x => x.TournamentStage).Nullable().Column("TournamentStage_id");
            References(x => x.Participant1).Nullable().Column("Participant1_id");
            References(x => x.Participant2).Nullable().Column("Participant2_id");
            References(x => x.Winner).Nullable().Column("Winner_id");
            References(x => x.Group).Nullable().Column("Group_Id");
        }
    }
}
