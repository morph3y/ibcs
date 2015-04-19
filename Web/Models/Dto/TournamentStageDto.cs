using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Web.Models.Dto
{
    [DataContract]
    public class TournamentStageDto
    {
        [DataMember(Name = "id")]
        public virtual int Id { get; set; }
        [DataMember(Name = "name")]
        public virtual string Name { get; set; }
        [DataMember(Name = "order")]
        public virtual int Order { get; set; }

        [DataMember(Name = "games")]
        public virtual IList<GameDto> Games { get; set; }
    }
}