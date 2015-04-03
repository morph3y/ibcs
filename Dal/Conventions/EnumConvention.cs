using System;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Dal.Conventions
{
    public class EnumConvention : IUserTypeConvention
    {
        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(x => x.Property.PropertyType.IsEnum ||
                (x.Property.PropertyType.IsGenericType &&
                 x.Property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                 x.Property.PropertyType.GetGenericArguments()[0].IsEnum)
                );
        }

        public void Apply(IPropertyInstance instance)
        {
            instance.CustomType(instance.Property.PropertyType);
        }

        public bool Accept(Type type)
        {
            return type.IsEnum;
        }
    }
}
