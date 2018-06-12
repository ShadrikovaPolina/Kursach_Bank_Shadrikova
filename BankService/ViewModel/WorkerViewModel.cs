using System.Runtime.Serialization;

namespace BankService.ViewModel
{
    [DataContract]
    public class WorkerViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string WorkerFIO { get; set; }

        [DataMember]
        public decimal Bonus { get; set; }
    }
}