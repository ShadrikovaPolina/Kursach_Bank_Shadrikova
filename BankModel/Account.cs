using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BankModel
{
    [DataContract]
    public class Account
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public int WorkerId { get; set; }

        [DataMember]
        [Required]
        public DateTime AccountDate { get; set; }

        [DataMember]
        public decimal Salary { get; set; }

        [DataMember]
        public decimal Bonus { get; set; }

        public virtual Worker Worker { get; set; }

        [DataMember]
        public bool Paid { get; set; }

    }
}
