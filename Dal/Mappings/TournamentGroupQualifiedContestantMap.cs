using System;

using Entities;

using FluentNHibernate.Mapping;

namespace Dal.Mappings
{
    internal sealed class TournamentGroupQualifiedContestantMap : ClassMap<TournamentGroupQualifiedContestant>
    {
        public TournamentGroupQualifiedContestantMap()
        {
            Id(x => x.Id);
            Map(x => x.Order).Column("[Order]");

            References(x => x.Group).Not.Nullable();
            References(x => x.Contestant).Not.Nullable();
        }
    }
}
