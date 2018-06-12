using BankModel;
using System.Runtime.Serialization;

namespace BankService.BindingModels
{
    [DataContract]
    public class SalaryBindingModel
    {
        [DataMember]
        public Worker Worker { get; set; }

        [DataMember]
        public decimal Salary { get; set; }
    }
}