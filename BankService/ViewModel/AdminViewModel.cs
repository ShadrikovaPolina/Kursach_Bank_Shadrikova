using System.Runtime.Serialization;

namespace BankService.ViewModel
{
    [DataContract]
    public class AdminViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string AdminFIO { get; set; }
    }
}