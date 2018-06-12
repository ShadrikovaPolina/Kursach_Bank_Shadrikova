using System;
using System.Runtime.Serialization;

namespace BankService.BindingModels
{
    [DataContract]
    public class ReportBindingModel
    {
        [DataMember]
        public string Mail { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string FontPath { get; set; }

        [DataMember]
        public DateTime? DateFrom { get; set; }

        [DataMember]
        public DateTime? DateTo { get; set; }
    }
}
