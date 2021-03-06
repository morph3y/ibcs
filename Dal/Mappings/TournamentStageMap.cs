﻿using Entities;
using FluentNHibernate.Mapping;

namespace Dal.Mappings
{
    internal sealed class TournamentStageMap : ClassMap<TournamentStage>
    {
        public TournamentStageMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Order).Column("[Order]");

            References(x => x.Tournament).Nullable();
            HasMany(x => x.Games).Cascade.AllDeleteOrphan();
            HasMany(x => x.Groups).Cascade.AllDeleteOrphan();
        }
    }
}
