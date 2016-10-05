using System;
using System.Runtime.Serialization;

using Entities;

using Web.Models.TournamentModels.SubjectModels;

namespace Web.Models.TournamentModels
{
    [DataContract]
    public class TournamentGroupQualifiedContestantViewModel
    {
        [DataMember(Name = "id")]
        public virtual int Id { get; set; }

        [DataMember(Name = "order")]
        public virtual int Order { get; set; }

        [DataMember(Name = "contestant")]
        public virtual SubjectViewModel Contestant { get; set; }

        [DataMember(Name = "group")]
        public virtual TournamentGroupViewModel Group { get; set; }

        public static TournamentGroupQualifiedContestantViewModel Build(TournamentGroupQualifiedContestant qualifiedContestant)
        {
            return new TournamentGroupQualifiedContestantViewModel
            {
                Id = qualifiedContestant.Id,
                Order = qualifiedContestant.Order,
                Contestant = SubjectViewModel.Build(qualifiedContestant.Contestant)
            };
        }
    }
}