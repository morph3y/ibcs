using System.Runtime.Serialization;
using Web.Models.TournamentModels.SubjectModels;

namespace Web.Models.TeamModels
{
    [DataContract]
    public class TeamMemberRequestViewModel
    {
        [DataMember(Name = "team")]
        public TournamentTeamViewModel Team { get; set; }

        [DataMember(Name = "member")]
        public TournamentPlayerViewModel Member { get; set; }
    }
}