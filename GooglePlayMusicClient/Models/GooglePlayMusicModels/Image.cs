using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace GooglePlayMusicAPI.Models.GooglePlayMusicModels
{
    [DataContract]
    public class Image
    {
        [DataMember(Name = "kind")]
        public string Kind { get; set; }
        [DataMember(Name = "url")]
        public string Url { get; set; }
        [DataMember(Name = "aspectRatio")]
        public int AspectRatio { get; set; }
        [DataMember(Name = "autogen")]
        public bool Autogen { get; set; }
    }
}
