using System.Runtime.Serialization;

namespace BankService.ViewModel
{
    [DataContract]
    public class AccountViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string WorkerFIO { get; set; }

        [DataMember]
        public string AccountDate { get; set; }

        [DataMember]
        public decimal Salary { get; set; }

        [DataMember]
        public decimal Bonus { get; set; }

        [DataMember]
        public decimal Total { get; set; }

        [DataMember]
        public string Paid { get; set; }
    }
}