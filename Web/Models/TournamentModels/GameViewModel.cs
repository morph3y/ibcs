using System.Runtime.Serialization;
using Entities;
using Web.Models.TournamentModels.SubjectModels;

namespace Web.Models.TournamentModels
{
    [DataContract]
    public class GameViewModel
    {
        [DataMember(Name = "id")]
        public virtual int Id { get; set; }

        [DataMember(Name = "status")]
        public virtual GameStatus Status { get; set; }

        [DataMember(Name = "participant1")]
        public virtual SubjectViewModel Participant1 { get; set; }
        [DataMember(Name = "participant1Score")]
        public virtual int Participant1Score { get; set; }
        [DataMember(Name = "participant2")]
        public virtual SubjectViewModel Participant2 { get; set; }
        [DataMember(Name = "participant2Score")]
        public virtual int Participant2Score { get; set; }
        [DataMember(Name = "winner")]
        public virtual SubjectViewModel Winner { get; set; }

        public static GameViewModel Build(Game game)
        {
            return new GameViewModel
            {
                Id = game.Id,
                Participant1 = SubjectViewModel.Build(game.Participant1),
                Participant1Score = game.Participant1Score,
                Participant2 = SubjectViewModel.Build(game.Participant2),
                Participant2Score = game.Participant2Score,
                Status = game.Status,
                Winner = SubjectViewModel.Build(game.Winner)
            };
        }
    }
}