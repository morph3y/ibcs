using System.Runtime.Serialization;

namespace Web.Models.TeamModels
{
    [DataContract]
    public class TeamMemberViewModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "accepted")]
        public bool Accepted { get; set; }
    }
}