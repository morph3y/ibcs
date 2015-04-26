using System;
using System.Runtime.Serialization;

namespace Web.Models.Dto
{
    [DataContract]
    [KnownType(typeof(TeamDto))]
    [KnownType(typeof(PlayerDto))]
    public class SubjectDto
    {
        [DataMember(Name = "id")]
        public virtual int Id { get; set; }
        [DataMember(Name = "name")]
        public virtual string Name { get; set; }
        [DataMember(Name = "dataCreated")]
        public virtual DateTime DateCreated { get; set; }
        [DataMember(Name = "deleted")]
        public virtual bool Deleted { get; set; }
    }
}