using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BankModel
{
    [DataContract]
    public class Worker
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string WorkerFIO { get; set; }

        [DataMember]
        [Required]
        public DateTime DateCreate { get; set; }

        [DataMember]
        public int Bonus { get; set; }

        [ForeignKey("WorkerId")]
        public virtual List<Account> Accounts { get; set; }
    }
}
