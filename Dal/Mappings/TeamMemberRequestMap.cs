using Entities;
using FluentNHibernate.Mapping;

namespace Dal.Mappings
{
    internal sealed class TeamMemberRequestMap : ClassMap<TeamMemberRequest>
    {
        public TeamMemberRequestMap()
        {
            Id(x => x.Id);

            References(x => x.Member).Not.Nullable().Column("MemberId");
            References(x => x.Team).Not.Nullable().Column("TeamId");
        }
    }
}
