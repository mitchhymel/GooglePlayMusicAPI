using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GooglePlayMusicAPI.Models.GooglePlayMusicModels
{
    [DataContract]
    public class Thumbnail
    {
        [DataMember(Name = "url")]
        public string Url { get; set; }
        [DataMember(Name = "width")]
        public int Width { get; set; }
        [DataMember(Name = "height")]
        public int Height { get; set; }
    }
}
