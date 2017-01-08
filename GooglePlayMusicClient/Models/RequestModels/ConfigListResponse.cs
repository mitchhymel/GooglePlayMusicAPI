using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace GooglePlayMusicAPI
{
    [DataContract]
    public class ConfigListResponse
    {
        [DataMember(Name = "kind")]
        public string Kind { get; set; }
        [DataMember(Name = "data")]
        public ConfigList Data { get; set; }
    }

    [DataContract]
    public class ConfigList
    {
        [DataMember(Name = "entries")]
        public List<ConfigListEntry> Entries;
    }
   
}
