using System.Collections.Generic;
using System.Runtime.Serialization;
using Entities;

namespace Web.Models.Dto
{
    [DataContract]
    public class TournamentDto
    {
        [DataMember(Name = "id")]
        public virtual int Id { get; set; }
        [DataMember(Name = "name")]
        public virtual string Name { get; set; }
        [DataMember(Name = "status")]
        public virtual TournamentStatus Status { get; set; }
        [DataMember(Name = "type")]
        public virtual TournamentType TournamentType { get; set; }

        [DataMember(Name = "stages")]
        public virtual IList<TournamentStageDto> Stages { get; set; }

        [DataMember(Name = "pointForWin")]
        public virtual int PointsForWin { get; set; }
        
        [DataMember(Name = "pointForTie")]
        public virtual int PointsForTie { get; set; }
    }
}