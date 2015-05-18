using System.Runtime.Serialization;

namespace Web.Models.Notifications
{
    [DataContract]
    public class TeamMemberRequestViewModel
    {
        [DataMember(Name = "teamId")]
        public int TeamId { get; set; }
        [DataMember(Name = "teamName")]
        public string TeamName { get; set; }

        [DataMember(Name = "memberId")]
        public int MemberId { get; set; }
        [DataMember(Name = "memberName")]
        public string MemberName { get; set; }
    }
}