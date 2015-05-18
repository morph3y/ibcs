using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Web.Models.Notifications
{
    [DataContract]
    public class NotificationsViewModel
    {
        public NotificationsViewModel()
        {
            MembershipRequests = new List<TeamMemberRequestViewModel>();
        }

        [DataMember(Name = "membershipRequests")]
        public IList<TeamMemberRequestViewModel> MembershipRequests { get; set; } 
    }
}