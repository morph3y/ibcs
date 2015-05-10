using System.Collections.Generic;
using System.Runtime.Serialization;
using Entities;

namespace Web.Models.TournamentModels
{
    [DataContract]
    public class TournamentBracketViewModel
    {
        [DataMember(Name = "id")]
        public virtual int Id { get; set; }
        [DataMember(Name = "name")]
        public virtual string Name { get; set; }
        [DataMember(Name = "status")]
        public virtual TournamentStatus Status { get; set; }
        [DataMember(Name = "type")]
        public virtual TournamentType TournamentType { get; set; }
        [DataMember(Name = "isRanked")]
        public virtual bool IsRanked { get; set; }

        [DataMember(Name = "stages")]
        public virtual IList<TournamentStageViewModel> Stages { get; set; }

        [DataMember(Name = "pointsForWin")]
        public virtual int PointsForWin { get; set; }

        [DataMember(Name = "pointsForTie")]
        public virtual int PointsForTie { get; set; }
        [DataMember(Name = "isTeamEvent")]
        public bool IsTeamEvent { get; set; }

        public static TournamentBracketViewModel Build(Tournament tournament)
        {
            var viewModel = new TournamentBracketViewModel
            {
                Id = tournament.Id,
                Name = tournament.Name,
                Status = tournament.Status,
                TournamentType = tournament.TournamentType,
                Stages = new List<TournamentStageViewModel>(),
                IsRanked = tournament.IsRanked,
                PointsForTie = tournament.PointsForTie,
                PointsForWin = tournament.PointsForWin
            };

            foreach (var stage in tournament.Stages)
            {
                viewModel.Stages.Add(TournamentStageViewModel.Build(stage));
            }

            return viewModel;
        }
    }
}