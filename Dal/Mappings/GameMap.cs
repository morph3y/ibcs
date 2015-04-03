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

            References(x => x.TournamentStage);
            References(x => x.Participant1);
            References(x => x.Participant2);
            References(x => x.Winner);
        }
    }
}
