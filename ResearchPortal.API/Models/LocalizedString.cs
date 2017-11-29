using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ResearchPortal.API.Models
{
    [DataContract]
    public class LocalizedString
    {
        public LocalizedString()
        {
            En = "";
            Fr = "";
        }

        [DataMember(Name = "en")]
        public string En { get; set; }
        [DataMember(Name = "fr")]
        public string Fr { get; set; }
    }
}
