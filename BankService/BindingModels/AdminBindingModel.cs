using BankService.App;
using System.Runtime.Serialization;

namespace BankService.BindingModels
{
    [DataContract]
    public class AdminBindingModel : AppUser
    {
        [DataMember]
        public string AdminFIO { get; set; }
    }
}