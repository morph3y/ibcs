using System;
using System.Linq.Expressions;

using Contracts.Business;
using Contracts.Business.Dal;

using Entities;

namespace Business
{
    internal sealed class SubjectService : ISubjectService
    {
        private readonly ISubjectDataAdapter _subjectDataAdapter;
        public SubjectService(ISubjectDataAdapter subjectDataAdapter)
        {
            _subjectDataAdapter = subjectDataAdapter;
        }

        public Subject Get(Expression<Func<Subject, bool>> @where)
        {
            return _subjectDataAdapter.Get(where);
        }
    }
}
