using System.Collections.Generic;
using Entities;

namespace Web.Models
{
    public class TournamentDetailViewModel
    {
        public Tournament Tournament { get; set; }
        public bool UserInTournament { get; set; }
        public IEnumerable<Team> MyTeams { get; set; }

        public TournamentDetailViewModel()
        {
            MyTeams = new List<Team>();
        }
    }
}