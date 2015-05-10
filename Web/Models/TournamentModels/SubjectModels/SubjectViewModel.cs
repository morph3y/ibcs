using System;
using System.Runtime.Serialization;
using Entities;

namespace Web.Models.TournamentModels.SubjectModels
{
    [DataContract]
    [KnownType(typeof(TournamentTeamViewModel))]
    [KnownType(typeof(TournamentPlayerViewModel))]
    public class SubjectViewModel
    {
        [DataMember(Name = "id")]
        public virtual int Id { get; set; }
        [DataMember(Name = "name")]
        public virtual string Name { get; set; }
        [DataMember(Name = "dataCreated")]
        public virtual DateTime DateCreated { get; set; }
        [DataMember(Name = "deleted")]
        public virtual bool Deleted { get; set; }

        public static SubjectViewModel Build(Subject subject)
        {
            SubjectViewModel viewModel;
            if (subject == null)
            {
                return null;
            }

            if (subject.Self.GetType() == typeof(Player))
            {
                viewModel = TournamentPlayerViewModel.Build((Player)subject.Self);
            }
            else if (subject.Self.GetType() == typeof(Team))
            {
                viewModel = TournamentTeamViewModel.Build((Team)subject.Self);
            }
            else
            {
                throw new NotSupportedException("Type of subject is not supported");
            }
            viewModel.Id = subject.Id;
            viewModel.Deleted = subject.Deleted;
            viewModel.Name = subject.Name;
            viewModel.DateCreated = subject.DateCreated;

            return viewModel;
        }
    }
}