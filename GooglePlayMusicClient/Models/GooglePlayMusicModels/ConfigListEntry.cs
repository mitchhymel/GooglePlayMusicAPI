using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace GooglePlayMusicAPI
{
    [DataContract]
    public class ConfigListEntry
    {
        [DataMember(Name = "key")]
        public string Key { get; set; }
        [DataMember(Name = "kind")]
        public string Kind { get; set; }
        [DataMember(Name = "value")]
        public string Value { get; set; }
    }
}
