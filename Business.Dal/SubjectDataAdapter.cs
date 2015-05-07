using System;
using System.Linq;
using System.Linq.Expressions;

using Contracts.Business.Dal;

using Dal.Contracts;

using Entities;

namespace Business.Dal
{
    internal sealed class SubjectDataAdapter : ISubjectDataAdapter
    {
        private readonly IDataAccessAdapter _dataAccessAdapter;
        public SubjectDataAdapter(IDataAccessAdapter dataAccessAdapter)
        {
            _dataAccessAdapter = dataAccessAdapter;
        }

        public Subject Get(Expression<Func<Subject, bool>> where)
        {
            return _dataAccessAdapter.GetCollection(where).FirstOrDefault();
        }
    }
}
