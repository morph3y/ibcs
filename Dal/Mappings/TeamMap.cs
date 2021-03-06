﻿using Entities;
using FluentNHibernate.Mapping;

namespace Dal.Mappings
{
    internal sealed class TeamMap : SubclassMap<Team>
    {
        public TeamMap()
        {
            References(x => x.Captain);
            HasManyToMany(x => x.Members);
            HasMany(x => x.MemberRequests).Cascade.AllDeleteOrphan().KeyColumn("TeamId");
        }
    }
}
