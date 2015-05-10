using System.Collections.Generic;
using System.Runtime.Serialization;
using Entities;

namespace Web.Models.TournamentModels.SubjectModels
{
    [DataContract]
    public class TournamentTeamViewModel : SubjectViewModel
    {
        [DataMember(Name = "captain")]
        public virtual TournamentPlayerViewModel Captain { get; set; }
        [DataMember(Name = "members")]
        public virtual IList<TournamentPlayerViewModel> Members { get; set; }

        public static TournamentTeamViewModel Build(Team team)
        {
            var resultDto = new TournamentTeamViewModel
            {
                Captain = TournamentPlayerViewModel.Build(team.Captain),
                Members = new List<TournamentPlayerViewModel>()
            };

            foreach (var member in team.Members)
            {
                resultDto.Members.Add(TournamentPlayerViewModel.Build(member));
            }
            return resultDto;
        }
    }
}