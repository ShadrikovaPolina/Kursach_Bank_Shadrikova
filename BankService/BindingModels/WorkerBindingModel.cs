using System.Runtime.Serialization;

namespace BankService.BindingModels
{
    [DataContract]
    public class WorkerBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string WorkerFIO { get; set; }
    }
}