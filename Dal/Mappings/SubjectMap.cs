using Entities;
using FluentNHibernate.Mapping;

namespace Dal.Mappings
{
    public class SubjectMap : ClassMap<Subject>
    {
        public SubjectMap()
        {
            Id(x => x.Id)
                .Not.Nullable()
                .GeneratedBy.Identity();

            Map(x => x.Name);
            Map(x => x.DateCreated);
            Map(x => x.Deleted);
        }
    }
}
