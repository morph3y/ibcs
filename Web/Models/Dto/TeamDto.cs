using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Web.Models.Dto
{
    [DataContract]
    public class TeamDto : SubjectDto
    {
        [DataMember(Name = "captain")]
        public virtual PlayerDto Captain { get; set; }
        [DataMember(Name = "members")]
        public virtual IList<PlayerDto> Members { get; set; }
    }
}