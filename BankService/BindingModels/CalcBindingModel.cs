using System.Runtime.Serialization;

namespace BankService.BindingModels
{
    [DataContract]
    public class CalcBindingModel
    {
        [DataMember]
        public int WorkerId { get; set; }

        [DataMember]
        public int Bonus { get; set; }

        [DataMember]
        public int Fine { get; set; }
    }
}