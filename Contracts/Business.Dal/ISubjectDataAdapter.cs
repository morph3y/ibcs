using System;
using System.Linq.Expressions;

using Entities;

namespace Contracts.Business.Dal
{
    public interface ISubjectDataAdapter
    {
        Subject Get(Expression<Func<Subject, bool>> where);
    }
}
