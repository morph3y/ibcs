using Entities;
using FluentNHibernate.Mapping;

namespace Dal.Mappings
{
    public class SubjectMap : ClassMap<Subject>
    {
        public SubjectMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.DateCreated).Not.Nullable().Not.Insert().Not.Update().Default("getdate()");
            Map(x => x.Deleted).Default("0");
            HasManyToMany(x => x.ContestantIn).Cascade.All();
        }
    }
}
