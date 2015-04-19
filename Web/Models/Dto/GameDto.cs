using System.Runtime.Serialization;
using Entities;

namespace Web.Models.Dto
{
    [DataContract]
    public class GameDto
    {
        [DataMember(Name = "id")]
        public virtual int Id { get; set; }

        [DataMember(Name = "status")]
        public virtual GameStatus Status { get; set; }

        [DataMember(Name = "participant1")]
        public virtual SubjectDto Participant1 { get; set; }
        [DataMember(Name = "participant1Score")]
        public virtual int Participant1Score { get; set; }
        [DataMember(Name = "participant2")]
        public virtual SubjectDto Participant2 { get; set; }
        [DataMember(Name = "participant2Score")]
        public virtual int Participant2Score { get; set; }
        [DataMember(Name = "winner")]
        public virtual SubjectDto Winner { get; set; }
    }
}