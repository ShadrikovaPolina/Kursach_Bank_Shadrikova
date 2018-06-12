using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BankModel
{
    [DataContract]
    public class Admin
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string AdminFIO { get; set; }

        [DataMember]
        [Required]
        public string UserName { get; set; }

        [DataMember]
        [Required]
        public string PasswordHash { get; set; }

        [DataMember]
        [Required]
        public string SecurityStamp { get; set; }
    }
}
