using System.Collections.Generic;
using System.Runtime.Serialization;
using Entities;
using Web.Models.TournamentModels.SubjectModels;

namespace Web.Models.TournamentModels
{
    [DataContract]
    public class TournamentGroupViewModel
    {
        [DataMember(Name = "id")]
        public virtual int Id { get; set; }

        [DataMember(Name = "name")]
        public virtual string Name { get; set; }

        [DataMember(Name = "games")]
        public virtual IList<GameViewModel> Games { get; set; }

        [DataMember(Name = "contestants")]
        public virtual IList<SubjectViewModel> Contestants { get; set; } 

        public static TournamentGroupViewModel Build(TournamentGroup group)
        {
            var model = new TournamentGroupViewModel
            {
                Games = new List<GameViewModel>(),
                Contestants = new List<SubjectViewModel>(),
                Id = group.Id,
                Name = group.Name
            };

            foreach (var game in group.Games)
            {
                model.Games.Add(GameViewModel.Build(game));
            }

            foreach (var contenstant in group.Contestants)
            {
                model.Contestants.Add(SubjectViewModel.Build(contenstant));
            }

            return model;
        }
    }
}