using System;
using System.Linq.Expressions;

using Entities;

namespace Contracts.Business
{
    public interface ISubjectService
    {
        Subject Get(Expression<Func<Subject, bool>> where);
    }
}
