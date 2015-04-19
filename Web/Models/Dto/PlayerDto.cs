using System.Runtime.Serialization;

namespace Web.Models.Dto
{
    [DataContract]
    public class PlayerDto : SubjectDto
    {
        [DataMember(Name = "username")]
        public virtual string UserName { get; set; }
        [DataMember(Name = "isAdmin")]
        public virtual bool IsAdmin { get; set; }

        [DataMember(Name = "firstName")]
        public virtual string FirstName { get; set; }
        [DataMember(Name = "lastname")]
        public virtual string LastName { get; set; }
    }
}