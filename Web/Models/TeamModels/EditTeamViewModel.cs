using System.Collections.Generic;
using System.Runtime.Serialization;
using Web.Models.TournamentModels.SubjectModels;

namespace Web.Models.TeamModels
{
    [DataContract]
    public class EditTeamViewModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "captain")]
        public TournamentPlayerViewModel Captain { get; set; }

        public bool IsAuthorized { get; set; }

        [DataMember(Name = "members")]
        public IList<TeamMemberViewModel> Members { get; set; } 
    }
}