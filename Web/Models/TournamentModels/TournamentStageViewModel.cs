using System.Collections.Generic;
using System.Runtime.Serialization;
using Entities;

namespace Web.Models.TournamentModels
{
    [DataContract]
    public class TournamentStageViewModel
    {
        [DataMember(Name = "id")]
        public virtual int Id { get; set; }
        [DataMember(Name = "name")]
        public virtual string Name { get; set; }
        [DataMember(Name = "order")]
        public virtual int Order { get; set; }

        [DataMember(Name = "games")]
        public virtual IList<GameViewModel> Games { get; set; }

        public static TournamentStageViewModel Build(TournamentStage stage)
        {
            var viewModel = new TournamentStageViewModel
            {
                Id = stage.Id,
                Name = stage.Name,
                Order = stage.Order,
                Games = new List<GameViewModel>()
            };

            foreach (var game in stage.Games)
            {
                viewModel.Games.Add(GameViewModel.Build(game));
            }

            return viewModel;
        }
    }
}