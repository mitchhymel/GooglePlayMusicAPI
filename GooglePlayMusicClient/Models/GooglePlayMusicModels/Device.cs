using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace GooglePlayMusicAPI
{
    [DataContract]
    public class Device
    {
        [DataMember(Name = "kind")]
        public string Kind { get; set; }
        [DataMember(Name = "friendlyName")]
        public string FriendlyName { get; set; }
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "lastAccessedTimeMs")]
        public string LastAccessedTimeMs { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "smartPhone")]
        public bool SmartPhone { get; set; }
    }
}
