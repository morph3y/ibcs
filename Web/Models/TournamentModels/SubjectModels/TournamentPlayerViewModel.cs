using System.Runtime.Serialization;
using Entities;

namespace Web.Models.TournamentModels.SubjectModels
{
    [DataContract]
    public class TournamentPlayerViewModel : SubjectViewModel
    {
        [DataMember(Name = "username")]
        public virtual string UserName { get; set; }
        [DataMember(Name = "isAdmin")]
        public virtual bool IsAdmin { get; set; }

        [DataMember(Name = "firstName")]
        public virtual string FirstName { get; set; }
        [DataMember(Name = "lastname")]
        public virtual string LastName { get; set; }

        public static TournamentPlayerViewModel Build(Player player)
        {
            return new TournamentPlayerViewModel
            {
                UserName = player.UserName,
                FirstName = player.FirstName,
                IsAdmin = player.IsAdmin,
                LastName = player.LastName
            };
        }
    }
}