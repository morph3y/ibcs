using System;
using System.Collections.Generic;
using Entities;

namespace Web.Models
{
    public sealed class RankingViewModel
    {
        public IList<Rank> Ranks { get; set; }
        public Type SubjectType { get; set; }
    }
}