using System.Collections.Generic;
using Entities;

namespace Web.Areas.Admin.Models
{
    public class ContestantsListModel
    {
        public List<Subject> Contestants { get; set; }
        public List<Subject> OtherSubjects { get; set; }
        public int TournamentId { get; set; }
    }
}