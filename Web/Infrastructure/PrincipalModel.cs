using System;

namespace Web.Infrastructure
{
    [Serializable]
    public class PrincipalModel
    {
        public string UserName { get; set; }
        public int Id { get; set; }
        public bool IsAdmin { get; set; }
    }
}