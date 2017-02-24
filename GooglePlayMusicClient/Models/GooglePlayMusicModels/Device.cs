using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
        public DeviceType Type { get; set; }
        [DataMember(Name = "smartPhone")]
        public bool SmartPhone { get; set; }

        [DataContract]
        [JsonConverter(typeof(StringEnumConverter))]
        public enum DeviceType
        {
            [DataMember(Name = "ANDROID")]
            Android,

            [DataMember(Name = "IOS")]
            IOS,

            [DataMember(Name = "DESKTOP_APP")]
            [EnumMember(Value = "DESKTOP_APP")]
            DesktopApp
        }
    }
}
