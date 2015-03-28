using Entities;
using FluentNHibernate.Mapping;

namespace Dal.Mappings
{
    internal sealed class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id);
            Map(x => x.UserName);
            Map(x => x.Passsword);
            Map(x => x.FirstName);
            Map(x => x.LastName);
        }
    }
}
